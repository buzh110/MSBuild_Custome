using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSBuild_Custome
    {
    /// <summary>
    /// 打包配置
    /// </summary>
   public class XmlModelHead
        {
        //app名称
        public string MyAppName { get; set; }
        //版本
        public string MyAppVersion { get; set; }

        //发行单位
        public string MyAppPublisher { get; set; }

        /// <summary>
        /// 图标地址
        /// </summary>
        public string AppIocPath { get; set; }
        //url
        public string MyUrl { get; set; }
        }

    /// <summary>
    /// 编译前的设置
    /// </summary>
    public class XmlPreMode
        {
        //可执行msbuild路径
        public string MSBuildPath { get; set; }

        /// <summary>
        /// 解决方案文件路径
        /// </summary>
        public string ProjectPath { get; set; }

        /// <summary>
        /// 输出路径
        /// </summary>
        public string OutPutPath { get; set; }

        /// <summary>
        /// 拷贝过滤文件类型 正则表达式用,隔开
        /// </summary>
        public string FilterStrings { get; set; }

        }

   /// <summary>
   /// 保存的文件格式
   /// </summary>
    public class XmlFilesMode
        {
        /// <summary>
        /// 当前文件的地址
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 全称（包含后缀名）
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 后缀名
        /// </summary>
        public string FormatName { get; set; }
        }
    }
