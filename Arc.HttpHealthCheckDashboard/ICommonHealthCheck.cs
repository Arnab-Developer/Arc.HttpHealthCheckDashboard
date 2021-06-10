using ArnabDeveloper.HttpHealthCheck;

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
        /// <returns>Returns true if the api is healthy otherwise false</returns>
        bool IsApiHealthy(ApiDetail? apiDetail);
    }
}
