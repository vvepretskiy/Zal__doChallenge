using Newtonsoft.Json;

namespace Zal__doChallenge.Background.Model
{
	public sealed class PriceModel
	{
		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("value")]
		public double Cost { get; set; }
	}
}
