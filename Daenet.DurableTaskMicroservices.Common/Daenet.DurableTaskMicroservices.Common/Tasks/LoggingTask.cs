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
using Daenet.Common.Logging;
using Daenet.DurableTaskMicroservices.Common.Base;
using Daenet.DurableTaskMicroservices.Common.Entities;
using Daenet.DurableTaskMicroservices.Core;
using DurableTask.Core;
using Microsoft.Extensions.Logging;
using System;

namespace Daenet.DurableTaskMicroservices.Common.Tasks
{


    public class LoggingTask<TInput, TAdapterOutput> : TaskBase<TInput, TAdapterOutput>
        where TInput : LoggingTaskInput
        where TAdapterOutput : Null
    {
        private ILoggerFactory m_LogFactory;

        protected override TAdapterOutput RunTask(TaskContext context, TInput input, ILogger logger)
        {
            if (this.m_LogFactory == null)
                return (TAdapterOutput)new Null();

            object[] parameters = input.MessageParams;
            if (parameters == null)
                parameters = new object[1] { "" };

            LoggingContext loggingContext = input.LoggingContext;
            string logTraceSourceName = this.GetConfiguration(input.Orchestration).LogTraceSourceName;
            if (String.IsNullOrEmpty(logTraceSourceName))
            {
                if (!loggingContext.LoggingScopes.TryGetValue("LogTraceSourceName", out logTraceSourceName))
                    logTraceSourceName = this.GetType().Namespace + "." + this.GetType().Name;
            }

            ILogManager logManager = new LogManager(m_LogFactory, logTraceSourceName);
            foreach (var scope in loggingContext.LoggingScopes)
                logManager.AddScope(scope.Key, scope.Value);

            // Add WorkflowInstanceId as a new ActivityId if not existing yet. 
            if (!logManager.CurrentScope.ContainsKey("OrchestrationInstanceId"))
                logManager.AddScope("OrchestrationInstanceId", context.OrchestrationInstance.InstanceId);


            switch (input.LoggingAction)
            {
                case Entities.Action.TraceInfo:
                    logManager.TraceMessage(input.TracingLevel, input.EventId, input.FormatedMessage, parameters);
                    break;
                case Entities.Action.TraceWarning:
                    logManager.TraceWarning(input.TracingLevel, input.EventId, input.FormatedMessage, parameters);
                    break;
                case Entities.Action.TraceError:
                    if (input.Exception != null)
                        logManager.TraceError(input.TracingLevel, input.EventId, input.Exception, input.FormatedMessage, parameters);
                    else
                        logManager.TraceError(input.TracingLevel, input.EventId, input.FormatedMessage, parameters);
                    break;
            }

            return (TAdapterOutput)new Null();
        }

     
    }
}
