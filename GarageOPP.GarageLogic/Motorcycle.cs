using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageOPP.GarageLogic
{
	public class Motorcycle : Vehicle
	{
		private const int k_NumberOfWheels = 2;
		private const float k_MaxAirPressure = 30f;
		private eLicenseType m_LicenseType;
		private int m_EngineVolume;

		public Motorcycle(string i_ModelName, string i_LicenseId, IEnergySource i_EnergySource)
			: base(i_ModelName, i_LicenseId)
		{
			this.m_EnergySource = i_EnergySource;
			AddWheels();
		}

		public override void UpdateVehicle(float i_EnergyPercentage, string i_TireModel, float i_CurrentAirPressure, string i_LicenseType, string i_EngineVolume)
		{
			base.UpdateVehicle(i_EnergyPercentage, i_TireModel, i_CurrentAirPressure, i_LicenseType, i_EngineVolume);

			if (!Enum.TryParse<eLicenseType>(i_LicenseType, true, out eLicenseType parsedLicenseType))
			{
				throw new ArgumentException("Invalid license type");
			}
			this.LicenseType = parsedLicenseType;

			if (!int.TryParse(i_EngineVolume, out int parsedEngineVolume))
			{
				throw new ArgumentException("Invalid engine volume");
			}
			this.EngineVolume = parsedEngineVolume;
		}

		public eLicenseType LicenseType
		{ 
			get { return m_LicenseType; }
			set { m_LicenseType = value; }
		}
		public int EngineVolume
		{
			get { return m_EngineVolume; }
			set { m_EngineVolume = value; }
		}

		protected override void AddWheels()
		{
			for (int i = 0; i < k_NumberOfWheels; i++)
			{
				m_Wheels.Add(new Wheel("DefaultForMotorcycle", k_MaxAirPressure, k_MaxAirPressure));
			}
		}
	}
}
