using Avalonia.Animation;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        [ObservableProperty]
        string currentUser = "Гость";



        static DemoexContext db = new DemoexContext();

        [ObservableProperty]
        List<Product> products = db.Products
                .Include(x => x.Category)
                .Include(x => x.Manufacture)
            .Include(x => x.DetailOrders).ThenInclude(x => x.Order)
                .Include(x => x.Stoks)
                .ToList();
        //db.Products.Include(x => x.Category).Include(x => x.Manufacture).ToList();

        [ObservableProperty]
        List<Stok> stoks = db.Stoks.Include(x => x.Product).ToList();

        [RelayCommand]
        void Enter()
        {
            
        }
    }
}
