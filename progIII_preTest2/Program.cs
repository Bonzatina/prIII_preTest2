using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Model;

namespace progIII_preTest2
{
    class MainClass
    {
        private static readonly object lockNames = new object();

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var source = new CancellationTokenSource();
            var token = source.Token;

            string url = "http://users.nik.uni-obuda.hu/prog3/_data/people.xml";

            Loader loader = new Loader(url);
            loader.LoadData();
            List<string> names = new List<string>();


            foreach (var item in loader.Data)
            {
                names.Add(item.Element("name").Value);
            }

            //ChangeFirstNames(ref names);
            //DisplayNames(ref names);
            //Console.WriteLine(names.Count);

            //Task.Run(() => ChangeFirstNames(ref names, token, source));
            Thread threadOne = new Thread(() => ChangeFirstNames(ref names, token, source));
            //Task.Run(() => DisplayNames(ref names));
            Thread threadTwo = new Thread(() => DisplayNames(ref names));

            threadOne.Start();
            threadTwo.Start();


            //task1.Start();
            //task2.Start();
            //Task<string> task = Task.Run(() => ChangeFirstName(item.Element("name").Value));

            // Process.Start("http://www.google.com");

            Console.ReadLine();

        }

        public static void ChangeFirstNames(ref List<string> names, CancellationToken tkn, CancellationTokenSource source)
        {
            Console.WriteLine("bef -------");
            Task.Delay(3000);

            lock (lockNames)
            {
                Monitor.Pulse(lockNames);
                for (var i = 0; i < names.Count(); i++)
                {
                    if (i == 80)
                    {
                        source.Cancel();
                    }
                    if (tkn.IsCancellationRequested)
                    {
                        return;
                    }
                    names[i] = names[i].Split(' ')[0];
                }
            }         
            Console.WriteLine("aft ---------");
        }

        public static void DisplayNames(ref List<string> names)
        {
            //Task.Delay(1000);
            Console.WriteLine("disp --------");
            lock (lockNames)
            {
                Monitor.Wait(lockNames);
                foreach (var name in names)
                {
                    Console.WriteLine(name);
                }
            }
      
        }
    }
}
