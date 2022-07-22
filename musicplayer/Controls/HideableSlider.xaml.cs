﻿using System;
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
using Wpf.Ui.Controls;
using XamlFlair;

namespace musicplayer.Controls
{
    /// <summary>
    /// HideableSlider.xaml 的互動邏輯
    /// </summary>
    public partial class HideableSlider : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged = delegate { };

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(MusicView), new FrameworkPropertyMetadata(0.5));

        public static readonly DependencyProperty MuteProperty = DependencyProperty.Register("Mute", typeof(bool), typeof(MusicView), new FrameworkPropertyMetadata(false));
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { Mute = false; value = Math.Max(0, Math.Min(1, value)); SetValue(ValueProperty, value); ChangeIcon(value); OnPropertyChanged("Value"); }
        }
        public bool Mute
        {
            get { return (bool)GetValue(MuteProperty); }
            set { SetValue(MuteProperty, value); OnPropertyChanged("Mute"); }
        }
        public HideableSlider()
        {
            InitializeComponent();
            ThisControl.PreviewMouseWheel += (sender, e) =>
            {
                Value += slider.SmallChange * e.Delta / 120;
            };
        }
        private void MuteToggle(object sender, RoutedEventArgs e)
        {
            Mute = !Mute;
            if (Mute)
            {
                button.Content = FindResource("Mute");
            }
            else
            {
                ChangeIcon(Value);
            }
        }
        private void ChangeIcon(double value)
        {
            var target = value switch
            {
                0 => FindResource("Mute"),
                < 0.33 => FindResource("Low"),
                < 0.66 => FindResource("Medium"),
                _ => FindResource("High")
            };
            button.Content = target;
        }
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
