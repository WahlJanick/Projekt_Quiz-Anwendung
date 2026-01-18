using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WPF_Quiz_Anwendung.Classes;

namespace WPF_Quiz_Anwendung
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void LoadQuiz_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        public void Load()
        {
            Load(null);
        }

        public void Load(string path)
        {
            try
            {
                Quiz loadedQuiz;

                if (!string.IsNullOrWhiteSpace(path))
                {
                    loadedQuiz = QuizFileHandler.LoadQuizFromFile(path);
                }
                else
                {
                    loadedQuiz = QuizFileHandler.LoadQuizFromFile();
                }

                if (loadedQuiz != null)
                {
                    if (!string.IsNullOrWhiteSpace(Config.UserName))
                    {
                        NavigationService.Navigate(new QuestionPage(loadedQuiz));
                    }
                    else
                    {
                        NavigationService.Navigate(new NameInputPage(new QuestionPage(loadedQuiz)));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden des Quiz: " + ex.Message);
            }
        }

        private void DefaultQuiz(object sender, RoutedEventArgs e)
        {
            Load(Config.DefaultQuestionPath);
        }

        public void CreateQuiz(object sender, RoutedEventArgs e)
        {
        }

        private void SettingsPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SettingsPage());
        }
    }
}