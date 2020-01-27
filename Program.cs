using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApp1
{
    class DataRaw
    {
        public double x;
        public double y;
        public string date;
        public string confirm;
        public string suspect;
        public string dead;
        public string heal;
    }
    class Program
    {
        const string dataFile = @"C:\Users\Administrator\Desktop\1\数据.txt";
        static void Main(string[] args)
        {
            string rawStr = System.IO.File.ReadAllText(dataFile);
            int _a = rawStr.IndexOf('(');
            int _b = rawStr.LastIndexOf(')');
            rawStr = rawStr.Substring(_a + 1, _b - _a - 1);
            JObject json = JsonConvert.DeserializeObject<JObject>(rawStr);
            List<DataRaw> raw = JsonConvert.DeserializeObject<List<DataRaw>>(json["data"].ToString());
            foreach (var item in raw)
            {
                item.x = DateTime.Parse("2020." + item.date).DayOfYear;
            }
            raw.Sort((a, b) => { return (int)(a.x - b.x); });
            double min = raw[0].x;
            DateTime min_date = DateTime.Parse("2020." + raw[0].date);
            foreach (var item in raw)
            {
                item.x -= min;
                item.y = int.Parse(item.confirm);
            }
            double[] xx = new double[raw.Count];
            double[] yy = new double[raw.Count];
            for (int i = 0; i < raw.Count; i++)
            {
                xx[i] = raw[i].x;
                yy[i] = raw[i].y;
                //Console.WriteLine(item.x + " " + item.y);
            }

            double[] p = Fit.Polynomial(xx, yy, 5);
            string c = $"{p[0]}+{p[1]}*x+{p[2]}*x*x+{p[3]}*x*x*x+{p[4]}*x*x*x*x+{p[5]}*x*x*x*x*x;";
            c = c.Replace("+-", "-");
            Console.WriteLine(c);
            Console.WriteLine();

            DateTime date = new DateTime(2020, 2, 1);
            double x = (date - min_date).TotalDays;
            double result = p[0] + p[1] * x + p[2] * Math.Pow(x, 2) + p[3] * Math.Pow(x, 3) + p[4] * Math.Pow(x, 4) + p[5] * Math.Pow(x, 5);
            Console.WriteLine(date.ToShortDateString() + " 人数：" + (int)result);

            Console.ReadKey();
        }
    }
}
