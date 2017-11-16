using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WorkoutAppWPF.CustomControls
{
    class SetPanel : StackPanel
    {
        
        public TextBox repsTextInput = new TextBox();
        public TextBox distTextInput = new TextBox();
        public TextBox coolTextInput = new TextBox();
        public ComboBox unitsComboInput = new ComboBox();
        public ComboBox paceComboInput = new ComboBox();

        public SetPanel(int idx)
        {
            this.Orientation = Orientation.Vertical;

            Label setLabel = new Label();
            setLabel.Content = "Set "+ idx.ToString();
            this.Children.Add(setLabel);

            Grid labelGrid = new Grid();
            this.Children.Add(labelGrid);

            // Create Columns for labels
            ColumnDefinition gridCol1 = new ColumnDefinition();
            gridCol1.Width = new GridLength(1, GridUnitType.Star);
            ColumnDefinition gridCol2 = new ColumnDefinition();
            gridCol2.Width = new GridLength(1, GridUnitType.Star);
            ColumnDefinition gridCol3 = new ColumnDefinition();
            gridCol3.Width = new GridLength(1, GridUnitType.Star);
            ColumnDefinition gridCol4 = new ColumnDefinition();
            gridCol4.Width = new GridLength(1, GridUnitType.Star);

            labelGrid.ColumnDefinitions.Add(gridCol1);
            labelGrid.ColumnDefinitions.Add(gridCol2);
            labelGrid.ColumnDefinitions.Add(gridCol3);
            labelGrid.ColumnDefinitions.Add(gridCol4);

            Label repsLabel = new Label();
            repsLabel.Content = "Reps";
            Grid.SetRow(repsLabel, 0);
            Grid.SetColumn(repsLabel, 0);
            labelGrid.Children.Add(repsLabel);

            Label distLabel = new Label();
            distLabel.Content = "Dist";
            Grid.SetRow(distLabel, 0);
            Grid.SetColumn(distLabel, 1);
            labelGrid.Children.Add(distLabel);

            Label coolLabel = new Label();
            coolLabel.Content = "Cool";
            Grid.SetRow(coolLabel, 0);
            Grid.SetColumn(coolLabel, 2);
            labelGrid.Children.Add(coolLabel);

            Label unitsLabel = new Label();
            repsLabel.Content = "Units";
            Grid.SetRow(unitsLabel, 0);
            Grid.SetColumn(unitsLabel, 3);
            labelGrid.Children.Add(unitsLabel);

            // Create Columns for controls
            ColumnDefinition gridCol5 = new ColumnDefinition();
            gridCol5.Width = new GridLength(1, GridUnitType.Star);
            ColumnDefinition gridCol6 = new ColumnDefinition();
            gridCol6.Width = new GridLength(1, GridUnitType.Star);
            ColumnDefinition gridCol7 = new ColumnDefinition();
            gridCol7.Width = new GridLength(1, GridUnitType.Star);
            ColumnDefinition gridCol8 = new ColumnDefinition();
            gridCol8.Width = new GridLength(1, GridUnitType.Star);

            Grid inputGrid = new Grid();
            inputGrid.ColumnDefinitions.Add(gridCol5);
            inputGrid.ColumnDefinitions.Add(gridCol6);
            inputGrid.ColumnDefinitions.Add(gridCol7);
            inputGrid.ColumnDefinitions.Add(gridCol8);
            this.Children.Add(inputGrid);

            //reps input
            repsTextInput.Text = "0";
            repsTextInput.Margin = new Thickness(0, 0, 5, 0);
            Grid.SetRow(repsTextInput, 0);
            Grid.SetColumn(repsTextInput, 0);
            inputGrid.Children.Add(repsTextInput);

            //distance input
            distTextInput.Text = "0";
            distTextInput.Margin = new Thickness(0, 0, 5, 0);
            Grid.SetRow(distTextInput, 0);
            Grid.SetColumn(distTextInput, 1);
            inputGrid.Children.Add(distTextInput);

            //cool down input
            coolTextInput.Margin = new Thickness(0, 0, 5, 0);
            coolTextInput.Text = "0";
            Grid.SetRow(coolTextInput, 0);
            Grid.SetColumn(coolTextInput, 2);
            inputGrid.Children.Add(coolTextInput);

            //units combo box
            unitsComboInput.Margin = new Thickness(0, 0, 5, 0);
            unitsComboInput.Items.Add("Yards");
            unitsComboInput.Items.Add("Meters");
            unitsComboInput.Items.Add("Miles");
            unitsComboInput.Items.Add("KiloMeters");
            unitsComboInput.SelectedValue = "Miles";
            Grid.SetRow(unitsComboInput, 0);
            Grid.SetColumn(unitsComboInput, 3);
            inputGrid.Children.Add(unitsComboInput);

            //pace combo box
            Label paceLabel = new Label();
            paceLabel.Content = "Pace";
            this.Children.Add(paceLabel);

            paceComboInput.Items.Add("Easy");
            paceComboInput.Items.Add("Long");
            paceComboInput.Items.Add("HMP");
            paceComboInput.Items.Add("MP");
            paceComboInput.Items.Add("FiveK");
            paceComboInput.Items.Add("TenK");
            paceComboInput.Items.Add("Interval");
            paceComboInput.Items.Add("Repition");
            paceComboInput.Items.Add("Hills");
            paceComboInput.SelectedValue = "Easy";
            paceComboInput.Width = 130;
            paceComboInput.HorizontalAlignment = HorizontalAlignment.Left;


            this.Children.Add(paceComboInput);



        }

    }
}
