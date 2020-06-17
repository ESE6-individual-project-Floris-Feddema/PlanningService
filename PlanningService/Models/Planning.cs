using System;
using System.Collections.Generic;

namespace PlanningService.Models
{
    public class Planning
    {
        public Guid id { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<WorkDay> WorksDays { get; set; }
    }
}
