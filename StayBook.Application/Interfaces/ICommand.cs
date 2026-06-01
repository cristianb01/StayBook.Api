using MediatR;

namespace StayBook.Application.Interfaces;

public interface ICommand<out TResponse>
    : IRequest<int>
{
}