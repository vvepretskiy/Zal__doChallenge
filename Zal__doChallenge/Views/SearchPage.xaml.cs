using Windows.Graphics.Display;
using Windows.System.Profile;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Zal__doChallenge.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class SearchPage : Page
	{
		private readonly InputPane _inputPane;

		public SearchPage()
		{
			this.InitializeComponent();

			_inputPane = InputPane.GetForCurrentView();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			_inputPane.Hiding += OnKeyboardHiding;
			_inputPane.Showing += OnKeyboardShowing;

			if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
			{
				DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
			}
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);

			_inputPane.Hiding -= OnKeyboardHiding;
			_inputPane.Showing -= OnKeyboardShowing;

			DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;
		}

		// TODO: The best way to write common behavior for every page
		private void OnKeyboardShowing(InputPane sender, InputPaneVisibilityEventArgs args)
		{
			args.EnsuredFocusedElementInView = true;
			this.Margin = new Thickness(0, 0, 0, args.OccludedRect.Height);
		}

		private void OnKeyboardHiding(InputPane sender, InputPaneVisibilityEventArgs args)
		{
			this.Margin = new Thickness();
		}
	}
}
