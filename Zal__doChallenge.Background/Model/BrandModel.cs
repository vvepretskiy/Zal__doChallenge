using Newtonsoft.Json;

namespace Zal__doChallenge.Background.Model
{
	public sealed class BrandModel
	{
		[JsonProperty("name")]
		public string Name { get; set; }
	}
}
