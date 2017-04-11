using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MSBuild_Custome
    {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
        {
        public MainWindow()
            {
            InitializeComponent();
            }
      
        private void Button_Click(object sender, RoutedEventArgs e)
            {
            new Thread(() => {
            //项目路径
             var PackagePath = @"F:\WorkPlace\DigitalBook\DigitalBook.sln";//tbProjectPath.Text;
            //编译Msbuild地址
                string currentdir = System.IO.Path.Combine( Environment.CurrentDirectory , "Output");
                string commands = string.Format(@"{0}  /p:Configuration=Release,OutDir={1}  /flp1:logfile=errors.txt;errorsonly", PackagePath, currentdir);
                Directory.Delete(currentdir,true);
            Process p = new Process();
            p.StartInfo.FileName = @"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe";
            p.StartInfo.UseShellExecute = false;
                p.StartInfo.Arguments = commands;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
      
            p.Start();
            p.OutputDataReceived += sortProcess_OutputDataReceived;
            p.ErrorDataReceived += errorProcess_OutputDataReceived;
            string strOutput = null;
            p.StandardInput.AutoFlush = true;

            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            p.WaitForExit();
                

            p.Close();
            }).Start();
            }

         private void sortProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
            {
            if (!String.IsNullOrEmpty(e.Data))
                {
                Dispatcher.Invoke(new Action(() => { this.lbshow.Items.Add(e.Data); lbshow.ScrollIntoView(e.Data); }));
                }
            }
        //错误信息
        private void errorProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
            {
            if (!String.IsNullOrEmpty(e.Data))
                {
                Dispatcher.Invoke(new Action(() => { this.lbshow.Items.Add(e.Data); lbshow.ScrollIntoView(e.Data); }));
                }
            }
        }
    }
