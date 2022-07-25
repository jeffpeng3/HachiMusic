using System;
using System.Linq;
using System.Windows;
using YoutubeExplode;
using Wpf.Ui.Controls;
using musicplayer.Modules;
using System.Windows.Input;
using musicplayer.Controls;
using YoutubeExplode.Common;
using System.Threading.Tasks;
using YoutubeExplode.Search;
using System.Collections.Generic;

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
            List<Task<MusicView?>> TaskList = new();
            foreach (var item in videos)
            {
                TaskList.Add(func(item));
            }
            await Task.WhenAny(TaskList);
            Ring.Visibility = Visibility.Collapsed;
            foreach (var item in TaskList)
            {
                ThisList.Items.Add(await item);
            }

            async Task<MusicView?> func(VideoSearchResult item)
            {
                var T1 = MusicView.TryToCreateMusicViewAsync(item.Url);
                if (await T1 is not MusicView musicView)
                {
                    return null;
                }
                musicView.Tag = item.Id;
                musicView.MouseDoubleClick += WhenDoubleClickSong;
                return musicView;
            }

        }
        private async void WhenDoubleClickSong(object sender, MouseButtonEventArgs e)
        {
            if (sender is not MusicView temp)
            {
                return;
            }
            var URL = $"https://www.youtube.com/watch?v={temp.Tag}";
            if (await Song.TryCreateSongAsync(URL) is not Song song)
            {
                return;
            }
            Player.CurrentPlayer?.AddSong(song);
        }

    }
}
