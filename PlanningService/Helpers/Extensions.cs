using System;
using System.Linq;
using PlanningService.Exceptions;
using PlanningService.Models;

namespace PlanningService.Helpers
{
    public static class Extensions
    {
        public static void Validate(this Planning planning)
        {
            if (planning.id == Guid.Empty)
                throw new ValidationException("The generated id is empty");
            if (string.IsNullOrEmpty(planning.CompanyId)) 
                throw new ValidationException("The companyId is empty");
            if (planning.Name.Length == 0)
                throw new ValidationException("The name cannot be empty");
            if (planning.StartDate < DateTime.Now)
                throw new ValidationException("The start date can not be before today");
            if (planning.EndDate < DateTime.Now)
                throw new ValidationException("The end date can not be before today");
            if (planning.EndDate < planning.StartDate)
                throw new ValidationException("The end date can not be before the start date");
            if (planning.WorkDays.Count == 0)
                throw new ValidationException("The there needs to be at least 1 workday");
            foreach (var day in planning.WorkDays)
            {
                if (day.id == Guid.Empty)
                    throw new ValidationException("The generated workday id is empty");
                if (day.Date < planning.StartDate || day.Date > planning.EndDate)
                    throw new ValidationException("The workdays need to be between the start and end");
                if (planning.WorkDays.Any(p => p.id != day.id && p.Date.Date == day.Date.Date))
                    throw new ValidationException("There cannot be 2 workdays on the same day");
            }
        }
    }
}
