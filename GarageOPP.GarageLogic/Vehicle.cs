using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace GarageOPP.GarageLogic
{
	public enum eCarColor
	{
		Yellow,
		Black,
		White,
		Silver
	}
	public enum eLicenseType
	{
		A,
		A2,
		AB,
		B2
	}

	public abstract class Vehicle
	{
		private readonly string r_ModelName;
		private readonly string r_LicenseId;
		protected List<Wheel> m_Wheels = new List<Wheel>();
		protected IEnergySource m_EnergySource;
		private float m_percentageRemainingEnergy;

		public Vehicle(string i_LicenseId, string i_ModelName)
		{
			this.r_LicenseId = i_LicenseId;
			this.r_ModelName = i_ModelName;
		}
		public string ModelName
		{
			get { return r_ModelName; }
		}
		public string LicenseId
		{
			get { return r_LicenseId; }
		}
		public IEnergySource EnergySource
		{
			get { return m_EnergySource; }
			set { m_EnergySource = value; }
		}
	
		public float PercentageRemainingEnergy
		{
			get { return m_percentageRemainingEnergy; }
			set {
				if (value < 0 || value > 100)
				{
					throw new ArgumentOutOfRangeException("Percentage must be between 0 and 100");
				}
				m_percentageRemainingEnergy = value;
				this.EnergySource.AddEnergy(value * this.EnergySource.GetMaxCapacity() / 100f);
			}
		}
		
		public List<Wheel> Wheels
		{
			get { return m_Wheels; }
			set { m_Wheels = value; }
		}
		protected abstract void AddWheels();

		public virtual void UpdateVehicle(float i_EnergyPercentage, string i_TireModel, float i_CurrentAirPressure, string i_additionalFeature1, string i_additionalFeature2)
		{
			this.PercentageRemainingEnergy = i_EnergyPercentage;
			foreach (Wheel wheel in m_Wheels)
			{
				wheel.CurrentAirPressure = i_CurrentAirPressure;
				wheel.ManufacturerName = i_TireModel;
			}
		}
	}
}
