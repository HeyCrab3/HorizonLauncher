using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HorizonLauncher
{
    internal class IVersionHelper
    {
        public static void VersionLoader(ListBox Container, IJsonSerializableHelper.Root RequestResult) { 
            for (var i=0; i < RequestResult.versionsList.Count; i++) {
                var info = RequestResult.versionsList[i];
                ListBoxItem vnode = new ListBoxItem();
                vnode.Content = $"{info.product} {info.version}, Build {info.build} 详情：{info.title}";
                vnode.Tag = $"{info.product}-{info.version}-Build-{info.build}.pptx";
                vnode.Uid = info.downloadLink;
                Container.Items.Add(vnode);
            }
        }
        public static void VersionReloader(ListBox Container, IJsonSerializableHelper.Root RequestResult)
        {
            Container.Items.Clear();
            for (var i = 0; i < RequestResult.versionsList.Count; i++)
            {
                var info = RequestResult.versionsList[i];
                ListBoxItem vnode = new ListBoxItem();
                vnode.Tag = $"{info.product}-{info.version}-Build-{info.build}.pptx";
                vnode.Content = $"{info.product} {info.version}, Build {info.build} 详情：{info.title}";
                vnode.Uid = info.downloadLink;
                Container.Items.Add(vnode);
            }
        }
    }
}
