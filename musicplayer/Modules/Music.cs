using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace musicplayer.Modules
{
    public class Music
    {
        static readonly YoutubeClient youtube = new();
        public Uri AudioStream { get; }
        public string Title { get; }
        public string Artist { get; }
        public Uri Thumbnail { get; }
        public TimeSpan Duration { get; }
        public string VideoId { get; }

        public Music(Uri _url, string _title, string _artist, Uri _Thumbnails, TimeSpan _duration)
        {
            VideoId = "";
            AudioStream = _url;
            Title = _title;
            Artist = _artist;
            Thumbnail = _Thumbnails;
            Duration = _duration;
        }

        public async static Task<Music> CreateMusicAsync(string url)
        {
            Uri? targetUrl;
            string reg = @"^((http[s]?):\/)?\/?([^:\/\s]+)((\/\w+)*\/)([\w\-\.]+[^#?\s]+)(.*)?(#[\w\-]+)?$";
            Regex r = new(reg);
            Match m = r.Match(url);
            if (!m.Success)
            {

            }
            targetUrl = new Uri(url);
            var MusicMetadata = await youtube.Videos.GetAsync(url);
            var MusicManifest = await youtube.Videos.Streams.GetManifestAsync(url);
            var StreamUrl = new Uri(MusicManifest.GetAudioOnlyStreams().GetWithHighestBitrate().Url);
            var title = MusicMetadata.Title;
            var artist = MusicMetadata.Author.ChannelTitle;
            var Thumbnails = new Uri(MusicMetadata.Thumbnails.OrderBy(x => x.Resolution.Area).Last().Url);
            var duration = MusicMetadata.Duration ?? TimeSpan.Zero;
            return new Music(StreamUrl, title, artist, Thumbnails, duration);
        }
    }
}
