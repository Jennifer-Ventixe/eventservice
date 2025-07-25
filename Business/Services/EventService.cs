﻿using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Repositories;

namespace Business.Services;

public class EventService(IEventRepository eventRepository) : IEventService
{
    private readonly IEventRepository _eventRepository = eventRepository;

    public async Task<EventResult> CreateEventAsync(CreateEventRequest request)
    {
        try
        {
            var eventEntity = new EventEntity
            {
                Image = request.Image,
                Title = request.Title,
                EventDate = request.EventDate,
                Location = request.Location,
                Description = request.Description,
                Price = request.Price,
                Currency = request.Currency
            };

            var result = await _eventRepository.AddAsync(eventEntity);
            return result.Success
                ? new EventResult { Success = true }
                : new EventResult { Success = false, Error = result.Error };
        }

        catch (Exception ex)
        {
            return new EventResult
            {
                Success = false,
                Error = ex.Message
            };
        }
    }

    public async Task<EventResult<IEnumerable<Event>>> GetEventsAsync()
    {
        var result = await _eventRepository.GetAllAsync();
        var events = result.Result?.Select(x => new Event
        {
            id = x.Id,
            Image = x.Image,
            Title = x.Title,
            EventDate = x.EventDate,
            Location = x.Location,
            Description = x.Description,
            Price = x.Price,
            Currency = x.Currency

        });

        return new EventResult<IEnumerable<Event>> { Success = true, Result = events };
    }


    public async Task<EventResult<Event?>> GetEventAsync(string eventId)
    {
        var result = await _eventRepository.GetAsync(x => x.Id == eventId);
        if (result.Success && result.Result != null)
        {
            var currentEvent = new Event
            {
                id = result.Result.Id,
                Image = result.Result.Image,
                Title = result.Result.Title,
                EventDate = result.Result.EventDate,
                Location = result.Result.Location,
                Description = result.Result.Description,
                Price = result.Result.Price,
                Currency = result.Result.Currency

            };
            return new EventResult<Event?> { Success = true, Result = currentEvent };
        }

        return new EventResult<Event?> { Success = false, Error = result.Error ?? "Event not found." };

    }
}
