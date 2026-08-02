using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Text.RegularExpressions;
using MechanicShop.Domain.Common;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.Customers.Vehicles;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.VisualBasic;

namespace MechanicShop.Domain.Customers;

public class Customer : AuditableEntity
{
    public string? Name {get;private set;}
    public string? PhoneNumber {get;private set;}
    public string? Email {get; private set;}
    private List<Vehicle> _vehicles = [];
    public ReadOnlyCollection<Vehicle> Vehicles => _vehicles.AsReadOnly();

    private Customer(Guid id,string name , string phoneNumber,string email,List<Vehicle> vehicles):base(id)
    {   
        Name = name;
        PhoneNumber = phoneNumber;
        Email = email;
        _vehicles = vehicles;
    }

    public static Result<Customer> Create(Guid id,string name , string phoneNumber,string email,List<Vehicle> vehicles)
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

        return new Customer(id,name,phoneNumber,email,vehicles);
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

    public Result<Updated> Upsert(List<Vehicle> incomingVehicles)
    {
        _vehicles.RemoveAll(existedVehicle=> incomingVehicles.All(i=>i.Id != existedVehicle.Id));// remove current vehicles that are not included in the new incoming vehicles

        foreach (var v in incomingVehicles)
        {
            var existing = _vehicles.FirstOrDefault(x=>x.Id==v.Id);
            if (existing is null)
            {
                _vehicles.Add(v);
            }
            else
            {
               var result = existing.Update(v.Make,v.Model,v.Year,v.LicensePlate);
               if (result.IsError)
                {
                    return result.Errors;
                }
            }
        }
        return Result.Updated;
    }
}

