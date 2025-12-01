using Garage1._0.Handler;
using System;

namespace Garage1._0.UI
{
    /// <summary>
    /// Console-based user interface for the Garage application.
    /// Responsible for showing menus, reading user input and delegating
    /// all business logic to the GarageHandler.
    /// </summary>
    public class ConsoleUI
    {
        /// <summary>
        /// Reference to the handler that encapsulates the garage logic.
        /// </summary>
        private readonly GarageHandler _handler;

        /// <summary>
        /// Creates a new ConsoleUI instance using the given handler.
        /// </summary>
        /// <param name="handler">Handler responsible for garage operations.</param>
        public ConsoleUI(GarageHandler handler)
        {
            _handler = handler;
        }

        /// <summary>
        /// Starts the main loop of the application:
        /// displays the main menu, reads user choices and dispatches to handlers
        /// until the user chooses to exit.
        /// </summary>
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
                        HandleShowVehicleStats();
                        break;

                    case "4":
                        HandleAddVehicle();
                        break;

                    case "5":
                        HandleRemoveVehicle();
                        break;

                    case "6":
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

        /// <summary>
        /// Shows a summary of the number of vehicles per type
        /// and the total number of vehicles in the garage.
        /// </summary>
        private void HandleShowVehicleStats()
        {
            Console.Clear();
            ShowHeader();

            if (!_handler.HasGarage)
            {
                ShowError("Inget garage finns. Skapa ett garage först.");
                Pause();
                return;
            }

            var vehicleCount = _handler.GetVehicleTypeCounts();

            Console.WriteLine("=== Fordon per typ ===\n");

            int totalNumber = 0;

            // Force a specific order when printing stats
            string[] order = { "Car", "Motorcycle", "Boat", "Bus", "Airplane" };

            foreach (var typeName in order)
            {
                vehicleCount.TryGetValue(typeName, out int countForType);
                totalNumber += countForType;

                string label = typeName switch
                {
                    "Car" => "Bilar",
                    "Motorcycle" => "Motorcyklar",
                    "Boat" => "Båtar",
                    "Bus" => "Bussar",
                    "Airplane" => "Flygplan",
                    _ => typeName
                };

                Console.WriteLine($"{label}: {countForType}");
            }

            Console.WriteLine($"\nTotalt antal fordon: {totalNumber}");

            Pause();
        }

        /// <summary>
        /// Top-level menu for adding a vehicle.
        /// Lets the user choose a vehicle type and delegates to the appropriate handler.
        /// </summary>
        private void HandleAddVehicle()
        {
            Header(" Lägg till fordon ");
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
                case "3":
                    HandleAddMotorcycle();
                    break;
                case "4":
                    HandleAddBus();
                    break;
                case "5":
                    HandleAddAirplane();
                    break;
                // osv
                default:
                    ShowError("Ogiltig fordonstyp.");
                    Pause();
                    break;
            }
        }

        /// <summary>
        /// Writes the main menu options to the console.
        /// </summary>
        private void ShowMenu()
        {
            Console.WriteLine("1) Skapa nytt garage");
            Console.WriteLine("2) Lista fordon");
            Console.WriteLine("3) Visa fordonsstatistik");
            Console.WriteLine("4) Lägg till fordon");
            Console.WriteLine("5) Ta bort fordon");
            Console.WriteLine("6) Sök efter fordon");
            Console.WriteLine("0) Avsluta");
            Console.WriteLine();
        }

        /// <summary>
        /// Sub-menu for searching vehicles.
        /// Allows the user to choose between quick search and advanced search.
        /// </summary>
        private void HandleSearchMenu()
        {
            while (true)
            {
                Header(" Sök fordon ");

                if (!_handler.HasGarage)
                {
                    ShowError("Inget garage finns. Skapa ett garage först.");
                    Pause();
                    return;
                }

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
                        // Back to main menu
                        return;

                    default:
                        ShowError("Ogiltigt menyval. Försök igen.");
                        Pause();
                        break;
                }
            }
        }

        /// <summary>
        /// Quick search handler: finds a single vehicle by registration number.
        /// </summary>
        private void HandleQuickSearchByRegNr()
        {
            Header("Snabbsök – registreringsnummer");
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

        /// <summary>
        /// Advanced search handler:
        /// reads optional filters (type, reg.nr, color, wheels, model)
        /// and forwards them to the handler.
        /// </summary>
        private void HandleAdvancedSearch()
        {
            Header(" Avancerad sökning ");
            Console.WriteLine("Lämna fält tomma om du inte vill filtrera på dem.\n");

            // Type of vehicle
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

        /// <summary>
        /// Creates a new garage and optionally seeds it with sample vehicles.
        /// </summary>
        private void HandleCreateGarage()
        {
            Header(" Skapa nytt garage ");
            int capacity = ReadInt("Ange garagekapacitet (antal platser): ");
            _handler.CreateGarage(capacity);
            Console.WriteLine($"Garage skapat för {capacity} platser");

            Console.Write("Vill du fylla garaget med exempel-fordon? (J/N): ");
            string? answer = Console.ReadLine()?.Trim().ToUpperInvariant();

            if (answer == "J")
            {
                _handler.SeedGarage();
                Console.WriteLine("\nGaraget har fyllts med exempel-fordon.");
            }
            else
            {
                Console.WriteLine("\nGaraget är tomt. Du kan lägga till fordon via menyn.");
            }

            Pause();
        }

        /// <summary>
        /// Helper method to read the base properties that all vehicles share:
        /// registration number, color, number of wheels and model.
        /// </summary>
        private (string regNr, string color, int wheels, string model) ReadBaseVehicleInfo(string title)
        {
            Header(title);

            string regNr = ReadString("Registreringsnummer: ");
            string color = ReadString("Färg: ");
            int wheels = ReadInt("Antal hjul: ");
            string model = ReadString("Modell: ");

            return (regNr, color, wheels, model);
        }

        /// <summary>
        /// Handles user input for creating and adding a car.
        /// </summary>
        private void HandleAddCar()
        {
            var (regNr, color, wheels, model) = ReadBaseVehicleInfo(" Lägg till bil ");

            string fueltype = ReadString("Bränsletyp: ");

            bool success = _handler.AddCar(regNr, color, wheels, model, fueltype);

            if (success)
                Console.WriteLine("Bilen lades till i garaget.");
            else
                ShowError("Kunde inte lägga till bil. " +
                          "Kontrollera att garaget inte är fullt " +
                          "och att registreringsnumret inte redan används.");

            Pause();
        }

        /// <summary>
        /// Handles user input for creating and adding a motorcycle.
        /// </summary>
        private void HandleAddMotorcycle()
        {
            var (regNr, color, wheels, model) = ReadBaseVehicleInfo(" Lägg till motorcykel ");

            int cylinderVolume = ReadInt("Cylindervolym: ");

            bool success = _handler.AddMotorcycle(regNr, color, wheels, model, cylinderVolume);
            if (success)
                Console.WriteLine("Motorcykeln lades till i garaget.");
            else
                ShowError("Kunde inte lägga till motorcykel. " +
                          "Kontrollera att garaget inte är fullt " +
                          "och att registreringsnumret inte redan används.");

            Pause();
        }

        /// <summary>
        /// Handles user input for creating and adding a bus.
        /// </summary>
        private void HandleAddBus()
        {
            var (regNr, color, wheels, model) = ReadBaseVehicleInfo(" Lägg till buss ");

            int numberOfSeats = ReadInt("Antal sittplatser: ");

            bool success = _handler.AddBus(regNr, color, wheels, model, numberOfSeats);

            if (success)
                Console.WriteLine("Bussen lades till i garaget.");
            else
                ShowError("Kunde inte lägga till buss. " +
                          "Kontrollera att garaget inte är fullt " +
                          "och att registreringsnumret inte redan används.");

            Pause();
        }

        /// <summary>
        /// Handles user input for creating and adding an airplane.
        /// </summary>
        private void HandleAddAirplane()
        {
            var (regNr, color, wheels, model) = ReadBaseVehicleInfo(" Lägg till flygplan ");

            int numberOfEngines = ReadInt("Antal motorer: ");

            bool success = _handler.AddAirplane(regNr, color, wheels, model, numberOfEngines);

            if (success)
                Console.WriteLine("Flygplanet lades till i garaget.");
            else
                ShowError("Kunde inte lägga till flygplan. " +
                          "Kontrollera att garaget inte är fullt " +
                          "och att registreringsnumret inte redan används.");

            Pause();
        }

        /// <summary>
        /// Handles user input for creating and adding a boat.
        /// </summary>
        private void HandleAddBoat()
        {
            var (regNr, color, wheels, model) = ReadBaseVehicleInfo(" Lägg till båt ");

            int length = ReadInt("Längd (fot): ");

            bool success = _handler.AddBoat(regNr, color, wheels, model, length);

            if (success)
                Console.WriteLine("Båten lades till i garaget.");
            else
                ShowError("Kunde inte lägga till båt. " +
                          "Kontrollera att garaget inte är fullt " +
                          "och att registreringsnumret inte redan används.");

            Pause();
        }

        /// <summary>
        /// Handles removal of a vehicle based on registration number.
        /// </summary>
        private void HandleRemoveVehicle()
        {
            Header(" Ta bort fordon ");

            if (!_handler.HasGarage)
            {
                ShowError("Inget garage finns. Skapa ett garage först.");
                Pause();
                return;
            }

            string regNr = ReadString("Registreringsnummer: ");
            bool success = _handler.RemoveVehicle(regNr);

            if (success)
                Console.WriteLine("Fordonet togs bort från garaget.");
            else
                ShowError("Kunde inte ta bort fordon");

            Pause();
        }

        /// <summary>
        /// Lists all vehicles currently parked in the garage.
        /// </summary>
        private void HandleListVehicles()
        {
            Header(" Lista fordon ");

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
                Console.WriteLine(v);   // Calls Vehicle.ToString()
            }

            if (!any)
            {
                Console.WriteLine("Garaget är tomt.");
            }

            Pause();
        }

        /// <summary>
        /// Common header helper: clears the console, shows the app header
        /// and prints a section title.
        /// </summary>
        private void Header(string message)
        {
            Console.Clear();
            ShowHeader();
            Console.WriteLine($"=== {message} ===\n");
            Console.WriteLine();
        }

        /// <summary>
        /// Writes the static application header / title.
        /// </summary>
        private void ShowHeader()
        {
            Console.WriteLine("===================================");
            Console.WriteLine("          Garage 1.0");
            Console.WriteLine("===================================");
            Console.WriteLine();
        }

        /// <summary>
        /// Reads an integer value from the user, repeating the prompt
        /// until a valid integer is entered.
        /// </summary>
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

        /// <summary>
        /// Reads a non-empty string from the user, trimming whitespace.
        /// Repeats the prompt until a non-empty value is entered.
        /// </summary>
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

        /// <summary>
        /// Shows an error message in red text.
        /// </summary>
        private void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Simple "press Enter to continue" pause helper.
        /// </summary>
        private void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Tryck Enter för att fortsätta...");
            Console.ReadLine();
        }
    }
}
