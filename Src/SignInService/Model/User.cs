using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }

        public User(int Id, string Name, string City)
        {
            this.Id = Id;
            this.Name = Name;
            this.City = City;
        }
    }
}
