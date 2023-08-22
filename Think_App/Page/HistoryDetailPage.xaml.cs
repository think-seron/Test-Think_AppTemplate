using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class HistoryDetailPage : ContentPage
    {
        HistoryDetailPageViewModel historyDetailPageViewModel;

        public HistoryDetailPage(ListVIewHistoryCellViewModel list)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            historyDetailPageViewModel = new HistoryDetailPageViewModel(list);


            this.BindingContext = historyDetailPageViewModel;
        }
    }
}
