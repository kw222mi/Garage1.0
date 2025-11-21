using Garage1._0.Handler;

using System;

namespace Garage1._0.UI
{
    public class ConsoleUI
    {
        private readonly GarageHandler _handler;

        public ConsoleUI(GarageHandler handler)
        {
            _handler = handler;
        }

        public void Run()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                ShowHeader();
                ShowMenu();

                Console.Write("Välj ett alternativ: ");
                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        HandleCreateGarage();
                        break;

                    case "2":
                        HandleListVehicles();
                        break;

                    case "3":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Ogiltigt val, tryck Enter för att försöka igen...");
                        Console.ReadLine();
                        break;
                }
            }

            Console.WriteLine("Programmet avslutas. Tryck Enter...");
            Console.ReadLine();
        }

        private void ShowHeader()
        {
            Console.WriteLine("===================================");
            Console.WriteLine("          Garage 1.0");
            Console.WriteLine("===================================");
            Console.WriteLine();
        }

        private void ShowMenu()
        {
            Console.WriteLine("1) Skapa nytt garage");
            Console.WriteLine("2) Lista fordon");
            Console.WriteLine("3) Avsluta");
            Console.WriteLine();
        }

        private void HandleCreateGarage()
        {
            Console.Clear();
            ShowHeader();
            Console.WriteLine("[TODO] Här kommer vi senare fråga efter kapacitet och skapa garage.");
            Console.WriteLine();
            Console.WriteLine("Tryck Enter för att återgå till menyn...");
            Console.ReadLine();
        }

        private void HandleListVehicles()
        {
            Console.Clear();
            ShowHeader();
            Console.WriteLine("[TODO] Här kommer vi senare att lista alla fordon i garaget.");
            Console.WriteLine();
            Console.WriteLine("Tryck Enter för att återgå till menyn...");
            Console.ReadLine();
        }
    }
}
