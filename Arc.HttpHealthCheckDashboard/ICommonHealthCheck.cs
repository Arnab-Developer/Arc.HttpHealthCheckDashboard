using ArnabDeveloper.HttpHealthCheck;

namespace Arc.HttpHealthCheckDashboard
{
    public interface ICommonHealthCheck
    {
        bool IsApiHealthy(ApiDetail? apiDetail);
    }
}
