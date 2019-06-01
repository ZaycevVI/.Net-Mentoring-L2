﻿using System;

namespace PdfPackage.File
{
    public class PathConverter : IPathConverter
    {
        public int Convert(string fileName)
        {
            var start = fileName.IndexOf("_", StringComparison.Ordinal);
            var end = fileName.IndexOf(".", StringComparison.Ordinal);

            return System.Convert.ToInt32(fileName.Substring(start + 1, end - start - 1));
        }
    }

    public interface IPathConverter
    {
        int Convert(string fileName);
    }
}