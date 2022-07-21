using System;
using System.Collections.Generic;
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
using Wpf.Ui.Controls;
using musicplayer.Controls;

namespace musicplayer.Pages
{
    /// <summary>
    /// main.xaml 的互動邏輯
    /// </summary>
    public partial class MainPage : UiPage
    {
        public MainPage()
        {
            InitializeComponent();
            for (int i = 0; i < 10; i++)
            {
                MusicView musicView = new()
                {
                    title = "『フタリボシ』 Ninomae Ina'nis & Tsukumo Sana (Cover)",
                    artist = "Ninomae Ina'nis Ch. hololive-EN",
                    dutation = new TimeSpan(0, 5, 5),
                    src = new Uri(@"https://img.youtube.com/vi/84uvWfZqqeg/maxresdefault.jpg"),
                    Height = 55,
                    Margin = new Thickness(0, 5, 0, 0)
                };
                QueueList.Items.Add(musicView);
            }
        }
    }
}
