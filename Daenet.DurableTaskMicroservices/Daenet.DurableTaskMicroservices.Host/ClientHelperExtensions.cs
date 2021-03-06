﻿//  ----------------------------------------------------------------------------------
//  Copyright daenet Gesellschaft für Informationstechnologie mbH
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  http://www.apache.org/licenses/LICENSE-2.0
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//  ----------------------------------------------------------------------------------
using Daenet.DurableTask.SqlInstanceStoreProvider;
using Daenet.DurableTaskMicroservices.Core;
using DurableTask.Core;
using DurableTask.ServiceBus;
using DurableTask.ServiceBus.Tracking;
using Microsoft.Extensions.Logging;

namespace Daenet.DurableTaskMicroservices.Host
{
    public static class ClientHelperExtensions
    {
        public static ServiceClient CreateMicroserviceClient(string serviceBusConnectionString, string storageConnectionString, string hubName, ILogger logger = null)
        {
            IOrchestrationServiceInstanceStore instanceStore;

            instanceStore = InstanceStoreFactory.CreateInstanceStore(hubName, storageConnectionString);

            ServiceBusOrchestrationService orchestrationServiceAndClient =
                 new ServiceBusOrchestrationService(serviceBusConnectionString, hubName, instanceStore, null, null);

            ServiceClient client;

            client = new ServiceClient(orchestrationServiceAndClient);

            return client;
        }
    }

}
