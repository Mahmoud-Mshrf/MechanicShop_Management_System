// this interface for getting userId from the httpContext (in case of authenticated users) 
// so we can use it in different places and logs who is the user trying to access request or doing something

public interface IUser
{
    string? Id {get;}
}
