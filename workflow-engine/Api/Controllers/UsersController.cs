using Api.ViewModels.Users;
using Common.Repositories;
using Common.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Common.Mapper;
using Common.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            MapperConfiguration config = new MapperConfiguration(config => config
                                                              .AddProfile(new MapperProfile()));
            IMapper mapper = config.CreateMapper();

            FlowEngineManagementDbContext context = new FlowEngineManagementDbContext();
            List<User> items = new List<User>();

            items =  context.Users.ToList();
            

            return Ok(new
            {
                success = true,
                data = items
            });
        }
        
        [HttpPut]
        public IActionResult Put([FromBody]CreateVM model)
        {
            MapperConfiguration config = new MapperConfiguration(config => config
                                                              .AddProfile(new MapperProfile()));
            IMapper mapper = config.CreateMapper();

            FlowEngineManagementDbContext context = new FlowEngineManagementDbContext();
            User item = new User();

            item.Username = model.Username;
            item.Password = model.Password;
            item.FirstName = model.FirstName;
            item.LastName = model.LastName;
            item.Phone = model.Phone;
            item.Email = model.Email;
            item.Role = "basic";

            context.Users.Add(item);
            context.SaveChanges();

            return Ok(new
            {
                success = true,
                data = mapper.Map<UserDto>(item)
            });
        }
        [HttpPost]
        public IActionResult Post([FromBody]EditVM model)
        {
            MapperConfiguration config = new MapperConfiguration(config => config
                                                              .AddProfile(new MapperProfile()));
            IMapper mapper = config.CreateMapper();

            FlowEngineManagementDbContext context = new FlowEngineManagementDbContext();
            User item = context.Users.Where(x => x.Id == model.Id).FirstOrDefault();

            if(item == null)
            {
                return NotFound(new
                {
                    message = "User doesnt exist"
                });
            }

            item.Id = model.Id;
            item.Username = model.Username;
            item.Password = model.Password;
            item.FirstName = model.FirstName;
            item.LastName = model.LastName;
            item.Phone = model.Phone;
            item.Email = model.Email;

            context.Users.Update(item);
            context.SaveChanges();

            return Ok(new
            {
                success = true,
                data = mapper.Map<UserDto>(item)
            });
        }
        [HttpDelete]
        public IActionResult Delete(int Id)
        {

            MapperConfiguration config = new MapperConfiguration(config => config
                                                              .AddProfile(new MapperProfile()));
            IMapper mapper = config.CreateMapper();

            FlowEngineManagementDbContext context = new FlowEngineManagementDbContext();

            User item = context.Users.Where(x => x.Id == Id).FirstOrDefault();
            FlowExecutions flowExecutions = context.FlowExecutions.Where(x => x.OwnerId == Id).FirstOrDefault();


            if (item == null)
            {
                return NotFound(new
                {
                    message = "User doesnt exist"
                });
            }
            else if (flowExecutions != null)
            {
                return Problem("The User has attached Projects");
            }

            context.Users.Remove(item);
            context.SaveChanges();

            return Ok(new
            {
                success = true,
                data = mapper.Map<UserDto>(item)
            });
        }
    }
}
