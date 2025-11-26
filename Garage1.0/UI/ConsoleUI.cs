using Garage1._0.Handler;

using System;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

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
                        HandleAddVehicle();
                        break;
                    case "4":
                        HandleRemoveVehicle();
                        break;
                    case "5":
                        HandleSearchMenu();
                        break;

                    case "0":
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

        
        private void HandleAddVehicle()
        {
            Console.Clear();
            Console.Clear();
            Console.WriteLine("=== Lägg till fordon ===\n");

            if (!_handler.HasGarage)
            {
                ShowError("Inget garage finns. Skapa ett garage först.");
                Pause();
                return;
            }

            Console.WriteLine("Vilken typ av fordon vill du lägga till?");
            Console.WriteLine("1) Bil");
            Console.WriteLine("2) Båt");
            Console.WriteLine("3) Motorcykel");
            Console.WriteLine("4) Buss");
            Console.WriteLine("5) Flygplan");
            Console.WriteLine();

            Console.Write("Val: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    HandleAddCar();
                    break;
                case "2":
                    HandleAddBoat();
                    break;
                // osv
                default:
                    ShowError("Ogiltig fordonstyp.");
                    Pause();
                    break;
            }
        }

        private void HandleAddBoat()
        {
            throw new NotImplementedException();
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
            Console.WriteLine("3) Lägg till fordon");
            Console.WriteLine("4) Ta bort fordon");
            Console.WriteLine("5) Sök efter fordon");
            Console.WriteLine("0) Avsluta");
            Console.WriteLine();
        }

        private void HandleSearchMenu()
        {
            while (true)
            {
                Console.Clear();
                ShowHeader();

                if (!_handler.HasGarage)
                {
                    ShowError("Inget garage finns. Skapa ett garage först.");
                    Pause();
                    return;
                }

                Console.WriteLine("=== Sök fordon ===\n");
                Console.WriteLine("1) Snabbsök på registreringsnummer");
                Console.WriteLine("2) Avancerad sökning (typ, färg, hjul, kombination)");
                Console.WriteLine("0) Tillbaka till huvudmenyn");
                Console.Write("\nVälj ett alternativ: ");

                string? choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        HandleQuickSearchByRegNr();
                        break;

                    case "2":
                        HandleAdvancedSearch();
                        break;

                    case "0":
                        // Back to mainmenu
                        return;

                    default:
                        ShowError("Ogiltigt menyval. Försök igen.");
                        Pause();
                        break;
                }
            }
        }

        private void HandleQuickSearchByRegNr()
        {
            Console.Clear();
            ShowHeader();
            Console.WriteLine("Snabbsök – registreringsnummer\n");

            string regNr = ReadString("Ange registreringsnummer: ");

            var vehicle = _handler.FindByRegNr(regNr);

            if (vehicle is null)
            {
                ShowError("Inget fordon med det registreringsnumret hittades.");
            }
            else
            {
                Console.WriteLine("\nFordon hittades:\n");
                Console.WriteLine(vehicle);
            }

            Pause();
        }

        private void HandleAdvancedSearch()
        {
            Console.Clear();
            ShowHeader();
            Console.WriteLine("Avancerad sökning\n");
            Console.WriteLine("Lämna fält tomma om du inte vill filtrera på dem.\n");

            // 1. Type of vehicle
            Console.WriteLine("Fordonstyp (lämna tomt för alla typer):");
            Console.WriteLine("1) Bil");
            Console.WriteLine("2) Båt");
            Console.WriteLine("3) Motorcykel");
            Console.WriteLine("4) Buss");
            Console.WriteLine("5) Flygplan");
            Console.Write("Val (1–5 eller Enter för alla): ");

            string? typeChoice = Console.ReadLine()?.Trim();

            string? vehicleType = typeChoice switch
            {
                "1" => "Car",
                "2" => "Boat",
                "3" => "Motorcycle",
                "4" => "Bus",
                "5" => "Airplane",
                _ => null // null = no filter
            };

            Console.WriteLine();
            Console.WriteLine("Övriga filter (valfria — lämna tomt för att hoppa över):\n");

            Console.Write("Registreringsnummer: ");
            string? regInput = Console.ReadLine();
            string? regNr = string.IsNullOrWhiteSpace(regInput) ? null : regInput.Trim();

            Console.Write("Färg: ");
            string? colorInput = Console.ReadLine();
            string? color = string.IsNullOrWhiteSpace(colorInput) ? null : colorInput.Trim();

            Console.Write("Antal hjul: ");
            string? wheelsInput = Console.ReadLine();
            int? wheels = null;
            if (!string.IsNullOrWhiteSpace(wheelsInput) &&
                int.TryParse(wheelsInput, out int w))
            {
                wheels = w;
            }
            Console.Write("Modell: ");
            string? modelInput = Console.ReadLine();
            string? model = string.IsNullOrWhiteSpace(modelInput) ? null : modelInput.Trim();

            var results = _handler.AdvancedSearch(vehicleType, regNr, color, wheels, model);

            Console.WriteLine();
            Console.WriteLine("Sökresultat:\n");

            bool any = false;
            foreach (var v in results)
            {
                any = true;
                Console.WriteLine(v);
            }

            if (!any)
            {
                Console.WriteLine("Inga fordon matchade sökkriterierna.");
            }

            Pause();
        }



        private void HandleCreateGarage()
        {
            Console.Clear();
            ShowHeader();
            Console.WriteLine("Ange garagekapacitet (antal platser):.");
            int capacity;
            bool parse = Int32.TryParse(Console.ReadLine(), out capacity);
            _handler.CreateGarage(capacity);
            Console.WriteLine($"Garage skapat för {capacity} platser");
            Console.WriteLine("Tryck Enter för att återgå till menyn...");
            Console.ReadLine();
        }

        private void HandleAddCar()
        {
            Console.Clear();
            Console.WriteLine("=== Lägg till bil ===\n");
            Console.WriteLine();

            string regNr = ReadString("Registreringsnummer: ");
            string color = ReadString("Färg: ");
            int wheels = ReadInt("Antal hjul: ");
            string model = ReadString("Bilmodell: ");
            string fueltype = ReadString("Bränsletyp: ");

            bool success = _handler.CreateAndAddCar( regNr, color, wheels, model,  fueltype);

            if (success)
                Console.WriteLine("Bilen lades till i garaget.");
            else
                ShowError("Kunde inte lägga till bil (är garaget fullt?)");

            Pause();
        }

        private void HandleRemoveVehicle()
        {
            Console.Clear();
            Console.Clear();
            Console.WriteLine("=== Ta bort fordon ===\n");
            if (!_handler.HasGarage)
            {
                ShowError("Inget garage finns. Skapa ett garage först.");
                Pause();
                return;
            }
            Console.WriteLine();

            string regNr = ReadString("Registreringsnummer: ");
            bool success = _handler.RemoveCar(regNr);

            if (success)
                Console.WriteLine("Bilen togs bort från garaget.");
            else
                ShowError("Kunde inte ta bort bil");

            Pause();

        }


        private void HandleListVehicles()
        {
            Console.Clear();
            Console.WriteLine("=== Lista fordon ===\n");

            if (!_handler.HasGarage)
            {
                Console.WriteLine("Inget garage finns. Skapa ett garage först.");
                Pause();
                return;
            }

            var vehicles = _handler.GetAllVehicles();

            bool any = false;

            foreach (var v in vehicles)
            {
                any = true;
                Console.WriteLine(v);   // Call Vehicle.ToString()
            }

            if (!any)
            {
                Console.WriteLine("Garaget är tomt.");
            }

            Pause();
        }

    

        private int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int value))
                {
                    return value;
                }

                ShowError("Du måste skriva ett heltal.");
            }
        }

        private string ReadString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }

                ShowError("Värdet kan inte vara tomt.");
            }
        }

        private void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Tryck Enter för att fortsätta...");
            Console.ReadLine();
        }


    }
}
