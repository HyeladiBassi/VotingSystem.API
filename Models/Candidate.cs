namespace VotingSystem.API.Models
{
    public class Candidate : SystemUser
    {
        public bool CanApply { get; set; } = true;
        public bool IsVerified { get; set; } = false;
        public string Occupation { get; set; }
        public string PoliticalParty { get; set; }
        public int TotalVotes { get; set; }
        public int ParticipatingIn { get; set; }
    }
}