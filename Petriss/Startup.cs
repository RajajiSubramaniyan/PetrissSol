using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Petriss.Startup))]
namespace Petriss
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
         // ConfigureAuth(app);
        }
    }
}
