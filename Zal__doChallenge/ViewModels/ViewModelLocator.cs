using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Zal__doChallenge.Models;
using Zal__doChallenge.Services;
using Zal__doChallenge.Views;

namespace Zal__doChallenge.ViewModels
{
	public class ViewModelLocator
	{
		public SearchViewModel SearchViewModel => ServiceLocator.Current.GetInstance<SearchViewModel>();
		public ResultViewModel ResultViewModel => ServiceLocator.Current.GetInstance<ResultViewModel>();

		static ViewModelLocator()
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			SimpleIoc.Default.Register<INavigationService, NavigationServiceHelper>();
			SimpleIoc.Default.Register<SearchViewModel>();
			SimpleIoc.Default.Register<ResultViewModel>();
			SimpleIoc.Default.Register<Zal__doService>();
			SimpleIoc.Default.Register<Search>();
			SimpleIoc.Default.Register<Result>();
		}

		public static void Cleanup()
		{
			SimpleIoc.Default.Unregister<INavigationService>();
			SimpleIoc.Default.Unregister<SearchViewModel>();
			SimpleIoc.Default.Unregister<ResultViewModel>();
			SimpleIoc.Default.Unregister<Zal__doService>();
			SimpleIoc.Default.Unregister<Search>();
			SimpleIoc.Default.Unregister<Result>();

			Messenger.Reset();
		}
	}

	public class NavigationServiceHelper : NavigationService
	{
		public NavigationServiceHelper()
		{
			this.Configure("SearchPage", typeof(SearchPage));
			this.Configure("ResultPage", typeof(ResultPage));
		}
	}
}
