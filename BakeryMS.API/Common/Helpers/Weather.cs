using System;
using System.Collections.Generic;

namespace BakeryMS.API.Common.Helpers
{
    public class WeatherInfo
    {
        public List<Daily> daily { get; set; }
    }
    public class Weather
    {
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Daily
    {
        public Daily()
        {
            rain = 0;
        }
        public int dt { get; set; }
        public DateTime date { get; set; }
        public float rain { get; set; }
        public List<Weather> weather { get; set; }
    }

    // public class Temp
    // {
    //     public double day { get; set; }
    //     public double min { get; set; }
    //     public double max { get; set; }
    //     public double night { get; set; }
    // }
}