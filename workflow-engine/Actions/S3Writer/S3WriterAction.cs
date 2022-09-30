using System;
using System.Collections.Generic;
using System.Dynamic;
using Amazon;
using System.Text.Json;
using WorkflowActionSdk;
using Amazon.S3;
using WorkflowActionSdk.Extentions;
using System.IO;
using Amazon.S3.Model;
using System.Linq;
using System.Reflection;
using ExecutionEngineCli;

namespace S3Writer
{
    public class S3WriterAction : IWorkflowAction
    {
        public IWorkflowAction.WorkflowActionStatus Status { get; set; }

        public string Execute(string input, List<ExpandoObject> payload)
        {
            ExpandoObject awsCredentials = JsonSerializer.Deserialize<ExpandoObject>(input);

            dynamic outputRes = new List<dynamic>();

            Output output = new Output();
            
            try
            {
                string bucketName = awsCredentials.GetField("S3BucketName").ToString();
                string accessKey = awsCredentials.GetField("AccessKey").ToString();
                string secretKey = awsCredentials.GetField("SecretKey").ToString();
                string keyName = awsCredentials.GetField("KeyName").ToString();
                string region = awsCredentials.GetField("Region").ToString();
                string contentType = awsCredentials.GetField("Content-type").ToString() ?? "application/text";
                string fileType = contentType.Split("/")[1].ToString();


                awsCredentials.GetField("PutDateToName").ToString();

                if (awsCredentials.GetField("PutDateToName").ToString().ToLower() == "yes")
                {
                    keyName += "-" + DateTime.UtcNow.ToString();
                }


                IAmazonS3 client = new AmazonS3Client(accessKey,secretKey,RegionEndpoint.GetBySystemName(region));

                using (var stream = new MemoryStream())
                {
                    using(var writer = new StreamWriter(stream))
                    {

                        if(fileType == "csv")
                        {
                            string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                            path = EngineCore.GetSolutionDir(path).ToString() + "\\GeneratedFiles\\CSVParser.csv";

                            if (File.Exists(path))
                            {
                                writer.Write(File.ReadAllText(path));
                            }
                            else if(JsonSerializer.Serialize(payload).Contains("CsvStringDataPreview"))
                            {
                                writer.Write(payload[0].GetField("CsvStringDataPreview").ToString());
                            }
                            else
                            {
                                throw new FileNotFoundException("CSVGenerated file not found. Are you sure that content-type is correct");
                            }

                        }
                        else if (fileType == "json")
                        {
                            string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                            path = EngineCore.GetSolutionDir(path).ToString() + "\\GeneratedFiles\\JSONParser.csv";

                            if (File.Exists(path))
                            {
                                writer.Write(File.ReadAllText(path));
                            }
                            else if(payload != null)
                            {
                                writer.Write(payload);
                            }
                            else
                            {
                                throw new FileNotFoundException("JSONGenerated file and payload are not found! Are you sure that content-type is correct?");
                            }
                        }
                        else
                        {
                            if(payload != null)
                            {
                                writer.Write(payload);
                            }

                            throw new InvalidDataException("There is no file or payload ! Are you sure that content-type is correct and pass data to this object? ");

                        }
                      
                        writer.Flush();

                        var put = new PutObjectRequest
                        {
                            BucketName = bucketName,
                            Key = keyName + "." + fileType,
                            InputStream = stream,
                            ContentType = contentType
                        };

                        PutObjectResponse res = client.PutObjectAsync(put).Result;

                        outputRes.Add(new
                        {
                            S3BucketName = bucketName,
                            FileName = keyName + "." + fileType,
                            Region = region,
                            AWSResponse = new { 
                                status = res.HttpStatusCode,
                                responseMetatada = res.ResponseMetadata
                            }
                            
                        });;
                    }
                
                }

                output.Message = JsonSerializer.Serialize(outputRes);
                output.Status = Output.JobStatus.Success.ToString();
            }
            catch (Exception ex)
            {
                object exeption = new
                {
                    error = ex.Message,
                    InnerExeption = ex.InnerException
                };

                
                outputRes.Add(exeption);


                output.Message = JsonSerializer.Serialize(outputRes);
                output.Status = Output.JobStatus.Error.ToString();
            }


            return JsonSerializer.Serialize(output);            
        }       
    }
}
