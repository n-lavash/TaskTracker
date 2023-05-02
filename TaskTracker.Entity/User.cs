using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskTrackerWeb.Entity
{
    public class User
    {
        public int Id { get; }
        public string Name { get; private set; }
        public string PhoneNumber { get; private set; }

        [JsonConstructor]
        public User(int id, string name, string phoneNumber)
        {
            Id = id;
            Name = name;
            PhoneNumber = phoneNumber;
        }

        public void EditName(string newName)
        {
            if (newName == null)
                throw new ArgumentNullException("str", "Text string cannot be null");

            Name = newName;
        }

        public void EditPhoneNumber(string newPhoneNumber)
        {
            if (newPhoneNumber == null)
                throw new ArgumentNullException("str", "Text string cannot be null");

            PhoneNumber = newPhoneNumber;
        }

        public override string? ToString()
        {
            var res = new StringBuilder();

            res.Append("Name: ").Append(Name).Append("\n")
                .Append("PhoneNumber: ").Append(PhoneNumber);

            return res.ToString();
        }
    }
}
