namespace MauiCoreLibrary.Models
{
    public interface IPersonalDataModel
    {
        string EmailAddress { get; set; }
        string FirstName { get; set; }
        string FullName { get; }
        string LastName { get; set; }
        string PhoneNumber { get; set; }

        void ClearData();
        PersonalDataModel Clone();
    }
}