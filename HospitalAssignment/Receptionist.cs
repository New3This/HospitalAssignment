using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalAssignment
{
    public class Receptionist : HospitalBase {

        public Receptionist(string id, string passWord, string firstName, string lastName, string email, string phone, string streetNumber, string street, string city, string state) : base(id, passWord, firstName, lastName, email, phone, streetNumber, street, city, state) {

        }

        public static void ReceptionistMenu(IHospitalMembers member)
        {
            ConsoleKeyInfo keyInfo;
            Console.Clear();

            Utils.PrintReceptionistMenu(member);
            keyInfo = Console.ReadKey();
            while (keyInfo.KeyChar != '7' && keyInfo.KeyChar != '8')
            {
                Console.Clear();

                if (keyInfo.KeyChar == '1')
                {
                    Utils.PrintReceiptionistDetails(member);
                    Console.ReadKey();
                    ReceptionistMenu(member);
                }
                else if (keyInfo.KeyChar == '2')
                {
                    Utils.CancelABooking(Login.AllMembers);
                    ReceptionistMenu(member);
                }
                if (keyInfo.KeyChar == '3')
                {
                    Utils.CancelAllAppointments(Login.AllMembers);
                    Console.ReadKey();
                    ReceptionistMenu(member);
                }
                else if (keyInfo.KeyChar == '4')
                {
                    Utils.SendDoctorMenu(member);
                    Console.ReadKey();
                    ReceptionistMenu(member);
                }
                else if (keyInfo.KeyChar == '5')
                {
                    Utils.DoctorsGivenReminder(Login.AllMembers);
                    Console.ReadKey();
                    ReceptionistMenu(member);
                }

                else if (keyInfo.KeyChar == '6')
                {
                    Utils.DoctorsGivenNoReminder(Login.AllMembers);
                    Console.ReadKey();
                    ReceptionistMenu(member);
                }

                else
                {
                    Utils.PrintReceptionistMenu(member);
                    Console.SetCursorPosition(0, 16);
                    Console.WriteLine("Please select an option between 1-8: ");
                    Console.SetCursorPosition(37, 16);
                }
                keyInfo = Console.ReadKey();
            }
            if (keyInfo.KeyChar == '7')
            {
                Console.Clear();
                Login.LoginPage();
            }
            else if (keyInfo.KeyChar == '8')
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
