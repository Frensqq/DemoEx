using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using DemoEx.Models;
using DemoEx.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DemoEx.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        UserControl page = new ProductPage();

        static DemoexContext db = new DemoexContext();

        [ObservableProperty]
        List<Product> products = db.Products.Include(x => x.Category).Include(x => x.Manufacture).ToList();



        public string Greeting { get; } = "Welcome to Avalonia!";
    }
}
