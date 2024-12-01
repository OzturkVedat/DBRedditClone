namespace DBRedditClone.Models
{
    public class UserModel
    {
        public string UserId { get; set; }= Guid.NewGuid().ToString();
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash {  get; set; }
        public int Karma { get; set; } = 0;
    }

    public class UserPublicDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }    
        public int Karma {  get; set; }
            
    }

    public class UserRegisterDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password {  get; set; }
    }

    public class UserUpdateDto
    {
        public string UserId {  get; set; }
        public string UserName { get; set; }
        public string NewPassword { get; set; }
    }

}
