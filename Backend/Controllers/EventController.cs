using System.Data.Common;
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public EventController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEvents()
        {
            if (await _dataContext.Events.FindAsync() == null)
            {
                return NotFound("No events found in the database.");
            }

            var events = await _dataContext.Events.ToListAsync();
            return Ok(events);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[Authorize(Roles = "Admin)]
        public async Task<IActionResult> CreateEvent([FromBody] Event events)
        {
        if (events == null) {
            return BadRequest();    
        }

            _dataContext.Events.Add(events);
            await _dataContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvents), new { id = events.EventId }, events);
        }


        [HttpPatch("{eventId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateEvent(int eventId, [FromBody] Event updatedEvent)
        {
            if (updatedEvent == null)
            {
                return BadRequest();
            }

            var events = await _dataContext.Events.FindAsync(eventId);
            if (events == null)
            {
                return NotFound("No event found with that ID");
            }

            var existing = await _dataContext.Events.FindAsync(eventId);
            if (existing == null)
            {
                return NotFound("No event with that ID was found.");
            }

            existing.Title = updatedEvent.Title;
            existing.Description = updatedEvent.Description;
            existing.ImageUrl = updatedEvent.ImageUrl;
            existing.Date = updatedEvent.Date;
            existing.Location = updatedEvent.Location;
            
            await _dataContext.SaveChangesAsync();

            return Ok(existing);
        }

    }
}