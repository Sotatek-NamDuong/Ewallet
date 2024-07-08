namespace EWallet.API.Services;

public class NotificationService : INotificationService
{
     public Task SendTransactionNotificationAsync(Guid userId, Guid transactionId)
     {
          throw new NotImplementedException();
     }
}

public interface INotificationService
{
     Task SendTransactionNotificationAsync(Guid userId, Guid transactionId);
}