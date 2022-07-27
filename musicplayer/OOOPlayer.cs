using Microsoft.VisualStudio.Threading;
using musicplayer.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace musicplayer.Modules
{
    public class OOOPlayer
    {
        readonly MediaPlayer player = new();
        readonly ObservableCollection<Song> queue = new();
        public int index;
        public bool Mute;
        public LoopModeEnum LoopMode;
        public double volume;
        public Song? nowPlaying;
        public PlayStatusEnum status;
        readonly AsyncQueue<MessageTopicEnum> MessageQueue;

        public OOOPlayer(AsyncQueue<MessageTopicEnum> _MessageQueue)
        {
            Mute = false;
            status = PlayStatusEnum.NotPlaying;
            MessageQueue = _MessageQueue;
            player.MediaEnded += AfterPlay;
            index = 1;
            LoopMode = LoopModeEnum.LoopNone;
            nowPlaying = null;
            volume = 0.25;
            _ = TickAsync();
        }

        private async Task TickAsync()
        {
            var Last = player.Position;
            while (true)
            {
                if (Last != player.Position)
                {
                    MessageQueue.Enqueue(MessageTopicEnum.TickUpdate);
                    Last = player.Position;
                }
                await Task.Delay(333);
            }
        }

        private void AfterPlay(object? sender, EventArgs e)
        {
            switch (LoopMode)
            {
                case LoopModeEnum.LoopNone:
                    if (nowPlaying != null)
                    {
                        index++;
                    }
                    break;
                case LoopModeEnum.LoopAll:
                    if (nowPlaying != null)
                    {
                        index = index == queue.Count ? 1 : index + 1;
                    }
                    break;
            }
            nowPlaying = null;
            status = PlayStatusEnum.NotPlaying;
            TryToPlayMusic(false);
            MessageQueue.Enqueue(MessageTopicEnum.PlayingUpdate);
            MessageQueue.Enqueue(MessageTopicEnum.StatusUpdate);
        }

        private void TryToPlayMusic(bool IsInternal = true)
        {
            if (nowPlaying == null)
            {
                if (index <= queue.Count)
                {
                    var song = queue[index - 1];
                    player.Open(song.AudioStream);
                    player.Play();
                    nowPlaying = song;
                    status = PlayStatusEnum.Playing;
                    if (IsInternal)
                    {
                        MessageQueue.Enqueue(MessageTopicEnum.PlayingUpdate);
                        MessageQueue.Enqueue(MessageTopicEnum.StatusUpdate);
                    }
                }
            }
        }

        public void AddQueue(Song music)
        {
            queue.Add(music);
            TryToPlayMusic();
            MessageQueue.Enqueue(MessageTopicEnum.QueueUpdate);
        }

        public void Volume(double? target, bool mute = false)
        {
            Mute = mute;
            if (Mute)
            {
                target = 0;
            }
            if (target is not null)
            {
                volume = (double)target;
                volume = volume * volume * volume;
                player.Volume = volume;
            }
            if (!MessageQueue.ToArray().Contains(MessageTopicEnum.VolumeChange))
                MessageQueue.Enqueue(MessageTopicEnum.VolumeChange);
        }

        public string ChangeLoopMode()
        {
            LoopMode = LoopMode switch
            {
                LoopModeEnum.LoopNone => LoopModeEnum.LoopSingle,
                LoopModeEnum.LoopSingle => LoopModeEnum.LoopAll,
                LoopModeEnum.LoopAll => LoopModeEnum.LoopNone,
                _ => LoopModeEnum.LoopNone,
            };
            return LoopMode switch
            {
                LoopModeEnum.LoopNone => "LoopNone",
                LoopModeEnum.LoopSingle => "LoopSingle",
                LoopModeEnum.LoopAll => "LoopAll",
                _ => "ERROR",
            };
        }

        public void Shuffle()
        {
            List<Song> temp;
            if (nowPlaying is not null)
                temp = queue.Except(new Song[] { nowPlaying }).ToList();
            else
                temp = queue.ToList();
            queue.Clear();
            foreach (var s in temp)
            {
                queue.Insert(Random.Shared.Next(queue.Count), s);
            }
            if (nowPlaying is not null)
                queue.Insert(0, nowPlaying);
            index = 1;
            MessageQueue.Enqueue(MessageTopicEnum.QueueUpdate);
        }

        public void Next()
        {
            if (nowPlaying is not null)
            {
                nowPlaying = null;
                player.Stop();
                index++;
                TryToPlayMusic();
            }
        }

        public void Back()
        {
            if (nowPlaying is not null)
            {
                index = index - 2 > 0 ? index - 2 : 0;
                nowPlaying = null;
                player.Stop();
                TryToPlayMusic();
            }

        }

        public void Pause()
        {
            if (nowPlaying is not null)
            {
                if (status == PlayStatusEnum.Playing)
                {
                    status = PlayStatusEnum.Pause;
                    player.Pause();
                }
                else
                {
                    status = PlayStatusEnum.Playing;
                    player.Play();
                }
            }
            else
            {
                index = 1;
                TryToPlayMusic();
            }
            MessageQueue.Enqueue(MessageTopicEnum.StatusUpdate);
        }

        public TimeSpan Position(TimeSpan? position = null)
        {
            if (position is not null)
            {
                player.Position = (TimeSpan)position;
            }
            return player.Position;
        }

        public ObservableCollection<Song> GetQueue()
        {
            return queue;
        }
    }
}


