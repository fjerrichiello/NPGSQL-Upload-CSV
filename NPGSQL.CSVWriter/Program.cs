// See https://aka.ms/new-console-template for more information

using Npgsql;

const string connectionString =
    "Host=localhost;Username=postgres;Password=postgres;Database=csvupload;IncludeErrorDetail=true;";
const string inputFilePath = "data.csv";
const string outputFilePath = "updated-data.csv"; // Path to the new CSV with the Id column

using (var reader1 = new StreamReader(inputFilePath))
using (var writer1 = new StreamWriter(outputFilePath))
{
    string? line;
    var id = 0;

    // Read and write the header
    if ((line = reader1.ReadLine()) != null)
    {
        writer1.WriteLine($"Id,{line}");
    }

    // Process each subsequent line
    while ((line = reader1.ReadLine()) != null)
    {
        id++;
        writer1.WriteLine($"{id},{line}");
    }
}


using var connection = new NpgsqlConnection(connectionString);
connection.Open();

// Use COPY command
using var writer =
    connection.BeginTextImport(
        """
                COPY public."Books" ("Id", "AuthorId", "Title") FROM STDIN (FORMAT CSV, HEADER TRUE)
        """);

using var reader = new StreamReader(outputFilePath);

while (!reader.EndOfStream)
{
    var line = reader.ReadLine();
    writer.WriteLine(line);
}