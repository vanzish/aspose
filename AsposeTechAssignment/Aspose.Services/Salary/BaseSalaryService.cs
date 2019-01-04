using Aspose.Services.Interfaces;
using Aspose.Staff;
using System;

namespace Aspose.Services.Salary
{
    public class BaseSalaryService
    {
        protected virtual decimal Percent => 3m;

        protected virtual decimal MaxPercent => 30m;

        protected virtual decimal SubordinatesPercent => 0.3m;

        protected readonly ISalaryService salaryService;

        public BaseSalaryService(ISalaryService salaryService)
        {
            this.salaryService = salaryService;
        }

        public virtual decimal GetSalary(Employee employee, DateTime? date = null)
        {
            var years = GetWorkYears(employee.HireDate, date);
            if (years < 0)
            {
                return 0m;
            }

            var percentage = Percent * years > MaxPercent ? MaxPercent : Percent * years;
            return employee.BaseSalary * (1 + percentage / 100);
        }

        private int GetWorkYears(DateTime hireDateTime, DateTime? searchDate = null)
        {
            var now = searchDate ?? DateTime.UtcNow;
            var years = now.Year - hireDateTime.Year;
            if(hireDateTime > now.AddYears(-years))
            {
                years -= 1;
            }

            return years;
        }
    }
}
