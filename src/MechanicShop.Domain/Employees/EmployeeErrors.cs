using MechanicShop.Domain.Common.Results;

namespace MechanicShop.Domain.Employees;

public static class EmployeeErrors
{
    public static readonly Error IdRequired =
        Error.Validation("Employee.Id.Required", "Employee Id is required.");

    public static Error FirstNameRequired
        => Error.Validation("FirstName_Is_Required","Employee first name is required and must be less than 50 characters");

    public static Error LastNameRequired
        => Error.Validation("LastName_Is_Required","Employee last name is required and must be less than 50 characters");
    
    public static Error InvalidRole
        => Error.Validation("Invalid_Role","Invalid role assigned to the employee");
    
}