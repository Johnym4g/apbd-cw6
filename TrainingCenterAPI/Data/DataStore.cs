using TrainingCenterAPI.Models;

namespace TrainingCenterAPI.Data;

public static class DataStore
{
    public static readonly List<Room> Rooms = new()
    {
        new Room { Id = 1, Name = "Lab 101", BuildingCode = "A", Floor = 1, Capacity = 20, HasProjector = true,  IsActive = true  },
        new Room { Id = 2, Name = "Lab 204", BuildingCode = "B", Floor = 2, Capacity = 24, HasProjector = true,  IsActive = true  },
        new Room { Id = 3, Name = "Room 305", BuildingCode = "B", Floor = 3, Capacity = 15, HasProjector = false, IsActive = true  },
        new Room { Id = 4, Name = "Auditorium 1", BuildingCode = "C", Floor = 0, Capacity = 80, HasProjector = true,  IsActive = true  },
        new Room { Id = 5, Name = "Old Room 12", BuildingCode = "A", Floor = 1, Capacity = 10, HasProjector = false, IsActive = false }
    };

    public static readonly List<Reservation> Reservations = new()
    {
        new Reservation { Id = 1, RoomId = 1, OrganizerName = "Anna Kowalska", Topic = "Introduction to REST", Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(9, 0),  EndTime = new TimeOnly(11, 0),  Status = "confirmed" },
        new Reservation { Id = 2, RoomId = 2, OrganizerName = "Jan Nowak",     Topic = "HTTP Deep Dive",       Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(12, 30), Status = "confirmed" },
        new Reservation { Id = 3, RoomId = 3, OrganizerName = "Piotr Wiśniewski", Topic = "Code Review Workshop", Date = new DateOnly(2026, 5, 11), StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(15, 0),  Status = "planned"  },
        new Reservation { Id = 4, RoomId = 4, OrganizerName = "Ewa Zielińska", Topic = "Company All-Hands",    Date = new DateOnly(2026, 5, 12), StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(16, 0),  Status = "planned"  },
        new Reservation { Id = 5, RoomId = 2, OrganizerName = "Marek Lewandowski", Topic = "Testing with Postman", Date = new DateOnly(2026, 5, 13), StartTime = new TimeOnly(9, 0),  EndTime = new TimeOnly(10, 30), Status = "cancelled" }
    };
}
