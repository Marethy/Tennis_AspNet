namespace FoodWebMVC.Models;

public class Bar
{
	public Data data;
	public Options options = new();
	public string type = "bar";

	public Bar(int soLuong)
	{
		data = new Data(soLuong);
	}


	public class Options
	{
		public Plugins plugins = new();
		public Scales scales = new();

		public class Plugins
		{
			public Title title = new();

			public class Title
			{
				public bool display = true;
				public string text;
			}
		}

		public class Scales
		{
			public Y y = new();

			public class Y
			{
				public bool beginAtZero = true;
			}
		}
	}

	public class Data
	{
		public Dataset[] datasets = new Dataset[1];
		public string[] labels;

		public Data(int soLuong)
		{
			if (soLuong > 0)
			{
				labels = new string[soLuong];
				datasets = new Dataset[1];
				datasets[0] = new Dataset(soLuong);
			}
		}

		public class Dataset
		{
			public string[] backgroundColor; // = new string[2]
			public string[] borderColor;
			public int borderWidth = 1;
			public float[] data;
			public string label;

			public Dataset(int soLuong)
			{
				backgroundColor = new string[soLuong];
				borderColor = new string[soLuong];
				data = new float[soLuong];
			}
		}
	}
}