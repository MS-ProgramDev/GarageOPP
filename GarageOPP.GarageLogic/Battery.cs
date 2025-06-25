using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageOPP.GarageLogic
{
	internal class Battery : IEnergySource
	{
		private float m_CurrentAmount;
		private float r_MaxCapacity;
		public Battery(float i_MaxCapacity)
		{
			this.m_CurrentAmount = 0;
			this.r_MaxCapacity = i_MaxCapacity;
		}
		public float GetCurrentAmount()
		{
			return m_CurrentAmount;
		}
		public float GetMaxCapacity()
		{
			return r_MaxCapacity;
		}
		public void AddEnergy(float i_AmountToAdd)
		{
			if (i_AmountToAdd < 0)
			{
				throw new ValueRangeException(0, r_MaxCapacity, "Charge amount must be positive.");
			}

			float newAmount = m_CurrentAmount + i_AmountToAdd;
			
			if (newAmount > this.r_MaxCapacity)
			{
				throw new ValueRangeException(0, r_MaxCapacity, "Charging exceeds battery capacity.");
			}

			m_CurrentAmount = newAmount;
		}
		public string GetEnergyType()
		{
			return "Battery";
		}
	
	}
}
