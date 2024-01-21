using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace HospitalAssignment
{
    public class Login {
        public static List<string> CurrentStoredIDs = new List<string>();
        public static List<IHospitalMembers> AllMembers = new List<IHospitalMembers>();//stores objects for use
        public static ConsoleKeyInfo keyInfo;
        private static Random random = new Random();
        public static List<IHospitalMembers> InitialMembers = new List<IHospitalMembers>();//stores default users



        public static void AddObjects(IHospitalMembers person) {//store any added objects to its respective file
            string filename = "";
            if (person is Doctor) {
                filename = "doctor.txt";
            }
            if (person is Patient) {
                filename = "patient.txt";
            }
            FileStream write = new FileStream(filename, FileMode.Append, FileAccess.Write);
            person.Password = random.Next(10000, 99999999).ToString();//generate random password
            //store added member/object to list
            string personStringed = person.ToString() + "\n";
            byte[] writeArr = Encoding.UTF8.GetBytes(personStringed);
            write.Write(writeArr, 0, personStringed.Length);
            write.Close();
            AllMembers.Add(person);//add to list, which menus derive its objects from
        }

        public static void InitialiseArray() {//store all ID in ID.txt to an array
            string[] lines = System.IO.File.ReadAllLines("ID.txt");
            foreach(String ID in lines) {
                CurrentStoredIDs.Add(ID);
            }
        }

        public static String GenerateRandomID() {
            InitialiseArray();
            string GeneratedRandomID = random.Next(10000, 99999999).ToString();//generate random ID
            while (CurrentStoredIDs.Contains(GeneratedRandomID)) {//regenerate ID if ID already exists
                GeneratedRandomID = random.Next(10000, 99999999).ToString();
            }
            FileStream writeID = new FileStream("ID.txt", FileMode.Append, FileAccess.Write);
            string StoreID = GeneratedRandomID + "\n";
            byte[] IDArr = Encoding.UTF8.GetBytes(StoreID);
            writeID.Write(IDArr, 0, StoreID.Length);//write (store) to ID.txt
            writeID.Close();
            return GeneratedRandomID;
        }


        public static String LoginCheck() {

            Console.SetCursorPosition(5, 5);
            String UserID = Console.ReadLine();
            Console.SetCursorPosition(11, 6);

            string Password = "";

            do {//astrix for password
                keyInfo = Console.ReadKey(true); // Hide pressed key
                if (keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Backspace)//add pressed keys
                {
                    Password += keyInfo.KeyChar;
                    Console.Write("*");
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && Password.Length>0)//backspace function
                {
                    Password = Password.Remove(Password.Length - 1);
                    Console.Write("\b \b");//deletes last char + moves caret back

                }
            } 
            while (keyInfo.Key != ConsoleKey.Enter);//loop if enter not pressed

            Console.SetCursorPosition(1, 7);
            String[] dataBase = {"patient.txt", "doctor.txt", "administrator.txt", "receptionist.txt"};//files for cross-checking

            foreach (String data in dataBase) {
                string[] lines = System.IO.File.ReadAllLines(data);
                foreach (string line in lines) {

                    string[] splits = line.Split('|');

                    if (UserID == splits[0] && Password == splits[1]) {//cross-examine login details in each file
                        Console.WriteLine("Valid Credentials");
                        return splits[0];
  
                    }
                }
            }
            return null;
        }


        public static void SelectMenu(IHospitalMembers member) {//pass objects into the menu for use
            //print out the respective user menu
            if (member is Patient) {
                Patient.PatientMenu(member);
            }
            else if (member is Doctor) {
                Doctor.DoctorMenu(member);
            }
            else if (member is Administrator) {
                Administrator.AdministratorMenu(member);
            }
            else if (member is Receptionist) {
                Receptionist.ReceptionistMenu(member);
            }
        }

        public static void LoginPage() {//decides who's logged in
            Utils.PrintLogin();
            string receivedId = LoginCheck();//grab logged in user id

            if (receivedId != null) {//if login & password is valid
                foreach (var member in AllMembers) {
                    if (member.ID == receivedId) {
                        SelectMenu(member);//login as that user
                    }
                }
            }
            else {//otherwise ask to try again
                Console.WriteLine("Try again (Y/N)?");
                keyInfo = Console.ReadKey();
                while (keyInfo.Key != ConsoleKey.N && keyInfo.Key != ConsoleKey.Y) {
                    Console.SetCursorPosition(0, 9);
                    Console.Write("Press Y for Yes and N for No");
                    Console.SetCursorPosition(18, 7);

                    keyInfo = Console.ReadKey();

                }
                if (keyInfo.Key == ConsoleKey.Y) {
                    Console.Clear();
                    LoginPage();
                }

            }
        }


        public static void InitialiseLoggedUsers(List<IHospitalMembers> InitialMembers) {
            foreach (var person in InitialMembers) {

                string filename = "";
                if (person is Patient) {
                    filename = "patient.txt";
                }
                else if (person is Doctor) {
                    filename = "doctor.txt";

                }
                else if (person is Administrator) {
                    filename = "administrator.txt";
                }
                else if (person is Receptionist) {
                    filename = "receptionist.txt";
                }


                if (!File.Exists(filename)) {//if no file, make
                    FileStream write = new FileStream(filename, FileMode.Append, FileAccess.Write);

                    //give member random password and id
                    person.ID = GenerateRandomID();
                    person.Password = random.Next(10000, 99999999).ToString();

                    //store member details to respective txt file
                    string personStringed = person.ToString() + "\n";//add logged in members
                    byte[] writeArr = Encoding.UTF8.GetBytes(personStringed);
                    write.Write(writeArr, 0, personStringed.Length);
                    AllMembers.Add(person);//add to list, which menus derive its objects from
                    write.Close();

                    //store randomID given to txt file ID to prevent duplicate IDs when adding any objects later
                    FileStream wroteID = new FileStream("ID.txt", FileMode.Append, FileAccess.Write);
                    string StoredID = person.ID + "\n";
                    byte[] IDArray = Encoding.UTF8.GetBytes(StoredID);
                    wroteID.Write(IDArray, 0, StoredID.Length);
                    wroteID.Close();
                    continue;
                }
                else {//if file already exists, retrieve and instantiate each object from file 
                    string[] lines = System.IO.File.ReadAllLines(filename);

                    foreach (string line in lines) {
                        string[] EachFileLine = line.Split('|');
                        if (person is Doctor) {
                            AllMembers.Add(new Doctor(EachFileLine[0], EachFileLine[1], EachFileLine[2], EachFileLine[3], EachFileLine[4], EachFileLine[5], EachFileLine[6], EachFileLine[7], EachFileLine[8], EachFileLine[9]));
                        }
                        else if (person is Receptionist) {
                            AllMembers.Add(new Receptionist(EachFileLine[0], EachFileLine[1], EachFileLine[2], EachFileLine[3], EachFileLine[4], EachFileLine[5], EachFileLine[6], EachFileLine[7], EachFileLine[8], EachFileLine[9]));
                        }

                        else if (person is Administrator) {
                            AllMembers.Add(new Administrator(EachFileLine[0], EachFileLine[1], EachFileLine[2], EachFileLine[3], null, null, null, null, null, null));
                        }

                        else if (person is Patient) {
                            AllMembers.Add(new Patient(EachFileLine[0], EachFileLine[1], EachFileLine[2], EachFileLine[3], EachFileLine[4], EachFileLine[5], EachFileLine[6], EachFileLine[7], EachFileLine[8], EachFileLine[9], new Doctor(EachFileLine[10], EachFileLine[11], EachFileLine[12], EachFileLine[13], EachFileLine[14], EachFileLine[15], EachFileLine[16], EachFileLine[17], EachFileLine[18], EachFileLine[19])));
                        }
                    }
                    
                }
            }
        }

        public static void initialiseFiles() {//spawn in the files
            if (!File.Exists("ID.txt")) {
                File.Create("ID.txt").Close();
            }
            if (!File.Exists("Appointment.txt")) {
                File.Create("Appointment.txt").Close();
            }
            if (!File.Exists("DoctorsReminded.txt")) {
                File.Create("DoctorsReminded.txt").Close();
            }

        }
        static void Main(string[] args) {
            initialiseFiles();
            //  setup default users
            Doctor initialDoctor = new Doctor("ID", "PassWord", "Bob", "Asmon", "bigfathotmail@gmail.com", "0284818821", "12", "big street", "Sydney", "NSW");
            InitialMembers.Add(new Receptionist("ID", "PassWord", "Fat", "Tony", "FatTony@gmail.com", "0221872712", "12", "Tony Street", "Sydney", "NSW"));
            InitialMembers.Add(initialDoctor);
            InitialMembers.Add(new Administrator("ID", "PassWord", "david", "adminsama", null, null, null, null, null, null));
            InitialMembers.Add(new Patient("ID", "PassWord", "Dylan", "Singhabahu", "realnotfake@email.com", "0533821849", "32", "brussel crusset", "Sydney", "NSW", initialDoctor));
            InitialiseLoggedUsers(InitialMembers);

            LoginPage();
        }
    }
}
