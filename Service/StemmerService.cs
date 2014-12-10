using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFIDF.ThirdParty.porter;

namespace TFIDF.Service
{
    public class StemmerService : IStemmerService
    {
        #region IStemmerService

        public string StemWord(string word)
        {
            Stemmer stemmer = new Stemmer();
            foreach (char character in word)
            {
                if (Char.IsLetter(character))
                {
                    stemmer.add(Char.ToLower(character));
                }
            }

            stemmer.stem();

            return stemmer.ToString();
        }

        public string StemText(string text)
        {
            StringBuilder stemmedText = new StringBuilder();

            string[] words = text.Split(' ');
            foreach (string word in words)
            {
                stemmedText.Append(this.StemWord(word));
                stemmedText.Append(" ");
            }

            return stemmedText.ToString();
        }

        #endregion // IStemmerService
    }
}
