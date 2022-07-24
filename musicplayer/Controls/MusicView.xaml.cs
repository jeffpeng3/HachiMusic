using System;
using System.Collections.Generic;
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
