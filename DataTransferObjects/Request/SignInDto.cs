using System.ComponentModel.DataAnnotations;

namespace VotingSystem.API.DataTransferObjects.Request
{
    public class SignInDto
    {
        [Required]
        public string UserName { get; set; }
        // [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}