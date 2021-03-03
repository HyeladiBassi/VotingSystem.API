using System.Collections.Generic;
using System.Threading.Tasks;
using VotingSystem.API.Models;

namespace VotingSystem.API.Services
{
    public interface IAdminService
    {
        Task<ICollection<Voter>> GetVoterList();
        Task<ICollection<Candidate>> GetCandidateList();
        Task<ICollection<Election>> GetElectionList();
        Task<ICollection<Candidate>> GetCandidateApplications(int electionId);
        Task<Voter> GetVoterById(int id);
        Task<Candidate> GetCandidateById(int id);
        Task<Election> GetElectionById(int id);
        Task<bool> VerifyCandidate(int id);
        Task<Election> AddElection(Election election);
        Task<bool> AddCandidateToElection(int electionId, int candidateId);
        Task<bool> UpdateElection(Election election);
        Task<bool> UserExists(int userId);
        Task<bool> ElectionExists(int electionId);
        Task<bool> DeleteByUserId(int id);
    }
}