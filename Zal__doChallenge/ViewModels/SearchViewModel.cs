using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using MetroLog;
using Zal__doChallenge.Models;
using Zal__doChallenge.NotificationData;
using Zal__doChallenge.Shared.Logger;
using Zal__doChallenge.Shared.Model;

namespace Zal__doChallenge.ViewModels
{
	public class SearchViewModel : BaseViewModel
	{
		private readonly INavigationService _navigationService;

		private Search _model;
		public Search Model
		{
			get { return _model; }
			set
			{
				if (_model != value)
				{
					_model = value;
					RaisePropertyChanged();
				}
			}
		}

		private bool _isBusy = true;
		public bool IsBusy
		{
			get { return _isBusy; }
			set
			{
				if (_isBusy != value)
				{
					_isBusy = value;
					RaisePropertyChanged();
				}
			}
		}

		private ObservableCollection<BrandModel> _filteredBrands;
		public ObservableCollection<BrandModel> FilteredBrands
		{
			get { return _filteredBrands; }
			set { _filteredBrands = value; RaisePropertyChanged(); }
		}

		// TODO: This is quick implementation, but better to use commandBehaviour or click on item
		private BrandModel _selectedBrandModel = null;
		public BrandModel SelectedBrandModel
		{
			get { return _selectedBrandModel; }
			set
			{
				_selectedBrandModel = value;
				if (_selectedBrandModel != null)
				{
					NavigateToResult();
				}
			}
		}

		public SearchViewModel(INavigationService navigationService, Search model)
		{
			_navigationService = navigationService;
			Model = model;
			Model.PropertyChanged += Model_PropertyChanged;
		}

		private void RefreshFilteredData()
		{
			try
			{
				IsBusy = true;

				if (String.IsNullOrEmpty(Model.Text) || !Model.Suggetions.Any())
				{
					FilteredBrands = new ObservableCollection<BrandModel>();
					return;
				}

				var sug = Model.Suggetions
					.Where(x => x.Name.StartsWith(Model.Text, StringComparison.OrdinalIgnoreCase));

				// This will limit the amount of view refreshes 
				if (FilteredBrands.Count == sug.Count())
					return;

				FilteredBrands = new ObservableCollection<BrandModel>(sug);
			}
			catch (Exception e)
			{
				LoggingService.Instance.WriteLine<SearchViewModel>("Suggestion filter has not been executed", LogLevel.Error, e);
			}
			finally
			{
				IsBusy = false;
			}
		}

		private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Text" || e.PropertyName == "Suggetions")
			{
				RefreshFilteredData();
			}
		}

		public ICommand LoadResult => new RelayCommand(NavigateToResult);

		private void NavigateToResult()
		{
			var searchData = new SearchData
			{
				IsMen = Model.IsMen,
				BrandName = Model.Text,
				BrandId = SelectedBrandModel?.Id
			};
			SelectedBrandModel = null;
			_navigationService.NavigateTo("ResultPage");

			Messenger.Default.Send(new NotificationMessage<SearchData>(searchData, SearchData.SearchRequestMessage));
		}

		public override void Cleanup()
		{
			Model.Dispose();
			base.Cleanup();
		}
	}
}

