namespace NPGSQL.CSVWriter;

/// <summary>
/// Represents an entry for uploading CSV data to a PostgreSQL database.
/// </summary>
public class UploadEntry 
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UploadEntry"/> class.
    /// </summary>
    /// <param name="databaseName">The name of the database.</param>
    /// <param name="csvName">The name of the CSV file.</param>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="indicesToRemove">A list of indices to remove from the CSV data.</param>
    public UploadEntry(string databaseName, string csvName, string tableName, List<int> indicesToRemove)
    {
        DatabaseName = databaseName;
        CsvName = csvName;
        TableName = tableName;
        IndicesToRemove = indicesToRemove;
        Writer = new StreamWriter(csvName);
    }

    /// <summary>
    /// Sets the headers for the CSV data.
    /// </summary>
    /// <param name="header">The header line of the CSV file.</param>
    public void SetHeaders(string header)
    {
        Headers = Helper.RemoveItemsByIndices(header, IndicesToRemove);
    }

    /// <summary>
    /// Gets or sets the name of the database.
    /// </summary>
    public string DatabaseName { get; set; }

    /// <summary>
    /// Gets or sets the name of the CSV file.
    /// </summary>
    public string CsvName { get; set; }

    /// <summary>
    /// Gets or sets the name of the table.
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// Gets or sets the headers of the CSV data.
    /// </summary>
    public string Headers { get; set; }

    /// <summary>
    /// Gets or sets the list of indices to remove from the CSV data.
    /// </summary>
    public List<int> IndicesToRemove { get; set; } = [];

    /// <summary>
    /// Gets or sets the StreamWriter for writing the CSV data.
    /// </summary>
    public StreamWriter Writer { get; set; }

    /// <summary>
    /// Writes a line to the CSV file after removing specified indices.
    /// </summary>
    /// <param name="line">The line to write to the CSV file.</param>
    public void WriteLine(string line)
    {
        Writer.WriteLine(Helper.RemoveItemsByIndices(line, IndicesToRemove));
    }
}