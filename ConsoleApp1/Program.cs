﻿using System;
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
        public string notHubei;
    }
    class Program
    {
        const string url1 = "https://view.inews.qq.com/g2/getOnsInfo?name=disease_h5";
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
            byte[] _str1 = webClient.DownloadData(url1);
            string str1 = UTF8Encoding.UTF8.GetString(_str1);
            JObject json1 = JsonConvert.DeserializeObject<JObject>(str1);
            JObject json2 = JsonConvert.DeserializeObject<JObject>(json1["data"].ToString());
            List<DataRaw> chinaDayList = json2["chinaDayList"].ToObject<List<DataRaw>>();
            JArray dailyDeadRateHistory = (JArray)json2["dailyNewAddHistory"];
            Dictionary<string, string> hubeiConfirmDict = new Dictionary<string, string>();
            foreach (var item in dailyDeadRateHistory)
            {
                hubeiConfirmDict[item["date"].ToString()] = item["notHubei"].ToString();
            }
            List<DataRaw> chinaDayList_2 = new List<DataRaw>();
            foreach (var item in chinaDayList)
            {
                item.x = DateTime.Parse("2020." + item.date).DayOfYear;
                if (hubeiConfirmDict.ContainsKey(item.date))
                {
                    item.notHubei = hubeiConfirmDict[item.date];
                    chinaDayList_2.Add(item);
                }
            }
            chinaDayList = chinaDayList_2;
            chinaDayList.Sort((a, b) => { return (int)(a.x - b.x); });

            double min = chinaDayList[0].x;
            DateTime min_date = DateTime.Parse("2020." + chinaDayList[0].date);
            DateTime max_date = DateTime.Parse("2020." + chinaDayList[chinaDayList.Count - 1].date + " 23:59");
            if (DateTime.Now < max_date)
                chinaDayList.RemoveAt(chinaDayList.Count - 1);
            foreach (var item in chinaDayList)
            {
                item.x -= min;
                item.y = int.Parse(item.notHubei);
            }
            double[] xx = new double[chinaDayList.Count];
            double[] yy = new double[chinaDayList.Count];
            for (int i = 0; i < chinaDayList.Count; i++)
            {
                xx[i] = chinaDayList[i].x;
                yy[i] = chinaDayList[i].y;
                //Console.WriteLine(item.x + " " + item.y);
            }

            double[] p = Fit.Polynomial(xx, yy, 5);
            string c = $"{p[0]}+{p[1]}*x+{p[2]}*x*x+{p[3]}*x*x*x+{p[4]}*x*x*x*x+{p[5]}*x*x*x*x*x;";
            c = c.Replace("+-", "-");
            Console.WriteLine(c);
            Console.WriteLine();

            //测试：
            //DateTime date = new DateTime(2020, 1, 20);
            //double x = (date - min_date).TotalDays;
            //double result = p[0] + p[1] * x + p[2] * Math.Pow(x, 2) + p[3] * Math.Pow(x, 3) + p[4] * Math.Pow(x, 4) + p[5] * Math.Pow(x, 5);
            //Console.WriteLine(date.ToShortDateString() + " 人数：" + (int)result);


            if (jsFile != null)
            {
                string jsTxt = File.ReadAllText(jsFile);
                string d = "\"2020." + chinaDayList[chinaDayList.Count - 1].date + " 24:00\";";

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
