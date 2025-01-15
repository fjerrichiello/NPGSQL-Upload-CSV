namespace NPGSQL.CSVWriter;

public class UploadEntry 
{
    public UploadEntry(string databaseName, string csvName, string tableName, List<int> indicesToRemove)
    {
        DatabaseName = databaseName;
        CsvName = csvName;
        TableName = tableName;
        IndicesToRemove = indicesToRemove;
        Writer = new StreamWriter(csvName);
    }

    public void SetHeaders(string header)
    {
        Headers = Helper.RemoveItemsByIndices(header, IndicesToRemove);
    }

    public string DatabaseName { get; set; }

    public string CsvName { get; set; }

    public string TableName { get; set; }

    public string Headers { get; set; }

    public List<int> IndicesToRemove { get; set; } = [];

    public StreamWriter Writer { get; set; }

    public void WriteLine(string line)
    {
        Writer.WriteLine(Helper.RemoveItemsByIndices(line, IndicesToRemove));
    }
}