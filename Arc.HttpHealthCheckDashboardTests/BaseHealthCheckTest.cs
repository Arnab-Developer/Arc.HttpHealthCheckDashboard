using Arc.HttpHealthCheckDashboard;
using ArnabDeveloper.HttpHealthCheck;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using System.Reflection;
using Tynamix.ObjectFiller;
using Xunit;

namespace Arc.HttpHealthCheckDashboardTests;

public class BaseHealthCheckTest
{
    [Fact]
    public async Task Can_CheckHealth_ReturnHealthy()
    {
        ApiCredential apiCredential1 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential2 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential3 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential4 = new(Randomizer<string>.Create(), Randomizer<string>.Create());

        IEnumerable<ApiDetail> urlDetails = new List<ApiDetail>()
        {
            new ApiDetail("api1", "url1", apiCredential1, true),
            new ApiDetail("api2", "url2", apiCredential2, true),
            new ApiDetail("Test", "url3", apiCredential3, true),
            new ApiDetail("api4", "url4", apiCredential4, true)
        };

        Mock<ICommonHealthCheck> commonHealthCheckMock = new();

        Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck healthCheck =
            new TestHealthCheck(urlDetails, commonHealthCheckMock.Object);

        commonHealthCheckMock
            .Setup(s => s.IsApiHealthyAsync(urlDetails.ElementAt(2)))
            .ReturnsAsync(true);

        HealthCheckResult healthCheckResult =
            await healthCheck.CheckHealthAsync(new HealthCheckContext(), new CancellationToken());

        Assert.Equal(HealthCheckResult.Healthy(), healthCheckResult);

        commonHealthCheckMock
            .Verify(m => m.IsApiHealthyAsync(urlDetails.ElementAt(2)),
                Times.Once);

        commonHealthCheckMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Can_CheckHealth_ReturnUnHealthy()
    {
        ApiCredential apiCredential1 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential2 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential3 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential4 = new(Randomizer<string>.Create(), Randomizer<string>.Create());

        IEnumerable<ApiDetail> urlDetails = new List<ApiDetail>()
        {
            new ApiDetail("api1", "url1", apiCredential1, true),
            new ApiDetail("api2", "url2", apiCredential2, true),
            new ApiDetail("Test", "url3", apiCredential3, true),
            new ApiDetail("api4", "url4", apiCredential4, true)
        };

        Mock<ICommonHealthCheck> commonHealthCheckMock = new();

        Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck healthCheck =
            new TestHealthCheck(urlDetails, commonHealthCheckMock.Object);

        commonHealthCheckMock
            .Setup(s => s.IsApiHealthyAsync(urlDetails.ElementAt(2)))
            .ReturnsAsync(false);

        HealthCheckResult healthCheckResult =
            await healthCheck.CheckHealthAsync(new HealthCheckContext(), new CancellationToken());

        Assert.Equal(HealthCheckResult.Unhealthy(), healthCheckResult);

        commonHealthCheckMock
            .Verify(m => m.IsApiHealthyAsync(urlDetails.ElementAt(2)),
                Times.Once);

        commonHealthCheckMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Can_CheckHealth_ReturnUnHealthyIfIsEnableFalse()
    {
        ApiCredential apiCredential1 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential2 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential3 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential4 = new(Randomizer<string>.Create(), Randomizer<string>.Create());

        IEnumerable<ApiDetail> urlDetails = new List<ApiDetail>()
        {
            new ApiDetail("api1", "url1", apiCredential1, true),
            new ApiDetail("api2", "url2", apiCredential2, true),
            new ApiDetail("Test", "url3", apiCredential3, false),
            new ApiDetail("api4", "url4", apiCredential4, true)
        };

        Mock<ICommonHealthCheck> commonHealthCheckMock = new();

        Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck healthCheck =
            new TestHealthCheck(urlDetails, commonHealthCheckMock.Object);

        commonHealthCheckMock
            .Setup(s => s.IsApiHealthyAsync(null))
            .ReturnsAsync(false);

        HealthCheckResult healthCheckResult =
            await healthCheck.CheckHealthAsync(new HealthCheckContext(), new CancellationToken());

        Assert.Equal(HealthCheckResult.Unhealthy(), healthCheckResult);

        commonHealthCheckMock
            .Verify(m => m.IsApiHealthyAsync(null),
                Times.Once);

        commonHealthCheckMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Can_CheckHealth_ReturnUnHealthyIfInvalidMatch()
    {
        ApiCredential apiCredential1 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential2 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential3 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential4 = new(Randomizer<string>.Create(), Randomizer<string>.Create());

        IEnumerable<ApiDetail> urlDetails = new List<ApiDetail>()
        {
            new ApiDetail("api1", "url1", apiCredential1, true),
            new ApiDetail("api2", "url2", apiCredential2, true),
            new ApiDetail("Test", "url3", apiCredential3, true),
            new ApiDetail("api4", "url4", apiCredential4, true)
        };

        Mock<ICommonHealthCheck> commonHealthCheckMock = new();

        Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck healthCheck =
            new Test1CustomMatchHealthCheck(urlDetails, commonHealthCheckMock.Object);

        commonHealthCheckMock
            .Setup(s => s.IsApiHealthyAsync(null))
            .ReturnsAsync(false);

        HealthCheckResult healthCheckResult =
            await healthCheck.CheckHealthAsync(new HealthCheckContext(), new CancellationToken());

        Assert.Equal(HealthCheckResult.Unhealthy(), healthCheckResult);

        commonHealthCheckMock
            .Verify(m => m.IsApiHealthyAsync(null),
                Times.Once);

        commonHealthCheckMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Can_CheckHealth_ReturnHealthyWithCustomMatch()
    {
        ApiCredential apiCredential1 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential2 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential3 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential4 = new(Randomizer<string>.Create(), Randomizer<string>.Create());

        IEnumerable<ApiDetail> urlDetails = new List<ApiDetail>()
        {
            new ApiDetail("api1", "url1", apiCredential1, true),
            new ApiDetail("api2", "url2", apiCredential2, true),
            new ApiDetail("Test", "url3", apiCredential3, true),
            new ApiDetail("api4", "url4", apiCredential4, true)
        };

        Mock<ICommonHealthCheck> commonHealthCheckMock = new();

        Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck healthCheck =
            new TestAnotherCustomMatchHealthCheck(urlDetails, commonHealthCheckMock.Object);

        commonHealthCheckMock
            .Setup(s => s.IsApiHealthyAsync(urlDetails.ElementAt(2)))
            .ReturnsAsync(true);

        HealthCheckResult healthCheckResult =
            await healthCheck.CheckHealthAsync(new HealthCheckContext(), new CancellationToken());

        Assert.Equal(HealthCheckResult.Healthy(), healthCheckResult);

        commonHealthCheckMock
            .Verify(m => m.IsApiHealthyAsync(urlDetails.ElementAt(2)),
                Times.Once);

        commonHealthCheckMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Can_GetMatch_ReturnCorrectMatch()
    {
        string randomUserName = Randomizer<string>.Create();
        string randomPwd = Randomizer<string>.Create();

        ApiCredential apiCredential1 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential2 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential3 = new(randomUserName, randomPwd);
        ApiCredential apiCredential4 = new(Randomizer<string>.Create(), Randomizer<string>.Create());

        IEnumerable<ApiDetail> urlDetails = new List<ApiDetail>()
        {
            new ApiDetail("api1", "url1", apiCredential1, true),
            new ApiDetail("api2", "url2", apiCredential2, true),
            new ApiDetail("Test", "url3", apiCredential3, true),
            new ApiDetail("api4", "url4", apiCredential4, true)
        };

        Mock<ICommonHealthCheck> commonHealthCheckMock = new();

        TestHealthCheck testHealthCheck = new(urlDetails, commonHealthCheckMock.Object);

        commonHealthCheckMock
            .Setup(s => s.IsApiHealthyAsync(urlDetails.ElementAt(2)))
            .ReturnsAsync(true);

        MethodInfo? GetMatchInfo = testHealthCheck.GetType()
            .GetMethod("GetMatch", BindingFlags.NonPublic | BindingFlags.Instance);

        Assert.NotNull(GetMatchInfo);
        if (GetMatchInfo != null)
        {
            object? returnVal = GetMatchInfo.Invoke(testHealthCheck, null);
            Assert.NotNull(returnVal);
            if (returnVal != null)
            {
                Predicate<ApiDetail> match = (Predicate<ApiDetail>)returnVal;
                ApiDetail? apiDetail = urlDetails.ToList().Find(match);
                Assert.NotNull(apiDetail);
                Assert.Equal("Test", apiDetail!.Name);
                Assert.Equal("url3", apiDetail.Url);
                Assert.NotNull(apiDetail.ApiCredential);
                Assert.Equal(randomUserName, apiDetail.ApiCredential!.UserName);
                Assert.Equal(randomPwd, apiDetail.ApiCredential!.Password);
                Assert.True(apiDetail.IsEnable);
            }
        }

        commonHealthCheckMock
            .Verify(m => m.IsApiHealthyAsync(urlDetails.ElementAt(2)),
                Times.Never);

        commonHealthCheckMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Can_GetMatch_ReturnCorrectCustomMatch()
    {
        string randomUserName = Randomizer<string>.Create();
        string randomPwd = Randomizer<string>.Create();

        ApiCredential apiCredential1 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential2 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential3 = new(randomUserName, randomPwd);
        ApiCredential apiCredential4 = new(Randomizer<string>.Create(), Randomizer<string>.Create());

        IEnumerable<ApiDetail> urlDetails = new List<ApiDetail>()
        {
            new ApiDetail("api1", "url1", apiCredential1, true),
            new ApiDetail("api2", "url2", apiCredential2, true),
            new ApiDetail("Test1", "url3", apiCredential3, true),
            new ApiDetail("api4", "url4", apiCredential4, true)
        };

        Mock<ICommonHealthCheck> commonHealthCheckMock = new();
        Test1CustomMatchHealthCheck test1CustomMatchHealthCheck = new(urlDetails, commonHealthCheckMock.Object);

        commonHealthCheckMock
            .Setup(s => s.IsApiHealthyAsync(urlDetails.ElementAt(2)))
            .ReturnsAsync(true);

        MethodInfo? GetMatchInfo = test1CustomMatchHealthCheck.GetType()
            .GetMethod("GetMatch", BindingFlags.NonPublic | BindingFlags.Instance);

        Assert.NotNull(GetMatchInfo);
        if (GetMatchInfo != null)
        {
            object? returnVal = GetMatchInfo.Invoke(test1CustomMatchHealthCheck, null);
            Assert.NotNull(returnVal);
            if (returnVal != null)
            {
                Predicate<ApiDetail> match = (Predicate<ApiDetail>)returnVal;
                ApiDetail? apiDetail = urlDetails.ToList().Find(match);
                Assert.NotNull(apiDetail);
                Assert.Equal("Test1", apiDetail!.Name);
                Assert.Equal("url3", apiDetail.Url);
                Assert.NotNull(apiDetail.ApiCredential);
                Assert.Equal(randomUserName, apiDetail.ApiCredential!.UserName);
                Assert.Equal(randomPwd, apiDetail.ApiCredential!.Password);
                Assert.True(apiDetail.IsEnable);
            }
        }

        commonHealthCheckMock
            .Verify(m => m.IsApiHealthyAsync(urlDetails.ElementAt(2)),
                Times.Never);

        commonHealthCheckMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Can_GetMatch_ReturnCorrectCustomDisableMatch()
    {
        ApiCredential apiCredential1 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential2 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential3 = new(Randomizer<string>.Create(), Randomizer<string>.Create());
        ApiCredential apiCredential4 = new(Randomizer<string>.Create(), Randomizer<string>.Create());

        IEnumerable<ApiDetail> urlDetails = new List<ApiDetail>()
        {
            new ApiDetail("api1", "url1", apiCredential1, true),
            new ApiDetail("api2", "url2", apiCredential2, true),
            new ApiDetail("Test1", "url3", apiCredential3, false),
            new ApiDetail("api4", "url4", apiCredential4, true)
        };

        Mock<ICommonHealthCheck> commonHealthCheckMock = new();
        Test1CustomMatchHealthCheck test1CustomMatchHealthCheck = new(urlDetails, commonHealthCheckMock.Object);

        commonHealthCheckMock
            .Setup(s => s.IsApiHealthyAsync(urlDetails.ElementAt(2)))
            .ReturnsAsync(true);

        MethodInfo? GetMatchInfo = test1CustomMatchHealthCheck.GetType()
            .GetMethod("GetMatch", BindingFlags.NonPublic | BindingFlags.Instance);

        Assert.NotNull(GetMatchInfo);
        if (GetMatchInfo != null)
        {
            object? returnVal = GetMatchInfo.Invoke(test1CustomMatchHealthCheck, null);
            Assert.NotNull(returnVal);
            if (returnVal != null)
            {
                Predicate<ApiDetail> match = (Predicate<ApiDetail>)returnVal;
                ApiDetail? apiDetail = urlDetails.ToList().Find(match);
                Assert.Null(apiDetail);
            }
        }

        commonHealthCheckMock
            .Verify(m => m.IsApiHealthyAsync(urlDetails.ElementAt(2)),
                Times.Never);

        commonHealthCheckMock.VerifyNoOtherCalls();
    }
}