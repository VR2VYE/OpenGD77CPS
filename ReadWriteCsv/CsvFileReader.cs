using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ReadWriteCsv
{
	public class CsvFileReader : StreamReader
	{
        public CsvFileReader(Stream stream) : base(stream)
		{
		}

        public CsvFileReader(string filename) : base(filename)
		{
		}

		public CsvFileReader(string path, Encoding encoding) : base( path,  encoding)
		{
		}

		public bool ReadRow(CsvRow row)
		{
			row.LineText = this.ReadLine();
			if (string.IsNullOrEmpty(row.LineText))
			{
				return false;
			}
			int i = 0;
			int num = 0;
			while (i < row.LineText.Length)
			{
				string text;
				if (row.LineText[i] == '"')
				{
					i++;
					int num2 = i;
					for (; i < row.LineText.Length; i++)
					{
						if (row.LineText[i] != '"')
						{
							continue;
						}
						i++;
						if (i < row.LineText.Length && row.LineText[i] == '"')
						{
							continue;
						}
						i--;
						break;
					}
					text = row.LineText.Substring(num2, i - num2);
					text = text.Replace("\"\"", "\"");
				}
				else
				{
					int num3 = i;
					for (; i < row.LineText.Length && row.LineText[i] != ','; i++)
					{
					}
					text = row.LineText.Substring(num3, i - num3);
				}
				if (num < row.Count)
				{
					((List<string>)row)[num] = text;
				}
				else
				{
					row.Add(text);
				}
				num++;
				for (; i < row.LineText.Length && row.LineText[i] != ','; i++)
				{
				}
				if (i < row.LineText.Length)
				{
					i++;
				}
			}
			while (row.Count > num)
			{
				row.RemoveAt(num);
			}
			return row.Count > 0;
		}
	}
}
