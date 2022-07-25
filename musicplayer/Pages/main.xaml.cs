using System;
using System.Windows;
using Wpf.Ui.Controls;
using musicplayer.Controls;
using musicplayer.Modules;

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
            QueueList.ItemsSource = Player.SongList;
            /*
            for (int i = 0; i < 10; i++)
            {
                MusicView musicView = new()
                {
                    Title = "[Playlist] The original stone of a star that appeared like a comet! (Hoshimachi Suisei Playlist)",
                    Artist = "Ninomae Ina'nis Ch. hololive-EN",
                    Dutation = new TimeSpan(0, 5, 5),
                    Src = new Uri(@"https://img.youtube.com/vi/84uvWfZqqeg/maxresdefault.jpg"),
                    Height = 55,
                    Margin = new Thickness(0, 5, 0, 0)
                };
                QueueList.Items.Add(musicView);
            }
            */
        }

    }
}
