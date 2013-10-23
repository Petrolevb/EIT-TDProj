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
            StreamReader testStream 
                = new StreamReader(new FileStream("data.txt", FileMode.Open, FileAccess.Read));
            while (!testStream.EndOfStream)
                list.Add(testStream.ReadLine());
            testStream.Close();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Depart");
            List<string> listBaseTrainDocs = new List<string>();
            Random RNG = new Random();

            Console.WriteLine("Read all docs");
            readValues(listBaseTrainDocs);
            string[] docs = listBaseTrainDocs.ToArray();

            int nombreCluster = 10;
            int nombreTry = 50;
            double[] moys = new double[nombreCluster];
            //for (int i = 0; i < nombreTry; i++)
            //{
                //Console.WriteLine("Try " + (i + 1));
                Console.WriteLine("Add all docs in base");
                // default value : 80%
                BaseDocs baseDocs = new BaseDocs();
                foreach (string doc in docs)
                    baseDocs.AddDoc(doc);
                Console.WriteLine("Init clusters"); 
                baseDocs.InitBase(nombreCluster);

                Console.WriteLine("done");
            //    for (int a = 0; a < baseDocs.Clusters.Length; a++)
            //        moys[a] += baseDocs.Clusters[a].Count;
            //}
                int sum = 0;
                Console.WriteLine("");
            for(int i = 0; i < nombreCluster; i++)
            {
                Console.WriteLine("Nombre doc clust " + i + " : " + baseDocs.Clusters[i].Count);
                sum += baseDocs.Clusters[i].Count;
            }
            Console.WriteLine("Total " + sum);
            //Console.WriteLine("\n Moyennes : ");
            //for (int i = 0; i < nombreCluster; i++)
            //    Console.WriteLine("Cluster " + i + " moy : " + moys[i] / (double)nombreTry + " documents");
            
            Console.Read();

        }
    }
}
