﻿

#pragma checksum "C:\Users\Owner\Documents\Unified Windows\Etsy\Etsy\Etsy.Windows\LoginPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "06B849C5BD85014AA821A0730DD7B164"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Etsy
{
    partial class LoginPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 68 "..\..\LoginPage.xaml"
                ((global::Windows.UI.Xaml.Controls.WebView)(target)).NavigationStarting += this.webView_NavigationStarting;
                 #line default
                 #line hidden
                #line 69 "..\..\LoginPage.xaml"
                ((global::Windows.UI.Xaml.Controls.WebView)(target)).NavigationCompleted += this.webView_NavigationCompleted;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


