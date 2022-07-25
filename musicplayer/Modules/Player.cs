using System;
using musicplayer.Enums;
using System.Collections.ObjectModel;

namespace musicplayer.Modules
{
    public class Player
    {
        public static readonly ObservableCollection<Song> SongList = new();

        // List<int> ReapeatTimes;
        public static int ListIndex { get; private set; } = 0;
        public static double Volume { get; set; } = 0.5;
        public static bool isMute { get; set; } = false;
        public static TimeSpan Position { get; set; } = TimeSpan.Zero;
        public static PlayStatusEnum Status { get; set; } = PlayStatusEnum.NotPlaying;
        public static LoopModeEnum LoopMode { get; set; } = LoopModeEnum.LoopNone;
        public static Player? CurrentPlayer { get; set; } = null;
        public Player()
        {
            CurrentPlayer = this;
        }
        public void Play(int index = -1)
        {
            Status = PlayStatusEnum.Playing;

        }
        public void Resume()
        {
            Status = PlayStatusEnum.Playing;

        }
        public void Pause()
        {
            Status = PlayStatusEnum.Pause;

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
        public void AfterPlay()
        {

        }
        public Song NowPlaying()
        {
            if (Status == PlayStatusEnum.Playing)
                return SongList[ListIndex];
            return new Song();
        }
    }
}
