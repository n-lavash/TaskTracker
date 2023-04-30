using TaskTrackerWeb.Entity;

namespace TaskTracker.PL.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public static UserModel UserFromEntity(User user)
        {
            return new UserModel()
            {
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}
