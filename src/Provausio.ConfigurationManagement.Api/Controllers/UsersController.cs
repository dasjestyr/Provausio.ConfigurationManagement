using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Provausio.ConfigurationManagement.Api.Auth;
using Provausio.ConfigurationManagement.Api.Data.Schemas;
using Provausio.ConfigurationManagement.Api.Model.UserManagement;
using XidNet;

namespace Provausio.ConfigurationManagement.Api.Controllers
{
    [Authorize(Roles = "global_admin, user_admin")]
    [Route("/users")]
    public class UsersController : Controller
    {
        private readonly UserManager<UserData> _userManager;
        private readonly SignInManager<UserData> _signInManager;
        private readonly ITokenHandler _tokenHandler;

        public UsersController(
            UserManager<UserData> userManager,
            SignInManager<UserData> signInManager,
            ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
        }

        [HttpPost, Route("login"), AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginInfo login)
        {
            var user = await _userManager.FindByNameAsync(login.Username).ConfigureAwait(false);
            if (user == null) return NotFound();

            var x = await _signInManager
                .PasswordSignInAsync(user.NormalizedUserName, login.Password, false, false)
                .ConfigureAwait(false);

            if (!x.Succeeded) return Unauthorized();

            var accessToken = _tokenHandler.GenerateToken(user);

            return Ok(new
            {
                Token = accessToken
            });
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> RegisterUser([FromBody] UserInfo payload)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new UserData
            {
                FirstName = payload.FirstName,
                LastName = payload.LastName,
                Username = payload.Username,
                Email = payload.Email,
                Password = payload.Password,
                NormalizedUserName = payload.Username,
                NormalizedEmail = payload.Email,
                UserId = Xid.NewXid().ToString()
            };

            var result = await _userManager.CreateAsync(user, payload.Password).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                    ModelState.AddModelError(error.Code, error.Description);
                
                return StatusCode(500, ModelState);
            }

            return Created($"/users/{user.UserId}", user);
        }

        [HttpPost, Route("{userId}/roles/{role}")]
        public async Task<IActionResult> AddUserRole(string userid, string role)
        {
            var user = await _userManager.FindByIdAsync(userid);
            if (user == null) return NotFound();
            await _userManager.AddToRoleAsync(user, role);
            return Ok();
        }

        [HttpGet, Route("testToken")]
        public async Task<IActionResult> TestToken()
        {
            return Ok(new
            {
                Result = "Success",
                Name = User.Identity.Name
            });
        }
    }
}