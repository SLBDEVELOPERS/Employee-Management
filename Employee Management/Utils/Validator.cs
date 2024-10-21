using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class Validator
    {
        public static bool IsValidSalary(string salaryText)
        {
            return decimal.TryParse(salaryText, out _);
        }

        public static bool IsFieldEmpty(string fieldText)
        {
            return string.IsNullOrWhiteSpace(fieldText);
        }

        public static bool IsValidDate(DateTime date)
        {
            return date <= DateTime.Now; // Ensure the date of hire is not in the future
        }
    }
}
