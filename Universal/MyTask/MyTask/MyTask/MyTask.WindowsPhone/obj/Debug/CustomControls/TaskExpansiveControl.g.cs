﻿

#pragma checksum "C:\Users\Owner\Documents\Unified Windows\MyTask\MyTask\MyTask\MyTask.Shared\CustomControls\TaskExpansiveControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F1595860903F9684C723A4C7380104BF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyTask.CustomControls
{
    partial class TaskExpansiveControl : global::Windows.UI.Xaml.Controls.UserControl, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 18 "..\..\CustomControls\TaskExpansiveControl.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.mainGrid_Tapped;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 33 "..\..\CustomControls\TaskExpansiveControl.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).ManipulationDelta += this.backPanel_ManipulationDelta;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 41 "..\..\CustomControls\TaskExpansiveControl.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.deleteButton_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

