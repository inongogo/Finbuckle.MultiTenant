//    Copyright 2018 Andrew White
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Threading.Tasks;
using Finbuckle.MultiTenant.AspNetCore;
using Finbuckle.MultiTenant.Core;
using Finbuckle.MultiTenant.Core.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Finbuckle.MultiTenant.AspNetCore
{
    public class BasePathMultiTenantStrategy : IMultiTenantStrategy
    {
        private readonly ILogger<BasePathMultiTenantStrategy> logger;

        public BasePathMultiTenantStrategy()
        {
        }

        public BasePathMultiTenantStrategy(ILogger<BasePathMultiTenantStrategy> logger)
        {
            this.logger = logger;
        }

        public virtual string GetIdentifier(object context)
        {
            if(!(context is HttpContext))
                throw new MultiTenantException(null,
                    new ArgumentException("\"context\" type must be of type HttpContext", nameof(context)));

            var path = (context as HttpContext).Request.Path;

            Utilities.TryLogInfo(logger, $"Path:  \"{path.Value ?? "<null>"}\"");

            var pathSegments =
                path.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (pathSegments.Length == 0)
                return null;

            string identifier = pathSegments[0];

            Utilities.TryLogInfo(logger, $"Found identifier:  \"{identifier ?? "<null>"}\"");

            return identifier;
        }
    }
}