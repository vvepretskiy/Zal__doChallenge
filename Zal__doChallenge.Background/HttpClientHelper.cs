using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using MetroLog;
using Newtonsoft.Json;
using Zal__doChallenge.Background.Model;
using Zal__doChallenge.Shared.Logger;
using ArticelModel = Zal__doChallenge.Shared.Model.ArticelModel;

namespace Zal__doChallenge.Background
{
	public sealed class HttpClientHelper
	{
		private static HttpClientHelper _instance;
		private static readonly object _lock = new object();

		public static HttpClientHelper Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (_lock)
					{
						if (_instance == null)
						{
							_instance = new HttpClientHelper();
						}
					}
				}
				return _instance;
			}
		}

		private async Task<T> Get<T>(string url)
		{
			try
			{
				using (var client = new HttpClient())
				{
					HttpResponseMessage responseGet = await client.GetAsync(url);
					if (responseGet.IsSuccessStatusCode)
					{
						string response = await responseGet.Content.ReadAsStringAsync();
						return JsonConvert.DeserializeObject<T>(response);
					}
				}
			}
			catch (Exception e)
			{
				LoggingService.Instance.WriteLine<HttpClientHelper>($"Request {url} has not been successful", LogLevel.Error, e);
			}
			return default(T);
		}

		internal async Task<IEnumerable<Shared.Model.BrandModel>> GetBrands(ValueSet request)
		{
			string url = "https://api.zalando.com/facets";
			url += GetParamByKey(request, "gender", "?gender={0}");

			var items = await Get<IList<FacetsFilterModel>>(url);
			return items?.FirstOrDefault(x => x.Filter == "brand").Items.Select(y => new Shared.Model.BrandModel { Id = y.Id, Name = y.Name});
		}

		private string GetParamByKey(ValueSet set, string key, string format)
		{
			string value = null;
			if (set.ContainsKey(key))
			{
				value = set[key] as string;
			}

			return !String.IsNullOrEmpty(value) ? String.Format(format, value) : String.Empty;
		}

		internal async Task<IEnumerable<ArticelModel>> GetArticels(ValueSet request)
		{
			StringBuilder builder = new StringBuilder();
			builder.Append(GetParamByKey(request, "gender", "&gender={0}"));
			builder.Append(GetParamByKey(request, "pageSize", "&pageSize={0}"));
			builder.Append(GetParamByKey(request, "brandFamily", "&brandfamily={0}"));
			builder.Append(GetParamByKey(request, "brand", "&brand={0}"));
			builder.Append(GetParamByKey(request, "category", "&category={0}"));
			builder.Append(GetParamByKey(request, "page", "&page={0}"));

			string url = "https://api.zalando.com/articles?fields=id%2C%20name%2C%20brand.key%2C%20brand.name%2C%20units.size%2C%20units.price.currency%2C%20units.price.value%2C%20media.images.smallUrl";

			url += builder.ToString();
			
			var response = await Get<ArticlesResponseModel>(url);
			IList<ArticelModel> articels = new List<ArticelModel>();
			foreach (Model.ArticelModel articel in response.Articels)
			{
				UnitModel unit = articel.Units.FirstOrDefault();
				articels.Add(new ArticelModel
				{
					BrandName = articel.Brand.Name,
					Name = articel.Name,
					Currency = unit.Price.Currency,
					ImgUrl = articel.Media.Images.FirstOrDefault().Url,
					Price = unit.Price.Cost,
					Size = unit.Size
				});
				//foreach (var unit in articel.Units)
				//{
				//	articels.Add(new ArticelModel
				//	{
				//		BrandName = articel.Brand.Name,
				//		Name = articel.Name,
				//		Currency = unit.Price.Currency,
				//		ImgUrl = articel.Media.Images.FirstOrDefault().Url,
				//		Price = unit.Price.Cost,
				//		Size = unit.Size
				//	});
				//}
			}
			return articels;
		}
	}
}
