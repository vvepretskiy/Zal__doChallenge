using Newtonsoft.Json;

namespace Zal__doChallenge.Background.Model
{
	public sealed class FacetsModel
	{
		[JsonProperty("key")]
		public string Id { get; set; }

		[JsonProperty("displayName")]
		public string Name { get; set; }
	}
}
