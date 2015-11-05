using System;
using System.Collections.Generic;
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
    public sealed partial class storyPage : Page
    {
        Dictionary<string, string> navigate_item;
        Dictionary<string, string> navigated_item;

        public storyPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                navigated_item = (Dictionary<string, string>)e.Parameter;
                Uri story_uri = new Uri("http://news-at.zhihu.com/api/4/news/" + navigated_item["id"]);
                GetStory(story_uri);
            }
        }

        private async void GetStory(Uri story_uri)
        {
            try
            {
                HttpClient client = new HttpClient();
                Uri uri = (Uri)story_uri;
                string data = await client.GetStringAsync(uri);
                JsonObject json_story = JsonObject.Parse(data);
                string html = json_story.GetNamedString("body");
                this.wv.NavigateToString(html);
            }
            catch (Exception)
            {
                this.wv.NavigateToString("<body><h>No story!!!</h></body>");
            }
            
        }

        private void go_Back(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void go_Home(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void go_Comments(object sender, RoutedEventArgs e)
        {
            navigate_item = navigated_item;
            this.Frame.Navigate(typeof(CommentsPage), navigate_item);
        }
    }
}
