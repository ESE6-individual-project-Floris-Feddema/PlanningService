
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PlanningService.Helpers;

namespace PlanningService.Models
{
    public class WorkDay
    {
        public Guid id { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime Date { get; set; }
        public List<AvailabilitySlot> Slots { get; set; }

    }
}
