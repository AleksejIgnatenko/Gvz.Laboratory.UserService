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
        private readonly IJwtProvider _jwtProvider;

        public UserController(IUserService userService, IJwtProvider jwtProvider)
        {
            _userService = userService;
            _jwtProvider = jwtProvider;
        }

        [HttpPost]
        [Route("registration")]
        public async Task<ActionResult> UserRegistrationAsync([FromBody] UserRegistrationRequest userRegistrationRequest)
        {
            var token = await _userService.CreateUserAsync(Guid.NewGuid(),
                                                            UserRole.Admin,
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
        [Authorize(Roles = "Admin,Manager")]
        [Route("getUsersForPage")]
        public async Task<ActionResult> GetUsersForPageAsync(int pageNumber)
        {
            var (users, numberUsers) = await _userService.GetUsersForPageAsync(pageNumber);

            var response = users.Select(u => new GetUsersResponse(
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
        [Authorize(Roles = "Admin,Manager")]
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

        [HttpGet]
        [Authorize]
        [Route("getUser")]
        public async Task<ActionResult> GetUserAsync()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var userId = _jwtProvider.GetUserIdFromToken(token);

            var user = await  _userService.GetUserByIdAsync(userId);

            var response = new GetUsersResponse(
                            user.Id,
                            user.Role.ToString(),
                            user.Surname,
                            user.UserName,
                            user.Patronymic,
                            user.Email);

            return Ok(response);
        }

        [HttpGet]
        [Route("exportUsersToExcel")]
        [Authorize]
        public async Task<ActionResult> ExportUsersToExcelAsync()
        {
            var stream = await _userService.ExportUsersToExcelAsync();
            var fileName = "Users.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet]
        [Authorize]
        [Route("searchUsers")]
        public async Task<ActionResult> SearchUsersAsync(string searchQuery, int pageNumber)
        {
            var (users, numberUsers) = await _userService.SearchUsersAsync(searchQuery, pageNumber);
            var response = users.Select(u => new GetUsersResponse(
                u.Id,
                u.Role.ToString(),
                u.Surname,
                u.UserName,
                u.Patronymic,
                u.Email)).ToList();

            var responseWrapper = new GetUsersForPageResponseWrapper(response, numberUsers);

            return Ok(responseWrapper);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Manager")]
        [Route("{id:guid}")]
        public async Task<ActionResult> UpdateUserAsync(Guid id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            if (Enum.TryParse<UserRole>(updateUserRequest.Role, true, out var role))
            {
                int roleValue = (int)role;
                roleValue--;

                if (Enum.IsDefined(typeof(UserRole), roleValue))
                {
                    UserRole updatedRole = (UserRole)roleValue;

                    await _userService.UpdateUserAsync(id, updatedRole, updateUserRequest.Surname, updateUserRequest.UserName, updateUserRequest.Patronymic);
                    return Ok();
                }
                else
                {
                    return BadRequest("Ошибка преобразования роли.");
                }
            }
            return BadRequest("Неверная роль пользователя.");
        }

        [HttpPut]
        [Authorize]
        [Route("updateUserDetails")]
        public async Task<ActionResult> UpdateUserDetailsAsync([FromBody] UpdateUserDetailsRequest updateUserDetails)
        {
            var userId = await _userService.UpdateUserDetailsAsync(updateUserDetails.Id, updateUserDetails.Surname, updateUserDetails.UserName, updateUserDetails.Patronymic);

            return Ok();
        }
    }
}
