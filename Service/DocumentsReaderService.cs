using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFIDF.Model;

namespace TFIDF.Service
{
    public class DocumentsReaderService : IDocumentsReaderService
    {
        #region Fields

        private IStemmerService _stemmerService;

        #endregion 

        #region Ctor

        public DocumentsReaderService(IStemmerService stemmerService)
        {
            _stemmerService = stemmerService;
        }

        #endregion

        #region IDocumentsReaderService

        public void GetData(string path, Action<List<Document>, Exception> callback)
        {
            callback(ReadDocuments(path), null);
        }

        #endregion // IDocumentsReaderService

        #region Methods

        public List<Document> ReadDocuments(string path)
        {
            List<Document> documents = new List<Document>(); 
            StringBuilder document = new StringBuilder();

            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                bool first = true;
                string title = String.Empty;
                while ((line = sr.ReadLine()) != null)
                {

                    if (first)
                    {
                        title = line;
                        first = false;
                    }
                    if (line == "")
                    {
                        string baseText = document.ToString();
                        string stemmedText = _stemmerService.StemText(baseText);

                        Document newDocument = new Document(baseText, stemmedText);
                        newDocument.Title = title;

                        documents.Add(newDocument);

                        document.Clear();
                        first = true;
                    }
                    else
                    {
                        document.Append(line);
                    }
                                    }

                if (document.ToString() != String.Empty)
                {
                    string baseText = document.ToString();
                    string stemmedText = _stemmerService.StemText(baseText);

                    documents.Add(new Document(baseText, stemmedText));
                    document.Clear();
                }
            }

            return documents;
        }


        #endregion
    }
}
