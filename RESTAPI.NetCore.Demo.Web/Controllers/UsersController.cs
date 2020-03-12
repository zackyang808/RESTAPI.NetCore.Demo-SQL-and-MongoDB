using Microsoft.AspNetCore.Mvc;
using RESTAPI.NetCore.Demo.Common.Models;
using RESTAPI.NetCore.Demo.Web.Domain.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RESTAPI.NetCore.Demo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        //api/user?name=x&pagesize=x&pagenum=x
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]  string name = "", int pageSize = 1, int pageNum = 1)
        {
            try
            {
                var users = await _userService.Get(name, pageSize, pageNum);
                if (users == null || users.Count == 0)
                {
                    return Json(new { IsSuccess = false, Message = "User not found" });
                }

                return Json(new { IsSuccess = true, Users = users });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Message = e.Message
                });
            }
        }

        //api/user/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                if (id == null)
                {
                    return Json(new { IsSuccess = false, Message = "Id is required" });
                }

                var user = await _userService.GetById(id);
                if (user == null)
                {
                    return Json(new { IsSuccess = false, Message = "User not found" });
                }

                return Json(new { IsSuccess = true, User = user });
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, Message = e.Message });
            }
        }

        //api/user?count=x
        [HttpPost]
        public async Task<IActionResult> Generate([FromQuery] int count)
        {
            try
            {
                var usersGenerated = await _userService.BulkGenerate(count);

                return Json(new { IsSuccess = true, Users = usersGenerated });

            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, Message = e.Message });
            }
        }

        //api/user
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { IsSuccess = false, Message = "User is invalid" });
                }

                var userUpdated = await _userService.Update(user);

                return Json(new { IsSuccess = true, User = userUpdated });

            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, Message = e.Message });
            }
        }

        //api/user/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (id == null)
                {
                    return Json(new { IsSuccess = false, Message = "Id is required" });
                }

                await _userService.Delete(id);

                return Json(new { IsSuccess = true });

            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, Message = e.Message });
            }
        }
    }
}