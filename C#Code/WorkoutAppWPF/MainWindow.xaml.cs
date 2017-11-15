using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorkoutAppWPF.ActivityTypes;

namespace WorkoutAppWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowApp : Window
    {
        Grid mileageGrid;
        Grid dayNumerGrid;
        Grid runTypeGrid;
        Grid weightTrainGrid;
        Grid crossTrainGrid;
        Grid runDefGrid;

        List<TextBox> mileageList;
        List<ComboBox> crossTrainInputList;
        List<ComboBox> runInputList;
        List<ComboBox> weightTrainInputList;
        List<List<RunButton>> runDefInputList;

        public MainWindowApp()
        {
            InitializeComponent();

            setDefaults();

            createMileageTable((int)double.Parse(numCyclesInput.Text));
            createRunTypeSelectorTable();

            createRunDefinitionTable();

            TextBox thisTextBox = mileageList[0];


        }

        private void setDefaults()
        {
            numCyclesInput.TextChanged -= numCycles_Changed;

            numCyclesInput.Text = 16.ToString();
            numDaysInput.Text = 9.ToString();
            numWeeksInput.Text = (Math.Round(double.Parse(numCyclesInput.Text) * double.Parse(numDaysInput.Text) / 7 * 10) / 10).ToString();
            minMileageInput.Text = 10.ToString();
            maxMileageInput.Text = 40.ToString();
            cycleMileageIncrease.Text = 4.ToString();
            numCyclesReset.Text = 4.ToString();
            cycleMileageDeltaInput.Text = 6.ToString();

            numCyclesInput.TextChanged += numCycles_Changed;
            numDaysInput.TextChanged += numCycles_Changed;

            speedSlider.Value = 8;
            longSlider.Value = 30;
            tempoSlider.Value = 12;

        }

        private void recalcMileage_click(object sender, EventArgs e)
        {
            mileageDefStackPanel.Children.Remove(mileageGrid);
            createMileageTable((int)double.Parse(numCyclesInput.Text));


            WorkoutDefStackPanel.Children.Remove(dayNumerGrid);
            WorkoutDefStackPanel.Children.Remove(runTypeGrid);
            WorkoutDefStackPanel.Children.Remove(weightTrainGrid);
            WorkoutDefStackPanel.Children.Remove(crossTrainGrid);
            createRunTypeSelectorTable();

            runDefStackPanel.Children.Remove(runDefGrid);
            createRunDefinitionTable();

        }

        private void recalcRunTable_click(object sender, EventArgs e)
        {
            mileageDefStackPanel.Children.Remove(mileageGrid);
            createMileageTable((int)double.Parse(numCyclesInput.Text));

        }

        private void numCycles_Changed(object sender, EventArgs e)
        {
            if (numCyclesInput.Text != "")
            {
                if (double.Parse(numCyclesInput.Text) > 0)
                {
                    numWeeksInput.Text = (Math.Round(double.Parse(numCyclesInput.Text) * double.Parse(numDaysInput.Text) / 7 * 10) / 10).ToString();
                }
            }

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void createMileageTable(int rows)
        {

            mileageList = new List<TextBox>();

            mileageGrid = new Grid();
            mileageGrid.Width = 150;
            mileageGrid.HorizontalAlignment = HorizontalAlignment.Left;
            mileageGrid.VerticalAlignment = VerticalAlignment.Top;

            // Create Columns
            ColumnDefinition gridCol1 = new ColumnDefinition();
            gridCol1.Width = new GridLength(2, GridUnitType.Star);
            mileageGrid.ColumnDefinitions.Add(gridCol1);

            ColumnDefinition gridCol2 = new ColumnDefinition();
            gridCol2.Width = new GridLength(3, GridUnitType.Star);
            mileageGrid.ColumnDefinitions.Add(gridCol2);


            int numberOfCycles = (int)Math.Ceiling(double.Parse(numCyclesInput.Text));

            // Create Rows
            for (int ii = 0; ii < numberOfCycles + 1; ii++)
            {
                RowDefinition gridRow = new RowDefinition();
                mileageGrid.RowDefinitions.Add(gridRow);
            }

            // Add first column header
            Label headerLabel1 = new Label();
            headerLabel1.Margin = new Thickness(0, 10, 0, 0);
            headerLabel1.Content = "Cycle";
            headerLabel1.FontWeight = FontWeights.Bold;
            headerLabel1.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetRow(headerLabel1, 0);
            Grid.SetColumn(headerLabel1, 0);
            mileageGrid.Children.Add(headerLabel1);

            Label headerLabel2 = new Label();
            headerLabel2.Margin = new Thickness(0, 10, 0, 0);
            headerLabel2.Content = "Mileage";
            headerLabel2.FontWeight = FontWeights.Bold;
            headerLabel2.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetRow(headerLabel2, 0);
            Grid.SetColumn(headerLabel2, 1);
            mileageGrid.Children.Add(headerLabel2);

            //build the table with the contols
            double mileageDelta = double.Parse(cycleMileageIncrease.Text);
            double doubleCycleMileage = 0;
            double maxMileage = double.Parse(maxMileageInput.Text);
            Boolean mileageExceeded = false;
            double lastMileageTgt = double.Parse(minMileageInput.Text);
            int numCyclesRest = (int)Math.Ceiling(double.Parse(numCyclesReset.Text));
            int periodCnt = numCyclesRest;

            for (int ii = 0; ii < numberOfCycles; ii++)
            {

                if (ii == 0)
                {
                    doubleCycleMileage = double.Parse(minMileageInput.Text);

                }
                else
                {
                    if (mileageExceeded == false)
                    {
                        doubleCycleMileage = lastMileageTgt + mileageDelta;

                        if (doubleCycleMileage >= maxMileage)
                        {
                            doubleCycleMileage = maxMileage;
                            mileageExceeded = true;
                        }
                    }

                    else
                    {
                        periodCnt++;
                        if (periodCnt > numCyclesRest)
                        {
                            doubleCycleMileage = doubleCycleMileage - double.Parse(cycleMileageDeltaInput.Text);
                            periodCnt = 1;
                        } else
                        {
                            doubleCycleMileage = lastMileageTgt + double.Parse(cycleMileageDeltaInput.Text) / (numCyclesRest - 1);
                            if (doubleCycleMileage >= maxMileage)
                            {
                                doubleCycleMileage = maxMileage;
                                mileageExceeded = true;
                            }
                        }
                    }


                }

                lastMileageTgt = doubleCycleMileage;

                //Create the controls
                Label cycleText = new Label();
                cycleText.Content = (ii + 1).ToString();
                cycleText.Margin = new Thickness(5, 0, 0, 0);
                Grid.SetRow(cycleText, ii + 1);
                Grid.SetColumn(cycleText, 0);
                mileageGrid.Children.Add(cycleText);

                TextBox mileageText = new TextBox();
                mileageText.PreviewTextInput += NumberValidationTextBox;
                mileageList.Add(mileageText);
                mileageText.Text = (doubleCycleMileage).ToString();
                mileageText.Margin = new Thickness(5, 0, 0, 0);
                Grid.SetRow(mileageText, ii + 1);
                Grid.SetColumn(mileageText, 1);
                mileageGrid.Children.Add(mileageText);
            }

            //add this control to the grid 
            mileageDefStackPanel.Children.Add(mileageGrid);

        }
   

        private void createRunTypeSelectorTable()
        {
            int numDaysInCycle = (int)Math.Ceiling(double.Parse(numDaysInput.Text));
            crossTrainInputList = new List<ComboBox>();
            runInputList = new List<ComboBox>();
            weightTrainInputList = new List<ComboBox>();

            dayNumerGrid = new Grid();
            runTypeGrid = new Grid();
            weightTrainGrid = new Grid();
            crossTrainGrid = new Grid();

            for (int ii = 0; ii < numDaysInCycle + 1; ii++)
            {
                // Create Columns
                ColumnDefinition gridCol1 = new ColumnDefinition();
                gridCol1.Width = new GridLength(1, GridUnitType.Star);

                ColumnDefinition gridCol2 = new ColumnDefinition();
                gridCol2.Width = new GridLength(1, GridUnitType.Star);

                ColumnDefinition gridCol3 = new ColumnDefinition();
                gridCol3.Width = new GridLength(1, GridUnitType.Star);

                ColumnDefinition gridCol4 = new ColumnDefinition();
                gridCol4.Width = new GridLength(1, GridUnitType.Star);

                dayNumerGrid.ColumnDefinitions.Add(gridCol1);
                runTypeGrid.ColumnDefinitions.Add(gridCol2);
                weightTrainGrid.ColumnDefinitions.Add(gridCol3);
                crossTrainGrid.ColumnDefinitions.Add(gridCol4);

            }

            //Add Day label
            Label dayText = new Label();
            dayText.Content = "Day";
            dayText.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetRow(dayText, 0);
            Grid.SetColumn(dayText, 0);
            dayNumerGrid.Children.Add(dayText);

            //Add run label
            Label runText = new Label();
            runText.Content = "Run";
            runText.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetRow(runText, 0);
            Grid.SetColumn(runText, 0);
            runTypeGrid.Children.Add(runText);

            //Add workout label
            Label workoutText = new Label();
            workoutText.Content = "Work";
            workoutText.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetRow(workoutText, 0);
            Grid.SetColumn(workoutText, 0);
            weightTrainGrid.Children.Add(workoutText);

            //Add cross train label
            Label crossTrainText = new Label();
            crossTrainText.Content = "CT";
            crossTrainText.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetRow(crossTrainText, 0);
            Grid.SetColumn(crossTrainText, 0);
            crossTrainGrid.Children.Add(crossTrainText);


            for (int ii = 0; ii < numDaysInCycle; ii++)
            {

                //Add Day Number Text
                Label dayNumberText = new Label();
                dayNumberText.Content = (ii + 1).ToString();
                dayNumberText.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetRow(dayNumberText, 0);
                Grid.SetColumn(dayNumberText, ii+1);
                dayNumerGrid.Children.Add(dayNumberText);

                //Add Cross Train Inputs
                ComboBox weightTrainInput = new ComboBox();
                weightTrainInput.Items.Add("Abs");
                weightTrainInput.Items.Add("W - Legs");
                weightTrainInput.Items.Add("W - Chest");
                weightTrainInput.Items.Add("W - Should");
                weightTrainInput.Items.Add("W - Back");
                weightTrainInput.Items.Add("Rest");
                weightTrainInput.SelectionChanged += crossTrainCmbBox_ValueChanged;
                weightTrainInput.SelectedItem = "Rest";
                Grid.SetRow(weightTrainInput, 0);
                Grid.SetColumn(weightTrainInput, ii + 1);
                weightTrainGrid.Children.Add(weightTrainInput);
                weightTrainInputList.Add(weightTrainInput);

                //Add Cross Train Inputs
                ComboBox crossTrainInput = new ComboBox();
                crossTrainInput.Items.Add("Swim");
                crossTrainInput.Items.Add("Bike");
                crossTrainInput.Items.Add("Rest");
                crossTrainInput.SelectionChanged += crossTrainCmbBox_ValueChanged;
                crossTrainInput.SelectedItem = "Rest";
                Grid.SetRow(crossTrainInput, 0);
                Grid.SetColumn(crossTrainInput, ii + 1);
                crossTrainGrid.Children.Add(crossTrainInput);
                crossTrainInputList.Add(crossTrainInput);

                //Add Run Train Inputs
                ComboBox runInput = new ComboBox();
                runInput.Items.Add("Rest");
                runInput.Items.Add("Easy");
                runInput.Items.Add("Long");
                runInput.Items.Add("Tempo");
                runInput.Items.Add("Interval");
                runInput.SelectionChanged += runComboBox_ValueChanged;
                runInput.SelectedItem = "Rest";
                Grid.SetRow(runInput, 0);
                Grid.SetColumn(runInput, ii + 1);
                runTypeGrid.Children.Add(runInput);
                runInputList.Add(runInput);

            }

            WorkoutDefStackPanel.Children.Add(dayNumerGrid);
            WorkoutDefStackPanel.Children.Add(runTypeGrid);
            WorkoutDefStackPanel.Children.Add(weightTrainGrid);
            WorkoutDefStackPanel.Children.Add(crossTrainGrid);

            setInputSelection(numDaysInCycle);

        }

        private void createRunDefinitionTable()
        {
            int numDaysInCycle = (int)Math.Ceiling(double.Parse(numDaysInput.Text));
            int numCycles = (int)Math.Ceiling(double.Parse(numCyclesInput.Text));

            runDefInputList = new List<List<RunButton>>();
            runDefGrid = new Grid();

            // Create Columns
            for (int ii = 0; ii < numDaysInCycle+1; ii++)
            {
                ColumnDefinition gridCol1 = new ColumnDefinition();
                gridCol1.Width = new GridLength(1, GridUnitType.Star);

                runDefGrid.ColumnDefinitions.Add(gridCol1);
            }

            // Create Rows
            for (int ii = 0; ii < numCycles; ii++)
            {
                RowDefinition gridRow = new RowDefinition();
                runDefGrid.RowDefinitions.Add(gridRow);
            }

            for (int jj = 0; jj < numCycles; jj++) {

                //Add Cycle Number Text
                Label cycleNumberText = new Label();
                cycleNumberText.Content = "Cycle " + (jj + 1).ToString();
                cycleNumberText.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetRow(cycleNumberText, jj);
                Grid.SetColumn(cycleNumberText, 0);
                runDefGrid.Children.Add(cycleNumberText);

                //create a new list for the row
                List<RunButton> cycleRunDefInputList =  new List<RunButton>();
                runDefInputList.Add(cycleRunDefInputList);

                for (int ii = 0; ii < numDaysInCycle; ii++)
                {
                    //Add Run Button for each day
                    RunButton runButton = createRunButton(jj, ii);

                    //runButton.Click = ;
                    //runButton.Content = "Easy - 4";
                    Grid.SetRow(runButton, jj);
                    Grid.SetColumn(runButton, ii+1);
                    runDefGrid.Children.Add(runButton);

                    //add the control to the list
                    cycleRunDefInputList.Add(runButton);
                }
            }

            runDefStackPanel.Children.Add(runDefGrid);
        }

        private RunButton createRunButton(int cycleWeek, int cycleDay)
        {
            RunButton runButton = new RunButton();

            //get cycle mileage
            double cycleMileage = double.Parse(mileageList[cycleWeek].Text);

            //get run type
            String runType = runInputList[cycleDay].SelectedItem.ToString();

            switch (runType)
            {
                case "Easy":
                    runButton.setRunType(RunTypes.Easy);
                    runButton.addWorkout(0,Units.Miles,Pace.Easy);
                    runButton.addRepCool(0, Units.Miles, Pace.Easy);
                    break;
                case "Long":
                    runButton.setRunType(RunTypes.Long);
                    runButton.addWorkout((cycleMileage*longSlider.Value/100.0), Units.Miles, Pace.Easy);
                    runButton.addRepCool(0,Units.Miles,Pace.Easy);
                    break;
                case "Tempo":
                    runButton.setRunType(RunTypes.Tempo);
                    runButton.addWarmup(1.5, Units.Miles, Pace.Easy);
                    runButton.addWorkout((cycleMileage * tempoSlider.Value / 100.0), Units.Miles, Pace.Easy);
                    runButton.addRepCool(0, Units.Miles, Pace.Easy);
                    runButton.addCooldown(1.5, Units.Miles, Pace.Easy);
                    break;
                case "Interval":

                    double intervalMileage = (cycleMileage * speedSlider.Value / 100.0);
                    double repDistance = 400.0;
                    int reps = (int)Math.Ceiling(intervalMileage / convertToMiles(repDistance, Units.Meters));

                    runButton.setRunType(RunTypes.Interval);
                    runButton.addWarmup(1.5, Units.Miles, Pace.Easy);

                    for (int ii = 0; ii< reps; ii++)
                    {
                        runButton.addWorkout(repDistance, Units.Meters, Pace.Interval);
                        runButton.addRepCool(repDistance, Units.Meters, Pace.Easy);
                    }

                    runButton.addCooldown(1.5, Units.Miles, Pace.Easy);
                    break;
                case "Rest":
                    runButton.setRunType(RunTypes.Rest);
                    break;
                default:
                    throw new Exception("Undefined run type");
            }

            return runButton;

        }


        private void CrossTrainInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void setInputSelection(int numRows)
        {
            switch (numRows)
            {

                case 7:

                    runInputList[0].SelectedItem = "Easy";
                    runInputList[1].SelectedItem = "Interval";
                    runInputList[2].SelectedItem = "Rest";
                    runInputList[3].SelectedItem = "Tempo";
                    runInputList[4].SelectedItem = "Easy";
                    runInputList[5].SelectedItem = "Long";
                    runInputList[6].SelectedItem = "Easy";

                    crossTrainInputList[0].SelectedItem = "Rest";
                    crossTrainInputList[1].SelectedItem = "Rest";
                    crossTrainInputList[2].SelectedItem = "Rest";
                    crossTrainInputList[3].SelectedItem = "Rest";
                    crossTrainInputList[4].SelectedItem = "Rest";
                    crossTrainInputList[5].SelectedItem = "Rest";
                    crossTrainInputList[6].SelectedItem = "Rest";

                    weightTrainInputList[0].SelectedItem = "W - Chest";
                    weightTrainInputList[1].SelectedItem = "W - Back";
                    weightTrainInputList[2].SelectedItem = "W - Legs";
                    weightTrainInputList[3].SelectedItem = "Abs";
                    weightTrainInputList[4].SelectedItem = "W - Should";
                    weightTrainInputList[5].SelectedItem = "Rest";
                    weightTrainInputList[6].SelectedItem = "Rest";

                    break;

                case 8:

                    runInputList[0].SelectedItem = "Easy";
                    runInputList[1].SelectedItem = "Interval";
                    runInputList[2].SelectedItem = "Rest";
                    runInputList[3].SelectedItem = "Tempo";
                    runInputList[4].SelectedItem = "Rest";
                    runInputList[5].SelectedItem = "Easy";
                    runInputList[6].SelectedItem = "Long";
                    runInputList[7].SelectedItem = "Easy";

                    crossTrainInputList[0].SelectedItem = "Rest";
                    crossTrainInputList[1].SelectedItem = "Rest";
                    crossTrainInputList[2].SelectedItem = "Rest";
                    crossTrainInputList[3].SelectedItem = "Rest";
                    crossTrainInputList[4].SelectedItem = "Rest";
                    crossTrainInputList[5].SelectedItem = "Rest";
                    crossTrainInputList[6].SelectedItem = "Rest";
                    crossTrainInputList[7].SelectedItem = "Rest";

                    weightTrainInputList[0].SelectedItem = "W - Chest";
                    weightTrainInputList[1].SelectedItem = "W - Back";
                    weightTrainInputList[2].SelectedItem = "W - Legs";
                    weightTrainInputList[3].SelectedItem = "Rest";
                    weightTrainInputList[4].SelectedItem = "Abs";
                    weightTrainInputList[5].SelectedItem = "W - Should";
                    weightTrainInputList[6].SelectedItem = "Rest";
                    weightTrainInputList[7].SelectedItem = "Rest";

                    break;

                case 9:
                    runInputList[0].SelectedItem = "Long";
                    runInputList[1].SelectedItem = "Easy";
                    runInputList[2].SelectedItem = "Rest";
                    runInputList[3].SelectedItem = "Tempo";
                    runInputList[4].SelectedItem = "Easy";
                    runInputList[5].SelectedItem = "Rest";
                    runInputList[6].SelectedItem = "Easy";
                    runInputList[7].SelectedItem = "Interval";
                    runInputList[8].SelectedItem = "Rest";

                    crossTrainInputList[0].SelectedItem = "Rest";
                    crossTrainInputList[1].SelectedItem = "Rest";
                    crossTrainInputList[2].SelectedItem = "Swim";
                    crossTrainInputList[3].SelectedItem = "Rest";
                    crossTrainInputList[4].SelectedItem = "Rest";
                    crossTrainInputList[5].SelectedItem = "Swim";
                    crossTrainInputList[6].SelectedItem = "Rest";
                    crossTrainInputList[7].SelectedItem = "Rest";
                    crossTrainInputList[7].SelectedItem = "Rest";

                    weightTrainInputList[0].SelectedItem = "Rest";
                    weightTrainInputList[1].SelectedItem = "Abs";
                    weightTrainInputList[2].SelectedItem = "W - Chest";
                    weightTrainInputList[3].SelectedItem = "Rest";
                    weightTrainInputList[4].SelectedItem = "W - Back";
                    weightTrainInputList[5].SelectedItem = "W - Legs";
                    weightTrainInputList[6].SelectedItem = "W - Should";
                    weightTrainInputList[7].SelectedItem = "Rest";
                    weightTrainInputList[8].SelectedItem = "Rest";

                    break;

                default:
                    break;
            }
        }

        private void crossTrainCmbBox_ValueChanged(object sender, EventArgs e)
        {
            ComboBox crossTrainBox = (ComboBox)sender;
            string selectedString = (string)crossTrainBox.SelectedItem;

            switch (selectedString)
            {
                case "Rest":
                    crossTrainBox.Background = Brushes.LightBlue;
                    break;
                case "W - Should":
                    crossTrainBox.Background = Brushes.Yellow;
                    break;
                case "Abs":
                    crossTrainBox.Background = Brushes.Yellow;
                    break;
                case "W - Chest":
                    crossTrainBox.Background = Brushes.Yellow;
                    break;
                case "W - Back":
                    crossTrainBox.Background = Brushes.Yellow;
                    break;
                case "W - Legs":
                    crossTrainBox.Background = Brushes.Yellow;
                    break;
                case "Swim":
                    crossTrainBox.Background = Brushes.Yellow;
                    break;
                case "Bike":
                    crossTrainBox.Background = Brushes.Yellow;
                    break;
                default:
                    break;


            }
        }

        private void runComboBox_ValueChanged(object sender, EventArgs e)
        {
            ComboBox runCmbBox = (ComboBox)sender;
            string selectedString = (string)runCmbBox.SelectedItem;

            switch (selectedString)
            {
                case "Rest":
                    runCmbBox.Background = Brushes.LightBlue;
                    break;
                case "Easy":
                    runCmbBox.Background = Brushes.LightGreen;
                    break;
                case "Long":
                    runCmbBox.Background = Brushes.Green;
                    break;
                case "Tempo":
                    runCmbBox.Background = Brushes.Orange;
                    break;
                case "Interval":
                    runCmbBox.Background = Brushes.Orange;
                    break;
                default:
                    break;


            }
        }

        //ToDo: this method should be combined with teh other one
        private void runTypeComboBox_ValueChanged(object sender, EventArgs e)
        {
            ComboBox runCmbBox = (ComboBox)sender;
            
            string selectedString = ((ComboBoxItem)runCmbBox.SelectedItem).Content.ToString();

            switch (selectedString)
            {
                case "Rest":
                    runCmbBox.Background = Brushes.LightBlue;
                    break;
                case "Easy":
                    runCmbBox.Background = Brushes.LightGreen;
                    break;
                case "Long":
                    runCmbBox.Background = Brushes.Green;
                    break;
                case "Tempo":
                    runCmbBox.Background = Brushes.Orange;
                    break;
                case "Interval":
                    runCmbBox.Background = Brushes.Orange;
                    break;
                default:
                    break;


            }
        }
        private double convertToMiles(double distance, Units units)
        {
            double unitFactor = 1;
            double convertedMiles = 0;

            switch (units)
            {
                case Units.Miles:
                    unitFactor = 1;
                    break;

                case Units.KiloMeters:
                    unitFactor = 0.621371;
                    break;

                case Units.Meters:
                    unitFactor = 0.621371 / 1000.0;
                    break;

                case Units.Yards:
                    unitFactor = 0.000568182;
                    break;

                default:
                    throw new Exception("unspecied units defined");
            }

            convertedMiles = unitFactor * distance;
            return convertedMiles;
        }
    }
}

    

