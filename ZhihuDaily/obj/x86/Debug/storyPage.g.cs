﻿#pragma checksum "C:\Users\xu_cc\Documents\Visual Studio 2015\Projects\ZhihuDaily\ZhihuDaily\storyPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "23BBCE961D9D27BD900ECDF49429AB8E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZhihuDaily
{
    partial class storyPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.header = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 2:
                {
                    this.wv = (global::Windows.UI.Xaml.Controls.WebView)(target);
                }
                break;
            case 3:
                {
                    this.loading = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 4:
                {
                    this.add_favorite_OK = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 5:
                {
                    this.share = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                }
                break;
            case 6:
                {
                    this.star = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 26 "..\..\..\storyPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.star).Click += this.AddFavorite;
                    #line default
                }
                break;
            case 7:
                {
                    this.comments = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 27 "..\..\..\storyPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.comments).Click += this.go_Comments;
                    #line default
                }
                break;
            case 8:
                {
                    this.upvote = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                }
                break;
            case 9:
                {
                    this.like_count = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 10:
                {
                    this.btn_Back = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 21 "..\..\..\storyPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.btn_Back).Click += this.go_Back;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

