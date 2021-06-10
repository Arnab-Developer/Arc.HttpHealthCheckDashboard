﻿using ArnabDeveloper.HttpHealthCheck;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Arc.HttpHealthCheckDashboard
{
    /// <summary>
    /// Base health check logic
    /// </summary>
    public abstract class BaseHealthCheck
        : Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck
    {
        private readonly IEnumerable<ApiDetail> _urlDetails;
        private readonly ICommonHealthCheck _commonHealthCheck;

        /// <summary>
        /// Derive class needs to call this constructor 
        /// </summary>
        /// <param name="urlDetails">Api detail collection</param>
        /// <param name="commonHealthCheck">Common health check object</param>
        public BaseHealthCheck(IEnumerable<ApiDetail> urlDetails,
            ICommonHealthCheck commonHealthCheck)
        {
            _urlDetails = urlDetails;
            _commonHealthCheck = commonHealthCheck;
        }

        Task<HealthCheckResult> Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck.CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken)
        {
            Predicate<ApiDetail> match = GetMatch();
            ApiDetail? apiDetail = _urlDetails.ToList().Find(match);

            return _commonHealthCheck.IsApiHealthy(apiDetail)
                ? Task.FromResult(HealthCheckResult.Healthy())
                : Task.FromResult(HealthCheckResult.Unhealthy());
        }

        /// <summary>
        /// Get the api detail match condition to find api from the 
        /// api detail collection
        /// </summary>
        /// <returns>Find condition</returns>
        protected virtual Predicate<ApiDetail> GetMatch()
        {
            int indexOfHealthCheck = GetType().Name.IndexOf("HealthCheck");
            string apiNameToTest = GetType().Name.Substring(0, indexOfHealthCheck);
            return new Predicate<ApiDetail>(u => u.Name == apiNameToTest && u.IsEnable);
        }
    }
}
