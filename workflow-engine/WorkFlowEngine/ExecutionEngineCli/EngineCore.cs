using Common.Entities;
using Common.Repositories;
using ExecutionEngineCli.ExecutionEngine;
using ExecutionEngineCli.Tools;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using WorkflowActionSdk;

namespace ExecutionEngineCli
{
    public static class EngineCore
    {
        public static object WorkFlowExecution(int Id)
        {
            bool isFileGenerated = false;
            FlowEngineManagementDbContext context = new FlowEngineManagementDbContext();
            FlowExecutions jsonConfig = context.FlowExecutions.Where(x=>x.OwnerId == Id).FirstOrDefault();

            if (jsonConfig.FlowConfig == null || jsonConfig.FlowConfig == "")
            {
               
                Console.WriteLine("File not found!");
                
                return new {response = "FileConfigNotFound"};
            }

            WorkflowModel workflow = JsonSerializer.Deserialize<WorkflowModel>(jsonConfig.FlowConfig);

            List<ExpandoObject> payload = new List<ExpandoObject>();

            foreach (WorkflowActionModel actionModel in workflow.Actions)
            {
                string input = JsonSerializer.Serialize(actionModel.Input);

                IWorkflowAction action = ActionLoader.LoadAction(actionModel.ActionIdentifier);
                Output output = JsonSerializer.Deserialize<Output>(action.Execute(input, payload));
                actionModel.Output = JsonSerializer.Deserialize<List<object>>(output.Message);

                if (isFileGenerated)
                {
                    DeleteGeneratedFiles();
                    isFileGenerated = false;
                }

                if (output.GeneratedFile)
                {
                    isFileGenerated = true;
                }

                if (output.Status == "Error")
                {
                    actionModel.Status = WorkflowActionModel.WorkflowActionStatus.Error;
                    jsonConfig.Status = "Error";
                    jsonConfig.FlowResult = JsonSerializer.Serialize(actionModel);
                    break;
                }
                else
                {
                    actionModel.Status = WorkflowActionModel.WorkflowActionStatus.Completed;
                    jsonConfig.Status = "Completed";
                    jsonConfig.FlowResult = JsonSerializer.Serialize(workflow);
                }

                payload = JsonSerializer.Deserialize<List<ExpandoObject>>(output.Message);

            }
            
            context.Update(jsonConfig);
            context.SaveChanges();

            DeleteGeneratedFiles();
            
            return workflow;
        }

        private static void DeleteGeneratedFiles()
        {
            string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = GetSolutionDir(path).ToString() + "\\GeneratedFiles";
            System.IO.DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo file in di.EnumerateFiles())
            {
                file.Delete();
            }
        }

        public static DirectoryInfo GetSolutionDir(string currentPath = null)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }

            return directory;
        }
    }
}
