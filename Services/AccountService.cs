using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VotingSystem.API.DataAccess;
using VotingSystem.API.Models;

namespace VotingSystem.API.Services
{
    public class AccountService : IAccountService
    {
        private VotingContext _context;

        public AccountService(VotingContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Election>> GetElectionList()
        {
            var elections = await _context.Elections
                .AsNoTracking()
                .ToListAsync();
            return elections;
        }

        public async Task<ICollection<Candidate>> GetCandidatesByElection(int electionId)
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

        public async Task<SystemUser> GetUserDetails(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            return user;
        }

        public async Task<bool> UpdateVoter(Voter user)
        {
            _context.Voters.Update(user);
            return await Save();
        }

        public async Task<bool> UpdateCandidate(Candidate user)
        {
            _context.Candidates.Update(user);
            return await Save();
        }

        public async Task<bool> CastVote(int userId, int candidateId)
        {
            var candidate = await _context.Candidates.FirstOrDefaultAsync(x => x.Id == candidateId);
            var currentUser = await _context.Voters.FirstOrDefaultAsync(x => x.Id == userId);
            if (currentUser.HasVoted == true)
            {
                return false;
            }
            candidate.TotalVotes = candidate.TotalVotes + 1;
            currentUser.HasVoted = true;
            _context.Candidates.Update(candidate);
            _context.Voters.Update(currentUser);
            return await Save();
        }

        public async Task<bool> ApplyForElection(int userId, int electionId)
        {
            var candidate = await _context.Candidates.FirstOrDefaultAsync(x => x.Id == userId);
            var election = await GetElectionById(electionId);
            candidate.ParticipatingIn = electionId;
            candidate.CanApply = false;
            if (candidate == null)
            {
                return false;
            }
            await UpdateCandidate(candidate);
            return await Save();
        }

        public async Task<bool> ElectionExists(int electionId)
        {
            return await _context.Elections.AnyAsync(x => x.Id == electionId);
        }

        private async Task<Election> GetElectionById(int electionId)
        {
            return await _context.Elections.FirstOrDefaultAsync(x => x.Id == electionId);
        }

        private async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}