using ConsoleAppClient.Utils;
using Core;
using Core.Models;
using Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleAppClient.Controllers
{
    public class VisitorConsoleView
    {
        private readonly VisitorService _visitorService;

        public VisitorConsoleView(VisitorService visitorService)
        {
            _visitorService = visitorService;
        }

        public void RunMenu()
        {
            while (true)
            {
                ShowMenu();
                string input = System.Console.ReadLine() ?? "0";
                if (input == "0") break;
                HandleMenuInput(input);
            }
        }

        private void ShowMenu()
        {
            System.Console.WriteLine("\nVisitor menu:");
            System.Console.WriteLine("1: Show all visitors");
            System.Console.WriteLine("2: Add visitor");
            System.Console.WriteLine("3: Update visitor");
            System.Console.WriteLine("4: Remove visitor");
            System.Console.WriteLine("5: Show and sort visitors");
            System.Console.WriteLine("0: Back");
        }

        private void HandleMenuInput(string input)
        {
            switch (input)
            {
                case "1": ShowAllVisitors(); break;
                case "2": AddVisitor(); break;
                case "3": UpdateVisitor(); break;
                case "4": RemoveVisitor(); break;
            }
        }

        private void ShowAllVisitors()
        {
            PrintVisitors(_visitorService.GetAll());
        }

        private void PrintVisitors(List<Visitor> visitors)
        {
            System.Console.WriteLine("\nVisitors:");
            foreach (Visitor v in visitors)
            {
                System.Console.WriteLine(v);
            }
        }

        private void AddVisitor()
        {
            Visitor visitor = InputVisitor();
            _visitorService.AddVisitor(visitor);
            System.Console.WriteLine("Visitor added.");
        }

        private void UpdateVisitor()
        {
            int id = InputId();
            Visitor visitor = InputVisitor();
            visitor.Id = id;

            Visitor? updated = _visitorService.UpdateVisitor(visitor);
            if (updated == null)
            {
                System.Console.WriteLine("Visitor not found.");
                return;
            }

            System.Console.WriteLine("Visitor updated.");
        }

        private void RemoveVisitor()
        {
            int id = InputId();
            Visitor? removed = _visitorService.RemoveVisitor(id);

            if (removed == null)
            {
                System.Console.WriteLine("Visitor not found.");
                return;
            }

            System.Console.WriteLine("Visitor removed.");
        }


        private Visitor InputVisitor()
        {
            System.Console.WriteLine("First name:");
            string firstName = System.Console.ReadLine() ?? "";

            System.Console.WriteLine("Last name:");
            string lastName = System.Console.ReadLine() ?? "";

            System.Console.WriteLine("Date of birth (dd.MM.yyyy. or yyyy-MM-dd):");
            DateTime dateOfBirth = ConsoleViewUtils.SafeInputDate();

            System.Console.WriteLine("Address id:");
            int addressId = ConsoleViewUtils.SafeInputInt();

            System.Console.WriteLine("Phone:");
            string phone = System.Console.ReadLine() ?? "";

            System.Console.WriteLine("Email:");
            string email = System.Console.ReadLine() ?? "";

            System.Console.WriteLine("Membership card number:");
            string cardNumber = System.Console.ReadLine() ?? "";

            System.Console.WriteLine("Membership year:");
            int year = ConsoleViewUtils.SafeInputInt();

            System.Console.WriteLine("Status (0 = Regular, 1 = VIP):");
            int statusInput = ConsoleViewUtils.SafeInputInt();
            VisitorStatus status = statusInput == 1 ? VisitorStatus.VIP : VisitorStatus.Regular;

            System.Console.WriteLine("Average rating:");
            double averageRating = ConsoleViewUtils.SafeInputDouble();

            return new Visitor
            {
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                AddressId = addressId,
                Phone = phone,
                Email = email,
                MembershipCardNumber = cardNumber,
                MembershipYear = year,
                Status = status,
                AverageRating = averageRating
            };
        }

        private int InputId()
        {
            System.Console.WriteLine("Enter visitor id:");
            return ConsoleViewUtils.SafeInputInt();
        }
    }
}
