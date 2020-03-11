using System.IO;
using System.Text;

namespace ReadWriteCsv
{
	public class CsvFileWriter : StreamWriter
	{

		public CsvFileWriter(Stream stream) : base(stream)
		{
			
		}

		public CsvFileWriter(string filename):base (filename)
		{
			
		}

		public CsvFileWriter(Stream stream, Encoding encoding):base(stream, encoding)
		{
			
		}

		public CsvFileWriter(string path, bool append, Encoding encoding) : base(path, append, encoding)
		{
			
			//base._002Ector(path, append, encoding);
		}

		public void WriteRow(CsvRow row)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (string item in row)
			{
				if (!flag)
				{
					stringBuilder.Append(',');
				}
				if (item.IndexOfAny(new char[2]
				{
					'"',
					','
				}) != -1)
				{
					stringBuilder.AppendFormat("\"{0}\"", item.Replace("\"", "\"\""));
				}
				else
				{
					stringBuilder.Append(item);
				}
				flag = false;
			}
			row.LineText = stringBuilder.ToString();
			this.WriteLine(row.LineText);
		}
	}
}
