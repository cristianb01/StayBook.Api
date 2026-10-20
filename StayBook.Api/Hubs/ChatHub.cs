using System.Text.RegularExpressions;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using StayBook.Application.Exceptions;
using StayBook.Application.Features.Bookings.Queries;
using StayBook.Application.Features.Conversations.Dtos;
using StayBook.Application.Features.Conversations.Queries;
using StayBook.Exceptions;
using StayBook.Extensions;

namespace StayBook.Hubs;

public class ChatHub(IMediator mediator) : Hub
{
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"{Context.ConnectionId} connected");
        await base.OnConnectedAsync();
    }
    
    public async Task SendTestMessage()
    {
        await Clients.All.SendAsync("ReceiveMessage");
    }

    public async Task JoinConversation(int bookingId)
    {
        if (Context.User?.Identity?.IsAuthenticated != true)
            throw new HubException("Unauthorized.");

        int userId;
        try
        {
            userId = Context.User.TryGetCurrentUserId();
        }
        catch (UnauthorizedException)
        {
            throw new HubException("Unauthorized.");
        }

        BookingConversationDto bookingConversation;
        try
        {
            bookingConversation = await mediator.Send(new GetConversationBybookingIdQuery(bookingId, userId), Context.ConnectionAborted);
        }
        catch (ResourceNotFoundException)
        {
            throw new HubException("Booking not found.");
        }
        catch (UnauthorizedConversationAccessException)
        {
            throw new HubException("You are not a member of this booking.");
        }

        if (bookingConversation.Conversation is null)
            throw new HubException("Conversation not found.");

        await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(bookingConversation.Conversation.Id), Context.ConnectionAborted);
    }

    public async Task LeaveConversation(int bookingId)
    {
        if (Context.User?.Identity?.IsAuthenticated != true) 
            throw new HubException("Unauthorized.");
        
        int userId;
        try
        {
            userId = Context.User.TryGetCurrentUserId();
        }
        catch (UnauthorizedException)
        {
            throw new HubException("Unauthorized.");
        }

        try
        {
            var isMember = await mediator.Send(new IsBookingMemberQuery(bookingId, userId));
            
            if (!isMember)
            {
                 throw new HubException("Access denied.");               
            }
            
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(bookingId), Context.ConnectionAborted);
        }
        catch (ResourceNotFoundException)
        {
            throw new HubException("Booking not found.");
        }
    }

    public async Task Typing(int bookingId)
    {
        if (Context.User?.Identity?.IsAuthenticated != true)
            throw new HubException("Unauthorized.");

        int userId;
        try
        {
            userId = Context.User.TryGetCurrentUserId();
        }
        catch (UnauthorizedException)
        {
            throw new HubException("Unauthorized.");
        }

        try
        {
            var isMember = await mediator.Send(new IsBookingMemberQuery(bookingId, userId));
            
            if (!isMember)
            {
                throw new HubException("Access denied.");               
            }
            
            await Clients.GroupExcept(GetGroupName(bookingId), Context.ConnectionId)
                .SendAsync("Typing", Context.ConnectionAborted);
        }
        catch (ResourceNotFoundException)
        {
            throw new HubException("Booking not found.");
        }
    }

    public async Task StopTyping(int bookingId)
    {
        if (Context.User?.Identity?.IsAuthenticated != true) 
            throw new HubException("Unauthorized.");
        
        int userId;
        try
        {
            userId = Context.User.TryGetCurrentUserId();
        }
        catch (UnauthorizedException) 
        {
            throw new HubException("Unauthorized.");
        }
        
        try
        {
            var isMember = await mediator.Send(new IsBookingMemberQuery(bookingId, userId));
            
            if (!isMember)
            {
                throw new HubException("Access denied.");               
            }
            
            await Clients.GroupExcept(GetGroupName(bookingId), Context.ConnectionId)
                .SendAsync("StopTyping", Context.ConnectionAborted);
        }
        catch (ResourceNotFoundException)
        {
            throw new HubException("Booking not found.");
        }
    }
    
    public static string GetGroupName(int bookingId) => $"conversation:{bookingId}";
}
