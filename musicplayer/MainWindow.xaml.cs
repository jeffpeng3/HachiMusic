using Microsoft.VisualStudio.Threading;
using musicplayer.Controls;
using musicplayer.Modules;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;

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

    }
}