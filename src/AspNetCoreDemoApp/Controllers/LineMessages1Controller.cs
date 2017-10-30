using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using AspNetCoreDemoApp.Models;
using System.IO;

namespace AspNetCoreDemoApp.Controllers

{
	[Route("api/[controller]")]
	public class LineMessages1Controller : Controller
	{
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post()
        {
            ////HttpRequestMessage request
            ////if (!await VaridateSignature(request))
            ////	return new HttpResponseMessage(HttpStatusCode.BadRequest);
            //Activity activity = new Activity();
            //using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            //{
            //	activity = JsonConvert.DeserializeObject < Activity > (await reader.ReadToEndAsync());
            //}


            //// Line may send multiple events in one message, so need to handle them all.
            //foreach (Event lineEvent in activity.Events)
            //{
            //	LineMessageHandler handler = new LineMessageHandler(lineEvent);

            //	Profile profile = await handler.GetProfile(lineEvent.Source.UserId);

            //	switch (lineEvent.Type)
            //	{
            //		case EventType.Beacon:
            //			await handler.HandleBeaconEvent();
            //			break;
            //		case EventType.Follow:
            //			await handler.HandleFollowEvent();
            //			break;
            //		case EventType.Join:
            //			await handler.HandleJoinEvent();
            //			break;
            //		case EventType.Leave:
            //			await handler.HandleLeaveEvent();
            //			break;
            //		case EventType.Message:
            //			Message message = JsonConvert.DeserializeObject<Message>(lineEvent.Message.ToString());
            //			switch (message.Type)
            //			{
            //				case MessageType.Text:
            //					await handler.HandleTextMessage();
            //					break;
            //				case MessageType.Audio:
            //				case MessageType.Image:
            //				case MessageType.Video:
            //					await handler.HandleMediaMessage();
            //					break;
            //				case MessageType.Sticker:
            //					await handler.HandleStickerMessage();
            //					break;
            //				case MessageType.Location:
            //					await handler.HandleLocationMessage();
            //					break;
            //			}
            //			break;
            //		case EventType.Postback:
            //			await handler.HandlePostbackEvent();
            //			break;
            //		case EventType.Unfollow:
            //			await handler.HandleUnfollowEvent();
            //			break;
            //	}
            //}

            //return new HttpResponseMessage(HttpStatusCode.OK);

            using (HttpClient client = GetClient())
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    //JsonSerializerSettings settings = new JsonSerializerSettings()
                    //{
                    //    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    //    NullValueHandling = NullValueHandling.Ignore
                    //};

                    //settings.Converters.Add(new StringEnumConverter(true));

                    //StringContent content = new StringContent(
                    //    JsonConvert.SerializeObject(reader.ReadToEndAsync(), settings),
                    //    Encoding.UTF8, "application/json");
                    //var result = await client.PostAsync("https://13.113.36.152/api/LineMessages", content);
                    
                        await client.PostAsync("https://13.113.36.152/api/LineMessages", reader.ReadToEndAsync());
						return new HttpResponseMessage(HttpStatusCode.OK);
                    
                }


            }
        }
	}
}