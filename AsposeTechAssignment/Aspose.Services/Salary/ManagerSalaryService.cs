using Aspose.Services.Interfaces;
using Aspose.Staff;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aspose.Services.Salary
{
    public class ManagerSalaryService : BaseSalaryService
    {
        protected override decimal Percent => 5m;

        protected override decimal MaxPercent => 40m;

        protected override decimal SubordinatesPercent => 0.5m;

        public ManagerSalaryService(ISalaryService salaryService) : base(salaryService)
        {
        }

        public override decimal GetSalary(Employee employee, DateTime? date = null)
        {
            var manager = (Manager)employee;
            var salary = base.GetSalary(manager, date);

            decimal salariesSum = 0;
            var queue = new Queue<Employee>(manager.Subordinates);
            while(queue.Any())
            {
                var empl = queue.Dequeue();
                salariesSum += salaryService.GetSalary(empl, date);
            }

            return salary + salariesSum * (SubordinatesPercent / 100);
        }
    }
}
