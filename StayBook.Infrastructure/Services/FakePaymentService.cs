using StayBook.Application.Interfaces;
using StayBook.Application.Models;

namespace StayBook.Infrastructure.Services;

public class FakePaymentService : IPaymentService
{
    public Task<PaymentResult> VerifyAsync(string paymentReferenceId, CancellationToken cancellationToken)
    {
        if (paymentReferenceId.StartsWith("fail"))
            return Task.FromResult(new PaymentResult(false));

        if (paymentReferenceId.StartsWith("error"))
            throw new Exception("Payment provider error");

        return Task.FromResult(new PaymentResult(true));
    }
}