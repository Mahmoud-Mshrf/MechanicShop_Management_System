using System.Net.Mail;
using System.Text.RegularExpressions;
using MechanicShop.Domain.Common;
using MechanicShop.Domain.Common.Results;
using Microsoft.CSharp.RuntimeBinder;

namespace MechanicShop.Domain.Customer;

public class Customer : AuditableEntity
{
    public string? Name {get;private set;}
    public string? PhoneNumber {get;private set;}
    public string? Email {get; private set;}
    // private List<Vehicle> _vehicles = [];

    private Customer(Guid id,string name , string phoneNumber,string email):base(id)
    {   
        Name = name;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public static Result<Customer> Create(Guid id,string name , string phoneNumber,string email)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return CustomerError.NameRequired;
        }
        if (string.IsNullOrWhiteSpace(phoneNumber)|| Regex.IsMatch(phoneNumber,@"^\+?\d{7,15}$"))
        {
            return CustomerError.InvalidPhoneNumber;
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            return CustomerError.EmailRequired;
        }

        try
        {
            _ = new MailAddress(email);
        }
        catch
        {
            return CustomerError.InvalidEmail;
        }

        return new Customer(id,name,phoneNumber,email);
    }

    public Result<Updated> Update(string name , string phoneNumber,string email)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return CustomerError.NameRequired;
        }
        if (string.IsNullOrWhiteSpace(phoneNumber)|| Regex.IsMatch(phoneNumber,@"^\+?\d{7,15}$"))
        {
            return CustomerError.InvalidPhoneNumber;
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            return CustomerError.EmailRequired;
        }

        try
        {
            _ = new MailAddress(email);
        }
        catch
        {
            return CustomerError.InvalidEmail;
        }

        Name =name;
        Email= email;
        PhoneNumber=phoneNumber;
        
        return Result.Updated;
    }
}

