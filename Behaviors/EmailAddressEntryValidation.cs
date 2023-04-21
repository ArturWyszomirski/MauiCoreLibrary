using System.Net.Mail;

namespace MauiCoreLibrary.Behaviors;

public class EmailAddressEntryValidation : EntryValidatonBase
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
    /// Verifies whether provided e-mail address is in correct format. 
    /// </summary>
    /// <returns>True if validation is passed.</returns>
    public static bool Validate(string eMailAddress)
    {
        if (eMailAddress != null && eMailAddress.Length > 0)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(eMailAddress);
                return mailAddress.Address == eMailAddress;
            }
            catch
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }
}