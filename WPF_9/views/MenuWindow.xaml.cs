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
using WPF_9.views;
using WPF_9.views.viewsmodels;

namespace WpfApp7.views
{
    /// <summary>
    /// Interaction logic for MenuViewWindow.xaml
    /// </summary>
    public partial class MenuViewWindow : Window
    {
        public MenuViewWindow()
        {
            InitializeComponent();
        }

        private void EncButton_Click(object sender, RoutedEventArgs e)
        {
            var encrypt = new EncWindow(new MenuWindowModel() { MenuWindow = this});
            encrypt.Show();
        }

        private void DecButton_Click(object sender, RoutedEventArgs e)
        {
            var decrypt = new DecWindow(new MenuWindowModel() { MenuWindow = this });
            decrypt.Show();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
