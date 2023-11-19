namespace MauiCoreLibrary.Models
{
    public interface IPersonalDataModel
    {
        int Id { get; set; }
        string EmailAddress { get; set; }
        string FirstName { get; set; }
        string FullName { get; }
        string LastName { get; set; }
        string PhoneNumber { get; set; }

        void ClearData();
        PersonalDataModel Clone();
    }
}