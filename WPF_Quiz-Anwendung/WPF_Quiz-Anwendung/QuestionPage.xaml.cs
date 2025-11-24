using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPF_Quiz_Anwendung.Classes;

namespace WPF_Quiz_Anwendung
{
    public partial class QuestionPage : Page
    {
        private Quiz currentQuiz;
        private int currentIndex = 0;
        private bool answerClicked = false;
        private int correctAnswers = 0;

        public QuestionPage(Quiz quiz)
        {
            InitializeComponent();
            currentQuiz = quiz ?? throw new ArgumentNullException(nameof(quiz));
            ShowQuestion(currentIndex);
        }

        private void ShowQuestion(int index)
        {
            if (currentQuiz == null || currentQuiz.Questions.Count == 0)
            {
                QuestionTitleTB.Text = "Kein Quiz geladen!";
                AnswerGrid.Children.Clear();
                return;
            }

            if (index < 0 || index >= currentQuiz.Questions.Count)
                return;

            answerClicked = false;
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
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0),
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
                else{
                    btn.Background = Brushes.LightCoral;
                    QuestionTitleTB.Text = "Falsch!";
                    await Task.Delay(2000);
                    NextQuestion();
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

                if (allCorrectClicked)
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
                QuestionTitleTB.Text = $"Quiz beendet mit {correctAnswers} richtigen Antworten";
                AnswerGrid.Children.Clear();
            }
        }
    }
}
