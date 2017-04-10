using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
        string installedPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Make-EXE\Make-EXE.exe";
        public MainWindow()
            {
            InitializeComponent();
            }

        private void Button_Click(object sender, RoutedEventArgs e)
            {
            //项目路径
            string PackagePath = tbProjectPath.Text;
            //编译Msbuild地址
            string MSBuildPath = tbMSBPath.Text;

          // var psi = new ProcessStartInfo("cmd.exe", String.Format(".\{0} {1} /p:Configuration=Release", MSBuildPath,PackagePath));
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = false;
            p.Start();
            string strOutput = null;
            //   p.StandardInput.WriteLine("cd D:\\flv\\mplayer");
            //   p.StandardInput.WriteLine("cd d:");
            // p.StandardInput.WriteLine(string.Format("D:\\flv\\mplayer\\mencoder \"c:\\vs.wmv\" -o \"c:\\output.flv\" -of lavf  -lavfopts i_certify_that_my_video_stream_does_not_use_b_frames -oac mp3lame -lameopts abr:br=56 -ovc lavc -lavcopts vcodec=flv:vbitrate={0}:mbd=2:mv0:trell:v4mv:cbp:last_pred=3:dia=4:cmp=6:vb_strategy=1 -vf scale=512:-3 -ofps 12 -srate 22050", 200));
            p.StandardInput.WriteLine(@"c: ");  //先转到系统盘下
            p.StandardInput.WriteLine(@"cd: C:\Program Files (x86)\MSBuild\14.0\Bin");  //先转到系统盘下
            p.StandardInput.WriteLine("exit");
            strOutput = p.StandardOutput.ReadToEnd();
            Console.WriteLine(strOutput);
            p.WaitForExit();
            
            p.Close();
        }
        }
    }
