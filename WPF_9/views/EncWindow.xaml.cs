using System;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using WPF_9.video.service;
using WPF_9.security.models;
using WPF_9.security.service;
using WPF_9.views.viewsmodels;

namespace WPF_9.views
{
    /// <summary>
    /// Interaction logic for EncWindow.xaml
    /// </summary>
    public partial class EncWindow : Window
    {
        private MenuWindowModel _menu = new MenuWindowModel();
        public EncWindowModel _enc = new EncWindowModel();
        public EncWindow(MenuWindowModel menu)
        {
            _menu = menu;
            _enc.Encoding = Encoding.Unicode;
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

        private void FindVideoFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Video Files (*.avi;*.mp4)|*.avi;*.mp4|All Files (*.*)|*.*";

            if ((bool)openFileDialog.ShowDialog())
            {
                _enc.InPath = openFileDialog.FileName;
                var fullOutPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName) +
                    $"\\out - {System.IO.Path.GetFileName(openFileDialog.FileName)}";
                _enc.OutPath = fullOutPath;
            }
        }
        
        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if(UserDataInputTextBox.Text == "")
            {
                MessageBox.Show("Enter the text to start the encryption");
                return;
            }
            if(_enc.InPath == null)
            {
                MessageBox.Show("Select the video file where the text will be encrypted");
                return;
            }

            var videoParams = FFmpegVideo_v2.GetVideoParams(_enc.InPath);
            var totalPlaceInBytes = videoParams.TotalVideoFrames * videoParams.Width * videoParams.Height;
            var oneNumberSizeInBytes = 4;
            var dataSizeInBytes = Encoding.Unicode.GetBytes(UserDataInputTextBox.Text).Length + oneNumberSizeInBytes;
            if (totalPlaceInBytes < dataSizeInBytes)
            {
                MessageBox.Show("There is not enough space in the video file for your text " +
                    "Make your message shorter, or choose another video");
                return;
            }

            _enc.Data = UserDataInputTextBox.Text;

            var aesModel = Aes256Model.Load("configs\\appconfig.json");
            aesModel.Encoding = _enc.Encoding;
            var aes = new Aes256(aesModel);

            var hash = aes.GenHashCode(_enc.Data);
            var hashInBytes = BitConverter.GetBytes(hash);

            var fullData = Encoding.Unicode.GetBytes(_enc.Data);
            Array.Resize( ref fullData, fullData.Length + 4);
            fullData[fullData.Length - 4] = hashInBytes[0];
            fullData[fullData.Length - 3] = hashInBytes[1];
            fullData[fullData.Length - 2] = hashInBytes[2];
            fullData[fullData.Length - 1] = hashInBytes[3];
            byte[] encryptedText = aes.Encrypt(fullData);
            FFmpegVideo_v2.Embending(_enc.InPath, _enc.OutPath, encryptedText);

            MessageBox.Show("Your text has been successfully inserted into the video");
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
