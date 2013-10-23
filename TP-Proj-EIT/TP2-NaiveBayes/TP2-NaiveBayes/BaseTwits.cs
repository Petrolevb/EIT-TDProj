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
            this.m_BetasPositive = new Dictionary<string, double>();
            this.m_BetasIrrelevant = new Dictionary<string, double>();
            this.m_BetasNegative = new Dictionary<string, double>();
            this.m_BetasNeutral = new Dictionary<string, double>();
            this.m_AllWords = new Dictionary<string, double>();
        }

        public bool AddTwit(string contenu)
        {
            Twit t = new Twit(contenu);
            return this.AddTwit(t);
        }

        public bool AddTwit(Twit t)
        {
            if (this.m_Twits.Contains(t)) return false;
            this.m_Twits.Add(t);
            return true;
        }

        public string checkTwit(string contenu)
        { return this.checkTwit(new Twit(contenu)); }

        public string checkTwit(Twit t)
        {
            if (this.m_Twits.Contains(t))
                return "skipped";
            double[] numberInCategories = 
            {
                this.GetByCategory("positive").ToArray().Length,
                this.GetByCategory("negative").ToArray().Length,
                this.GetByCategory("neutral").ToArray().Length,
                this.GetByCategory("irrelevant").ToArray().Length
            };
            double probaPositive = 0,
                   probaNegative = 0,
                   probaNeutral = 0,
                   probaIrrelevant = 0;

            foreach (string word in this.m_AllWords.Keys)
            {
                probaPositive += t.IsWordIn(word) ? 
                    Math.Log(this.m_BetasPositive[word]) : Math.Log(1 - this.m_BetasPositive[word]);
                probaNegative += t.IsWordIn(word) ?
                    Math.Log(this.m_BetasNegative[word]) : Math.Log(1-this.m_BetasNegative[word]);
                probaNeutral += t.IsWordIn(word) ?
                    Math.Log(this.m_BetasNeutral[word]) : Math.Log(1-this.m_BetasNeutral[word]);
                probaIrrelevant += t.IsWordIn(word) ?
                    Math.Log(this.m_BetasIrrelevant[word]) : Math.Log(1-this.m_BetasIrrelevant[word]);
            }

            if (probaPositive > probaNegative && probaPositive > probaNeutral && probaPositive > probaIrrelevant)
                return "positive and was " + t.Categorie + " : " + (t.Categorie.ToString().ToLower().Equals("positive")).ToString();

            if (probaNegative > probaNeutral && probaNegative > probaIrrelevant)
                return "negative and was " + t.Categorie + " : " + (t.Categorie.ToString().ToLower().Equals("negative")).ToString();

            if (probaIrrelevant > probaNeutral)
                return "irrelevant and was " + t.Categorie + " : " + (t.Categorie.ToString().ToLower().Equals("irrelevant")).ToString();

            return "neutral and was " + t.Categorie + " : " + (t.Categorie.ToString().ToLower().Equals("neutral")).ToString();
           
        }

        public void InitBase()
        {
            double[] numberInCategories = 
            {
                this.GetByCategory("positive").ToArray().Length,
                this.GetByCategory("negative").ToArray().Length,
                this.GetByCategory("neutral").ToArray().Length,
                this.GetByCategory("irrelevant").ToArray().Length
            };
            foreach (Twit t in this.m_Twits)
                foreach(string word in t.Words)
                {
                    if (!this.m_BetasPositive.ContainsKey(word))
                        this.m_BetasPositive.Add(word, 0);
                    if (!this.m_BetasNegative.ContainsKey(word)) 
                        this.m_BetasNegative.Add(word, 0);
                    if (!this.m_BetasNeutral.ContainsKey(word)) 
                        this.m_BetasNeutral.Add(word, 0);
                    if (!this.m_BetasIrrelevant.ContainsKey(word)) 
                        this.m_BetasIrrelevant.Add(word, 0);

                    switch (t.Categorie)
                    {
                        case "positive":  this.m_BetasPositive[word]++; break;
                        case "negative":   this.m_BetasNegative[word]++; break;
                        case "neutral":    this.m_BetasNeutral[word]++; break;
                        case "irrelevant": this.m_BetasIrrelevant[word]++; break;
                    }

                    if(!this.m_AllWords.ContainsKey(word)) this.m_AllWords.Add(word, 0);
                    this.m_AllWords[word] += t.NumberWordIn(word);
                }
            foreach (string key in this.m_AllWords.Keys)
            {
                if (this.m_BetasPositive.ContainsKey(key))
                    this.m_BetasPositive[key] = 
                        Math.Min(Math.Max(0.1,
                                          this.m_BetasPositive[key]), 
                                 numberInCategories[0]-0.1) 
                        / numberInCategories[0];
                double testP = this.m_BetasPositive[key];
                if (this.m_BetasNegative.ContainsKey(key))
                    this.m_BetasNegative[key] =
                        Math.Min(Math.Max(0.1,
                                          this.m_BetasNegative[key]),
                                 numberInCategories[1] - 0.1)
                        / numberInCategories[1];
                double testNg = this.m_BetasNegative[key];
                if (this.m_BetasNeutral.ContainsKey(key))
                    this.m_BetasNeutral[key] =
                        Math.Min(Math.Max(0.1,
                                          this.m_BetasNeutral[key]),
                                 numberInCategories[2] - 0.1)
                        / numberInCategories[2];
                double testNt = this.m_BetasNeutral[key];
                if (this.m_BetasIrrelevant.ContainsKey(key))
                    this.m_BetasIrrelevant[key] =
                        Math.Min(Math.Max(0.1,
                                          this.m_BetasIrrelevant[key]),
                                 numberInCategories[3] - 0.1)
                        / numberInCategories[3];
                double testI = this.m_BetasIrrelevant[key];
            }
        }

        private List<Twit> m_Twits;
        Dictionary<string, double> m_BetasPositive;
        Dictionary<string, double> m_BetasNegative;
        Dictionary<string, double> m_BetasNeutral;
        Dictionary<string, double> m_BetasIrrelevant;
        
        Dictionary<string, double> m_AllWords;
        /* *
         * Awd = 1 if string w appears in document d
         *          bool Awd = d.dictionary.contains(w)
         * nw : number of word in the document
         * 
         * Bw = Nombre d'appartition de w / Nombre de document total
         * 
         * P(d, (B1, ..., Bnw)) = MULT_{w=1 -> nw} ( Bw^Awd * (1-Bw)^(1-Awd))
         *  p(d, listB) = (\x -> for(int i = 0; i < nw; i++) x*= (A(w,d) ? B[w] : 1-B[w]) ) 1
         *              = (\x -> for(int i = 0; i < nw; i++) x+= A(w,d)*log(B[w]) + (1-A(w,d))*log(1-B[w]) ) 0
         *  
         * */

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
