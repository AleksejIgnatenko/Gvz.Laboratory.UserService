﻿using Gvz.Laboratory.UserService.Abstractions;
using Gvz.Laboratory.UserService.Contracts;
using Gvz.Laboratory.UserService.Enums;
using Gvz.Laboratory.UserService.Models;
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

            return new JsonResult(token);
        }

        [HttpGet]
        [Route("getUsersForPage")]
        public async Task<ActionResult> GetUsersForPageAsync(int page)
        {
            var (users, countUser) = await _userService.GetUsersForPageAsync(page);
            Console.WriteLine(page);
            var response = users.Select(u => new GetUsersForPageResponse(
                u.Id,
                u.Role,
                u.Surname,
                u.UserName,
                u.Patronymic,
                u.Email)).ToList();

            var responseWrapper = new GetUsersForPageResponseWrapper(response, countUser);

            return Ok(responseWrapper);
        }

        [HttpGet]
        [Route("getAllUsers")]
        public async Task<ActionResult<List<UserModel>>> GetAllUsersAsync()
        {
            return Ok(await _userService.GetAllUsersAsync());
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
