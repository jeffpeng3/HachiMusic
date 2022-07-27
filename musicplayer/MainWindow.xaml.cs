using System;
using System.Windows;
using Wpf.Ui.Controls;
using musicplayer.Enums;
using musicplayer.Pages;
using musicplayer.Modules;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using Wpf.Ui.Mvvm.Services;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace musicplayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UiWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            _ = new Player();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void NextClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void PreviousClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void PlayPauseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void OnSearch(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }
            var TargetString = ((TextBox)sender).Text;
            if (string.IsNullOrEmpty(TargetString))
            {
                return;
            }
            if (Utils.IsValidAddress(TargetString))
            {
                ((TextBox)sender).Text = string.Empty;
                var ThisSong = await Song.TryCreateSongAsync(TargetString);
                if (ThisSong is null)
                {
                    return;
                }
                var player = Player.CurrentPlayer;
                player?.AddSong(ThisSong);
                return;
            }
            else
            {
                if (RootNavigation.Current?.PageTag != "Discover")
                {
                    RootNavigation.Navigate("Discover");
                    await Task.Delay(100);
                }
                var page = RootFrame.Content as DiscoverPage;
                _ = page?.SearchAsync(TargetString);
            }
        }
        private async void WindowMove(object sender, MouseButtonEventArgs e)
        {
            var pos = Mouse.GetPosition(this);
            if (pos.Y > 75)
            {
                return;
            }
            if (WindowState == WindowState.Maximized)
            {
                while (pos == e.GetPosition(this))
                {
                    await Task.Delay(100);
                    if (Mouse.LeftButton != MouseButtonState.Pressed)
                    {
                        return;
                    }
                }
                Left = pos.X - Width / 2;
                Top = pos.Y - 30;
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Normal;
                WindowStyle = WindowStyle.SingleBorderWindow;
            }
            DragMove();
        }
        private void WindowClose(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }
        private void WindowMaxOrNormal(object sender, RoutedEventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    WindowState = WindowState.Maximized;
                    break;
                case WindowState.Maximized:
                    WindowState = WindowState.Normal;
                    break;
                default:
                    break;
            }
        }
        private void WindowMin(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

    }
}