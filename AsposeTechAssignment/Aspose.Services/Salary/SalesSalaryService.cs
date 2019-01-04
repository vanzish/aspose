using Aspose.Services.Interfaces;
using Aspose.Staff;
using System;

namespace Aspose.Services.Salary
{
    public class SalesSalaryService : BaseSalaryService
    {
        protected override decimal Percent => 1m;

        protected override decimal MaxPercent => 35m;

        protected override decimal SubordinatesPercent => 0.3m;

        public SalesSalaryService(ISalaryService salaryService) : base(salaryService)
        {
        }

        public override decimal GetSalary(Employee employee, DateTime? date = null)
        {
            var manager = (Sales)employee;
            var salary = base.GetSalary(manager, date);
            decimal salariesSum = salaryService.EmployeesSalariesSum(manager.Subordinates, date);
            return salary + salariesSum * (SubordinatesPercent / 100);
        }
    }
}
