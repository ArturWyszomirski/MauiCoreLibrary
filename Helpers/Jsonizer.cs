﻿using Newtonsoft.Json;

namespace MauiCoreLibrary.Helpers;

public class Jsonizer
{
    /// <summary>
    /// Converts .csv file to Json string.
    /// </summary>
    /// <param name="csvFilePath">Path to .csv file.</param>
    /// <param name="kvpFormat">If true .csv file will be converted to a collection of JSON objects with kvp pair. 
    ///                         If false .csv file will be converted to 2D array. 
    ///                         Default set to true.</param>
    /// <param name="getHeaders">Defines wether headers should be put in JSON string. Must be set true if <paramref name="kvpFormat"/> is set to true. Default set to true.</param>
    /// <param name="columnsScope">Column range. Provide in format: [first column of range, last column of range].</param>
    /// <param name="columns">Selected columns.</param>
    /// <param name="rowsScope">Rows range. Provide in format: [first row of range, last row of range].</param>
    /// <param name="rows">Selected rows.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string ConvertCsvToJson(string csvFilePath,
                                            bool kvpFormat = true,
                                            bool getHeaders = true,
                                            int[] columnsScope = null,
                                            int[] columns = null,
                                            int[] rowsScope = null,
                                            int[] rows = null)
    {
        #region Exceptions
        if (string.IsNullOrEmpty(csvFilePath))
            throw new ArgumentException($"Parameter {nameof(csvFilePath)} cannot be null or empty.", nameof(csvFilePath));

        if(!getHeaders && kvpFormat)
            throw new ArgumentException($"{nameof(getHeaders)} can't be false if {nameof(kvpFormat)} is set to true." ,nameof(kvpFormat));

        if (columnsScope is not null && columnsScope.Length != 2)
            throw new ArgumentException($"Incorrect format of {nameof(columnsScope)}.", nameof(columnsScope));

        if (columnsScope is not null && columnsScope[0] > columnsScope[1])
            throw new ArgumentException($"Parameter {nameof(columnsScope)}[0] can't be greater than {nameof(columnsScope)}[1].", nameof(columnsScope));

        if (rowsScope is not null && rowsScope.Length != 2)
            throw new ArgumentException($"Incorrect format of {nameof(rowsScope)}.", nameof(rowsScope));

        if (rowsScope is not null && rowsScope[0] > rowsScope[1])
            throw new ArgumentException($"Parameter {nameof(rowsScope)}[0] can't be greater than {nameof(rowsScope)}[1].", nameof(rowsScope));
        #endregion

        string[] csvLines = File.ReadAllLines(csvFilePath);

        if (csvLines.Length == 0)
            return null;

        if (csvLines[0].StartsWith("sep"))
            csvLines = csvLines.Skip(1).ToArray();

        columns = GetColumns(columnsScope, columns, csvLines);
        rows = GetRows(rowsScope, rows, csvLines);

        if (kvpFormat)
            return ConvertToDictionaries(columns, ref rows, csvLines);
        else
            return ConvertToArrays(getHeaders, ref rows, columns, csvLines);
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
        Dictionary<string, object> jsonObject = new();

        string[] headers = csvLines[0].Split(',');
        rows = rows.Skip(1).ToArray();

        foreach (int rowIndex in rows)
        {
            string[] csvValues = csvLines[rowIndex].Split(',');

            foreach (int columnIndex in columns)
            {
                string key = headers[columnIndex];
                object value = GetValue(csvValues[columnIndex]);
                jsonObject[key] = value;
            }

            jsonData.Add(jsonObject);
        }

        return JsonConvert.SerializeObject(jsonData, Formatting.Indented);
    }

    private static string ConvertToArrays(bool getHeaders, ref int[] rows, int[] columns, string[] csvLines)
    {
        if (!getHeaders && rows.Contains(0))
            rows = rows.Skip(1).ToArray();

        object[,] jsonData = new object[rows.Length, columns.Length];
        int jsonDataRowIndex = 0;

        foreach (int rowIndex in rows)
        {
            string[] csvValues = csvLines[rowIndex].Split(",");
            int jsonDataColumnIndex = 0;

            foreach (int columnIndex in columns)
            {
                jsonData[jsonDataRowIndex, jsonDataColumnIndex] = GetValue(csvValues[columnIndex]);
                jsonDataColumnIndex++;
            }
            jsonDataRowIndex++;
        }

        return JsonConvert.SerializeObject(jsonData, Formatting.Indented);
    }

    private static object GetValue(string value)
    {
        if (int.TryParse(value, out int intValue))
            return intValue;
        else if (double.TryParse(value, out double doubleValue))
            return doubleValue;
        else if (bool.TryParse(value, out bool boolValue))
            return boolValue;
        else
            return value;
    }
}