using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using StayBook.Application.Exceptions;
using StayBook.Application.Features.Conversations.Dtos;
using StayBook.Application.Features.Conversations.Queries;
using StayBook.Exceptions;
using StayBook.Extensions;

namespace StayBook.Hubs;

public class ChatHub(IMediator mediator) : Hub
{
    private const string JoinedBookingsContextKey = "StayBook.ChatHub.JoinedBookings";

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
        var userId = GetAuthenticatedUserId();

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

        await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(bookingId), Context.ConnectionAborted);
        lock (Context.Items)
        {
            GetJoinedBookings().Add(bookingId);
        }
    }

    public async Task LeaveConversation(int bookingId)
    {
        GetAuthenticatedUserId();

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(bookingId), Context.ConnectionAborted);
        lock (Context.Items)
        {
            GetJoinedBookings().Remove(bookingId);
        }
    }

    public async Task Typing(int bookingId)
    {
        GetAuthenticatedUserId();
        EnsureJoinedConversation(bookingId);

        await Clients.GroupExcept(GetGroupName(bookingId), Context.ConnectionId)
            .SendAsync("Typing", Context.ConnectionAborted);
    }

    public async Task StopTyping(int bookingId)
    {
        GetAuthenticatedUserId();
        EnsureJoinedConversation(bookingId);

        await Clients.GroupExcept(GetGroupName(bookingId), Context.ConnectionId)
            .SendAsync("StopTyping", Context.ConnectionAborted);
    }

    private int GetAuthenticatedUserId()
    {
        if (Context.User?.Identity?.IsAuthenticated != true)
            throw new HubException("Unauthorized.");

        try
        {
            return Context.User.TryGetCurrentUserId();
        }
        catch (UnauthorizedException)
        {
            throw new HubException("Unauthorized.");
        }
    }

    private void EnsureJoinedConversation(int bookingId)
    {
        lock (Context.Items)
        {
            if (!GetJoinedBookings().Contains(bookingId))
                throw new HubException("Access denied.");
        }
    }

    private HashSet<int> GetJoinedBookings()
    {
        if (Context.Items.TryGetValue(JoinedBookingsContextKey, out var value)
            && value is HashSet<int> joinedBookings)
        {
            return joinedBookings;
        }

        var newJoinedBookings = new HashSet<int>();
        Context.Items[JoinedBookingsContextKey] = newJoinedBookings;
        return newJoinedBookings;
    }

    public static string GetGroupName(int bookingId) => $"conversation:{bookingId}";
}
