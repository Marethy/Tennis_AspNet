namespace Tennis.Models;

public class Pie(int sogoi)
{
    public Data data = new Data(sogoi);
    public Options options = new();
    public string type = "pie";

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
        public Dataset[] datasets = new Dataset[1];
        public string[] labels;

        public Data(int sogoi)
        {
            labels = new string[sogoi];
            datasets = new Dataset[1];
            datasets[0] = new Dataset(sogoi);
        }

        public class Dataset
        {
            public string[] backgroundColor; // = new string[2]
            public float[] data;

            public Dataset(int sogoi)
            {
                backgroundColor = new string[sogoi];
                data = new float[sogoi];
            }
        }
    }
}