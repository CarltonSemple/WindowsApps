﻿

#pragma checksum "C:\Users\Owner\Documents\Unified Windows\ASCII Converter\ASCII Converter\ASCII Converter.WindowsPhone\HubPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "117D47EB9B94BFDB2831FA7C8887CB26"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASCII_Converter
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
                #line 160 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.sourceChoice_Loaded;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 117 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.TextBox)(target)).TextChanged += this.aInput_TextChanged;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 87 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ToggleSwitch)(target)).Toggled += this.bitSwitch_Toggled;
                 #line default
                 #line hidden
                #line 88 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.bitSwitch_Loaded;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 57 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.TextBox)(target)).TextChanged += this.cInput_TextChanged;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

