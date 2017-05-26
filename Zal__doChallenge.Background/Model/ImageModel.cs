using Newtonsoft.Json;

namespace Zal__doChallenge.Background.Model
{
	public sealed class ImageModel
	{
		[JsonProperty("smallUrl")]
		public string Url { get; set; }
	}
}
