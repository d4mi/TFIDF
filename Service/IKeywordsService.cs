using System;
using System.Collections.Generic;
using TFIDF.Model;
namespace TFIDF.Service
{
    public interface IKeywordsService
    {
        void GetData(string path, Action<List<Keyword>, Exception> callback);
    }
}
