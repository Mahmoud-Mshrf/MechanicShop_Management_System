// this interface will be implemented using SignalR to send notifications 

public interface IWorkOrderNotifier
{
    Task NotifyWorkOrdersChangedAsync(CancellationToken ct=default);
}