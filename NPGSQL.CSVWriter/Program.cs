// See https://aka.ms/new-console-template for more information

using Npgsql;

const string apiUrl = "http://localhost:5085/download-csv"; // Update the port if needed

const string connectionString =
    "Host=localhost;Username=postgres;Password=postgres;Database=csvupload;IncludeErrorDetail=true;";
const string downloadFilePath = "downloaded_books.csv";

const string outputFilePath1 = "data1.csv"; // Path to the new CSV with the Id column

using (var httpClient = new HttpClient())
{
    var response = await httpClient.GetAsync(apiUrl, HttpCompletionOption.ResponseHeadersRead);

    if (response.IsSuccessStatusCode)
    {
        // Open the response stream
        await using var stream = await response.Content.ReadAsStreamAsync();
        using var responseStreamReader = new StreamReader(stream);
        await using var csvWriter1 = new StreamWriter(outputFilePath1);
        string? line;
        var id = 0;

        // Read and write the header
        if ((line = responseStreamReader.ReadLine()) != null)
        {
            csvWriter1.WriteLine($"Id,{line}");
        }

        // Process each subsequent line
        while ((line = responseStreamReader.ReadLine()) != null)
        {
            id++;
            csvWriter1.WriteLine($"{id},{line}");
        }
    }
}


await using var connection = new NpgsqlConnection(connectionString);
connection.Open();

// Use COPY command
await using var writer =
    connection.BeginTextImport(
        """
                COPY public."Books" ("Id", "AuthorId", "Title") FROM STDIN (FORMAT CSV, HEADER TRUE)
        """);

using var reader = new StreamReader(outputFilePath1);

while (!reader.EndOfStream)
{
    var line = reader.ReadLine();
    writer.WriteLine(line);
}