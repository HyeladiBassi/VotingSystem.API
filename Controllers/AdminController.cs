using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VotingSystem.API.DataTransferObjects.Request;
using VotingSystem.API.DataTransferObjects.Response;
using VotingSystem.API.Models;
using VotingSystem.API.Services;

namespace VotingSystem.API.Controllers
{
    [Route("api")]
    [Produces("application/json")]
    public class AdminController : ControllerBase
    {

        private IAdminService _context;
        private IMapper _mapper;
        private IConfiguration _config;

        public AdminController(IConfiguration config, IAdminService context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _config = config;

        }

        /// <summary>
        /// Get list of candidates
        /// </summary>
        [HttpGet("admin/candidates")]
        public async Task<IActionResult> GetCandidates()
        {
            var users = await _context.GetCandidateList();
            var candidates = _mapper.Map<ICollection<CandidateResponseDto>>(users);
            return Ok(candidates);
        }

        /// <summary>
        /// Get list of voters
        /// </summary>
        [HttpGet("admin/voters")]
        public async Task<IActionResult> GetVoters()
        {
            var users = await _context.GetVoterList();
            var voters = _mapper.Map<ICollection<UserResponseDto>>(users);
            return Ok(voters);
        }

        /// <summary>
        /// Get list of elections
        /// </summary>
        [HttpGet("admin/elections")]
        public async Task<IActionResult> GetElections()
        {
            var elections = await _context.GetElectionList();
            var mappedElections = _mapper.Map<ICollection<ElectionResponseDto>>(elections);
            return Ok(mappedElections);
        }

        /// <summary>
        /// Get candidate applications by election Id
        /// </summary>
        [HttpGet("admin/election/candidate-applications")]
        public async Task<IActionResult> GetElectionCandidateApplications(int electionId)
        {
            var users = await _context.GetCandidateApplications(electionId);
            var candidates = _mapper.Map<ICollection<CandidateResponseDto>>(users);
            return Ok(candidates);
        }

        /// <summary>
        /// Get candidate details by candidate Id
        /// </summary>
        [HttpGet("admin/candidate/{userId}")]
        public async Task<IActionResult> GetCandidateDetails(int userId)
        {
            var details = await _context.GetCandidateById(userId);
            if(details == null)
            {
                return NotFound();
            }
            var mappedDetails = _mapper.Map<CandidateResponseDto>(details);
            return Ok(mappedDetails);
        }

        /// <summary>
        /// Get voter details by voter Id
        /// </summary>
        [HttpGet("admin/voter/{userId}")]
        public async Task<IActionResult> GetVoterDetails(int userId)
        {
            var details = await _context.GetVoterById(userId);
            if(details == null)
            {
                return NotFound();
            }
            var mappedDetails = _mapper.Map<UserResponseDto>(details);
            return Ok(mappedDetails);
        }

        /// <summary>
        /// Get election details by election Id
        /// </summary>
        [HttpGet("admin/election/{electionId}")]
        public async Task<IActionResult> GetElectionById(int electionId)
        {
            var election = await _context.GetElectionById(electionId);
            var mappedElection = _mapper.Map<ElectionResponseDto>(election);
            return Ok(mappedElection);
        }

        /// <summary>
        /// Create new election
        /// </summary>
        [HttpPost("admin/election/add-new")]
        public async Task<IActionResult> CreateNewElection(CreateElectionDto electionDto)
        {
            var electionToCreate = _mapper.Map<Election>(electionDto);
            var createdElection = await _context.AddElection(electionToCreate);
            var response = _mapper.Map<ElectionResponseDto>(createdElection);
            return Ok(response);
        }

        /// <summary>
        /// Add candidate to election
        /// </summary>
        [HttpPost("admin/election/{electionId}/add-candidate/{candidateId}")]
        public async Task<IActionResult> AddCandidate(int electionId, int candidateId)
        {
            if (!await _context.UserExists(candidateId))
            {
                return BadRequest("User does not exist");
            }
            if (!await _context.ElectionExists(electionId))
            {
                return BadRequest("Election does not exist");
            }
            await _context.AddCandidateToElection(electionId,candidateId);
            return Ok();
        }

        /// <summary>
        /// Verify candidate
        /// </summary>
        [HttpPost("admin/verify-candidate/{userId}")]
        public async Task<IActionResult> VerifyCandidate(int userId)
        {
            var response = await _context.VerifyCandidate(userId);
            if (response)
            {
                return Ok();
            }
            return BadRequest("Something went wrong !!!");
        }

        /// <summary>
        /// Update election details
        /// </summary>
        [HttpPut("admin/election/update/{electionId}")]
        public async Task<IActionResult> UpdateElection(int electionId, UpdateElectionDto electionDto)
        {
            var currentElection = await _context.GetElectionById(electionId);
            var updatedElection = _mapper.Map<UpdateElectionDto, Election>(electionDto, currentElection);
            if (currentElection == null)
            {
                await _context.UpdateElection(updatedElection);
                return NoContent();
            }
            else
            {
                return NotFound();
            }

        }

        /// <summary>
        /// Delete user by Id
        /// </summary>
        [HttpDelete("admin/deleteuser/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            if (await _context.UserExists(userId))
            {
                await _context.DeleteByUserId(userId);
                return NoContent();
            }
            return BadRequest("User does not exist or already deleted");
        }
    }
}