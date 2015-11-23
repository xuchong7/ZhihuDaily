using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
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
    public sealed partial class storyPage : Page
    {
        Dictionary<string, string> navigate_item;
        Dictionary<string, string> navigated_item;
        string story_id;
        string story_image;
        string story_title;
        string story_date;

        public storyPage()
        {
            this.InitializeComponent();

            //Cache Enable
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                navigated_item = (Dictionary<string, string>)e.Parameter;
                story_id = navigated_item["id"];
                story_image = navigated_item["image"];
                story_title = navigated_item["title"];
                story_date = navigated_item["date"];
                Uri story_uri = new Uri("http://news-at.zhihu.com/api/4/news/" + navigated_item["id"]);
                Uri extra_uri = new Uri("http://news-at.zhihu.com/api/4/story-extra/" + navigated_item["id"]);
                this.loading.Visibility = Windows.UI.Xaml.Visibility.Visible;
                GetStory(story_uri, extra_uri);
                
            }
        }

        private async void GetStory(Uri story_uri, Uri extra_uri)
        {
            
            try
            {
                HttpClient client = new HttpClient();
                Uri uri = (Uri)story_uri;
                string data = await client.GetStringAsync(uri);
                JsonObject json_story = JsonObject.Parse(data);
                string html = json_story.GetNamedString("body").Replace("<div class=\"headline\">\n\n<div class=\"img-place-holder\"></div>\n\n\n\n</div>", "");
                string css = json_story.GetNamedArray("css")[0].ToString().Replace("\"", "");

                #region Get story css
                //TODO Get story css
                html = "<html><head><link rel=\"stylesheet\" type=\"text/css\" href=\"" + css + "\" /></head>" + html + "</html>";
                #endregion

                this.wv.NavigateToString(html);
            }
            catch (Exception)
            {
                this.wv.NavigateToString("<body><h>No story!!!</h></body>");
            }

            try
            {
                HttpClient client = new HttpClient();
                string string_extra = await client.GetStringAsync(extra_uri);
                JsonObject json_extra = JsonObject.Parse(string_extra);
                string likeCount = json_extra.GetNamedNumber("popularity").ToString();
                string commentsCount = json_extra.GetNamedNumber("comments").ToString();
                this.like_count.Text = likeCount;
                this.comments_count.Text = commentsCount;
            }
            catch (Exception)
            {
                this.like_count.Text = "0";
                this.comments_count.Text = "0";
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void GoComments(object sender, RoutedEventArgs e)
        {
            navigate_item = navigated_item;
            this.Frame.Navigate(typeof(CommentsPage), navigate_item);
        }

        private async void AddFavorite(object sender, RoutedEventArgs e)
        {
            try
            {
                Button favorite_button = sender as Button;
                favorite_button.IsEnabled = false;

                string now = DateTime.Now.ToString("yyyy年MM月dd日");
                string[] lines = new string[] { "{\"id\":\"" + story_id + "\",\"title\":\"" + story_title + "\",\"image\":\"" + story_image + "\",\"date\":\"" + now + "\"}" };

                StorageFolder local = ApplicationData.Current.LocalFolder;
                StorageFile favoriteFile = await local.CreateFileAsync("FavoriteData.txt", CreationCollisionOption.OpenIfExists);
                await FileIO.AppendLinesAsync(favoriteFile, lines);

                favorite_button.IsEnabled = true;
                this.add_favorite_OK.Visibility = Windows.UI.Xaml.Visibility.Visible;
                AddFavoriteNotice(this.add_favorite_OK);
            }
            catch (Exception)
            {

            }
            
        }

        private void AddFavoriteNotice(Button notice_border)
        {
            
            string now = System.DateTime.Now.Second.ToString();
            notice_border.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void closeMsg(object sender, RoutedEventArgs e)
        {
            this.add_favorite_OK.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
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

        private void ReFresh(object sender, RoutedEventArgs e)
        {
            navigate_item = new Dictionary<string, string>();
            navigate_item.Add("id", story_id);
            navigate_item.Add("title", story_title);
            navigate_item.Add("image", story_image);
            navigate_item.Add("date", story_date);
            this.Frame.Navigate(typeof(storyPage), navigate_item);
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
