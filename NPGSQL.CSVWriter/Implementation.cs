using Npgsql;

namespace NPGSQL.CSVWriter;

public class Implementation
{
    /// <summary>
    /// Writes CSV data by fetching it from a specified API, processing it, and uploading it to a PostgreSQL database.
    /// </summary>
    /// <param name="uploadEntries">A list of UploadEntry objects to handle the CSV data.</param>
    public async Task WriteCsvGeneration(List<UploadEntry> uploadEntries)
    {
        // Create an HttpClient instance to send HTTP requests
        using (var httpClient = new HttpClient())
        {
            // Send a GET request to the API URL
            var response = await httpClient.GetAsync(Helper.ApiUrl);

            // Check if the response is successful
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

                // Close the writer for each upload entry
                foreach (var uploadEntry in uploadEntries)
                {
                    uploadEntry.Writer.Close();
                }
            }
        }

        // Upload the CSV data to the PostgreSQL database
        foreach (var uploadEntry in uploadEntries)
        {
            await using (var connection =
                         new NpgsqlConnection(string.Format(Helper.ConnectionString, uploadEntry.DatabaseName)))
            {
                connection.Open();

                // Use COPY command to import data
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
    }
}