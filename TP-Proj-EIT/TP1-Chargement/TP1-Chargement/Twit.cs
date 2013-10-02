using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP1_Chargement
{
    class Twit
    {
        public enum Category { Positive, Negative, Neutral, Irrelevant, Unknown };

        public Twit(string contenu)
        {
            string[] catType = contenu.Substring(0, contenu.IndexOf(')')).Split(',');
            this.m_Categorie = catType[0].Remove(0, 1);
            /* typeof(this.m_Categorie) == Enum:Category
            string cat = catType[0].Substring(1, 1).ToUpper();
            cat += catType[0].Substring(2);
            this.m_Categorie = (Category)Enum.Parse(typeof(Category), cat);
            // */
            this.m_Type = catType[1];
            this.m_Contenu = contenu.Remove(0, contenu.IndexOf(')')+1);
            this.m_NbMots = new Dictionary<string, int>();
            foreach (string mot in this.m_Contenu.Split(' '))
            {
                if (mot == null) continue;
                if (mot == "" || mot == "\n" ||mot == "\t") continue;
                if (!this.m_NbMots.ContainsKey(mot))
                    this.m_NbMots.Add(mot, 0);
                this.m_NbMots[mot]++;
            }
        }

        public override string ToString()
        {
            return this.m_Categorie + " - " + this.m_Type + " :: " + this.m_Contenu;
        }

        private Dictionary<String, int> m_NbMots;

        //private Category m_Categorie;
        //public Category Categorie { get { return this.m_Categorie; } }
        private string m_Categorie;
        public string Categorie { get { return this.m_Categorie; } }

        private string m_Type;
        public string Type { get { return this.m_Type; } }

        private string m_Contenu;
        public string Contenu { get { return this.m_Contenu; } }

    }
}
