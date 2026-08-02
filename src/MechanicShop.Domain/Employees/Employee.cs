using MechanicShop.Domain.Common;
using MechanicShop.Domain.Common.Results;

namespace MechanicShop.Domain.Employees;

public sealed class Employee : AuditableEntity
{
    public string? FirstName {get;private set;}
    public string? LastName {get;set;}
    public Role Role {get;set;}
    public string FullName => $"{FirstName} {LastName}";

    private Employee()
    {
        
    }
    private Employee(Guid id ,string firstName, string lastName,Role role):base(id)
    {
        FirstName=firstName;
        LastName=lastName;
        Role = role;
    }

    public static Result<Employee> Create(Guid id ,string firstName, string lastName,Role role)
    {
        if (string.IsNullOrWhiteSpace(firstName) || firstName.Length > 50)
        {
            return EmployeeErrors.FirstNameRequired;
        }

        if (string.IsNullOrWhiteSpace(lastName) || lastName.Length > 50)
        {
            return EmployeeErrors.LastNameRequired;
        }

        // if (!Enum.IsDefined(typeof(Role),role))
        // {
        //     return EmployeeErrors.InvalidRole;
        // }
         if (!Enum.IsDefined(role))
        {
            return EmployeeErrors.InvalidRole;
        }
        
        return new Employee(id,firstName,lastName,role);
    }

    // public Result<Updated> Update(string firstName, string lastName)
    // {
    //     if (string.IsNullOrWhiteSpace(firstName) || firstName.Length > 50)
    //     {
    //         return EmployeeErrors.FirstNameRequired;
    //     }

    //     if (string.IsNullOrWhiteSpace(lastName) || lastName.Length > 50)
    //     {
    //         return EmployeeErrors.LastNameRequired;
    //     }

    //     FirstName = firstName;
    //     LastName = lastName;

    //     return Result.Updated;
    // }


}
