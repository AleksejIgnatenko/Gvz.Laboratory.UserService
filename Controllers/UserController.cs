using Gvz.Laboratory.UserService.Abstractions;
using Gvz.Laboratory.UserService.Contracts;
using Gvz.Laboratory.UserService.Enums;
using Gvz.Laboratory.UserService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Gvz.Laboratory.UserService.Controllers
{
    //[Remote]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("registration")]
        public async Task<ActionResult> UserRegistrationAsync([FromBody] UserRegistrationRequest userRegistrationRequest)
        {
            var token = await _userService.CreateUserAsync(Guid.NewGuid(),
                                                            UserRole.User,
                                                            userRegistrationRequest.Surname,
                                                            userRegistrationRequest.UserName,
                                                            userRegistrationRequest.Patronymic,
                                                            userRegistrationRequest.Email,
                                                            userRegistrationRequest.Password,
                                                            userRegistrationRequest.RepeatPassword);

            return Ok(new { token });
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> UserLoginAsync([FromBody] UserLoginRequest userLoginRequest)
        {
            var token = await _userService.LoginUserAsync(userLoginRequest.Email, userLoginRequest.Password);

            return Ok(new { token });
        }

        [HttpGet]
        [Route("getUsersForPage")]
        public async Task<ActionResult> GetUsersForPageAsync(int pageNumber)
        {
            var (users, numberUsers) = await _userService.GetUsersForPageAsync(pageNumber);

            var response = users.Select(u => new GetUsersForPageResponse(
                u.Id,
                u.Role.ToString(),
                u.Surname,
                u.UserName,
                u.Patronymic,
                u.Email)).ToList();

            var responseWrapper = new GetUsersForPageResponseWrapper(response, numberUsers);

            return Ok(responseWrapper);
        }

        [HttpGet]
        [Route("getAllUsers")]
        public async Task<ActionResult<List<UserModel>>> GetAllUsersAsync()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("isAdmin")]
        public ActionResult IsAdmin()
        {
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        [Route("isManager")]
        public ActionResult IsManager()
        {
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        [Route("isAdminOrManager")]
        public ActionResult IsAdminOrManager()
        {
            return Ok();
        }

        [HttpPut]
        [Route("updateUser/{id:guid}")]
        public async Task<ActionResult> UpdateUserAsync(Guid id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            await _userService.UpdateUserAsync(id, updateUserRequest.Surname, updateUserRequest.Name, updateUserRequest.Patronymic);
            return Ok();
        }
    }
}
