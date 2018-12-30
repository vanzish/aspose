using Autofac;

namespace Aspose.IoC
{
    public class ContainerConfig
    {
        public static ContainerBuilder Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ServiceModule());
            return builder;
        }
    }
}
