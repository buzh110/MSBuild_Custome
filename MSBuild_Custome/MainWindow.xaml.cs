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

            var psi = new ProcessStartInfo("cmd.exe", String.Format(".\{0} {1} /p:Configuration=Release", MSBuildPath,PackagePath));
            psi.WindowStyle = ProcessWindowStyle.Normal;
            psi.Verb = "runas";
            var proc = Process.Start(psi);
            proc.WaitForExit();
        //    buttonInstall.IsEnabled = false;
          //  buttonRemove.IsEnabled = true;
            MessageBox.Show("Install completed!  If the 'Make EXE' option isn't showing up, reset your program defaults and reinstall Make-EXE.", "Install Completed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
