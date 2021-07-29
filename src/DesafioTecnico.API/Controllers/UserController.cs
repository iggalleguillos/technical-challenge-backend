using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechnicalChallenge.Application.User.Commands;
using TechnicalChallenge.Application.User.Queries;

namespace TechnicalChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserAuthQueries _userAuthQueries;
        private readonly IMediator _mediator;

        public UserController(IUserAuthQueries userAuthQueries, IMediator mediator)
        {
            _userAuthQueries = userAuthQueries;
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [Route("{username}")]
        [ProducesResponseType(typeof(FindUserModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            return Ok(await _userAuthQueries.GetUserByUsername(username));
        }

        [HttpPost]
        [Route("auth")]
        [ProducesResponseType(typeof(AccessTokenVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAccessToken([FromBody] AuthModel model)
        {
            var result = await _userAuthQueries.GetAccessToken(model);

            return Ok(result);
        }

        /// <summary>
        /// test
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(CreatedUserDTO), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            var result = await _mediator.Send(command);

            return Created(string.Empty, result);
        }

        [HttpPut]
        [Route("")]
        [Authorize]
        [ProducesResponseType(typeof(UpdatedUserDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok();
        }

        [HttpDelete]
        [Route("{username}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromRoute] string username)
        {
            await _mediator.Send(new DeleteUserCommand() { UserName = username });

            return NoContent();
        }

    }
}
