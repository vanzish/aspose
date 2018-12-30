using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Services.Interfaces;
using Aspose.Staff;

namespace Aspose.Services.Salary
{
    public class ManagerSalaryService : BaseSalaryService
    {
        protected new const decimal Percent = 5m;
        protected new const decimal MaxPercent = 40m;
        protected new const decimal SubordinatesPercent = 0.5m;

        public ManagerSalaryService(ISalaryService salaryService) : base(salaryService)
        {
        }

        public override decimal GetSalary(Employee employee)
        {
            var manager = (BaseManager)employee;
            var salary = base.GetSalary(manager);

            decimal salariesSum = 0;
            var queue = new Queue<Employee>(manager.Subordinates);
            while (queue.Any())
            {
                var empl = queue.Dequeue();
                salariesSum += GetSalaryFromCache(empl);
            }

            return salary + salariesSum * (SubordinatesPercent / 100);
        }
    }
}
