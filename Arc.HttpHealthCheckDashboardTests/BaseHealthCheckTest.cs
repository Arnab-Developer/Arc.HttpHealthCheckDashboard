using Arc.HttpHealthCheckDashboard;
using ArnabDeveloper.HttpHealthCheck;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using System.Reflection;
using Xunit;

namespace Arc.HttpHealthCheckDashboardTests;

public class BaseHealthCheckTest
{
    [Fact]
    public async Task Can_CheckHealth_ReturnHealthy()
    {
        IEnumerable<ApiDetail> urlDetails = new List<ApiDetail>()
            {
                new ApiDetail("api1", "url1", new ApiCredential("user1", "pass1"), true),
                new ApiDetail("api2", "url2", new ApiCredential("user2", "pass2"), true),
                new ApiDetail("Test", "url3", new ApiCredential("user3", "pass3"), true),
                new ApiDetail("api4", "url4", new ApiCredential("user4", "pass4"), true)
            };

        Mock<ICommonHealthCheck> commonHealthCheckMock = new();
        Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck healthCheck
            = new TestHealthCheck(urlDetails, commonHealthCheckMock.Object);

        commonHealthCheckMock
            .Setup(s => s.IsApiHealthyAsync(urlDetails.ElementAt(2)))
            .ReturnsAsync(true);

        HealthCheckResult healthCheckResult = await healthCheck.CheckHealthAsync(
            new HealthCheckContext(), new CancellationToken());

        Assert.Equal(HealthCheckResult.Healthy(), healthCheckResult);
    }

    [Fact]
    public async Task Can_CheckHealth_ReturnUnHealthy()
    {
        IEnumerable<ApiDetail> urlDetails = new List<ApiDetail>()
            {
                new ApiDetail("api1", "url1", new ApiCredential("user1", "pass1"), true),
                new ApiDetail("api2", "url2", new ApiCredential("user2", "pass2"), true),
                new ApiDetail("Test", "url3", new ApiCredential("user3", "pass3"), true),
                new ApiDetail("api4", "url4", new ApiCredential("user4", "pass4"), true)
            };

        Mock<ICommonHealthCheck> commonHealthCheckMock = new();
        Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck healthCheck
            = new TestHealthCheck(urlDetails, commonHealthCheckMock.Object);

        commonHealthCheckMock
            .Setup(s => s.IsApiHealthyAsync(urlDetails.ElementAt(2)))
            .ReturnsAsync(false);

        HealthCheckResult healthCheckResult = await healthCheck.CheckHealthAsync(
            new HealthCheckContext(), new CancellationToken());

        Assert.Equal(HealthCheckResult.Unhealthy(), healthCheckResult);
    }

    [Fact]
    public async Task Can_CheckHealth_ReturnUnHealthyIfIsEnableFalse()
    {
        IEnumerable<ApiDetail> urlDetails = new List<ApiDetail>()
            {
                new ApiDetail("api1", "url1", new ApiCredential("user1", "pass1"), true),
                new ApiDetail("api2", "url2", new ApiCredential("user2", "pass2"), true),
                new ApiDetail("Test", "url3", new ApiCredential("user3", "pass3"), false),
                new ApiDetail("api4", "url4", new ApiCredential("user4", "pass4"), true)
            };

        Mock<ICommonHealthCheck> commonHealthCheckMock = new();
        Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck healthCheck
            = new TestHealthCheck(urlDetails, commonHealthCheckMock.Object);

        commonHealthCheckMock
            .Setup(s => s.IsApiHealthyAsync(null))
            .ReturnsAsync(false);

        HealthCheckResult healthCheckResult = await healthCheck.CheckHealthAsync(
            new HealthCheckContext(), new CancellationToken());

        Assert.Equal(HealthCheckResult.Unhealthy(), healthCheckResult);
    }

    [Fact]
    public async Task Can_CheckHealth_ReturnUnHealthyIfInvalidMatch()
    {
        IEnumerable<ApiDetail> urlDetails = new List<ApiDetail>()
            {
                new ApiDetail("api1", "url1", new ApiCredential("user1", "pass1"), true),
                new ApiDetail("api2", "url2", new ApiCredential("user2", "pass2"), true),
                new ApiDetail("Test", "url3", new ApiCredential("user3", "pass3"), true),
                new ApiDetail("api4", "url4", new ApiCredential("user4", "pass4"), true)
            };

        Mock<ICommonHealthCheck> commonHealthCheckMock = new();
        Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck healthCheck
            = new Test1CustomMatchHealthCheck(urlDetails, commonHealthCheckMock.Object);

        commonHealthCheckMock
            .Setup(s => s.IsApiHealthyAsync(null))
            .ReturnsAsync(false);

        HealthCheckResult healthCheckResult = await healthCheck.CheckHealthAsync(
            new HealthCheckContext(), new CancellationToken());

        Assert.Equal(HealthCheckResult.Unhealthy(), healthCheckResult);
    }

    [Fact]
    public async Task Can_CheckHealth_ReturnHealthyWithCustomMatch()
    {
        IEnumerable<ApiDetail> urlDetails = new List<ApiDetail>()
            {
                new ApiDetail("api1", "url1", new ApiCredential("user1", "pass1"), true),
                new ApiDetail("api2", "url2", new ApiCredential("user2", "pass2"), true),
                new ApiDetail("Test", "url3", new ApiCredential("user3", "pass3"), true),
                new ApiDetail("api4", "url4", new ApiCredential("user4", "pass4"), true)
            };

        Mock<ICommonHealthCheck> commonHealthCheckMock = new();
        Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck healthCheck
            = new TestAnotherCustomMatchHealthCheck(urlDetails, commonHealthCheckMock.Object);

        commonHealthCheckMock
            .Setup(s => s.IsApiHealthyAsync(urlDetails.ElementAt(2)))
            .ReturnsAsync(true);

        HealthCheckResult healthCheckResult = await healthCheck.CheckHealthAsync(
            new HealthCheckContext(), new CancellationToken());

        Assert.Equal(HealthCheckResult.Healthy(), healthCheckResult);
    }

    [Fact]
    public void Can_GetMatch_ReturnCorrectMatch()
    {
        IEnumerable<ApiDetail> urlDetails = new List<ApiDetail>()
            {
                new ApiDetail("api1", "url1", new ApiCredential("user1", "pass1"), true),
                new ApiDetail("api2", "url2", new ApiCredential("user2", "pass2"), true),
                new ApiDetail("Test", "url3", new ApiCredential("user3", "pass3"), true),
                new ApiDetail("api4", "url4", new ApiCredential("user4", "pass4"), true)
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
                Assert.Equal("user3", apiDetail.ApiCredential!.UserName);
                Assert.Equal("pass3", apiDetail.ApiCredential!.Password);
                Assert.True(apiDetail.IsEnable);
            }
        }
    }

    [Fact]
    public void Can_GetMatch_ReturnCorrectCustomMatch()
    {
        IEnumerable<ApiDetail> urlDetails = new List<ApiDetail>()
            {
                new ApiDetail("api1", "url1", new ApiCredential("user1", "pass1"), true),
                new ApiDetail("api2", "url2", new ApiCredential("user2", "pass2"), true),
                new ApiDetail("Test1", "url3", new ApiCredential("user3", "pass3"), true),
                new ApiDetail("api4", "url4", new ApiCredential("user4", "pass4"), true)
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
                Assert.Equal("user3", apiDetail.ApiCredential!.UserName);
                Assert.Equal("pass3", apiDetail.ApiCredential!.Password);
                Assert.True(apiDetail.IsEnable);
            }
        }
    }

    [Fact]
    public void Can_GetMatch_ReturnCorrectCustomDisableMatch()
    {
        IEnumerable<ApiDetail> urlDetails = new List<ApiDetail>()
            {
                new ApiDetail("api1", "url1", new ApiCredential("user1", "pass1"), true),
                new ApiDetail("api2", "url2", new ApiCredential("user2", "pass2"), true),
                new ApiDetail("Test1", "url3", new ApiCredential("user3", "pass3"), false),
                new ApiDetail("api4", "url4", new ApiCredential("user4", "pass4"), true)
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
    }
}