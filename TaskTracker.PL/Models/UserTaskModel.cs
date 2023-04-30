using TaskTrackerWeb.Entity;

namespace TaskTracker.PL.Models
{
    public class UserTaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DeadLine { get; set; }

        public static UserTaskModel TaskFromEntity(UserTask task)
        {
            return new UserTaskModel()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                CreatedDate = task.CreatedDate,
                DeadLine = task.Deadline
            };
        }
    }
}
