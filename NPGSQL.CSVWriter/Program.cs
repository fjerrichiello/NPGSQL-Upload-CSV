// See https://aka.ms/new-console-template for more information

using Npgsql;
using NPGSQL.CSVWriter;

const string apiUrl = "http://localhost:5085/download-csv"; // Update the port if needed

const string connectionString =
    "Host=localhost;Username=postgres;Password=postgres;Database={0};IncludeErrorDetail=true;";

List<UploadEntry> uploadEntries =
[
    new UploadEntry(
        "csvupload",
        "data1.csv",
        "Books",
        []
    ),
    new UploadEntry(
        "csvupload2",
        "data2.csv",
        "OtherBooks",
        [1]
    )
];

using (var httpClient = new HttpClient())
{
    var response = await httpClient.GetAsync(apiUrl);

    if (response.IsSuccessStatusCode)
    {
        // Open the response stream
        await using var stream = await response.Content.ReadAsStreamAsync();
        using var responseStreamReader = new StreamReader(stream);

        string? line;
        var id = 0;

        // Read and write the header
        if ((line = responseStreamReader.ReadLine()) != null)
        {
            var newLine = $"Id,{line}";
            foreach (var uploadEntry in uploadEntries)
            {
                uploadEntry.SetHeaders(newLine);
                uploadEntry.WriteLine(newLine);
            }
        }

        // Process each subsequent line
        while ((line = responseStreamReader.ReadLine()) != null)
        {
            id++;
            var newLine = $"{id},{line}";
            foreach (var uploadEntry in uploadEntries)
            {
                uploadEntry.WriteLine(newLine);
            }
        }
    }

    foreach (var uploadEntry in uploadEntries)
    {
        uploadEntry.Writer.Close();
    }
}


foreach (var uploadEntry in uploadEntries)
{
    await using (var connection = new NpgsqlConnection(string.Format(connectionString, uploadEntry.DatabaseName)))
    {
        connection.Open();

        // Use COPY command
        await using var writer =
            connection.BeginTextImport(Helper.CreateCopyStatement(uploadEntry.TableName, uploadEntry.Headers));

        using var reader = new StreamReader(uploadEntry.CsvName);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            await writer.WriteLineAsync(line);
        }
    }
}