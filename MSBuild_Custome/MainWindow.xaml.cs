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
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

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
            Loaded += MainWindow_Loaded;

            }

        XmlDocument xmldoc;
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
            {
            tbProjectPath.Text = @"F:\WorkPlace\DigitalBook\DigitalBook.sln";
            tbMSBPath.Text = @"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe";

          
            }

        /// <summary>
        /// 初始化xml文件
        /// </summary>
        /// <returns>返回文件路径</returns>
        private  string InitXml(string filePath)
            {
            var xmlpath = filePath;
            XmlHelper.CreateXmlDocument(xmlpath, "root", "1.0", "utf-8", "yes");
            //打包配置节点
            XmlHelper.CreateOrUpdateXmlNodeByXPath(xmlpath, "root", "XmlModelHead", "");
            //ms编译节点
            XmlHelper.CreateOrUpdateXmlNodeByXPath(xmlpath, "root", "XmlPreMode", "");
            //文件对照节点
            XmlHelper.CreateOrUpdateXmlNodeByXPath(xmlpath, "root", "XmlFilesMode", "");
            return xmlpath;
            }

        static AutoResetEvent mEvent = new AutoResetEvent(false);
        /// <summary>
        /// 编译按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
            {
            //生成配置文件名称
            if (string.IsNullOrEmpty(tbPackage.Text)) { tbPackage.Text = string.Format("{0}/{1}{2}", Environment.CurrentDirectory, tbPackageName.Text, ".xml"); }
            if (!Directory.Exists(tbPackage.Text)) InitXml(tbPackage.Text);
            XElement xe = XElement.Load(tbPackage.Text);
            var list = from s in xe.Elements("XmlPreMode") select s;
            if (list.Count()>0)
                {
                XElement first = list.First();
                          ///替换新的节点
           first.ReplaceNodes(
                       new XElement("MSBuildPath", tbMSBPath.Text),
                    new XElement("ProjectPath", tbProjectPath),
                   //  new XElement("OutPutPath", (double)dgvBookInfo.CurrentRow.Cells[4].Value),
                     new XElement("FilterStrings", @"^.+\.(pdb)|(log)$")
                       );
                xe.Save(tbPackage.Text);
                }
            else
                {
                XElement record = new XElement(
            new XElement("XmlPreMode",
             new XElement("MSBuildPath", tbMSBPath.Text),
                    new XElement("ProjectPath", tbProjectPath),
                     //  new XElement("OutPutPath", (double)dgvBookInfo.CurrentRow.Cells[4].Value),
                     new XElement("FilterStrings", @"^.+\.(pdb)|(log)$")));
                           xe.Add(record);
                           xe.Save(tbPackage.Text);
                }
            var projectPath = tbProjectPath.Text;//tbProjectPath.Text;
            var msbuildPath = tbMSBPath.Text;
            Thread msThread=   new Thread(() => {
                string commands = string.Format(@"{0} /t:Build /p:TargetFramework=v4.0 /p:Configuration=Release  /flp1:logfile=errors.txt;errorsonly", projectPath);
            Process p = new Process();
            p.StartInfo.FileName = msbuildPath;
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

        /// <summary>
        /// 打包项目文件（xml）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbPackage_MouseDown(object sender, MouseButtonEventArgs e)
            {
            string filePath = GetFilePath("(*.xml)");
            if (!string.IsNullOrEmpty(filePath)) {
                tbPackage.Text = filePath; }
            }
        /// <summary>
        /// 项目地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbProjectPath_MouseDown(object sender, MouseButtonEventArgs e)
            {
            string filePath = GetFilePath("(*.sln,*.project)|*.sln;*.project");
            if (!string.IsNullOrEmpty(filePath)) tbProjectPath.Text = filePath;
            }

        private string GetFilePath(string filter)
            {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            string filePath = "";
            ofd.Filter = filter;
            ofd.FileOk += (m, s) => {
                filePath = ofd.FileName;
            };
            ofd.ShowDialog();
            return filePath;
            }

        private void tbPackage_GotFocus(object sender, RoutedEventArgs e)
            {
            string filePath = GetFilePath("(*.xml)|*.xml|All files(*.*)|*.*");
            if (!string.IsNullOrEmpty(filePath)) {
                tbPackage.Text = filePath;
                //文件存在 读取文件并赋值
                if (File.Exists(filePath))
                    {
                    XElement xe = XElement.Load(filePath);
                    var se = from s in xe.Elements("XmlPreMode") select s.FirstNode;
                    
                 //   tbMSBPath.Text = se.ToList().Element("MSBuildPath").Value;
                    //tbProjectPath.Text = se.Element("ProjectPath").Value;
                    }
                        }
            }
        }
    }
