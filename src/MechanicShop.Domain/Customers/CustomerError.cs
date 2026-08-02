using MechanicShop.Domain.Common.Results;

namespace MechanicShop.Domain.Customers;

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

