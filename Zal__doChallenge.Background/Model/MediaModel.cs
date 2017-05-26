using System.Collections.Generic;
using Newtonsoft.Json;

namespace Zal__doChallenge.Background.Model
{
	public sealed class MediaModel
	{
		[JsonProperty("images")]
		public IList<ImageModel> Images { get; set; }
	}
}
