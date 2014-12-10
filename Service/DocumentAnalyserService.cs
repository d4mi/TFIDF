using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFIDF.Model;

namespace TFIDF.Service
{
    public class DocumentAnalyserService
    {
        /*
        #region Fields

        private List<string> documents;
        private SortedDictionary<string, double> idfs;
        private SortedDictionary<string, SortedSet<int>> invertedFile;
        private List<SortedDictionary<string, double>> tf;

        private IDocumentsReaderService documentReaderService;

        #endregion // Fields
        
        #region Ctor

        public DocumentAnalyserService(IDocumentsReaderService documentReaderService)
        {
            this.documentReaderService = documentReaderService;            
            Initialize();
        }

        private void Initialize()
        {
            documents = documentReaderService.Read(Config.DB_FILE);
            idfs = new SortedDictionary<string, double>();
            invertedFile = new SortedDictionary<string, SortedSet<int>>();
            tf = new List<SortedDictionary<string, double>>();

            init();            
            PrintDocuments();  

            // idfs and tfs
            Console.WriteLine("IDFs:");
            // print the vocabulary
            PrintVoc();
                            
            Console.WriteLine("\nTFs for Equations:");
            for (int i = 0; i < documents.Count; i++)
            {
                Console.WriteLine("Equations: doc " + i + " : " + GetTF("Equations", i));
            }
            // similarities for different queries
            rank("Differential Equations");

            foreach (string document in documents)
            {

            }
        }                 #endregion // Ctor

        #region Method

        // lists the vocabulary
        private void PrintVoc()        {
            Console.WriteLine("Vocabulary:");
            foreach (KeyValuePair<string, Double> entry in idfs)            {
                Console.WriteLine(entry.Key + ", idf = " + entry.Value);
            }
        }
        // lists the database
        private void PrintDocuments()        {
            Console.WriteLine("size of the database: " + documents.Count);
            for (int i = 0; i < documents.Count; i++)
            {
                Console.WriteLine("doc " + i + ": " + documents.ElementAt(i));
            }
            Console.WriteLine("");
        }
        // calculates the similarity between two vectors
        // each vector is a term -> weight map
        private double Similarity(SortedDictionary<String, Double> v1, SortedDictionary<String, Double> v2)        {
            double sum = 0;
            // iterate through one vector
            foreach (KeyValuePair<string, double> entry in v1)            {
                string term = entry.Key;
                double w1 = entry.Value;
                // multiply weights if contained in second vector        
                if( v2.ContainsKey(term) )                {
                    double w2 = v2[term];
                    sum += w1 * w2;                }
            }
            // TODO write the formula for computation of cosinus
            // note that v.values() is Collection<Double> that you may need to calculate length of the vector
            // take advantage of vecLength() function
            return 1;
        }        
        // returns the length of a vector
        private double VecLength(List<double> vec)        {
            double sum = 0;
            foreach (double d in vec)             {
                sum += Math.Pow(d, 2);
            }
            return Math.Sqrt(sum);
        }
        // ranks a query to the documents of the database
        private void rank(String query)        {
            Console.WriteLine("");
            Console.WriteLine("query = " + query);

            // get term frequencies for the query terms
            SortedDictionary<string, double> termFreqs = GetTF(query);
            // construct the query vector
            // the query vector
            SortedDictionary<string, double> queryVec = new SortedDictionary<string, double>();
            // iterate through all query terms
            foreach (KeyValuePair<string, double> entry in termFreqs)            {
                String term = entry.Key;

                //TODO compute tfidf value for terms of query
                double tfidf = 0;
                queryVec.Add(term, tfidf);
            }
            SortedSet<int> union;
            SortedSet<string> queryTerms = new SortedSet<string>(termFreqs.Keys);
            // from the inverted file get the union of all docIDs that contain any query term
            union = invertedFile[queryTerms.First()];
            foreach (string term in queryTerms)            {
                foreach (int i in invertedFile[term])
                {
                    union.Add(i);
                }
            }
            // calculate the scores of documents in the union
            List<DocScore> scores = new List<DocScore>();
            foreach (int i in union)            {
                scores.Add(new DocScore(Similarity(queryVec, getDocVec(i)), i));
            }            
            // sort and print the scores
            scores.Sort();
            foreach (DocScore docScore in scores)            {
                Console.WriteLine("score of doc " + docScore.docId + " = " + docScore.score);
            }
        }
        // returns the idf of a term
        private double Idf(string term)        {
            return idfs[term];
        }
        // calculates the document vector for a given docID
        private SortedDictionary<String, Double> getDocVec(int docId)        {
            SortedDictionary<string, double> vec = new SortedDictionary<String, Double>();
            // get all term frequencies
            SortedDictionary<String, Double> termFreqs = tf.ElementAt(docId);
            // for each term, tf * idf
            foreach(KeyValuePair<String, Double> entry in termFreqs)            {
                String term = entry.Key;
                //TODO compute tfidf value for a given term
                //take advantage of idf() function
                double tfidf = 0;
                vec.Add(term, tfidf);
            }
            return vec;
        }        
        // returns the term frequency for a term and a docID
        private double GetTF(String term, int docId)        {
            double freq = 0;
            if (tf.ElementAt(docId).ContainsKey(term))
            {
                freq = tf.ElementAt(docId)[term];
            }
            return freq;
        }
    
        // calculates the term frequencies for a document
        private SortedDictionary<string, double> GetTF(String doc)        {           SortedDictionary<string, double> termFreqs = new SortedDictionary<string, double>();
           double max = 0;

           // tokenize document           string[] tokens = doc.Split(' ');
           // for all tokens
            foreach( string term in tokens)            {                // count the max term frequency
                double count = 0;
                if (termFreqs.ContainsKey(term))
                {
                    count = termFreqs[term];
                }
                count++;
                termFreqs.Add(term, count);
                if (count > max)                {                    max = count;                }
            }        
            // normalize tf
            foreach (double tf in termFreqs.Values )            {
        	    //TODO write the formula for normalization of TF
        	    //tf = 0.0;
            }
            return termFreqs;
        }        
        // init tf, invertedFile, and idfs
        private void init()        {
            int docId = 0;
            // for all docs in the database
            foreach (string doc in documents)            {
                // get the tfs for a doc
                SortedDictionary<string, double> termFreqs = GetTF(doc);

                // add to global tf vector
                tf.Add(termFreqs);
                // for all terms
                foreach (string term in termFreqs.Keys)                {
                    // add the current docID to the posting list
                    SortedSet<int> docIds = new SortedSet<int>();
                    if (invertedFile.ContainsKey(term))
                    {
                        docIds = invertedFile[term];
                    }
                    docIds.Add(docId);
                    invertedFile.Add(term, docIds);
                }
                docId++;               }        
            // calculate idfs
            int dbSize = documents.Count;
            // for all terms
            foreach (KeyValuePair<String, SortedSet<int>> entry in invertedFile)            {
                String term = entry.Key;
                // get the size of the posting list, i.e. the document frequency
                int df = entry.Value.Count;
                //TODO write the formula for calculation of IDF 
                idfs.Add(term, 0.0);
            }
        }

        #endregion
        */
    }
}
