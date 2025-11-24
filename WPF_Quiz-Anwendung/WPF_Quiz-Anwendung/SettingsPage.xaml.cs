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

namespace WPF_Quiz_Anwendung
{
    /// <summary>
    /// Interaktionslogik für SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void DarkModeToggle_Checked(object sender, RoutedEventArgs e)
        {
            // Dunkelmodus aktivieren
            this.Background = (Brush)new BrushConverter().ConvertFromString("#754882");
            Überschrift.Foreground = Brushes.White;
            Accessibility.Foreground = Brushes.White;
            Theme.Foreground = Brushes.White;
            DarkMode.Foreground = Brushes.White;
            Standardfragen.Foreground = Brushes.White;
            Box1.Background = (Brush)new BrushConverter().ConvertFromString("#754882");
            Box2.Background = (Brush)new BrushConverter().ConvertFromString("#754882");
            Border1.Background = (Brush)new BrushConverter().ConvertFromString("#5a3763");
            Border2.Background = (Brush)new BrushConverter().ConvertFromString("#5a3763");
            Border3.Background = (Brush)new BrushConverter().ConvertFromString("#5a3763");
            Border4.Background = (Brush)new BrushConverter().ConvertFromString("#5a3763");

        }

        private void DarkModeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            // Dunkelmodus deaktivieren
            this.Background = (Brush)new BrushConverter().ConvertFromString("#F3C8FF");
            Überschrift.Foreground = Brushes.Black;
            Accessibility.Foreground = Brushes.Black;
            Theme.Foreground = Brushes.Black;
            DarkMode.Foreground = Brushes.Black;
            Standardfragen.Foreground = Brushes.Black;
            Box1.Background = (Brush)new BrushConverter().ConvertFromString("#E9C0F0");
            Box2.Background = (Brush)new BrushConverter().ConvertFromString("#E9C0F0");
            Border1.Background = (Brush)new BrushConverter().ConvertFromString("#E9C0F0");
            Border2.Background = (Brush)new BrushConverter().ConvertFromString("#E9C0F0");
            Border3.Background = (Brush)new BrushConverter().ConvertFromString("#E9C0F0");
            Border4.Background = (Brush)new BrushConverter().ConvertFromString("#E9C0F0");
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Platzhalter: Reagiere auf Auswahl (z.B. Theme-Wechsel)
            // var listBox = (ListBox)sender;
            // var selected = listBox.SelectedItem;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
