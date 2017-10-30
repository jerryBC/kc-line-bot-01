using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
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
	public class LineMessagesController : Controller
	{
		private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            return client;
        }
	
		/// <summary>
		/// POST: api/Messages
		/// Receive a message from a user and reply to it
		/// </summary>
		public async Task<HttpResponseMessage> Post()
		{
			//HttpRequestMessage request
			//if (!await VaridateSignature(request))
			//	return new HttpResponseMessage(HttpStatusCode.BadRequest);
			Activity activity = new Activity();
			using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
			{
				activity = JsonConvert.DeserializeObject < Activity > (await reader.ReadToEndAsync());
			}
			

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
                    StringContent content = new StringContent(
                        JsonConvert.SerializeObject(activity),
                        Encoding.UTF8, "application/json");
						
                        await client.PostAsync("https://13.113.36.152/api/LineMessages", content);
						
                    
                }
            }
			return new HttpResponseMessage(HttpStatusCode.OK);

			
		}

		private async Task<bool> VaridateSignature(HttpRequestMessage request)
		{
			var hmac = new HMACSHA256(Encoding.UTF8.GetBytes("9833045df93888c8ed9f4924389d9cdc"));
			var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(await request.Content.ReadAsStringAsync()));
			var contentHash = Convert.ToBase64String(computeHash);
			var headerHash = Request.Headers["X-Line-Signature"];
			//Request.Headers.GetValues("X-Line-Signature").First();

			return contentHash == headerHash;
		}
	}

	public class LineMessageHandler
	{

		private Event lineEvent;
		private LineClient lineClient = new LineClient(@"VC/sPEIz8O7WYo0THlaM5laorgw+GifJmN8cFR5eZ0seYxNsR3ZOVblQgOeI8xNaCOyTsUz2VsajbtAyt8hj7+NdP2/oYB+7eQ/FGnEAN/ICGCPj5nX36E848piCYi16BPkXlDR3N0CDiPnrfbpPcAdB04t89/1O/w1cDnyilFU="); //ConfigurationManager.AppSettings["ChannelToken"].ToString());

		public LineMessageHandler(Event lineEvent)
		{
			this.lineEvent = lineEvent;
		}

		public async Task HandleBeaconEvent()
		{
		}

		public async Task HandleFollowEvent()
		{
		}

		public async Task HandleJoinEvent()
		{
		}

		public async Task HandleLeaveEvent()
		{
		}

		public async Task HandlePostbackEvent()
		{
			var replyMessage = new TextMessage(lineEvent.Postback.Data);
			await Reply(replyMessage);
		}

		public async Task HandleUnfollowEvent()
		{
		}

		public async Task<Profile> GetProfile(string mid)
		{
			return await lineClient.GetProfile(mid);
		}

		public async Task HandleTextMessage()
		{
			var textMessage = JsonConvert.DeserializeObject<TextMessage>(lineEvent.Message.ToString());
			Message replyMessage = null;
			if (textMessage.Text.ToLower() == "buttons")
			{
				List<TemplateAction> actions = new List<TemplateAction>();
				actions.Add(new MessageTemplateAction("Message Label", "sample data"));
				actions.Add(new PostbackTemplateAction("Postback Label", "sample data", "sample data"));
				actions.Add(new UriTemplateAction("Uri Label", "https://github.com/kenakamu"));
				ButtonsTemplate buttonsTemplate = new ButtonsTemplate("https://github.com/apple-touch-icon.png", "Sample Title", "Sample Text", actions);

				replyMessage = new TemplateMessage("Buttons", buttonsTemplate);
			}
			else if (textMessage.Text.ToLower() == "confirm")
			{
				List<TemplateAction> actions = new List<TemplateAction>();
				actions.Add(new MessageTemplateAction("Yes", "yes"));
				actions.Add(new MessageTemplateAction("No", "no"));
				ConfirmTemplate confirmTemplate = new ConfirmTemplate("Confirm Test", actions);
				replyMessage = new TemplateMessage("Confirm", confirmTemplate);
			}
			else if (textMessage.Text.ToLower() == "carousel")
			{
				List<TemplateColumn> columns = new List<TemplateColumn>();
				List<TemplateAction> actions = new List<TemplateAction>();
				actions.Add(new MessageTemplateAction("Message Label", "sample data"));
				actions.Add(new PostbackTemplateAction("Postback Label", "sample data", "sample data"));
				actions.Add(new UriTemplateAction("Uri Label", "https://github.com/kenakamu"));
				columns.Add(new TemplateColumn() { Title = "Casousel 1 Title", Text = "Casousel 1 Text", ThumbnailImageUrl = "https://github.com/apple-touch-icon.png", Actions = actions });
				columns.Add(new TemplateColumn() { Title = "Casousel 2 Title", Text = "Casousel 2 Text", ThumbnailImageUrl = "https://github.com/apple-touch-icon.png", Actions = actions });
				CarouselTemplate carouselTemplate = new CarouselTemplate(columns);
				replyMessage = new TemplateMessage("Carousel", carouselTemplate);
			}
			else if (textMessage.Text.ToLower() == "imagemap")
			{

				var imageUrl = "";
				List<ImageMapAction> actions = new List<ImageMapAction>();
				actions.Add(new UriImageMapAction("http://github.com", new ImageMapArea(0, 0, 520, 1040)));
				actions.Add(new MessageImageMapAction("I love LINE!", new ImageMapArea(520, 0, 520, 1040)));
				replyMessage = new ImageMapMessage(imageUrl, "GitHub", new BaseSize(1040, 1040), actions);
			}
			else
			{
				replyMessage = new TextMessage(textMessage.Text);
			}
			await Reply(replyMessage);
		}

		public string MyMethod(HttpContext context)
		{
			var imageUrl = $"{context.Request.Scheme}://{context.Request.Host}/images/githubicon";
			return imageUrl;
		}

		public async Task HandleMediaMessage()
		{
			Message message = JsonConvert.DeserializeObject<Message>(lineEvent.Message.ToString());
			// Get media from Line server.
			Media media = await lineClient.GetContent(message.Id);
			Message replyMessage = null;

			// Reply Image 
			switch (message.Type)
			{
				case MessageType.Image:
				case MessageType.Video:
				case MessageType.Audio:
					replyMessage = new ImageMessage("https://github.com/apple-touch-icon.png", "https://github.com/apple-touch-icon.png");
					break;
			}

			await Reply(replyMessage);
		}

		public async Task HandleStickerMessage()
		{
			//https://devdocs.line.me/files/sticker_list.pdf
			var stickerMessage = JsonConvert.DeserializeObject<StickerMessage>(lineEvent.Message.ToString());
			var replyMessage = new StickerMessage("1", "1");
			await Reply(replyMessage);
		}

		public async Task HandleLocationMessage()
		{
			var locationMessage = JsonConvert.DeserializeObject<LocationMessage>(lineEvent.Message.ToString());
			LocationMessage replyMessage = new LocationMessage(
				locationMessage.Title,
				locationMessage.Address,
				locationMessage.Latitude,
				locationMessage.Longitude);
			await Reply(replyMessage);
		}

		private async Task Reply(Message replyMessage)
		{
			try
			{
				await lineClient.ReplyToActivityAsync(lineEvent.CreateReply(message: (TextMessage)replyMessage));
			}
			catch
			{
				
				await lineClient.PushAsync(lineEvent.CreatePush(message: (TextMessage)replyMessage));
			}
		}
		
		
	}
}