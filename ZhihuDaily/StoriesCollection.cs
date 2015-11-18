using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using Windows.Web.Http;

namespace ZhihuDaily
{
    public sealed class MainPageStoriesCollection : ObservableCollection<ObservableCollection<StoryItem>>, ISupportIncrementalLoading
    {
        /// <summary>
        /// 加载操作是否正在运行
        /// </summary>
        bool isLoading = false;
        /// <summary>
        /// 最大加载数
        /// </summary>
        const uint MAX_ITEM_COUNT = 20;
        /// <summary>
        /// 新闻列表信息
        /// </summary>
        public ObservableCollection<StoryItem> storyItems = null;

        #region 实现ISupportIncrementalLoading接口的成员
        public bool HasMoreItems
        {
            get
            {
                if (this.Count >= MAX_ITEM_COUNT)
                {
                    return false;
                }
                return true;
            }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            if (isLoading)
            {
                throw new InvalidOperationException("当前正在加载，请等待本次加载完成后再试");
            }
            uint x = MAX_ITEM_COUNT - (uint)this.Count;
            uint itemCountToLoad = 0;
            if (x < count)
            {
                itemCountToLoad = x;
            }
            else
            {
                itemCountToLoad = count;
            }

            return AsyncInfo.Run(c => LoadMoreItemsPrivateAsync(c, itemCountToLoad));
        }
        #endregion

        private async Task<LoadMoreItemsResult> LoadMoreItemsPrivateAsync(CancellationToken ct, uint n)
        {
            isLoading = true;
            if(LoadItemsStarted != null)
            {
                LoadItemsStarted(this, EventArgs.Empty);
            }

            uint hadLoaded = 0;
            storyItems = new ObservableCollection<StoryItem>();

            for (uint i = 0; i < n; i++)
            {
                
                int howManyDaysBefore = 0 - this.Count;
                string date = DateTime.Now.AddDays(howManyDaysBefore).ToString("yyyy年MM月dd日");
                string stringDate = date.Replace("年", "").Replace("月", "").Replace("日", "");
                Uri sourceUri = new Uri("http://news.at.zhihu.com/api/4/news/before/" + stringDate);

                #region 获取新闻列表
                HttpClient client = new HttpClient();
                string stringData = await client.GetStringAsync(sourceUri);
                JsonObject jsonData = JsonObject.Parse(stringData);
                JsonArray jsonStoriesArray = jsonData.GetNamedArray("stories");
                foreach (var item in jsonStoriesArray)
                {
                    string stringItem = item.ToString();
                    JsonObject jsonItem = JsonObject.Parse(stringItem);
                    string title = jsonItem.GetNamedString("title");
                    string id = jsonItem.GetNamedNumber("id").ToString();
                    string image = jsonItem.GetNamedArray("images")[0].ToString();
                    storyItems.Add(new StoryItem { Date = date, Title = title, Id = id, Image = image });
                }
                this.Add(storyItems);
                hadLoaded++;
                #endregion
            }

            isLoading = false;

            if(LoadItemsCompleted != null)
            {
                LoadItemsCompleted(this, EventArgs.Empty);
            }

            return new LoadMoreItemsResult { Count = hadLoaded };
        }

        #region 事件
        public EventHandler LoadItemsStarted;
        public EventHandler LoadItemsCompleted;
        #endregion
    }


    public sealed class StoriesCollection : ObservableCollection<StoryItem>, ISupportIncrementalLoading
    {
        /// <summary>
        /// 加载操作是否正在运行
        /// </summary>
        bool isLoading = false;
        /// <summary>
        /// 最大加载数
        /// </summary>
        const uint MAX_ITEM_COUNT = 20;
        /// <summary>
        /// 新闻列表信息
        /// </summary>
        //public ObservableCollection<StoryItem> storyItems = null;

        #region 实现ISupportIncrementalLoading接口的成员
        public bool HasMoreItems
        {
            get
            {
                if (this.Count >= MAX_ITEM_COUNT)
                {
                    return false;
                }
                return true;
            }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            if (isLoading)
            {
                throw new InvalidOperationException("当前正在加载，请等待本次加载完成后再试");
            }
            uint x = MAX_ITEM_COUNT - (uint)this.Count;
            uint itemCountToLoad = 0;
            if (x < count)
            {
                itemCountToLoad = x;
            }
            else
            {
                itemCountToLoad = count;
            }

            return AsyncInfo.Run(c => LoadMoreItemsPrivateAsync(c, itemCountToLoad));
        }
        #endregion

        private async Task<LoadMoreItemsResult> LoadMoreItemsPrivateAsync(CancellationToken ct, uint n)
        {
            isLoading = true;
            if (LoadItemsStarted != null)
            {
                LoadItemsStarted(this, EventArgs.Empty);
            }

            uint hadLoaded = 0;
            //storyItems = new ObservableCollection<StoryItem>();

            for (uint i = 0; i < n; i++)
            {

                int howManyDaysBefore = 0 - this.Count;
                string date = DateTime.Now.AddDays(howManyDaysBefore).ToString("yyyy年MM月dd日");
                string stringDate = date.Replace("年", "").Replace("月", "").Replace("日", "");
                Uri sourceUri = new Uri("http://news.at.zhihu.com/api/4/news/before/" + stringDate);

                #region 获取新闻列表
                HttpClient client = new HttpClient();
                string stringData = await client.GetStringAsync(sourceUri);
                JsonObject jsonData = JsonObject.Parse(stringData);
                JsonArray jsonStoriesArray = jsonData.GetNamedArray("stories");
                foreach (var item in jsonStoriesArray)
                {
                    string stringItem = item.ToString();
                    JsonObject jsonItem = JsonObject.Parse(stringItem);
                    string title = jsonItem.GetNamedString("title");
                    string id = jsonItem.GetNamedNumber("id").ToString();
                    string image = jsonItem.GetNamedArray("images")[0].ToString();
                    this.Add(new StoryItem { Date = date, Title = title, Id = id, Image = image });
                }
                
                hadLoaded++;
                #endregion
            }

            isLoading = false;

            if (LoadItemsCompleted != null)
            {
                LoadItemsCompleted(this, EventArgs.Empty);
            }

            return new LoadMoreItemsResult { Count = hadLoaded };
        }

        #region 事件
        public EventHandler LoadItemsStarted;
        public EventHandler LoadItemsCompleted;
        #endregion
    }
}
