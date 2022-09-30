using Api.Sevices;
using Api.ViewModels.Configuration;
using Common.Entities;
using Common.Repositories;
using ExecutionEngineCli;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Linq;
using System.Text.Json;

namespace Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class FlowController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(int Id)
        {
            FlowEngineManagementDbContext context = new FlowEngineManagementDbContext();
            FlowExecutions item = context.FlowExecutions.Where(x => x.OwnerId == Id).FirstOrDefault();
            object jsonConfig = new { };
            if (item == null)
            {

            }
            else
            {
                jsonConfig = JsonSerializer.Deserialize(item.FlowConfig, typeof(object));
            }



            return Ok(new
            {
                success = true,
                config = JsonSerializer.Serialize(jsonConfig)
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateVM content)
        {
            FlowEngineManagementDbContext context = new FlowEngineManagementDbContext();
            FlowExecutions item = context.FlowExecutions.Where(x => x.OwnerId == content.Id).FirstOrDefault();

            string flowConfig = JsonSerializer.Serialize(content.FlowConfig);

            if(item == null)
            {
                item = new FlowExecutions();
                item.FlowConfig = content.FlowConfig;
                item.OwnerId = content.Id;
                item.Status = "Waiting Execution";

                context.FlowExecutions.Add(item);
            }
            else
            {
                item.FlowConfig = content.FlowConfig;
                item.Status = "Waiting Execution";
                item.UpdateTime = System.DateTime.Now;
                context.FlowExecutions.Update(item);
            }


            context.SaveChanges();


            return Ok(new
            {
                success = true,
                config = content.FlowConfig
            });
        }
        [HttpExecute]
        public IActionResult Execute(int Id)
        {
            FlowEngineManagementDbContext context = new FlowEngineManagementDbContext();

            object a = EngineCore.WorkFlowExecution(Id);
            FlowExecutions item = context.FlowExecutions.Where(x => x.OwnerId == Id).FirstOrDefault();

            FlowConfigurationHistory history = new FlowConfigurationHistory();
            history.FlowConfig = item.FlowConfig;
            history.FlowResult = item.FlowResult;
            history.Status = item.Status;
            history.OwnerId = item.OwnerId;
            history.ExecutionTime = System.DateTime.Now;

            context.FlowConfigurationHistory.Add(history);
            context.SaveChanges();

            return Ok(new
            {
                status = "Starting",
                result = item.FlowResult
            });
        }
    }
}
