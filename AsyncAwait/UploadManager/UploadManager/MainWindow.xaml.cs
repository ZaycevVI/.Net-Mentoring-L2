using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace UploadManager
{
    // Напишите простейший менеджер закачек. Пользователь задает адрес страницы, 
    // которую необходимо загрузить. В процессе загрузки пользователь может ее отменить. 
    // Пользователь может задавать несколько источников для закачки. 
    // Скачивание страниц не должно блокировать интерфейс приложения.

    // Save files to \bin\debug folder with random name as *.html
    public partial class MainWindow : Window
    {
        private readonly WebPageUploader _webPageUploader;

        public MainWindow()
        {
            InitializeComponent();
            _webPageUploader = new WebPageUploader();
            _webPageUploader.OnCancelUpload += OnCancelUploadHandler;
            _webPageUploader.OnSuccessfullUpload += OnSuccessfullUploadHandler;
        }

        private async void Button_Upload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var url1 = new Uri(UrlTxtBox1.Text);
                var url2 = new Uri(UrlTxtBox2.Text);
                var url3 = new Uri(UrlTxtBox3.Text);

                await Task.WhenAll(
                    _webPageUploader.UploadAsync(url1),
                    _webPageUploader.UploadAsync(url2), 
                    _webPageUploader.UploadAsync(url3));
            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.RequestCanceled)
            {
                // ignore this exception
            }
            catch (Exception exception)
            {
                Logs.Items.Add($"Unexpected exception occurs: {exception.Message}");
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            _webPageUploader.Cancel();
        }

        private void OnCancelUploadHandler(object sender, string url)
        {
            Logs.Items.Add($"Web page upload was canceled: {url}.");
        }

        private void OnSuccessfullUploadHandler(object sender, string url)
        {
            Logs.Items.Add($"Web page: {url} was downloaded successfully.");
        }
    }
}
