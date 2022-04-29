using MediaValet.Samples.SkyHOOK.WebApi.Hubs;
using MediaValet.Samples.SkyHOOK.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace MediaValet.Samples.SkyHOOK.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {

        #region Data Members

        private bool EventTypeSubcriptionValidation
            => HttpContext.Request.Headers["aeg-event-type"].FirstOrDefault() ==
               "SubscriptionValidation";

        private bool EventTypeNotification
            => HttpContext.Request.Headers["aeg-event-type"].FirstOrDefault() ==
               "Notification";

        private readonly IHubContext<CloudEventsHub> _hubContext;

        #endregion
        
        public EventsController(IHubContext<CloudEventsHub> cloudEventsHubContext)
        {
            this._hubContext = cloudEventsHubContext;
        }

        #region Public Methods

        [HttpOptions]
        public async Task<IActionResult> Options()
        {
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var headers = new Dictionary<string, string>();
                var webhookRequestOrigin = HttpContext.Request.Headers["WebHook-Request-Origin"].FirstOrDefault();
                headers.Add("WebHook-Request-Origin", webhookRequestOrigin);
                var webhookRequestCallback = HttpContext.Request.Headers["WebHook-Request-Callback"];
                headers.Add("WebHook-Request-Callback", webhookRequestCallback);
                var webhookRequestRate = HttpContext.Request.Headers["WebHook-Request-Rate"];
                headers.Add("WebHook-Request-Rate", webhookRequestRate);
                HttpContext.Response.Headers.Add("WebHook-Allowed-Rate", "*");
                HttpContext.Response.Headers.Add("WebHook-Allowed-Origin", webhookRequestOrigin);
                await this._hubContext.Clients.All.SendAsync(
                    "gridupdate",
                    Guid.Empty.ToString(),
                    "Options",
                    "Handshake",
                    DateTime.Now.ToLongTimeString(),
                    JsonConvert.SerializeObject(headers));
            }

            return Ok();
        }

        [HttpPost]
        
        public async Task<IActionResult> Post()
        {
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var jsonContent = await reader.ReadToEndAsync();


                // Check to see if this is passed in using
                // the CloudEvents schema
                if (IsCloudEvent(jsonContent))
                {
                    return await HandleCloudEvent(jsonContent);
                }

                return BadRequest();
            }
        }

        #endregion

        #region Private Methods

        private async Task<IActionResult> HandleCloudEvent(string jsonContent)
        {
            var details = JsonConvert.DeserializeObject<CloudEvent<dynamic>>(jsonContent);
            var eventData = JObject.Parse(jsonContent);

            await this._hubContext.Clients.All.SendAsync(
                "CloudEventTriggered",
                details.Id,
                details.Type,
                details.Subject,
                details.Time,
                eventData.ToString()
            );

            return Ok();
        }

        private static bool IsCloudEvent(string jsonContent)
        {
            // Cloud events are sent one at a time, while Grid events
            // are sent in an array. As a result, the JObject.Parse will 
            // fail for Grid events. 
            try
            {
                // Attempt to read one JSON object. 
                var eventData = JObject.Parse(jsonContent);

                // Check for the spec version property.
                var version = eventData["specversion"].Value<string>();
                if (!string.IsNullOrEmpty(version)) return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        #endregion

    }
}
