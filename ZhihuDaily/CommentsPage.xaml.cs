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
    public sealed partial class CommentsPage : Page
    {
        Dictionary<string, string> navigate_item;
        Dictionary<string, string> navigated_item;

        ObservableCollection<CommentsItem> lc_items;
        ObservableCollection<CommentsItem> sc_items;

        public CommentsPage()
        {
            this.InitializeComponent();

            //Cache Enable
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            lc_items = new ObservableCollection<CommentsItem>();
            this.long_lv.ItemsSource = lc_items;

            sc_items = new ObservableCollection<CommentsItem>();
            this.short_lv.ItemsSource = sc_items;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigated_item = (Dictionary<string, string>)e.Parameter;
            Uri long_comments_uri = new Uri("http://news-at.zhihu.com/api/4/story/" + navigated_item["id"] + "/long-comments");
            Uri short_comments_uri = new Uri("http://news-at.zhihu.com/api/4/story/" + navigated_item["id"] + "/short-comments");

            this.header.Text = "评论 · " + navigated_item["title"];

            GetComments(long_comments_uri, lc_items);
            GetComments(short_comments_uri, sc_items);
        }

        private async void GetComments(Uri comments_uri, ObservableCollection<CommentsItem> items)
        {
            HttpClient client = new HttpClient();
            string string_comments = await client.GetStringAsync(comments_uri);
            JsonObject json_long_comments = JsonObject.Parse(string_comments);
            JsonArray comments_array = json_long_comments.GetNamedArray("comments");
            foreach (var comment_item in comments_array)
            {
                string string_item = comment_item.Stringify();
                JsonObject json_item = JsonObject.Parse(string_item);
                string author = json_item.GetNamedString("author");
                string avatar = json_item.GetNamedString("avatar");
                string content = json_item.GetNamedString("content");
                string likes = json_item.GetNamedNumber("likes").ToString();
                string time = json_item.GetNamedNumber("time").ToString();
                items.Add(new CommentsItem { author = author, avatar = avatar, content = content, likes = likes, time = time });
            }
        }

        private void go_Back(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void GoHome(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void GoHome(object sender, RoutedEventArgs e)
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

        private void GoSetting(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingPage));
        }

        private void GoSetting(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingPage));
        }
    }
}
