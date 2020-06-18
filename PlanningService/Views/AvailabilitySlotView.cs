using System;
using PlanningService.Models;

namespace PlanningService.Views
{
    public class AvailabilitySlotView
    {
        public Guid WorkDayId { get; set; }
        public User User { get; set; }
    }
}
