using Npgsql;

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

                // Read and write the header
                foreach (var uploadEntry in uploadEntries)
                {
                    uploadEntry.OpenConnection();
                    await using (var command = new NpgsqlCommand(
                                     $"TRUNCATE TABLE public.\"{uploadEntry.TableName}\" RESTART IDENTITY",
                                     uploadEntry.Connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                if ((line = await responseStreamReader.ReadLineAsync()) != null)
                {
                    var newLine = $"Id,{line}";
                    foreach (var uploadEntry in uploadEntries)
                    {
                        uploadEntry.SetHeaders(newLine);
                        await uploadEntry.CreateWriter();
                        uploadEntry.WriteLine(newLine);
                    }
                }

                // Process each subsequent line
                while ((line = await responseStreamReader.ReadLineAsync()) != null)
                {
                    id++;
                    var newLine = $"{id},{line}";
                    foreach (var uploadEntry in uploadEntries)
                    {
                        uploadEntry.WriteLine(newLine);
                    }
                }

                foreach (var uploadEntry in uploadEntries)
                {
                    uploadEntry.Writer.Close();
                }
            }
        }
    }
}