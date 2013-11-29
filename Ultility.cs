using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Xna.Framework.Media;

namespace XmasRingtones
{
    public static class Ultility
    {
        public static List<Ringtone> GetAllRingtone()
        {
            var list = new List<Ringtone>();
            var resource = Application.GetResourceStream(new Uri("List.txt", UriKind.Relative));

            // Read the file and display it line by line.
            using (var file = new StreamReader(resource.Stream))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] split = Regex.Split(line, ":");
                    var ringtone = new Ringtone(split[1], split[0]);
                    list.Add(ringtone);
                }
            }
            return list;
        }
    }

    public class Ringtone
    {
        public string Title { get; set; }
        public string Source { get; set; }
        public string Icon { get; set; }
        public Ringtone(string title, string source)
        {
            Title = title;
            Source = "/Ringtones/" + source + ".mp3";
            Icon = "/Icon/" + source + ".png";
        }
    }
}
