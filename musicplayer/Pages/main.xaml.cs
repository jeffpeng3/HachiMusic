using System;
using System.Windows;
using Wpf.Ui.Controls;
using musicplayer.Controls;
using musicplayer.Modules;
using System.Windows.Data;
using System.Windows.Controls;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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
            Player.SongList.CollectionChanged += IsTimeToChange_Ouo;
        }

        private async void IsTimeToChange_Ouo(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is not ObservableCollection<Song> SongList)
            {
                return;
            }
            QueueList.ItemsSource = await Song2MusicViewConverter.ConvertAsync(SongList);
        }
    }
}
