using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP1_Chargement
{
    class BaseTwits
    {
        public BaseTwits()
        {
            this.m_Twits = new List<Twit>();
        }

        public void AddTwit(string contenu)
        {
            Twit t = new Twit(contenu);
            this.AddTwit(t);
        }

        public void AddTwit(Twit t)
        {
            this.m_Twits.Add(t);
        }

        private List<Twit> m_Twits;

        //public List<Twit> GetByCategory(Twit.Category category)

        #region Get Categories
        public IEnumerable<Twit> GetByCategory(string category)
        {
            return from twit in this.m_Twits
                   where twit.Categorie == category
                   select twit;
        }
        public IEnumerable<Twit> GetAllByCategory()
        {
            return from twit in this.m_Twits
                   orderby twit.Categorie
                   select twit;
        }
        #endregion

        #region Get types
        public IEnumerable<Twit> GetByType(string type)
        {
            return from twit in this.m_Twits
                   where twit.Type == type
                   select twit;
        }
        public IEnumerable<Twit> GetAllByType()
        {
            return from twit in this.m_Twits
                   orderby twit.Type
                   select twit;
        }
        #endregion

        #region Get Categories By Type Frequencies
        public Dictionary<string, int> GetFreqCatByType(string type)
        {
            Dictionary<string, int> ret = new Dictionary<string, int>();
            Twit[] twits = this.GetByType(type).ToArray();
            foreach (Twit t in twits)
            {
                if (!ret.ContainsKey(t.Categorie))
                    ret.Add(t.Categorie, 0);
                ret[t.Categorie]++;
            }
            return ret;
        }
        #endregion
        
        #region Get Types By Category Frequencies
        public Dictionary<string, int> GetFreqTypeByCat(string category)
        {
            Dictionary<string, int> ret = new Dictionary<string, int>();
            Twit[] twits = this.GetByCategory(category).ToArray();
            foreach (Twit t in twits)
            {
                if (!ret.ContainsKey(t.Type))
                    ret.Add(t.Type, 0);
                ret[t.Type]++;
            }
            return ret;
        }
        #endregion

        public Dictionary<string, int> GetFreqAllCat()
        {
            Dictionary<string, int> ret = new Dictionary<string, int>();
            Twit[] twits = this.GetAllByCategory().ToArray();
            foreach (Twit t in twits)
            {
                if (!ret.ContainsKey(t.Categorie))
                    ret.Add(t.Categorie, 0);
                ret[t.Categorie]++;
            }
            return ret;
        }
        public Dictionary<string, int> GetFreqAllType()
        {
            Dictionary<string, int> ret = new Dictionary<string, int>();
            Twit[] twits = this.GetAllByType().ToArray();
            foreach (Twit t in twits)
            {
                if (!ret.ContainsKey(t.Type))
                    ret.Add(t.Type, 0);
                ret[t.Type]++;
            }
            return ret;
        }


        public string ToStringContenu()
        {
            string ret = "";
            foreach (Twit t in this.m_Twits)
                ret += t.ToString() + "\n";
            return ret;
        }
        
    }
}
