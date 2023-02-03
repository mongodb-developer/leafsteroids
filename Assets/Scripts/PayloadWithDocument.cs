using System;

[Serializable]
public class PayloadWithDocument
{
    public string dataSource;
    public string database;
    public string collection;
    public RoundResultWithoutId document;
}