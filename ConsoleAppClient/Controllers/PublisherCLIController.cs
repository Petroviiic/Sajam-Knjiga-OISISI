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
    public class PublisherCLIController
    {
        private readonly PublisherService _publisherService;

        public PublisherCLIController(PublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        public void RunMenu()
        {
            while (true)
            {
                ShowMenu();
                string input = System.Console.ReadLine() ?? "0";

                if (input == "0")
                    break;

                HandleMenuInput(input);
            }
        }

        private void ShowMenu()
        {
            System.Console.WriteLine("\n=== PUBLISHER MENU ===");
            System.Console.WriteLine("1. Show all publishers");
            System.Console.WriteLine("2. Add publisher");
            System.Console.WriteLine("3. Update publisher");
            System.Console.WriteLine("4. Remove publisher");
            System.Console.WriteLine("0. Back");
        }

        private void HandleMenuInput(string input)
        {
            switch (input)
            {
                case "1":
                    ShowAllPublishers();
                    break;
                case "2":
                    AddPublisher();
                    break;
                case "3":
                    UpdatePublisher();
                    break;
                case "4":
                    RemovePublisher();
                    break;
            }
        }

        private void ShowAllPublishers()
        {
            List<Publisher> publishers =
                _publisherService.GetAll();

            PrintPublishers(publishers);
        }

        private void PrintPublishers(List<Publisher> publishers)
        {
            System.Console.WriteLine("\nPublishers:");
            foreach (Publisher p in publishers)
            {
                System.Console.WriteLine(p);
                System.Console.WriteLine("---------------------");
            }
        }

        private void AddPublisher()
        {
            Publisher publisher = InputPublisher();
            _publisherService.AddPublisher(publisher);
            System.Console.WriteLine("Publisher added.");
        }

        private void UpdatePublisher()
        {
            System.Console.WriteLine("Enter publisher id:");
            int id = ConsoleViewUtils.SafeInputInt();

            Publisher publisher = InputPublisher();
            publisher.Id = id;

            Publisher? updated = _publisherService.UpdatePublisher(publisher);
            if (updated == null)
            {
                System.Console.WriteLine("Publisher not found.");
                return;
            }

            System.Console.WriteLine("Publisher updated.");
        }

        private void RemovePublisher()
        {
            System.Console.WriteLine("Enter publisher id:");
            int id = ConsoleViewUtils.SafeInputInt();

            Publisher? removed = _publisherService.RemovePublisher(id);
            if (removed == null)
            {
                System.Console.WriteLine("Publisher not found.");
                return;
            }

            System.Console.WriteLine("Publisher removed.");
        }

        private Publisher InputPublisher()
        {
            System.Console.WriteLine("Enter publisher code:");
            string code = System.Console.ReadLine() ?? "";

            System.Console.WriteLine("Enter publisher name:");
            string name = System.Console.ReadLine() ?? "";

            System.Console.WriteLine("Enter chief editor author id (0 if none):");
            int chiefEditorId = ConsoleViewUtils.SafeInputInt();

            System.Console.WriteLine("Enter author ids separated by comma (empty if none):");
            List<int> authorIds = ConsoleViewUtils.ParseIntList(System.Console.ReadLine() ?? "");

            System.Console.WriteLine("Enter published book ids separated by comma (empty if none):");
            List<int> publishedBookIds = ConsoleViewUtils.ParseIntList(System.Console.ReadLine() ?? "");

            return new Publisher
            {
                Code = code,
                Name = name,
                ChiefEditorId = chiefEditorId,
                AuthorIds = authorIds,
                PublishedBookIds = publishedBookIds
            };
        }

    }
}
