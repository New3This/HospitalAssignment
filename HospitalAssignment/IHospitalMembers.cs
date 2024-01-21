using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalAssignment{
    public interface IHospitalMembers {//interface for defining common properties/methods to be implemented
        string ID {get; set;}
        string Password { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email {get; set;}
        string Phone { get; set; }
        string StreetNumber { get; set; }
        string Street {get; set;}
        string City { get; set; }
        string State { get; set; }

        string ToString();
    }
}
