using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageOPP.GarageLogic
{
	public enum eVehicleStatus
	{
		InRepair,
		Repaired,
		Paid
	}
	public class GarageVehicle
	{
		private readonly Vehicle r_Vehicle;
		private string m_OwnerName;
		private string m_OwnerPhone;
		private eVehicleStatus m_Status;
		public GarageVehicle(Vehicle i_Vehicle, string i_OwnerName, string i_OwnerPhone)
		{
			r_Vehicle = i_Vehicle;
			m_OwnerName = i_OwnerName;
			m_OwnerPhone = i_OwnerPhone;
			m_Status = eVehicleStatus.InRepair;
		}
		public Vehicle Vehicle
		{
			get { return r_Vehicle; }
		}
		public string OwnerName
		{
			get { return m_OwnerName; }
			set { m_OwnerName = value; }
		}
		public string OwnerPhone
		{
			get { return m_OwnerPhone; }
			set { m_OwnerPhone = value; }
		}
		public eVehicleStatus Status
		{
			get { return m_Status; }
			set { m_Status = value; }
		}

	}
}
