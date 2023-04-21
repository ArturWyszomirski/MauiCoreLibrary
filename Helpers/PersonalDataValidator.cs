namespace MauiCoreLibrary.Helpers;

public class PersonalDataValidator
{
    public static bool ValidateEntries(IPersonalDataModel personalData)
    {
        if (personalData != null)
        {
            return (!string.IsNullOrEmpty(personalData.FirstName) || !string.IsNullOrEmpty(personalData.LastName))
                   && NameEntryValidation.Validate(personalData.FirstName)
                   && NameEntryValidation.Validate(personalData.LastName)
                   && EmailAddressEntryValidation.Validate(personalData.EmailAddress)
                   && PhoneNumberEntryValidation.Validate(personalData.PhoneNumber);
        }
        else
            return false;
    }
}
