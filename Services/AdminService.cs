using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VotingSystem.API.DataAccess;
using VotingSystem.API.Models;

namespace VotingSystem.API.Services
{
    public class AdminService : IAdminService
    {

        private VotingContext _context;

        public AdminService(VotingContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Voter>> GetVoterList()
        {
            var users = await _context.Voters.Where(x => x.UserRole == "Voter").ToListAsync();
            return users;
        }

        public async Task<ICollection<Candidate>> GetCandidateList()
        {
            var users = await _context.Candidates
                .Where(x => x.UserRole == "Candidate" && x.IsDeleted == false)
                .ToListAsync();
            return users;
        }

        public async Task<ICollection<Election>> GetElectionList()
        {
            var elections = await _context.Elections.ToListAsync();
            return elections;
        }

        public async Task<ICollection<Candidate>> GetCandidateApplications(int electionId)
        {
            var candidates = await _context.Candidates.Where(x => x.ParticipatingIn == electionId).ToListAsync();
            return candidates;
        }

        public async Task<Voter> GetVoterById(int id)
        {
            var user = await _context.Voters.FirstOrDefaultAsync(x => x.Id == id);
            return user;
        }

        public async Task<Candidate> GetCandidateById(int id)
        {
            var user = await _context.Candidates.FirstOrDefaultAsync(x => x.Id == id);
            return user;
        }

        public async Task<Election> GetElectionById(int id)
        {
            var election = await _context.Elections.FirstOrDefaultAsync(x => x.Id == id);
            return election;
        }

        public async Task<bool> VerifyCandidate(int id)
        {
            var user = await _context.Candidates.FirstOrDefaultAsync(u => u.Id == id);
            user.IsVerified = true;
            return await Save();
        }

        public async Task<Election> AddElection(Election election)
        {
            await _context.AddAsync(election);
            await Save();
            return election;
        }

        public async Task<bool> AddCandidateToElection(int electionId, int candidateId)
        {
            var election = await _context.Elections.FirstOrDefaultAsync(x => x.Id == electionId);
            var user = await GetCandidateById(candidateId);
            user.CanApply = false;
            election.CandidateList.Add(user);
            _context.Elections.Update(election);
            return await Save();
        }

        public async Task<bool> UpdateElection(Election election)
        {
            _context.Elections.Update(election);
            return await Save();
        }

        public async Task<bool> UserExists(int userId)
        {
            if (await _context.Users.AnyAsync(x => x.Id == userId))
            {
                return true;
            }
            return false;
        }
        public async Task<bool> ElectionExists(int electionId)
        {
            if (await _context.Elections.AnyAsync(x => x.Id == electionId))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteByUserId(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            user.IsDeleted = true;
            return await Save();
        }

        private async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}