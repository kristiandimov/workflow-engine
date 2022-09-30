using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using WorkflowActionSdk;

namespace ExecutionEngineCli.Tools
{
    public static class ActionLoader
    {
        public static IWorkflowAction LoadAction(string identifier)
        {
            Assembly actionAssembly = null;

            string actionsPath = ConfigurationService.Instance.ActionRepositoryPath;
            foreach (string filePath in Directory.GetFiles(actionsPath))
            {
                string fileName = filePath.Substring(filePath.LastIndexOf(@"\") + 1);
                if (fileName == identifier + ".dll")
                {
                    actionAssembly = Assembly.LoadFile(filePath);
                    break;
                }
            }

            if (actionAssembly == null)
                throw new FileNotFoundException("Action was not found in the Repository");

            foreach(Type type in actionAssembly.GetTypes())
            {
                if (typeof(IWorkflowAction).IsAssignableFrom(type))
                    return (IWorkflowAction)Activator.CreateInstance(type);
            }

            throw new NotSupportedException("Action is in bad format.");
        }
    }
}
