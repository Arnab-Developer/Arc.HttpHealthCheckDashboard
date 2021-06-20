using ArnabDeveloper.HttpHealthCheck;
using System.Threading.Tasks;

namespace Arc.HttpHealthCheckDashboard
{
    /// <summary>
    /// Common health check class.
    /// </summary>
    public interface ICommonHealthCheck
    {
        /// <summary>
        /// Check the api is healthy or not.
        /// </summary>
        /// <param name="apiDetail">Api detail to be checked</param>
        /// <returns>
        /// Returns a task object representing true if the endpoint is healthy otherwise false.
        /// </returns>
        Task<bool> IsApiHealthyAsync(ApiDetail? apiDetail);
    }
}
