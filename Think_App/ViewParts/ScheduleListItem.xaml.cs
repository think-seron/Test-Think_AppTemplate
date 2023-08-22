﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
    public partial class ScheduleListItem : ContentView
    {
        public ScheduleListItem()
        {
            InitializeComponent();
            Task.Run(() =>
            {
                this.EnableBtnTap.Tapped += (sender, e) =>
                {
                    try
                    {
                        var bc = this.BindingContext as ScheduleListItemViewModel;
                        if (bc.CanReservation)
                        {
                            this.EnableBtn.Opacity = 0.6;
                            this.EnableBtn.FadeTo(1.0);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("tap ex :" + ex);
                    }
                };

            });
        }

    }
}
