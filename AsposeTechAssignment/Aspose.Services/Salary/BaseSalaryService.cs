using System;
using System.Collections.Generic;
using Aspose.Services.Interfaces;
using Aspose.Staff;

namespace Aspose.Services.Salary
{
    public class BaseSalaryService
    {
        protected const decimal Percent = 3m;
        protected const decimal MaxPercent = 30m;
        protected const decimal SubordinatesPercent = 0.3m;

        protected readonly ISalaryService salaryService;

        private readonly Dictionary<int, decimal> salaryCache = new Dictionary<int, decimal>();

        public BaseSalaryService(ISalaryService salaryService)
        {
            this.salaryService = salaryService;
        }

        public virtual decimal GetSalary(Employee employee)
        {
            var years = GetWorkYears(employee.HireDate);
            var percentage = Percent * years > MaxPercent ? MaxPercent : Percent * years;
            return employee.BaseSalary * (1 + percentage / 100);
        }

        protected decimal GetSalaryFromCache(Employee employee)
        {
            if (salaryCache.TryGetValue(employee.Id, out decimal result))
            {
                return result;
            }

            result = salaryService.GetSalary(employee);
            salaryCache[employee.Id] = result;
            return result;
        }

        private int GetWorkYears(DateTimeOffset dateTime)
        {
            return DateTime.UtcNow.Year - dateTime.UtcDateTime.Year;
        }
    }
}
