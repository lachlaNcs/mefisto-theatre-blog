using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(M_McMillan_Assessment_2.Startup))]
namespace M_McMillan_Assessment_2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
