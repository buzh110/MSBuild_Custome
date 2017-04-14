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
using Path = System.IO.Path;

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
        static AutoResetEvent mEvent = new AutoResetEvent(false);
        private void Button_Click(object sender, RoutedEventArgs e)
            {


         Thread msThread=   new Thread(() => {
          
             //项目路径
             var PackagePath = @"F:\WorkPlace\DigitalBook\DigitalBook.sln";//tbProjectPath.Text;
            //编译Msbuild地址
             //   string currentdir = System.IO.Path.Combine( Environment.CurrentDirectory , "Output");
                string commands = string.Format(@"{0} /t:Build /p:TargetFramework=v4.0 /p:Configuration=Release  /flp1:logfile=errors.txt;errorsonly", PackagePath);
                //Directory.Delete(currentdir,true);
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

            p.StandardInput.AutoFlush = true;
            
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            p.WaitForExit();
                

            p.Close();
             mEvent.Set();
             p.OutputDataReceived -= sortProcess_OutputDataReceived;
             p.ErrorDataReceived -= errorProcess_OutputDataReceived;
         });
            msThread.Start();

            new Thread(() => {
                mEvent.WaitOne();
                P_Exited(null, null);
            }).Start();
            }

        private void P_Exited(object sender, EventArgs e)
            {
            var PackagePath = @"F:\WorkPlace\DigitalBook\DigitalBook.sln";//tbProjectPath.Text;
            //生成包地址
            string foldpath =Path.Combine(PackagePath.Substring(0, PackagePath.LastIndexOf("\\")),"Build","Release");
            //拷贝地址（默认）
            string OutPutPath = Path.Combine(Environment.CurrentDirectory, "OutPut");
            if (Directory.Exists(OutPutPath)) Directory.Delete(OutPutPath, true);
            FileHelper.CopyDirectory(foldpath, OutPutPath, @"^.+\.(pdb)|(log)$");

            var filelist = FileHelper.GetFileNames(OutPutPath, "*", true);
            //var list = FileHelper.GetFileNames(OutPutPath).Where(s => s.EndsWith(".pdb") || s.Contains(".log"));
         
            //foreach (var item in list)
            //    {
            //    FileHelper.DeleteFile(item);
            //    }
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

        private void btPackage_Click(object sender, RoutedEventArgs e)
            {
            //Dispatcher.Invoke(() => {
            //    lbshow.Items.;
            //});
            //打包
            new Thread(()=> {
              var  INNOSCRIPTFILE = "C:\\Users\\yinji\\Desktop\\test.iss";
                string Argument = string.Format("/q \"{0}\"", INNOSCRIPTFILE);
                //Directory.Delete(currentdir,true);
                Process p = new Process();
                p.StartInfo.FileName = "iscc";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.Arguments = Argument;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;

                p.Start();
                p.OutputDataReceived += sortProcess_OutputDataReceived;
                p.ErrorDataReceived += errorProcess_OutputDataReceived;

                p.StandardInput.AutoFlush = true;

                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.WaitForExit();


                p.Close();
              
            }).Start();
            }
        }
    }
