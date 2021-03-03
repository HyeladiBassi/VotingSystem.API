using System.Threading.Tasks;
using VotingSystem.API.Models;

namespace VotingSystem.API.Services
{
    public interface IAuthService
    {
        Task<SystemUser> Login(string username, string password);
        Task<SystemUser> RegisterAsVoter(SystemUser user, string password);
        Task<SystemUser> RegisterAsCandidate(SystemUser user, string password);
        Task<bool> UserExists(string username);
    }
}