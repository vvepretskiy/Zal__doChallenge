using System;
using GalaSoft.MvvmLight.Messaging;
using Zal__doChallenge.Models;
using Zal__doChallenge.NotificationData;

namespace Zal__doChallenge.ViewModels
{
	public class ResultViewModel : BaseViewModel
	{
		private Result _model;

		public Result Model
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

		public ResultViewModel(Result model)
		{
			Model = model;

			Messenger.Default.Register<NotificationMessage<SearchData>>(this, OnSearchTransmitted);
		}

		private async void OnSearchTransmitted(NotificationMessage<SearchData> message)
		{
			if (message.Notification == SearchData.SearchRequestMessage)
			{
				try
				{
					IsBusy = true;
					SearchData data = message.Content;
					Model.IsMen = data.IsMen;
					Model.BrandName = data.BrandName;
					Model.BrandId = data.BrandId;
					Model.Articels.ResetPage();
					Model.Articels.Clear();
					await Model.Articels.LoadMoreItemsAsync(0);
				}
				finally
				{
					IsBusy = false;
				}
			}
		}

		public override void Cleanup()
		{
			Model.Dispose();
			base.Cleanup();
		}
	}
}
