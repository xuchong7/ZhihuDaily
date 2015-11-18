using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ZhihuDaily
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ObservableCollection<SectionItem> s_items = null;
        ObservableCollection<StoryItem> st_items = null;
        ObservableCollection<StoryItem> t_items = null;

        Dictionary<string, string> navigate_item;
        Dictionary<string, string> navigated_item;

        Uri source_uri;

        public MainPage()
        {
            this.InitializeComponent();

            s_items = new ObservableCollection<SectionItem>();
            this.list_section.ItemsSource = s_items;

            st_items = new ObservableCollection<StoryItem>();

            t_items = new ObservableCollection<StoryItem>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            #region init section items
            s_items.Clear();
            GetSectionData();
            #endregion

            source_uri = new Uri("http://news-at.zhihu.com/api/4/news/latest");
            GetStories(source_uri);
        }

        #region Get Section list
        private async void GetSectionData()
        {
            HttpClient client = new HttpClient();
            string data = await client.GetStringAsync(new Uri("http://news-at.zhihu.com/api/4/themes"));
            JsonObject json_data = JsonObject.Parse(data);
            JsonArray section_array = json_data.GetNamedArray("others");
            foreach (var item in section_array)
            {
                string string_item = item.ToString();
                JsonObject json_item = JsonObject.Parse(string_item);
                string name = json_item.GetNamedString("name");
                string id = json_item.GetNamedNumber("id").ToString();
                string description = json_item.GetNamedString("description");
                string thumbnail = json_item.GetNamedString("thumbnail");
                s_items.Add(new SectionItem { Name = name, Id = id, Description = description, Thumbnail = thumbnail });
            }
        }
        #endregion

        #region Get stories list with source_uri
        private async void GetStories(Uri source_uri)
        {
            HttpClient client = new HttpClient();
            string data = await client.GetStringAsync(source_uri);
            JsonObject json_data = JsonObject.Parse(data);

            #region Get stories list

            for (int i = 0; i < 3; i++)
            {
                int howManyDaysBefore = 0 - i;
                string date = DateTime.Now.AddDays(howManyDaysBefore).ToString("yyyy年MM月dd日");
                string stringDate = date.Replace("年", "").Replace("月", "").Replace("日", "");
                Uri sourceUri = new Uri("http://news.at.zhihu.com/api/4/news/before/" + stringDate);

                #region 获取新闻列表
                string stringData = await client.GetStringAsync(sourceUri);
                JsonObject jsonData = JsonObject.Parse(stringData);
                JsonArray jsonStoriesArray = jsonData.GetNamedArray("stories");
                foreach (var item in jsonStoriesArray)
                {
                    string stringItem = item.ToString();
                    JsonObject jsonItem = JsonObject.Parse(stringItem);
                    string title = jsonItem.GetNamedString("title");
                    string id = jsonItem.GetNamedNumber("id").ToString();
                    string image = jsonItem.GetNamedArray("images")[0].ToString().Replace("\"", "");
                    st_items.Add(new StoryItem { Date = date, Title = title, Id = id, Image = image });
                }
                #endregion
            }
            var groups = from n in st_items group n by n.Date;
            this.cvs.Source = groups;
            #endregion

            //TopStories List

            JsonArray top_stories_array = json_data.GetNamedArray("top_stories");
            foreach (var item in top_stories_array)
            {
                string string_item = item.Stringify();
                JsonObject json_item = JsonObject.Parse(string_item);
                string title = json_item.GetNamedString("title");
                string image = json_item.GetNamedString("image");
                string id = json_item.GetNamedNumber("id").ToString();
                t_items.Add(new StoryItem { Title = title, Date = "today", Id = id, Image = image });
            }
            this.flip_TopStories.ItemsSource = t_items;
        }

        private void dataSource_LoadItemsCompleted(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void dataSource_LoadItemsStarted(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        private void GoStoryPage(object sender, ItemClickEventArgs e)
        {
            StoryItem item = (StoryItem)e.ClickedItem;
            string id = item.Id;
            string title = item.Title;
            string image = item.Image;
            string date = item.Date;
            navigate_item = new Dictionary<string, string>();
            navigate_item.Add("id", id);
            navigate_item.Add("title", title);
            navigate_item.Add("image", image);
            navigate_item.Add("date", date);
            this.Frame.Navigate(typeof(storyPage), navigate_item);
        }

        private void GoHome(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void GoHome(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void GoFavorite(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(FavoritePage));
        }

        private void GoFavorite(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FavoritePage));
        }

        private void GoSection(object sender, ItemClickEventArgs e)
        {
            SectionItem item = (SectionItem)e.ClickedItem;
            string uri = "http://news-at.zhihu.com/api/4/theme/" + item.Id;
            string title = item.Name;
            navigate_item = new Dictionary<string, string>();
            navigate_item.Add("title", title);
            navigate_item.Add("uri", uri);
            this.Frame.Navigate(typeof(SectionPage), navigate_item);
        }

        private void PaneClick(object sender, RoutedEventArgs e)
        {
            this.splitView.IsPaneOpen = !this.splitView.IsPaneOpen;
        }
    }
}
