using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalAssignment
{
    public abstract class HospitalBase : IHospitalMembers {//abstract class defines default interface implementations - better code reusability/management
        public string ID { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string StreetNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public HospitalBase(string id, string passWord, string firstName, string lastName, string email, string phone, string streetNumber, string street, string city, string state)
        {
            ID = id;
            Password = passWord;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            StreetNumber = streetNumber;
            Street = street;
            City = city;
            State = state;
        }

        public override string ToString() {
            return $"{ID}|{Password}|{FirstName}|{LastName}|{Email}|{Phone}|{StreetNumber}|{Street}|{City}|{State}";
        }
    }
}
