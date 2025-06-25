using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageOPP.GarageLogic
{
	internal class ElectricMotorcycle : Motorcycle
	{
		private const float k_MaxBatteryTime = 3.2f;

		public ElectricMotorcycle(string i_ModelName, string i_LicenseId)
			: base(i_ModelName, i_LicenseId, new Battery(k_MaxBatteryTime))
		{
		}

		public float CurrentBatteryHours
		{
			get
			{
				Battery battery = m_EnergySource as Battery;

				if (battery == null)
				{
					throw new FormatException("Energy source is not a battery.");
				}
				return battery.GetCurrentAmount();
			}

		}

		public float MaxBatteryHours
		{
			get
			{
				Battery battery = m_EnergySource as Battery;

				if (battery == null)
				{
					throw new FormatException("Energy source is not a battery.");
				}
				return battery.GetMaxCapacity();
			}
		}
		public void Charge(float i_MinutesToAdd)
		{
			Battery battery = m_EnergySource as Battery;
			if (battery == null)
			{
				throw new FormatException("This vehicle cannot be charged. It is not electric.");
			}
			battery.AddEnergy(i_MinutesToAdd);
		}
	}
}
