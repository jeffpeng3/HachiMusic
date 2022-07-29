using musicplayer.Enums;
using musicplayer.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace musicplayer.Controls
{
    /// <summary>
    /// MusicControlPanel.xaml 的互動邏輯
    /// </summary>
    public partial class MusicControlPanel : UserControl
    {
        public MusicControlPanel()
        {
            InitializeComponent();
        }
        private void PlayButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is not Button)
                return;
            if (Player.CurrentPlayer is not Player player)
                return;
            if (Player.SongList.Count == 0)
                return;
            switch (Player.Status)
            {
                case PlayStatusEnum.NotPlaying:
                    _ = player.Play(0);
                    PlayButton.Content = FindResource("PauseIcon");
                    break;
                case PlayStatusEnum.Playing:
                    player.Pause();
                    PlayButton.Content = FindResource("PlayIcon");
                    break;
                case PlayStatusEnum.Pause:
                    player.Resume();
                    PlayButton.Content = FindResource("PauseIcon");
                    break;
            }
        }
        private void RandomButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;
            switch (Player.IsRandom)
            {
                case true:
                    button.Background = new SolidColorBrush(Colors.Gray);
                    break;
                case false:
                    button.Background = new SolidColorBrush(Color.FromRgb(85, 85, 85));
                    break;
            }
            Player.IsRandom ^= true;
        }
        private void LoopButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;
            Player.LoopMode = Player.LoopMode.Next();
            button.Content = FindResource(Enum.GetName(Player.LoopMode));
        }
    }
}
