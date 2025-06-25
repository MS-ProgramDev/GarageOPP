using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageOPP.GarageLogic
{
	internal class ValueRangeException : Exception
	{
		float MaxValue { get;}
		float MinValue { get;}
		public ValueRangeException(float i_MinValue, float i_MaxValue, string i_Message = null)
		 : base(i_Message ?? $"Value is out of range ({i_MinValue} – {i_MaxValue})")
		{
			MinValue = i_MinValue;
			MaxValue = i_MaxValue;
		}

	}
}
