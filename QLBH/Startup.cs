using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QLBH.Startup))]
namespace QLBH
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
