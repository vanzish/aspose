using Aspose.IoC;
using Aspose.Services.Interfaces;
using Aspose.Staff;
using Autofac;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Aspose.Tests
{
    [TestFixture]
    public class SalaryTests
    {
        private IContainer container;
        private ISalaryService salaryService;
        private ICacheService cacheService;
        private const decimal BaseSalary = 1000m;

        #region Employee

        [Test]
        public void ZeroYearSalaryPercantageTest()
        {
            var zeroYearEmployee = new Employee
            {
                Id = 0,
                BaseSalary = BaseSalary,
                HireDate = DateTime.UtcNow.AddMonths(-1),
                Name = "Zero"
            };

            var zeroSalary = salaryService.GetSalary(zeroYearEmployee);
            Assert.AreEqual(BaseSalary, zeroSalary);
        }

        [Test]
        public void ArbitraryYearSalaryPercantageTest()
        {
            var employee = new Employee
            {
                Id = 0,
                BaseSalary = BaseSalary,
                HireDate = DateTime.UtcNow.AddYears(-15).AddMonths(-1),
                Name = "Employee"
            };

            // get salary in 1 year and 4 months after hire date.
            // 1000 * (1 + 1 * 3 / 100) = 1030
            var salary = salaryService.GetSalary(employee, DateTime.UtcNow.AddYears(-14).AddMonths(2));
            Assert.AreEqual(1030, salary);

            TearDown();

            //get salary in a year from now. it will be 16 years of work
            salary = salaryService.GetSalary(employee, DateTime.UtcNow.AddYears(1).AddMonths(2));
            Assert.AreEqual(1300, salary);

            TearDown();

            //get salary in a year before hire date.
            salary = salaryService.GetSalary(employee, DateTime.UtcNow.AddYears(-16).AddMonths(-2));
            Assert.AreEqual(0, salary);
        }

        [Test]
        public void EmployeeSalaryPercantageTest()
        {
            var employee = new Employee()
            {
                Id = 0,
                BaseSalary = BaseSalary,
                HireDate = DateTime.UtcNow.AddYears(-5).AddMonths(1),
                Name = "Employee"
            };

            var employeeSalary = salaryService.GetSalary(employee);
            Assert.AreEqual(BaseSalary * (1m + 4m * 3m / 100m), employeeSalary);
            TearDown();

            employee.HireDate = DateTime.UtcNow.AddYears(-11).AddMonths(-2);
            employeeSalary = salaryService.GetSalary(employee);
            Assert.AreEqual(BaseSalary * (1m + 30m / 100m), employeeSalary);
        }
        #endregion

        #region Sales

        [Test]
        public void SalesSalaryPercantageTest()
        {
            var salesEmployee = new Sales()
            {
                Id = 0,
                BaseSalary = BaseSalary,
                HireDate = DateTime.UtcNow.AddYears(-15).AddMonths(1),
                Name = "Sales"
            };

            var salesSalary = salaryService.GetSalary(salesEmployee);
            Assert.AreEqual(BaseSalary * (1m + 14m * 1m / 100m), salesSalary);

            TearDown();

            salesEmployee.HireDate = DateTime.UtcNow.AddYears(-36).AddMonths(-2);
            salesSalary = salaryService.GetSalary(salesEmployee);
            Assert.AreEqual(BaseSalary * (1m + 35m / 100m), salesSalary);
        }

        [Test]
        public void SalesDeepHierarchyTest()
        {
            var sales = new Sales
            {
                Id = 1,
                BaseSalary = BaseSalary,
                HireDate = DateTime.UtcNow.AddYears(-10).AddMonths(2),
                Name = "Sales",
            };

            var manager = new Manager
            {
                Id = 2,
                BaseSalary = BaseSalary,
                HireDate = DateTime.UtcNow.AddYears(-10).AddMonths(-5),
                Name = "Manager",
                Supervisor = sales
            };

            var salesNewbie = new Sales
            {
                Id = 3,
                BaseSalary = BaseSalary,
                HireDate = DateTime.UtcNow.AddYears(-2).AddMonths(-2),
                Name = "Newbie",
                Supervisor = manager
            };

            sales.Subordinates.Add(manager);
            manager.Subordinates.Add(salesNewbie);

            // newbieSalary: 1000 * (1 + 2 * 1 / 100) = 1020

            // managerSalary: 1000 * (1 + 40 / 100) + 0.5 / 100 * 1020 = 1405.1

            // 1000 * (1 + 9 * 1 / 100) + 0.3 / 100 * (1405.1 + 1020) = 1090 + 7.2753 = 1097.2753
            var salesSalary = salaryService.GetSalary(sales);

            Assert.AreEqual(salesSalary, 1097.2753m);
        }

        [Test]
        public void SalesSimpleHierarchyTest()
        {
            var sales = new Sales
            {
                Id = 1,
                BaseSalary = BaseSalary,
                HireDate = DateTime.UtcNow.AddYears(-10).AddMonths(2),
                Name = "Sales",
            };

            var employee = new Employee
            {
                Id = 2,
                BaseSalary = BaseSalary,
                HireDate = DateTime.UtcNow.AddYears(-10).AddMonths(-5),
                Name = "Employee",
                Supervisor = sales
            };

            sales.Subordinates.Add(employee);

            // employee: 1000 * (1 + 30 / 100) = 1300

            // 1000 * (1 + 9 * 1 / 100) + 0.3 / 100 * 1300 = 1093.9
            var salesSalary = salaryService.GetSalary(sales);

            Assert.AreEqual(salesSalary, 1093.9m);
        }

        #endregion

        #region Manager

        [Test]
        public void ManagerSalaryPercantageTest()
        {
            var manager = new Manager
            {
                Id = 0,
                BaseSalary = BaseSalary,
                HireDate = DateTime.UtcNow.AddYears(-7).AddMonths(1),
                Name = "Manager"
            };

            var managerSalary = salaryService.GetSalary(manager);
            Assert.AreEqual(BaseSalary * (1m + 6m * 5m / 100m), managerSalary);

            TearDown();

            manager.HireDate = DateTime.UtcNow.AddYears(-8).AddMonths(-2);
            managerSalary = salaryService.GetSalary(manager);
            Assert.AreEqual(BaseSalary * (1m + 40m / 100m), managerSalary);
        }

        [Test]
        public void ManagerHierarchyTest()
        {
            // 1000 * (1 + 40 / 100) = 1400
            var manager = new Manager
            {
                Id = 1,
                BaseSalary = BaseSalary,
                HireDate = DateTime.UtcNow.AddYears(-10).AddMonths(-2),
                Name = "Manager",
            };

            // 1000 * (1 + 35 / 100) = 1350
            // salesSalary = 1350 + 0.3 / 100 * 1090 = 1353.27
            var sales = new Sales
            {
                Id = 2,
                BaseSalary = BaseSalary,
                HireDate = DateTime.UtcNow.AddYears(-38).AddMonths(-2),
                Name = "Sales",
                Supervisor = manager
            };

            // 1000 * (1 + 3 * 3 / 100) = 1090
            var employee = new Employee
            {
                Id = 3,
                BaseSalary = BaseSalary,
                HireDate = DateTime.UtcNow.AddYears(-3).AddMonths(-4),
                Supervisor = sales,
                Name = "Employee"
            };

            // 1000 * (1 + 30 / 100) = 1300
            var employee1 = new Employee
            {
                Id = 4,
                BaseSalary = BaseSalary,
                HireDate = DateTime.UtcNow.AddYears(-11).AddMonths(-2),
                Supervisor = manager,
                Name = "Employee1"
            };

            manager.Subordinates.Add(sales);
            manager.Subordinates.Add(employee1);
            sales.Subordinates.Add(employee);

            // 1400 + 0.5 / 100 * (1353.27 + 1300) = 1413.26635
            var managerSalary = salaryService.GetSalary(manager);
            Assert.AreEqual(1413.26635m, managerSalary);
        }

        #endregion

        #region All

        [Test]
        public void AllSalariesSumTest()
        {
            var staff = Init();
            var result = salaryService.GetAllSalaries(staff);
            // 1413.26635 + 1353.27 + 1090 + 1300 = 5156.53635
            Assert.AreEqual(5156.53635m, result);
        }

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var builder = ContainerConfig.Register();
            container = builder.Build();
            salaryService = container.Resolve<ISalaryService>();
            cacheService = container.Resolve<ICacheService>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            container.Dispose();
        }

        [TearDown]
        public void TearDown()
        {
            cacheService.GetCache().Dispose();
        }

        private static List<Employee> Init()
        {
            // 1000 * (1 + 40 / 100) = 1400
            // manager salary = 1400 + 0.5 / 100 * (1353.27 + 1300) = 1413.26635
            var manager = new Manager
            {
                Id = 1,
                BaseSalary = BaseSalary,
                HireDate = DateTime.UtcNow.AddYears(-10).AddMonths(-2),
                Name = "Manager",
            };

            // 1000 * (1 + 35 / 100) = 1350
            // salesSalary = 1350 + 0.3 / 100 * 1090 = 1353.27
            var sales = new Sales
            {
                Id = 2,
                BaseSalary = BaseSalary,
                HireDate = DateTime.UtcNow.AddYears(-38).AddMonths(-2),
                Name = "Sales",
                Supervisor = manager
            };

            // 1000 * (1 + 3 * 3 / 100) = 1090
            var employee = new Employee
            {
                Id = 3,
                BaseSalary = BaseSalary,
                HireDate = DateTime.UtcNow.AddYears(-3).AddMonths(-4),
                Supervisor = sales,
                Name = "Employee"
            };

            // 1000 * (1 + 30 / 100) = 1300
            var employee1 = new Employee
            {
                Id = 4,
                BaseSalary = BaseSalary,
                HireDate = DateTime.UtcNow.AddYears(-11).AddMonths(-2),
                Supervisor = manager,
                Name = "Employee1"
            };

            manager.Subordinates.Add(sales);
            manager.Subordinates.Add(employee1);
            sales.Subordinates.Add(employee);

            return new List<Employee> { manager, sales, employee1, employee };
        }

        #endregion
    }
}
