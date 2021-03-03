using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VotingSystem.API.DataTransferObjects.Request;
using VotingSystem.API.DataTransferObjects.Response;
using VotingSystem.API.Models;
using VotingSystem.API.Services;

namespace VotingSystem.API.Controllers
{
    [Route("api")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private IAuthService _context;
        private IMapper _mapper;
        private IConfiguration _config;

        public AuthController(IConfiguration config, IAuthService context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _config = config;

        }

        /// <summary>
        /// Login
        /// </summary>
        [AllowAnonymous]
        [HttpPost("auth/login")]
        public async Task<IActionResult> Login(SignInDto userForLogin)
        {
            userForLogin.UserName = userForLogin.UserName.ToLower();
            var userFromRepo = await _context.Login(userForLogin.UserName, userForLogin.Password);
            if (userFromRepo == null)
            {
                return Unauthorized();
            }
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.UserName),
                new Claim(ClaimTypes.Role, userFromRepo.UserRole)
            };
            var secret = _config.GetSection("jwtSettings:Secret").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var user = _mapper.Map<UserResponseDto>(userFromRepo);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });
        }

        /// <summary>
        /// Register as voter
        /// </summary>
        [AllowAnonymous]
        [HttpPost("auth/register/voter")]
        public async Task<IActionResult> RegisterAsVoter(CreateVoterDto userDto)
        {
            var userToCreate = new Voter();

            userToCreate = _mapper.Map<Voter>(userDto);
            userToCreate.UserName = userDto.FirstName.Substring(0, 1) + userDto.LastName.Substring(0, 1) + userDto.NationalId;
            userToCreate.UserName = userToCreate.UserName.ToLower();
            
            if (await _context.UserExists(userToCreate.UserName))
            {
                return BadRequest("User already exists");
            }

            var createdUser = await _context.RegisterAsVoter(userToCreate, userDto.Password);
            var response = _mapper.Map<UserResponseDto>(createdUser);

            Email(userToCreate.UserName, response.Email, response.FirstName);

            return Ok(response);
        }

        /// <summary>
        /// Register as candidate
        /// </summary>
        [AllowAnonymous]
        [HttpPost("auth/register/candidate")]
        public async Task<IActionResult> RegisterAsCandidate(CreateCandidateDto userDto)
        {
            var userToCreate = new Candidate();

            userToCreate = _mapper.Map<Candidate>(userDto);
            userToCreate.UserName = userDto.FirstName.Substring(0, 1) + userDto.LastName.Substring(0, 1) + userDto.NationalId;
            userToCreate.UserName = userToCreate.UserName.ToLower();

            if (await _context.UserExists(userToCreate.UserName))
            {
                return BadRequest("User already exists");
            }

            var createdUser = await _context.RegisterAsCandidate(userToCreate, userDto.Password);
            var response = _mapper.Map<UserResponseDto>(createdUser);

            // Email(userToCreate.UserName, response.Email, response.FirstName);

            return Ok(response);
        }

        private void Email(string htmlMessage, string toEmail, string subject)
        {
            try
            {
                MailMessage message = new MailMessage();
                var key = _config.GetSection("sendGrid:api_key").Value;

                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("bassihyeladi@gmail.com");
                message.To.Add(new MailAddress(toEmail));
                message.Subject = "UserName for " + subject;
                message.IsBodyHtml = true;
                message.Body = "Your username is " + htmlMessage + ". Do not share with anyone";
                smtp.Port = 587;
                smtp.Host = "smtp.sendgrid.net";
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("apikey", key);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.Send(message);
            }
            catch (SmtpException ex)
            {
                string msg = "Mail cannot be sent because of the server problem:";
                msg += ex.Message;
                Console.WriteLine("Error: Inside catch block of Mail sending");
                Console.WriteLine("Error msg:" + ex);
                Console.WriteLine("Stack trace:" + ex.StackTrace);
                Console.WriteLine("Something went wrong!!");
            }

        }
    }
}