using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HospitalAssignment
{
    public class Administrator : HospitalBase {
        public Administrator(string id, string passWord, string firstName, string lastName, string email, string phone, string streetNumber, string street, string city, string state) : base(id, passWord, firstName, lastName, email, phone, streetNumber, street, city, state)
        {

        }

        public static void AdministratorMenu(IHospitalMembers member) {
            ConsoleKeyInfo keyInfo;
            Console.Clear();

            Utils.PrintAdministratorMenu((Administrator)member, Login.AllMembers);
            keyInfo = Console.ReadKey();
            while (keyInfo.KeyChar != '7' && keyInfo.KeyChar != '8')
            {
                Console.Clear();

                if (keyInfo.KeyChar == '1')
                {
                    Utils.ListAllDoctors();
                    Console.ReadKey();
                    AdministratorMenu(member);
                }
                else if (keyInfo.KeyChar == '2') {
                    Utils.CheckDoctorDetails(Login.AllMembers);
                    AdministratorMenu(member);
                }
                if (keyInfo.KeyChar == '3') {
                    Utils.ListAllPatients();
                    Console.ReadKey();
                    AdministratorMenu(member);
                }
                else if (keyInfo.KeyChar == '4')
                {
                    Utils.CheckPatientDetails(Login.AllMembers);
                    AdministratorMenu(member);
                }
                else if (keyInfo.KeyChar == '5') {
                    Utils.AddDoctorsMenu();
                    Console.ReadKey();
                    AdministratorMenu(member);
                }

                else if (keyInfo.KeyChar == '6')
                {
                    Utils.AddPatientMenu(Login.AllMembers);
                    Console.ReadKey();
                    AdministratorMenu(member);
                }

                else {
                    Utils.PrintAdministratorMenu((Administrator)member, Login.AllMembers);
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
            return $"{ID}|{Password}|{FirstName}|{LastName}";
        }

    }
}
