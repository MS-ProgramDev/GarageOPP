using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageOPP.GarageLogic
{
	public interface IEnergySource
	{
		float GetCurrentAmount();
		float GetMaxCapacity();
		void AddEnergy(float i_AmountToAdd);
		string GetEnergyType();
	}
}
