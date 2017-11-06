﻿#pragma checksum "..\..\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "576D527159A4AFA033C07278AB6FED50"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
using WorkoutAppWPF;


namespace WorkoutAppWPF {
    
    
    /// <summary>
    /// MainWindowApp
    /// </summary>
    public partial class MainWindowApp : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 49 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel mileageDefStackPanel;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox numCyclesInput;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox numWeeksInput;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox numDaysInput;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox minMileageInput;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox maxMileageInput;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox cycleMileageIncrease;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox cycleMileageDeltaInput;
        
        #line default
        #line hidden
        
        
        #line 113 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox numCyclesReset;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button recalcMileageTarget;
        
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
            System.Uri resourceLocater = new System.Uri("/WorkoutAppWPF;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
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
            this.mileageDefStackPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            this.numCyclesInput = ((System.Windows.Controls.TextBox)(target));
            
            #line 67 "..\..\MainWindow.xaml"
            this.numCyclesInput.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            
            #line 67 "..\..\MainWindow.xaml"
            this.numCyclesInput.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.numCycles_Changed);
            
            #line default
            #line hidden
            return;
            case 3:
            this.numWeeksInput = ((System.Windows.Controls.TextBox)(target));
            
            #line 68 "..\..\MainWindow.xaml"
            this.numWeeksInput.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            return;
            case 4:
            this.numDaysInput = ((System.Windows.Controls.TextBox)(target));
            
            #line 72 "..\..\MainWindow.xaml"
            this.numDaysInput.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            return;
            case 5:
            this.minMileageInput = ((System.Windows.Controls.TextBox)(target));
            
            #line 91 "..\..\MainWindow.xaml"
            this.minMileageInput.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            return;
            case 6:
            this.maxMileageInput = ((System.Windows.Controls.TextBox)(target));
            
            #line 92 "..\..\MainWindow.xaml"
            this.maxMileageInput.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            return;
            case 7:
            this.cycleMileageIncrease = ((System.Windows.Controls.TextBox)(target));
            
            #line 96 "..\..\MainWindow.xaml"
            this.cycleMileageIncrease.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            return;
            case 8:
            this.cycleMileageDeltaInput = ((System.Windows.Controls.TextBox)(target));
            
            #line 112 "..\..\MainWindow.xaml"
            this.cycleMileageDeltaInput.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            return;
            case 9:
            this.numCyclesReset = ((System.Windows.Controls.TextBox)(target));
            
            #line 113 "..\..\MainWindow.xaml"
            this.numCyclesReset.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            return;
            case 10:
            this.recalcMileageTarget = ((System.Windows.Controls.Button)(target));
            
            #line 116 "..\..\MainWindow.xaml"
            this.recalcMileageTarget.Click += new System.Windows.RoutedEventHandler(this.recalcMileage_click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

