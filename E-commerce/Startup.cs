using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(E_commerce.Startup))]
namespace E_commerce
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
