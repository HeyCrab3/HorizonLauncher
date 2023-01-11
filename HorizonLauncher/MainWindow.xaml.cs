using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Handlers;
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
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace HorizonLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UiWindow
    {
        public Process p = new();

        public MainWindow()
        {
            InitializeComponent();
            Dropdown.Items.Clear();
            IStartupHelper.GetPluginList(Dropdown);
            var VersionsList = WebHandler.IGetReq("https://gc.heycrab.xyz/yu0ZlG8qUPyA4nmjXfcf/9Q7WKxDepMrpicDwbo4x/horizon/versions.json");
            IJsonSerializableHelper.Root root = JsonConvert.DeserializeObject<IJsonSerializableHelper.Root>(VersionsList);
            ConfigVersion.Text = root.configVersion.ToString();
            ConfigTitle.Text = root.configTitle;
            ConfigInfo.Text = root.configInfo;
            IVersionHelper.VersionLoader(VersionsList1, root);
            LoadingBar.Visibility = Visibility.Hidden;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadingBar.Visibility = Visibility.Visible;
            var VersionsList = WebHandler.IGetReq("https://gc.heycrab.xyz/yu0ZlG8qUPyA4nmjXfcf/9Q7WKxDepMrpicDwbo4x/horizon/versions.json");
            IJsonSerializableHelper.Root root = JsonConvert.DeserializeObject<IJsonSerializableHelper.Root>(VersionsList);
            ConfigVersion.Text = root.configVersion.ToString();
            ConfigTitle.Text = root.configTitle;
            ConfigInfo.Text = root.configInfo;
            IVersionHelper.VersionReloader(VersionsList1, root);
            LoadingBar.Visibility = Visibility.Hidden;
        }
        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void RefreshVersion(object sender, RoutedEventArgs e)
        {
            Dropdown.Items.Clear();
            IStartupHelper.GetPluginList(Dropdown);
        }
        private async void DownloadVersion_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem SelectedItem = VersionsList1.SelectedItem as ListBoxItem;
            if (SelectedItem == null)
            {
                Operation.Text = "主播没有选中任何一个文件";
                STatusBar.Background = Brushes.Crimson;
            }
            else
            {
                DownloadProgress.Visibility= Visibility.Visible;
                string downloadLink = SelectedItem.Uid;
                string filename = (string)SelectedItem.Tag;
                Operation.Text = "正在下载文件 " + filename;
                var result = await DownloadFile(downloadLink, filename);
                if (result == true)
                {
                    Operation.Text = "文件下载成功：" + filename;
                    DownloadProgress.Visibility = Visibility.Hidden;
                }
            }
        }
        public async Task<bool> DownloadFile(string url, string fileName)
        {
            try
            {
                var progressMessageHandler = new ProgressMessageHandler(new HttpClientHandler());
                progressMessageHandler.HttpReceiveProgress += (_, e) =>
                {
                    Dispatcher.BeginInvoke(new Action(delegate
                    {
                        DownloadProgress.Value = e.ProgressPercentage;//下载进度百分比
                        STatusBar.Background = Brushes.ForestGreen;
                        Operation.Text = "正在下载文件 " + fileName + " " + e.ProgressPercentage + "%"; // 操作栏展示实时进度
                    }));
                };
                using (var client = new HttpClient(progressMessageHandler))
                using (var filestream = new FileStream(fileName, FileMode.Create))
                {
                    var netstream = await client.GetStreamAsync(url);
                    await netstream.CopyToAsync(filestream);//写入文件
                }
                return true;
            }
            catch (Exception ex)
            {
                Operation.Text = "未能下载文件：" + fileName + "，错误已弹窗显示";
                DownloadProgress.Visibility = Visibility.Hidden;
                STatusBar.Background = Brushes.Crimson;
                System.Windows.MessageBox.Show(ex.ToString(),"未能下载文件，以下是错误信息",MessageBoxButton.OK,MessageBoxImage.Error);
                return false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string? file = Dropdown.SelectedValue.ToString();
                if (file == "无可用版本，请先下载一个可用版本")
                {
                    System.Windows.MessageBox.Show("当前没有可以用于启动的版本。请去下载页面下载一个。", "没有可以启动的版本", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    STatusBar.Background = Brushes.Crimson;
                    Operation.Text = "未能启动：当前没有可以用于启动的版本";
                }
                else if (file == "正在加载版本，请稍等")
                {
                    System.Windows.MessageBox.Show("正在加载版本，请稍等。", "正在加载", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    STatusBar.Background = Brushes.Crimson;
                    Operation.Text = "未能启动：版本列表正在加载。";
                }
                else
                {
                    start.Content = "已启动";
                    start.IsEnabled = false;
                    STatusBar.Background = Brushes.ForestGreen;
                    Operation.Text = "已启动版本：" + file;
                    p.Exited += P_Exited;
                    p.StartInfo.FileName = "C:\\Program Files\\Microsoft Office\\root\\Office16\\POWERPNT.exe";
                    p.StartInfo.Arguments = file;
                    p.Start();
                }
            }
            catch
            {
            }
        }

        private void P_Exited(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                start.Content = "启动";
                start.IsEnabled = true;
            });
        }
    }
}
