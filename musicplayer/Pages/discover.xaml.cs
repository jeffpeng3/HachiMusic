using System;
using System.Windows;
using YoutubeExplode;
using Wpf.Ui.Controls;
using musicplayer.Modules;
using System.Windows.Input;
using musicplayer.Controls;
using YoutubeExplode.Common;
using System.Threading.Tasks;

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
                    Tag = item.Id,
                    Src = await task
                };
                musicView.MouseDoubleClick += WhenDoubleClickSong;
                ThisList.Items.Add(musicView);
            }
        }
        private async void WhenDoubleClickSong(object sender, MouseButtonEventArgs e)
        {
            if (sender is not MusicView temp)
            {
                return;
            }
            var URL = $"https://www.youtube.com/watch?v={temp.Tag}";
            var song = await Song.TryCreateSongAsync(URL);
            if (song == null)
            {
                return;
            }
            Player.CurrentPlayer?.AddSong(song);
        }

    }
}
