using Microsoft.AspNetCore.Mvc;
using TrainingCenterAPI.Data;
using TrainingCenterAPI.Models;

namespace TrainingCenterAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Room>> GetAll(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
    {
        IEnumerable<Room> result = DataStore.Rooms;

        if (minCapacity.HasValue)
            result = result.Where(r => r.Capacity >= minCapacity.Value);

        if (hasProjector.HasValue)
            result = result.Where(r => r.HasProjector == hasProjector.Value);

        if (activeOnly == true)
            result = result.Where(r => r.IsActive);

        return Ok(result.ToList());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Room> GetById(int id)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
            return NotFound();

        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public ActionResult<IEnumerable<Room>> GetByBuilding(string buildingCode)
    {
        var rooms = DataStore.Rooms
            .Where(r => string.Equals(r.BuildingCode, buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(rooms);
    }

    [HttpPost]
    public ActionResult<Room> Create([FromBody] Room room)
    {
        var nextId = DataStore.Rooms.Count == 0 ? 1 : DataStore.Rooms.Max(r => r.Id) + 1;
        room.Id = nextId;
        DataStore.Rooms.Add(room);

        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Room> Update(int id, [FromBody] Room room)
    {
        var existing = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (existing is null)
            return NotFound();

        existing.Name = room.Name;
        existing.BuildingCode = room.BuildingCode;
        existing.Floor = room.Floor;
        existing.Capacity = room.Capacity;
        existing.HasProjector = room.HasProjector;
        existing.IsActive = room.IsActive;

        return Ok(existing);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
            return NotFound();

        var hasReservations = DataStore.Reservations.Any(r => r.RoomId == id);
        if (hasReservations)
            return Conflict("Cannot delete a room that has reservations");

        DataStore.Rooms.Remove(room);
        return NoContent();
    }
}
