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

    void OnSizeChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is not ComboBox box) return;
        if (box.SelectedItem is not Stok stok) return;

        var countBox = this.FindControl<NumericUpDown>("CountBox");
        if (countBox == null) return;

        countBox.Maximum = stok.Count;
        if (countBox.Value > stok.Count)
            countBox.Value = stok.Count;
        if (countBox.Value < 1)
            countBox.Value = 1;
    }

    void OnCancelClick(object? sender, RoutedEventArgs e) => Close();
}