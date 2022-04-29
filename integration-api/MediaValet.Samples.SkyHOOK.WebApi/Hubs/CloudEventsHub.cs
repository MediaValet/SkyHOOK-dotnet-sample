using MediaValet.Samples.SkyHOOK.WebApi.Hubs.Clients;
using Microsoft.AspNetCore.SignalR;

namespace MediaValet.Samples.SkyHOOK.WebApi.Hubs
{
    public class CloudEventsHub : Hub<IEventViewer>
    {
        public CloudEventsHub()
        {
        }
    }
}
