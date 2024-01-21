using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HospitalAssignment {
    public class Patient : HospitalBase {//subclasses inherit default implementations using "base" keyword 

        public Doctor Doctor { get; set; }

        //derives passed values from base constructor - less  reptition for initialising each object constructor
        public Patient(string id, string passWord, string firstName, string lastName, string email, string phone, string streetNumber, string street, string city, string state, Doctor doctor) : base(id, passWord, firstName, lastName, email, phone, streetNumber, street, city, state)
        {
            if (doctor != null) {
                Doctor = doctor;
            }
            else {
                doctor = null;
            }
        }

            public static void PatientMenu(IHospitalMembers member) {//opens patient menu for logged in patient
            ConsoleKeyInfo keyInfo;
            Console.Clear();

            Utils.PrintPatientMenu((Patient)member);
            keyInfo = Console.ReadKey();
            while (keyInfo.KeyChar != '5' && keyInfo.KeyChar != '6') {
                Console.Clear();

                if (keyInfo.KeyChar == '1')
                {
                    Utils.PrintPatientDetails(member);
                    Console.ReadKey();
                    PatientMenu(member);
                }
                else if (keyInfo.KeyChar == '2')
                {
                    Utils.PrintListMyDoctor((Patient)member);
                    Console.ReadKey();
                    PatientMenu(member);

                }
                else if (keyInfo.KeyChar == '3')
                {
                    Utils.PrintListAppointments((Patient)member);
                    Console.ReadKey();
                    PatientMenu(member);

                }
                else if (keyInfo.KeyChar == '4')
                {
                    Utils.PrintBookAppointment(Login.AllMembers, (Patient)member);
                    Console.ReadKey();
                    PatientMenu(member);

                }
                else {
                    Utils.PrintPatientMenu((Patient)member);
                    Console.SetCursorPosition(0, 14);
                    Console.WriteLine("Please select an option between 1-6: ");
                    Console.SetCursorPosition(37, 14);
                }

                keyInfo = Console.ReadKey();
            }
            if (keyInfo.KeyChar == '5')
            {
                Console.Clear();
                Login.LoginPage();
            }
            else if (keyInfo.KeyChar == '6')
            {
                Environment.Exit(0);//exits console
            }
        }
        public override string ToString() {
            return $"{ID}|{Password}|{FirstName}|{LastName}|{Email}|{Phone}|{StreetNumber}|{Street}|{City}|{State}|{Doctor}";
        }

    }
}
