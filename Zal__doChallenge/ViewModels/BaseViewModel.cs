using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace Zal__doChallenge.ViewModels
{
	public abstract class BaseViewModel : ViewModelBase
	{
		public const string CleanUpMessage = "CleanUp";

		protected BaseViewModel()
		{
			Messenger.Default.Send(new NotificationMessage(this, CleanUpMessage));
		}
	}
}
