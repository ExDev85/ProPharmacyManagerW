﻿#pragma checksum "..\..\..\Pages\AllMeds.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9E4AB2AAC81AEE0B49D4610B41F9F743"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ExtendedButton;
using ProPharmacyManagerW.MVVM;
using ProPharmacyManagerW.Pages;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ProPharmacyManagerW.Pages {
    
    
    /// <summary>
    /// AllMeds
    /// </summary>
    public partial class AllMeds : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 29 "..\..\..\Pages\AllMeds.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dataGrid;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\Pages\AllMeds.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox SearchBox;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\Pages\AllMeds.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar Pb;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\Pages\AllMeds.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cByName;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\Pages\AllMeds.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cByBar;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\Pages\AllMeds.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cBySub;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\Pages\AllMeds.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ExtendedButton.ImageButton SearchB;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\Pages\AllMeds.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ExtendedButton.ImageButton UpdateB;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\Pages\AllMeds.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ExtendedButton.ImageButton BackMainB;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ProPharmacyManagerW;component/pages/allmeds.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\AllMeds.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 11 "..\..\..\Pages\AllMeds.xaml"
            ((ProPharmacyManagerW.Pages.AllMeds)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.dataGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 29 "..\..\..\Pages\AllMeds.xaml"
            this.dataGrid.Loaded += new System.Windows.RoutedEventHandler(this.dataGrid_Loaded);
            
            #line default
            #line hidden
            return;
            case 3:
            this.SearchBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 47 "..\..\..\Pages\AllMeds.xaml"
            this.SearchBox.KeyDown += new System.Windows.Input.KeyEventHandler(this.SearchBox_KeyDown);
            
            #line default
            #line hidden
            
            #line 47 "..\..\..\Pages\AllMeds.xaml"
            this.SearchBox.GotFocus += new System.Windows.RoutedEventHandler(this.SearchBox_GotFocus);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Pb = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 5:
            this.cByName = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 6:
            this.cByBar = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 7:
            this.cBySub = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 8:
            this.SearchB = ((ExtendedButton.ImageButton)(target));
            
            #line 52 "..\..\..\Pages\AllMeds.xaml"
            this.SearchB.Click += new System.Windows.RoutedEventHandler(this.SearchB_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.UpdateB = ((ExtendedButton.ImageButton)(target));
            
            #line 53 "..\..\..\Pages\AllMeds.xaml"
            this.UpdateB.Click += new System.Windows.RoutedEventHandler(this.UpdateB_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.BackMainB = ((ExtendedButton.ImageButton)(target));
            
            #line 54 "..\..\..\Pages\AllMeds.xaml"
            this.BackMainB.Click += new System.Windows.RoutedEventHandler(this.BackMainB_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

