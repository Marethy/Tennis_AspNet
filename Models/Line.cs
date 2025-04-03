using System.Drawing;

namespace Tennis.Models;

public class Line
{
	public Data data;
	public Options options = new();

	public string type = "line";

	public Line(int SoLuong)
	{
		data = new Data(SoLuong);
	}


	public class Options
	{
		public Plugins plugins = new();

		public class Plugins
		{
			public Title title = new();

			public class Title
			{
				public bool display = true;
				public string text = "hi";
			}
		}
	}

	public class Data
	{
		public Dataset[] datasets;
		public string[] labels;

		public Data(int soLuong)
		{
			//datasets = new Dataset[soLuong];
			//for(int i= 0; i < datasets.Length; i++)
			//    datasets[i] = new Dataset(soLuong);
			labels = new string[soLuong];
		}

		public class Dataset
		{
			private static readonly Random rand = new();
			public string borderColor;
			public float[] data;
			public bool fill = false;
			public string label;
			public float tension = 0.45f;

			public Dataset(int soLuong)
			{
				data = new float[soLuong];
			}

			private string GetRandomColour()
			{
				var x = (DateTime.Now.Millisecond + DateTime.Now.Second) % 256;
				var y = (DateTime.Now.Millisecond + DateTime.Now.Second + DateTime.Now.Hour) % 256;
				var z = (DateTime.Now.Millisecond + DateTime.Now.Second + DateTime.Now.Minute) % 256;
				return Color.FromArgb(x, y, z, (x + y + z) % 256).ToString();
			}
		}
	}
}