using System;
using Aspose.Services.Interfaces;
using Aspose.Staff;

namespace Aspose.Services.Salary
{
    public class SalaryService : ISalaryService
    {
        private readonly Func<Employee, BaseSalaryService> salaryServiceFactory;

        public SalaryService(Func<Employee, BaseSalaryService> salaryServiceFactory)
        {
            this.salaryServiceFactory = salaryServiceFactory;
        }

        public decimal GetSalary(Employee employee)
        {
            return salaryServiceFactory(employee).GetSalary(employee);
        }
    }
}
