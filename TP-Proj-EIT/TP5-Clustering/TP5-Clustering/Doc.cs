using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP4_FreqBayes
{
    class Doc
    {
        public static string[] MotsOutils = { "le", "la", "les", 
                                              "un", "une", "a", "et",
                                               "de", "des", "du",
                                                "que", "à", "en",
                                            "nous", "il", "dans"};
        public Doc(string contenu)
        {
            string[] catType = contenu.Substring(0, contenu.IndexOf(')')).Split(',');
            this.m_Politician = catType[0].Remove(0, 1);

            this.m_Type = catType[1];
            this.m_Contenu = contenu.Remove(0, contenu.IndexOf(')')+1);
            this.m_FreqNbMots = new Dictionary<string, double>();

            string[] words = this.m_Contenu.Split(' ');
            this.m_SizeDocument = words.Length;
            
            foreach (string mot in words)
            {
                if (mot == null) continue;
                // Retrait des ponctuations
                string newMot = mot.Replace(",","").Replace(";","").Replace(":","")
                                    .Replace(".","").ToLower();
                if (mot == "" || mot == "\n" ||mot == "\t") continue;
                

                // retrait des mots outils
                if (MotsOutils.Contains(mot)) continue;


                if (!this.m_FreqNbMots.ContainsKey(newMot))
                    this.m_FreqNbMots.Add(newMot, 0);
                this.m_FreqNbMots[newMot]++;
            }
        }

        public override string ToString()
        { return this.m_Politician + " - " + this.m_Type + " :: " + this.m_Contenu; }

        private Dictionary<String, double> m_FreqNbMots;
        public Dictionary<String, double> FreqNbMots { get { return this.m_FreqNbMots; } }

        public bool IsWordIn(string word)
        { return this.m_FreqNbMots.ContainsKey(word); }
        public double FreqWordIn(string word)
        { return (this.m_FreqNbMots.ContainsKey(word) ? this.m_FreqNbMots[word] : 0.0); }

        private int m_SizeDocument;
        public int SizeDocument { get { return this.m_SizeDocument; } }


        public List<String> Words { get { return this.m_FreqNbMots.Keys.ToList(); } }


        private string m_Politician;
        public string Politician { get { return this.m_Politician; } }

        private string m_Type;
        public string Type { get { return this.m_Type; } set { this.m_Type = value; } }

        private string m_Contenu;
        public string Contenu { get { return this.m_Contenu; } }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            Doc comp = obj as Doc;
            if (comp == null) return false;
            return this.m_Contenu.Equals(comp.Contenu);
        }

    }
}
