using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleFileManager
{
    public partial class Form1 : Form
    {
        private string filePath = @"\\192.168.0.15\ubuntu@15";
        private bool isFile = false;
        private string currentlySelectedItemName = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FilePathTextBox.Text = filePath;
            loadFilesAndDirectories();
        }

        private void removeBackSlash()
        {
            try
            {
                string path = FilePathTextBox.Text;
                if (path.LastIndexOf("\\") == path.Length - 1)
                {
                    FilePathTextBox.Text = path.Substring(0, path.Length - 1);
                }
            }
            catch
            {
            }
        }

        private void GoBack()
        {
            try
            {
                removeBackSlash();
                string path = FilePathTextBox.Text;
                path = path.Substring(0, path.LastIndexOf("\\"));
                this.isFile = false;
                FilePathTextBox.Text = path;
                removeBackSlash();
            }
            catch (Exception ex)
            {
            }
        }

        private void loadFilesAndDirectories()
        {
            DirectoryInfo fileList;
            string tempFilePath = "";
            FileAttributes fileAttr;
            try
            {
                if (isFile)
                {
                    tempFilePath = string.Format(filePath + "\\" + currentlySelectedItemName);
                    FileInfo fileDetails = new FileInfo(tempFilePath);
                    FileNameLabel.Text = fileDetails.Name;
                    FileTypeLabel.Text = fileDetails.Extension;
                    fileAttr = File.GetAttributes(tempFilePath);
                    Process.Start(@tempFilePath);
                }
                else
                {
                    fileAttr = File.GetAttributes(filePath);
                }

                if ((fileAttr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    fileList = new DirectoryInfo(filePath);
                    FileInfo[] files = fileList.GetFiles(); // get all files
                    DirectoryInfo[] dirs = fileList.GetDirectories(); // get all directories

                    listView1.Items.Clear();

                    for (int i = 0; i < files.Length; i++)
                    {
                        listView1.Items.Add(files[i].Name);
                    }

                    for (int i = 0; i < dirs.Length; i++)
                    {
                        listView1.Items.Add(dirs[i].Name);
                    }
                }
                else
                {
                    FileNameLabel.Text = this.currentlySelectedItemName;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void loadButtonAction()
        {
            removeBackSlash();
            filePath = FilePathTextBox.Text;
            loadFilesAndDirectories();
            isFile = false;
        }

        private void GoButton_Click(object sender, EventArgs e)
        {
            loadButtonAction();
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            currentlySelectedItemName = e.Item.Text;
            FileNameLabel.Text = e.Item.Text;

            FileAttributes fileAttributes = File.GetAttributes(filePath + "\\" + currentlySelectedItemName);
            if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                isFile = false;
                FilePathTextBox.Text = filePath + "\\" + currentlySelectedItemName;
            }
            else
            {
                isFile = true;
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            loadButtonAction();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            GoBack();
            loadButtonAction();
        }
    }
}