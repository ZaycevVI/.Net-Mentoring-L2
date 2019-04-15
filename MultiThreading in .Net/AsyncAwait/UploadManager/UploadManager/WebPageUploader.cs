using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UploadManager
{
    public class WebPageUploader
    {
        private readonly ConcurrentDictionary<Guid, (string url, Action cancel)> _cancelActions = 
            new ConcurrentDictionary<Guid, (string url, Action cancel)>();

        public EventHandler<string> OnSuccessfullUpload;
        public EventHandler<string> OnCancelUpload;

        public async Task UploadAsync(Uri uri)
        {
            string content;

            if (string.IsNullOrWhiteSpace(uri.ToString()))
                throw new ArgumentNullException(nameof(uri), @"Uri can not be null or empty.");

            if (!Uri.IsWellFormedUriString(uri.ToString(), UriKind.Absolute))
                throw new ArgumentException(nameof(uri), @"Uri si not well-formed.");

            using (var webClient = new WebClient())
            {
                var guid = Guid.NewGuid();
                _cancelActions[guid] = (uri.ToString(), () => webClient.CancelAsync());
                var dataArr = await webClient.DownloadDataTaskAsync(uri);
                _cancelActions.TryRemove(guid, out var item);
                content = Encoding.UTF8.GetString(dataArr);
                File.WriteAllText($"{Guid.NewGuid()}.html", content);
            }

            OnSuccessfullUpload?.Invoke(this, uri.ToString());
        }

        public void Cancel()
        {
            foreach (var (url, cancel) in _cancelActions.Values)
            {
                cancel();
                OnCancelUpload(this, url);
            }

            _cancelActions.Clear();
        }
    }
}