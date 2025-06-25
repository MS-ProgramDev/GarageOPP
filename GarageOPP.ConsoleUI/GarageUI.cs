using GarageOPP.GarageLogic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.ConstrainedExecution;

namespace GarageOPP.ConsoleUI
{
	public class GarageUI
	{
		private readonly GarageService r_GarageService = new GarageService();


		public void Run()
		{
			bool exit = false;
			while (!exit)
			{
				showMainMenu();
				Console.Write("Your choice is: ");
				string choice = Console.ReadLine();

				try
				{
					switch (choice)
					{
						case "1":
							loadVehiclesFromDb();
							break;
						case "2":
							insertNewVehicle();
							break;
						case "3":
							showNumberLicense();
							break;
						case "4":
							changeVehicleStatus();
							break;
						case "5":
							inflateWheelsToMax();
							break;
						case "6":
							refuelVehicle();
							break;
						case "7":
							rechargeVehicle();
							break;
						case "8":
							showVehicleDetails();
							break;
						case "9":
							exit = true;
							continue;
						default:
							Console.WriteLine("Invalid option");
							break;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error: " + ex.Message);
				}
				Console.WriteLine();
				Console.WriteLine("Press Enter to continue");
				Console.ReadLine();
			}
		}




		private void showMainMenu()
		{
			Console.Clear();
			Console.WriteLine("Welcome to the main menu!");
			Console.WriteLine("Please press the number of the option you want to choose");
			Console.WriteLine("1. Loading vehicle from Vehicles.db file");
			Console.WriteLine("2. Insert new vehicle to the garage");
			Console.WriteLine("3. List of number license of the vehicles in the garage(with filter by status)");
			Console.WriteLine("4. Change status of car in the garage");
			Console.WriteLine("5. Inflate tires to maximum");
			Console.WriteLine("6. Refuel vehicle");
			Console.WriteLine("7. Charge vehicle");
			Console.WriteLine("8. Show full vehicle details");
			Console.WriteLine("9. Exit");
			Console.WriteLine();
		}

		private void loadVehiclesFromDb()
		{
			string fileName = "Vehicles.db";

			if (!File.Exists(fileName))
			{
				Console.WriteLine("File not exist");
				return;
			}

			string[] linesInDb;

			try
			{
				linesInDb = File.ReadAllLines(fileName);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Failed to read from file: " + ex.Message);
				return;
			}

			foreach (string line in linesInDb)
			{
				if (line.StartsWith("*"))
					break;

				try
					{
					GarageVehicle garageVehicle = VehicleFactory.CreateFromLineInDb(line);
					if (!r_GarageService.VehicleExists(garageVehicle.Vehicle.LicenseId))
					{
						r_GarageService.AddNewVehicle(garageVehicle);					
					}
					else
					{
						Console.WriteLine("Vehicle with the same license id already exists");
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Failed to create vehicle from line: " + ex.Message);
				}
			}
		}


		private string getLicenseIdFromUser()
		{
			Console.Write("Enter license ID: ");
			return Console.ReadLine();
		}

		private void insertNewVehicle()
		{
			string licenseId = getLicenseIdFromUser();

			if (handleExistingVehicle(licenseId))
			{
				return; 
			}
			string vehicleType = chooseVehicleType();

			Console.WriteLine("Enter model name: ");
			string modelName = Console.ReadLine();

			Vehicle vehicle = VehicleCreator.CreateVehicle(vehicleType, licenseId, modelName);
			
			updateVehicleData(vehicle);

			Console.WriteLine("Enter owner name: ");
			string ownerName = Console.ReadLine();

			Console.WriteLine("Enter owner phone: ");
			string ownerPhone = Console.ReadLine();

			r_GarageService.AddNewVehicle(new GarageVehicle(vehicle, ownerName, ownerPhone));
			Console.WriteLine("Vehicle added to garage.");

		}
		private void updateVehicleData(Vehicle i_Vehicle)
		{
			float energyPercentage = getEnergyPercentage();
			string tireModel = getTireManufacturer();
			float airPressure = getAirPressure();
			string extraParamsFirst;
			string extraParamsSecond;
			if (i_Vehicle is Car car)
			{
				extraParamsFirst = chooseCarColor().ToString();
				extraParamsSecond = getNumberOfDoors().ToString();
			}
			else if (i_Vehicle is Motorcycle motorcyle)
			{
				extraParamsFirst = chooseMotorcycleLicenseType().ToString();
				extraParamsSecond = getEngineVolume().ToString();
			}
			else if (i_Vehicle is Truck truck)
			{
				extraParamsFirst = isHazardousMaterial().ToString();
				extraParamsSecond = getCargoVolume().ToString();
			}
			else
			{
				throw new ArgumentException("Unknown vehicle type.");
			}

			i_Vehicle.UpdateVehicle(energyPercentage, tireModel, airPressure, extraParamsFirst, extraParamsSecond);
			}
		private void showNumberLicense()
		{
			Console.WriteLine("Do you want to filter by status? Please Enter the num of your choice.");
			Console.WriteLine("1. yes");
			Console.WriteLine("2. no");

			int choice;
			while (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2))
			{
				Console.Write("Invalid input.  Enter 1 (yes) or 2 (no): ");
			}
			bool filter = (choice == 1);
			eVehicleStatus? status = null;

			if (filter)
			{
				Console.WriteLine("Choose status to filter:");
				Console.WriteLine("1. InRepair");
				Console.WriteLine("2. Repaired");
				Console.WriteLine("3. Paid");
				int statusChoice;
				while (!int.TryParse(Console.ReadLine(), out statusChoice) || statusChoice < 1 || statusChoice > 3)
				{
					Console.WriteLine("Invalid input. Enter 1, 2 or 3: ");
				}
				status = (eVehicleStatus)(statusChoice - 1);
			}
			List<string> plates = r_GarageService.GetLicensePlates(status);

			if (plates.Count == 0)
			{
				Console.WriteLine("No vehicles found with the specified filter.");
				return;
			}

			Console.WriteLine("License ID:");

			foreach (string plate in plates)
			{
				Console.WriteLine(plate);
			}
		}



		private void changeVehicleStatus()
		{
			Console.WriteLine("Enter License ID: ");
			string licenseId = Console.ReadLine();


			if (!r_GarageService.VehicleExists(licenseId))
			{
				Console.WriteLine("Vehicle not found in garage.");
				return;
			}

			Console.WriteLine("Enter the number of option of the new vehicle status: ");
			Console.WriteLine("1. InRepair");
			Console.WriteLine("2. Repaired");
			Console.WriteLine("3. Paid");

			int statusOption;
			while (!int.TryParse(Console.ReadLine(), out statusOption) || statusOption < 1 || statusOption > 3)
			{
				Console.WriteLine("Invalid input. Please enter 1, 2 or 3: ");
			}

			eVehicleStatus newStatus = (eVehicleStatus)(statusOption - 1);
			r_GarageService.SetVehicleStatus(licenseId, newStatus);
			Console.WriteLine("Status updated");
		}

		private void inflateWheelsToMax()
		{
			string licenseId = getLicenseIdFromUser();
			
			if (!r_GarageService.VehicleExists(licenseId))
			{
				Console.WriteLine("Vehicle not found in garage.");
				return;                 
			}

			r_GarageService.InflateWheelsToMax(licenseId);
			Console.WriteLine("Wheels inflated to maximum.");

		}

		private void refuelVehicle()
		{
			string license = getLicenseIdFromUser();

			if (!r_GarageService.VehicleExists(license))
			{
				Console.WriteLine("Vehicle not found in garage.");
				return;
			}
			eTypeFuel fuel = chooseFuelType();
			float liters = getFuelAmount();
			r_GarageService.RefuelVehicle(license, fuel, liters);
			Console.WriteLine("Refueled.");
		}

		private void rechargeVehicle()
		{
			string license = getLicenseIdFromUser();

			if (!r_GarageService.VehicleExists(license))
			{
				Console.WriteLine("Vehicle not found in garage.");
				return;
			}

			float minutes = getBatteryMintuesToAdd();
			r_GarageService.RechargeVehicle(license, minutes);
			Console.WriteLine("Vehicle charged.");
		}

		private void showVehicleDetails()
		{
			string license = getLicenseIdFromUser();
			Console.WriteLine(r_GarageService.GetFullVehicleDetails(license));
		}
		private eTypeFuel chooseFuelType()
		{
			Console.WriteLine("Choose fuel type:");
			Console.WriteLine("1. Soler");
			Console.WriteLine("2. Octan95");
			Console.WriteLine("3. Octan96");
			Console.WriteLine("4. Octan98");

			int choice;
			while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
			{
				Console.WriteLine("Invalid input. Please enter a number between 1 and 4: ");
			}

			return (eTypeFuel)(choice - 1);
		}

		private float getFuelAmount()
		{
			float liters;
			Console.WriteLine("Liters to add: ");
			
			while (!float.TryParse(Console.ReadLine(), out liters) || liters < 0)
			{
				Console.WriteLine("Invalid input. Please enter a positive number:");
			}
			return liters;
		}

		private string chooseVehicleType()
		{
			Console.WriteLine("Choose vehicle type:");
			Console.WriteLine("1. Fuel Car");
			Console.WriteLine("2. Electric Car");
			Console.WriteLine("3. Fuel Motorcycle");
			Console.WriteLine("4. Electric Motorcycle");
			Console.WriteLine("5. Truck");

			int choice;
			while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 5)
			{
				Console.Write("Invalid input. Please enter a number 1-5: ");
			}
			string vehicleType;

			switch (choice)
			{
				case 1:
					vehicleType = "FuelCar";
					break;
				case 2:
					vehicleType = "ElectricCar";
					break;
				case 3:
					vehicleType = "FuelMotorcycle";
					break;
				case 4:
					vehicleType = "ElectricMotorcycle";
					break;
				case 5:
					vehicleType = "Truck";
					break;
				default:
					throw new FormatException("Invalid vehicle type.");
			}

			return vehicleType;
		}

		private eCarColor chooseCarColor()
		{
			Console.WriteLine("Choose car color:");
			Console.WriteLine("1. Red");
			Console.WriteLine("2. Blue");
			Console.WriteLine("3. Black");
			Console.WriteLine("4. Silver");

			int choice;
			while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
			{
				Console.WriteLine("Invalid input. Please enter a number between 1 and 4:");
			}

			return (eCarColor)(choice - 1);
		}
		private int getNumberOfDoors()
		{
			Console.WriteLine("Enter number of doors (2–5):");

			int doors;
			while (!int.TryParse(Console.ReadLine(), out doors) || doors < 2 || doors > 5)
			{
				Console.WriteLine("Invalid input. Please enter a number between 2 and 5:");
			}

			return doors;
		}

		private eLicenseType chooseMotorcycleLicenseType()
		{
			Console.WriteLine("Choose license type:");
			Console.WriteLine("1. A");
			Console.WriteLine("2. A1");
			Console.WriteLine("3. A2");
			Console.WriteLine("4. B1");

			int choice;
			while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
			{
				Console.WriteLine("Invalid input. Please enter a number between 1 and 4:");
			}

			return (eLicenseType)(choice - 1);
		}

		private int getEngineVolume()
		{
			Console.WriteLine("Enter engine volume: ");

			int volume;
			while (!int.TryParse(Console.ReadLine(), out volume) || volume <= 0)
			{
				Console.WriteLine("Invalid input. Please enter a positive number:");
			}

			return volume;
		}
		//need to change
		private bool isHazardousMaterial()
		{
			Console.WriteLine("Is the truck carrying hazardous material?");
			Console.WriteLine("1. Yes");
			Console.WriteLine("2. No");
			int choice;
			while (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2))
			{
				Console.WriteLine("Invalid input. Please enter 1 for Yes or 2 for No:");
			}

			return choice == 1;
		}
		private float getCargoVolume()
		{
			Console.WriteLine("Enter cargo volume (in liters):");

			float volume;
			while (!float.TryParse(Console.ReadLine(), out volume) || volume < 0)
			{
				Console.WriteLine("Invalid input. Please enter a non-negative number:");
			}

			return volume;
		}

		private float getBatteryMintuesToAdd()
		{
			Console.WriteLine("Enter number of minutes to charge:");

			float minutes;
			while (!float.TryParse(Console.ReadLine(), out minutes) || minutes < 0)
			{
				Console.WriteLine("Invalid input. Please enter a non-negative number:");
			}

			return minutes/60f;
		}
		private float getEnergyPercentage()
		{
			Console.Write("Enter remaining energy percentage (0-100): ");
			float percentage;
			while (!float.TryParse(Console.ReadLine(), out percentage) ||
				   percentage < 0 || percentage > 100)
			{
				Console.Write("Invalid input. Enter a number between 0 and 100: ");
			}
			return percentage;
		}

		private float getAirPressure()
		{
			Console.Write("Enter current air pressure: ");
			float pressure;
			while (!float.TryParse(Console.ReadLine(), out pressure) || pressure < 0)
			{
				Console.Write("Invalid input. Enter a positive number: ");
			}
			return pressure;
		}
		private string getTireManufacturer()
		{
			Console.Write("Enter tire manufacturer: ");
			return Console.ReadLine();
		}

		private bool handleExistingVehicle(string i_LicenseId)
		{
			if (r_GarageService.VehicleExists(i_LicenseId))
			{
				r_GarageService.SetVehicleStatus(i_LicenseId, eVehicleStatus.InRepair);
				Console.WriteLine("Vehicle already exists. Status set to 'InRepair'.");
				return true;           
			}
			return false;              
		}
	}
}
