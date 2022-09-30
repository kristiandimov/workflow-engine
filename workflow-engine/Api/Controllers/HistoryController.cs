using Common.Entities;
using Common.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        // GET: api/<HistoryController>
        [HttpGet]
        public IActionResult Get(int Id)
        {
            FlowEngineManagementDbContext context = new FlowEngineManagementDbContext();
            List<FlowConfigurationHistory> items = context.FlowConfigurationHistory.Where(x => x.OwnerId == Id).ToList();
            
            if(items == null)
            {
                items = new List<FlowConfigurationHistory>();
            }

            return Ok(new
            {
                success = true,
                data = items
            });
        }

     
    }
}
