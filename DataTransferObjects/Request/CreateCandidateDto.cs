using System.ComponentModel.DataAnnotations;

namespace VotingSystem.API.DataTransferObjects.Request
{
    public class CreateCandidateDto
    {
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string NationalId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string DOB { get; set; }
        public string Address { get; set; }
        [Required]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        public string Occupation { get; set; }
        public string PoliticalParty { get; set; }
        
    }
}