using System;

namespace Recording
{
    [Serializable]
    public class DocumentWithId
    {
        public RoundResultWithId document;

        public override string ToString()
        {
            return document!.ToString();
        }
    }
}