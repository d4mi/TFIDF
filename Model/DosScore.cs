using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFIDF.Model
{
    // helper class to store a docId and its score

        public class DocScore : IComparable<DocScore>
        {
            public double score;
            public int docId;

            public DocScore(double score, int docId)
            {
                this.score = score;
                this.docId = docId;
            }

            public int CompareTo(DocScore docScore)
            {

                if (score > docScore.score) return -1;

                if (score < docScore.score) return 1;

                return 0;

            }
        }
}
