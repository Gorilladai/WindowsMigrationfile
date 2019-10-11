using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace GetFolderNames
{
    class Program
    {
        static string filename = "a.bat";
        static void Main(string[] args)
        {
            var currentdirectory = System.Environment.CurrentDirectory;
            Directory.SetCurrentDirectory(Directory.GetParent(currentdirectory).FullName);

            var dir = Directory.GetCurrentDirectory();


            if (File.Exists(Path.Combine(dir, filename)))
            {
                Console.WriteLine("File already exists. Please delete Y/N?");
                var key = Console.ReadKey();
                if (key.Key != ConsoleKey.Y)
                {
                    Console.WriteLine("Please press any key to finish.");
                    Console.ReadKey();
                    return;
                }
            }
            Console.WriteLine("Please enter a saved directory");
            var saveDir = Console.ReadLine();
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
            }





            DirectoryInfo directoryInfo = new DirectoryInfo(dir);
            var dirs = directoryInfo.GetDirectories();
            Array.Sort(dirs, new TestGetDir(true));//排序
            List<string> msgs = new List<string>();
            msgs.Add("@echo off");
            foreach (var item in dirs)
            {
                var copy = string.Format(@"move {0} {1}", item.FullName, saveDir);

                //mklink / J "e:\test" "f:\test"
                var mklink = string.Format(@"mklink /J {0} {1}", item.FullName, Path.Combine(saveDir, item.Name));
                msgs.Add(copy);
                msgs.Add(mklink);
                Console.WriteLine(item.FullName);
            }
            msgs.Add("echo finish");
            msgs.Add("pause");
            Write(msgs);
            Console.ReadKey();
        }
        static void Write(List<string> msgs)
        {



            if (!File.Exists(filename))
            {
                var a = File.Create(filename);
                a.Flush();
                a.Close();
            }

            using (StreamWriter f = new StreamWriter(filename, true))
            {
                foreach (var item in msgs)
                {
                    f.WriteLine(item);
                }
            }

        }


    }
    class TestGetDir : IComparer
    {
        bool m_bAsc = true;
        public TestGetDir(bool bAsc)
        {
            m_bAsc = bAsc;
        }
        public int Compare(object a, object b)
        {
            DirectoryInfo fia = (DirectoryInfo)a;
            DirectoryInfo fib = (DirectoryInfo)b;
            return (m_bAsc ? 1 : -1) * (fia.LastWriteTime - fib.LastWriteTime).Seconds;

        }
    }
}
