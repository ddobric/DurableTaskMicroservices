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
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Daenet.DurableTaskMicroservices.Common.Entities
{
    /// <summary>
    /// Holds task input arguments and related logging context information.
    /// </summary>
    [DataContract]
    public class TaskInput
    {
        /// <summary>
        /// initizales Context property
        /// </summary>
        public TaskInput()
        {
            Context = new Dictionary<string, object>();
        }

        /// <summary>
        /// Task input argument. This is typically set programmatically by orchestration.
        /// You are not required to set this in configuration of the task.
        /// </summary>
        [DataMember]
        public object Data { get; set; }

        /// <summary>
        /// Dictionary of values which defines context.
        /// </summary>
        [DataMember]
        public Dictionary<string, object> Context { get; set; }

        /// <summary>
        /// Name of the orchestration which triggered this Task
        /// </summary>
        [IgnoreDataMember]
        public string Orchestration
        {
            get
            {
                if (Context.ContainsKey("Orchestration"))
                {
                    return Context["Orchestration"].ToString();
                }
                else
                    return null;
            }
            set
            {
                if (Context.ContainsKey("Orchestration"))
                {
                    Context["Orchestration"] = value;
                }
                else
                    Context.Add("Orchestration", value);
            }
        }
        
        //The logging context which is propagated from orchestration to child tasks.
        public LoggingContext ParentLoggingContext { get; set; }

        /// <summary>
        /// The Trase Source Name used by logging.
        /// </summary>
        [DataMember]
        public string LogTraceSourceName { get; set; }

        /// <summary>
        /// Holds the list of all validator rules which have to be executed in a chain.
        /// The chain of rules will be executed before the task is executed.
        /// If any of rules does not pass validation (returns FALSE) the task will not be executed.
        /// </summary>
        [DataMember]
        public ICollection<ValidationRuleDescriptor> ValidatorRules { get; set; }

    }
}
