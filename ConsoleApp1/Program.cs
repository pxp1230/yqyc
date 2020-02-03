using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
        const string url1 = "https://view.inews.qq.com/g2/getOnsInfo?name=wuwei_ww_cn_day_counts";
        static void Main(string[] args)
        {
            string jsFile = null;
            DirectoryInfo directoryInfo = new DirectoryInfo(Environment.CurrentDirectory);
            while (directoryInfo.Parent != null)
            {
                directoryInfo = directoryInfo.Parent;
                FileInfo[] fileInfos = directoryInfo.GetFiles("abc.js", SearchOption.TopDirectoryOnly);
                if (fileInfos.Length == 1)
                {
                    jsFile = fileInfos[0].FullName;
                    break;
                }
            }
            if (jsFile == null)
            {
                Console.WriteLine("找不到js文件");
            }

            WebClient webClient = new WebClient();
            string str1 = webClient.DownloadString(url1);
            JObject json1 = JsonConvert.DeserializeObject<JObject>(str1);
            List<DataRaw> raw = JsonConvert.DeserializeObject<List<DataRaw>>(json1["data"].ToString());
            foreach (var item in raw)
            {
                item.x = DateTime.Parse("2020." + item.date).DayOfYear;
            }
            raw.Sort((a, b) => { return (int)(a.x - b.x); });

            double min = raw[0].x;
            DateTime max_date = DateTime.Parse("2020." + raw[raw.Count - 1].date + " 23:59");
            if (DateTime.Now < max_date)
                raw.RemoveAt(raw.Count - 1);
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

            //测试：
            //DateTime date = new DateTime(2020, 2, 1);
            //double x = (date - min_date).TotalDays;
            //double result = p[0] + p[1] * x + p[2] * Math.Pow(x, 2) + p[3] * Math.Pow(x, 3) + p[4] * Math.Pow(x, 4) + p[5] * Math.Pow(x, 5);
            //Console.WriteLine(date.ToShortDateString() + " 人数：" + (int)result);


            if (jsFile != null)
            {
                string jsTxt = File.ReadAllText(jsFile);
                string d = "\"2020/" + raw[raw.Count - 1].date + " 24:00\";";

                Regex r1 = new Regex(@"//1\n.+\n//2\n");
                jsTxt = r1.Replace(jsTxt, "//1\n" + d + "\n//2\n");

                Regex r2 = new Regex(@"//3\n.+\n//4\n");
                jsTxt = r2.Replace(jsTxt, "//3\n" + c + "\n//4\n");

                File.WriteAllText(jsFile, jsTxt);
            }


            Console.ReadKey();
        }
    }
}
