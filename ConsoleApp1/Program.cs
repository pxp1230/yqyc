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
        public string day;
        public string amount;
    }
    class Program
    {
        const string url1 = "https://shcovid192022.azurewebsites.net/data?v=function%20random()%20{%20[native%20code]%20}";
        static void Main(string[] args)
        {
            string jsFile = null;
            string dataFile = "";
            DirectoryInfo directoryInfo = new DirectoryInfo(Environment.CurrentDirectory);
            while (directoryInfo.Parent != null)
            {
                directoryInfo = directoryInfo.Parent;
                FileInfo[] fileInfos = directoryInfo.GetFiles("abc.js", SearchOption.TopDirectoryOnly);
                if (fileInfos.Length == 1)
                {
                    jsFile = fileInfos[0].FullName;
                    FileInfo[] fileInfos2 = directoryInfo.GetFiles("data.json", SearchOption.TopDirectoryOnly);
                    dataFile = fileInfos2[0].FullName;
                }
            }
            if (jsFile == null)
            {
                Console.WriteLine("找不到js文件");
            }

            //WebClient webClient = new WebClient();
            //byte[] _str1 = webClient.DownloadData(url1);
            //string str1 = UTF8Encoding.UTF8.GetString(_str1);

            string str1 = File.ReadAllText(dataFile);

            JObject json1 = JsonConvert.DeserializeObject<JObject>(str1);
            List<DataRaw> shanghaiDayList = json1["amounts"].ToObject<List<DataRaw>>();
            shanghaiDayList.Add(new DataRaw { day = "3.29", amount = "5982" });
            shanghaiDayList.Add(new DataRaw { day = "3.30", amount = "5653" });
            shanghaiDayList.Add(new DataRaw { day = "3.31", amount = "" + (358 + 4144) });
            shanghaiDayList.Add(new DataRaw { day = "4.1", amount = "" + (260 + 6051) });
            shanghaiDayList.Add(new DataRaw { day = "4.2", amount = "" + (438 + 7788) });
            shanghaiDayList.Add(new DataRaw { day = "4.3", amount = "" + (425 + 8581) });
            shanghaiDayList.Add(new DataRaw { day = "4.4", amount = "" + (268 + 13086) });
            shanghaiDayList.Add(new DataRaw { day = "4.5", amount = "" + (311 + 16766) });
            shanghaiDayList.Add(new DataRaw { day = "4.6", amount = "" + (322 + 19660) });

            foreach (var item in shanghaiDayList)
            {
                item.x = DateTime.Parse("2022." + item.day).DayOfYear;
            }
            shanghaiDayList.Sort((a, b) => { return (int)(a.x - b.x); });


            double min = shanghaiDayList[17].x;//从3月15日开始算
            DateTime min_date = DateTime.Parse("2022." + shanghaiDayList[0].day);
            DateTime max_date = DateTime.Parse("2022." + shanghaiDayList[shanghaiDayList.Count - 1].day + " 23:59");
            if (DateTime.Now < max_date)
                shanghaiDayList.RemoveAt(shanghaiDayList.Count - 1);
            foreach (var item in shanghaiDayList)
            {
                item.x -= min;
                item.y = int.Parse(item.amount);
            }

            List<DataRaw> new_shanghaiDayList = new List<DataRaw>();
            for (int i = 17; i < shanghaiDayList.Count; i++)
            {
                new_shanghaiDayList.Add(shanghaiDayList[i]);
            }


            double[] xx = new double[new_shanghaiDayList.Count];
            double[] yy = new double[new_shanghaiDayList.Count];
            for (int i = 0; i < new_shanghaiDayList.Count; i++)
            {
                xx[i] = new_shanghaiDayList[i].x;
                yy[i] = new_shanghaiDayList[i].y;
                Console.WriteLine(new_shanghaiDayList[i].day + " " + new_shanghaiDayList[i].amount);
            }

            double[] p = Fit.Polynomial(xx, yy, 5);
            string c = $"{p[0]}+{p[1]}*x+{p[2]}*x*x+{p[3]}*x*x*x+{p[4]}*x*x*x*x+{p[5]}*x*x*x*x*x;";
            c = c.Replace("+-", "-");
            Console.WriteLine(c);
            Console.WriteLine();

            ////测试：
            ////DateTime date = new DateTime(2020, 1, 20);
            ////double x = (date - min_date).TotalDays;
            ////double result = p[0] + p[1] * x + p[2] * Math.Pow(x, 2) + p[3] * Math.Pow(x, 3) + p[4] * Math.Pow(x, 4) + p[5] * Math.Pow(x, 5);
            ////Console.WriteLine(date.ToShortDateString() + " 人数：" + (int)result);


            if (jsFile != null)
            {
                string jsTxt = File.ReadAllText(jsFile);
                string d = "\"2022." + new_shanghaiDayList[new_shanghaiDayList.Count - 1].day + " 24:00\";";

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
