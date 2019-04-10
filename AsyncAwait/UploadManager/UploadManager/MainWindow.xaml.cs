using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace UploadManager
{
    // Напишите простейший менеджер закачек. Пользователь задает адрес страницы, 
    // которую необходимо загрузить. В процессе загрузки пользователь может ее отменить. 
    // Пользователь может задавать несколько источников для закачки. 
    // Скачивание страниц не должно блокировать интерфейс приложения.
    public partial class MainWindow : Window
    {
        private delegate void CancelHandler();
        private event CancelHandler Cancel;
        private int i = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Upload_Click(object sender, RoutedEventArgs e)
        {
            //var saveFileDialog = new SaveFileDialog { DefaultExt = ".html" };

            //if (saveFileDialog.ShowDialog() == true)
            //{
                try
                {
                    var url = new Uri(UrlTxtBox.Text);
                    var content = await DownloadDataAsync(url);
                    File.WriteAllText($"{i}.html", content);
                    Interlocked.Increment(ref i);
                    Logs.Items.Add($"Web page: {UrlTxtBox.Text} was downloaded successfully.");
                }
                catch (Exception exception)
                {
                    Logs.Items.Add($"{exception.Message}");
                }
            //}
        }

        private async Task<string> DownloadDataAsync(Uri uri)
        {
            var content = string.Empty;
            var url = uri.ToString();


            if (string.IsNullOrWhiteSpace(uri.ToString()))
                throw new ArgumentNullException(nameof(uri), @"Uri can not be null or empty.");

            if (!Uri.IsWellFormedUriString(uri.ToString(), UriKind.Absolute))
                throw new ArgumentException(nameof(uri), @"Uri si not well-formed.");

            using (var webClient = new WebClient())
            {
                Cancel += webClient.CancelAsync;
                var dataArr = await webClient.DownloadDataTaskAsync(uri);
                content = Encoding.UTF8.GetString(dataArr);
            }

            return content;
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            OnCancelEvent();
        }

        protected virtual void OnCancelEvent()
        {
            Cancel?.Invoke();
        }
    }
}
