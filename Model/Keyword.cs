using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFIDF.Model
{
    public class Keyword
    {
        public Keyword(string baseWord, string stemmedWord)
        {
            Base = baseWord;
            Stemmed = stemmedWord;
        }

        public string Base { get; set; }
        public string Stemmed { get; set; }
    }
}
