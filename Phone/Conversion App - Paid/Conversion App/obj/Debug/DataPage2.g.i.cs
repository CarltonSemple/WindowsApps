﻿#pragma checksum "C:\Users\Carlton\Documents\App Development\Conversion App\Conversion App\DataPage2.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D79A4A9523B23FBF5B522F6470259319"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
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
    
    
    public partial class DataPage2 : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock fromBox;
        
        internal System.Windows.Controls.TextBlock unitBox;
        
        internal System.Windows.Controls.TextBlock truthBlock2;
        
        internal Microsoft.Phone.Controls.LongListSelector unitList;
        
        internal System.Windows.Controls.Button continueButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Conversion%20App;component/DataPage2.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.fromBox = ((System.Windows.Controls.TextBlock)(this.FindName("fromBox")));
            this.unitBox = ((System.Windows.Controls.TextBlock)(this.FindName("unitBox")));
            this.truthBlock2 = ((System.Windows.Controls.TextBlock)(this.FindName("truthBlock2")));
            this.unitList = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("unitList")));
            this.continueButton = ((System.Windows.Controls.Button)(this.FindName("continueButton")));
        }
    }
}

