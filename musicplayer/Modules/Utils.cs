using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace musicplayer.Modules
{
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

        public async static Task<Uri> GetMaxResolutionAsync(string VideoId)
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
