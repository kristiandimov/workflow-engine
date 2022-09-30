using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.Json;
using WorkflowActionSdk;
using WorkflowActionSdk.Extentions;

namespace MicrosoftSqlAction
{
    public class MicrosoftSqlAction : IWorkflowAction
    {
        public IWorkflowAction.WorkflowActionStatus Status { get; set; }

        public string Execute(string input, List<ExpandoObject> payload)
        {

            ExpandoObject sqlCredentials = JsonSerializer.Deserialize<ExpandoObject>(input);
            
            dynamic outputData = new List<dynamic>();

            Output output = new Output();
            

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.DataSource = sqlCredentials.GetField("ServerName").ToString();
                builder.InitialCatalog = sqlCredentials.GetField("DatabaseName").ToString();
                builder.UserID = sqlCredentials.GetField("UserId").ToString();
                builder.Password = sqlCredentials.GetField("Password").ToString();

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    string sqlQuery = sqlCredentials.GetField("Query").ToString();
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        connection.Open();
                                               
                        using (SqlDataReader reader = command.ExecuteReader())
                        {                          
                            while (reader.Read())
                            {
                                if (reader.HasRows)
                                {
                                    ExpandoObject row = new ExpandoObject();

                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        row.TryAdd(reader.GetName(i), reader.GetValue(i));
                                    }
                                    outputData.Add(row);
                                }
                                else
                                {
                                    throw new Exception("There is no data selected!");
                                   
                                }
                            }
                        }
                        connection.Close();
                    }
                }

                output.Message = JsonSerializer.Serialize(outputData);
                output.Status = Output.JobStatus.Success.ToString();

            }
            catch (SqlException ex)
            {
                
                object exeptionObject = new
                {
                    error = ex.Message,
                    InnerExeption = ex.InnerException,
                    errors = ex.Errors
                };

                dynamic exeptionList = new List<dynamic>();
                exeptionList.Add(exeptionObject);


                output.Message = JsonSerializer.Serialize(exeptionList);
                output.Status = Output.JobStatus.Error.ToString();

                return JsonSerializer.Serialize(output);

            }

            return JsonSerializer.Serialize(output);
            
            
        }
    }
}
