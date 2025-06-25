using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageOPP.GarageLogic
{
	public class Wheel
	{
		private readonly float r_MaxAirPressure;
		private string m_ManufacturerName;
		private float m_CurrentAirPressure;

		public Wheel(string i_ManufacturerName, float i_MaxAirPressure, float i_CurrentAirPressure) 
		{
			this.r_MaxAirPressure = i_MaxAirPressure;
			this.m_ManufacturerName = i_ManufacturerName;
			this.m_CurrentAirPressure = i_CurrentAirPressure;
		}
		
		public string ManufacturerName
		{
			get { return m_ManufacturerName; }
			set { m_ManufacturerName = value; }
		}
		public float MaxAirPressure
		{
			get { return r_MaxAirPressure; }
		}
		public float CurrentAirPressure
		{
			get { return m_CurrentAirPressure; }
			set { m_CurrentAirPressure = value; }
		}

		public void TryAddAir(float i_amountAirToAdd)
		{
			if (i_amountAirToAdd < 0)
			{
				throw new ValueRangeException(0, r_MaxAirPressure, "Air amount must be positive.");
			}

			float newAmount = m_CurrentAirPressure + i_amountAirToAdd;
			
			if (newAmount > r_MaxAirPressure)
			{
				throw new ValueRangeException(0, r_MaxAirPressure, "Exceeds max air pressure.");
			}
			m_CurrentAirPressure = newAmount;
		}
	}
	
}
