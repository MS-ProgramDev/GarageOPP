using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageOPP.GarageLogic
{
	internal class FuelCar : Car
	{
		private const float k_MaxLitersFuel = 48f;
		private const eTypeFuel k_FuelType = eTypeFuel.Octan95;
		public FuelCar(string i_LicenseId, string i_ModelName)
			: base(i_LicenseId, i_ModelName, new FuelTank(k_FuelType, k_MaxLitersFuel))
		{

		}
		public float CurrentFuelAmount
		{
			get
			{
				FuelTank tank = m_EnergySource as FuelTank;

				if (tank == null)
				{
					throw new FormatException("Energy source is not a fuel tank.");
				}

				return tank.GetCurrentAmount();
			}
		}

		public float MaxFuelCapacity
		{
			get
			{
				FuelTank tank = m_EnergySource as FuelTank;

				if (tank == null)
				{
					throw new FormatException("Energy source is not a fuel tank.");
				}

				return tank.GetMaxCapacity();
			}
		}

		public string FuelType
		{
			get
			{
				FuelTank tank = m_EnergySource as FuelTank;

				if (tank == null)
				{
					throw new FormatException("Energy source is not a fuel tank.");
				}

				return tank.GetEnergyType();
			}
		}

		public void Refuel(float i_AmountToAdd, eTypeFuel i_FuelType)
		{
			FuelTank tank = m_EnergySource as FuelTank;

			if (tank == null)
			{
				throw new FormatException("This vehicle cannot be refueled. It is not a fuel-based vehicle.");
			}

			tank.AddEnergy(i_AmountToAdd);
			this.PercentageRemainingEnergy = (tank.GetCurrentAmount() / tank.GetMaxCapacity()) * 100f;
		}


	}
}
