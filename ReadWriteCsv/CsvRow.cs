using System.Collections.Generic;

namespace ReadWriteCsv
{
	public class CsvRow : List<string>
	{
		public string LineText
		{
			get;
			set;
		}

		public CsvRow()
		{
		}
	}
}
