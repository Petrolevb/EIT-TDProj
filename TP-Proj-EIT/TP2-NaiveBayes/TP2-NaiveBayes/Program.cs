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
            List<string> twits = new List<string>();
            
            using (StreamReader sr = new StreamReader(train))
            {
                while (!sr.EndOfStream)
                    twits.Add(sr.ReadLine()); 
            }
            Random RNG = new Random();

            baseTwits = new BaseTwits();
            Console.Write("Taille corpus de test (QUIT) : ");
            int taille;
            string sTaille = Console.ReadLine();
            if (sTaille.Equals("QUIT")) return;
            if (!Int32.TryParse(sTaille, out taille))
            { Console.WriteLine("Not a Number"); Console.Read(); return; }

            for (int i = 0; i < taille; i++)
            {
                string selected = twits[RNG.Next(0, twits.Count - 1)];
                twits.Remove(selected);
                baseTwits.AddTwit(selected);
            }
            baseTwits.InitBase();
            string prompt = "";
            while (prompt != "QUIT")
            {
                Console.WriteLine("Press enter to continue, ALL to run all tests or QUIT to exit");
                prompt = Console.ReadLine();
                if (prompt.Equals("ALL"))
                {
                    int trueResult = 0, falseResult = 0;
                    foreach (string t in twits)
                    {
                        string result = baseTwits.checkTwit(t);
                        if (result.EndsWith("True")) trueResult++;
                        else if(result.EndsWith("False")) falseResult++;
                    }
                    Console.WriteLine("True result : " + trueResult);
                    Console.WriteLine("False result : " + falseResult);
                    baseTwits = new BaseTwits();
                    Console.Write("Taille corpus de test (QUIT) : ");
                    sTaille = Console.ReadLine();
                    if (sTaille.Equals("QUIT")) return;
                    if (!Int32.TryParse(sTaille, out taille))
                    { Console.WriteLine("Not a Number"); Console.Read(); break; }
                    for (int i = 0; i < taille; i++)
                        baseTwits.AddTwit(twits[RNG.Next(0, twits.Count - 1)]);
                    baseTwits.InitBase();
                }
                else Console.WriteLine(baseTwits.checkTwit(twits[RNG.Next(0, twits.Count - 1)]));
            }
            
        }
    }
}
