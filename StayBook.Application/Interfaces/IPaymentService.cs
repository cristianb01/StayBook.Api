using StayBook.Application.Models;

namespace StayBook.Application.Interfaces;

public interface IPaymentService
{
    Task<PaymentResult> VerifyAsync(string paymentReferenceId, CancellationToken cancellationToken);
}