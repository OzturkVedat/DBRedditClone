namespace DBRedditClone.Models.Users
{
    public class User
    {
        public string UserId { get; set; }= Guid.NewGuid().ToString();
        public string UserName { get; set; }
        public string Email {  get; set; }
        public string Password { get; set; }
        public int Karma {  get; set; }
    }
}
