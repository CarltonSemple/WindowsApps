﻿#pragma checksum "C:\Users\Carlton\Documents\App Development\Conversion App\Conversion App\CustomUserControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A898D7DC06EFEEFF04EF43B95673DD78"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Conversion_App {
    
    
    public partial class CustomUserControl : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.VisualStateGroup CommonStates;
        
        internal System.Windows.VisualState Normal;
        
        internal System.Windows.VisualState Selected;
        
        internal System.Windows.Controls.TextBlock textBlock;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Conversion%20App;component/CustomUserControl.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.CommonStates = ((System.Windows.VisualStateGroup)(this.FindName("CommonStates")));
            this.Normal = ((System.Windows.VisualState)(this.FindName("Normal")));
            this.Selected = ((System.Windows.VisualState)(this.FindName("Selected")));
            this.textBlock = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock")));
        }
    }
}

