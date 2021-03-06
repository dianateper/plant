﻿using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace WebClient
{
    /// <summary>
    /// Логика взаимодействия для StatisticaDetail.xaml
    /// </summary>
    public partial class StatisticaDetail : Window
    {
        public Func<double, string> Formatter { get; set; }
        ControllerStatistica statistica;
        public SeriesCollection TemperatureCollection { get; set; }
        public SeriesCollection HumidityCollection { get; set; }

        public List<Plant> historyPlants { get; set; }

        public StatisticaDetail(int X, int Y)
        {
            InitializeComponent();
            statistica = MainWindow.channel.GetControllerStatistica(X, Y);
            historyPlants = MainWindow.channel.GetPlantsHistoryByPosition(X,Y);

            SetGridsVariable();
            SetChartsVariable();


        
        }

        public void SetGridsVariable()
        {
            ShowDetail(statistica.statisticaTemperature, Temperature);
            ShowDetail(statistica.statisticaHumidity, Humidity);
            PlantsHistoryGrid.ItemsSource = historyPlants;
        }


        List<double> temperature = new List<double>();
        List<double> humidity = new List<double>();


        List<double> temperatureForecast = new List<double>();
        List<double> humidityForecast = new List<double>();


        public void SetChartsVariable()
        {

            TemperatureCollection = new SeriesCollection
            {
                new  LineSeries
                {
                    Title = "Temperature",
                    Values = RawDataSeriesTemperature,
                    Fill = new SolidColorBrush(Colors.Bisque),
                    Opacity = 0.5
                },
                new  LineSeries
                {
                    Title = "Temperature forecast",
                    Values = RawDataSeriesTemperatureForecast,
                    StrokeDashArray = new System.Windows.Media.DoubleCollection(new double[] { 20 }),
                    Fill = Brushes.Transparent

                }


            };

            HumidityCollection = new SeriesCollection
            {
                new  LineSeries
                {
                    Title = "Humidity",
                    Values = RawDataSeriesHumidity,
                    Fill = new SolidColorBrush(Colors.Bisque),
                    Opacity = 0.5
                },
                new  LineSeries
                {
                    Title = "Humidity",
                    Values = RawDataSeriesHumidityForecast,
                    StrokeDashArray = new System.Windows.Media.DoubleCollection(new double[] { 20 }),
                    Fill = Brushes.Transparent
                }
            };

           

            GraphTemperatureAxisX.LabelFormatter = value => new DateTime((long)value).ToString("dd/MM/yy");
            GraphHumidityAxisX.LabelFormatter = value => new DateTime((long)value).ToString("dd/MM/yy");



            DataContext = this;
            GraphTemperature.Series = TemperatureCollection;
            GraphHumidity.Series = HumidityCollection;

           
            minValueDate.Text = statistica.controllerHistories.Select(x => x).FirstOrDefault().datetime.ToString();
            maxValueDate.Text = statistica.controllerHistories.Select(x => x).LastOrDefault().datetime.ToString();


        }

        public void ShowDetail(StatisticaView view, System.Windows.Controls.DataGrid grid)
        {
            grid.Items.Add(new { Property = "Max", Value = view.Max });
            grid.Items.Add(new { Property = "Min", Value = view.Min });
            grid.Items.Add(new { Property = "Median", Value = view.Median });
            grid.Items.Add(new { Property = "Mean", Value = view.Mean });
            grid.Items.Add(new { Property = "Range", Value = view.Range });
            grid.Items.Add(new { Property = "Variance", Value = view.Variance });
            grid.Items.Add(new { Property = "StandartDeriation", Value = view.StandartDeriation });
            grid.Items.Add(new { Property = "Kurtosis", Value = view.Kurtosis });
            grid.Items.Add(new { Property = "Skewnes", Value = view.Skewnes });
        }


        public ChartValues<DateTimePoint> RawDataSeriesTemperature
        {
            get
            {
                ChartValues<DateTimePoint> Values = new ChartValues<DateTimePoint>();
                foreach (ControllerHistory history in statistica.controllerHistories)
                {
                    Values.Add(new DateTimePoint(history.datetime, history.temperature));
                    temperature.Add(history.temperature);
                }
                return Values;
            }
        }


        public ChartValues<DateTimePoint> RawDataSeriesTemperatureForecast
        {
            get
            {
                temperatureForecast = MainWindow.channel.MakeArimaPrediction(3, temperature);
                DateTime date = new DateTime();
                ChartValues<DateTimePoint> Values = new ChartValues<DateTimePoint>();
                foreach (ControllerHistory history in statistica.controllerHistories)
                {
                    Values.Add(new DateTimePoint(history.datetime, history.temperature));
                    date = history.datetime;
                }

                int days = 1;
                foreach (double temperature in temperatureForecast)
                {

                    Values.Add(new DateTimePoint(date.AddDays(days), temperature));
                    days++;
                }

                return Values;
            }
        }

        public ChartValues<DateTimePoint> RawDataSeriesHumidity
        {
            get
            {
                ChartValues<DateTimePoint> Values = new ChartValues<DateTimePoint>();
                foreach (ControllerHistory history in statistica.controllerHistories)
                {
                    Values.Add(new DateTimePoint(history.datetime, history.humidity));
                    humidity.Add(history.humidity);
                }
                return Values;
            }
        }

        public ChartValues<DateTimePoint> RawDataSeriesHumidityForecast
        {
            get
            {
                humidityForecast = MainWindow.channel.MakeArimaPrediction(3, humidity);
                DateTime date = new DateTime();
                ChartValues<DateTimePoint> Values = new ChartValues<DateTimePoint>();
                foreach (ControllerHistory history in statistica.controllerHistories)
                {
                    Values.Add(new DateTimePoint(history.datetime, history.humidity));
                    date = history.datetime;
                }

                int days = 1;
                foreach(double humidity in humidityForecast)
                {

                    Values.Add(new DateTimePoint(date.AddDays(days),humidity));
                    days++;
                }


                return Values;
            }
        }



        private void minValueDate_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            GraphTemperatureAxisX.MinValue = minValueDate.SelectedDate.GetValueOrDefault().Ticks;
            GraphHumidityAxisX.MinValue = minValueDate.SelectedDate.GetValueOrDefault().Ticks;

        }

        private void maxValueDate_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            GraphTemperatureAxisX.MaxValue = maxValueDate.SelectedDate.GetValueOrDefault().Ticks;
            GraphHumidityAxisX.MaxValue = maxValueDate.SelectedDate.GetValueOrDefault().Ticks;

        }
    }


}
