using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFIDF.Service
{
    public class WordNetService : TFIDF.Service.IWordNetService
    {
        public WordNetService()
        {
            Wnlib.WNCommon.path = @"C:\dict\";
        }

        public List<string> GetSynonyms(string word)
        {
            List<string> synonyms = new List<string>();

            WnLexicon.WordInfo wordinfo = WnLexicon.Lexicon.FindWordInfo(word, true);

            if (wordinfo.partOfSpeech != Wnlib.PartsOfSpeech.Unknown)
            {
                synonyms = WnLexicon.Lexicon.FindSynonyms(word, wordinfo.partOfSpeech, true).ToList();
            }

            return synonyms;
        }

    }
}
