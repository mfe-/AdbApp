using AdbApp.Maui.ViewModels;
using System.Collections.Specialized;

namespace AdbApp.Maui.Views;

public partial class AdbPage : ContentPage
{
    private readonly AdbPageViewModel viewModel;

    public AdbPage(AdbPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = this.viewModel = viewModel;
        this.viewModel.Output.CollectionChanged += HandleOutputCollectionChanged;
    }

    private void HandleOutputCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        var lastItem = viewModel.FilterOutput.LastOrDefault();
        if (lastItem is null)
        {
            return;
        }

        Dispatcher.Dispatch(() =>
        {
            OutputCollectionView.ScrollTo(lastItem, position: ScrollToPosition.End, animate: false);
        });
    }
}
