using System.Threading.Tasks;
using MediaValet.Samples.SkyHOOK.WebApi.Models;

namespace MediaValet.Samples.SkyHOOK.WebApi.Hubs.Clients
{
	public interface IEventViewer
	{
		Task ReceiveCloudEvent(CloudEvent<AssetMediaFileAddedEvent> cloudEvent);
	}
}