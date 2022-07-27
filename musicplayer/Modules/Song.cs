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
            var T1 = youtube.Videos.GetAsync(url);
            var T2 = youtube.Videos.Streams.GetManifestAsync(url);
            var MusicMetadata = await T1;
            var T3 = Utils.GetMaxResolutionAsync(MusicMetadata.Id);
            var title = MusicMetadata.Title;
            var artist = MusicMetadata.Author.ChannelTitle;
            var duration = MusicMetadata.Duration ?? TimeSpan.Zero;
            var videoID = MusicMetadata.Id;
            var thumbnails = await T3;
            var MusicManifest = await T2;
            var streamUrl = new Uri(MusicManifest.GetAudioOnlyStreams().GetWithHighestBitrate().Url);
            return new Song(streamUrl, title, artist, thumbnails, duration, videoID);
        }
    }
}
