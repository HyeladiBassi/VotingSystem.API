using System;
using System.Collections.Generic;

namespace VotingSystem.API.Models
{
    public class Election
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public string Country { get; set; }
        public DateTime ElectionDate { get; set; }
        public string ElectionType { get; set; }
        public int TotalVotes { get; set; }
        public ICollection<SystemUser> CandidateList { get; set; }
    }
}