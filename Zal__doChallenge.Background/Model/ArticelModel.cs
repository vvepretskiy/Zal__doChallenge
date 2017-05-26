using System.Collections.Generic;
using Newtonsoft.Json;

namespace Zal__doChallenge.Background.Model
{
	public sealed class ArticelModel
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("brand")]
		public BrandModel Brand { get; set; }

		[JsonProperty("units")]
		public IList<UnitModel> Units { get; set; }

		[JsonProperty("media")]
		public MediaModel Media { get; set; }
	}
}
