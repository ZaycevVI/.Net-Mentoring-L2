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
                    var url1 = new Uri(UrlTxtBox1.Text);
                    var url2 = new Uri(UrlTxtBox2.Text);
                    var url3 = new Uri(UrlTxtBox3.Text);
                    var content1 = await DownloadDataAsync(url1);
                    var content2 = await DownloadDataAsync(url2);
                    var content3 = await DownloadDataAsync(url3);

                    var task1 = Task.Run(() => File.WriteAllText($"{Guid.NewGuid()}.html", content1));
                    var task2 = Task.Run(() => File.WriteAllText($"{Guid.NewGuid()}.html", content2));
                    var task3 = Task.Run(() => File.WriteAllText($"{Guid.NewGuid()}.html", content3));

                    await Task.WhenAll(task1, task2, task3);

                    Logs.Items.Add($"Web pages: {url1}|{url2}|{url3} were downloaded successfully.");
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
