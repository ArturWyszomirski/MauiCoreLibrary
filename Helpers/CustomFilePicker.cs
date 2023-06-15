﻿namespace MauiCoreLibrary.Helpers;

public class CustomFilePicker
{
    public static async Task<FileResult> PickCsvFileAsync()
    {
        return await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Select CSV File",
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.Android, new[] { "application/csv", "text/csv" } },
                { DevicePlatform.iOS, new[] { "com.microsoft.csv" } },
                { DevicePlatform.WinUI, new[] { ".csv" } }
            })
        });
    }
}
