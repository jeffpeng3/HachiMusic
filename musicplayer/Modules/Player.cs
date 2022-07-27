using System;
using NAudio.Wave;
using musicplayer.Enums;
using NAudio.Wave.SampleProviders;
using System.Collections.ObjectModel;

namespace musicplayer.Modules
{
    public class Player
    {
        private static readonly WaveOut waveOut = new();
        public static readonly ObservableCollection<Song> SongList = new();

        // List<int> ReapeatTimes;
        public static int ListIndex { get; private set; } = 0;
        public static double Volume { get; set; } = 0.5;
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
        public void Play(int index = -1)
        {
            if (SongList.Count == 0)
                return;
            Status = PlayStatusEnum.Playing;
            index = index < 0 ? ListIndex : index;
            var currSong = SongList[index];

            WaveStream MFR = new MediaFoundationReader(currSong.AudioStream.ToString());
            VolumeSampleProvider VSP = new(MFR.ToSampleProvider());
            VSP.Volume = 0.05f;
            waveOut.Init(VSP);
            waveOut.Play();

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
        private void AfterPlay(object sender, StoppedEventArgs e)
        {
            if(IsRandom)
            {
                // do random
                return;
            }
            if (LoopMode != LoopModeEnum.LoopSingle)
                ListIndex++;
            if(ListIndex >= SongList.Count)
            {
                if (LoopMode == LoopModeEnum.LoopAll)
                    ListIndex = 0;
                else
                {
                    Status = PlayStatusEnum.NotPlaying;
                    // tell front-end that need to render
                    return;
                }
            }
            Play(ListIndex);
        }
        public Song NowPlaying()
        {
            if (Status == PlayStatusEnum.Playing)
                return SongList[ListIndex];
            return new Song();
        }
    }
}
