using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.API.DataTransferObjects.Request;
using VotingSystem.API.DataTransferObjects.Response;
using VotingSystem.API.Models;
using VotingSystem.API.Services;

namespace VotingSystem.API.Controllers
{
    [Route("api")]
    [Produces("application/json")]
    [Authorize(Roles = "SystemRole.Admin, SystemRole.Candidate, SystemRole.Voter")]
    public class AccountController : ControllerBase
    {
        private IAccountService _context;
        private IMapper _mapper;

        public AccountController(IAccountService context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of elections
        /// </summary>
        [HttpGet("elections")]
        public async Task<IActionResult> GetElections()
        {
            
            var elections = await _context.GetElectionList();
            var response = _mapper.Map<ICollection<ElectionResponseDto>>(elections);
            return Ok(response);
        }

        /// <summary>
        /// Get list of candidates by election id
        /// </summary>
        [HttpGet("election/{electionId}/candidates")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetElectionCandidates(int electionId)
        {
            var candidates = await _context.GetCandidatesByElection(electionId);
            var response = _mapper.Map<ICollection<CandidateResponseDto>>(candidates);
            return Ok(response);
        }
        
        /// <summary>
        /// Get user details
        /// </summary>
        [HttpGet("{userId}/details")]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDetails(int userId)
        {
            // if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            // {
            //     return Unauthorized();
            // }
            var user = await _context.GetUserDetails(userId);
            if (user.UserRole == "Voter")
            {
                var response = _mapper.Map<UserResponseDto>(user);
                return Ok(response);
            }
            else
            {
                var response = _mapper.Map<CandidateResponseDto>(user);
                return Ok(response);
            }
        }

        /// <summary>
        /// Apply for election
        /// </summary>
        [HttpPost("{userId}/apply/{electionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ApplyForElection(int userId, int electionId)
        {
            // if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            // {
            //     return Unauthorized();
            // }
            if (!await _context.ElectionExists(electionId))
            {
                BadRequest("Election does not exitst");
            }
            if(await _context.ApplyForElection(userId, electionId))
            {
                return Ok();
            }
            return BadRequest("You are not eligible to apply !");
        }

        /// <summary>
        /// Cast vote for a candidate
        /// </summary>
        [HttpPost("{userId}/castvote/{candidateId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CastVote(int candidateId, int userId)
        {
            // if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            // {
            //     return Unauthorized();
            // }
            var candidate = await _context.GetCandidateById(candidateId);
            if (candidate.CanApply == true)
            {
                return BadRequest("Candidate is not participating in election");
            }
            var response = await _context.CastVote(userId, candidateId);
            if (response)
            {
                return Ok();
            }
            return BadRequest("Something went wrong");
        }
        
        /// <summary>
        /// Update voter profile
        /// </summary>
        [HttpPut("{userId}/update-voter")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateVoterProfile(int userId, UpdateUserDto updateDto)
        {
            // if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            // {
            //     return Unauthorized();
            // }
            var currentUser = await _context.GetVoterById(userId);
            var updatedUser = _mapper.Map<UpdateUserDto, Voter>(updateDto, currentUser);
            if (currentUser != null)
            {
                await _context.UpdateVoter(updatedUser);
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
        
        /// <summary>
        /// Update candidate profile
        /// </summary>
        [HttpPut("{userId}/update-candidate")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateCandidateProfile(int userId, UpdateCandidateDto updateDto)
        {
            // if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            // {
            //     return Unauthorized();
            // }
            var currentUser = await _context.GetCandidateById(userId);
            var updatedUser = _mapper.Map<UpdateCandidateDto, Candidate>(updateDto, currentUser);
            if (currentUser != null)
            {
                await _context.UpdateCandidate(updatedUser);
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}