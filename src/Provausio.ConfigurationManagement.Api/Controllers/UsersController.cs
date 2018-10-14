using System.IdentityModel.Tokens.Jwt;
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
    [Authorize]
    [Route("/users")]
    public class UsersController : Controller
    {
        private readonly UserManager<UserData> _userManager;
        private readonly SignInManager<UserData> _signInManager;
        private readonly ITokenService _tokenService;

        public UsersController(
            UserManager<UserData> userManager,
            SignInManager<UserData> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
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

        [HttpPost, Route("login"), AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginInfo login)
        {
            var user = await _userManager.FindByNameAsync(login.Username).ConfigureAwait(false);
            if (user == null) return NotFound();
            
            var x = await _signInManager
                .PasswordSignInAsync(user.NormalizedUserName, login.Password, false, false)
                .ConfigureAwait(false);
            
            if (!x.Succeeded) return Unauthorized();
            
            var accessToken = _tokenService.GenerateToken(user);
            
            return Ok(new
            {
                Token = accessToken
            });
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