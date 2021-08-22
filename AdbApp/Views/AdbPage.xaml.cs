
using AdbApp.ViewModels;
using System;

namespace AdbApp.Views
{
    public partial class AdbPage : ISearchPage
    {
        public AdbPage()
        {
            InitializeComponent();
            SearchBarTextChanged += HandleSearchBarTextChanged;
        }

        public event EventHandler<string> SearchBarTextChanged;

        public void OnSearchBarTextChanged(string text)
        {
            SearchBarTextChanged?.Invoke(this, text);
        }

        void HandleSearchBarTextChanged(object sender, string searchBarText)
        {
            //Logic to handle updated search bar text
            if (BindingContext is AdbPageViewModel adbPageViewModel)
            {
                adbPageViewModel.FilterCommand.Execute(searchBarText);
            }
        }
    }
}
