﻿using System.Collections.Generic;
using System.Linq;
using Aspose.Services.Interfaces;
using Aspose.Staff;

namespace Aspose.Services.Salary
{
    public class SalesSalaryService : BaseSalaryService
    {
        protected new const decimal Percent = 1m;
        protected new const decimal MaxPercent = 35m;
        protected new const decimal SubordinatesPercent = 0.3m;

        public SalesSalaryService(ISalaryService salaryService) : base(salaryService)
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
                if (empl.StaffType != StaffType.Employee)
                {
                    foreach (var subordinate in ((BaseManager)empl).Subordinates)
                    {
                        queue.Enqueue(subordinate);
                    }
                }

                salariesSum += GetSalaryFromCache(empl);
            }

            return salary + salariesSum * (SubordinatesPercent / 100);
        }
    }
}