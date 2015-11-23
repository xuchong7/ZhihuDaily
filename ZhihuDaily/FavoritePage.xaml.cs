using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZhihuDaily
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FavoritePage : Page
    {
        ObservableCollection<StoryItem> st_items = null;
        Dictionary<string, string> navigate_item = null;

        public FavoritePage()
        {
            this.InitializeComponent();
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetFavoriteStories();

        }

        private async void GetFavoriteStories()
        {
            try
            {
                ObservableCollection<StoryItem> st_items = new ObservableCollection<StoryItem>();
                var data = await PathIO.ReadLinesAsync("ms-appdata:///local/FavoriteData.txt");
                int count = 0;
                foreach (var item in data)
                {
                    string favorite_item = item.ToString();
                    JsonObject json_favorite_item = JsonObject.Parse(favorite_item);
                    string title = json_favorite_item.GetNamedString("title");
                    string image = json_favorite_item.GetNamedString("image");
                    string date = json_favorite_item.GetNamedString("date");
                    string id = json_favorite_item.GetNamedString("id");
                    st_items.Add(new StoryItem { Title = title, Date = date, Id = id, Image = image });
                    count += 1;
                }
                var groups = from n in st_items group n by n.Date;
                this.cvs.Source = groups;
                this.header_Content.Text = count.ToString() + "条收藏";
            }
            catch (Exception)
            {
                StorageFolder local = ApplicationData.Current.LocalFolder;
                StorageFile FavoriteData = await local.CreateFileAsync("FavoriteData.txt", CreationCollisionOption.OpenIfExists);
                this.header_Content.Text = "0条收藏";
            }
            
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

        private void ReFresh(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FavoritePage));
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
