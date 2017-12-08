﻿using Daenet.Common.Logging;
using DurableTask.Core;
using System.Collections.Generic;

namespace Daenet.DurableTaskMicroservices.Common.Extensions
{
    public static class OrchestrationContextExtensions
    {

        private static Dictionary<OrchestrationContext, LoggingContext> m_LogManagers = new Dictionary<OrchestrationContext, LoggingContext>();

        internal static void SetLoggingContext(this OrchestrationContext context, LoggingContext logMgr)
        {
            lock (m_LogManagers)
            {
                if (m_LogManagers.ContainsKey(context) == false)
                    m_LogManagers.Add(context, logMgr);
            }
        }

        public static LoggingContext GetLoggingContext(this OrchestrationContext context)
        {
            lock (m_LogManagers)
            {
                if (m_LogManagers.ContainsKey(context))
                    return m_LogManagers[context];
                else
                    return null;
            }
        }
    }
}
