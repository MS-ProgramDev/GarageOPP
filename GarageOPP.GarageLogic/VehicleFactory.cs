using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageOPP.GarageLogic
{
	public class VehicleFactory
	{
		public static GarageVehicle CreateFromLineInDb(string i_Line)
		{
			string[] partsOfTheLine = i_Line.Split(',');

			if(partsOfTheLine.Length < 9)
			{
				throw new ArgumentException("Input line must contain at least 9 parts: VehicleType, LicenseID, ModelName.", nameof(i_Line));
			}
			string vehicleType = partsOfTheLine[0];
			string licenseId = partsOfTheLine[1];
			string modelName = partsOfTheLine[2];
			float energyPercentage = float.Parse(partsOfTheLine[3]);
			string tierModel = partsOfTheLine[4];
			float currentAirPressure = float.Parse(partsOfTheLine[5]);
			string ownerName = partsOfTheLine[6];
			string ownerPhone = partsOfTheLine[7];

			// NOTE:
			// This output is for demo/testing purposes only,
			// to let the user see which vehicles were loaded from Vehicles.db.
			// In a real-world scenario, data loading should be silent,
			// and presentation logic should be handled separately.
			Console.WriteLine($"licenseId: {licenseId} modelName: {modelName} from VehicleFactory.cs ");
			Vehicle vehicle = VehicleCreator.CreateVehicle(vehicleType, licenseId, modelName);

			if (vehicle is Car car)
			{
				string carColor = partsOfTheLine[8];
				string numberOfDoors = partsOfTheLine[9];
				car.UpdateVehicle(energyPercentage, tierModel, currentAirPressure, carColor, numberOfDoors);
			}
			else if (vehicle is Motorcycle motorcycle)
			{
				string licenseType = partsOfTheLine[8];
				string engineVolume = partsOfTheLine[9];
				motorcycle.UpdateVehicle(energyPercentage, tierModel, currentAirPressure, licenseType, engineVolume);
			}
			else if (vehicle is Truck truck)
			{
				string carriesHazardous = partsOfTheLine[8];
				string cargoVolume = partsOfTheLine[9];
				truck.UpdateVehicle(energyPercentage, tierModel, currentAirPressure, carriesHazardous, cargoVolume);
			}
			else
			{
				throw new ArgumentException("Unsupported vehicle type");
			}
			return new GarageVehicle(vehicle, ownerName, ownerPhone);



		}
	}
}
