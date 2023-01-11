using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonLauncher
{
    internal class IJsonSerializableHelper
    {
        public class VersionsListItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string product { get; set; }
            /// <summary>
            /// 产品
            /// </summary>
            public int version { get; set; }
            /// <summary>
            /// 版本号
            /// </summary>
            public int build { get; set; }
            /// <summary>
            /// 构建
            /// </summary>
            public string title { get; set; }
            /// <summary>
            /// 描述
            /// </summary>
            public string downloadLink { get; set; }
            /// <summary>
            /// 下载链接
            /// </summary>
        }

        public class Root
        {
            /// <summary>
            /// 文件信息
            /// </summary>
            public int configVersion { get; set; }
            /// <summary>
            /// 配置版本
            /// </summary>
            public string configTitle { get; set; }
            /// <summary>
            /// 配置标题
            /// </summary>
            public string configInfo { get; set; }
            /// <summary>
            /// 配置信息
            /// </summary>
            public List<VersionsListItem> versionsList { get; set; }
        }
    }
}
