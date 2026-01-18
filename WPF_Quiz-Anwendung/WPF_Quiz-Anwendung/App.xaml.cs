using System;
using System.Windows;
using System.Windows.Media;
using WPF_Quiz_Anwendung.Classes;

namespace WPF_Quiz_Anwendung
{
    public partial class App : Application
    {
        public Theme CurrentTheme { get; private set; } = Theme.Light;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            QuizFileHandler.LoadConfigFromFile();

            if (!Enum.IsDefined(typeof(Theme), Config.Theme))
                Config.Theme = Theme.Light;

            ApplyTheme(Config.Theme);
        }
        protected override void OnExit(ExitEventArgs e)
        {
            QuizFileHandler.SaveConfigToFile();
            base.OnExit(e);
        }

        public event EventHandler<Theme> ThemeChanged;

        public void ApplyTheme(Theme mode)
        {
            void Set(string key, string colorHex)
            {
                var color = (Color)ColorConverter.ConvertFromString(colorHex);
                Resources[key] = new SolidColorBrush(color);
            }

            if (mode == Theme.Dark)
            {
                Set("AppBackgroundBrush", "#3E2546");
                Set("PanelBackgroundBrush", "#6E4581");
                Set("TextBrush", "#FFFFFF");
                Set("AccentBrush", "#8A4B97");
                Set("AccentBrushLight", "#A66BC4");
                Set("BorderBrush", "#5A3763");
            }
            else
            {
                Set("AppBackgroundBrush", "#F3C8FF");
                Set("PanelBackgroundBrush", "#E9C0F0"); 
                Set("TextBrush", "#000000");
                Set("AccentBrush", "#C772DF");
                Set("AccentBrushLight", "#D88BEF");
                Set("BorderBrush", "#FFF3D7FF");
            }

            CurrentTheme = mode;
            Config.Theme = mode;

            ThemeChanged?.Invoke(this, mode);
        }
    }
}
