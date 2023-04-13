using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using OOPTask13;
using Information = OOPTask13.Information;

bool more = false;
int choice;
string path;

Console.WriteLine("Welcome to JSON file processing.");
Console.Write("Enter the JSON file path: ");
path = Console.ReadLine();
if (String.IsNullOrEmpty(path))
{
    Console.WriteLine("The field was left empty, try again!");
}while (String.IsNullOrEmpty(path));

do
{
    Console.WriteLine("Select an action (1-5):");
    Console.WriteLine("1. Show all the members");
    Console.WriteLine("2. Add a new member");
    Console.WriteLine("3. Modify information");
    Console.WriteLine("4. Remove information");
    Console.WriteLine("5. Leave");
    Console.WriteLine(" ");
    Console.Write("Select an action: ");

    string received = Console.ReadLine();
    while (!Int32.TryParse(received, out choice) || choice < 1 || choice > 5)
    {
        Console.Write("Not valid, try again: ");
        received = Console.ReadLine();
    }

    InformationHandlet informationHandlet = new InformationHandlet();

    switch (choice)
    {
        case 1:
            Console.WriteLine("You chose to show all the members:");
            informationHandlet.ShowAll(path);
            break;

        case 2:
            Console.WriteLine("You chose to add a new member.");
            informationHandlet.AddMember(path);
            break;

        case 3:
            Console.WriteLine("You chose to modify information.");
            informationHandlet.ModifyInformation(path);
            break;

        case 4:
            Console.WriteLine("You chose to remove information.");
            informationHandlet.RemoveInformation(path);
            break;

        case 5:
            Console.WriteLine("You chose to leave.");
            break;      
    }

    Console.WriteLine();
    Console.Write("Do you want to continue with the new operation? (Y/N): ");
    string decision = Console.ReadLine().ToUpper();
    if (decision.StartsWith("Y"))
        more = true;
    else
        more = false;

} while (more);





