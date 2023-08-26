using Microsoft.Extensions.DependencyInjection;

namespace ColorMixer.Contracts.DependecyInjection
{
    /// <summary>
    /// Composition node. 
    /// </summary>
    public interface IAppModule
    {
        /// <summary>
        /// Registers implementations of internal services.
        /// </summary>
        /// <param name="services">Application services descriptors.</param>
        public void ConfigureServices(IServiceCollection services);
    }
}
