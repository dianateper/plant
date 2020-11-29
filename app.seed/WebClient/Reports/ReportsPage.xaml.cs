﻿using Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WebClient
{
    /// <summary>
    /// Логика взаимодействия для ReportsPage.xaml
    /// </summary>
    public partial class ReportsPage : Window
    {

        List<Pricing> prices = new List<Pricing>();
        
        public ReportsPage()
        {
            InitializeComponent();

            prices = MainWindow.channel.GetPrice();
            PriceDataGrid.ItemsSource = prices;

            MakeForecastForNextMonth();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var newForm = new MainWindow();
            newForm.Show();
            this.Close();
        }


        void MakeForecastForNextMonth()
        {
            StringBuilder forecast = new StringBuilder();
            forecast.Append("Fertilizer forecast: "  + Math.Round(
                MainWindow.channel.MakeArimaPrediction(1, 
                prices.Select(c => c.PriceForFertilizer).ToList()).Select(x=>x).FirstOrDefault(),2));
            forecast.Append("\nSeeds forecast: " + Math.Round(
                MainWindow.channel.MakeArimaPrediction(1, 
                prices.Select(c => c.PriceForPlant).ToList()).Select(x => x).FirstOrDefault(),2));

            forecastText.Text = forecast.ToString();

        }
    }
}
