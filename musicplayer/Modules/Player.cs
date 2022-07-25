using System;
using musicplayer.Enums;
using System.Collections.ObjectModel;

namespace musicplayer.Modules
{
    public class Player
    {
        public static readonly ObservableCollection<Song> SongList;
        // List<int> ReapeatTimes;
        public static int ListIndex { get; private set; }
        public static double Volume
        {
            get { return Volume; }
            set
            {

            }
        }
        public static bool isMute
        {
            get { return isMute; }
            set
            {

            }
        }
        public static TimeSpan Position
        {
            get { return Position; }
            set
            {

            }
        }
        public static PlayStatusEmun Status
        {
            get { return Status; }
            set
            {

            }
        }
        public static LoopModeEnum LoopMode
        {
            get { return LoopMode; }
            set
            {

            }
        }
        public static Player? CurrentPlayer { get; set; }
        static Player()
        {
            ListIndex = 0;
            Volume = 0.5;
            isMute = false;
            Position = TimeSpan.Zero;
            Status = PlayStatusEmun.NotPlaying;
            LoopMode = LoopModeEnum.LoopNone;
            SongList = new();
        }
        public Player()
        {
            CurrentPlayer = this;
        }
        public void Play(int index = -1)
        {

        }
        public void Resume()
        {

        }
        public void Pause()
        {

        }
        public void AddSong(string url)
        {

        }
        public void AfterPlay()
        {

        }
        public Song NowPlaying()
        {
            if (Status == PlayStatusEmun.Play)
                return SongList[ListIndex];
            return new Song();
        }
    }
}
