using System;
using System.Windows;
using System.Windows.Media;

namespace WPF_Quiz_Anwendung
{
    public enum ThemeMode { Light, Dark }

    public partial class App : Application
    {
        public ThemeMode CurrentTheme { get; private set; } = ThemeMode.Light;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (Properties.Contains("Theme")
                && Enum.TryParse(Properties["Theme"] as string, out ThemeMode saved))
            {
                ApplyTheme(saved);
            }
            else
            {
                ApplyTheme(CurrentTheme);
            }
        }

        public event EventHandler<ThemeMode> ThemeChanged;

        public void ApplyTheme(ThemeMode mode)
        {
            void Set(string key, string colorHex)
            {
                var color = (Color)ColorConverter.ConvertFromString(colorHex);
                Resources[key] = new SolidColorBrush(color);
            }

            if (mode == ThemeMode.Dark)
            {
                Set("AppBackgroundBrush", "#3E2546");   // außen dunkel
                Set("PanelBackgroundBrush", "#6E4581"); // Karten: heller als außen
                Set("TextBrush", "#FFFFFF");
                Set("AccentBrush", "#8A4B97");
                Set("AccentBrushLight", "#A66BC4");
                Set("BorderBrush", "#5A3763");
            }
            else
            {
                Set("AppBackgroundBrush", "#F3C8FF");
                Set("PanelBackgroundBrush", "#E9C0F0"); // Karten etwas dunkler als außen
                Set("TextBrush", "#000000");
                Set("AccentBrush", "#C772DF");
                Set("AccentBrushLight", "#D88BEF");
                Set("BorderBrush", "#FFF3D7FF");
            }

            CurrentTheme = mode;
            Properties["Theme"] = mode.ToString();
            ThemeChanged?.Invoke(this, CurrentTheme);
        }
    }
}
