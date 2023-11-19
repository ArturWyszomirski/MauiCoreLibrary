namespace MauiCoreLibrary.Models;

public partial class PersonalDataModel : ObservableRecipient, IPersonalDataModel // ObservableValidator !!
{
    [PrimaryKey, AutoIncrement] 
    public int Id { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullName))]
    [NotifyPropertyChangedRecipients]
    private string _firstName;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullName))]
    [NotifyPropertyChangedRecipients]
    private string _lastName;

    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    private string _emailAddress;

    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    private string _phoneNumber;

    public string FullName {
        get
        {
            if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
                return $"{FirstName} {LastName}";
            else if (!string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName))
                return $"{FirstName}";
            else if (string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
                return $"{LastName}";
            else
                return null;
        }
    }

    public virtual void ClearData()
    {
        FirstName = default;
        LastName = default;
        EmailAddress = default;
        PhoneNumber = default;
    }
    public virtual PersonalDataModel Clone()
    {
        return (PersonalDataModel)MemberwiseClone();
    }

}
