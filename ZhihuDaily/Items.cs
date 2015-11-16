using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhihuDaily
{
    
    /// <summary>
    /// 各种数据项集合的类
    /// </summary>
    public class SectionItem
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
    }


    public class StoryItem
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public string Image { get; set; }
        public string Date { get; set; }
    }


    public class CommentsItem
    {
        public string author { get; set; }
        public string content { get; set; }
        public string avatar { get; set; }
        public string time { get; set; }
        public string likes { get; set; }
    }


    public class EditorItem
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Bio { get; set; }
    }
}
