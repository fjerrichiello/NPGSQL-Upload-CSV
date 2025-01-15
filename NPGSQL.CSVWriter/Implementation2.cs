namespace NPGSQL.CSVWriter;

public class Implementation2
{
    /// <summary>
    /// Writes CSV data by fetching it from a specified API and processing it.
    /// </summary>
    /// <param name="uploadEntries">A list of UploadEntry2 objects to handle the CSV data.</param>
    public async Task WriteCsvGeneration(List<UploadEntry2> uploadEntries)
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
                var first = true;

                // Open connections and clean tables before writing data
                await uploadEntries.OpenConnectionsAndCleanTables();

                // Process each line from the response stream
                while ((line = await responseStreamReader.ReadLineAsync()) != null)
                {
                    if (first)
                    {
                        // Write the header line with an added Id column
                        var header = $"Id,{line}";
                        uploadEntries.SetHeader(header);
                        await uploadEntries.CreateWriter();
                        uploadEntries.WriteLine(header);
                        first = false;
                        continue;
                    }

                    // Write each subsequent line with an incremented Id
                    id++;
                    var newLine = $"{id},{line}";
                    uploadEntries.WriteLine(newLine);
                }

                // Close connections after writing data
                uploadEntries.CloseConnections();
            }
        }
    }
}