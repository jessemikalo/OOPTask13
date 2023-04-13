using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Reflection;

namespace OOPTask13
{
    internal class Information
    {
        public Information(string name, DateTime? dateOfBirth, List<ContactInformation> contactInformation)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
            ContactInformation = contactInformation;
        }

        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public List<ContactInformation> ContactInformation { get; set; }
    }

    //Inner structure:
    internal class ContactInformation
    {
        public ContactInformation(string email, string mobile)
        {
            Email = email;
            Mobile = mobile;
        }

        public string Email { get; set; }
        public string Mobile { get; set; }
    }

    internal class InformationHandlet
    {
        public void ShowAll(string path)
        {
            Console.WriteLine();
            List<Information> members;

            using (StreamReader streamReader = new StreamReader(path))
            {
                var jsonString = streamReader.ReadToEnd();
                members = JsonConvert.DeserializeObject<List<Information>>(jsonString);
            }
            foreach (var items in members)
            {
                Console.WriteLine("Name: {0}, date of birth: {1}", items.Name, items.DateOfBirth.Value.ToString("yyyy-M-d"));
                //Inner structure:
                foreach (var contact in items.ContactInformation)
                {
                    Console.WriteLine("Email: {0}, mobile: {1}", contact.Email, contact.Mobile);
                    Console.WriteLine(" ");
                }
            }

        }
        public void AddMember(string path)
        {
            string received;
            DateTime dateOfBirth;

            Console.Write("Enter the name: ");
            string name = Console.ReadLine();
            if (String.IsNullOrEmpty(name))
            {
                Console.Write("The field was empty, try again: ");
                name = Console.ReadLine();
            }

            Console.Write("Enter the date of birth (yyyy-M-d): ");
            received = Console.ReadLine();
            while (!DateTime.TryParse(received, out dateOfBirth))
            {
                Console.Write("Not valid, try again: ");
                received = Console.ReadLine();
            }

            Console.Write("Enter the email: ");
            string email = Console.ReadLine();
            while (!email.Contains("@"))
            {
                Console.Write("Invalid email, please enter a valid email: ");
                email = Console.ReadLine();
            }

            Console.Write("Enter the mobile: ");
            string mobile = Console.ReadLine();
            while (!mobile.All(c => char.IsDigit(c) || c == ' '))
            {
                Console.Write("Invalid mobile number, please enter a valid number containing only digits and spaces: ");
                mobile = Console.ReadLine();
            }

            ContactInformation contactInformation = new ContactInformation(email, mobile);
            List<ContactInformation> contactInformationList = new List<ContactInformation> { contactInformation };

            Information newMember = new Information(name, dateOfBirth, contactInformationList);

            List<Information> members;
            using (StreamReader streamReader = new StreamReader(path))
            {
                var jsonString = streamReader.ReadToEnd();
                members = JsonConvert.DeserializeObject<List<Information>>(jsonString);
            }

            members.Add(newMember);

            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
                {
                    serializer.Serialize(jsonWriter, members);
                }
                Console.WriteLine("New member added successfully.");
            }
        }

        public void ModifyInformation(string path)
        {
            Console.Write("Enter the name or partial name of the member to modify: ");
            string name = Console.ReadLine().ToLower();

            List<Information> members;
            using (StreamReader streamReader = new StreamReader(path))
            {
                var jsonString = streamReader.ReadToEnd();
                members = JsonConvert.DeserializeObject<List<Information>>(jsonString);
            }

            var matchedMembers = members.Where(mem => mem.Name.ToLower().Contains(name)).ToList();

            if (matchedMembers.Any())
            {
                if (matchedMembers.Count > 1)
                {
                    Console.WriteLine("Multiple members match the search:");
                    for (int i = 0; i < matchedMembers.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {matchedMembers[i].Name}");
                    }
                    Console.Write("Enter the number of the member you want to modify: ");
                    int selectedIndex;
                    while (!int.TryParse(Console.ReadLine(), out selectedIndex) || selectedIndex < 1 || selectedIndex > matchedMembers.Count)
                    {
                        Console.Write("Invalid choice, please enter a valid number: ");
                    }
                    selectedIndex--;
                }
                else
                {
                    Console.WriteLine("Found one member: {0}", matchedMembers[0].Name);
                }

                Information memberToModify = matchedMembers[0];

                Console.WriteLine("If you do not want to change any information, press ENTER.");

                Console.WriteLine("Current name is {0}.", memberToModify.Name);
                Console.Write("Enter new name: ");
                string newName = Console.ReadLine();
                if (!string.IsNullOrEmpty(newName))
                {
                    memberToModify.Name = newName;
                }

                Console.WriteLine("Current date of birth is {0}.", memberToModify.DateOfBirth.Value.ToString("yyyy-M-d"));
                Console.Write("Enter new date of birth (yyyy-M-d): ");
                DateTime newDateOfBirth;
                string dobInput = Console.ReadLine();
                while(!DateTime.TryParse(dobInput, out newDateOfBirth))
                {
                    Console.Write("Not valid, please enter a valid date of birth (yyy-M-d): ");
                    dobInput = Console.ReadLine();
                }
                if (!string.IsNullOrEmpty(dobInput) && DateTime.TryParse(dobInput, out newDateOfBirth))
                {
                    memberToModify.DateOfBirth = newDateOfBirth;
                }

                for (int i = 0; i < memberToModify.ContactInformation.Count; i++)
                { 

                    Console.WriteLine("Current email: {0}", memberToModify.ContactInformation[i].Email);
                    Console.Write("Enter new email: ");
                    string newEmail = Console.ReadLine();
                    while(!newEmail.Contains("@"))
                    {
                        Console.Write("Invalid email, please enter a valid email: ");
                        newEmail = Console.ReadLine();
                    }

                    if (!string.IsNullOrEmpty(newEmail))
                    {
                        memberToModify.ContactInformation[i].Email = newEmail;
                    }

                    Console.WriteLine("Current mobile: {0}", memberToModify.ContactInformation[i].Mobile);
                    Console.Write("Enter new mobile: ");
                    string newMobile = Console.ReadLine();
                    while (!newMobile.All(c => char.IsDigit(c) || c == ' '))
                    {
                        Console.Write("Invalid mobile number, please enter a valid number containing only digits and spaces: ");
                        newMobile = Console.ReadLine();
                    }
                    if (!string.IsNullOrEmpty(newMobile))
                    {
                        memberToModify.ContactInformation[i].Mobile = newMobile;
                    }
                }

                using (StreamWriter streamWriter = new StreamWriter(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        serializer.Serialize(jsonWriter, members);
                    }
                }
                Console.WriteLine("Member information updated successfully.");
            }
            else
            {
                Console.WriteLine("No member found with the given name.");
            }
        }

        public void RemoveInformation(string path)
        {
            Console.Write("Enter the name or partial name of the member to remove: ");
            string name = Console.ReadLine().ToLower(); ;

            List<Information> members;
            using (StreamReader streamReader = new StreamReader(path))
            {
                var jsonString = streamReader.ReadToEnd();
                members = JsonConvert.DeserializeObject<List<Information>>(jsonString);
            }

            var matchedMembers = members.Where(mem => mem.Name.ToLower().Contains(name)).ToList();

            if (matchedMembers.Any())
            {
                if (matchedMembers.Count > 1)
                {
                    Console.WriteLine("Multiple members match the search:");
                    for (int i = 0; i < matchedMembers.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {matchedMembers[i].Name}");
                    }
                    Console.Write("Enter the number of the member you want to remove: ");
                    int selectedIndex;
                    while (!int.TryParse(Console.ReadLine(), out selectedIndex) || selectedIndex < 1 || selectedIndex > matchedMembers.Count)
                    {
                        Console.Write("Invalid choice. Please enter a valid number: ");
                    }
                    selectedIndex--;

                    members.Remove(matchedMembers[selectedIndex]);
                }
                else
                {
                    Console.WriteLine("Found one member: {0}", matchedMembers[0].Name);
                    Console.Write("Do you want to remove this member? (Y/N): ");
                    string decision = Console.ReadLine().ToUpper();

                    if (decision.StartsWith("Y"))
                    {
                        members.Remove(matchedMembers[0]);
                    }
                }

                using (StreamWriter streamWriter = new StreamWriter(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        serializer.Serialize(jsonWriter, members);
                    }
                }
                Console.WriteLine("Member removed successfully.");
            }
            else
            {
                Console.WriteLine("No member found with the given name.");
            }
        }
    }
}
