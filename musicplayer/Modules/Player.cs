using System;
using NAudio.Wave;
using musicplayer.Enums;
using NAudio.Wave.SampleProviders;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace musicplayer.Modules
{
    public class Player
    {
        private static readonly WaveOut waveOut = new();
        private static WaveStream? MFR;
        private static VolumeSampleProvider? VSP;
        public static readonly ObservableCollection<Song> SongList = new();

        // List<int> ReapeatTimes;
        public static int ListIndex { get; private set; } = 0;
        private static double _Volume = 0.5;
        public static double Volume
        {
            get => _Volume;
            set
            {
                _Volume = value;
                if (VSP is not null)
                    VSP.Volume = (float)_Volume * (float)_Volume * (float)_Volume;
            }
        }
        public static bool IsMute { get; set; } = false;
        public static bool IsRandom { get; set; } = false;
        public static TimeSpan Position { get; set; } = TimeSpan.Zero;
        public static PlayStatusEnum Status { get; set; } = PlayStatusEnum.NotPlaying;
        public static LoopModeEnum LoopMode { get; set; } = LoopModeEnum.LoopNone;
        public static Player? CurrentPlayer { get; set; } = null;
        public Player()
        {
            CurrentPlayer = this;
            waveOut.PlaybackStopped += AfterPlay;
        }
        public async Task Play(int index = -1)
        {
            if (SongList.Count == 0)
                return;
            Status = PlayStatusEnum.Playing;
            index = index < 0 ? ListIndex : index;
            var currSong = SongList[index];

            MFR = new MediaFoundationReader(currSong.AudioStream.ToString());
            VSP = new(MFR.ToSampleProvider());
            Volume = 0.5f;
            waveOut.Init(VSP);
            await Task.Run(() => waveOut.Play());
        }
        public void Resume()
        {
            Status = PlayStatusEnum.Playing;
            waveOut.Resume();
        }
        public void Pause()
        {
            Status = PlayStatusEnum.Pause;
            waveOut.Pause();
        }
        public void AddSong(Song song)
        {
            SongList.Add(song);
        }
        public async void AddSong(string url)
        {
            var song = await Song.TryCreateSongAsync(url);
            if (song is not null)
            {
                AddSong(song);
            }
        }
        private void AfterPlay(object? sender, StoppedEventArgs e)
        {
            Console.WriteLine("After peeyan.");
            if (IsRandom)
            {
                // do random
                return;
            }
            if (LoopMode != LoopModeEnum.LoopSingle)
                ListIndex++;
            if (ListIndex >= SongList.Count)
            {
                if (LoopMode != LoopModeEnum.LoopAll)
                {
                    Status = PlayStatusEnum.NotPlaying;
                    // tell front-end that need to render
                    return;
                }
                ListIndex = 0;
            }
            _ = Play(ListIndex);
        }
        public Song NowPlaying()
        {
            if (Status == PlayStatusEnum.Playing)
                return SongList[ListIndex];
            return new Song();
        }
    }
}
