using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TFIDF.Model
{
    public class Document
    {
        private Dictionary<string, int> words = new Dictionary<string, int>();
        public SortedDictionary<string, double> TF { get; set; }
        public SortedDictionary<string, double> TFIDF { get; set; }

        public string Title { get; set; }
        public string Base { get; set; }
        public string Stemmed { get; set; }
        public double VectorLengthTF { get; set; }
        public double VectorLengthTFIDF { get; set; }
        public Dictionary<string, int> Words { get { return words; } }

        public Document(string baseText, string stemmedText)
        {
            Base = baseText;
            Stemmed = stemmedText;

            InitializeDictionary();

        }

        void InitializeDictionary()
        {
            string[] splittedText = Stemmed.Split(' ');
            foreach (string word in splittedText)
            {
                if (word != String.Empty)
                {
                    if (words.ContainsKey(word))
                    {
                        words[word]++;
                    }
                    else
                    {
                        words.Add(word, 1);
                    }
                }
            }
        }

        public void CalculateTF(List<Keyword> keywords)
        {
            TF = new SortedDictionary<string,double>();
            int maxCount = 0;

            foreach( Keyword keyword in keywords)
            {
                if( !TF.ContainsKey(keyword.Stemmed) )
                {
                    int count = 0; 
                    if( words.ContainsKey(keyword.Stemmed) )
                    {
                        count = words[keyword.Stemmed];
                    }

                    TF.Add(keyword.Stemmed, count);

                    if( maxCount < count )
                    {
                        maxCount = count;
                    }
                }
            }

            if (maxCount > 0)
            {
                foreach (string key in TF.Keys.ToList())
                {
                    TF[key] = TF[key] / (double) maxCount;
                }
            }

            CalculateVectorLength(keywords);
        }

        public void CalculateTFIDF(SortedDictionary<string, double> idf)
        {
            TFIDF = new SortedDictionary<string,double>();

            foreach( string key in TF.Keys )
            {
                TFIDF.Add(key, TF[key] * idf[key] );
            }

		    VectorLengthTFIDF = 0.0;
		    foreach (string key in TF.Keys)
            {
                VectorLengthTFIDF += Math.Pow(TFIDF[key], 2);
		    }

            VectorLengthTFIDF = Math.Sqrt(VectorLengthTFIDF);

        }

        private void CalculateVectorLength(List<Keyword> keywords)
        {
            VectorLengthTF = 0.0;
            HashSet<string> usedKeywords = new HashSet<string>();
            foreach( Keyword keyword in keywords)
            {
                if( usedKeywords.Add(keyword.Stemmed) )
                {
                    VectorLengthTF += Math.Pow( TF[keyword.Stemmed], 2);
                }
            }

            VectorLengthTF = Math.Sqrt(VectorLengthTF);
        }

        public bool Contains(string word)
        {
            return words.ContainsKey(word);
        }

        public double CalculateSimilarity(Document document)
        {
            double similarity = 0.0;

		    if (this.VectorLengthTFIDF == 0 || document.VectorLengthTFIDF == 0)
			    return similarity;

		    foreach (string key in document.Words.Keys)
            {
                if (TFIDF.ContainsKey(key))
                    similarity += TFIDF[key] * document.TFIDF[key];
            }

            similarity = similarity / (VectorLengthTFIDF * document.VectorLengthTFIDF);

            return similarity;
        }

    }
}
