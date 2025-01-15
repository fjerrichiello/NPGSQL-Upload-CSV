using Npgsql;

namespace NPGSQL.CSVWriter;

public class UploadEntry2
{
    public UploadEntry2(string databaseName, string csvName, string tableName, List<int> indicesToRemove)
    {
        DatabaseName = databaseName;
        CsvName = csvName;
        TableName = tableName;
        IndicesToRemove = indicesToRemove;
        Connection = new NpgsqlConnection(string.Format(Helper.ConnectionString, databaseName));
    }

    public void OpenConnection()
    {
        Connection.Open();
    }

    public void CloseConnection()
    {
        Connection.Close();
    }

    public void SetHeaders(string header)
    {
        Headers = Helper.RemoveItemsByIndices(header, IndicesToRemove);
    }

    public async Task CreateWriter()
    {
        Writer = await Connection.BeginTextImportAsync(Helper.CreateCopyStatement(TableName, Headers));
    }

    public string DatabaseName { get; set; }

    public string CsvName { get; set; }

    public string TableName { get; set; }

    public string Headers { get; set; }

    public List<int> IndicesToRemove { get; set; } = [];

    public NpgsqlConnection Connection { get; set; }

    public TextWriter Writer { get; set; }

    public void WriteLine(string line)
    {
        Writer.WriteLine(Helper.RemoveItemsByIndices(line, IndicesToRemove));
    }
}