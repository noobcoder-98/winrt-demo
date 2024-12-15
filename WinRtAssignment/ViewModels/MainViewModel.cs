using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Storage;
using WinRtAssignment.Models;
using WinRtAssignment.Utils;

namespace WinRtAssignment.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty] private ObservableCollection<Note> _notes;
        [ObservableProperty] private string _log;
        private IDataService _service;
        public MainViewModel()
        {
            _service = new DataService();
            Notes = new ObservableCollection<Note>(SeedData.GenerateRandomNotes(15));
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
        }

        #region Events

        private void NetworkInformation_NetworkStatusChanged(object sender)
        {
            if (!IsConnectInternet())
            {
                return;
            }


        }

        #endregion

        #region Add Note

        [RelayCommand]
        private void AddNote()
        {

        }


        #endregion

        #region Package Data and Roaming

        [RelayCommand]
        private async Task SaveNotesAsync()
        {
            StorageFolder folder;
            if (IsConnectInternet())
            {
                folder = ApplicationData.Current.RoamingFolder;
            }
            else
            {
                folder = ApplicationData.Current.LocalFolder;
            }

            Log = folder.Path;
            await _service.SaveData(folder.Path + "\\test.json", Notes.ToList(), false);
        }

        

        private bool IsConnectInternet()
        {
            var profile = NetworkInformation.GetInternetConnectionProfile();
            return profile != null && profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
        }


        #endregion
    }
}
