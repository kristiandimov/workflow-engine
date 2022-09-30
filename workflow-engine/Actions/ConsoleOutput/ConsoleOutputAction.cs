using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using WorkflowActionSdk;
using WorkflowActionSdk.Extentions;

namespace ConsoleOutput
{
    public class ConsoleOutputAction : IWorkflowAction
    {
        public IWorkflowAction.WorkflowActionStatus Status { get; set; }

        public string Execute(string data, List<ExpandoObject> payload)
        {
            //Input input = JsonSerializer.Deserialize<Input>(data);
            Input input = new Input();
            MatchCollection matches = Regex.Matches(input.Message, @"(\{\{)([a-zA-Z0-9\.]+)(\}\})");
            foreach(Match match in matches)
            {
                string targetField = match.Value.Replace("{", "").Replace("}", "");
                targetField = targetField.Replace("input.", "");

                //input.Message = input.Message.Replace(match.Value, payload.GetField(targetField).ToString());
            }

            Console.WriteLine(input.Message);

            Output output = new Output();
            output.Message = input.Message;
            output.Status = Output.JobStatus.Success.ToString();

            return JsonSerializer.Serialize(output);
        }
    }
}
