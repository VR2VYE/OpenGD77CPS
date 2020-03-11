using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMR
{
	class DmrMarcData
	{
		public List<DmrMarcDataDataItem> users {get; set; }
		public override string ToString()
		{
			string retVal = String.Empty;
			foreach (DmrMarcDataDataItem i in users)
			{
				retVal += i.ToString() + "\n";
			}
			return retVal;
		}
	}

	public class DmrMarcDataDataItem
	{
		public string Id { get; set; }

		public string Callsign { get; set; }
		public string Name { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string Remarks { get; set; }
		public string ToString()
		{
			return Id + " " + " " + Callsign + " " + Name + " " +  City + " " + State + " " + Country + " " + Remarks;
		}

		public static DmrMarcDataDataItem FromCsv(string csvLine)
		{

			if (csvLine != "")
			{
				string[] values;

				DmrMarcDataDataItem value = new DmrMarcDataDataItem();

				values = csvLine.Split(',');
				value.Id = values[0].Trim('"') ;
				value.Callsign = values[1].Trim('"');
				value.Name = values[2].Trim('"');
				value.City = values[3].Trim('"');
				value.State = values[4].Trim('"');
				value.Country = values[5].Trim('"');
				value.Remarks = values[6].Trim('"');
				return value;
			}
			else
			{
				return null;
			}

		}

	}


}
