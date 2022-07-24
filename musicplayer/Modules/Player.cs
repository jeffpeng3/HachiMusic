using musicplayer.Enums;
using System;
using System.Collections.ObjectModel;

namespace musicplayer.Modules
{
    public class Player
    {
        readonly ObservableCollection<Song> SongList = new();
        // List<int> ReapeatTimes;
        int ListIndex;
        double Volume
        {
            get { return Volume; }
            set
            {

            }
        }

        static Player CurrentPlayer
        {
            get { return CurrentPlayer; }
            set { CurrentPlayer = value; }
        }
        bool isMute
        {
            get { return isMute; }
            set
            {

            }
        }
        TimeSpan Position
        {
            get { return Position; }
            set
            {

            }
        }
        PlayStatusEmun Status
        {
            get { return Status; }
            set
            {

            }
        }
        LoopModeEnum LoopMode
        {
            get { return LoopMode; }
            set
            {

            }
        }
        public Player()
        {
            ListIndex = 0;
            Volume = 0.5;
            isMute = false;
            Position = TimeSpan.Zero;
            Status = PlayStatusEmun.NotPlaying;
            LoopMode = LoopModeEnum.LoopNone;
            CurrentPlayer = this;
        }
        public ObservableCollection<Song> GetSongs() => SongList;
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
