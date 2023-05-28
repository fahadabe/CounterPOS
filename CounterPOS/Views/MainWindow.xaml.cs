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
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using HandyControl.Controls;

namespace CounterPOS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UiWindow
    {
        public MainWindow()
        {
            SetTheme();
            InitializeComponent();
            DataContext = App.GetService<MainViewModel>();
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Application.Current.Shutdown();
        }


        private void SetTheme()
        {
            try
            {
                string savedAccentBrush = Settings.Default.AccentBrush;

                HandyControl.Themes.ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
                Wpf.Ui.Appearance.Theme.Apply(ThemeType.Dark, BackgroundType.Tabbed);
                ApplicationThemeHelper.ApplicationThemeName = DevExpress.Xpf.Core.Theme.VS2019DarkName;

                if (IsValidHexColor(savedAccentBrush))
                {
                    var converter = new BrushConverter();
                    var brush = converter.ConvertFromString(savedAccentBrush) as System.Windows.Media.Brush;

                    System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(savedAccentBrush.ToString());
                    HandyControl.Themes.ThemeManager.Current.AccentColor = brush;
                    Accent.Apply(color);
                }
                else
                {
                    System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Brushes.LightSkyBlue.ToString());
                    HandyControl.Themes.ThemeManager.Current.AccentColor = Brushes.LightSkyBlue;
                    Accent.Apply(color);
                }
            }
            catch (Exception e)
            {
                Growl.ErrorGlobal(e.Message);
            }
        }

        bool IsValidHexColor(string colorCode)
        {
            // Check if the color code starts with a '#' character
            if (colorCode[0] != '#')
            {
                return false;
            }

            // Check if the color code has a valid length
            if (colorCode.Length != 4 && colorCode.Length != 7 && colorCode.Length != 9)
            {
                return false;
            }

            // Check if each character in the color code is valid
            for (int i = 1; i < colorCode.Length; i++)
            {
                char c = colorCode[i];
                bool isValid = (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f');
                if (!isValid)
                {
                    return false;
                }
            }

            // If all checks passed, the color code is valid
            return true;
        }
    }
}
