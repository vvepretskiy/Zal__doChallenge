using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using MetroLog;
using Zal__doChallenge.Shared.Logger;

namespace Zal__doChallenge.Services
{
	public class Zal__doService
	{
		const string AppServiceName = "com.Zal--doChallenge";
		private AppServiceConnection _connection;
		public event Action<ValueSet> OnMessageReceived;

		public bool IsConnected => _connection != null;

		private async Task<AppServiceConnection> CachedConnection()
		{
			if (_connection != null) return _connection;
			_connection = await MakeConnection();
			_connection.RequestReceived += ConnectionOnRequestReceived;
			_connection.ServiceClosed += ConnectionOnServiceClosed;
			return _connection;
		}

		public async Task Open()
		{
			await CachedConnection();
		}

		private async Task<AppServiceConnection> MakeConnection()
		{
			var listing = await AppServiceCatalog.FindAppServiceProvidersAsync(AppServiceName);

			if (listing.Count == 0)
			{
				LoggingService.Instance.WriteLine<Zal__doService>("Unable to find app service '" + AppServiceName + "'", LogLevel.Error);
			}
			var packageName = listing[0].PackageFamilyName;

			var connection = new AppServiceConnection
			{
				AppServiceName = AppServiceName,
				PackageFamilyName = packageName
			};

			var status = await connection.OpenAsync();

			if (status != AppServiceConnectionStatus.Success)
			{
				LoggingService.Instance.WriteLine<Zal__doService>("Could not connect to MessageRelay, status: " + status, LogLevel.Error);
			}

			return connection;
		}

		private void ConnectionOnServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
		{
			DisposeConnection();
		}

		private void DisposeConnection()
		{
			if (_connection == null) return;

			_connection.RequestReceived -= ConnectionOnRequestReceived;
			_connection.ServiceClosed -= ConnectionOnServiceClosed;
			_connection.Dispose();
			_connection = null;
		}

		private void ConnectionOnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
		{
			var appServiceDeferral = args.GetDeferral();
			try
			{
				ValueSet valueSet = args.Request.Message;
				OnMessageReceived?.Invoke(valueSet);
			}
			finally
			{
				appServiceDeferral.Complete();
			}
		}

		public void CloseConnection()
		{
			DisposeConnection();
		}

		private async Task<ValueSet> SendMessageAsync(KeyValuePair<string, object> keyValuePair)
		{
			return await SendMessageAsync(new ValueSet { keyValuePair });
		}

		private async Task<ValueSet> SendMessageAsync(ValueSet keyValuePair)
		{
			var connection = await CachedConnection();
			var result = await connection.SendMessageAsync(keyValuePair);
			if (result.Status == AppServiceResponseStatus.Success)
			{
				return result.Message;
			}

			LoggingService.Instance.WriteLine<Zal__doService>("Error sending " + result.Status, LogLevel.Error);

			return null;
		}

		public async Task<ValueSet> SendMessageAsync(string key, string value)
		{
			return await SendMessageAsync(new KeyValuePair<string, object>(key, value));
		}

		public async Task<ValueSet> SendMessageAsync(IDictionary<string, string> dict)
		{
			var set = new ValueSet();
			foreach (KeyValuePair<string, string> pair in dict)
			{
				set.Add(pair.Key, pair.Value);
			}
			return await SendMessageAsync(set);
		}
	}
}
