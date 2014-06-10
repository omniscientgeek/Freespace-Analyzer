using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FreespaceAnalyzer
{
    class Engine
    {
        Form1 form;
        DirectoryInfo searchDirectory;

        public Engine(Form1 form, DirectoryInfo searchDirectory)
        {
            this.form = form;
            this.searchDirectory = searchDirectory;
        }

        public void run()
        {
            ulong totalSize = 0;

            foreach (var childDirectory in searchDirectory.GetDirectories())
            {
                Directory directory = new Directory(childDirectory);

                totalSize += directory.totalSize;

                display(directory, totalSize);
            }

            form.Done();
        }

        private void display(Directory directory, ulong totalSize)
        {
            if (directory.loaded == false)
                return;

            DirectoryTreeNode childNode = new DirectoryTreeNode(directory);

            form.AddTreeNode(childNode, totalSize);
        }

        private TreeNode getTreeNode(Directory directory)
        {
            string percent = directory.percent.ToString("P0");
            string size = directory.gbTotalSize.ToString("N2");
            string text = string.Format("{0} - {1}({2} GB)", directory.path, percent, size);

            TreeNode node = new TreeNode(text);

            foreach (Directory child in directory.children)
            {
                TreeNode childNode = getTreeNode(child);

                node.Nodes.Add(childNode);
            }

            return node;
        }
    }
}
