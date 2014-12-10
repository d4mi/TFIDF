using System;
namespace TFIDF.Service
{
    public interface IWordNetService
    {
        System.Collections.Generic.List<string> GetSynonyms(string word);
    }
}
