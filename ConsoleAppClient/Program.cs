using ConsoleAppClient.Controllers;
using Core.Services;

namespace ConsoleClient
{
    internal class Program
    {
        private static void Main()
        {
            AddressCLIController addressView = new(new AddressService());

            while (true)
            {
                ShowMainMenu();
                string input = System.Console.ReadLine() ?? "0";

                if (input == "0")
                    break;

                switch (input)
                {
                    case "1":
                        addressView.RunMenu();
                        break;
                    default:
                        System.Console.WriteLine("Unknown option.");
                        break;
                }
            }

            System.Console.WriteLine("Application closed.");
        }

        private static void ShowMainMenu()
        {
            System.Console.WriteLine("\n===== BOOK FAIR OPTIONS =====");
            System.Console.WriteLine("1: Addresses");
            System.Console.WriteLine("0: Exit");
            System.Console.Write("Choose: ");
        }
    }
}
