namespace MechanicShop.Application.Common.Models;
public class EmailMessage
{
    public string To {get;set;}
    public string Body {get;set;}
    public string Subject {get;set;}
    public bool IsHtml{get;set;}
}