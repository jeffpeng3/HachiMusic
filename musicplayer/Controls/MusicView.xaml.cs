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

        public static readonly DependencyProperty dutationProperty = DependencyProperty.Register("dutation", typeof(TimeSpan), typeof(MusicView), new FrameworkPropertyMetadata(TimeSpan.Zero));

        public static readonly DependencyProperty artistProperty = DependencyProperty.Register("artist", typeof(string), typeof(MusicView), new FrameworkPropertyMetadata(string.Empty));

        public TimeSpan dutation
        {
            get { return (TimeSpan)GetValue(dutationProperty); }
            set { SetValue(dutationProperty, value); }
        }

        public string artist
        {
            get { return (string)GetValue(artistProperty); }
            set { SetValue(artistProperty, value); }
        }
        public MusicView()
        {
            InitializeComponent();
        }
    }
}
