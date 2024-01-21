using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HospitalAssignment
{
    public class Utils {//defines set of commonly reused methods
        public static ConsoleKeyInfo keyInfo;
        public static void toString(string FirstName, string LastName, string Email, string Phone, string StreetNo, string Street, string City, string State) {

            Console.WriteLine($"{FirstName + " " + LastName,-20}| {Email,-30}| {Phone,-13}| {StreetNo + " " + Street + ", " + City + ", " + State,-40}");
        }
        public static void PrintPatientMenu(Patient patient) {
            Console.WriteLine("|===========================================|");
            Console.WriteLine("|         DOTNET Management System          |");
            Console.WriteLine("|-------------------------------------------|");
            Console.WriteLine("|                 Patient Menu              |");
            Console.WriteLine("|===========================================|");
            Console.WriteLine("Welcome to DOTNET Hospital Management System {0} {1}", patient.FirstName, patient.LastName);
            Console.WriteLine("\nPlease choose an option:");
            Console.WriteLine("1. List patient details");
            Console.WriteLine("2. List my doctor details");
            Console.WriteLine("3. List all appointments");
            Console.WriteLine("4. Book appointment");
            Console.WriteLine("5. Exit to login");
            Console.WriteLine("6. Exit System");

        }

        public static void PrintLogin() {
            Console.WriteLine("|===========================================|");
            Console.WriteLine("|         DOTNET Management System          |");
            Console.WriteLine("|-------------------------------------------|");
            Console.WriteLine("|                    Login                  |");
            Console.WriteLine("|===========================================|");
            Console.WriteLine("|ID:                                        |");
            Console.WriteLine("|Password:                                  |");
            Console.WriteLine("|                                           |");
            Console.WriteLine("|===========================================|");
        }

        public static void PrintPatientDetails(IHospitalMembers patient) {
            Console.WriteLine("|===========================================|");
            Console.WriteLine("|         DOTNET Management System          |");
            Console.WriteLine("|-------------------------------------------|");
            Console.WriteLine("|                 My Details                |");
            Console.WriteLine("|===========================================|\n");
            Console.WriteLine("{0} {1}'s Details", patient.FirstName, patient.LastName);
            Console.WriteLine("\nPatient ID: {0}", patient.ID);
            Console.WriteLine("Full Name: {0} {1}", patient.FirstName, patient.LastName);
            Console.WriteLine("Address: {0} {1}, {2}, {3}", patient.StreetNumber, patient.Street, patient.City, patient.State);
            Console.WriteLine("Email: {0}", patient.Email);
            Console.WriteLine("Phone: {0}", patient.Phone);
        }


        public static void PrintListMyDoctor(Patient patient) {
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("|                               DOTNET Management System                           |");
            Console.WriteLine("|----------------------------------------------------------------------------------|");
            Console.WriteLine("|                                     My Doctor                                    |");
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine(" Your Doctor:                                                                      \n");
            Console.WriteLine($" {"Name",-20}| {"Email Address",-30}| {"Phone",-10}| {"Address",-40}");
            Console.WriteLine($" --------------------------------------------------------------------------------------------------------------");
            try {
                Console.WriteLine($" {patient.Doctor.FirstName + " " + patient.Doctor.LastName,-20}| {patient.Doctor.Email,-30}| {patient.Doctor.Phone,-10}| {patient.Doctor.StreetNumber + " " + patient.Doctor.Street + ", " + patient.Doctor.City + ", " + patient.Doctor.State,-40}");
            }
            catch (NullReferenceException e) {
                Console.WriteLine("Error: " + e.Message);
            }
        }


        public static void PrintListAppointments(Patient patient) {
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("|                               DOTNET Management System                           |");
            Console.WriteLine("|----------------------------------------------------------------------------------|");
            Console.WriteLine("|                                    My Appointments                               |");
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("\n Appointments for " + patient.FirstName + " " + patient.LastName + "\n");
            Console.WriteLine($" {"Doctor",-20}| {"Patient",-20}| {"Description",-40}");

            Console.WriteLine(" ------------------------------------------------------------------------------------");

            string[] lines = System.IO.File.ReadAllLines("Appointment.txt");
            foreach (string line in lines) {
                string[] splits = line.Split('|');
                if (patient.ID == splits[2]) {//if patient found in appointment.txt
                    Console.WriteLine($" {splits[6] + " " + splits[7],-20}| {splits[0] + " " + splits[1],-20}| {splits[3],-40}");
                }
            }

        }

        public static void PrintBookAppointment(List<IHospitalMembers> person, Patient patient) {
            bool InputValid = false;
            bool ErrorMsgOnce = false;
            List<Doctor> doctors = new List<Doctor>();
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("|                             DOTNET Management System                             |");
            Console.WriteLine("|----------------------------------------------------------------------------------|");
            Console.WriteLine("|                                Book Appointment                                  |");
            Console.WriteLine("|==================================================================================|");
            if (patient.Doctor == null) {
                Console.WriteLine("You are not registered with any doctor! Please check which doctor you would like to register with\n");

                for (int i = 0; i < person.Count; i++) {
                    if (person[i] is Doctor && !doctors.Contains(person[i])) {
                        doctors.Add((Doctor)person[i]);//create list with no duplicate doctors 
                    }
                }

                for (int j = 0; j < doctors.Count; j++) {//display each unique doctor
                    Console.WriteLine(j + 1 + " " + doctors[j].FirstName + " " + doctors[j].LastName + " | " + doctors[j].Email + " | " + doctors[j].Phone + " | " + doctors[j].StreetNumber + " " + doctors[j].Street + ", " + doctors[j].City + ", " + doctors[j].State);
                }

                while (!InputValid) {
                    try {//select doctor
                        int option = Convert.ToInt32(Console.ReadLine()) - 1;
                        patient.Doctor = doctors[option];
                        InputValid = true;
                    }
                    catch (FormatException) {
                        if (!ErrorMsgOnce) {
                            Console.WriteLine("Please input a number representing your chosen doctor: ");
                            ErrorMsgOnce = true;
                        }
                        Console.SetCursorPosition(55, Console.CursorTop - 1);

                    }
                }

            }

            Console.WriteLine("You are booking a new appointment with {0} {1}", patient.Doctor.FirstName, patient.Doctor.LastName);
            Console.Write("Description of the appointment: ");
            string Description = Console.ReadLine();
            string PatientID = patient.ID;

            Console.WriteLine("The appointment has been booked succesfully");
            //create appointment line = patient + description + doctor
            string AppointmentLine = patient.FirstName + "|" + patient.LastName + "|" + PatientID + "|" + Description + "|" + patient.Doctor.ID + "|" + patient.Doctor.Password + "|" + patient.Doctor.FirstName + "|" + patient.Doctor.LastName + "|" + patient.Doctor.Email + "|" + patient.Doctor.Phone + "|" + patient.Doctor.StreetNumber + "|" + patient.Doctor.Street + "|" + patient.Doctor.City + "|" + patient.Doctor.State + "\n";
            string filename = "Appointment.txt";

            FileStream write = new FileStream(filename, FileMode.Append, FileAccess.Write);//store line
            byte[] WriteArr = Encoding.UTF8.GetBytes(AppointmentLine);
            write.Write(WriteArr, 0, AppointmentLine.Length);
            write.Close();
        }

        public static void PrintDoctorMenu(Doctor doctor)
        {
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("|                               DOTNET Management System                           |");
            Console.WriteLine("|----------------------------------------------------------------------------------|");
            Console.WriteLine("|                                    Doctor Menu                                   |");
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("Welcome to DOTNET Hospital Management System {0} {1}", doctor.FirstName, doctor.LastName);
            Console.WriteLine("\nPlease choose an option:");
            Console.WriteLine("1. List doctor details");
            Console.WriteLine("2. List patients");
            Console.WriteLine("3. List appointments");
            Console.WriteLine("4. Check particular patient");
            Console.WriteLine("5. List appointments with patient");
            Console.WriteLine("6. Logout");
            Console.WriteLine("7. Exit");
        }
        public static void PrintDoctorDetails(Doctor doctor)
        {
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("|                              DOTNET Management System                            |");
            Console.WriteLine("|----------------------------------------------------------------------------------|");
            Console.WriteLine("|                                    My Details                                    |");
            Console.WriteLine("|==================================================================================|");

            Console.WriteLine($"{"Name",-20}| {"Email Address",-30}| {"Phone",-13}| {"Address",-40}");
            Console.WriteLine($"---------------------------------------------------------------------------------------------------------");
            toString(doctor.FirstName, doctor.LastName, doctor.Email, doctor.Phone, doctor.StreetNumber, doctor.Street, doctor.City, doctor.State);
        }

        public static void PrintListDoctorPatients(List<IHospitalMembers> person, Doctor doctor) {
            List<string> CurrentlyDisplayedPatients = new List<string>();
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("|                              DOTNET Management System                            |");
            Console.WriteLine("|----------------------------------------------------------------------------------|");
            Console.WriteLine("|                                   My Patients                                    |");
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("Patients assigned to " + doctor.FirstName + " " + doctor.LastName + "\n");
            Console.WriteLine($"{"Patient",-20}| {"Doctor",-20}| {"Email Address",-30}| {"Phone",-10}| {"Address",-40}");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");

            foreach (IHospitalMembers member in person) {//print all patients with an assigned doctor object identical to currently logged in doctor
                if (member is Patient) {
                    Patient PatientMember = (Patient)member;
                    if (PatientMember.Doctor != null && PatientMember.Doctor.ID.Equals(doctor.ID)) {
                        if (!CurrentlyDisplayedPatients.Contains(PatientMember.ID)) {
                            Console.WriteLine($"{PatientMember.FirstName + " " + PatientMember.LastName,-20}| {doctor.FirstName + " " + doctor.LastName,-20}| {PatientMember.Email,-30}| {PatientMember.Phone,-10}| {PatientMember.StreetNumber + " " + PatientMember.Street + ", " + PatientMember.City + ", " + PatientMember.State,-40}");
                            CurrentlyDisplayedPatients.Add(PatientMember.ID);
                        }
                    }
                }
            }

        }

        public static void PrintAllAppointments(List<IHospitalMembers> person, Doctor doctor) {//iterate through appointment.txt, grabbing every description and patient if stored doctor matches logged in doctor
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("|                             DOTNET Management System                             |");
            Console.WriteLine("|----------------------------------------------------------------------------------|");
            Console.WriteLine("|                                 All Appointments                                 |");
            Console.WriteLine("|==================================================================================|\n");

            Console.WriteLine($" {"Doctor",-20}| {"Patient",-20}| {"Description",-30}");
            Console.WriteLine("--------------------------------------------------------------------------------------------");

            string[] lines = System.IO.File.ReadAllLines("Appointment.txt");

            foreach (string line in lines)
            {
                string[] EachFileLine = line.Split('|');
                for (int i = 0; i < EachFileLine.Length; i++) {
                    if (EachFileLine[4] == doctor.ID) {
                        Console.WriteLine($" {EachFileLine[6] + " " + EachFileLine[7],-20}| {EachFileLine[0] + " " + EachFileLine[1],-20}| {EachFileLine[3],-30}");
                        break;
                    }
                }
            }


        }

        public static void CheckParticularPatientDetails(List<IHospitalMembers> person) {
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("|                             DOTNET Management System                             |");
            Console.WriteLine("|----------------------------------------------------------------------------------|");
            Console.WriteLine("|                              Check Patient Details                               |");
            Console.WriteLine("|==================================================================================|");

            Console.Write(" Enter the ID of the patient to check: ");
            try {
                bool IdentifiedPatient = false;
                int SearchPatient = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine($"\n {"Patient",-20}| {"Doctor",-20}| {"Email Address",-30}| {"Phone",-10}| {"Address",-40}");
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");


                foreach (IHospitalMembers member in person) {//search for patient in person list
                    if (member is Patient) {
                        Patient PatientMember = (Patient)member;
                        if (Convert.ToInt32(PatientMember.ID) == SearchPatient) {
                            Console.WriteLine($" {PatientMember.FirstName + " " + PatientMember.LastName,-20}| {PatientMember.Doctor.FirstName + " " + PatientMember.Doctor.LastName,-20}| {PatientMember.Email,-30}| {PatientMember.Phone,-10}| {PatientMember.StreetNumber + " " + PatientMember.Street + ", " + PatientMember.City + ", " + PatientMember.State,-40}");
                            IdentifiedPatient = true;
                            break;
                        }
                    }
                }
                if (IdentifiedPatient) {
                    IdentifiedPatient = false;
                }
                else {
                    Console.WriteLine("No such patient was found");
                }
            }
            catch (FormatException e) {
                Console.WriteLine("\nPlease type the ID in its appropiate, numerical form\nPress a key to return to the Menu");
            }

            catch (OverflowException e) {
                Console.WriteLine("The ID is 5-8 numbers long. Please follow this length.");
            }

        }

        public static void CheckParticularAppointmentDetails(List<IHospitalMembers> person, Doctor doctor) {
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("|                             DOTNET Management System                             |");
            Console.WriteLine("|----------------------------------------------------------------------------------|");
            Console.WriteLine("|                                Appointments With                                 |");
            Console.WriteLine("|==================================================================================|");
            Console.Write(" Enter the ID of the patient to check: ");
            try {//if currently logged in doctor matches doctor in a appointment.txt line, print patient on that line
                bool IdentifiedPatient = false;
                int SearchPatient = Convert.ToInt32(Console.ReadLine());
                string[] lines = System.IO.File.ReadAllLines("Appointment.txt");
                Console.WriteLine($"\n {"Doctor",-20}| {"Patient",-20}| {"Description",-30}");
                Console.WriteLine(" --------------------------------------------------------------------------------------------");
                foreach (string line in lines) {
                    string[] EachFileLine = line.Split('|');
                    if (EachFileLine[4] == doctor.ID && Convert.ToInt32(EachFileLine[2]) == SearchPatient) {
                        Console.WriteLine($" {EachFileLine[6] + " " + EachFileLine[7],-20}| {EachFileLine[0] + " " + EachFileLine[1],-20}| {EachFileLine[3],-30}");
                        IdentifiedPatient = true;
                        break;
                    }
                }
                if (IdentifiedPatient) {
                    IdentifiedPatient = false;
                }
                else {
                    Console.WriteLine(" No such patient was found");
                }

            }
            catch (FormatException e) {
                Console.WriteLine("\nPlease type the ID in its appropiate, numerical form\nPress a key to return to the Menu");
            }
            catch (OverflowException e) {
                Console.WriteLine("The ID is 5-8 numbers long. Please follow this length.");
            }

        }

        public static void PrintAdministratorMenu(Administrator member, List<IHospitalMembers> person) {
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("|                               DOTNET Management System                           |");
            Console.WriteLine("|----------------------------------------------------------------------------------|");
            Console.WriteLine("|                                  Adminstrator Menu                               |");
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("Welcome to DOTNET Hospital Management System " + member.FirstName + " " + member.LastName);
            Console.WriteLine("\nPlease choose an option:");
            Console.WriteLine("1. List all doctors");
            Console.WriteLine("2. Check doctor details");
            Console.WriteLine("3. List all patients");
            Console.WriteLine("4. Check patient details");
            Console.WriteLine("5. Add doctor");
            Console.WriteLine("6. Add patient");
            Console.WriteLine("7. Logout");
            Console.WriteLine("8. Exit");
        }

        public static void ListAllDoctors() {
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("|                             DOTNET Management System                             |");
            Console.WriteLine("|----------------------------------------------------------------------------------|");
            Console.WriteLine("|                                  All Doctors                                     |");
            Console.WriteLine("|==================================================================================|");

            Console.WriteLine("\nAll doctors registered to the DOTNET Hospital Management System \n");

            Console.WriteLine($"{"Name",-20}| {"Email Address",-30}| {"Phone",-13}| {"Address",-40}");
            Console.WriteLine($"---------------------------------------------------------------------------------------------------------------------");


            string[] lines = System.IO.File.ReadAllLines("doctor.txt");

            foreach (string line in lines) {//print out all doctors in doctor.txt
                string[] EachFileLine = line.Split('|');
                toString(EachFileLine[2], EachFileLine[3], EachFileLine[4], EachFileLine[5], EachFileLine[6], EachFileLine[7], EachFileLine[8], EachFileLine[9]);
            }
        }

        public static void CheckDoctorDetails(List<IHospitalMembers> person) {
            bool IdentifiedDoctor = false;
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("|                             DOTNET Management System                             |");
            Console.WriteLine("|----------------------------------------------------------------------------------|");
            Console.WriteLine("|                                 Doctor Details                                   |");
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("\nPlease enter the ID of the doctor who's details you are checking. Or press n to return to menu");
            string FormID = "";
            try {

                do {
                    keyInfo = Console.ReadKey();
                    if (keyInfo.Key == ConsoleKey.Backspace && FormID.Length > 0) {
                        Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                        FormID = FormID.Remove(FormID.Length - 1);
                        Console.Write("\b \b");//deletes last char + moves caret back
                    }

                    else if (keyInfo.Key == ConsoleKey.Enter) {
                        int SearchDoctor = Convert.ToInt32(FormID);
                        foreach (IHospitalMembers member in person) {//if doctor found in list, print
                            if (member is Doctor) {
                                Doctor DoctorMember = (Doctor)member;
                                if (Convert.ToInt32(DoctorMember.ID) == SearchDoctor) {
                                    Console.WriteLine("\n" + "Details for {0} {1}", DoctorMember.FirstName, DoctorMember.LastName);
                                    Console.WriteLine($"\n{"Name",-20}| {"Email Address",-30}| {"Phone",-13}| {"Address",-40}");
                                    Console.WriteLine($"---------------------------------------------------------------------------------------------------------------------");
                                    toString(DoctorMember.FirstName, DoctorMember.LastName, DoctorMember.Email, DoctorMember.Phone, DoctorMember.StreetNumber, DoctorMember.Street, DoctorMember.City, DoctorMember.State);

                                    IdentifiedDoctor = true;
                                    Console.ReadKey();
                                    break;
                                }
                            }
                        }
                        if (!IdentifiedDoctor) {//otherwise
                            Console.WriteLine("No such Doctor exists!");
                            Console.ReadKey();
                        }
                        break;
                    }

                    else
                    {
                        FormID += keyInfo.KeyChar;
                    }
                } while (keyInfo.Key != ConsoleKey.N);

            }

            catch (FormatException) {
                if (keyInfo.KeyChar != 'n')
                {
                    Console.WriteLine("\nPlease type the ID in its appropiate, numerical form\nPress a key to return to the Menu");
                    Console.ReadKey();

                }
            }
            catch (OverflowException)
            {
                Console.WriteLine("\nThe ID is no longer than 5-8 characters\nPress a key to return to the Menu");
                Console.ReadKey();

            }

        }

        public static void ListAllPatients()
        {
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("|                             DOTNET Management System                             |");
            Console.WriteLine("|----------------------------------------------------------------------------------|");
            Console.WriteLine("|                                   All Patients                                   |");
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("\n");
            Console.WriteLine("All patients registered to the DOTNET Hospital Management System");
            Console.WriteLine("\n");
            Console.WriteLine($"{"Name",-20}| {"Email Address",-30}| {"Phone",-13}| {"Address",-40}");
            Console.WriteLine($"---------------------------------------------------------------------------------------------------------------------");



            string[] lines = System.IO.File.ReadAllLines("patient.txt");

            foreach (string line in lines) {//print out all patients
                string[] EachFileLine = line.Split('|');
                toString(EachFileLine[2], EachFileLine[3], EachFileLine[4], EachFileLine[5], EachFileLine[6], EachFileLine[7], EachFileLine[8], EachFileLine[9]);
            }
        }

        public static void CheckPatientDetails(List<IHospitalMembers> person)
        {
            bool IdentifiedPatient = false;
            string FormID = "";

            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("|                             DOTNET Management System                             |");
            Console.WriteLine("|----------------------------------------------------------------------------------|");
            Console.WriteLine("|                                 Patient Details                                  |");
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("\nPlease enter the ID of the patient who's details you are checking. Or press n to return to menu");

            try {

                do {
                    keyInfo = Console.ReadKey();
                    if (keyInfo.Key == ConsoleKey.Backspace && FormID.Length > 0)
                    {
                        Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                        FormID = FormID.Remove(FormID.Length - 1);
                        Console.Write("\b \b");//deletes last char + moves caret back
                    }

                    else if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        int SearchPatient = Convert.ToInt32(FormID);
                        foreach (IHospitalMembers member in person) {
                            if (member is Patient) {
                                Patient PatientMember = (Patient)member;
                                if (Convert.ToInt32(PatientMember.ID) == SearchPatient) {//if patient found in list, print
                                    Console.WriteLine("\nDetails for {0} {1}", PatientMember.FirstName, PatientMember.LastName);
                                    Console.WriteLine($"\n{"Name",-20}| {"Email Address",-30}| {"Phone",-13}| {"Address",-40}");
                                    Console.WriteLine($"---------------------------------------------------------------------------------------------------------------------");
                                    toString(PatientMember.FirstName, PatientMember.LastName, PatientMember.Email, PatientMember.Phone, PatientMember.StreetNumber, PatientMember.Street, PatientMember.City, PatientMember.State);

                                    IdentifiedPatient = true;
                                    Console.ReadKey();
                                    break;
                                }
                            }
                        }
                        if (!IdentifiedPatient) {//otherwise
                            Console.WriteLine("No such Patient exists!");
                            Console.ReadKey();
                        }
                        break;
                    }

                    else {
                        FormID += keyInfo.KeyChar;
                    }
                } while (keyInfo.Key != ConsoleKey.N);
            }

            catch (FormatException) {
                Console.WriteLine("\nPlease type the ID in its appropiate, numerical form\nPress a key to return to the Menu");
                Console.ReadKey();
            }

            catch (OverflowException) {
                Console.WriteLine("\nThe ID is no longer than 5-8 characters\nPress a key to return to the Menu");
                Console.ReadKey();
            }
        }

        public static void AddDoctorsMenu() {
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("|                             DOTNET Management System                             |");
            Console.WriteLine("|----------------------------------------------------------------------------------|");
            Console.WriteLine("|                                   Add Doctor                                     |");
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("Registering a new doctor with the DOTNET Hospital Assignment Management System");
            Console.Write("First Name: ");
            string FirstName = Console.ReadLine();
            Console.Write("Last Name: ");
            string LastName = Console.ReadLine();
            Console.Write("Email: ");
            string Email = Console.ReadLine();
            Console.Write("Phone: ");
            string Phone = Console.ReadLine();
            Console.Write("Street Number: ");
            string StreetNum = Console.ReadLine();
            Console.Write("Street: ");
            string Street = Console.ReadLine();
            Console.Write("City: ");
            string City = Console.ReadLine();
            Console.Write("State: ");
            string State = Console.ReadLine();
            Login.AddObjects(new Doctor(Login.GenerateRandomID(), "PassWord", FirstName, LastName, Email, Phone, StreetNum, Street, City, State));//add new doctor object
            Console.WriteLine(FirstName + " " + LastName + " added to the System!");
        }

        public static void AddPatientMenu(List<IHospitalMembers> person) {
            List<Doctor> doctors = new List<Doctor>();
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("|                             DOTNET Management System                             |");
            Console.WriteLine("|----------------------------------------------------------------------------------|");
            Console.WriteLine("|                                  Add Patient                                     |");
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("Registering a new patient with the DOTNET Hospital Assignment Management System");
            Console.Write("First Name: ");
            string FirstName = Console.ReadLine();
            Console.Write("Last Name: ");
            string LastName = Console.ReadLine();
            Console.Write("Email: ");
            string Email = Console.ReadLine();
            Console.Write("Phone: ");
            string Phone = Console.ReadLine();
            Console.Write("Street Number: ");
            string StreetNum = Console.ReadLine();
            Console.Write("Street: ");
            string Street = Console.ReadLine();
            Console.Write("City: ");
            string City = Console.ReadLine();
            Console.Write("State: ");
            string State = Console.ReadLine();
            Patient AddPatient = new Patient(Login.GenerateRandomID(), "PassWord", FirstName, LastName, Email, Phone, StreetNum, Street, City, State, null);
            PrintBookAppointment(Login.AllMembers, AddPatient);//ask which doctor to be assigned
            Login.AddObjects(AddPatient);//add new doctor object
            Console.WriteLine(FirstName + " " + LastName + " added to the System!");

        }

        public static void PrintReceptionistMenu(IHospitalMembers member) {
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("|                             DOTNET Management System                             |");
            Console.WriteLine("|----------------------------------------------------------------------------------|");
            Console.WriteLine("|                                  Receptionist Menu                               |");
            Console.WriteLine("|==================================================================================|");
            Console.WriteLine("Welcome to DOTNET Hospital Management System " + member.FirstName + " " + member.LastName);
            Console.WriteLine("\nPlease choose an option:");
            Console.WriteLine("1. View Receptionist Details");
            Console.WriteLine("2. Cancel an appointment");
            Console.WriteLine("3. Cancel all appointments");
            Console.WriteLine("4. Send Doctor Appointment Reminder");
            Console.WriteLine("5. View Doctors that have been sent a reminder");
            Console.WriteLine("6. View Doctors yet to receive a reminder");
            Console.WriteLine("7. Logout");
            Console.WriteLine("8. Exit");
        }

        public static void PrintReceiptionistDetails(IHospitalMembers receptionist) {
            Console.WriteLine("|===========================================|");
            Console.WriteLine("|         DOTNET Management System          |");
            Console.WriteLine("|-------------------------------------------|");
            Console.WriteLine("|                 My Details                |");
            Console.WriteLine("|===========================================|\n");
            Console.WriteLine("{0} {1}'s Details", receptionist.FirstName, receptionist.LastName);
            Console.WriteLine("\nReceptionist ID: {0}", receptionist.ID);
            Console.WriteLine("Full Name: {0} {1}", receptionist.FirstName, receptionist.LastName);
            Console.WriteLine("Address: {0} {1}, {2}, {3}", receptionist.StreetNumber, receptionist.Street, receptionist.City, receptionist.State);
            Console.WriteLine("Email: {0}", receptionist.Email);
            Console.WriteLine("Phone: {0}", receptionist.Phone);


        }

        public static void CancelABooking(List<IHospitalMembers> person) {
            bool AppFound = false;
            Console.WriteLine("|===========================================|");
            Console.WriteLine("|         DOTNET Management System          |");
            Console.WriteLine("|-------------------------------------------|");
            Console.WriteLine("|            Cancel an Appointment          |");
            Console.WriteLine("|===========================================|\n");

            try {
                Console.Write("Patient ID: ");
                int PatID = Convert.ToInt32(Console.ReadLine());
                Console.Write("Doctor ID: ");
                int DoctorID = Convert.ToInt32(Console.ReadLine());
                Console.Write("Appointment Description: ");
                string AppDesc = Console.ReadLine();
                List<string> lines = System.IO.File.ReadAllLines("Appointment.txt").ToList();
                foreach (string line in lines) {
                    string[] parts = line.Split('|');

                    if (lines.Count == 0) {
                        Console.WriteLine("There exists no appointments to cancel");
                        break;
                    }
                    if (PatID == Convert.ToInt32(parts[2]) && DoctorID == Convert.ToInt32(parts[4]) && AppDesc.Equals(parts[3])) {//remove appointment.txt line with matching description and patient, doctor id
                        lines.Remove(line);
                        AppFound = true;
                        break;
                    }
                }
                if (AppFound) {//if line removed
                    Console.WriteLine("Appointment has succesfully been removed");
                    File.WriteAllLines("Appointment.txt", lines);
                }
                else {//otherwise
                    Console.WriteLine("Appointment with this description and Doctor, Patient ID has not been found");
                }
                Console.ReadKey();

            }
            catch (FormatException e) {//inputs of incorrect format, for e.g., input of regex [A-Z] for id asking for int
                Console.WriteLine("Please ensure the inputs are appropiate");
                Console.WriteLine("Returning to the menu");
                Console.ReadKey();
            }
            catch (OverflowException) {//input exceeds int32 size
                Console.WriteLine("\nThe ID is no longer than 5-8 characters\nPress a key to return to the Menu");
                Console.ReadKey();
            }



        }

        public static void CancelAllAppointments(List<IHospitalMembers> person) {
            bool AppFound = false;
            Console.WriteLine("|===========================================|");
            Console.WriteLine("|         DOTNET Management System          |");
            Console.WriteLine("|-------------------------------------------|");
            Console.WriteLine("|          Cancel All Appointment           |");
            Console.WriteLine("|===========================================|\n");

            try {
                Console.Write("Patient ID: ");
                int PatID = Convert.ToInt32(Console.ReadLine());
                Console.Write("Doctor ID: ");
                int DoctorID = Convert.ToInt32(Console.ReadLine());
                List<string> lines = System.IO.File.ReadAllLines("Appointment.txt").ToList();
                for (int NextLine = lines.Count -1; NextLine >= 0; NextLine--) {//iterate appointment backwards - prevents InvalidOperationException: Collection was modified, when removing line;
                    string[] parts = lines[NextLine].Split('|');

                    if (lines.Count == 0) {
                        Console.WriteLine("There exists no appointments to cancel");
                        break;
                    }
                    if (PatID == Convert.ToInt32(parts[2]) && DoctorID == Convert.ToInt32(parts[4])) {//remove all lines in appointment where patient and doctor id match inputted pat and doc id
                        lines.Remove(lines[NextLine]);
                        AppFound = true;
                    }
                }
                if (AppFound) {//if removed
                    Console.WriteLine("Appointment has succesfully been removed");
                    File.WriteAllLines("Appointment.txt", lines);
                }
                else {//otherwise
                    Console.WriteLine("Appointment with this description and Doctor, Patient ID has not been found");
                }
                Console.ReadKey();

            }
            catch (FormatException e) {
                Console.WriteLine("Please ensure the inputs are appropiate");
                Console.WriteLine("Returning to the menu");
                Console.ReadKey();
            }
            catch (OverflowException) {
                Console.WriteLine("\nThe ID is no longer than 5-8 characters\nPress a key to return to the Menu");
                Console.ReadKey();
            }
        }

        public static void SendDoctorMenu(IHospitalMembers member) {
            try {
                Console.Write("Send reminder to which doctor's email (your/any Gmail): ");
                string DoctorEmail = Console.ReadLine();
                bool DoctorFound = false;
                bool DupeFound = false;
                Console.Write("ID of the doctor you're pretending to send the email to: ");
                string DoctorId = Console.ReadLine();

                string[] lines = System.IO.File.ReadAllLines("doctor.txt");
                foreach (string line in lines) {//check if doctor id exists
                    string[] parts = line.Split('|');
                    if (Convert.ToInt32(parts[0]) == Convert.ToInt32(DoctorId)) {//checks if doctor id is valid/exists
                        DoctorFound = true;
                        string[] ReceptionistLines = System.IO.File.ReadAllLines("DoctorsReminded.txt");
                        foreach (string ReminderLine in ReceptionistLines) {
                            if (ReminderLine.Equals(DoctorId)) {//checks if this id has already received a reminder
                                DupeFound = true;
                            }
                        }

                        if (!DupeFound) {//if not, store in DoctorsReminded.txt
                            FileStream writeID = new FileStream("DoctorsReminded.txt", FileMode.Append, FileAccess.Write);
                            string StoreID = DoctorId + "\n";
                            byte[] IDArr = Encoding.UTF8.GetBytes(StoreID);
                            writeID.Write(IDArr, 0, StoreID.Length);
                            writeID.Close();
                        }

                    }
                }
                if (!DoctorFound) {
                    Console.WriteLine("No such doctor with that ID exists");
                    Console.WriteLine("Press to return to the menu");
                }
                else {//send email
                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential("hospitalstaff85@gmail.com", "ixwv twhh jfsc wjcd");

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("hospitalstaff85@gmail.com");
                    mail.To.Add(DoctorEmail);
                    mail.Subject = "Appointment Reminder";
                    mail.Body = "Yo Doc,\n\nUhh...bye Doc\n\n" + member.FirstName + " " + member.LastName+",";

                    smtpClient.Send(mail);
                    Console.WriteLine("Reminder sent.");
                }

            }
            catch (OverflowException) {
                Console.WriteLine("\nThe ID is no longer than 5-8 characters\nPress a key to return to the Menu");
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("Press to return to the menu");
            }

        }
        public static void DoctorsGivenReminder(List<IHospitalMembers> person) {

            Console.WriteLine("|===========================================|");
            Console.WriteLine("|         DOTNET Management System          |");
            Console.WriteLine("|-------------------------------------------|");
            Console.WriteLine("|              Doctors Reminded             |");
            Console.WriteLine("|===========================================|");


            Console.WriteLine($"\n{"Name",-20}| {"Email Address",-30}| {"Phone",-13}| {"Address",-40}");
            Console.WriteLine($"---------------------------------------------------------------------------------------------------------------------");

            try {
                string[] ReceptionistLines = System.IO.File.ReadAllLines("DoctorsReminded.txt");
                string[] DoctorLines = System.IO.File.ReadAllLines("doctor.txt");

                foreach (string RemindedDoctor in ReceptionistLines) {
                    foreach (string doctor in DoctorLines) {
                        string[] DoctorParts = doctor.Split('|');
                        if (DoctorParts[0] == RemindedDoctor) {//if doctor ID in DoctorsReminded.txt matches doctor ID in doctor.txt
                            toString(DoctorParts[2], DoctorParts[3], DoctorParts[4], DoctorParts[5], DoctorParts[6], DoctorParts[7], DoctorParts[8], DoctorParts[9]);//print out doctor
                        }
                    }
                }

            }
            catch (Exception e) {
                Console.WriteLine("Error: " + e.Message);
            }

        }

        public static void DoctorsGivenNoReminder(List<IHospitalMembers> person)
        {

            Console.WriteLine("|===========================================|");
            Console.WriteLine("|         DOTNET Management System          |");
            Console.WriteLine("|-------------------------------------------|");
            Console.WriteLine("|             Doctors Unreminded            |");
            Console.WriteLine("|===========================================|");


            Console.WriteLine($"\n{"Name",-20}| {"Email Address",-30}| {"Phone",-13}| {"Address",-40}");
            Console.WriteLine($"---------------------------------------------------------------------------------------------------------------------");
            try
            {
                string[] ReceptionistLines = System.IO.File.ReadAllLines("DoctorsReminded.txt");
                List<string> DoctorLines = System.IO.File.ReadAllLines("doctor.txt").ToList();

                foreach (string RemindedDoctor in ReceptionistLines) {//check if doctor id in doctor.txt is in doctorsreminded.txt
                    for (int NextLine = DoctorLines.Count - 1; NextLine >= 0; NextLine--) {
                        string[] parts = DoctorLines[NextLine].Split('|');
                        if (parts[0] == RemindedDoctor) {
                            DoctorLines.Remove(DoctorLines[NextLine]);//if it is, then doctor is reminded, remove from list
                        }

                    }
                }

                foreach (string doctor in DoctorLines) {//print every unreminded doctor
                    string[] DoctorParts = doctor.Split('|');
                    toString(DoctorParts[2], DoctorParts[3], DoctorParts[4], DoctorParts[5], DoctorParts[6], DoctorParts[7], DoctorParts[8], DoctorParts[9]);
                }
            }
            catch (Exception e) {
                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
}
