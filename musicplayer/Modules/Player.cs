using Microsoft.VisualStudio.Threading;
using musicplayer.Enums;
using Nito.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace musicplayer.Modules
{



    public class Player
    {
        readonly MediaPlayer player = new();
        readonly ObservableCollection<Music> queue = new();
        public int index;
        public bool Mute;
        public LoopModeEnum LoopMode;
        public double volume;
        public Music? nowPlaying;
        public PlayStatus status;
        readonly AsyncQueue<MessageTopic> MessageQueue;


        public Player(AsyncQueue<MessageTopic> _MessageQueue)
        {
            Mute = false;
            status = PlayStatus.NotPlaying;
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
                    MessageQueue.Enqueue(MessageTopic.TickUpdate);
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
            status = PlayStatus.NotPlaying;
            TryToPlayMusic(false);
            MessageQueue.Enqueue(MessageTopic.PlayingUpdate);
            MessageQueue.Enqueue(MessageTopic.StatusUpdate);
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
                    status = PlayStatus.Play;
                    if (IsInternal)
                    {
                        MessageQueue.Enqueue(MessageTopic.PlayingUpdate);
                        MessageQueue.Enqueue(MessageTopic.StatusUpdate);
                    }
                }
            }
        }

        public void AddQueue(Music music)
        {
            queue.Add(music);
            TryToPlayMusic();
            MessageQueue.Enqueue(MessageTopic.QueueUpdate);
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
            if (!MessageQueue.ToArray().Contains(MessageTopic.VolumeChange))
                MessageQueue.Enqueue(MessageTopic.VolumeChange);
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
            List<Music> temp;
            if (nowPlaying is not null)
                temp = queue.Except(new Music[] { nowPlaying }).ToList();
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
            MessageQueue.Enqueue(MessageTopic.QueueUpdate);
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
                if (status == PlayStatus.Play)
                {
                    status = PlayStatus.Pause;
                    player.Pause();
                }
                else
                {
                    status = PlayStatus.Play;
                    player.Play();
                }
            }
            else
            {
                index = 1;
                TryToPlayMusic();
            }
            MessageQueue.Enqueue(MessageTopic.StatusUpdate);
        }

        public TimeSpan Position(TimeSpan? position = null)
        {
            if (position is not null)
            {
                player.Position = (TimeSpan)position;
            }
            return player.Position;
        }

        public ObservableCollection<Music> GetQueue()
        {
            return queue;
        }
    }
}
