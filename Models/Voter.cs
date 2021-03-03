namespace VotingSystem.API.Models
{
    public class Voter : SystemUser
    {
        public bool CanVote { get; set; }
        public bool HasVoted { get; set; }
    }
}