using System.Collections.Generic;
using System.Threading.Tasks;
using VotingSystem.API.Models;

namespace VotingSystem.API.Services
{
    public interface IAccountService
    {
        Task<ICollection<Election>> GetElectionList();
        Task<ICollection<Candidate>> GetCandidatesByElection(int electionId);
        Task<Voter> GetVoterById(int id);
        Task<Candidate> GetCandidateById(int id);
        Task<SystemUser> GetUserDetails(int userId);
        Task<bool> UpdateCandidate(Candidate user);
        Task<bool> UpdateVoter(Voter user);
        Task<bool> CastVote(int userId, int candidateId);
        Task<bool> ApplyForElection(int userId, int electionId);
        Task<bool> ElectionExists(int electionId);
    }
}