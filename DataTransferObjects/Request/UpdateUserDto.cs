namespace VotingSystem.API.DataTransferObjects.Request
{
    public class UpdateUserDto
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int NationalId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string DOB { get; set; }
        public string Address { get; set; }
        
    }
}