using System;
using System.Collections.Generic;
using MetroLog;
using Newtonsoft.Json;
using Zal__doChallenge.Services;
using Zal__doChallenge.Shared.Logger;
using Zal__doChallenge.Shared.Model;

namespace Zal__doChallenge.Models
{
	public class Search : BindableBase, IDisposable
	{
		private readonly Zal__doService _service;
		
		private readonly ICollection<BrandModel> _suggetions;

		public ICollection<BrandModel> Suggetions
		{
			get
			{
				lock (_suggetions)
				{
					return _suggetions;
				}
			}
		}

		private string _text;
		public string Text
		{
			get { return _text; }
			set
			{
				if (_text != value)
				{
					_text = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _isMen;
		public bool IsMen
		{
			get { return _isMen; }
			set
			{
				if (_isMen != value)
				{
					_isMen = value;
					OnPropertyChanged();
					GetBrands();
				}
			}
		}

		public Search(Zal__doService service)
		{
			_service = service;
			 _suggetions = new HashSet<BrandModel>();
			GetBrands();
		}

		private async void GetBrands()
		{
			try
			{
				Suggetions.Clear();
				OnPropertyChanged("Suggetions");

				if (!_service.IsConnected)
				{
					await _service.Open();
				}

				var response = await _service.SendMessageAsync(new Dictionary<string, string>
				{
					{"command", "getBrands"},
					{"gender", IsMen ? "male" : "female"}
				});

				foreach (BrandModel model in JsonConvert.DeserializeObject<List<BrandModel>>(response["response"] as string))
				{
					Suggetions.Add(model);
				}
				OnPropertyChanged("Suggetions");
			}
			catch (Exception e)
			{
				LoggingService.Instance.WriteLine<Search>("Brands from service couldn't been reached", LogLevel.Error, e);
			}
		}

		public void Dispose()
		{
			_service.CloseConnection();
		}
	}
}
