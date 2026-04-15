using Microsoft.AspNetCore.Mvc;
using TrainingCenterAPI.Data;
using TrainingCenterAPI.Models;

namespace TrainingCenterAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> GetAll(
        [FromQuery] DateOnly? date,
        [FromQuery] string? status,
        [FromQuery] int? roomId)
    {
        IEnumerable<Reservation> result = DataStore.Reservations;

        if (date.HasValue)
            result = result.Where(r => r.Date == date.Value);

        if (!string.IsNullOrWhiteSpace(status))
            result = result.Where(r => string.Equals(r.Status, status, StringComparison.OrdinalIgnoreCase));

        if (roomId.HasValue)
            result = result.Where(r => r.RoomId == roomId.Value);

        return Ok(result.ToList());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Reservation> GetById(int id)
    {
        var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null)
            return NotFound();

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult<Reservation> Create([FromBody] Reservation reservation)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
        if (room is null)
            return NotFound($"Room with id {reservation.RoomId} doesn't exist");

        if (!room.IsActive)
            return Conflict($"Room with id {reservation.RoomId} isn't active");

        var overlapping = DataStore.Reservations.Any(r =>
            r.RoomId == reservation.RoomId &&
            r.Date == reservation.Date &&
            r.StartTime < reservation.EndTime &&
            reservation.StartTime < r.EndTime);

        if (overlapping)
            return Conflict("The room is already reserved in this time range");

        var nextId = DataStore.Reservations.Count == 0 ? 1 : DataStore.Reservations.Max(r => r.Id) + 1;
        reservation.Id = nextId;
        DataStore.Reservations.Add(reservation);

        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Reservation> Update(int id, [FromBody] Reservation reservation)
    {
        var existing = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (existing is null)
            return NotFound();

        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
        if (room is null)
            return NotFound($"Room with id {reservation.RoomId} doesn't exist");

        if (!room.IsActive)
            return Conflict($"Room with id {reservation.RoomId} isn't active");

        var overlapping = DataStore.Reservations.Any(r =>
            r.Id != id &&
            r.RoomId == reservation.RoomId &&
            r.Date == reservation.Date &&
            r.StartTime < reservation.EndTime &&
            reservation.StartTime < r.EndTime);

        if (overlapping)
            return Conflict("The room is already reserved in this time range");

        existing.RoomId = reservation.RoomId;
        existing.OrganizerName = reservation.OrganizerName;
        existing.Topic = reservation.Topic;
        existing.Date = reservation.Date;
        existing.StartTime = reservation.StartTime;
        existing.EndTime = reservation.EndTime;
        existing.Status = reservation.Status;

        return Ok(existing);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null)
            return NotFound();

        DataStore.Reservations.Remove(reservation);
        return NoContent();
    }
}
