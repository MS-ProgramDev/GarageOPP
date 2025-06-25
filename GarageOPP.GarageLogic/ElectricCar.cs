using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageOPP.GarageLogic
{
	internal class ElectricCar : Car
	{
		private const float k_MaxBatteryTime = 4.8f;
		public ElectricCar(string i_LicenseId,  string i_ModelName)
		   : base(i_LicenseId, i_ModelName, new Battery(k_MaxBatteryTime))
		{ }

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
