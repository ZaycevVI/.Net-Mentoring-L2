using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Library.Helper;
using Library.Validator;

namespace Library.FolderListener
{
    public class DirectoryListener : IDirectoryListener
    {
        public DirectoryListenerSettings Settings { get; }

        private readonly IValidator _imgNameValidator = new ImageNameValidator();
        private readonly FileSystemWatcher _watcher;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private long _currentDelay;
        private string _prevImg;
        private string _currentImg;

        private readonly List<string> _imgs = new List<string>();

        public DirectoryListener(DirectoryListenerSettings settings)
        {
            Settings = settings;

            TryCreateDirectory(Settings.InDir);

            _watcher = new FileSystemWatcher(Settings.InDir);
            _watcher.Created += WatcherOnCreated;
            _stopwatch.Start();
        }

        public void Start()
        {
            Settings.CurrentStatus = "Client was just started.";
            _currentImg = _prevImg = null;
            _imgs.Clear();
            var imgs = Directory.GetFiles(Settings.InDir).Select(Path.GetFileName);

            foreach (var img in imgs)
            {
                ProcNewImg(img);
            }
           
            _watcher.EnableRaisingEvents = true;
            _stopwatch.Restart();
            Settings.CurrentStatus = "Waiting for next img.";
        }

        public void Stop()
        {
            Settings.CurrentStatus = "Client was stopped.";
            _watcher.EnableRaisingEvents = false;
        }

        public EventHandler<PdfFileEventArg> OnFileReady { get; set; }

        private void WatcherOnCreated(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            ProcNewImg(fileSystemEventArgs.Name);
        }

        private void TryCreateDirectory(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        private void ProcNewImg(string img)
        {
            Settings.CurrentStatus = $"Start processing img: {img}";
            _currentImg = img;

            if (_imgNameValidator.Validate(_currentImg))
            {
                _currentDelay = _stopwatch.ElapsedMilliseconds / 1000;

                if (IsEndOfDoc(_prevImg, _currentImg))
                {
                    NotifyOnFileReady();
                }

                _imgs.Add(_currentImg);
                _prevImg = _currentImg;
                _stopwatch.Restart();
            }
            Settings.CurrentStatus = "Waiting for next img.";
        }

        private void  NotifyOnFileReady()
        {
            OnFileReady?.Invoke(this, new PdfFileEventArg(_imgs.ToList()));
            _imgs.Clear();
        }

        private bool IsEndOfDoc(string prevImg, string nextImg)
        {
            if (prevImg == null)
                return false;

            if (_currentDelay > Settings.Timeout)
                return true;

            var startNum = PathConverter.Convert(prevImg);
            var nextNum = PathConverter.Convert(nextImg);

            return nextNum - startNum != 1;
        }

        ~DirectoryListener()
        {
            _watcher.Created -= WatcherOnCreated;
            _watcher.Dispose();
        }
    }
}