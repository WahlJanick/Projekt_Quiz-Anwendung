using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Quiz_Anwendung.Classes;

namespace WPF_Quiz_Anwendung
{
    /// <summary>
    /// Interaktionslogik für QuestionPage.xaml
    /// </summary>
    public partial class QuestionPage : Page
    {
        public QuestionPage(Quiz currentQuiz)
        {
            InitializeComponent();
            for(int i = 0; i < currentQuiz.Questions.Count; i++)
            {
                LoadQuestion(currentQuiz.Questions[i]);
            }
            
        }
        public void LoadQuestion(Question currentQuestion)
        {
            currentQuestion.Answers.ForEach(answer => { AnswerGrid.Children.Add(new Button() { Content = answer.Title() }) });
        }
    }
}
