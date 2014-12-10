using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFIDF.Model;

namespace TFIDF.Service
{
    public class KeywordsService : TFIDF.Service.IKeywordsService
    {
        #region Fields

        private IStemmerService _stemmerService;

        #endregion

        #region Ctor

        public KeywordsService (IStemmerService stemmerService)
	    {
            _stemmerService = stemmerService;
	    }

        #endregion

        #region Methods

        public void GetData(string path, Action<List<Keyword>, Exception> callback)
        {
            List<Keyword> keywords = GetKeywords(path);
            callback(keywords, null);
        }

        private List<Keyword> GetKeywords(string path)
        {
            List<Keyword> keywords = new List<Keyword>();

            using (StreamReader sr = new StreamReader(path))
            {
                string word;
          
                while ((word = sr.ReadLine()) != null)
                {
                    keywords.Add( new Keyword(word, _stemmerService.StemWord(word)) );
                }
            }

            return keywords;
        }

        #endregion // Methods
    }
}
