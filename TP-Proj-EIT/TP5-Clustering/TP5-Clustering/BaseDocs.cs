using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP4_FreqBayes
{
    class BaseDocs
    {
        public BaseDocs()
        {
            this.m_Docs = new List<Doc>();
            this.m_AllWords = new Dictionary<string, int>();
            this.m_Politicians = new List<string>();
        }

        public bool AddDoc(string contenu)
        {
            Doc t = new Doc(contenu);
            return this.AddDoc(t);
        }

        public bool AddDoc(Doc t)
        {
            if (this.m_Docs.Contains(t)) return false;

            this.m_Docs.Add(t);
            foreach (string w in t.FreqNbMots.Keys)
                if (!this.m_AllWords.ContainsKey(w))
                    this.m_AllWords.Add(w, this.m_AllWords.Keys.Count);

            if (!this.m_Politicians.Contains(t.Politician))
                this.m_Politicians.Add(t.Politician);
            return true;
        }

        public struct point 
        {
            public point(long size)
            {
                coordinates = new double[size];
            }
            public double[] coordinates;
            public point Clone() 
            { return new point() { coordinates = (double[])this.coordinates.Clone() }; }
        };

        Dictionary<Doc, point> positionsDocs; // position des docs
        point[] points; // positions des clusters
        public void InitBase(int nombreCluster)
        {
            // Init des points cluster
            points = new point[nombreCluster];
            positionsDocs = new Dictionary<Doc, point>();
            Random rng = new Random();
            foreach (Doc d in this.m_Docs)
            {
                point p = new point(this.m_AllWords.Count);
                foreach (KeyValuePair<string, double> k in d.FreqNbMots)
                    p.coordinates[this.m_AllWords[k.Key]] = k.Value;
                positionsDocs.Add(d, p);
            }

            for (int i = 0; i < nombreCluster; i++)
                points[i] = positionsDocs.Values.ToArray()[rng.Next(0, positionsDocs.Values.Count-1)].Clone();

            bool change = true;
            int countChange = 1;
            while (change)
            {
                Console.WriteLine("Change number " + countChange++);
                // Re-init des clusters
                this.m_Clusters = new List<Doc>[nombreCluster];
                for (int i = 0; i < nombreCluster; i++)
                    this.m_Clusters[i] = new List<Doc>();

                change = false;

                Console.WriteLine("\taffectations");
                #region Mise a jour des affectations aux classes
                foreach (Doc d in this.m_Docs)
                {
                    int indDistanceMin = nombreCluster;
                    double distanceMin = double.MaxValue;
                    for (int i = 0; i < nombreCluster; i++)
                    {
                        double distance = calcDistance(points[i], positionsDocs[d]);
                        if (distance < distanceMin)
                        {
                            indDistanceMin = i;
                            distanceMin = distance;
                        }
                    }
                    // on met le point le plus proche
                    if (d.Type != indDistanceMin.ToString())
                    {
                        d.Type = indDistanceMin.ToString();
                        change = true;
                    }
                    this.m_Clusters[indDistanceMin].Add(d);
                }
                #endregion

                Console.WriteLine("\tmoyennes");
                #region Mise a jour des moyennes
                // pour chaque point i
                for (int i = 0; i < nombreCluster; i++)
                {
                    Console.WriteLine("\t\tCluster num " + i + " (" + points[i].coordinates.LongLength + " coords)");
                    // pout chaque coordonnée coordI du point i
                    for (long coordI = 0; coordI < points[i].coordinates.LongLength; coordI++)
                    // pour chaque documents dans le cluster du point i
                    {
                        points[i].coordinates[coordI] = 0;
                        foreach (Doc d in this.m_Clusters[i])
                            // la coordonnée coordI du point i est 
                            //      Somme(cluster[i].docs.position[coordI]) / cluster[i].size
                            points[i].coordinates[coordI] += positionsDocs[d].coordinates[coordI];

                        points[i].coordinates[coordI] /= this.m_Clusters[i].Count;
                    }
                }
                #endregion

            } // end while change
        }

        public List<Doc>[] Clusters { get { return this.m_Clusters; } }
        private List<Doc>[] m_Clusters;
        private double calcDistance(point p1, point p2)
        {
            double somme = 0;
            for (long i = 0; i < p1.coordinates.LongLength; i++)
            /* Norme1
                somme += Math.Abs(p1.coordinates[i] - p2.coordinates[i]);
            // */ //* Norme2
                somme += Math.Pow(p1.coordinates[i] - p2.coordinates[i], 2);
            somme = Math.Sqrt(somme);
            // */
            return somme;
        }

        public List<Doc> getXDocsTypiqueClusterN(int x, int N)
        {
            if (N < 0 || N >= this.m_Clusters.Length) return null;

            List<Doc> docs = this.m_Clusters[N];

            Doc[] ordered = (from d in docs
                             orderby calcDistance(positionsDocs[d], points[N])
                             select d).ToArray();
            if (ordered.Length < x) return ordered.ToList();
            List<Doc> retour = new List<Doc>();
            for (int i = 0; i < x; i++) retour.Add(ordered[i]);
            return retour;
        }

        private List<string> m_Politicians;

        public Dictionary<string, double> freqDocNum(int i)
        {
            return this.m_Docs[i].FreqNbMots;
        }

        private List<Doc> m_Docs;
        private Dictionary<string, int> m_AllWords;


        
    }
}
