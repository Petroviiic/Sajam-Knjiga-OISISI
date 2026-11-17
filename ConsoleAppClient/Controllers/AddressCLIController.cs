using Core.Models;
using Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAppClient.Utils;

namespace ConsoleAppClient.Controllers
{
    public class AddressCLIController
    {
        // preuzeto sa 3. vjezbi
        private AddressService addressService;

        public AddressCLIController(AddressService addressService)
        {
            this.addressService = addressService;
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
            System.Console.WriteLine("1: Add address");
            System.Console.WriteLine("2: Show all addresses");
            System.Console.WriteLine("3: Update address");
            System.Console.WriteLine("4: Remove address");
            System.Console.WriteLine("0: Close");
        }

        private void handleMenuInput(string input)
        {
            switch (input)
            {
                case "1":
                    addAddress();
                    break;
                case "2":
                    showAllAddresses();
                    break;
                case "3":
                    updateAddress();
                    break;
                case "4":
                    removeAddress();
                    break;
            }
        }
        private void addAddress()
        {
            try
            {
                Address address = inputAddress();
                addressService.Add(address);
                System.Console.WriteLine("Address added.");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private Address inputAddress()
        {
            System.Console.WriteLine("Enter street name: ");
            string streetName = System.Console.ReadLine() ?? string.Empty;

            System.Console.WriteLine("Enter No.: ");
            int number = ConsoleViewUtils.SafeInputInt();

            System.Console.WriteLine("Enter city name: ");
            string city = System.Console.ReadLine() ?? string.Empty;

            System.Console.WriteLine("Enter country name: ");
            string country = System.Console.ReadLine() ?? string.Empty;


            return new Address(streetName, number, city, country);
        }

        private void showAllAddresses()
        {
            List<Address> addresses = addressService.GetAll();
            foreach (Address address in addresses)
            {
                System.Console.WriteLine(address);
            }
        }


        private void updateAddress()
        {
            int id = inputId();
            Address? existingAddress = addressService.GetById(id);
            if (existingAddress is null)
            {
                System.Console.WriteLine("Address not found.");
                return;
            }
            try
            {
                Address address = inputAddress();
                address.Id = id;

                Address? updatedAddress = addressService.Update(address);
                if (updatedAddress is null)
                {
                    System.Console.WriteLine("Address not found.");
                    return;
                }

                System.Console.WriteLine("Address updated.");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
            }       
        }

        private int inputId()
        {
            System.Console.WriteLine("Enter address id: ");
            return ConsoleViewUtils.SafeInputInt();
        }

        private void removeAddress()
        {
            int id = inputId();
            Address? removedAddress = addressService.Remove(id);
            if (removedAddress is null)
            {
                System.Console.WriteLine("Address not found");
                return;
            }

            System.Console.WriteLine("Address removed");
        }


    }
}
