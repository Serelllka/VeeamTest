namespace Server.Entity
{
    public class User
    {
        public User(string id, string code)
        {
            Id = id;
            Code = code;
        }
        
        public string Id { get; }
        public string Code { get; }
    }
}