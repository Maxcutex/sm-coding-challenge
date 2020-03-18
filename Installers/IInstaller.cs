using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace sm_coding_challenge.Installers
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}