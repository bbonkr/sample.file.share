namespace Sample.Mediator.UserDomain.Models
{
    public class UserBaseModel
    {
        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }
    }

    public class UserModel: UserBaseModel
    {
        public string Id { get; set; }
        
    }


}
