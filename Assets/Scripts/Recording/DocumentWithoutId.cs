using System;

namespace Recording
{
    [Serializable]
    public class DocumentWithoutId
    {
        public RoundResultWithoutId document;

        public override string ToString()
        {
            return document!.ToString();
        }
    }
}