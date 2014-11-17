using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using FiestaMode.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace FiestaMode.Pages
{
    public partial class MainPage : Page
    {
        private static Page _currentModePage;

        public static Page CurrentModePage
        {
            get
            {
                return _currentModePage;
            }
        }


        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void modeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = ((ListBox)sender).SelectedItem as ItemViewModel;

            if (item != null)
            {
                //clear selection
                ((ListBox)sender).SelectedItem = null;

                Debug.WriteLine("tapped " + item.PageURL);

                //TODO  make this get the actual type needed
                Frame.Navigate(item.PageType);
            }
        }
    }
}