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


using DurableTask;
using DurableTask.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Daenet.DurableTaskMicroservices.UnitTests
{
    public class Null { }

    public class CounterOrchestration : TaskOrchestration<int, TestOrchestrationInput>
    {
        public async override Task<int> RunTask(OrchestrationContext context, TestOrchestrationInput input)
        {
            int cnt = input.Counter;

            while (cnt > 0)
            {
                cnt--;

                await context.ScheduleTask<Null>(typeof(Task1), ":)");

                await context.ScheduleTask<Null>(typeof(Task2), ":<");

                Task.Delay(input.Delay).Wait();
            }

            return cnt;
        }
    }
}
