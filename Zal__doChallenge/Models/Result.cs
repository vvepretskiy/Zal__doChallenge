using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MetroLog;
using Newtonsoft.Json;
using Zal__doChallenge.Common;
using Zal__doChallenge.Services;
using Zal__doChallenge.Shared.Logger;
using Zal__doChallenge.Shared.Model;

namespace Zal__doChallenge.Models
{
	public class Result : BindableBase, IDisposable
	{
		private readonly Zal__doService _service;

		public string BrandName { get; set; }

		public string BrandId { get; set; }

		public bool IsMen { get; set; }

		public IncrementalLoadingCollection<ArticelModel> Articels { get; }
			= new IncrementalLoadingCollection<ArticelModel>(10);

		public Result(Zal__doService service)
		{
			_service = service;
			Articels.GetPagedItems += GetPagedItems;
		}

		public void Dispose()
		{
			_service.CloseConnection();
		}

		public async Task<IEnumerable<ArticelModel>> GetPagedItems(int pageIndex, int pageSize)
		{
			try
			{
				if (!_service.IsConnected)
				{
					await _service.Open();
				}

				var request = new Dictionary<string, string>
				{
					{"command", "getArticels"},
					{"page", pageIndex.ToString()},
					{"pageSize", pageSize.ToString()},
					{"gender", IsMen ? "male" : "female"}
				};

				if (!String.IsNullOrEmpty(BrandId))
				{
					request.Add("brand", BrandId);
				}
				else if (!String.IsNullOrEmpty(BrandName))
				{
					request.Add("brandFamily", BrandName);
				}

				var response = await _service.SendMessageAsync(request);

				return JsonConvert.DeserializeObject<List<ArticelModel>>(response["response"] as string);
			}
			catch (Exception e)
			{
				LoggingService.Instance.WriteLine<Result>("Items from service couldn't been reached", LogLevel.Error, e);
			}
			return new List<ArticelModel>();
		}
	}
}
