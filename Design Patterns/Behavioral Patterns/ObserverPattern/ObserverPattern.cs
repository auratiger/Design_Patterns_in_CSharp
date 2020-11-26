using System;
using System.Collections.Generic;

namespace Design_Patterns.Behavioral_Patterns.ObserverPattern
{
    /*
     * The Observer is a behavioral design pattern that lets you define a subscription mechanism
     * to notify multiple objects about any events that happen to the object they're observing.
     *
     * Imagine that you have two types of objects: a weather station and weather app which fetches that information.
     * The weather app is very interested in lots of weather conditions which change fairly regularly.
     *
     * Suppose our app queries/asks the station every 10 minutes to check the weather conditions,
     * but during those periods the app won't hove the latest prognosis.
     * One approach would be to make the weather app query the station every 5 seconds,
     * but that would be a very heavy operation which might put strain on the system of the station,
     * especially if other weather apps start doing the same. The best solution would be to create
     * a publish/subscribe model:
     *     - every time the weather station had new data it will push a notification/update to all of the apps
     *     and inform them of the changes.
     * The observer pattern has a one to many relationship, meaning many objects observe one object
     * referred to as the subject. In our case, the weather station is the subject (observable)
     * and the weather apps  are the observers.
     *
     * Difference between observer and mediator
     * https://stackoverflow.com/questions/9226479/mediator-vs-observer-object-oriented-design-patterns
     */
    public class ObserverPattern
    {
        public static void Test()
        {
            var weatherStation = new WeatherStation();
            var app1 = new WeatherApplication("The Weather channel");
            var app2 = new WeatherApplication("National news");
            
            //add observers to the station
            weatherStation.AddObserver(app1);
            weatherStation.AddObserver(app2);
            
            // set weather
            weatherStation.SetWeather(17);
            app1.DisplayWeather();
            app2.DisplayWeather();
            
            // weather changes
            weatherStation.SetWeather(20);
            app1.DisplayWeather();
            app2.DisplayWeather();
        }
    }

    public class WeatherStation
    {
        private double temperature;
        private List<MobileApp> observers = new List<MobileApp>();

        public void AddObserver(MobileApp observer)
        {
            observers.Add(observer);
        }

        public void RemoveObserver(MobileApp observer)
        {
            observers.Remove(observer);
        }

        public void SetWeather(double temperature)
        {
            this.temperature = temperature;

            foreach (var app in observers) app.Update(this.temperature);
        }
    }

    public interface MobileApp
    {
        void Update(double temperature);
    }

    class WeatherApplication : MobileApp
    {
        public double Temperature { get; set; }
        public string ApplicationName { get; private set; }

        public WeatherApplication(string applicationName)
        {
            ApplicationName = applicationName;
        }

        public void Update(double temperature)
        {
            Temperature = temperature;
        }

        public void DisplayWeather()
        {
            Console.WriteLine($"{ApplicationName} reports the current temperature is {Temperature} degrees celsius.");
        }
    }
    
}