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
using System.Diagnostics;
using System.Configuration;
using System.ComponentModel;

namespace Mwall
{
    /// <summary>
    /// Interaction logic for OptionForm.xaml
    /// </summary>
    public partial class OptionForm : Window
    {
        public OptionForm()
        {
            InitializeComponent();
            ((Grid)FindName("MainGrid")).DataContext = MConfig.GetInstance();

        }

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            if(comboBoxFontSize.SelectedIndex < comboBoxFontSize.Items.Count - 1)
                comboBoxFontSize.SelectedIndex++;
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            if( comboBoxFontSize.SelectedIndex > 0 )
                comboBoxFontSize.SelectedIndex--;
        }
        private void comboBoxFontSize_Loaded(object sender, RoutedEventArgs e)
        {
            List<float> data = new List<float> { 16, 20, 22, 28, 32, 36};

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data;

            //comboBox.SelectedIndex = 2;
        }

        private void comboBoxFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //// ... Get the ComboBox.
            //var comboBox = sender as ComboBox;

            //// ... Set SelectedItem as Window Title.
            ////string value = comboBox.SelectedItem as string;
            //string value = comboBox.SelectedItem.ToString();
            //this.Title = "Selected: " + value;
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            MConfig.GetInstance().SaveConfig();
            MScreen.ReGen();
            DialogResult = true;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            MConfig.GetInstance().LoadConfig();
            DialogResult = false;
        }

    }


    public enum MWStyle
    {
        Comet, Straight
    };


    /// <summary>
    /// Enum to boolean converter.
    /// From http://stackoverflow.com/questions/397556/how-to-bind-radiobuttons-to-an-enum
    /// </summary>
    public class EBConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }


    public class MConfig : INotifyPropertyChanged
    {
        private static MConfig _instance;
        public static MConfig GetInstance()
        {

            if (null == _instance)
                _instance = new MConfig();
            return _instance;
        }


        public const MWStyle DEFAULT_STYLE = MWStyle.Straight;
        public const float DEFAULT_FONTSIZE = 20;
        public MWStyle style;
        public float fsize;
        public MWStyle Style
        {
            get { return style; }
            //set { style = value; FSize = FSize + 1;  }
            set { style = value; NotifyPropertyChanged("Style"); }
        }

        public float FSize
        {
            get { return fsize; }
            set { fsize = value; NotifyPropertyChanged("FSize"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private MConfig()
        {
            if (LoadConfig()) return;
            else
            {
                style = DEFAULT_STYLE;
                fsize = DEFAULT_FONTSIZE;
            }
        }

        public bool LoadConfig()
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection appsettings = config.AppSettings.Settings;

                KeyValueConfigurationElement kv = appsettings["Style"];

                // if not defined "Style", set it to default
                if (null == kv || !Enum.TryParse(kv.Value, true, out style))
                    style = DEFAULT_STYLE;
               
                // if not defined "FontSize", set it to default
                kv = appsettings["FontSize"];
                if (null == kv || !float.TryParse(kv.Value, out fsize))
                    fsize = DEFAULT_FONTSIZE;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return false;
            }
            return true;
        }
        public bool SaveConfig()
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection appsettings = config.AppSettings.Settings;

                if (appsettings.AllKeys.Contains("Style")) appsettings["Style"].Value = style.ToString();
                else appsettings.Add("Style", style.ToString());

                if (appsettings.AllKeys.Contains("FontSize")) appsettings["FontSize"].Value = fsize.ToString();
                else appsettings.Add("FontSize", fsize.ToString());
                config.Save(ConfigurationSaveMode.Full);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return false;
            }
            return true;
        }

        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
