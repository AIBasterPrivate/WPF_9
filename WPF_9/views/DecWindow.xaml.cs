using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_9.security.models;
using WPF_9.security.service;
using WPF_9.video.service;
using WPF_9.views.viewsmodels;

namespace WPF_9.views
{
    /// <summary>
    /// Interaction logic for DecWindow.xaml
    /// </summary>
    public partial class DecWindow : Window
    {
        MenuWindowModel _menu = new MenuWindowModel();
        DecWindowModel _dec = new DecWindowModel();
        public DecWindow(MenuWindowModel menu)
        {
            _menu= menu;
            _dec.Encoding = Encoding.Unicode;
            this.Closing += Grid_Closing;
            this.Loaded += Grid_Loaded;
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = _menu.MenuWindow.Left;
            this.Top = _menu.MenuWindow.Top;
            _menu.MenuWindow.Hide();
        }

        private void Grid_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _menu.MenuWindow.Left = this.Left;
            _menu.MenuWindow.Top = this.Top;
            _menu.MenuWindow.Show();
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Video Files (*.avi;*.mp4)|*.avi;*.mp4|All Files (*.*)|*.*";

            if ((bool)openFileDialog.ShowDialog())
            {
                _dec.InPath = openFileDialog.FileName;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if(_dec.InPath == null)
            {
                MessageBox.Show("Choose a video to decode");
                return;
            }

            var encDataBytes = FFmpegVideo_v2.UnEmbending(_dec.InPath);
            var aesModel = Aes256Model.Load("configs\\appconfig.json");
            aesModel.Encoding = _dec.Encoding;
            var aes = new Aes256(aesModel);
            var dataBytes = aes.Decrypt(encDataBytes);
            var oneNumberSizeInBytes = new byte[4];
            Array.Copy(dataBytes, dataBytes.Length - 4, oneNumberSizeInBytes, 0, 4);
            var hashFromFile = BitConverter.ToInt32(oneNumberSizeInBytes, 0);

            Array.Resize(ref dataBytes, dataBytes.Length - 4);

            var dataHash =  aes.GenHashCode(dataBytes);
            if(hashFromFile != dataHash)
            {
                MessageBox.Show("The file is corrupted. Hashes do not match!");
                return;
            }

            _dec.Data = _dec.Encoding.GetString(dataBytes);

            DecryptedTextBox.Text = _dec.Data;
        }
    }

}
