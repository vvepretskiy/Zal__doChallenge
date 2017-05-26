using System.Collections.Generic;
using Newtonsoft.Json;

namespace Zal__doChallenge.Background.Model
{
	public sealed class FacetsFilterModel
	{
		[JsonProperty("filter")]
		public string Filter { get; set; }

		[JsonProperty("facets")]
		public IList<FacetsModel> Items { get; set; }
	}
}
