using Avalonia.Animation;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoEx.Models;
using DemoEx.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DemoEx.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        static DemoexContext db = new DemoexContext();

        [ObservableProperty]
        UserControl page = new ProductPage();

        [RelayCommand]
        void ToProductPage()
        {
            Page = new ProductPage();
        }

        // авторизация
        [ObservableProperty]
        string currentUser = "Гость";

        [RelayCommand]
        void ToLogPage()
        {
            IsAuth = false;
            CurrentUser = "Гость";
            Currentlogin = string.Empty;
            Page = new LoginPage();
        }

        [ObservableProperty]
        static string currentlogin = string.Empty;

        [ObservableProperty]
        bool isAuth = false;

        [ObservableProperty]
        string loginError = string.Empty;

        [ObservableProperty]
        string login = string.Empty;

        [RelayCommand]
        void LoginIn()
        {
            var current = Users.FirstOrDefault(u => u.Login == Currentlogin);

            if (!string.IsNullOrEmpty(Currentlogin) && current != null)
            {
                IsAuth = true;
                CurrentUser = current.Name;
                Page = new ProductPage();
                LoginError = string.Empty;
            }
            else
            {
                LoginError = "Введен не верный логин";
            }
        }

        [ObservableProperty]
        Product currentProduct;


        OrderWindow orderWindow;

        [RelayCommand]
        void toOrderPage(Product product)
        {
            CurrentProduct = product;

            var owner = (Avalonia.Application.Current?.ApplicationLifetime
                 as Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime)
                 ?.MainWindow;
            orderWindow = new OrderWindow(this);
            orderWindow.ShowDialog(owner);
        }

        //Заказ

        [ObservableProperty]
        Order currnetOrder = new Order() { Date = DateOnly.FromDateTime(DateTime.Now), Login = currentlogin };

        [ObservableProperty]
        Stok currentStock = null;

        [ObservableProperty]
        int currentCount;

        [RelayCommand]
        void AddToOrderCommand()
        {

            

        }






        // данные с бд

        [ObservableProperty]
        List<User> users = db.Users.ToList();

        static ObservableCollection<Product> products = new(db.Products
            .Include(x => x.Category)
            .Include(x => x.Manufacture)
            .Include(x => x.DetailOrders).ThenInclude(x => x.Order)
            .Include(x => x.Stoks).ThenInclude(s => s.Size)
            .ToList());

        [ObservableProperty]
        ObservableCollection<Product> sortProducts = new(products);

        [ObservableProperty]
        List<Stok> stoks = db.Stoks.Include(x => x.Product).ToList();

        //сортировка, фильтрация и поиск

        static ObservableCollection<Category> categories = new(db.Categories.ToList());

        public ObservableCollection<string> FilterOption { get; } = new(
            new[] { "Все категории" }.Concat(categories.Select(p => p.Name).Distinct().ToList()));

        public ObservableCollection<string> SortOption { get; } = new()
        {
            "По умолчанию",
            "Цена возрастание",
            "Цена убывание",
        };

        [ObservableProperty]
        string selectedFilter = "Все категории";

        [ObservableProperty]
        string selectedSort = "По умолчанию";

        [ObservableProperty]
        string selectedSearch = "";

        void ApplyAll()
        {
            IEnumerable<Product> query = products;

            if (!string.IsNullOrEmpty(SelectedFilter) && SelectedFilter != "Все категории")
                query = query.Where(p => p.Category.Name == SelectedFilter);

            if (!string.IsNullOrWhiteSpace(SelectedSearch))
            {
                var s = SelectedSearch;
                query = query.Where(p =>
                    (p.Name?.Contains(s, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (p.Description?.Contains(s, StringComparison.OrdinalIgnoreCase) ?? false));
            }

            query = SelectedSort switch
            {
                "Цена возрастание" => query.OrderBy(p => p.DiscountPrice),
                "Цена убывание" => query.OrderByDescending(p => p.DiscountPrice),
                _ => query
            };

            SortProducts = new ObservableCollection<Product>(query);
        }

        partial void OnSelectedFilterChanged(string value) => ApplyAll();
        partial void OnSelectedSortChanged(string value) => ApplyAll();
        partial void OnSelectedSearchChanged(string value) => ApplyAll();

    }
}