using Arc.HttpHealthCheckDashboard;
using ArnabDeveloper.HttpHealthCheck;

namespace Arc.HttpHealthCheckDashboardTests;

public class Test1CustomMatchHealthCheck : BaseHealthCheck
{
    public Test1CustomMatchHealthCheck(IEnumerable<ApiDetail> urlDetails, ICommonHealthCheck commonHealthCheck)
        : base(urlDetails, commonHealthCheck)
    {
    }

    protected override Predicate<ApiDetail> GetMatch()
    {
        int indexOfHealthCheck = GetType().Name.IndexOf("CustomMatchHealthCheck");
        string apiNameToTest = GetType().Name.Substring(0, indexOfHealthCheck);
        return new Predicate<ApiDetail>(u => u.Name == apiNameToTest && u.IsEnable);
    }
}