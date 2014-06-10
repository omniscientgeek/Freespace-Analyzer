using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace FreespaceAnalyzer
{
    public partial class Form1 : Form
    {
        public static Form1 Instance { get; private set; }

        public Form1()
        {
            InitializeComponent();
            Form1.Instance = this;
            treeView1.TreeViewNodeSorter = new NodeSorter();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            button1.Text = "Processing";
            button1.Enabled = false;

            var info = new DirectoryInfo(textBox1.Text);
            Engine engine = new Engine(this, info);

            ThreadStart threadStart = new ThreadStart(engine.run);
            Thread thread = new Thread(threadStart);

            thread.Start();
        }

        delegate void SetStatusCallback(string status);
        public void SetStatus(string status)
        {
            if (statusStrip1.InvokeRequired)
            {
                statusStrip1.Invoke(new SetStatusCallback(SetStatus), status);
            }
            else
            {
                toolStripStatusLabel1.Text = status;
            }
        }

        private delegate void AddTreeNodeCallback(DirectoryTreeNode node, ulong totalSize);
        internal void AddTreeNode(DirectoryTreeNode node, ulong totalSize)
        {
            if (treeView1.InvokeRequired)
            {
                treeView1.Invoke(new AddTreeNodeCallback(AddTreeNode), node, totalSize);
            }
            else
            {
                foreach (DirectoryTreeNode child in treeView1.Nodes)
                {
                    child.Refresh(totalSize);
                }

                treeView1.Nodes.Add(node);
            }
        }

        delegate void AddErrorMessageCallback(string msg);
        public void AddErrorMessage(string msg)
        {
            if (errorListBox.InvokeRequired)
            {
                errorListBox.Invoke(new AddErrorMessageCallback(AddErrorMessage), msg);
            }
            else
            {
                errorListBox.Items.Add(msg);
            }
        }

        delegate void DoneCallback();
        public void Done()
        {
            if (treeView1.InvokeRequired)
            {
                treeView1.Invoke(new DoneCallback(Done));
            }
            else
            {
                button1.Text = "Done";
                button1.Enabled = true;
            }
        }
    }
}
