using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace TP4_FreqBayes
{
    class Program
    {
        static void readValues(List<string> list)
        {
            StreamReader testStream, trainStream;
            testStream = new StreamReader(new FileStream("twitter/test.txt", FileMode.Open, FileAccess.Read));
            trainStream = new StreamReader(new FileStream("twitter/train.txt", FileMode.Open, FileAccess.Read));
            while (!trainStream.EndOfStream)
                list.Add(trainStream.ReadLine());
            trainStream.Close();
            testStream.Close();
        }
        static void Main(string[] args)
        {
            string prompt = "";
            while (prompt != "QUIT")
            {
                #region Init Cross Validation de K, trouvé à 0.0017
                /* Cross Validation de K
                Dictionary<double, List<double>> resultK = new Dictionary<double, List<double>>();
                Dictionary<double, double> moyenneK = new Dictionary<double, double>(),
                                           ecartTypeK = new Dictionary<double, double>();
                
                for(int a = 450;  a < 550; a++)
                {
                    double exp = - a/100,
                           k = (10 + (a/10 - (a/100)*10)) * Math.Pow(10, exp);
                    // */ double k = 0.0017;
                #endregion

                BaseTwits baseTwits = new BaseTwits();
                List<string> listBaseTrainTwits = new List<string>();
                Random RNG = new Random();

                readValues(listBaseTrainTwits);
                string[] twits = listBaseTrainTwits.ToArray();

                Console.Write("Taille corpus de test (" + twits.Length + " twits) (default : " + twits.Length*80/100 + ") (QUIT) : ");
                int taille;
                string sTaille = Console.ReadLine();
                if (sTaille.Equals("QUIT")) { prompt = "QUIT";  continue; } // will properly exit

                // default value : 80%
                if(sTaille.Equals("")) sTaille = ((int)twits.Length*80/100).ToString();
                if (!Int32.TryParse(sTaille, out taille))
                { Console.WriteLine("Not a Number"); continue; }

                Console.WriteLine("Press enter to run all test once, or a number to have average result");
                prompt = Console.ReadLine();

                int nombreTest;
                if (!Int32.TryParse(prompt, out nombreTest)) nombreTest = 1;
                List<Double> results = new List<double>();
                while (nombreTest-- != 0)
                {
                    baseTwits = new BaseTwits();
                    for (int i = 0; i < taille; i++)
                    {
                        int selectedNumb = RNG.Next(0, twits.Length-1-i);
                        string selected = twits[selectedNumb];
                        // swap with last
                        twits[selectedNumb] = twits[twits.Length-1-i];
                        twits[twits.Length-1-i] = selected;
                        
                        baseTwits.AddTwit(selected);
                    }
                    baseTwits.InitBase(k);

                    int trueResult = 0, falseResult = 0;
                    foreach (string t in twits)
                    {
                        string result = baseTwits.checkTwit(t);
                        if (result.EndsWith("True")) trueResult++;
                        else if (result.EndsWith("False")) falseResult++;
                    }
                    Console.WriteLine("True result : " + trueResult + " ~ " + (double)trueResult / (double)(trueResult + falseResult));
                    results.Add((double)trueResult / (double)(trueResult + falseResult));
                    /* Cross Validation de K
                    if (!resultK.ContainsKey(k)) resultK.Add(k, new List<Double>());
                    resultK[k].Add((double)trueResult/(double)(trueResult+falseResult));
                    // */
                    Console.WriteLine("False result : " + falseResult + " ~ " + (double)falseResult / (double)(trueResult + falseResult));
                    twits = listBaseTrainTwits.ToArray();
                    
                }
                double av = 0; 
                    foreach (double var in results) av += var; 
                    av /= results.Count;
                double ecType = 0; 
                    foreach (double var in results) ecType += Math.Pow(var - av, 2);
                    ecType /= results.Count; 
                    ecType = Math.Sqrt(ecType);

                Console.WriteLine("Average : " + av + "\t" + "+/- " + ecType);
                #region Cross Validation de K
                /*
                }
                // Calc Moy & Ecart Type
                foreach (KeyValuePair<double, List<double>> results in resultK)
                {
                    // Calc Moy
                    if (!moyenneK.ContainsKey(results.Key)) moyenneK.Add(results.Key, 0);
                    foreach (double res in results.Value)
                        moyenneK[results.Key] += res / results.Value.Count;

                    // Calc Ecart Type
                    if (!ecartTypeK.ContainsKey(results.Key)) ecartTypeK.Add(results.Key, 0);
                    foreach (double res in results.Value)
                        ecartTypeK[results.Key] += Math.Pow(res - moyenneK[results.Key], 2) / results.Value.Count;
                    ecartTypeK[results.Key] = Math.Sqrt(ecartTypeK[results.Key]);
                }
                // Presentation Resultats
                // Retenu : k = 0.0017
                foreach (KeyValuePair<double, double> ks in moyenneK)
                    Console.WriteLine("K : " + ks.Key + "\t\t" + ks.Value + " (+/- " + ecartTypeK[ks.Key] + ")");
                prompt = "QUIT";
                // */
                #endregion
            } // fin while prompt != "QUIT"
            Console.WriteLine("Exited..."); Console.CursorVisible = true; Console.Read();
        }
    }
}
