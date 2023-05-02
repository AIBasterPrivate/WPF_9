using Microsoft.Win32;
using System;
using System.Drawing;
using System.Text;
using System.Windows;
using WPF_9.image.model;
using WPF_9.image.service;
using WPF_9.image.views.models;
using WPF_9.security.models;
using WPF_9.security.service;
using WPF_9.views.viewsmodels;

namespace WPF_9.image.views
{
    /// <summary>
    /// Interaction logic for ImageEncWindow.xaml
    /// </summary>
    public partial class ImageEncWindow : Window
    {
        MenuWindowModel _menu = new MenuWindowModel();
        ImageEncWindowModel _enc = new ImageEncWindowModel();
        public ImageEncWindow(MenuWindowModel menu)
        {
            _menu= menu;
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
        private void FindImageFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg;*.png)|*.jpg;*.png|All Files (*.*)|*.*";

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
            if (UserDataInputTextBox.Text == "")
            {
                MessageBox.Show("Enter the text to start the encryption");
                return;
            }
            if (_enc.InPath == null)
            {
                MessageBox.Show("Select the image file where the text will be encrypted");
                return;
            }

            Bitmap bitmap = (Bitmap)Bitmap.FromFile(_enc.InPath);
            ARGBImageModel model = new ARGBImageModel(bitmap);

            var totalPlaceInBytes = model.Width() * model.Height();
            var oneNumberSizeInBytes = 4;
            var dataSizeInBytes = Encoding.Unicode.GetBytes(UserDataInputTextBox.Text).Length + oneNumberSizeInBytes;
            if (totalPlaceInBytes < dataSizeInBytes)
            {
                MessageBox.Show("There is not enough space in the image file for your text " +
                    "Make your message shorter, or choose another image");
                return;
            }
            _enc.Data = UserDataInputTextBox.Text;
            Embending(model);

            MessageBox.Show("Your text has been successfully inserted into the image");
        }

        private void Embending(ARGBImageModel model)
        {
            byte[] encryptedText = Encrypting();
            var embending = new ARGBImageEmbending();
            encryptedText = embending.AddStopBytes(encryptedText);
            var embedBitmap = embending.Embending(model, encryptedText);
            embedBitmap.Save(_enc.OutPath);
        }

        private byte[] Encrypting()
        {
            var aesModel = Aes256Model.Load("configs\\appconfig.json");
            aesModel.Encoding = _enc.Encoding;
            var aes = new Aes256(aesModel);

            var hash = aes.GenHashCode(_enc.Data);
            var hashInBytes = BitConverter.GetBytes(hash);

            var fullData = Encoding.Unicode.GetBytes(_enc.Data);

            Array.Resize(ref fullData, fullData.Length + 4);
            fullData[fullData.Length - 4] = hashInBytes[0];
            fullData[fullData.Length - 3] = hashInBytes[1];
            fullData[fullData.Length - 2] = hashInBytes[2];
            fullData[fullData.Length - 1] = hashInBytes[3];
            byte[] encryptedText = aes.Encrypt(fullData);
            return encryptedText;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
