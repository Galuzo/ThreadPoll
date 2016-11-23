using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading;

namespace ThreadPoll
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static CustomThreadPool MyPool = CustomThreadPool.Instance;
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
           
            // Get the subdirectories for the specified directory.
            if (sourceDirName != "")
            {
                
               
                DirectoryInfo dir = new DirectoryInfo(sourceDirName);
                Form1 form1 = new Form1();
                if (!dir.Exists)
                {
                    throw new DirectoryNotFoundException(
                        "Source directory does not exist or could not be found: "
                        + sourceDirName);
                }

                DirectoryInfo[] dirs = dir.GetDirectories();
                // If the destination directory doesn't exist, create it.
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                }

                // Get the files in the directory and copy them to the new location.
                FileInfo[] files = dir.GetFiles();

                foreach (FileInfo file in files)
                {
                    string temppath = Path.Combine(destDirName, file.Name);

                    MyPool.QueueUserTask(() =>
                    {
                        // sho(Thread.CurrentThread.ManagedThreadId);

                        file.CopyTo(temppath, false);


                    },
                     (ts) =>
                     {

                         using (StreamWriter sw = File.AppendText("C:\\Users\\Galik\\Desktop\\CustomThreadPool\\CustomThreadPool\\log.txt"))
                         {


                             string str = (Thread.CurrentThread.ManagedThreadId).ToString();
                             str = str + " " + temppath + " " + (DateTime.Now).ToString();
                             sw.WriteLine(str);

                             
                             // Console.WriteLine("Текст записан в файл");
                         }


                     });
        

                }
                
                // If copying subdirectories, copy them and their contents to new location.
                if (copySubDirs)
                {
                    foreach (DirectoryInfo subdir in dirs)
                    {
                        string temppath = Path.Combine(destDirName, subdir.Name);
                        DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                    }
                }
               

            }
            

        }

        private void Btn_CopyDir_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            string path = "", newpath = "";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                path = folderBrowserDialog1.SelectedPath;
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    newpath = folderBrowserDialog1.SelectedPath;

                }
                DirectoryCopy(path, newpath, true);

            }
        }

       
        private void button2_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = File.CreateText("C:\\Users\\Galik\\Desktop\\CustomThreadPool\\CustomThreadPool\\log.txt")) ;
;

        }
        public void  CopyLongFile(string path,string newpath, long fileOffset ,long sizeOfpackage)
        {
            
            FileStream srcFile = null;
            FileStream destFile = null;

            
            destFile =
               File.Open(newpath,
               FileMode.OpenOrCreate,
               FileAccess.Write,
               FileShare.Write);
            srcFile =
               File.Open(path,
               FileMode.Open, FileAccess.Read,
               FileShare.Read);

            long bufSize = 10000000;
                if (bufSize >sizeOfpackage ) bufSize = sizeOfpackage;
                byte[] buf = new byte[bufSize];
                int readed = 0;
                int writed = 0;
                destFile.Seek(fileOffset+1, SeekOrigin.Begin);
                srcFile.Seek(fileOffset+1, SeekOrigin.Begin);
                while (writed < sizeOfpackage)
                {
                    readed = srcFile.Read(buf, 0, (int)bufSize);
                    destFile.Write(buf, 0, readed);
                    writed += readed;
                }
            destFile.Close();
            srcFile.Close();     
        }


        public void CopyFileJob(string path,string newpath)
        {
            FileStream srcFile =
                File.Open(path,
                FileMode.Open, FileAccess.Read,
                FileShare.Read);
            
            long length = srcFile.Length;
            srcFile.Close();
            int countOfThread = 10;
            CopyFile copy = new CopyFile();
            int i;
            List<PartOfPackage> list = copy.slicedPart(length, countOfThread);
            for ( i = 0; i < countOfThread; i++)
            {
              
                    CopyLongFile(path, newpath, list[i].offset, list[i].size);
               
            }
            
         


        }
        public void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
             FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
             string path = "", newpath = "";
             if (openFileDialog.ShowDialog() == DialogResult.OK)
             {
                 path = openFileDialog.FileName;


                 if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                 {
                     newpath = folderBrowserDialog1.SelectedPath+ "\\"+openFileDialog.SafeFileName;

                    CopyFileJob(path, newpath);

                 }
             }
            

        }

        private void Btn_CopyDIr_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            string path = "", newpath = "";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog.FileName;


                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    newpath = folderBrowserDialog1.SelectedPath + "\\" + openFileDialog.SafeFileName;

                    CopyFileJob(path, newpath);

                }
            }
        }
    }
}
