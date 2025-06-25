using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageOPP.GarageLogic
{
	public enum eTypeFuel
	{
		Soler,
		Octan95,
		Octan96,
		Octan98
	}
	internal class FuelTank : IEnergySource
	{
		private eTypeFuel m_TypeFuel;
		private float m_CurrentAmountFuel;
		private readonly float r_MaxAmountFuel;

		public FuelTank(eTypeFuel i_TypeFuel, float i_MaxAmountFuel)
		{
			this.m_TypeFuel = i_TypeFuel;
			this.m_CurrentAmountFuel = 0;
			this.r_MaxAmountFuel = i_MaxAmountFuel;
		}
		public float GetCurrentAmount()
		{
			return m_CurrentAmountFuel;
		}
		public float GetMaxCapacity()
		{
			return r_MaxAmountFuel;
		}
		public void AddEnergy(float i_AmountToAdd)
		{
			if (i_AmountToAdd < 0)
			{
				throw new ValueRangeException(0, r_MaxAmountFuel, "Fuel amount must be positive.");
			}

			float newAmount = m_CurrentAmountFuel + i_AmountToAdd;

			if (newAmount > this.r_MaxAmountFuel)
			{
				throw new ValueRangeException(0, r_MaxAmountFuel, "Fuel amount exceeds capacity.");
			}

			m_CurrentAmountFuel = newAmount;
		}
		public string GetEnergyType()
		{
			return m_TypeFuel.ToString();
		}
	}

}
