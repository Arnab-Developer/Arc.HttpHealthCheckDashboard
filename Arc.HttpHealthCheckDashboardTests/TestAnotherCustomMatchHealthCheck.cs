using Arc.HttpHealthCheckDashboard;
using ArnabDeveloper.HttpHealthCheck;

namespace Arc.HttpHealthCheckDashboardTests;

public class TestAnotherCustomMatchHealthCheck : BaseHealthCheck
{
    public TestAnotherCustomMatchHealthCheck(IEnumerable<ApiDetail> urlDetails, ICommonHealthCheck commonHealthCheck)
        : base(urlDetails, commonHealthCheck)
    {
    }

    protected override Predicate<ApiDetail> GetMatch()
    {
        int indexOfHealthCheck = GetType().Name.IndexOf("AnotherCustomMatchHealthCheck");
        string apiNameToTest = GetType().Name.Substring(0, indexOfHealthCheck);
        return new Predicate<ApiDetail>(u => u.Name == apiNameToTest && u.IsEnable);
    }
}