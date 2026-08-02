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

    private Customer(string? name , string? phoneNumber,string? email)
    {   
        Name = name;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public Result<Customer> Create(string? name , string? phoneNumber,string? email)
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

        return new Customer(name,phoneNumber,email);
    }

    public Result<Updated> Update(string? name , string? phoneNumber,string? email)
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
public static class CustomerError
{
    public static Error NameRequired 
        => Error.Validation("Name_Is_Required","Customer Name is required"); 

    public static Error EmailRequired
        => Error.Validation("PhoneNumber_Is_Required","Phone number is required");

    public static Error InvalidEmail
        => Error.Validation("Email_Is_Invalid","Email is invalid");

    public static Error InvalidPhoneNumber
        => Error.Validation("Invalid_PhoneNumber","Phone number is invalid");
}

