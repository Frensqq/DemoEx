using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DemoEx.Models;
using DemoEx.ViewModels;

namespace DemoEx.Views;

public partial class OrderWindow : Window
{

    public OrderWindow()
    {
        InitializeComponent();
    }
    public OrderWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

   

    void OnCancelClick(object? sender, RoutedEventArgs e) => Close();
}