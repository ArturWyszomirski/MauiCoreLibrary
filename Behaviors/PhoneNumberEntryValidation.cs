using System.Text.RegularExpressions;

namespace MauiCoreLibrary.Behaviors;

public class PhoneNumberEntryValidation : EntryValidatonBase
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
    /// Verifies whether provided phone number is in correct format. 
    /// Correct format is a 9-digit number or 11-digit number begining with + prefix.
    /// Examples: 604945108, +48586657039 
    /// </summary>
    /// <returns>True if validation is passed.</returns>
    public static bool Validate(string phoneNumber)
    {
        if (phoneNumber != null)
            return Regex.IsMatch(phoneNumber, @"^[0-9]{9}$|^.+[0-9]{11}$") || phoneNumber.Length == 0;
        else
            return true;
    }
}
