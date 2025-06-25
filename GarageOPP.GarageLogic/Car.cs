using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageOPP.GarageLogic
{
	public class Car : Vehicle
	{
		protected const int k_NumberOfWheels = 5;
		protected const float k_MaxAirPressure = 32f;
		protected const eCarColor k_DefaultColor = eCarColor.Black;
		protected const int k_DefaultNumberOfDoors = 4;

		private int m_NumberOfDoors;
		private eCarColor m_CarColor;

		
		public Car(string i_LicenseId, string i_ModelName, IEnergySource i_EnergySource, eCarColor i_CarColor = k_DefaultColor, int i_NumberOfDoors = k_DefaultNumberOfDoors)
			: base(i_LicenseId, i_ModelName)
		{
			if (i_NumberOfDoors < 2 || i_NumberOfDoors > 5)
			{
				throw new ValueRangeException(2, 5, "Car must have between 2 and 5 doors.");
			}
			this.m_EnergySource = i_EnergySource;
			this.m_CarColor = i_CarColor;
			this.m_NumberOfDoors = i_NumberOfDoors;
			AddWheels();
		}

		public override void UpdateVehicle(float i_EnergyPercentage, string i_TireModel, float i_CurrentAirPressure, string i_CarColor, string i_NumberOfDoors)
		{
			base.UpdateVehicle(i_EnergyPercentage, i_TireModel, i_CurrentAirPressure, i_CarColor, i_NumberOfDoors);
			
			if (!Enum.TryParse<eCarColor>(i_CarColor, true, out eCarColor parsedColor))
			{
				throw new ArgumentException("Invalid car color");
			}
			this.CarColor = parsedColor;

			if (!int.TryParse(i_NumberOfDoors, out int parsedDoors))
			{
				throw new ArgumentException($"Invalid number of doors: '{i_NumberOfDoors}'. Must be an integer.");
			}
			this.NumberOfDoors = parsedDoors;
		}

		public int NumberOfDoors
		{	
			get { return m_NumberOfDoors; }
			set { m_NumberOfDoors = value; }
		}

		public eCarColor CarColor
		{
			get { return m_CarColor; }
			set { m_CarColor = value; }
		}
		protected override void AddWheels()
		{
			for (int i = 0; i < k_NumberOfWheels; i++)
			{
				m_Wheels.Add(new Wheel("DefaultForCar", k_MaxAirPressure, k_MaxAirPressure));
			}
		}
	}
}
