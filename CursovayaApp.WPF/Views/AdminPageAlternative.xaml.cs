﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.ViewModels;

namespace CursovayaApp.WPF.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminPageAlternative.xaml
    /// </summary>
    public partial class AdminPageAlternative : Page
    {
        public AdminPageAlternative()
        {
            InitializeComponent();
            DataContext = new AdminViewModel();
        }
    }
}