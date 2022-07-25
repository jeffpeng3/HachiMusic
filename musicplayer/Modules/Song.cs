using System;
using System.Linq;
using YoutubeExplode;
using System.Threading.Tasks;
using YoutubeExplode.Videos.Streams;

namespace musicplayer.Modules
{
    public class Song
    {
        static readonly YoutubeClient youtube = new();
        public Uri AudioStream { get; }
        public string Title { get; }
        public string Artist { get; }
        public Uri Thumbnail { get; }
        public TimeSpan Duration { get; }
        public string VideoId { get; }

        public Song()
        {
            AudioStream = new Uri("about:blank");
            Title = string.Empty;
            Artist = string.Empty;
            Thumbnail = new Uri("about:blank");
            Duration = TimeSpan.Zero;
            VideoId = string.Empty;
        }
        public Song(Uri _url, string _title, string _artist, Uri _Thumbnails, TimeSpan _duration, string _videoId)
        {
            AudioStream = _url;
            Title = _title;
            Artist = _artist;
            Thumbnail = _Thumbnails;
            Duration = _duration;
            VideoId = _videoId;
        }
        public async static Task<Song?> TryCreateSongAsync(string url)
        {
            if (!Utils.IsValidAddress(url))
            {
                return null;
            }
            Uri? targetUrl;
            targetUrl = new Uri(url);
            var MusicMetadata = await youtube.Videos.GetAsync(url);
            var MusicManifest = await youtube.Videos.Streams.GetManifestAsync(url);

            var streamUrl = new Uri(MusicManifest.GetAudioOnlyStreams().GetWithHighestBitrate().Url);
            var title = MusicMetadata.Title;
            var artist = MusicMetadata.Author.ChannelTitle;
            var thumbnails = new Uri(MusicMetadata.Thumbnails.OrderBy(x => x.Resolution.Area).Last().Url);
            var duration = MusicMetadata.Duration ?? TimeSpan.Zero;
            var videoID = MusicMetadata.Id;
            return new Song(streamUrl, title, artist, thumbnails, duration, videoID);
        }
    }
}
