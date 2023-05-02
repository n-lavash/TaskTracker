using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskTrackerWeb.Entity
{
    public class UserTask
    {
        public int Id { get; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedDate { get; }
        public DateTime Deadline { get; private set; }

        public UserTask(int id, string title, string description, DateTime deadline)
        {
            Id = id;
            Title = title;
            Description = description;
            CreatedDate = DateTime.Now;
            Deadline = deadline;
        }

        [JsonConstructor]
        public UserTask(int id, string title, string description, DateTime createdDate, DateTime deadline) : this(id, title, description, deadline)
        {
            CreatedDate = createdDate;
        }

        public void EditTitle(string newTitle)
        {
            if (newTitle == null)
                throw new ArgumentNullException("str", "Text string cannot be null");

            Title = newTitle;
        }

        public void EditDescription(string newDescription)
        {
            if (newDescription == null)
                throw new ArgumentNullException("str", "Text string cannot be null");

            Description = newDescription;
        }

        public override string? ToString()
        {
            var res = new StringBuilder();

            res.Append("Title: ").Append(Title).Append("\n")
                .Append("Description: ").Append(Description).Append("\n")
                .Append("Created date: ").Append(CreatedDate).Append("\n")
                .Append("Deadline: ").Append(Deadline);

            return res.ToString();
        }
    }
}
