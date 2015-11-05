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

            if (e.Parameter == null || e.Parameter.ToString() == "")
            {
                this.header_Content.Text = "首页";
                source_uri = new Uri("http://news-at.zhihu.com/api/4/news/latest");
            }
            else
            {
                navigated_item = (Dictionary<string, string>)e.Parameter;
                this.header_Content.Text = navigated_item["title"];
                source_uri = new Uri(navigated_item["uri"]);
            }

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

            //Stories List
            JsonArray stories_array = json_data.GetNamedArray("stories");
            string date = "今日消息";
            foreach (var item in stories_array)
            {
                string string_item = item.Stringify();
                JsonObject json_item = JsonObject.Parse(string_item);
                string title = json_item.GetNamedString("title");
                string image;
                try
                {
                    image = json_item.GetNamedArray("images")[0].GetString();
                }
                catch (Exception)
                {

                    image = "http://pic1.zhimg.com/84dadf360399e0de406c133153fc4ab8_t.jpg";
                }
                string id = json_item.GetNamedNumber("id").ToString();
                st_items.Add(new StoryItem { Title = title, Image = image, Id = id, Date = date });
            }
            //Data binding
            var groups = from n in st_items group n by n.Date;
            this.cvs.Source = groups;

            //TopStories List
            if (this.header_Content.Text == "首页")
            {
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
            }
            else
            {
                string title = json_data.GetNamedString("description");
                string image = json_data.GetNamedString("image");
                t_items.Add(new StoryItem { Title = title, Image = image, Date = "today", Id = "1" });
            }
            
            this.flip_TopStories.ItemsSource = t_items;
        }
        #endregion

        private void btn_Pane_Click(object sender, RoutedEventArgs e)
        {
            this.splitView.IsPaneOpen = !this.splitView.IsPaneOpen;
        }

        private void go_Home(object sender, RoutedEventArgs e)
        {
            navigate_item = new Dictionary<string, string>();
            navigate_item.Add("uri", "http://news-at.zhihu.com/api/4/news/latest");
            navigate_item.Add("title", "首页");
            this.Frame.Navigate(typeof(MainPage), navigate_item);
        }

        private void profile_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void go_Favorite(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void go_Section(object sender, ItemClickEventArgs e)
        {
            SectionItem item = (SectionItem)e.ClickedItem;
            string uri = "http://news-at.zhihu.com/api/4/theme/" + item.Id;
            string title = item.Name;
            navigate_item = new Dictionary<string, string>();
            navigate_item.Add("title", title);
            navigate_item.Add("uri", uri);
            this.Frame.Navigate(typeof(MainPage), navigate_item);
        }

        private void go_StoryPage(object sender, ItemClickEventArgs e)
        {
            StoryItem item = (StoryItem)e.ClickedItem;
            string id = item.Id;
            string title = item.Title;
            navigate_item = new Dictionary<string, string>();
            navigate_item.Add("id", id);
            navigate_item.Add("title", title);
            this.Frame.Navigate(typeof(storyPage), navigate_item);
        }

        private void go_Home(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
