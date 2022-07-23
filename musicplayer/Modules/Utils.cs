using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace musicplayer.Modules
{
    public class Utils
    {
        public static bool IsValidAddress(string target)
        {
            string reg = @"^((http[s]?):\/)?\/?([^:\/\s]+)((\/\w+)*\/)([\w\-\.]+[^#?\s]+)(.*)?(#[\w\-]+)?$";
            Regex r = new(reg);
            Match m = r.Match(target);
            return m.Success;
        }
    }
}
