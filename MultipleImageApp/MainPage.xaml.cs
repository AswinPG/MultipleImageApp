﻿using MultipleImageApp.Models;
using MultipleImageApp.Services;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MultipleImageApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        public ObservableCollection<MediaFile> Media { get; set; }
        IMultiMediaPickerService _multiMediaPickerService;
        public MainPage(IMultiMediaPickerService multiMediaPickerService)
        {
            InitializeComponent();
            _multiMediaPickerService = multiMediaPickerService;
            MainCollectionView.ItemsSource = Media;



            _multiMediaPickerService.OnMediaPicked += (s, a) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Media.Add(a);

                });

            };
        }

        private async void Button_ClickedAsync(object sender, EventArgs e)
        {
            var hasPermission = await CheckPermissionsAsync();
            if (hasPermission)
            {
                Media = new ObservableCollection<MediaFile>();
                await _multiMediaPickerService.PickPhotosAsync();
                MainCollectionView.ItemsSource = null;
                MainCollectionView.ItemsSource = Media;

            }
        }






        async Task<bool> CheckPermissionsAsync()
        {
            var retVal = false;
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Storage))
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "Need Storage permission to access to your photos.", "Ok");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Plugin.Permissions.Abstractions.Permission.Storage });
                    status = results[Plugin.Permissions.Abstractions.Permission.Storage];
                }

                if (status == PermissionStatus.Granted)
                {
                    retVal = true;

                }
                else if (status != PermissionStatus.Unknown)
                {
                    await App.Current.MainPage.DisplayAlert("Alert", "Permission Denied. Can not continue, try again.", "Ok");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await App.Current.MainPage.DisplayAlert("Alert", "Error. Can not continue, try again.", "Ok");
            }

            return retVal;

        }

    }
}
