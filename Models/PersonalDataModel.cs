namespace MauiCoreLibrary.Models;

//[KnownType(typeof(PersonalDataModel))]
//[DataContract]
//[ObservableObject]
public partial class PersonalDataModel : ObservableRecipient, IPersonalDataModel
{
    //[DataMember]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullName))]
    [NotifyPropertyChangedRecipients]
    private string _firstName;

    //[DataMember]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullName))]
    [NotifyPropertyChangedRecipients]
    private string _lastName;

    //[DataMember]
    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    private string _emailAddress;

    //[DataMember]
    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    private string _phoneNumber;

    public string FullName { get => $"{FirstName} {LastName}"; }

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
