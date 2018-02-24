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

namespace Annoyer___Windows_Edition
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            if (Convert.ToBoolean(Properties.Settings.Default["autoArm"]))
            {
                pref1.IsChecked = true;
            }
            pref2.SelectedIndex = Convert.ToInt32(Properties.Settings.Default["defaultVoice"]);
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            
            Properties.Settings.Default["autoArm"] = pref1.IsChecked;
            Properties.Settings.Default["defaultVoice"] = pref2.SelectedIndex;
            Properties.Settings.Default["defaultText"] = pref3.Text;
            
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
    }
}
