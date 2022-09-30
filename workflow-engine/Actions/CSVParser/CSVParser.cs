using CsvHelper;
using ExecutionEngineCli;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using WorkflowActionSdk;

namespace CSVParser
{
    public class CSVParser : IWorkflowAction
    {
        public IWorkflowAction.WorkflowActionStatus Status { get; set; }

        public string Execute(string input, List<ExpandoObject> payload)
        {
            dynamic JSONToCSV = new List<dynamic>(payload);

            Output output = new Output();
            
            try
            {               
                string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                path = EngineCore.GetSolutionDir(path).ToString() + "\\GeneratedFiles\\CSVParser.csv";

                using (var writer = new StreamWriter(path))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(JSONToCSV);                    
                    writer.Flush();

                }

                JSONToCSV = new List<dynamic>();
                JSONToCSV.Add(new
                {                   
                    CsvStringDataPreview = File.ReadLines(path).Take<string>(20)
                });

                output.Message = JsonSerializer.Serialize(JSONToCSV);
                output.Status = Output.JobStatus.Success.ToString();
                output.GeneratedFile = true;

            }
            catch(Exception ex)
            {
                object exeption = new
                {
                    error = ex.Message,
                    InnerExeption = ex.InnerException         
                };

                dynamic error = new List<dynamic>();
                error.Add(exeption);


                output.Message = JsonSerializer.Serialize(error);
                output.Status = Output.JobStatus.Error.ToString();
            }

            return JsonSerializer.Serialize(output);
            
        }

    }
}
