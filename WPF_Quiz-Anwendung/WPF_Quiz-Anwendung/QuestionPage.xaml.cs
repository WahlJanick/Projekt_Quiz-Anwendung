using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPF_Quiz_Anwendung.Classes;
using System.Windows.Input;

namespace WPF_Quiz_Anwendung
{
    public partial class QuestionPage : Page
    {
        private Quiz currentQuiz;
        private int currentIndex = 0;
        private bool answerClicked;
        private int correctAnswers = 0;
        private bool multiHadError;
            
        public QuestionPage(Quiz quiz)
        {
            InitializeComponent();
            this.PreviewKeyDown += QuestionPage_PreviewKeyDown;
            this.Loaded += (s, e) => { this.Focusable = true; this.Focus(); };
            try
            {
                currentQuiz = quiz;
                ShowQuestion(currentIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden des Quiz: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ;
        }
        private void ShowQuestion(int index)
        {
            HelpTitleTB.Text = "[S] Frage überspringen";
            answerClicked = false;
            multiHadError = false;
            if (currentQuiz == null || currentQuiz.Questions.Count == 0)
            {
                QuestionTitleTB.Text = "Kein Quiz geladen!";
                AnswerGrid.Children.Clear();
                return;
            }

            if (index < 0 || index >= currentQuiz.Questions.Count)
                return;
            var currentQuestion = currentQuiz.Questions[index];
            QuestionTitleTB.Text = $"Frage {index + 1}/{currentQuiz.Questions.Count}: {currentQuestion.Text}";
            AnswerGrid.Children.Clear();

            for (int i = 0; i < currentQuestion.Answers.Count; i++)
            {
                var border = new Border
                {
                    Margin = new Thickness(15),
                    Style = (Style)FindResource("RoundedContainerStyle")
                };
                var button = new Button
                {
                    Content = currentQuestion.Answers[i].Text,
                    Style = (Style)FindResource("RoundedButtonStyle"),
                    Tag = i 
                };
                button.Click += AnswerClick;
                border.Child = button;
                AnswerGrid.Children.Add(border);
            }
        }

        private async void AnswerClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;

            var currentQuestion = currentQuiz.Questions[currentIndex];
            int answerIndex = (int)btn.Tag;
            bool isCorrect = currentQuestion.CorrectAnswerIndexes.Contains(answerIndex);

            if (currentQuestion.Type == QuestionType.SingleRightAnswer)
            {
                if (answerClicked) return;
                answerClicked = true;

                btn.Background = isCorrect ? Brushes.LightGreen : Brushes.LightCoral;
                QuestionTitleTB.Text = isCorrect ? "Richtig!" : "Falsch!";
                if (isCorrect)
                {
                    QuestionTitleTB.Text = "Richtig!";
                    correctAnswers++;
                }
                else QuestionTitleTB.Text = "Falsch!";
                foreach (Border border in AnswerGrid.Children)
                {
                    if (border.Child is Button b)
                        b.IsEnabled = false;
                }

                await Task.Delay(2000);
                NextQuestion();
            }
            else if (currentQuestion.Type == QuestionType.MultipleRightAnswers)
            { 
                btn.IsEnabled = false;
                if (isCorrect) 
                {
                    btn.Background = Brushes.LightGreen;
                }
                else
                {
                    multiHadError = true;
                    btn.Background = Brushes.IndianRed;
                    QuestionTitleTB.Text = "Falsch!";

                    foreach (Border border in AnswerGrid.Children)
                    {
                        if (border.Child is Button b)
                            b.IsEnabled = false;
                    }

                    await Task.Delay(2000);
                    NextQuestion();
                    return;
                }

                bool allCorrectClicked = true;
                foreach (Border border in AnswerGrid.Children)
                {
                    if (border.Child is Button b)
                    {
                        int idx = (int)b.Tag;
                        if (currentQuestion.CorrectAnswerIndexes.Contains(idx) && b.IsEnabled)
                        {
                            allCorrectClicked = false;
                            break;
                        }
                    }
                }

                if (allCorrectClicked && !multiHadError)
                {
                    QuestionTitleTB.Text = "Richtig!";
                    correctAnswers++;
                    foreach (Border border in AnswerGrid.Children)
                    {
                        if (border.Child is Button b)
                            b.IsEnabled = false;
                    }
                    await Task.Delay(2000);
                    NextQuestion();
                }
            }
        }

        private void NextQuestion()
        {
            currentIndex++;
            if (currentIndex < currentQuiz.Questions.Count)
            {
                ShowQuestion(currentIndex);
            }
            else
            {
                double scorepercent = (double)correctAnswers / (double)currentQuiz.Questions.Count;
                QuestionTitleTB.Text = $"Quiz beendet mit {(scorepercent * 100):F2}% richtigen Antworten";
                HelpTitleTB.Text = "[ESC] um zum Hauptmenü zurückzukehren...";
                AnswerGrid.Children.Clear();
            }
        }
        private void SkipQuestion()
        {
            if (currentIndex < currentQuiz.Questions.Count)
            {
                currentIndex++;
                ShowQuestion(currentIndex);
            }
        }

        private void QuestionPage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S)
            {
                e.Handled = true;
                SkipQuestion();
            }
        }
    }
}
