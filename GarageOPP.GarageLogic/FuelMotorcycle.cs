using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageOPP.GarageLogic
{
	internal class FuelMotorcycle : Motorcycle
	{
		private const float k_MaxLitersFuel = 5.8f;
		private const eTypeFuel k_FuelType = eTypeFuel.Octan98;

		public FuelMotorcycle(string i_ModelName, string i_LicenseId)
			: base(i_ModelName, i_LicenseId, new FuelTank(k_FuelType, k_MaxLitersFuel))
		{}

		public float MaxFuelCapacity
		{
			get
			{
				FuelTank tank = m_EnergySource as FuelTank;

				if (tank == null)
				{
					throw new InvalidOperationException("Energy source is not a fuel tank.");
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
					throw new InvalidOperationException("Energy source is not a fuel tank.");
				}

				return tank.GetEnergyType();
			}
		}
		public void Refuel(float i_AmountToAdd, eTypeFuel i_FuelType)
		{
			FuelTank tank = m_EnergySource as FuelTank;

			if (tank == null)
			{
				throw new InvalidOperationException("This vehicle cannot be refueled. It is not a fuel-based vehicle.");
			}

			tank.AddEnergy(i_AmountToAdd);
			this.PercentageRemainingEnergy = (tank.GetCurrentAmount() / tank.GetMaxCapacity()) * 100f;

		}

	}
}
