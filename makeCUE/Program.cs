using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace makeCUE
{
    class Program
    {
        static string getTitle(string titleWithNum)
        {
            string s = Path.GetFileNameWithoutExtension(titleWithNum);
            if (s.IndexOf(' ') != -1)
            {
                return s.Substring(s.IndexOf(' ') + 1);
            }
            return null;
        }

        static void process(string[] args)
        {
            //双引号改为单引号
            for (int i = 0; i != args.Length; ++i)
            {
                args[i]=args[i].Replace('\"', '\'');
            }

            //排序
            for (int i = 0; i != args.Length - 1; ++i)
                for (int j = i + 1; j != args.Length; ++j)
                    if (args[i].CompareTo(args[j]) > 0)
                    {
                        string s = args[i];
                        args[i] = args[j];
                        args[j] = s;
                    }
        }

        static void Main(string[] args)
        {
            if (args.Length == 0) return;

            process(args);
            
            string path=Path.GetDirectoryName(args[0]);

            Console.Write("Album artist: ");
            string artist = Console.ReadLine();

            Console.Write("Title: ");
            string title = Console.ReadLine();

            string cueName = path + "\\" + artist + " - " + title + ".cue";

            if (!File.Exists(cueName))
            {
                using (StreamWriter sw=new StreamWriter(File.Open(cueName, FileMode.Create), Encoding.UTF8))
                {
                    Console.Write("Year: ");
                    string year = Console.ReadLine();

                    sw.WriteLine("REM DATE " + year);
                    sw.WriteLine("PERFORMER \"" + artist + "\"");
                    sw.WriteLine("TITLE \"" + title + "\"");

                    int cnt = 1;
                    foreach(string arg in args)
                    {
                        sw.WriteLine("FILE \""+Path.GetFileName(arg)+"\" WAVE");
                        sw.WriteLine(" TRACK " + ((cnt < 9) ? ("0" + cnt.ToString()) : cnt.ToString()) + " AUDIO");
                        sw.WriteLine("  TITLE \"" + getTitle(arg) + "\"");
                        sw.WriteLine("  PERFORMER \"" + artist + "\"");
                        sw.WriteLine("  INDEX 01 00:00:00");
                        ++cnt;
                    }

                    Console.WriteLine(cueName + " written!");
                }
            }
            else
            {
                Console.WriteLine("CUE ALREADY EXISTED!");
            }

            Console.ReadLine();
        }
    }
}
