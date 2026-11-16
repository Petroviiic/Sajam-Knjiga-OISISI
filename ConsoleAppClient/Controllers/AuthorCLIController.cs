using ConsoleClient.Utils;
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
    public class AuthorCLIController
    {
        private AuthorService authorService;

        public AuthorCLIController(AuthorService authorService)
        {
            this.authorService = authorService;
        }

        public void RunMenu()
        {
            while (true)
            {
                showMenu();

                string userInput = System.Console.ReadLine() ?? "0";
                if (userInput == "0")
                    break;

                handleMenuInput(userInput);
            }
        }

        private void showMenu()
        {
            System.Console.WriteLine("\nChoose an option: ");
            System.Console.WriteLine("1: Add author");
            System.Console.WriteLine("2: Show all authors");
            System.Console.WriteLine("3: Update author");
            System.Console.WriteLine("4: Remove author");
            System.Console.WriteLine("0: Close");
        }

        private void handleMenuInput(string input)
        {
            switch (input)
            {
                case "1":
                    addAuthor();
                    break;
                case "2":
                    showAllAuthors();
                    break;
                case "3":
                    updateAuthor();
                    break;
                case "4":
                    removeAuthor();
                    break;
            }
        }

        private void showAllAuthors()
        {
            List<Author> authors = authorService.GetAll();
            foreach (Author author in authors)
            {
                System.Console.WriteLine(author);
            }
        }


        private void addAuthor()
        {
            Author author = inputAuthor();
            authorService.Add(author);
            System.Console.WriteLine("Author added");
        }

        private Author inputAuthor()
        {
            System.Console.WriteLine("Enter first name: ");
            string firstName = System.Console.ReadLine() ?? string.Empty;

            System.Console.WriteLine("Enter last name: ");
            string lastName = System.Console.ReadLine() ?? string.Empty;

            System.Console.WriteLine("Enter date of birth (dd.MM.yyyy. or yyyy-MM-dd): ");
            DateTime dateOfBirth = ConsoleViewUtils.SafeInputDate();


            System.Console.WriteLine("Enter address id: ");
            int addressid = ConsoleViewUtils.SafeInputInt();

            System.Console.WriteLine("Enter phone: ");
            string phone = System.Console.ReadLine() ?? string.Empty;

            System.Console.WriteLine("Enter email: ");
            string email = System.Console.ReadLine() ?? string.Empty;

            System.Console.WriteLine("Enter id card num: ");
            string idcn = System.Console.ReadLine() ?? string.Empty;

            System.Console.WriteLine("Enter years of experience: ");
            int yearsOfExperience = ConsoleViewUtils.SafeInputInt();

            return new Author(firstName, lastName, dateOfBirth, addressid, phone, email, idcn, yearsOfExperience);
        }

        private void updateAuthor()
        {
            int id = inputId();
            Author author = inputAuthor();
            author.Id = id;

            Author? updatedAuthor = authorService.Update(author);
            if (updatedAuthor is null)
            {
                System.Console.WriteLine("Author not found");
                return;
            }

            System.Console.WriteLine("Author updated");
        }

        private int inputId()
        {
            System.Console.WriteLine("Enter author id: ");
            return ConsoleViewUtils.SafeInputInt();
        }

        private void removeAuthor()
        {
            int id = inputId();
            Author? removedAuthor = authorService.Remove(id);
            if (removedAuthor is null)
            {
                System.Console.WriteLine("Author not found");
                return;
            }

            System.Console.WriteLine("Author removed");
        }


    }
}
