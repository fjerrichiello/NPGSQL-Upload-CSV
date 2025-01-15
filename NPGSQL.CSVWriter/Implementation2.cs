namespace NPGSQL.CSVWriter;

public class Implementation2
{
    public async Task WriteCsvGeneration(List<UploadEntry2> uploadEntries)
    {
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync(Helper.ApiUrl);

            if (response.IsSuccessStatusCode)
            {
                // Open the response stream
                await using var stream = await response.Content.ReadAsStreamAsync();
                using var responseStreamReader = new StreamReader(stream);

                string? line;
                var id = 0;
                var first = true;

                // Read and write the header
                await uploadEntries.OpenConnectionsAndCleanTables();

                // Process each subsequent line
                while ((line = await responseStreamReader.ReadLineAsync()) != null)
                {
                    if (first)
                    {
                        var header = $"Id,{line}";
                        uploadEntries.SetHeader(header);
                        await uploadEntries.CreateWriter();
                        uploadEntries.WriteLine(header);
                        first = false;
                        continue;
                    }

                    id++;
                    var newLine = $"{id},{line}";
                    uploadEntries.WriteLine(newLine);
                }

                uploadEntries.CloseConnections();
            }
        }
    }
}