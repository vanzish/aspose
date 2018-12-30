using System;
using Aspose.Services;
using Aspose.Services.Interfaces;
using Aspose.Services.Salary;
using Aspose.Staff;
using Autofac;

namespace Aspose.IoC
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(AssemblyHolder).Assembly)
                .Where(x => x.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            builder
                .RegisterAssemblyTypes(typeof(AssemblyHolder).Assembly)
                .Where(x => x.IsSubclassOf(typeof(BaseSalaryService)))
                .AsSelf()
                .InstancePerLifetimeScope();
            builder
                .RegisterType<BaseSalaryService>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder
                .Register<Func<Employee, BaseSalaryService>>(context =>
                    {
                        var ctx = context.Resolve<IComponentContext>();
                        return employee =>
                        {
                            switch(employee.StaffType)
                            {
                                case StaffType.Manager:
                                    return ctx.Resolve<ManagerSalaryService>();
                                case StaffType.Sales:
                                    return ctx.Resolve<SalesSalaryService>();
                                default:
                                    return ctx.Resolve<BaseSalaryService>();

                            }
                        };
                    }
                )
                .InstancePerLifetimeScope();
        }
    }
}
