using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HorizonLauncher
{
    internal class IStartupHelper
    {
        public static void GetPluginList(ComboBox list)
        {
            try
            {
                string path = Environment.CurrentDirectory;
                string[] files = Directory.GetFiles(path, "*.pptx");
                foreach (string file in files)
                {
                    list.Items.Add(file);
                }
                if (files.Length == 0)
                {
                    ComboBoxItem vnode = new();
                    vnode.Content = "无可用版本，请先下载一个可用版本";
                    vnode.IsEnabled = false;
                    vnode.Foreground = Brushes.Red;
                    list.Items.Add(vnode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "发生了错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
