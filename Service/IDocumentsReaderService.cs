using System;
using System.Collections.Generic;
using TFIDF.Model;
namespace TFIDF.Service
{
    public interface IDocumentsReaderService
    {
        void GetData(string path, Action<List<Document>, Exception> callback);
    }
}
