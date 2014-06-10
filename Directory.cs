using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace FreespaceAnalyzer
{
    class Directory
    {
        public ulong totalSize;
        public ulong totalFileSize;
        public double percent;
        public double gbTotalSize;
        public double gbTotalFileSize;
        public string path;
        public Directory[] children;
        public bool loaded = true;

        public Directory(DirectoryInfo info)
        {
            path = info.FullName;
            Form1.Instance.SetStatus(path);

            FileInfo[] files;
            
            try
            {
                files = info.GetFiles();
            }
            catch (Exception e)
            {
                Form1.Instance.AddErrorMessage("Unable to access " + path);
                Console.Error.WriteLine(e.ToString());
                loaded = false;
                return;
            }

            processChildFiles(files);
            processChildFolder(info);

            gbTotalSize = convertToGB(totalSize);
            gbTotalFileSize = convertToGB(totalFileSize);
        }

        private void processChildFiles(FileInfo[] files)
        {
            foreach (var file in files)
            {
                totalSize += (ulong)file.Length;
                totalFileSize += (ulong)file.Length;
            }
        }

        private void processChildFolder(DirectoryInfo info)
        {
            LinkedList<Directory> array = new LinkedList<Directory>();

            foreach (var folder in info.GetDirectories())
            {
                var child = new Directory(folder);

                totalSize += child.totalSize;

                //Only add folder if larger than 100 mb
                if (child.totalSize > 104857600)
                {
                    array.AddLast(child);
                }
            }

            children = array.ToArray();

            // Update percent 
            foreach (var folder in children)
            {
                folder.UpdatePercent(totalSize);
            }
        }

        public void UpdatePercent(ulong parentTotalSize)
        {
            if (totalSize == 0)
                return;

            percent = (double)totalSize / parentTotalSize;
        }

        private double convertToGB(ulong length)
        {
            return (double)length / 1024 / 1024 / 1024;
        }
    }
}
