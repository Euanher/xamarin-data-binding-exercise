using FlagData;
using Xamarin.Forms;
using FlagFacts.Extensions;
using System;
using System.Collections;

namespace FlagFacts
{
    public partial class MainPage : ContentPage
    {
        FlagRepository repository;
        int currentFlag;

        public MainPage()
        {
            InitializeComponent();

            // Load our data
            repository = new FlagRepository();

            // Setup the view
            InitializeData();
        }

        public Flag CurrentFlag
        {
            get {
                return repository.Flags[currentFlag];
            }
        }

        private void InitializeData()
        {
            ItemsSource = (IList) repository.Countries;
            SelectedItem = CurrentFlag;
            SelectedIndexChanged += (s, e) => CurrentFlag = repository.Countries[SelectedIndex];

            flagImage.Source = CurrentFlag.GetImageSource();

            Date = CurrentFlag.Date;
            DateSelected += (s, e) => CurrentFlag.Date = e.NewDate;

            IsToggled = CurrentFlag.IncludesShield;
            Toggled += (s, e) => CurrentFlag.IncludesShield = IsToggled;

            Text = CurrentFlag;
        }

        private async void OnShow(object sender, EventArgs e)
        {
            await DisplayAlert(CurrentFlag.Country,
                $"{CurrentFlag.DateAdopted:D} - {CurrentFlag.IncludesShield}: {CurrentFlag.MoreInformationUrl}", 
                "OK");
        }

        private void OnMoreInformation(object sender, EventArgs e)
        {
            Device.OpenUri(CurrentFlag.MoreInformationUrl);
        }

        private void OnPrevious(object sender, EventArgs e)
        {
            currentFlag--;
            if (currentFlag < 0)
                currentFlag = 0;
            InitializeData();
        }

        private void OnNext(object sender, EventArgs e)
        {
            currentFlag++;
            if (currentFlag >= repository.Flags.Count)
                currentFlag = repository.Flags.Count-1;
            InitializeData();
        }
    }
}
