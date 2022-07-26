using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows;
using System.Globalization;
using musicplayer.Controls;
using System.Collections.ObjectModel;

namespace musicplayer.Modules
{
    public class Song2MusicViewConverter
    {
        public static async Task<ObservableCollection<MusicView?>?> ConvertAsync(object value)
        {
            ObservableCollection<MusicView?> convertible = new();

            if (value is not ObservableCollection<Song> result)
            {
                return null;
            }
            List<Task<MusicView?>> TaskList = new();
            foreach (var item in result)
            {
                TaskList.Add(MusicView.TryToCreateMusicViewAsync($"https://www.youtube.com/watch?v={item.VideoId}"));
            }
            await Task.WhenAny(TaskList.ToArray());
            foreach (var item in TaskList)
            {
                convertible.Add(await item.ConfigureAwait(false));
            }
            return convertible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    public static class Extensions
    {
        public static T Next<T>(this T src) where T : Enum
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }
    }
    public class Utils
    {
        static readonly HttpClient client = new();

        public static bool IsValidAddress(string target)
        {
            string reg = @"^((http[s]?):\/)?\/?([^:\/\s]+)((\/\w+)*\/)([\w\-\.]+[^#?\s]+)(.*)?(#[\w\-]+)?$";
            Regex r = new(reg);
            Match m = r.Match(target);
            return m.Success;
        }

        public async static Task<Uri> GetMaxResolutionAsync(string? VideoId)
        {
            List<string> list = new List<string>()
            {
                "maxresdefault.jpg",
                "hqdefault.jpg",
                "sddefault.jpg",
                "mqdefault.jpg",
                "default.jpg"
            };

            foreach (var item in list)
            {
                var s = $"https://img.youtube.com/vi/{VideoId}/{item}";
                var respond = await client.GetAsync(s);
                if (respond.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new Uri(s);
                }
            }
            return new Uri($"https://img.youtube.com/vi/{VideoId}/0.jpg");
        }
    }
}
