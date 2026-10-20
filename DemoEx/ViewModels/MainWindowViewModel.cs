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



        //лист заказов



        [ObservableProperty]
        string listOrderButtonName;

        [ObservableProperty]
        bool listOrderButtonVisible = false;

        [ObservableProperty]
        ObservableCollection<Order> orderList = new(db.Orders
            .Include(x => x.LoginNavigation)
            .Include(x => x.DetailOrders)
                .ThenInclude(x => x.Size)
            .Include(x => x.DetailOrders)
                .ThenInclude(x => x.Product)

            .ToList());

        void DeleteOrder(object obj)
        {

            if(obj is Order order)
            {
                db.Orders.Remove(order);
            }
            if(obj is DetailOrder detailOrders)
            {
                db.DetailOrders.Remove(detailOrders);
            }
            db.SaveChanges();
            updateOrderList();
            Page = new OrderListPage();

        }


        void updateOrderList()
        {
            OrderList = new(db.Orders
            .Include(x => x.LoginNavigation)
            .Include(x => x.DetailOrders)
                .ThenInclude(x => x.Size)
            .Include(x => x.DetailOrders)
                .ThenInclude(x => x.Product)

            .ToList());

        }



        [RelayCommand]
        void ListOrderToMainWindow()
        {
          
            if (Page is ProductPage)
            {
                ListOrderButtonName = "На главную";
                updateOrderList();
                Page = new OrderListPage();
            }
            else
            {
                ListOrderButtonName = "К заказам";
                Page = new ProductPage();
            }
        }
        

        // авторизация

        [ObservableProperty]
        string currentUserName = "Гость";
        [ObservableProperty]
        static string currentlogin = string.Empty;
        [ObservableProperty]
        User currentUser = null;

        [RelayCommand]
        void ToLogPage()
        {
            IsAuth = false;
            CurrentUserName = "Гость";
            Currentlogin = string.Empty;
            ListOrderButtonVisible = false;
            CurrentOrder = null;
            CurrentUser = null;
            Page = new LoginPage();
        }



        [ObservableProperty]
        bool isAuth = false;

        [ObservableProperty]
        string loginError = string.Empty;
        

        [RelayCommand]
        void LoginIn()
        {
            var current = Users.FirstOrDefault(u => u.Login == Currentlogin);

            if (!string.IsNullOrEmpty(Currentlogin) && current != null)
            {
                IsAuth = true;
                CurrentUserName = current.Name + "\n" + current.Surname + "\n" + current.Patronimyc;
                CurrentUser =current ;

                if (current.IdRole < 3)
                {
                    updateOrderList();
                    ListOrderButtonVisible = true;
                    ListOrderButtonName = "На главную";
                    Page = new OrderListPage();
                }
                else
                {
                    Page = new ProductPage();
                }
                
                LoginError = string.Empty;
                CurrentOrder = new Order() { Date = DateOnly.FromDateTime(DateTime.Now), Login = currentlogin };
            }
            else
            {
                LoginError = "Введен не верный логин";
            }
        }



        //Заказ

        [ObservableProperty]
        Product currentProduct;

        OrderWindow orderWindow;

        [ObservableProperty]
        Order currentOrder;

        [ObservableProperty]
        Stok currentStock = null;

        [RelayCommand]
        void toOrderPage(Product product)
        {
            CurrentProduct = product;

            CurrentStock = product.Stoks?.FirstOrDefault();
            CurrentCount = 1;

            var owner = (Avalonia.Application.Current?.ApplicationLifetime
                 as Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime)
                 ?.MainWindow;
            orderWindow = new OrderWindow(this);
            orderWindow.ShowDialog(owner);

            
        }

        

        

        [ObservableProperty]
        int currentCount;

        [ObservableProperty]
        string orderError = string.Empty;

        [RelayCommand]
        void AddToOrder()
        {
            OrderError = string.Empty;

            if (CurrentCount <= 0) { OrderError = "Количество должно быть больше 0"; return; }
            if (CurrentCount > CurrentStock.Count)
            {
                OrderError = $"На складе только {CurrentStock.Count} шт.";
                return;
            }

            CurrentOrder.DetailOrders.Add(new DetailOrder
            {
                Count = CurrentCount,
                ProductId = CurrentStock.ProductId,
                SizeId = CurrentStock.SizeId,
                Product = CurrentProduct,
                Size = CurrentStock.Size
            });

            CurrentStock.Count -= CurrentCount;

            orderWindow?.Close();
            Page = new ProductPage();
        }


        [RelayCommand]
        void CreateOrder()
        {
            if (CurrentOrder == null || CurrentOrder.DetailOrders.Count == 0) return;

            db.Orders.Add(CurrentOrder);
            db.SaveChanges();

            Page = new ProductPage();

            // сброс
            CurrentOrder = null;
            CurrentProduct = null;
            CurrentStock = null;
            CurrentCount = 1;
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