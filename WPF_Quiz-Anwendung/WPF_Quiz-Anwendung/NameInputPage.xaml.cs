using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaktionslogik für NameInputPage.xaml
    /// </summary>
    public partial class NameInputPage : Page
    {
        Page redir = null;
        public NameInputPage(Page redirectTo = null)
        {
            InitializeComponent();
            redir = redirectTo;
        }

        private void ConfirmUsername_Click(object sender, RoutedEventArgs e)
        {
            Config.UserName = UsernameTextBox.Text;
            if (redir != null)
            {
                NavigationService.Navigate(redir);
                return;
            }
            NavigationService.GoBack();
        }
    }
}
