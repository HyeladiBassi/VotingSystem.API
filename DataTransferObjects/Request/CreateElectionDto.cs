using System;
using System.Collections.Generic;
using VotingSystem.API.Models;

namespace VotingSystem.API.DataTransferObjects.Request
{
    public class CreateElectionDto
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public string Country { get; set; }
        public DateTime ElectionDate { get; set; }
        public string ElectionType { get; set; }
        public ICollection<Candidate> CandidateList { get; set; }
    }
}