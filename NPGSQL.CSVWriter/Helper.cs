using Dumpify;

namespace NPGSQL.CSVWriter;

public static class Helper
{
    public static string RemoveItemsByIndices(string input, List<int> indicesToRemove)
    {
        if (indicesToRemove.Count == 0)
        {
            return input;
        }

        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        // Split the input string by commas
        var items = input.Split(',');

        // Create a set for indices to remove for faster lookup
        var indicesSet = new HashSet<int>(indicesToRemove);

        // Filter out items that are at the specified indices
        var filteredItems = items
            .Where((item, index) => !indicesSet.Contains(index))
            .ToList();

        // Join the remaining items back into a comma-delimited string
        return string.Join(",", filteredItems);
    }

    public static string CreateCopyStatement(string tableName, string columnNames)
    {
        var columns = string.Join(", ", columnNames.Split(',').Select(c => $"\"{c}\""));

        var copyStatement = $@"
            COPY public.""{tableName}"" ({columns}) FROM STDIN (FORMAT CSV, HEADER TRUE)
        ";
        copyStatement.Dump();
        return copyStatement;
    }
}