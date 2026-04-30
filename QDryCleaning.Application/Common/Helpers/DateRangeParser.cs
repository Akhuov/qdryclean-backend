using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDryClean.Application.Common.Helpers
{
    public static class DateRangeParser
    {
        public static (DateTime? from, DateTime? to, string? error) Parse(string? from, string? to)
        {
            DateTime? fromDate = null;
            DateTime? toDate = null;

            if (!string.IsNullOrWhiteSpace(from))
            {
                if (!DateTime.TryParse(from, out var parsedFrom))
                    return (null, null, "Invalid 'From' date format.");

                fromDate = parsedFrom.Date;
            }

            if (!string.IsNullOrWhiteSpace(to))
            {
                if (!DateTime.TryParse(to, out var parsedTo))
                    return (null, null, "Invalid 'To' date format.");

                toDate = parsedTo.Date.AddDays(1).AddTicks(-1);
            }

            if (fromDate.HasValue && toDate.HasValue && fromDate > toDate)
            {
                return (null, null, "'From' date cannot be greater than 'To' date.");
            }

            return (fromDate, toDate, null);
        }
    }
}
