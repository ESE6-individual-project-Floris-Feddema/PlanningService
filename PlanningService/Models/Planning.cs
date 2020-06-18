using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PlanningService.Helpers;

namespace PlanningService.Models
{
    public class Planning
    {
        public Guid id { get; set; }
        public string CompanyId { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime StartDate { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime EndDate { get; set; }
        public List<WorkDay> WorkDays { get; set; }
    }
}
