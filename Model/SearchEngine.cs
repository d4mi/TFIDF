using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFIDF.Model
{
    public class SearchEngine
    {
        private List<Document> _documents = new List<Document>();
        private List<Keyword> _keywords = new List<Keyword>();
        public SortedDictionary<string, double> IDF { get; set; }
    
        public List<Document> Documents 
        {
            get
            {
                return _documents;
            }
            set
            {
                _documents = value;
                CalculateIDF();
                CalculateTF();
                CalculateTFIDF();
            }
        }

        public List<Keyword> Keywords 
        {
            get 
            { 
                return _keywords;
            }
            set 
            { 
                _keywords = value;
                CalculateIDF();
                CalculateTF();
                CalculateTFIDF();
            }
        }

        public SearchEngine()
        {

        }

        public SearchEngine(List<Document> documents, List<Keyword> keywords)
        {
            Documents = documents;
            Keywords = keywords;

            CalculateIDF();
        }

        public void CalculateIDF()
        {
            IDF = new SortedDictionary<string, double>();

            foreach (Keyword keyword in Keywords)
            {
                if (!IDF.ContainsKey(keyword.Stemmed))
                {
                    int count = 0;
                    foreach (Document document in Documents)
                    {
                        if (document.Contains(keyword.Stemmed))
                        {
                            count++;
                        }
                    }

                    if (count == 0)
                    {
                        IDF.Add(keyword.Stemmed, 0.0);
                    }
                    else
                    {
                        IDF.Add(keyword.Stemmed, Math.Log10(Documents.Count / (double)count));
                    }
                }
            }
        }

        public void CalculateTF()
        {
            foreach (Document document in Documents)
            {
                document.CalculateTF(Keywords);
            }
        }

        public void CalculateTFIDF()
        {
            foreach (Document document in Documents)
            {
                document.CalculateTFIDF(IDF);
            }
        }

        public bool Search(Document document)
        {


            return true;
        }

        public Dictionary<Document, double> CalculateSimilarity(Document document)
        {
            Dictionary<Document, double> similarity = new Dictionary<Document, double>();

            document.CalculateTF(Keywords);
            document.CalculateTFIDF(IDF);

            foreach (Document doc in Documents)
            {
                similarity.Add(doc, doc.CalculateSimilarity(document));
            }

            return similarity;
        }
    }
}
