namespace Backend.Models
{
    public class User
    {
        #nullable disable
        public int Id { get; set; }
        required public string Username { get; set; }
        required public string Email { get; set; }
        public string PasswordHash { get; set; }
        required public string Role { get; set; }
    }
}