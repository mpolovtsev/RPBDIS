using System;
using System.Collections.Generic;

namespace ConsoleUI.Models
{
    public partial class Manager
    {
        public Manager()
        {
            Organizations = new HashSet<Organization>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<Organization> Organizations { get; set; }

        public override string ToString()
        {
            return $"Id: {Id},\nName: {Name},\nSurname: {Surname},\nMiddle name: {MiddleName}\n" +
                $"Phone number: {PhoneNumber}.";
        }
    }
}
