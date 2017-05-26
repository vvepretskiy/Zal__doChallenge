using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Zal__doChallenge.Common
{
	public class IncrementalLoadingCollection<T> : ObservableCollection<T>,
		 ISupportIncrementalLoading
	{
		private readonly int _itemsPerPage;
		private bool _hasMoreItems;
		private int _currentPage;

		public IncrementalLoadingCollection(int itemsPerPage = 20)
		{
			this._itemsPerPage = itemsPerPage;
			this._hasMoreItems = true;
		}

		public void ResetPage()
		{
			_currentPage = 0;
		}

		public delegate Task<IEnumerable<T>> AsyncEventHandler(int page, int pageSize);

		public event AsyncEventHandler GetPagedItems;

		public bool HasMoreItems => _hasMoreItems;

		public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
		{
			var dispatcher = Window.Current.Dispatcher;

			return Task.Run<LoadMoreItemsResult>(
				async () =>
				{
					uint resultCount = 0;
					if (GetPagedItems != null)
					{
						var result = await GetPagedItems(++_currentPage, _itemsPerPage);

						if (result == null || !result.Any())
						{
							_hasMoreItems = false;
						}
						else
						{
							resultCount = (uint)result.Count();

							await dispatcher.RunAsync(
								CoreDispatcherPriority.Normal,
								() =>
								{
									foreach (T item in result)
										this.Add(item);
								});
						}
					}

					return new LoadMoreItemsResult() { Count = resultCount };

				}).AsAsyncOperation<LoadMoreItemsResult>();
		}
	}
}
