using System;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;
using MetroLog;
using Newtonsoft.Json;
using Zal__doChallenge.Shared.Logger;

namespace Zal__doChallenge.Background
{
	public sealed class ApiZal__do : IBackgroundTask
	{
		private BackgroundTaskDeferral _deferral;
		private AppServiceConnection _appServiceConnection;

		public void Run(IBackgroundTaskInstance taskInstance)
		{
			_deferral = taskInstance.GetDeferral();

			taskInstance.Canceled += TaskInstance_Canceled;

			var trigger = taskInstance.TriggerDetails as AppServiceTriggerDetails;
			_appServiceConnection = trigger.AppServiceConnection;
			_appServiceConnection.RequestReceived += AppServiceConnection_RequestReceived;
		}

		private async void AppServiceConnection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
		{
			var deferral = args.GetDeferral();
			string command = String.Empty;
			try
			{
				var requestMessage = args.Request.Message;
				command = requestMessage["command"] as string;
				var responseMessage = new ValueSet();

				switch (command)
				{
					case "getBrands":
					{
						var result = await HttpClientHelper.Instance.GetBrands(requestMessage);
						responseMessage.Add("response", JsonConvert.SerializeObject(result));
					}
						break;
					case "getArticels":
					{
						var result = await HttpClientHelper.Instance.GetArticels(requestMessage);
						responseMessage.Add("response", JsonConvert.SerializeObject(result));
					}
						break;
				}

				await args.Request.SendResponseAsync(responseMessage);
			}
			catch (Exception e)
			{
				LoggingService.Instance.WriteLine<ApiZal__do>($"Request {command} has not been successful", LogLevel.Error, e);
			}
			finally
			{
				deferral.Complete();
			}
		}

		private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
		{
			if (_deferral != null)
			{
				//Complete the service deferral
				_deferral.Complete();
				_deferral = null;
			}
		}
	}
}
