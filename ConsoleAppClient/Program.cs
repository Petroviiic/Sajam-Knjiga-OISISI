using ConsoleAppClient.Controllers;
using Core.Services;

namespace ConsoleClient
{
    internal class Program
    {
        private static void Main()
        {
            AddressCLIController addressView = new(new AddressService());
            AuthorCLIController authorView = new(new AuthorService());
            VisitorConsoleView visitorView = new(new VisitorService());
            BookCLIController bookView = new(new BookService());


            while (true)
            {
                showMainMenu();
                string input = System.Console.ReadLine() ?? "0";

                if (input == "0")
                    break;

                switch (input)
                {
                    case "1":
                        addressView.RunMenu();
                        break;
                    case "2":
                        authorView.RunMenu();
                        break;
                    case "3":
                        visitorView.RunMenu();
                        break;
                    case "4":
                        bookView.RunMenu();
                        break;
                    default:
                        System.Console.WriteLine("Unknown option.");
                        break;
                }
            }

            System.Console.WriteLine("Application closed.");
        }

        private static void showMainMenu()
        {
            System.Console.WriteLine("\n===== BOOK FAIR OPTIONS =====");
            System.Console.WriteLine("1: Addresses");
            System.Console.WriteLine("2: Authors");
            System.Console.WriteLine("3: Visitors");
            System.Console.WriteLine("4: Books");
            System.Console.WriteLine("0: Exit");
            System.Console.Write("Choose: ");
        }
    }
}
