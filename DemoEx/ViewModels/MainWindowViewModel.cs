using Avalonia.Animation;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoEx.Models;
using DemoEx.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        //авторизация
        [ObservableProperty]
        string currentUser = "Гость";

        [RelayCommand]
        void ToLogPage()
        {
            CurrentUser = "Гость";
            Page = new LoginPage();
        }


        [ObservableProperty]
        string login = string.Empty;

        [ObservableProperty]
        string loginError = string.Empty;

        [RelayCommand]
        void LoginIn()
        {
            var current = Users.FirstOrDefault(u => u.Login == Login);


            if (Login!= null && current != null) {
                CurrentUser = current.Name;
                Page = new ProductPage();
            }
            else
            {
                LoginError = "Введен не верный логин";
            }
        }





        //сортировка, фильтрация и поиск


        [ObservableProperty]
        static ObservableCollection<Category> categories = new(db.Categories.ToList());


        [ObservableProperty]
        public ObservableCollection<string> filterOption = new(new[] { "Все категории" }.Concat(categories.Select(p => p.Name).Distinct().ToList()));

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

        void Filter(string value)
        {

            if (value != null && value != "Все категории")
            {
                SortProducts = new(products.Where(p => p.Category.Name == value).ToList());
               
            }
            else if (value == "Все категории")
            {
                SortProducts = new(products);
            }
            if (SelectedSort != "По умолчанию")
            {
                Sort(SelectedSort);
            }
        }

        void Sort(string value)
        {
            switch (value)
            {
                case "По умолчанию":
                    SortProducts = products;
                    Filter(SelectedFilter);
                    break;
                case "Цена возрастание":
                    SortProducts = new(SortProducts.OrderBy(p => p.DiscountPrice));
                    break;
                case "Цена убывание":
                    SortProducts = new(SortProducts.OrderByDescending(p => p.DiscountPrice));
                    break;
            }
        }

        void Search(string value)
        {

            if (value == null)
            {
                Filter(SelectedFilter);
            }
            else {
                SortProducts = new(SortProducts.Where(p => (p.Name != null && p.Name.Contains(value, System.StringComparison.OrdinalIgnoreCase)) || (p.Description != null && p.Description.Contains(value, System.StringComparison.OrdinalIgnoreCase))));
            }
        }

        partial void OnSelectedFilterChanged(string value)
        {
            Filter(value);
        }

        partial void OnSelectedSortChanged(string value)
        {
            Sort(value);
        }

        partial void OnSelectedSearchChanged(string value) { 
            Search(value);
        }

        [ObservableProperty]
        List<User> users = db.Users.ToList();

        [ObservableProperty]
        static ObservableCollection<Product> products = new(db.Products.Include(x => x.Category)
                .Include(x => x.Manufacture)
            .Include(x => x.DetailOrders).ThenInclude(x => x.Order)
                .Include(x => x.Stoks)
            .Include(x=>x.Category)
            .ToList());

        [ObservableProperty]
        ObservableCollection<Product> sortProducts = new(products);
        [ObservableProperty]
        ObservableCollection<Product> filterProducts = new(products);

        //db.Products.Include(x => x.Category).Include(x => x.Manufacture).ToList();

        [ObservableProperty]
        List<Stok> stoks = db.Stoks.Include(x => x.Product).ToList();

        [RelayCommand]
        void Enter()
        {
            
        }
    }
}
