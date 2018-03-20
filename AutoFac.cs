using Autofac;
using Autofac.Extras.DynamicProxy;

namespace spell_check
{
    public class AutoFac
    {
        // Using the TYPED attribute:
        public IContainer AutoFacInit()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Spell>().EnableClassInterceptors().InterceptedBy(typeof(LoggerInterceptor));
            builder.Register(c => new LoggerInterceptor());
            var container = builder.Build();
            return container;
        }
    }
}