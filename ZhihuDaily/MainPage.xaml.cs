using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ZhihuDaily
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ObservableCollection<ProfileItem> p_items = null;
        ObservableCollection<SectionItem> s_items = null;
        public MainPage()
        {
            this.InitializeComponent();

            p_items = new ObservableCollection<ProfileItem>();
            s_items = new ObservableCollection<SectionItem>();

            this.list_profile.ItemsSource = p_items;
            this.list_section.ItemsSource = s_items;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            p_items.Clear();
            p_items.Add(new ProfileItem { Icon = "People", Content = "登录" });
            p_items.Add(new ProfileItem { Icon = "Home", Content = "首页" });
            p_items.Add(new ProfileItem { Icon = "Favorite", Content = "收藏" });

            s_items.Clear();
            s_items.Add(new SectionItem { Id = "1", Section = "日常心理学" });
            s_items.Add(new SectionItem { Id = "1", Section = "用户推荐日报" });
            s_items.Add(new SectionItem { Id = "1", Section = "电影日报" });
            s_items.Add(new SectionItem { Id = "1", Section = "不许无聊" });
            s_items.Add(new SectionItem { Id = "1", Section = "设计日报" });
            s_items.Add(new SectionItem { Id = "1", Section = "大公司日报" });
            s_items.Add(new SectionItem { Id = "1", Section = "财经日报" });
            s_items.Add(new SectionItem { Id = "1", Section = "互联网安全" });
            s_items.Add(new SectionItem { Id = "1", Section = "开始游戏" });
            s_items.Add(new SectionItem { Id = "1", Section = "音乐日报" });
            s_items.Add(new SectionItem { Id = "1", Section = "动漫日报" });
            s_items.Add(new SectionItem { Id = "1", Section = "体育日报" });

        }
    }


    public class ProfileItem
    {
        public string Icon { get; set; }
        public string Content { get; set; }
    }


    public class SectionItem
    {
        public string Section { get; set; }
        public string Id { get; set; }
    }
}
