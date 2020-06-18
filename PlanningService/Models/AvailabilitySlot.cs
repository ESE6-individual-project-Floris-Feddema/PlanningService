using System;

namespace PlanningService.Models
{
    public class AvailabilitySlot
    {
        public Guid id { get; set; }
        public User User { get; set; }
    }
}
