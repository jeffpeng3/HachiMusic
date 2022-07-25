using System;
using System.Windows;
using System.Windows.Controls;

namespace musicplayer.Controls
{
    /// <summary>
    /// MusicView.xaml 的互動邏輯
    /// </summary>
    public partial class MusicView : UserControl
    {
        public static readonly DependencyProperty dutationProperty = DependencyProperty.Register("Dutation", typeof(TimeSpan), typeof(MusicView), new FrameworkPropertyMetadata(TimeSpan.Zero));

        public static readonly DependencyProperty artistProperty = DependencyProperty.Register("Artist", typeof(string), typeof(MusicView), new FrameworkPropertyMetadata(string.Empty));

        public static readonly DependencyProperty titleProperty = DependencyProperty.Register("Title", typeof(string), typeof(MusicView), new FrameworkPropertyMetadata(string.Empty));

        public static readonly DependencyProperty srcProperty = DependencyProperty.Register("Src", typeof(Uri), typeof(MusicView), new FrameworkPropertyMetadata(new Uri("https://img.youtube.com/vi/%3Cinsert-youtube-video-id-here%3E/maxresdefault.jpg")));

        public TimeSpan Dutation
        {
            get { return (TimeSpan)GetValue(dutationProperty); }
            set { SetValue(dutationProperty, value); }
        }

        public string Artist
        {
            get { return (string)GetValue(artistProperty); }
            set { SetValue(artistProperty, value); }
        }

        public string Title
        {
            get { return (string)GetValue(titleProperty); }
            set { SetValue(titleProperty, value); }
        }
        public Uri Src
        {
            get { return (Uri)GetValue(srcProperty); }
            set { SetValue(srcProperty, value); }
        }
        public MusicView()
        {
            InitializeComponent();
        }
    }
}
