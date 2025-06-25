using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageOPP.GarageLogic
{
		public class Truck : Vehicle
		{
			private const int k_NumberOfWheels = 12;
			private const float k_MaxAirPressure = 27f;
			private const float k_MaxLitersFuel = 135f;
			private const eTypeFuel k_FuelType = eTypeFuel.Soler;
			private float m_CargoVolume;
			private bool m_CarriesHazardousMaterials;

			public Truck(string i_ModelName, string i_LicenseId)
			   : base(i_ModelName, i_LicenseId)
			{
				this.EnergySource = new FuelTank(k_FuelType, k_MaxLitersFuel);
				AddWheels();
			}

			public override void UpdateVehicle(float i_EnergyPercentage, string i_TireModel, float i_CurrentAirPressure, string i_CarriesHazardousMaterials, string i_CargoVolume)
			{
				base.UpdateVehicle(i_EnergyPercentage, i_TireModel, i_CurrentAirPressure, i_CarriesHazardousMaterials, i_CargoVolume);

				if (!bool.TryParse(i_CarriesHazardousMaterials, out bool parsedCarriesHazardous))
				{
					throw new ArgumentException("Invalid value for carries hazardous materials.");
				}
				this.CarriesHazardousMaterials = parsedCarriesHazardous;

				if (!float.TryParse(i_CargoVolume, out float parsedCargoVolume))
				{
					throw new ArgumentException("Invalid cargo volume.");
				}
				this.CargoVolume = parsedCargoVolume;
			}

			public float CargoVolume
			{
				get { return m_CargoVolume; }
				set { m_CargoVolume = value; }
			}

			public bool CarriesHazardousMaterials
			{
				get { return m_CarriesHazardousMaterials; }
				set { m_CarriesHazardousMaterials = value; }
			}
			protected override void AddWheels()
			{
				for (int i = 0; i < k_NumberOfWheels; i++)
				{
					m_Wheels.Add(new Wheel("DefaultForTruck", k_MaxAirPressure, k_MaxAirPressure));
				}
			}
		}
		
	};
