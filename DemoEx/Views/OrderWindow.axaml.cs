using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DemoEx.ViewModels;

namespace DemoEx.Views;

public partial class OrderWindow : Window
{
    public OrderWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}