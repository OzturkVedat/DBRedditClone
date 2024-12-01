namespace DBRedditClone.Models
{
    public class UserModel
    {
        public Guid UserID { get; set; }= Guid.NewGuid();
        public string UserName { get; set; }
        public string Email { get; set; }
        public int Karma { get; set; } = 0;
    }

    public class UserRegisterDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password {  get; set; }
    }
}
