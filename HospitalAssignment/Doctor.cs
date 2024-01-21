using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalAssignment
{
    public class Doctor : HospitalBase {//subclasses inherit default implementations using "base" keyword

        public Doctor(string id, string passWord, string firstName, string lastName, string email, string phone, string streetNumber, string street, string city, string state) : base(id, passWord, firstName, lastName, email, phone, streetNumber, street, city, state) {
        }
        public static void DoctorMenu(IHospitalMembers member) {//will crash if the logged user regenerates still object doesnt exist anymore even tho id and pw does
            ConsoleKeyInfo keyInfo;
            Console.Clear();

            Utils.PrintDoctorMenu((Doctor)member);
            keyInfo = Console.ReadKey();
            while (keyInfo.KeyChar != '6' && keyInfo.KeyChar != '7') {
                Console.Clear();

                if (keyInfo.KeyChar == '1')
                {
                    Utils.PrintDoctorDetails((Doctor)member);
                    Console.ReadKey();
                    DoctorMenu(member);
                }
                else if (keyInfo.KeyChar == '2')
                {
                    Utils.PrintListDoctorPatients(Login.AllMembers, (Doctor)member);
                    Console.ReadKey();
                    DoctorMenu(member);

                }
                else if (keyInfo.KeyChar == '3')
                {
                    Utils.PrintAllAppointments(Login.AllMembers, (Doctor)member);
                    Console.ReadKey();
                    DoctorMenu(member);

                }
                else if (keyInfo.KeyChar == '4')
                {
                    Utils.CheckParticularPatientDetails(Login.AllMembers);
                    Console.ReadKey();
                    DoctorMenu(member);

                }
                else if (keyInfo.KeyChar == '5')
                {
                    Utils.CheckParticularAppointmentDetails(Login.AllMembers, (Doctor)member);
                    Console.ReadKey();
                    DoctorMenu(member);
                }
                else
                {
                    Utils.PrintDoctorMenu((Doctor)member);
                    Console.SetCursorPosition(0, 15);
                    Console.WriteLine("Please select an option between 1-7: ");
                    Console.SetCursorPosition(37, 15);
                }

                keyInfo = Console.ReadKey();
            }
            if (keyInfo.KeyChar == '6')
            {
                Console.Clear();
                Login.LoginPage();
            }
            else if (keyInfo.KeyChar == '7')
            {
                Environment.Exit(0);
            }
        }
        public override string ToString()
        {
            return $"{ID}|{Password}|{FirstName}|{LastName}|{Email}|{Phone}|{StreetNumber}|{Street}|{City}|{State}";
        }

    }
}
