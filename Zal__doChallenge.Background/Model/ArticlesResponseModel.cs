using System.Collections.Generic;
using Newtonsoft.Json;

namespace Zal__doChallenge.Background.Model
{
	public sealed class ArticlesResponseModel
	{
		[JsonProperty("content")]
		public IList<ArticelModel> Articels { get; set; }
	}
}
