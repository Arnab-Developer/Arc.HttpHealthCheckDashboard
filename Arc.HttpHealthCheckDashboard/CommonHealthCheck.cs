using ArnabDeveloper.HttpHealthCheck;

namespace Arc.HttpHealthCheckDashboard
{
    /// <inheritdoc cref="ICommonHealthCheck"/>
    public class CommonHealthCheck : ICommonHealthCheck
    {
        private readonly IHealthCheck _healthCheck;

        /// <summary>
        /// Creates a new object of CommonHealthCheck class.
        /// </summary>
        /// <param name="healthCheck">IHealthCheck type object</param>
        public CommonHealthCheck(IHealthCheck healthCheck)
        {
            _healthCheck = healthCheck;
        }

        async Task<bool> ICommonHealthCheck.IsApiHealthyAsync(ApiDetail? apiDetail)
        {
            if (apiDetail == null)
            {
                return false;
            }
            try
            {
                bool isApiHealthy = false;
                if (apiDetail.ApiCredential is null ||
                    string.IsNullOrWhiteSpace(apiDetail.ApiCredential.UserName) ||
                    string.IsNullOrWhiteSpace(apiDetail.ApiCredential.Password))
                {
                    isApiHealthy = await _healthCheck.IsHealthyAsync(apiDetail.Url);
                }
                else
                {
                    isApiHealthy = await _healthCheck.IsHealthyAsync(apiDetail.Url, apiDetail.ApiCredential);
                }
                return isApiHealthy;
            }
            catch
            {
                return false;
            }
        }
    }
}
