using System.Text.RegularExpressions;

namespace MauiCoreLibrary.Behaviors;

public class NameEntryValidation : EntryValidatonBase
{
    /// <summary>
    /// Change text color of <paramref name="sender"/> (entry) to red if text did not pass validation or black if validation is passed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    protected override void OnEntryTextChanged(object sender, TextChangedEventArgs args)
    {
        ((Entry)sender).TextColor = Validate(args.NewTextValue) ? Colors.Black : Colors.Red;
    }

    /// <summary>
    /// Verifies whether provided name is in correct format. 
    /// Correct format is a string begging with capital letter or several strings separated with space or dash all beginning with capital letter.
    /// Examples: Tadeusz, Boy-Żeleński, Jan Maria
    /// </summary>
    /// <returns>True if validation is passed.</returns>
    public static bool Validate(string name)
    {
        if (name != null)
            return Regex.IsMatch(name,
                @"^([A-ZĆŁŚŹŻ][a-ząćęłóńśźż]+$)|([A-ZĆŁŚŹŻ][a-ząćęłóńśźż]+((\s|-)[A-ZĆŁŚŹŻ][a-ząćęłóńśźż]+)+)$")
                || name.Length == 0;
        else
            return true;
    }
}
