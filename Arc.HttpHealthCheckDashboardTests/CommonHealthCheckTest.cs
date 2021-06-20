using Arc.HttpHealthCheckDashboard;
using ArnabDeveloper.HttpHealthCheck;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Arc.HttpHealthCheckDashboardTests
{
    public class CommonHealthCheckTest
    {
        private readonly Mock<IHealthCheck> _healthCheckMock;
        private readonly ICommonHealthCheck _commonHealthCheck;

        public CommonHealthCheckTest()
        {
            _healthCheckMock = new Mock<IHealthCheck>();
            _commonHealthCheck = new CommonHealthCheck(_healthCheckMock.Object);
        }

        [Fact]
        public async Task Can_IsApiHealthyAsync_ReturnTrueForOnlyUrl()
        {
            ApiDetail apiDetail = new("demo name", "demo url", null, true);

            _healthCheckMock
                .Setup(s => s.IsHealthyAsync(apiDetail.Url, null))
                .ReturnsAsync(true);

            bool IsApiHealthyAsync = await _commonHealthCheck.IsApiHealthyAsync(apiDetail);

            Assert.True(IsApiHealthyAsync);
        }

        [Fact]
        public async Task Can_IsApiHealthyAsync_ReturnTrueForUrlWithCredential()
        {
            ApiCredential apiCredential = new("demo user", "demo pass");
            ApiDetail apiDetail = new("demo name", "demo url", apiCredential, true);

            _healthCheckMock
                .Setup(s => s.IsHealthyAsync(apiDetail.Url, apiCredential))
                .ReturnsAsync(true);

            bool IsApiHealthyAsync = await _commonHealthCheck.IsApiHealthyAsync(apiDetail);

            Assert.True(IsApiHealthyAsync);
        }

        [Fact]
        public async Task Can_IsApiHealthyAsync_ReturnFalseForOnlyUrl()
        {
            ApiDetail apiDetail = new("demo name", "demo url", null, true);

            _healthCheckMock
                .Setup(s => s.IsHealthyAsync(apiDetail.Url, null))
                .ReturnsAsync(false);

            bool IsApiHealthyAsync = await _commonHealthCheck.IsApiHealthyAsync(apiDetail);

            Assert.False(IsApiHealthyAsync);
        }

        [Fact]
        public async Task Can_IsApiHealthyAsync_ReturnFalseForUrlWithCredential()
        {
            ApiCredential apiCredential = new("demo user", "demo pass");
            ApiDetail apiDetail = new("demo name", "demo url", apiCredential, true);

            _healthCheckMock
                .Setup(s => s.IsHealthyAsync(apiDetail.Url, apiCredential))
                .ReturnsAsync(false);

            bool IsApiHealthyAsync = await _commonHealthCheck.IsApiHealthyAsync(apiDetail);

            Assert.False(IsApiHealthyAsync);
        }

        [Fact]
        public async Task Can_IsApiHealthyAsync_ReturnFalseForNullApiDetail()
        {
            ApiDetail apiDetail = new("demo name", "demo url", null, true);

            _healthCheckMock
                .Setup(s => s.IsHealthyAsync(apiDetail.Url, null))
                .ReturnsAsync(true);

            bool IsApiHealthyAsync = await _commonHealthCheck.IsApiHealthyAsync(null);

            Assert.False(IsApiHealthyAsync);
        }

        [Fact]
        public async Task Can_IsApiHealthyAsync_ReturnFalseForException()
        {
            ApiDetail apiDetail = new("demo name", "demo url", null, true);

            _healthCheckMock
                .Setup(s => s.IsHealthyAsync(apiDetail.Url, null))
                .Throws<NullReferenceException>();

            bool IsApiHealthyAsync = await _commonHealthCheck.IsApiHealthyAsync(apiDetail);

            Assert.False(IsApiHealthyAsync);
        }

        [Fact]
        public async Task Can_IsApiHealthyAsync_ReturnTrueForUrlWithEmptyCredential()
        {
            ApiCredential apiCredential = new(string.Empty, string.Empty);
            ApiDetail apiDetail = new("demo name", "demo url", apiCredential, true);

            _healthCheckMock
                .Setup(s => s.IsHealthyAsync(apiDetail.Url, null))
                .ReturnsAsync(true);

            bool IsApiHealthyAsync = await _commonHealthCheck.IsApiHealthyAsync(apiDetail);

            Assert.True(IsApiHealthyAsync);
        }

        [Fact]
        public async Task Can_IsApiHealthyAsync_ReturnFalseForUrlWithEmptyCredential()
        {
            ApiCredential apiCredential = new(string.Empty, string.Empty);
            ApiDetail apiDetail = new("demo name", "demo url", apiCredential, true);

            _healthCheckMock
                .Setup(s => s.IsHealthyAsync(apiDetail.Url, null))
                .ReturnsAsync(false);

            bool IsApiHealthyAsync = await _commonHealthCheck.IsApiHealthyAsync(apiDetail);

            Assert.False(IsApiHealthyAsync);
        }

        [Fact]
        public async Task Can_IsApiHealthyAsync_ReturnTrueForUrlWithSpaceInCredential()
        {
            ApiCredential apiCredential = new(" ", " ");
            ApiDetail apiDetail = new("demo name", "demo url", apiCredential, true);

            _healthCheckMock
                .Setup(s => s.IsHealthyAsync(apiDetail.Url, null))
                .ReturnsAsync(true);

            bool IsApiHealthyAsync = await _commonHealthCheck.IsApiHealthyAsync(apiDetail);

            Assert.True(IsApiHealthyAsync);
        }

        [Fact]
        public async Task Can_IsApiHealthyAsync_ReturnFalseForUrlWithSpaceInCredential()
        {
            ApiCredential apiCredential = new(" ", " ");
            ApiDetail apiDetail = new("demo name", "demo url", apiCredential, true);

            _healthCheckMock
                .Setup(s => s.IsHealthyAsync(apiDetail.Url, null))
                .ReturnsAsync(false);

            bool IsApiHealthyAsync = await _commonHealthCheck.IsApiHealthyAsync(apiDetail);

            Assert.False(IsApiHealthyAsync);
        }
    }
}
