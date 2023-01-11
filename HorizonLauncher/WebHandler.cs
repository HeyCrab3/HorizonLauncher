using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HorizonLauncher
{
    internal class WebHandler
    {
        public static string IGetReq(string url)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var result = httpClient.GetAsync(url);
                string? content = result.Result.Content.ReadAsStringAsync().Result.ToString();
                // MessageBox.Show(content);
                if (content == null)
                {
                    return null;
                }
                return content;
            }
            catch(Exception ex)
            {
                MessageBox.Show("发生了一些不好的事情，流式传输未能成功。\n" + ex, "Ooops!", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
