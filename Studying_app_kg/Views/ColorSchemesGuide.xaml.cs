﻿using Studying_app_kg.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CG.Views
{
    /// <summary>
    /// Interaction logic for ColorSchemesGuide.xaml
    /// </summary>
    public partial class ColorSchemesGuide : Page
    {
        public ColorSchemesGuide()
        {
            InitializeComponent();
        }
        private void Home_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ColorSchemes(this));
        }
    }
}
