using Newtonsoft.Json;

namespace Zal__doChallenge.Background.Model
{
	public sealed class UnitModel
	{
		[JsonProperty("size")]
		public string Size { get; set; }

		[JsonProperty("price")]
		public PriceModel Price { get; set; }
	}
}
