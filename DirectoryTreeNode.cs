using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace FreespaceAnalyzer
{
    class DirectoryTreeNode : TreeNode
    {
        public Directory directory;
        public ulong TotalSize
        {
            get
            {
                return directory.totalSize;
            }
        }

        public DirectoryTreeNode(Directory directory)
        {
            this.directory = directory;
            base.Text = GetText();

            foreach (Directory child in directory.children)
            {
                TreeNode childNode = new DirectoryTreeNode(child);

                Nodes.Add(childNode);
            }
        }

        public void Refresh(ulong totalSize)
        {
            directory.UpdatePercent(totalSize);

            Text = GetText();
        }

        private string GetText()
        {
            string percent = directory.percent.ToString("P0");
            string size = directory.gbTotalSize.ToString("N2");

            return string.Format("{0} - {1}({2} GB)", directory.path, percent, size);
        }
    }

    // Create a node sorter that implements the IComparer interface.
    public class NodeSorter : IComparer
    {
        // Compare the length of the strings, or the strings
        // themselves, if they are the same length.
        public int Compare(object x, object y)
        {
            DirectoryTreeNode tX = x as DirectoryTreeNode;
            DirectoryTreeNode tY = y as DirectoryTreeNode;

            return tX.TotalSize.CompareTo(tY.TotalSize) * -1;
        }
    }
}
