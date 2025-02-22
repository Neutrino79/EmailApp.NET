namespace EmailApp.Models.IEntities
{
    public interface IUser
    {
        int UserId { get; set; }
        string Name { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        bool IsSubscribed { get; set; }
    }
}
