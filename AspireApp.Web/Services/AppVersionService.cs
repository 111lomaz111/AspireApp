using System.Reflection;

namespace AspireApp.Web.Services
{
    public interface IAppVersionService
    {
        string Version { get; }
    }

    public class AppVersionService : IAppVersionService
    {
        public string Version =>
            typeof(AppVersionService).Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion
            ?? "unknown";
    }
}
