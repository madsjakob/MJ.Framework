using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace MJS.Framework.Base.Types
{
    public class ZipFile
    {
        public ZipFile(Stream baseStream)
        {
            _baseStream = baseStream;
            _zipStream = new ZipOutputStream(baseStream);
        }

        private Stream _baseStream;
        private ZipOutputStream _zipStream;

        public static void Zip(string path, string zipFile)
        {
            using (FileStream fs = new FileStream(zipFile, FileMode.Create))
            {
                ZipFile zf = new ZipFile(fs);
                if (Directory.Exists(path))
                {
                    zf.AddDirectory(path);
                }
                else
                {
                    zf.AddZipEntry(Path.GetFileName(path), path);
                }
                zf.Close();
            }
        }

        public void AddDirectory(string path, string subpath = "")
        {
            string[] fileList = Directory.GetFiles(path);
            foreach (string file in fileList)
            {
                AddZipEntry(Path.Combine(subpath, Path.GetFileName(file)), file);
            }
            string[] directoryList = Directory.GetDirectories(path);
            foreach (string directory in directoryList)
            {
                AddDirectory(directory, Path.GetFileName(directory));
            }
        }

        public void AddZipEntry(string entryName, string filename)
        {
            FileInfo fi = new FileInfo(filename);
            byte[] data = File.ReadAllBytes(filename);
            ZipEntry entry = new ZipEntry(ZipEntry.CleanName(entryName));
            _zipStream.PutNextEntry(entry);
            _zipStream.Write(data, 0, data.Length);
            _zipStream.CloseEntry();
        }

        public void Close()
        {
            _zipStream.Finish();
            _zipStream.Close();
        }
    }
}
