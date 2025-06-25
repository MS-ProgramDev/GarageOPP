using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace GarageOPP.GarageLogic
{
	public class GarageService
	{
		private readonly Dictionary<string, GarageVehicle> r_GarageVehicles = new Dictionary<string, GarageVehicle>();

		public bool VehicleExists(string i_LicenseId)
		{
			return r_GarageVehicles.ContainsKey(i_LicenseId);
		}

		public GarageVehicle GetVehicle(string i_LicenseId)
		{
			if (!r_GarageVehicles.TryGetValue(i_LicenseId, out GarageVehicle o_vehicle))
			{
				throw new ArgumentException("Vehicle not found.");
			}
			return o_vehicle;
		}

		public void AddNewVehicle(GarageVehicle i_Vehicle)
		{
			if (VehicleExists(i_Vehicle.Vehicle.LicenseId))
			{
				throw new FormatException("Vehicle already exists in the garage.");
			}
			r_GarageVehicles.Add(i_Vehicle.Vehicle.LicenseId, i_Vehicle);
		}

		public void SetVehicleStatus(string i_LicenseId, eVehicleStatus i_NewStatus)
		{
			GetVehicle(i_LicenseId).Status = i_NewStatus;
		}


		public void InsertNewVehicle(string i_VehicleType, string i_LicenseId, string i_ModelName, float i_EnergyPercentage, string i_TireModel,
									 float i_CurrentAirPressure, string i_OwnerName, string i_OwnerPhone, string i_ExtraParamsFirst, string i_ExtraParamsSecond)
		{
			if (VehicleExists(i_LicenseId))
			{
				SetVehicleStatus(i_LicenseId, eVehicleStatus.InRepair);
				return;
			}

			Vehicle vehicle = VehicleCreator.CreateVehicle(i_VehicleType, i_LicenseId, i_ModelName);

			vehicle.UpdateVehicle(i_EnergyPercentage, i_TireModel, i_CurrentAirPressure, i_ExtraParamsFirst, i_ExtraParamsSecond);

			GarageVehicle newGarageVehicle = new GarageVehicle(vehicle, i_OwnerName, i_OwnerPhone);
			AddNewVehicle(newGarageVehicle);
		}





		public List<string> GetLicensePlates(eVehicleStatus? i_Status = null)
		{
			List<string> licensePlates = new List<string>();

			foreach (GarageVehicle garageVehicle in r_GarageVehicles.Values)
			{
				if (!i_Status.HasValue || garageVehicle.Status == i_Status.Value)
				{
					licensePlates.Add(garageVehicle.Vehicle.LicenseId);
				}
			}

			return licensePlates;
		}
		public void InflateWheelsToMax(string i_LicenseId)
		{
			float amountToAddForMaxAir;
			List<Wheel> wheels = GetVehicle(i_LicenseId).Vehicle.Wheels;
			foreach (Wheel wheel in wheels)
			{
				amountToAddForMaxAir = wheel.MaxAirPressure - wheel.CurrentAirPressure;
				wheel.TryAddAir(amountToAddForMaxAir);
			}
		}
		public void RefuelVehicle(string i_LicenseId, eTypeFuel i_FuelType, float i_AmountToAdd)
		{
			IEnergySource source = GetVehicle(i_LicenseId).Vehicle.EnergySource;

			if (source is FuelTank fuelTank)
			{
				if (fuelTank.GetEnergyType() != i_FuelType.ToString())
				{
					throw new ArgumentException("Fuel type mismatch.");
				}

				fuelTank.AddEnergy(i_AmountToAdd);
			}
			else
			{
				throw new FormatException("This vehicle is not fuel based.");
			}
		}
		public void RechargeVehicle(string i_LicenseId, float i_MinutesToCharge)
		{
			IEnergySource source = GetVehicle(i_LicenseId).Vehicle.EnergySource;

			if (source is Battery battery)
			{
				float minutes = i_MinutesToCharge;
				battery.AddEnergy(minutes);
			}
			else
			{
				throw new FormatException("This vehicle is not electric.");
			}
		}


		public string GetFullVehicleDetails(string i_LicenseId)
		{
			GarageVehicle garageVehicle = GetVehicle(i_LicenseId);
			StringBuilder garageVehicleDetails = new StringBuilder();
			string vehicleWheelsManufacturerName = garageVehicle.Vehicle.Wheels[0].ManufacturerName;
			string vehicleWheelsAirPressure = garageVehicle.Vehicle.Wheels[0].CurrentAirPressure.ToString();
			string vehicleWheelsAirMaxPressure = garageVehicle.Vehicle.Wheels[0].MaxAirPressure.ToString();

			garageVehicleDetails.AppendLine($"License ID: {garageVehicle.Vehicle.LicenseId}");
			garageVehicleDetails.AppendLine($"Model Name: {garageVehicle.Vehicle.ModelName}");
			garageVehicleDetails.AppendLine($"Owner: {garageVehicle.OwnerName}, Phone: {garageVehicle.OwnerPhone}");
			garageVehicleDetails.AppendLine($"Status: {garageVehicle.Status}");

			garageVehicleDetails.AppendLine($"Wheels Manufacturer Name: {vehicleWheelsManufacturerName}");
			garageVehicleDetails.AppendLine($"Wheels Air Pressere: {vehicleWheelsAirPressure} / {vehicleWheelsAirMaxPressure}");

			garageVehicleDetails.AppendLine($"Energy Remaining: {garageVehicle.Vehicle.PercentageRemainingEnergy}%");

			if (garageVehicle.Vehicle.EnergySource is FuelTank fuel)
			{
				garageVehicleDetails.AppendLine($"Fuel Type: {fuel.GetEnergyType()}, Amount: {fuel.GetCurrentAmount():F2}L / {fuel.GetMaxCapacity()}L");
			}
			else if (garageVehicle.Vehicle.EnergySource is Battery battery)
			{
				garageVehicleDetails.AppendLine($"Battery: {battery.GetCurrentAmount():F2}h / {battery.GetMaxCapacity()}h");
			}

			if (garageVehicle.Vehicle is Car car)
			{
				garageVehicleDetails.AppendLine($"Car Color: {car.CarColor}");
				garageVehicleDetails.AppendLine($"Number of Doors: {car.NumberOfDoors}");
			}
			else if (garageVehicle.Vehicle is Motorcycle motorcycle)
			{
				garageVehicleDetails.AppendLine($"License Type: {motorcycle.LicenseType}");
				garageVehicleDetails.AppendLine($"Engine Volume: {motorcycle.EngineVolume}");
			}
			else if (garageVehicle.Vehicle is Truck truck)
			{
				garageVehicleDetails.AppendLine($"Carries Hazardous Materials: {truck.CarriesHazardousMaterials}");
				garageVehicleDetails.AppendLine($"Cargo Volume: {truck.CargoVolume}");
			}

			return garageVehicleDetails.ToString();



		}
	}
}
