using Npgsql;

namespace NPGSQL.CSVWriter;

public static class Extensions
{
    public static async Task OpenConnectionsAndCleanTables(this List<UploadEntry2> uploadEntries)
    {
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
    }

    public static void SetHeader(this List<UploadEntry2> uploadEntries, string line)
    {
        uploadEntries.ForEach(ue => ue.SetHeaders(line));
    }

    public static async Task CreateWriter(this List<UploadEntry2> uploadEntries)
    {
        foreach (var uploadEntry in uploadEntries)
        {
            await uploadEntry.CreateWriter();
        }
    }

    public static void WriteLine(this List<UploadEntry2> uploadEntries, string line)
    {
        uploadEntries.ForEach(ue => ue.WriteLine(line));
    }

    public static void CloseConnections(this List<UploadEntry2> uploadEntries)
    {
        uploadEntries.ForEach(ue => ue.CloseConnection());
    }
}