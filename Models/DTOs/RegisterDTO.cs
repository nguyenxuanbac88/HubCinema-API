using System.ComponentModel.DataAnnotations;

namespace API_Project.Models.DTOs
{
    public class RegisterDTO
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTime dob { get; set; }
        public bool gender { get; set; }
        public string zoneAddress { get; set; }
    }
}
