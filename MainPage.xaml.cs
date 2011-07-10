using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using System.IO;

namespace sl_Storage
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("MyMessage"))
            {
                textMessage.Text =
                    IsolatedStorageSettings.ApplicationSettings["MyMessage"] as string;
            }
            if (IsolatedStorageSettings.ApplicationSettings.Contains("MyFlag"))
            {
                checkFlag.IsChecked = IsolatedStorageSettings.ApplicationSettings["MyFlag"] as bool?;
            }
            LoadIcon();

        }
        private void LoadIcon()
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!store.FileExists("icon.img")) {
                    return;
                }
                using (IsolatedStorageFileStream stream = store.OpenFile("icon.img" , FileMode.Open)) {
                    try {
                        BitmapImage image = new BitmapImage();
                        image.SetSource(stream);
                        imageIcon.Source = image;

                    }
                    catch {
                        imageIcon.Source = null;
                    }
                }
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e) {
            IsolatedStorageSettings.ApplicationSettings["MyMessage"] = textMessage.Text;
            IsolatedStorageSettings.ApplicationSettings["MyFlag"] = checkFlag.IsChecked;
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings.Remove("MyMessage");
            IsolatedStorageSettings.ApplicationSettings.Remove("MyFlag");
        }
        private void btnChoose_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Image files(*.jpg,*.png,)|*.jpg;*.png";
            if(dlg.ShowDialog() == true) {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication()) {
                    if (store.FileExists("icon.img")) {
                        store.DeleteFile("icon.img");
                    }

                    FileStream streamSource = dlg.File.OpenRead();

                    using (IsolatedStorageFileStream streamDest = store.CreateFile("icon.img")) {
                        byte[] buffer = new byte[streamSource.Length];
                        streamSource.Read(buffer, 0, (int)streamSource.Length);
                        streamDest.Write(buffer , 0 , (int)streamSource.Length);
                        streamDest.Close();
                        streamSource.Close();
                    }
                }
                LoadIcon();
            }
        }
    }
}
