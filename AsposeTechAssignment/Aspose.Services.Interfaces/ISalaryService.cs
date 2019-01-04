using Aspose.Staff;
using System;
using System.Collections.Generic;

namespace Aspose.Services.Interfaces
{
    public interface ISalaryService
    {
        decimal GetSalary(Employee employee, DateTime? date = null);

        decimal GetAllSalaries(IEnumerable<Employee> employees, DateTime? date = null);

        decimal EmployeesSalariesSum(IEnumerable<Employee> employees, DateTime? date = null);
    }
}
