using ConsoleAppClient.Utils;
using Core.Models;
using Core.Services;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppClient.Controllers
{
    public class PurchaseCLIController
    {
        private PurchaseService purchaseService;

        public PurchaseCLIController(PurchaseService purchaseService)
        {
            this.purchaseService = purchaseService;
        }

        public void RunMenu()
        {
            while (true)
            {
                showMenu();
                string input = System.Console.ReadLine() ?? "0";

                if (input == "0")
                    break;

                handleMenuInput(input);
            }
        }

        private void showMenu()
        {
            System.Console.WriteLine("\n=== PURCHASE MENU ===");
            System.Console.WriteLine("1. Add purchase");
            System.Console.WriteLine("2. Show all purchases");
            System.Console.WriteLine("3. Update purchase");
            System.Console.WriteLine("4. Remove purchase");
            System.Console.WriteLine("0. Back");
        }

        private void handleMenuInput(string input)
        {
            switch (input)
            {
                case "1":
                    addPurchase();
                    break;
                case "2":
                    showAllPurchases();
                    break;
                case "3":
                    updatePurchase();
                    break;
                case "4":
                    removePurchase();
                    break;
            }
        }

        private void showAllPurchases()
        {
            List<Purchase> purchases = purchaseService.GetAll();
            printPurchases(purchases);
        }

        private void printPurchases(List<Purchase> purchases)
        {
            System.Console.WriteLine("\nPurchases:");
            foreach (Purchase p in purchases)
            {
                System.Console.WriteLine(p);
                System.Console.WriteLine("---------------------");
            }
        }

        private void addPurchase()
        {
            Purchase purchase = inputPurchase();
            purchaseService.Add(purchase);
            System.Console.WriteLine("Purchase added.");
        }

        private void updatePurchase()
        {
            System.Console.WriteLine("Enter purchase id:");
            int id = ConsoleViewUtils.SafeInputInt();

            Purchase purchase = inputPurchase();
            purchase.Id = id;

            Purchase? updated = purchaseService.Update(purchase);
            if (updated == null)
            {
                System.Console.WriteLine("Purchase not found.");
                return;
            }

            System.Console.WriteLine("Purchase updated.");
        }

        private void removePurchase()
        {
            System.Console.WriteLine("Enter purchase id:");
            int id = ConsoleViewUtils.SafeInputInt();

            Purchase? removed = purchaseService.Remove(id);
            if (removed == null)
            {
                System.Console.WriteLine("Purchase not found.");
                return;
            }

            System.Console.WriteLine("Purchase removed.");
        }

        private Purchase inputPurchase()
        {
            System.Console.WriteLine("Enter visitor id:");
            int visitorId = ConsoleViewUtils.SafeInputInt();

            System.Console.WriteLine("Enter book id:");
            int bookId = ConsoleViewUtils.SafeInputInt();

            System.Console.WriteLine("Enter rating (1-5):");
            int rating = ConsoleViewUtils.SafeInputInt();

            System.Console.WriteLine("Enter purchase date (dd.MM.yyyy. or yyyy-MM-dd):");
            DateTime purchaseDate = ConsoleViewUtils.SafeInputDate();

            System.Console.WriteLine("Enter comment:");
            string comment = System.Console.ReadLine() ?? "";

            return new Purchase
            {
                VisitorId = visitorId,
                BookId = bookId,
                PurchaseDate = purchaseDate,
                Rating = rating,
                Comment = comment
            };
        }
    }
}
