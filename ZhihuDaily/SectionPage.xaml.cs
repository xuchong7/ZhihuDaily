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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZhihuDaily
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SectionPage : Page
    {
        ObservableCollection<StoryItem> st_items = null;
        ObservableCollection<StoryItem> t_items = null;
        ObservableCollection<SectionItem> s_items = null;
        ObservableCollection<EditorItem> e_items = null;

        Dictionary<string, string> navigate_item = null;
        Dictionary<string, string> navigated_item = null;

        Uri source_uri;

        public SectionPage()
        {
            this.InitializeComponent();
            st_items = new ObservableCollection<StoryItem>();
            t_items = new ObservableCollection<StoryItem>();
            s_items = new ObservableCollection<SectionItem>();
            e_items = new ObservableCollection<EditorItem>();
            this.list_section.ItemsSource = s_items;
            this.editors.ItemsSource = e_items;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            s_items.Clear();
            GetSectionData();
            try
            {
                navigated_item = (Dictionary<string, string>)e.Parameter;
                this.header_Content.Text = navigated_item["title"];
                source_uri = new Uri(navigated_item["uri"]);
                GetStories(source_uri);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //TODO Get stories list, editors and so on
        private async void GetStories(Uri source_uri)
        {
            HttpClient client = new HttpClient();
            string data = await client.GetStringAsync(source_uri);
            JsonObject json_data = JsonObject.Parse(data);

            //Stories List
            JsonArray stories_array = json_data.GetNamedArray("stories");
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

                    image = "https://liukanshan.zhihu.com/images/downloads/avatars/classic/06-33098635.png";
                }
                string id = json_item.GetNamedNumber("id").ToString();
                st_items.Add(new StoryItem { Title = title, Image = image, Id = id, Date = "today" });
            }
            //Data binding
            var groups = from n in st_items group n by n.Date;
            this.list_Stories.ItemsSource = st_items;

            //Section TopImage
            string t_title = json_data.GetNamedString("description");
            string t_image = json_data.GetNamedString("image");
            t_items.Add(new StoryItem { Title = t_title, Image = t_image, Date = "today", Id = "1" });
            this.flip_TopStories.ItemsSource = t_items;

            //Editors
            try
            {
                JsonArray editors_array = json_data.GetNamedArray("editors");
                foreach (var item in editors_array)
                {
                    string string_item = item.Stringify();
                    JsonObject json_item = JsonObject.Parse(string_item);
                    string url = json_item.GetNamedString("url");
                    string name = json_item.GetNamedString("name");
                    string id = json_item.GetNamedNumber("id").ToString();
                    string avatar = json_item.GetNamedString("avatar");
                    string bio = json_item.GetNamedString("bio");
                    e_items.Add(new EditorItem { Url = url, Avatar = avatar, Bio = bio, Id = id, Name = name });
                }

            }
            catch (Exception)
            {
                this.editors_text.Text = "主编    多人";
            }
            
            //Image Source
            string image_source = json_data.GetNamedString("image_source");
        }

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
    }
}
