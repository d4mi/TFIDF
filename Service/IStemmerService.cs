using System;
namespace TFIDF.Service
{
    public interface IStemmerService
    {
        string StemText(string text);
        string StemWord(string word);
    }
}
