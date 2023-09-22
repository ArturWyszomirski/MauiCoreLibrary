namespace MauiCoreLibrary.Helpers;

public class CsvDataExtractor
{
    /// <summary>
    /// Converts .csv file to a generic list.
    /// </summary>
    /// <param name="csvFilePath">Path to .csv file.</param>
    /// <param name="columnsScope">Column range. Provide in format: [first column of range, last column of range].</param>
    /// <param name="columns">Selected columns.</param>
    /// <param name="rowsScope">Rows range. Provide in format: [first row of range, last row of range].</param>
    /// <param name="rows">Selected rows.</param>
    public static (List<T[]>, int, int) ExtractData<T>(string csvFilePath, int[] columnsScope = null, int[] columns = null, int[] rowsScope = null, int[] rows = null)
    {
        CheckScopes(csvFilePath, columnsScope, rowsScope);
        string[] csvLines = GetCsvLines(csvFilePath);
        columns = GetColumns(columnsScope, columns, csvLines);
        rows = GetRows(rowsScope, rows, csvLines);

        List<T[]> data = new();
        foreach (int rowIndex in rows)
        {
            List<T> values = new();
            string[] csvValues = csvLines[rowIndex].Split(',');

            foreach (int columnIndex in columns)
            {
                object value = GetValue(csvValues[columnIndex]);
                Type type = value.GetType();
                if (typeof(T) == value.GetType())
                    values.Add((T)value);
                else
                    throw new Exception($"Value {value} cannot be converted to type {typeof(T)}.");
            }

            data.Add(values.ToArray());
        }

        return (data, columns.Length, rows.Length);
    }

    /// <summary>
    /// Converts .csv file to Json string.
    /// </summary>
    /// <param name="csvFilePath">Path to .csv file.</param>
    /// <param name="columnsScope">Column range. Provide in format: [first column of range, last column of range].</param>
    /// <param name="columns">Selected columns.</param>
    /// <param name="rowsScope">Rows range. Provide in format: [first row of range, last row of range].</param>
    /// <param name="rows">Selected rows.</param>
    public static string ConvertCsvToJson(string csvFilePath, int[] columnsScope = null, int[] columns = null, int[] rowsScope = null, int[] rows = null)
    {
        CheckScopes(csvFilePath, columnsScope, rowsScope);
        string[] csvLines = GetCsvLines(csvFilePath);
        columns = GetColumns(columnsScope, columns, csvLines);
        rows = GetRows(rowsScope, rows, csvLines);

        return ConvertToDictionaries(columns, ref rows, csvLines);
    }

    private static void CheckScopes(string csvFilePath, int[] columnsScope, int[] rowsScope)
    {
        #region Exceptions
        if (string.IsNullOrEmpty(csvFilePath))
            throw new ArgumentException($"Parameter {nameof(csvFilePath)} cannot be null or empty.", nameof(csvFilePath));

        if (columnsScope is not null && columnsScope.Length != 2)
            throw new ArgumentException($"Incorrect format of {nameof(columnsScope)}.", nameof(columnsScope));

        if (columnsScope is not null && columnsScope[0] > columnsScope[1])
            throw new ArgumentException($"Parameter {nameof(columnsScope)}[0] can't be greater than {nameof(columnsScope)}[1].", nameof(columnsScope));

        if (rowsScope is not null && rowsScope.Length != 2)
            throw new ArgumentException($"Incorrect format of {nameof(rowsScope)}.", nameof(rowsScope));

        if (rowsScope is not null && rowsScope[0] > rowsScope[1])
            throw new ArgumentException($"Parameter {nameof(rowsScope)}[0] can't be greater than {nameof(rowsScope)}[1].", nameof(rowsScope));
        #endregion
    }

    private static string[] GetCsvLines(string csvFilePath)
    {
        string[] csvLines = File.ReadAllLines(csvFilePath);

        if (csvLines.Length == 0)
            return null;

        if (csvLines[0].StartsWith("sep"))
            csvLines = csvLines.Skip(1).ToArray();

        return csvLines;
    }

    private static int[] GetColumns(int[] columnsScope, int[] columns, string[] csvLines)
    {
        int totalColumns = csvLines[0].Split(',').Length;

        if (columnsScope != null && columns != null)
            columns = GetCombinedIndices(columnsScope, columns);
        else if (columnsScope != null)
            columns = GetRangeIndices(columnsScope);
        else if (columnsScope == null && columns == null)
            columns = GetAllIndices(totalColumns);

        ValidateIndicies(columns, totalColumns, "column");

        return columns;
    }

    private static int[] GetRows(int[] rowsScope, int[] rows, string[] csvLines)
    {
        int totalRows = csvLines.Length;

        if (rowsScope != null && rows != null)
            rows = GetCombinedIndices(rowsScope, rows);
        else if (rowsScope != null)
            rows = GetRangeIndices(rowsScope);
        else if (rowsScope == null && rows == null)
            rows = GetAllIndices(totalRows);

        ValidateIndicies(rows, totalRows, "row");

        return rows;
    }

    private static int[] GetCombinedIndices(int[] range, int[] indices)
    {
        int[] rangeIndices = GetRangeIndices(range);
        return rangeIndices.Union(indices).ToArray();
    }

    private static int[] GetRangeIndices(int[] range)
    {
        int start = range[0];
        int end = range[1];

        return Enumerable.Range(start, end - start + 1).ToArray();
    }

    private static int[] GetAllIndices(int upperBound)
    {
        int[] range = new int[2] {0, upperBound - 1};

        return GetRangeIndices(range);
    }

    private static void ValidateIndicies(int[] indices, int upperBound, string type)
    {
        if (indices != null)
            foreach (int index in indices)
                if (index < 0 || index >= upperBound)
                    throw new ArgumentOutOfRangeException(type, $"The {type} index '{index}' is out of range.");
    }

    private static string ConvertToDictionaries(int[] columns, ref int[] rows, string[] csvLines)
    {
        List<Dictionary<string, object>> jsonData = new();

        string[] headers = csvLines[0].Split(',');
        rows = rows.Skip(1).ToArray();

        foreach (int rowIndex in rows)
        {
            Dictionary<string, object> jsonObject = new();
            string[] csvValues = csvLines[rowIndex].Split(',');

            foreach (int columnIndex in columns)
            {
                string key = headers[columnIndex];
                object value = GetValue(csvValues[columnIndex]);
                jsonObject[key] = value;
            }

            jsonData.Add(jsonObject);
        }

        return JsonSerializer.Serialize(jsonData);
    }

    private static object GetValue(string value)
    {
        if (float.TryParse(value, out float floatValue))
            return floatValue;
        else if (int.TryParse(value, out int intValue))
            return intValue;
        else if (bool.TryParse(value, out bool boolValue))
            return boolValue;
        else
            return value;
    }
}