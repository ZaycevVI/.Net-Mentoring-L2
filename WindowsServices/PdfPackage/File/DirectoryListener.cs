﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using PdfPackage.Pdf;
using PdfPackage.Validator;

namespace PdfPackage.File
{
    public class DirectoryListener : IDirectoryListener
    {
        private readonly string _outDir;
        private readonly string _inDir;

        private readonly IValidator _imgNameValidator = new ImageNameValidator();
        private readonly IPdfGenerator _pdfGenerator = new PdfSharpGenerator();
        private readonly FileSystemWatcher _watcher;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private const long Timeout = 100000; // 100 seconds
        private long _currentDelay;
        private string _prevImg;
        private string _currentImg;

        private readonly List<string> _imgs = new List<string>();

        public DirectoryListener(string inDir, string outDir)
        {
            _inDir = inDir;
            _outDir = outDir;
            _watcher = new FileSystemWatcher(inDir);
            _watcher.Created += WatcherOnCreated;
            _stopwatch.Start();
        }

        public void Start()
        {
            _watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
        }

        private void WatcherOnCreated(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            _currentImg = fileSystemEventArgs.Name;

            if (_imgNameValidator.Validate(_currentImg))
            {
                _currentDelay = _stopwatch.ElapsedMilliseconds / 1000;

                if (IsEndOfDoc(_prevImg, _currentImg))
                {
                    CreatePdf(_imgs.ToArray());
                    _imgs.Clear();
                }

                _imgs.Add(_currentImg);
                _prevImg = _currentImg;
                _stopwatch.Restart();
            }
        }

        private bool IsEndOfDoc(string prevImg, string nextImg)
        {
            if (prevImg == null)
                return false;

            if (_currentDelay > Timeout)
                return true;

            var startNum = PathConverter.Convert(prevImg);
            var nextNum = PathConverter.Convert(nextImg);

            return nextNum - startNum != 1;
        }

        private void CreatePdf(params string[] imgPaths)
        {
            _pdfGenerator.GenerateAsync($"{_outDir}\\{Guid.NewGuid()}.pdf", imgPaths.Select(img => $"{_inDir}\\{img}").ToArray());
        }
    }
}