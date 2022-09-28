using CoombsBank.Interfaces;
using Google.Cloud.Firestore;

namespace CoombsBank.Models;

[FirestoreData]
public class User : IFirebaseEntity
{
    [FirestoreProperty]
    public string Id { get; set; }

    public User() { }
    
    [FirestoreProperty]
    public string FirstName { get; set; }
    
    [FirestoreProperty]
    public string LastName { get; set; }
    
    [FirestoreProperty]
    public string Email { get; set; }
    
    [FirestoreProperty]
    public string IbanNo { get; set; }
    
    public User(string firstName, string lastName, string email, string ibanNo)
    {
        Id = Guid.NewGuid().ToString("N");
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        IbanNo = ibanNo;
    }
}