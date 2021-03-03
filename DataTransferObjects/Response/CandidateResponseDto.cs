namespace VotingSystem.API.DataTransferObjects.Response
{
    public class CandidateResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string DOB { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Occupation { get; set; }
        public string PoliticalParty { get; set; }
        public bool IsVerified { get; set; }
        public bool CanApply { get; set; }
        public int ParticipatingIn { get; set; }
    }
}