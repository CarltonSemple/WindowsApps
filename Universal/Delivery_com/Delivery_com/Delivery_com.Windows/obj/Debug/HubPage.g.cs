﻿

#pragma checksum "C:\Users\Owner\Documents\Unified Windows\Delivery_com\Delivery_com\Delivery_com.Windows\HubPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "67B3A196DE812841D50076E21D35F992"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Delivery_com
{
    partial class HubPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 58 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Hub)(target)).SectionHeaderClick += this.Hub_SectionHeaderClick;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 253 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.orderButton_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 166 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.merchantList_Tapped;
                 #line default
                 #line hidden
                #line 169 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).GotFocus += this.merchantList_GotFocus;
                 #line default
                 #line hidden
                #line 170 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.merchantList_Loaded;
                 #line default
                 #line hidden
                #line 171 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.merchantList_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 135 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.AddressList_SelectionChanged;
                 #line default
                 #line hidden
                #line 136 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.AddressList_Loaded;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 108 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.locationManagementClick;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 110 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target)).Checked += this.search_checked;
                 #line default
                 #line hidden
                #line 111 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target)).Unchecked += this.search_unchecked;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


