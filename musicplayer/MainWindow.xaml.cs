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

namespace musicplayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UiWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged = delegate { };
        private bool _IsPadOpen;
        public bool IsPadOpen
        {
            set { _IsPadOpen = value; OnPropertyChanged("IsPadOpen"); }
            get { return _IsPadOpen; }
        }
        public MainWindow()
        {
            IsPadOpen = true;
            InitializeComponent();
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

                /*
                 * 
                 * Add this song to music player and update front-end.
                 * 
                 */
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
        private void PadOpenOrClose(object sender, RoutedEventArgs e)
        {
            IsPadOpen = !IsPadOpen;
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
            Close();
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
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void OnAnyControlPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //Console.WriteLine(e.PropertyName);
        }

        private void LoopButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
                return;
            Player.LoopMode = (LoopModeEnum)(((int)Player.LoopMode + 1) % 3);
            btn.Content = FindResource(Enum.GetName(typeof(LoopModeEnum), Player.LoopMode));
        }
    }
}