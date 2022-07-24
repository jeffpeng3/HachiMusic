using musicplayer.Controls;
using musicplayer.Modules;
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
using YoutubeExplode;
using YoutubeExplode.Common;

namespace musicplayer.Pages
{
    /// <summary>
    /// main.xaml 的互動邏輯
    /// </summary>
    public partial class DiscoverPage : UiPage
    {
        readonly static YoutubeClient Youtube = new();
        public DiscoverPage()
        {
            InitializeComponent();
        }

        public async Task SearchAsync(string target)
        {
            ThisList.Items.Clear();
            Ring.Visibility = Visibility.Visible;
            var videos = await Youtube.Search.GetVideosAsync(target).CollectAsync(30);
            Ring.Visibility = Visibility.Collapsed;
            foreach (var item in videos)
            {
                var task = Utils.GetMaxResolutionAsync(item.Id);
                MusicView musicView = new()
                {
                    Title = item.Title,
                    Artist = item.Author.ChannelTitle,
                    Dutation = (TimeSpan)item.Duration,
                    Height = 55,
                    Margin = new Thickness(0, 5, 0, 0),
                    Src = await task
                };
                ThisList.Items.Add(musicView);
            }

        }
    }
}
