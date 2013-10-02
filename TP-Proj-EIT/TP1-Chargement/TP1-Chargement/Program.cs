using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace TP1_Chargement
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream test = new FileStream("twitter/test.txt", FileMode.Open, FileAccess.Read),
                       train = new FileStream("twitter/train.txt", FileMode.Open, FileAccess.Read);
            BaseTwits baseTwits = new BaseTwits();

            using (StreamReader sr = new StreamReader(train))
            {
                while (!sr.EndOfStream)
                    baseTwits.AddTwit(sr.ReadLine());
            }
            //Console.WriteLine(baseTwits.ToStringContenu());
            string query = "";
            do
            {
                Console.Write("([freq] (cat | type) name | QUIT )>");
                query = Console.ReadLine();
                if (query.Equals("QUIT")) break;
                #region Frequency request
                if (query.StartsWith("freq"))
                {
                    Dictionary<string, int> twits;
                    query = query.Remove(0, "freq ".Length);
                    if (query.StartsWith("type"))
                    {
                        if (query == "type")
                            twits = baseTwits.GetFreqAllType();
                        else
                        {
                            query = query.Remove(0, "type ".Length);
                            twits = baseTwits.GetFreqCatByType(query);
                        }
                    }
                    else if (query.StartsWith("cat"))
                    {
                        if (query == "cat")
                            twits = baseTwits.GetFreqAllCat();
                        else
                        {
                            query = query.Remove(0, "cat ".Length);
                            twits = baseTwits.GetFreqTypeByCat(query);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Unknown request... [freq] (cat | type) name");
                        continue;
                    }
                    foreach (KeyValuePair<string, int> k in twits)
                        Console.WriteLine(k.Key + " : " + k.Value);
                }
                #endregion
                else
                {
                    Twit[] twits;
                    if (query.StartsWith("type"))
                    {
                        query = query.Remove(0, "type ".Length);
                        twits = baseTwits.GetByType(query).ToArray();
                    }
                    else if (query.StartsWith("cat"))
                    {
                        query = query.Remove(0, "cat ".Length);
                        twits = baseTwits.GetByCategory(query).ToArray();
                    }
                    else
                    {
                        Console.WriteLine("Unknown request... [freq] (cat | type) name");
                        continue;
                    }
                    foreach (Twit t in twits)
                        Console.WriteLine(t.ToString());
                }
            } while (!query.Equals("QUIT"));
            /*
            Twit[] twitsGoogle = baseTwits.GetByType("google").ToArray();
            foreach (Twit t in twitsGoogle)
                Console.WriteLine(t.ToString());
            Console.ReadLine();
            // * */
        }
    }
}
