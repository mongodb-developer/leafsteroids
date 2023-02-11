using System;

namespace ReplaySystem
{
    [Serializable]
    public class Payload
    {
        public string dataSource;
        public string database;
        public string collection;
        public Recording document;

        public override string ToString()
        {
            return $"\n{dataSource}\n{database}\n{collection}\n{document.ToString()}";
        }
    }
}