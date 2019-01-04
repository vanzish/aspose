using Aspose.Services.Interfaces;
using Aspose.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Aspose.Services.Salary
{
    public class SalaryService : ISalaryService
    {
        private readonly Func<Employee, BaseSalaryService> salaryServiceFactory;
        private readonly MemoryCache cache;

        public SalaryService(
            Func<Employee, BaseSalaryService> salaryServiceFactory,
            ICacheService cacheService)
        {
            this.salaryServiceFactory = salaryServiceFactory;
            this.cache = cacheService.GetCache();
        }

        /// <summary>
        /// Calculates salary of the employee.
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public decimal GetSalary(Employee employee, DateTime? date = null)
        {
            var id = employee.Id.ToString();
            if(cache.Contains(id))
            {
                // Exception handling must be performed on the using side. Here try/catch will reduce performance.
                return (decimal)cache.Get(id);
            }

            var result = salaryServiceFactory(employee).GetSalary(employee, date);
            cache.Set(id, result, DateTimeOffset.UtcNow.AddDays(1));
            return result;
        }

        // Takes list of employees. If there would be a repository staff should be taken out of repository.
        /// <summary>
        /// Calculates sum of salaries of all company staff.
        /// </summary>
        /// <param name="employees"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public decimal GetAllSalaries(IEnumerable<Employee> employees, DateTime? date = null)
        {
            var noReportingEmployees = employees.Where(x => x.Supervisor == null);
            var result = 0m;
            foreach(var employee in noReportingEmployees)
            {
                result += GetSalary(employee, date);
                if(employee.StaffType != StaffType.Employee)
                {
                    result += EmployeesSalariesSum(((BaseManager)employee).Subordinates);
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates sum of list of employees.
        /// </summary>
        /// <param name="employees"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public decimal EmployeesSalariesSum(IEnumerable<Employee> employees, DateTime? date = null)
        {
            decimal salariesSum = 0;
            var queue = new Queue<Employee>(employees);
            while(queue.Any())
            {
                var empl = queue.Dequeue();
                if(empl.StaffType != StaffType.Employee)
                {
                    foreach(var subordinate in ((BaseManager)empl).Subordinates)
                    {
                        queue.Enqueue(subordinate);
                    }
                }

                salariesSum += GetSalary(empl, date);
            }

            return salariesSum;
        }
    }
}
