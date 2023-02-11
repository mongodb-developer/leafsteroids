using System;

namespace Recording
{
    [Serializable]
    public class PayloadWithDocument
    {
        public string dataSource;
        public string database;
        public string collection;
        public object Document;
    }
}