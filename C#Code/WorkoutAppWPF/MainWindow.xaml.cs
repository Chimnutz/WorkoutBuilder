﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorkoutAppWPF.ActivityTypes;
using WorkoutAppWPF.CustomControls;
using System.Windows.Controls.DataVisualization.Charting;
using Microsoft.Win32;
using System.Threading;
//using PlotTools = System.Windows.Forms.DataVisualization.Charting;

namespace WorkoutAppWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowApp : Window
    {
        Grid mileageGrid;
        Grid dayNumberGrid;
        Grid runTypeGrid;
        Grid weightTrainGrid;
        Grid crossTrainGrid1;
        Grid crossTrainGrid2;
        Grid runDefGrid;

        List<TextBox> mileageList;
        List<TextBox> actualMileageList;
        List<TextBox> stressIndexList;
        List<ComboBox> crossTrainInputList1;
        List<ComboBox> crossTrainInputList2;
        List<ComboBox> runInputList;
        List<ComboBox> weightTrainInputList;
        List<List<RunButton>> runDefInputList;
        List<SetPanel> currentSetPanels = new List<SetPanel>();
        RunButton currentRunButton;

        public MainWindowApp()
        {
            InitializeComponent();

            setDefaults();

            createMileageTable((int)double.Parse(numCyclesInput.Text));
            createRunTypeSelectorTable();

            createRunDefinitionTable();

            SetPanel setPanel = addSetPanel(1);
            currentSetPanels.Add(setPanel);

            calculateTotalMiles();
        }

        private void setDefaults()
        {
            numCyclesInput.TextChanged -= numCycles_Changed;

            numCyclesInput.Text = 16.ToString();
            numDaysInput.Text = 9.ToString();


            numWeeksInput.Text = (Math.Round(double.Parse(numCyclesInput.Text) * double.Parse(numDaysInput.Text) / 7 * 10) / 10).ToString();

            DateTime startDate = DateTime.Today;

            targetTrainingStartDate.SelectedDate = startDate;
            targetRaceDateInput.SelectedDate  = startDate.AddDays(double.Parse(numCyclesInput.Text)* double.Parse(numDaysInput.Text)-1);

            minMileageInput.Text = 15.ToString();
            maxMileageInput.Text = 50.ToString();
            cycleMileageIncrease.Text = 4.ToString();
            numCyclesReset.Text = 4.ToString();
            cycleMileageDeltaInput.Text = 12.ToString();
            maxLongRunMiles.Text = 14.ToString();
            maxTempoRunMiles.Text = 10.ToString();
            maxSpeedRunMiles.Text = 10.ToString();

            numCyclesInput.TextChanged += numCycles_Changed;
            numDaysInput.TextChanged += numCycles_Changed;

            speedSlider.Value = 7;
            longSlider.Value = 28;
            tempoSlider.Value = 14;

        }

        private void recalcMileage_click(object sender, EventArgs e)
        {
            mileageDefStackPanel.Children.Remove(mileageGrid);
            createMileageTable((int)double.Parse(numCyclesInput.Text));


        }

        private void recalcRunTable_click(object sender, EventArgs e)
        {

            int numDays = (int)Math.Ceiling(double.Parse(numDaysInput.Text));

            //only remove all children if the number of days has changed
            if ((dayNumberGrid.Children.Count-1) != numDays)
            {

                WorkoutDefStackPanel.Children.Remove(dayNumberGrid);
                WorkoutDefStackPanel.Children.Remove(runTypeGrid);
                WorkoutDefStackPanel.Children.Remove(weightTrainGrid);
                WorkoutDefStackPanel.Children.Remove(crossTrainGrid1);
                WorkoutDefStackPanel.Children.Remove(crossTrainGrid2);
                createRunTypeSelectorTable();
            }

            runDefStackPanel.Children.Remove(runDefGrid);
            createRunDefinitionTable();

        }

        private void numCycles_Changed(object sender, EventArgs e)
        {
            if ((numCyclesInput.Text != "") && (numDaysInput.Text != ""))
            {
                if (double.Parse(numCyclesInput.Text) > 0)
                {
                    numWeeksInput.Text = (Math.Round(double.Parse(numCyclesInput.Text) * double.Parse(numDaysInput.Text) / 7 * 10) / 10).ToString();
                }

                DateTime startDate = (DateTime)targetRaceDateInput.SelectedDate;

                targetTrainingStartDate.SelectedDate = startDate.AddDays(double.Parse(numCyclesInput.Text) * double.Parse(numDaysInput.Text) * -1 + 1);
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
            actualMileageList = new List<TextBox>();
            stressIndexList = new List<TextBox>();

            mileageGrid = new Grid();
            mileageGrid.Width = 250;
            mileageGrid.HorizontalAlignment = HorizontalAlignment.Left;
            mileageGrid.VerticalAlignment = VerticalAlignment.Top;

            // Create Columns
            ColumnDefinition gridCol1 = new ColumnDefinition();
            gridCol1.Width = new GridLength(2, GridUnitType.Star);
            mileageGrid.ColumnDefinitions.Add(gridCol1);

            ColumnDefinition gridCol2 = new ColumnDefinition();
            gridCol2.Width = new GridLength(3, GridUnitType.Star);
            mileageGrid.ColumnDefinitions.Add(gridCol2);

            ColumnDefinition gridCol3 = new ColumnDefinition();
            gridCol3.Width = new GridLength(3, GridUnitType.Star);
            mileageGrid.ColumnDefinitions.Add(gridCol3);

            ColumnDefinition gridCol4 = new ColumnDefinition();
            gridCol4.Width = new GridLength(3, GridUnitType.Star);
            mileageGrid.ColumnDefinitions.Add(gridCol4);


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
            headerLabel1.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetRow(headerLabel1, 0);
            Grid.SetColumn(headerLabel1, 0);
            mileageGrid.Children.Add(headerLabel1);

            Label headerLabel2 = new Label();
            headerLabel2.Margin = new Thickness(0, 10, 0, 0);
            headerLabel2.Content = "Target";
            headerLabel2.FontWeight = FontWeights.Bold;
            headerLabel2.VerticalAlignment = VerticalAlignment.Top;
            headerLabel2.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetRow(headerLabel2, 0);
            Grid.SetColumn(headerLabel2, 1);
            mileageGrid.Children.Add(headerLabel2);

            Label headerLabel3 = new Label();
            headerLabel3.Margin = new Thickness(0, 10, 0, 0);
            headerLabel3.Content = "Actual";
            headerLabel3.FontWeight = FontWeights.Bold;
            headerLabel3.VerticalAlignment = VerticalAlignment.Top;
            headerLabel3.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetRow(headerLabel3, 0);
            Grid.SetColumn(headerLabel3, 2);
            mileageGrid.Children.Add(headerLabel3);

            Label headerLabel4 = new Label();
            headerLabel4.Margin = new Thickness(0, 10, 0, 0);
            headerLabel4.Content = "SI";
            headerLabel4.FontWeight = FontWeights.Bold;
            headerLabel4.VerticalAlignment = VerticalAlignment.Top;
            headerLabel4.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetRow(headerLabel4, 0);
            Grid.SetColumn(headerLabel4, 3);
            mileageGrid.Children.Add(headerLabel4);

            //build the table with the contols
            double mileageDelta = double.Parse(cycleMileageIncrease.Text);
            double doubleCycleMileage = 0;
            double maxMileage = double.Parse(maxMileageInput.Text);
            Boolean mileageExceeded = false;
            double lastMileageTgt = double.Parse(minMileageInput.Text);
            int numCyclesRest = (int)Math.Ceiling(double.Parse(numCyclesReset.Text));
            int periodCnt = numCyclesRest;
            int cnt = 1;

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
                        if (cnt == 0)
                        {
                            doubleCycleMileage = lastMileageTgt + mileageDelta;
                            cnt = 1;
                        }else
                        {
                            cnt = 0;
                        }
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
                        }
                        else
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

                TextBox actualMileage = new TextBox();
                actualMileage.PreviewTextInput += NumberValidationTextBox;
                actualMileage.IsEnabled = false;
                actualMileageList.Add(actualMileage);
                actualMileage.Text = (0).ToString();
                actualMileage.Margin = new Thickness(5, 0, 0, 0);
                Grid.SetRow(actualMileage, ii + 1);
                Grid.SetColumn(actualMileage, 2);
                mileageGrid.Children.Add(actualMileage);

                TextBox stressIndex = new TextBox();
                stressIndex.PreviewTextInput += NumberValidationTextBox;
                stressIndex.IsEnabled = false;
                stressIndexList.Add(stressIndex);
                stressIndex.Text = (0).ToString();
                stressIndex.Margin = new Thickness(5, 0, 0, 0);
                Grid.SetRow(stressIndex, ii + 1);
                Grid.SetColumn(stressIndex, 3);
                mileageGrid.Children.Add(stressIndex);

            }

            //add this control to the grid 
            mileageDefStackPanel.Children.Add(mileageGrid);

        }


        private void createRunTypeSelectorTable()
        {
            int numDaysInCycle = (int)Math.Ceiling(double.Parse(numDaysInput.Text));
            crossTrainInputList1 = new List<ComboBox>();
            crossTrainInputList2 = new List<ComboBox>();
            runInputList = new List<ComboBox>();
            weightTrainInputList = new List<ComboBox>();

            dayNumberGrid = new Grid();
            runTypeGrid = new Grid();
            weightTrainGrid = new Grid();
            crossTrainGrid1 = new Grid();
            crossTrainGrid2 = new Grid();

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

                ColumnDefinition gridCol5 = new ColumnDefinition();
                gridCol5.Width = new GridLength(1, GridUnitType.Star);

                dayNumberGrid.ColumnDefinitions.Add(gridCol1);
                runTypeGrid.ColumnDefinitions.Add(gridCol2);
                weightTrainGrid.ColumnDefinitions.Add(gridCol3);
                crossTrainGrid1.ColumnDefinitions.Add(gridCol4);
                crossTrainGrid2.ColumnDefinitions.Add(gridCol5);

            }

            //Add Day label
            Label dayText = new Label();
            dayText.Content = "Day";
            dayText.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetRow(dayText, 0);
            Grid.SetColumn(dayText, 0);
            dayNumberGrid.Children.Add(dayText);

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
            Label crossTrainText1 = new Label();
            crossTrainText1.Content = "CT1";
            crossTrainText1.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetRow(crossTrainText1, 0);
            Grid.SetColumn(crossTrainText1, 0);
            crossTrainGrid1.Children.Add(crossTrainText1);

            //Add cross train label
            Label crossTrainText2 = new Label();
            crossTrainText2.Content = "CT2";
            crossTrainText2.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetRow(crossTrainText2, 0);
            Grid.SetColumn(crossTrainText2, 0);
            crossTrainGrid2.Children.Add(crossTrainText2);


            for (int ii = 0; ii < numDaysInCycle; ii++)
            {

                //Add Day Number Text
                Label dayNumberText = new Label();
                dayNumberText.Content = (ii + 1).ToString();
                dayNumberText.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetRow(dayNumberText, 0);
                Grid.SetColumn(dayNumberText, ii + 1);
                dayNumberGrid.Children.Add(dayNumberText);

                //Add Cross Train Inputs
                ComboBox weightTrainInput = new ComboBox();
                weightTrainInput.Items.Add("W - Core");
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
                ComboBox crossTrainInput1 = new ComboBox();
                crossTrainInput1.Items.Add("Swim");
                crossTrainInput1.Items.Add("Bike");
                crossTrainInput1.Items.Add("Rest");
                crossTrainInput1.SelectionChanged += crossTrainCmbBox_ValueChanged;
                crossTrainInput1.SelectedItem = "Rest";
                Grid.SetRow(crossTrainInput1, 0);
                Grid.SetColumn(crossTrainInput1, ii + 1);
                crossTrainGrid1.Children.Add(crossTrainInput1);
                crossTrainInputList1.Add(crossTrainInput1);

                //Add Cross Train Inputs
                ComboBox crossTrainInput2 = new ComboBox();
                crossTrainInput2.Items.Add("Swim");
                crossTrainInput2.Items.Add("Bike");
                crossTrainInput2.Items.Add("Rest");
                crossTrainInput2.SelectionChanged += crossTrainCmbBox_ValueChanged;
                crossTrainInput2.SelectedItem = "Rest";
                Grid.SetRow(crossTrainInput2, 0);
                Grid.SetColumn(crossTrainInput2, ii + 1);
                crossTrainGrid2.Children.Add(crossTrainInput2);
                crossTrainInputList2.Add(crossTrainInput2);

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

            WorkoutDefStackPanel.Children.Add(dayNumberGrid);
            WorkoutDefStackPanel.Children.Add(runTypeGrid);
            WorkoutDefStackPanel.Children.Add(weightTrainGrid);
            WorkoutDefStackPanel.Children.Add(crossTrainGrid1);
            WorkoutDefStackPanel.Children.Add(crossTrainGrid2);

            setInputSelection(numDaysInCycle);

        }

        private void createRunDefinitionTable()
        {
            int numDaysInCycle = (int)Math.Ceiling(double.Parse(numDaysInput.Text));
            int numCycles = (int)Math.Ceiling(double.Parse(numCyclesInput.Text));

            runDefInputList = new List<List<RunButton>>();
            runDefGrid = new Grid();

            // Create Columns
            for (int ii = 0; ii < numDaysInCycle + 1; ii++)
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

            double lastDistance = 0;

            for (int jj = 0; jj < numCycles; jj++)
            {

                //Add Cycle Number Text
                Label cycleNumberText = new Label();
                cycleNumberText.Content = "Cycle " + (jj + 1).ToString();
                cycleNumberText.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetRow(cycleNumberText, jj);
                Grid.SetColumn(cycleNumberText, 0);
                runDefGrid.Children.Add(cycleNumberText);

                //create a new list for the row
                List<RunButton> cycleRunDefInputList = new List<RunButton>();

                int numEasyRuns = 0;
                double totalMileage = 0;

                List<int> easyIndex = new List<int>();
                List<double> previousMileage = new List<double>();

                List<RunButton> cycleRunDefList = new List<RunButton>();



                for (int ii = 0; ii < numDaysInCycle; ii++)
                {
                    //Add Run Button for each day
                    RunButton runButton = createRunButton(jj, ii);

                    //get run type
                    String runType = runInputList[ii].SelectedItem.ToString();

                    if (runType == "Easy")
                    {
                        numEasyRuns++;
                        easyIndex.Add(ii); //add the index of the easy day
                        previousMileage.Add(lastDistance);
                    }

                    runButton.Click += runButton_click;
                    Grid.SetRow(runButton, jj);
                    Grid.SetColumn(runButton, ii + 1);
                    runDefGrid.Children.Add(runButton);
                    cycleRunDefList.Add(runButton);

                    totalMileage = totalMileage + runButton.getTotalMileage();
                    lastDistance = runButton.getTotalMileage();

                    //add the control to the list
                    cycleRunDefInputList.Add(runButton);
                }

                runDefInputList.Add(cycleRunDefList);


                //figure out easy miles
                double desiredCycleMiles = double.Parse(mileageList[jj].Text);
                double remainingMiles = Math.Max((desiredCycleMiles - totalMileage), 0);

                double dailyMiles = 0;
                int remainder = 0;

                while (dailyMiles < 3)
                {
                    dailyMiles = Math.Floor(remainingMiles / numEasyRuns);
                    remainder = (int)Math.Round(remainingMiles % numEasyRuns);
                    if (dailyMiles < 3)
                    {
                        numEasyRuns--;
                    }

                    if (numEasyRuns < 1)
                    {
                        numEasyRuns = 1;
                        break;
                    }
                }

                //sort easy days based on how hard the previous day was
                var sorted = previousMileage
                    .Select((x, i) => new KeyValuePair<double, int>(x, i))
                    .OrderBy(x => x.Key)
                    .ToList();

                List<double> sortedValue = sorted.Select(x => x.Key).ToList();
                List<int> idx = sorted.Select(x => x.Value).ToList();

                //divide up easy miles
                double[] easyMiles = new double[idx.Count];

                if (remainder == 0)
                {
                    //use base miles for harder days
                    for (int ii = 0; ii < numEasyRuns; ii++)
                    {
                        easyMiles[ii] = dailyMiles;
                    }
                }
                else
                {

                    for (int ii = 0; ii < numEasyRuns; ii++)
                    {
                        easyMiles[ii] = dailyMiles;
                    }

                    while (remainder > 0)
                    {
                        //add extra miles to easist days
                        for (int ii = 0; ii < numEasyRuns; ii++)
                        {
                            easyMiles[ii] = easyMiles[ii] + 1;
                            remainder--;

                            if (remainder == 0)
                            {
                                break;
                            }

                        }
                    }

                }
                //set the value of the easy controls
                for (int ii = 0; ii < idx.Count; ii++)
                {
                    int thisControlIndex = easyIndex[idx[ii]];
                    if (easyMiles[ii] == 0)
                    {
                        cycleRunDefInputList[thisControlIndex].setRunType(RunTypes.Rest);
                        cycleRunDefInputList[thisControlIndex].addWorkout(0, 0, Units.Miles, Pace.Easy, 0, Units.Miles, Pace.Easy);

                    }
                    else
                    {
                        cycleRunDefInputList[thisControlIndex].addWorkout(1, easyMiles[ii], Units.Miles, Pace.Easy, 0, Units.Miles, Pace.Easy);

                    }
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
                    //runButton.addWorkout(1,0,Units.Miles,Pace.Easy, 0, Units.Miles, Pace.Easy);
                    break;
                case "Long":
                    runButton.setRunType(RunTypes.Long);
                    runButton.addWorkout(1, Math.Min(Math.Round(cycleMileage * longSlider.Value / 100.0 * 2) / 2, (double.Parse(maxLongRunMiles.Text))), Units.Miles, Pace.Long, 0, Units.Miles, Pace.Easy);

                    break;
                case "Tempo":
                    runButton.setRunType(RunTypes.Tempo);
                    runButton.addWarmup(1.5, Units.Miles, Pace.Easy);
                    runButton.addWorkout(1, Math.Min(Math.Round(cycleMileage * tempoSlider.Value / 100.0 * 2) / 2, (double.Parse(maxTempoRunMiles.Text) - 3)), Units.Miles, Pace.HMP, 0, Units.Miles, Pace.Easy);
                    runButton.addCooldown(1.5, Units.Miles, Pace.Easy);
                    break;
                case "Interval":

                    double intervalMileage = Math.Min(Math.Round(cycleMileage * speedSlider.Value / 100.0 * 2) / 2, (double.Parse(maxTempoRunMiles.Text) - 3) / 2);
                    double repDistance = 400.0;
                    int reps = (int)Math.Round(intervalMileage / convertToMiles(repDistance, Units.Meters));

                    runButton.setRunType(RunTypes.Interval);
                    runButton.addWarmup(1.5, Units.Miles, Pace.Easy);

                    runButton.addWorkout(reps, repDistance, Units.Meters, Pace.Interval, repDistance, Units.Meters, Pace.Easy);

                    runButton.addCooldown(1.5, Units.Miles, Pace.Easy);
                    break;
                case "Rest":
                    runButton.setRunType(RunTypes.Rest);
                    break;
                case "Strides":
                    runButton.setRunType(RunTypes.Strides);
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

                    crossTrainInputList1[0].SelectedItem = "Rest";
                    crossTrainInputList1[1].SelectedItem = "Rest";
                    crossTrainInputList1[2].SelectedItem = "Rest";
                    crossTrainInputList1[3].SelectedItem = "Rest";
                    crossTrainInputList1[4].SelectedItem = "Rest";
                    crossTrainInputList1[5].SelectedItem = "Rest";
                    crossTrainInputList1[6].SelectedItem = "Rest";

                    crossTrainInputList2[0].SelectedItem = "Rest";
                    crossTrainInputList2[1].SelectedItem = "Rest";
                    crossTrainInputList2[2].SelectedItem = "Rest";
                    crossTrainInputList2[3].SelectedItem = "Rest";
                    crossTrainInputList2[4].SelectedItem = "Rest";
                    crossTrainInputList2[5].SelectedItem = "Rest";
                    crossTrainInputList2[6].SelectedItem = "Rest";

                    weightTrainInputList[0].SelectedItem = "W - Chest";
                    weightTrainInputList[1].SelectedItem = "W - Back";
                    weightTrainInputList[2].SelectedItem = "W - Legs";
                    weightTrainInputList[3].SelectedItem = "W - Core";
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

                    crossTrainInputList1[0].SelectedItem = "Rest";
                    crossTrainInputList1[1].SelectedItem = "Rest";
                    crossTrainInputList1[2].SelectedItem = "Rest";
                    crossTrainInputList1[3].SelectedItem = "Rest";
                    crossTrainInputList1[4].SelectedItem = "Rest";
                    crossTrainInputList1[5].SelectedItem = "Rest";
                    crossTrainInputList1[6].SelectedItem = "Rest";
                    crossTrainInputList1[7].SelectedItem = "Rest";

                    crossTrainInputList2[0].SelectedItem = "Rest";
                    crossTrainInputList2[1].SelectedItem = "Rest";
                    crossTrainInputList2[2].SelectedItem = "Rest";
                    crossTrainInputList2[3].SelectedItem = "Rest";
                    crossTrainInputList2[4].SelectedItem = "Rest";
                    crossTrainInputList2[5].SelectedItem = "Rest";
                    crossTrainInputList2[6].SelectedItem = "Rest";
                    crossTrainInputList2[7].SelectedItem = "Rest";

                    weightTrainInputList[0].SelectedItem = "W - Chest";
                    weightTrainInputList[1].SelectedItem = "W - Back";
                    weightTrainInputList[2].SelectedItem = "W - Legs";
                    weightTrainInputList[3].SelectedItem = "Rest";
                    weightTrainInputList[4].SelectedItem = "W - Core";
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

                    crossTrainInputList1[0].SelectedItem = "Rest";
                    crossTrainInputList1[1].SelectedItem = "Rest";
                    crossTrainInputList1[2].SelectedItem = "Bike";
                    crossTrainInputList1[3].SelectedItem = "Rest";
                    crossTrainInputList1[4].SelectedItem = "Rest";
                    crossTrainInputList1[5].SelectedItem = "Swim";
                    crossTrainInputList1[6].SelectedItem = "Rest";
                    crossTrainInputList1[7].SelectedItem = "Rest";
                    crossTrainInputList1[8].SelectedItem = "Swim";

                    crossTrainInputList2[0].SelectedItem = "Rest";
                    crossTrainInputList2[1].SelectedItem = "Rest";
                    crossTrainInputList2[2].SelectedItem = "Rest";
                    crossTrainInputList2[3].SelectedItem = "Rest";
                    crossTrainInputList2[4].SelectedItem = "Rest";
                    crossTrainInputList2[5].SelectedItem = "Rest";
                    crossTrainInputList2[6].SelectedItem = "Rest";
                    crossTrainInputList2[7].SelectedItem = "Rest";
                    crossTrainInputList2[8].SelectedItem = "Rest";

                    weightTrainInputList[0].SelectedItem = "Rest";
                    weightTrainInputList[1].SelectedItem = "W - Core";
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
                case "W - Core":
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
                case "HMP":
                    runCmbBox.Background = Brushes.Orange;
                    break;
                case "MP":
                    runCmbBox.Background = Brushes.Orange;
                    break;
                case "Hills":
                    runCmbBox.Background = Brushes.Orange;
                    break;
                case "TenK":
                    runCmbBox.Background = Brushes.Orange;
                    break;
                case "FiveK":
                    runCmbBox.Background = Brushes.Orange;
                    break;
                case "Repition":
                    runCmbBox.Background = Brushes.Orange;
                    break;
                case "Progression":
                    runCmbBox.Background = Brushes.Orange;
                    break;
                case "Strides":
                    runCmbBox.Background = Brushes.Pink;
                    break;
                default:
                    break;
            }
        }

        //ToDo: this method should be combined with the other one, need to add combo box items rather than strings to the list
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
                case "HMP":
                    runCmbBox.Background = Brushes.Orange;
                    break;
                case "MP":
                    runCmbBox.Background = Brushes.Orange;
                    break;
                case "Hills":
                    runCmbBox.Background = Brushes.Orange;
                    break;
                case "TenK":
                    runCmbBox.Background = Brushes.Orange;
                    break;
                case "FiveK":
                    runCmbBox.Background = Brushes.Orange;
                    break;
                case "Repition":
                    runCmbBox.Background = Brushes.Orange;
                    break;
                case "Progression":
                    runCmbBox.Background = Brushes.Orange;
                    break;
                case "Strides":
                    runCmbBox.Background = Brushes.Pink;
                    break;
                default:
                    break;


            }
        }

        private void runButton_click(object sender, EventArgs e)
        {
            currentRunButton = (RunButton)sender;

            //remove all but one set panel
            for (int ii = 0; ii < currentSetPanels.Count; ii++)
            {
                setStackPanel.Children.Remove(currentSetPanels[ii]);
            }

            //clear out list
            currentSetPanels = new List<SetPanel>();

            RunButton runButton = (RunButton)sender;

            String stringRunType = runButton.getRunType().ToString();

            runType.SelectedValue = runButton.getRunType().ToString();
            totalDistanceDisp.Text = runButton.getTotalMileage().ToString();
            totalDistanceUnits.SelectedValue = "Miles";

            int numSets = runButton.getRepDistance().Count;

            List<int> reps = runButton.getNumberOfReps();

            List<double> repDistance = runButton.getRepDistance();
            List<Units> repDistanceUnits = runButton.getRepUnits();
            List<Pace> repPace = runButton.getRepPace();

            List<double> repCoolDistance = runButton.getRepCoolDistance();
            List<Units> repCoolDistanceUnits = runButton.getRepCoolUnits();
            List<Pace> repCoolPace = runButton.getRepCoolPace();

            if (numSets == 0)
            {
                if (currentSetPanels.Count == 0)
                {
                    SetPanel newSetPanel = addSetPanel(currentSetPanels.Count + 1);
                    currentSetPanels.Add(newSetPanel);
                }

                currentSetPanels[0].repsTextInput.Text = "0";
                currentSetPanels[0].distTextInput.Text = "0";
                currentSetPanels[0].coolTextInput.Text = "0";
                currentSetPanels[0].unitsComboInput.SelectedValue = "Miles";
                currentSetPanels[0].paceComboInput.SelectedValue = "Easy";
            }
            else
            {
                for (int ii = 0; ii < numSets; ii++)
                {
                    if (currentSetPanels.Count < (ii + 1))
                    {
                        SetPanel newSetPanel = addSetPanel(currentSetPanels.Count + 1);
                        currentSetPanels.Add(newSetPanel);
                    }
                    currentSetPanels[ii].repsTextInput.Text = reps[ii].ToString();
                    currentSetPanels[ii].distTextInput.Text = repDistance[ii].ToString();
                    currentSetPanels[ii].coolTextInput.Text = repCoolDistance[ii].ToString();
                    currentSetPanels[ii].unitsComboInput.SelectedValue = repDistanceUnits[ii].ToString();
                    currentSetPanels[ii].paceComboInput.SelectedValue = repPace[ii].ToString();
                }
            }
            warmupDistanceInput.Text = runButton.getWarmupDistance().ToString();
            warmupDistanceUnits.SelectedValue = runButton.getWarmupUnits().ToString();

            coolDownDistanceInput.Text = runButton.getCoolDistance().ToString();
            coolDownDistanceUnits.SelectedValue = runButton.getCoolUnits().ToString();

        }

        private void addSet_click(object sender, EventArgs e)
        {

            SetPanel newSetPanel = addSetPanel(currentSetPanels.Count + 1);
            currentSetPanels.Add(newSetPanel);

        }

        private SetPanel addSetPanel(int setNumber)
        {
            //create setpanel with set number
            SetPanel setPanel = new SetPanel(setNumber);

            //add callback to pace combo box
            setPanel.paceComboInput.SelectionChanged += runComboBox_ValueChanged;

            //add panel to the view
            setStackPanel.Children.Add(setPanel);

            return setPanel;
        }


        private void removeSet_click(object sender, EventArgs e)
        {
            if (currentSetPanels.Count > 1)
            {
                //get the last setpanel
                SetPanel lastSetPanel = currentSetPanels[currentSetPanels.Count - 1];

                //remove it from the windown
                setStackPanel.Children.Remove(lastSetPanel);

                //remove it from the list
                currentSetPanels.Remove(lastSetPanel);
            }
        }

        private void updateRunButton_Click(object sender, EventArgs e)
        {


            //clear out old data
            //currentRunButton = new RunButton();
            currentRunButton.clearData();

            //set run type
            currentRunButton.setRunType((RunTypes)Enum.Parse(typeof(RunTypes), runType.SelectedValue.ToString(), true));

            //add warmup
            double warmupDistance = double.Parse(warmupDistanceInput.Text);
            Units warmupUnits = (Units)Enum.Parse(typeof(Units), warmupDistanceUnits.SelectedValue.ToString(), true);
            currentRunButton.addWarmup(warmupDistance, warmupUnits, Pace.Easy);

            //workout
            for (int ii = 0; ii < currentSetPanels.Count; ii++)
            {
                int numReps = (int)Math.Round(double.Parse(currentSetPanels[ii].repsTextInput.Text));
                double repDistance = double.Parse(currentSetPanels[ii].distTextInput.Text);
                Units repUnits = (Units)Enum.Parse(typeof(Units), currentSetPanels[ii].unitsComboInput.SelectedValue.ToString(), true);
                Pace repPace = (Pace)Enum.Parse(typeof(Pace), currentSetPanels[ii].paceComboInput.SelectedValue.ToString(), true);
                double coolDistance = double.Parse(currentSetPanels[ii].coolTextInput.Text);
                currentRunButton.addWorkout(numReps, repDistance, repUnits, repPace, coolDistance, repUnits, repPace);
            }

            //add cooldown
            double coolDownDistance = double.Parse(coolDownDistanceInput.Text);
            Units coolDownUnits = (Units)Enum.Parse(typeof(Units), coolDownDistanceUnits.SelectedValue.ToString(), true);
            currentRunButton.addCooldown(coolDownDistance, coolDownUnits, Pace.Easy);

            calculateTotalMiles();

        }

        private void targetRaceDay_Changed(object sender, EventArgs e)
        {
            DateTime startDate = (DateTime)targetRaceDateInput.SelectedDate;
            targetTrainingStartDate.SelectedDate = startDate.AddDays(double.Parse(numCyclesInput.Text) * double.Parse(numDaysInput.Text) * -1 + 1);

        }

        private void calculateTotalMiles()
        {
            int numCycles = (int)Math.Ceiling(double.Parse(numCyclesInput.Text));
            int numDays = (int)Math.Ceiling(double.Parse(numDaysInput.Text));

            for (int ii =0; ii<numCycles; ii++)
            {
                double totalMileage = 0;
                for (int jj = 0; jj < numDays; jj++)
                {
                    RunButton thisRunButton = runDefInputList[ii][jj];

                    totalMileage = totalMileage + thisRunButton.getTotalMileage();

                }

                double targetMileage = double.Parse(mileageList[ii].Text);

                actualMileageList[ii].Text = (Math.Round(totalMileage*10)/10).ToString();

                //TODO: Need to get this working
                if(totalMileage> targetMileage)
                {
                    actualMileageList[ii].Background = Brushes.Red;
                }else
                {
                    actualMileageList[ii].Background = Brushes.Green;
                }

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

        //private void newMenuItem_Click(object sender, EventArgs e)
        //{

        //}

        private void loadMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Title = "Select File To Load";
            openFileDialog.DefaultExt = ".xml";
            openFileDialog.Filter = "Settings (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.RestoreDirectory = true;

            // Show save file dialog box
            Nullable<bool> result = openFileDialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filePath = openFileDialog.FileName;
                this.Title = "Training Paln Builder - Opening...";
                loadSettings(filePath);
                this.Title = "Training Paln Builder";
            }

           
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = @"C:\";
            saveFileDialog.Title = "Select File To Save";
            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.Filter = "Settings (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.CheckFileExists = false;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.RestoreDirectory = true;

            // Show save file dialog box
            Nullable<bool> result = saveFileDialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filePath = saveFileDialog.FileName;
                this.Title = "Training Paln Builder - Saving...";
                saveSettings(filePath);
                this.Title = "Training Paln Builder";
            }

        }


        private void excelExportMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = @"C:\";
            saveFileDialog.Title = "Select File To Save";
            saveFileDialog.DefaultExt = "xlsx";
            saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.CheckFileExists = false;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.RestoreDirectory = true;

            // Show save file dialog box
            Nullable<bool> result = saveFileDialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filePath = saveFileDialog.FileName;
                this.Title = "Training Plan Builder - Saving...";
                exportWorkoutToExcel(filePath);
                this.Title = "Training Plan Builder";
            }
        }

        private void exportWorkoutToExcel(string filePath)
        {
            List<List<ComboBox>> weightTrainCycleList = new List<List<ComboBox>>();
            List<List<ComboBox>> crossTrainCycleList1 = new List<List<ComboBox>>();
            List<List<ComboBox>> crossTrainCycleList2 = new List<List<ComboBox>>();
            List<List<RunButton>> runCycleList = new List<List<RunButton>>();

            int numCycles = (int)Math.Ceiling(double.Parse(numCyclesInput.Text));
            int numDays = (int)Math.Ceiling(double.Parse(numDaysInput.Text));

            //write cycle data
            for (int ii = 0; ii < numCycles; ii++)
            {
                weightTrainCycleList.Add(weightTrainInputList);
                crossTrainCycleList1.Add(crossTrainInputList1);
                crossTrainCycleList2.Add(crossTrainInputList2);
            }

            runCycleList = runDefInputList;

            //create workbook
            ExcelExportUtility excelExportUtility = new ExcelExportUtility();
            excelExportUtility.createWorkbook();

            //write cycle data
            exportExcelSheet(excelExportUtility,2, runDefInputList, weightTrainCycleList, crossTrainCycleList1, crossTrainCycleList2, filePath);


            weightTrainCycleList = new List<List<ComboBox>>();
            crossTrainCycleList1 = new List<List<ComboBox>>();
            crossTrainCycleList2 = new List<List<ComboBox>>();
            runCycleList = new List<List<RunButton>>();

            List<ComboBox> weightTrainWeekList = new List<ComboBox>();
            List<ComboBox> crossTrainWeekList1 = new List<ComboBox>();
            List<ComboBox> crossTrainWeekList2 = new List<ComboBox>();
            List<RunButton> runWeekList = new List<RunButton>();

            ComboBox defaultCTBox = new ComboBox();
            defaultCTBox.Items.Add("Rest");
            defaultCTBox.SelectedItem = "Rest";

            RunButton defaultRunButton = new RunButton();
            defaultRunButton.setRunType(RunTypes.Rest);

            //resort the date time so it starts on a monday
            DateTime currDate = (DateTime)targetTrainingStartDate.SelectedDate;

            int dayOfWeek = (int)currDate.DayOfWeek;

            //should this be zero index
            for (int ii = 0; ii < dayOfWeek-1; ii++)
            {
                runWeekList.Add(defaultRunButton);
                weightTrainWeekList.Add(defaultCTBox);
                crossTrainWeekList1.Add(defaultCTBox);
                crossTrainWeekList2.Add(defaultCTBox);
            }

            int dayCount = dayOfWeek;
            for (int ii = 0; ii < numCycles; ii++)
            {
                for (int jj = 0; jj < numDays; jj++)
                {

                    runWeekList.Add(runDefInputList[ii][jj]);
                    weightTrainWeekList.Add(weightTrainInputList[jj]);
                    crossTrainWeekList1.Add(crossTrainInputList1[jj]);
                    crossTrainWeekList2.Add(crossTrainInputList2[jj]);

                    if (dayCount == 7)
                    {
                        runCycleList.Add(runWeekList);
                        weightTrainCycleList.Add(weightTrainWeekList);
                        crossTrainCycleList1.Add(crossTrainWeekList1);
                        crossTrainCycleList2.Add(crossTrainWeekList2);

                        weightTrainWeekList = new List<ComboBox>();
                        crossTrainWeekList1 = new List<ComboBox>();
                        crossTrainWeekList2 = new List<ComboBox>();
                        runWeekList = new List<RunButton>();
                        dayCount = 0;
                    }

                    dayCount++;
                }

            }



            if (dayCount != 1)
            {
                //fill in the blanks
                for (int ii = dayCount - 1; ii < 7; ii++)
                {
                    runWeekList.Add(defaultRunButton);
                    weightTrainWeekList.Add(defaultCTBox);
                    crossTrainWeekList1.Add(defaultCTBox);
                    crossTrainWeekList2.Add(defaultCTBox);
                }

                runCycleList.Add(runWeekList);
                weightTrainCycleList.Add(weightTrainWeekList);
                crossTrainCycleList1.Add(crossTrainWeekList1);
                crossTrainCycleList2.Add(crossTrainWeekList2);
            }

            //write week data
            exportExcelSheet(excelExportUtility,1, runCycleList, weightTrainCycleList, crossTrainCycleList1, crossTrainCycleList2, filePath);

            //save the workbook
            try
            {
                excelExportUtility.saveWorkbook(filePath);
            }
            catch
            {
                Thread thread = new Thread(() => MessageBox.Show("Something went wrong saving to excel...", "Error", MessageBoxButton.OK, MessageBoxImage.Error));
                thread.Start();
            }

        }

        private void exportExcelSheet(ExcelExportUtility excelExportUtility, int sheetIdx, List<List<RunButton>> runButtonList, List<List<ComboBox>> weightTrainCycleList, List<List<ComboBox>> crossTrainCycleList1, List<List<ComboBox>> crossTrainCycleList2, string filePath)
        {


            //get number of cycles and number of days
            int numCycles = runButtonList.Count;
            int numDays = runButtonList[0].Count;

            //initilize row pointer
            int rowPointer = 1;

            //loop through days to create header with day number
            for (int jj = 0; jj < numDays; jj++)
            {
                excelExportUtility.writeSingleCell(rowPointer, jj + 2, "Day " + (jj + 1).ToString(), sheetIdx);
                excelExportUtility.setCellColor(rowPointer, jj + 2, Brushes.LightGray, sheetIdx);
                excelExportUtility.setNormalBorderOutline(rowPointer, jj + 2, rowPointer, jj + 2, sheetIdx);
            }
            //increment row pointer
            rowPointer = rowPointer + 1;

            //get training start date
            DateTime currDate = (DateTime)targetTrainingStartDate.SelectedDate;

            if (numDays == 7)
            {
                int dayOfWeek = (int)currDate.DayOfWeek;
                int deltaDays = 1 - dayOfWeek;
                currDate = currDate.AddDays(deltaDays);
            }

            //loop through each cycle to extrat information
            for (int ii = 0; ii < numCycles; ii++)
            {
                //set start of cycle for thick boarder
                int rowStart = rowPointer;

                //write the cycle number and date to the first column
                excelExportUtility.writeSingleCell(rowPointer, 1, "Cycle " + (ii + 1), sheetIdx);
                excelExportUtility.writeSingleCell(rowPointer + 1, 1, currDate, sheetIdx);
                excelExportUtility.writeSingleCell(rowPointer + 3, 1, "Target Mileage", sheetIdx);
                excelExportUtility.writeSingleCell(rowPointer + 4, 1, "Actual Mileage", sheetIdx);
                excelExportUtility.setNormalBorderOutline(rowPointer, 1, rowPointer, 1, sheetIdx);

                //loop though each day
                for (int jj = 0; jj < numDays; jj++)
                {
                    //get current run button
                    RunButton thisRunButton = runButtonList[ii][jj];

                    //get run type of this button
                    RunTypes thisRunType = thisRunButton.getRunType();

                    //init text to write for run and total mileage
                    string runText = "";
                    double totalMileage = 0;

                    //get data for workouts
                    List<int> workoutReps = thisRunButton.getNumberOfReps();
                    List<double> workoutDist = thisRunButton.getRepDistance();
                    List<Pace> workoutPace = thisRunButton.getRepPace();
                    List<Units> workoutUnits = thisRunButton.getRepUnits();
                    int numSets = thisRunButton.getNumberOfReps().Count;

                    //init brush color
                    SolidColorBrush cellColor = Brushes.LightBlue;

                    //create text based on runtype
                    switch (thisRunType)
                    {
                        case RunTypes.Easy:
                            runText = thisRunButton.getTotalMileage().ToString() + "Miles Easy";
                            totalMileage = thisRunButton.getTotalMileage();
                            cellColor = Brushes.LightGreen;
                            break;
                        case RunTypes.Tempo:
                            //get warmup info
                            runText = thisRunButton.getWarmupDistance().ToString() + thisRunButton.getWarmupUnits().ToString() + " Warmup \n";

                            //loop through each set
                            for (int kk = 0; kk < numSets; kk++)
                            {
                                runText = runText + workoutReps[kk].ToString() + "X" + workoutDist[kk].ToString() + workoutUnits[kk].ToString() + " @ " + workoutPace[kk].ToString() + "\n";
                            }

                            //get cool down info
                            runText = runText + thisRunButton.getCoolDistance().ToString() + thisRunButton.getCoolUnits().ToString() + " Cooldown";
                            cellColor = Brushes.Orange;
                            break;
                        case RunTypes.Interval:
                            //get warmup info
                            runText = thisRunButton.getWarmupDistance().ToString() + thisRunButton.getWarmupUnits().ToString() + " Warmup \n";

                            //loop through each set
                            for (int kk = 0; kk < numSets; kk++)
                            {
                                runText = runText + workoutReps[kk].ToString() + "X" + workoutDist[kk].ToString() + workoutUnits[kk].ToString() + " @ " + workoutPace[kk].ToString() + "\n";
                            }

                            //get cool down info
                            runText = runText + thisRunButton.getCoolDistance().ToString() + thisRunButton.getCoolUnits().ToString() + " Cooldown";
                            cellColor = Brushes.Orange;
                            break;

                        case RunTypes.Strides:
                            //get warmup info
                            //runText = thisRunButton.getWarmupDistance().ToString() + thisRunButton.getWarmupUnits().ToString() + " Warmup \n";

                            //loop through each set
                            for (int kk = 0; kk < numSets; kk++)
                            {
                                runText = runText + workoutReps[kk].ToString() + "X" + workoutDist[kk].ToString() + workoutUnits[kk].ToString() + " @ " + workoutPace[kk].ToString() + "\n";
                            }

                            //get cool down info
                            //runText = runText + thisRunButton.getCoolDistance().ToString() + thisRunButton.getCoolUnits().ToString() + " Cooldown";
                            cellColor = Brushes.Pink;
                            break;

                        case RunTypes.Long:
                            runText = thisRunButton.getTotalMileage().ToString() + "Miles Easy";
                            totalMileage = thisRunButton.getTotalMileage();
                            cellColor = Brushes.Green;
                            break;
                        case RunTypes.Rest:
                            runText = "Rest";
                            totalMileage = thisRunButton.getTotalMileage();
                            cellColor = Brushes.LightBlue;
                            break;
                        case RunTypes.Repition:
                            //get warmup info
                            runText = thisRunButton.getWarmupDistance().ToString() + thisRunButton.getWarmupUnits().ToString() + " Warmup \n";

                            //loop through each set
                            for (int kk = 0; kk < numSets; kk++)
                            {
                                runText = runText + workoutReps[kk].ToString() + "X" + workoutDist[kk].ToString() + workoutUnits[kk].ToString() + " @ " + workoutPace[kk].ToString() + "\n";
                            }

                            //get cool down info
                            runText = runText + thisRunButton.getCoolDistance().ToString() + thisRunButton.getCoolUnits().ToString() + " Cooldown";
                            cellColor = Brushes.Orange;
                            break;
                    }

                    //write the data to a cell
                    excelExportUtility.writeSingleCell(rowPointer, jj + 2, runText, sheetIdx);
                    excelExportUtility.setCellColor(rowPointer, jj + 2, cellColor,sheetIdx);
                    excelExportUtility.setNormalBorderOutline(rowPointer, jj + 2, rowPointer, jj + 2, sheetIdx);

                }
                //increment row pointer
                rowPointer = rowPointer + 1;

                //loop through each day to extract weight lift info
                for (int jj = 0; jj < numDays; jj++)
                {
                    //write date
                    excelExportUtility.writeSingleCell(rowPointer, jj + 2, weightTrainCycleList[ii][jj].SelectedItem.ToString(),sheetIdx);
                    excelExportUtility.setNormalBorderOutline(rowPointer, jj + 2, rowPointer, jj + 2, sheetIdx);

                    //set color to yellow if there is a workout
                    if (weightTrainCycleList[ii][jj].SelectedItem.ToString() != "Rest")
                    {
                        excelExportUtility.setCellColor(rowPointer, jj + 2, Brushes.Yellow, sheetIdx);
                    }
                    else
                    {
                        excelExportUtility.setCellColor(rowPointer, jj + 2, Brushes.LightBlue, sheetIdx);
                    }
                }
                //set row hieght
                excelExportUtility.setRowHeight(rowPointer, sheetIdx, 20);

                //increment row pointer
                rowPointer = rowPointer + 1;

                //create text for each days cross train
                for (int jj = 0; jj < numDays; jj++)
                {
                    //extract information
                    string crossTrain1 = crossTrainCycleList1[ii][jj].SelectedItem.ToString();
                    string crossTrain2 = crossTrainCycleList2[ii][jj].SelectedItem.ToString();

                    string output = "";

                    //set text based on populated info
                    if (crossTrain1 == "Rest" && crossTrain1 == "Rest")
                    {
                        output = "Rest";
                        excelExportUtility.setCellColor(rowPointer, jj + 2, Brushes.LightBlue, sheetIdx);
                    }
                    else if (crossTrain1 != "Rest" && crossTrain2 == "Rest")
                    {
                        output = crossTrain1;
                        excelExportUtility.setCellColor(rowPointer, jj + 2, Brushes.Yellow, sheetIdx);

                    }
                    else if (crossTrain1 == "Rest" && crossTrain2 != "Rest")
                    {
                        output = crossTrain2;
                        excelExportUtility.setCellColor(rowPointer, jj + 2, Brushes.Yellow, sheetIdx);
                    }
                    else
                    {
                        output = crossTrain1 + "+" + crossTrain2;
                        excelExportUtility.setCellColor(rowPointer, jj + 2, Brushes.Yellow, sheetIdx);
                    }

                    //write data to excel
                    excelExportUtility.writeSingleCell(rowPointer, jj + 2, output, sheetIdx);
                    excelExportUtility.setNormalBorderOutline(rowPointer, jj + 2, rowPointer, jj + 2, sheetIdx);
                }
                //set row height
                excelExportUtility.setRowHeight(rowPointer,sheetIdx, 20);

                //increment row pointer
                rowPointer = rowPointer + 1;

                //set data for mileage total for each day and week
                double weeklyTotalMileage = 0;
                for (int jj = 0; jj < numDays; jj++)
                {
                    RunButton thisRunButton = runButtonList[ii][jj];
                    weeklyTotalMileage = thisRunButton.getTotalMileage() + weeklyTotalMileage;
                    excelExportUtility.writeSingleCell(rowPointer, jj + 2, (Math.Round(thisRunButton.getTotalMileage() * 10) / 10).ToString(),sheetIdx);
                    excelExportUtility.setNormalBorderOutline(rowPointer, jj + 2, rowPointer, jj + 2, sheetIdx);
                    excelExportUtility.setNormalBorderOutline(rowPointer+1, jj + 2, rowPointer+1, jj + 2, sheetIdx);
                }
                excelExportUtility.writeSingleCell(rowPointer, numDays + 2, (Math.Round(weeklyTotalMileage * 10) / 10).ToString(),sheetIdx);
                excelExportUtility.setRowHeight(rowPointer, sheetIdx, 20);
                excelExportUtility.setRowHeight(rowPointer+1, sheetIdx, 20);

                //add row to record actual mileage
                rowPointer++;

                //draw thick boarder around cycle
                int rowEnd = rowPointer;
                excelExportUtility.setThickBorderOutline(rowStart, 1, rowEnd, numDays + 1, sheetIdx);

                //increment row pointer and cycle start date
                rowPointer = rowPointer + 1;
                currDate = currDate.AddDays(numDays);


            }
            ////save the workbook
            //try
            //{
            //    excelExportUtility.saveWorkbook(filePath);
            //}
            //catch
            //{
            //    Thread thread = new Thread(() => MessageBox.Show("Something went wrong saving to excel...", "Error", MessageBoxButton.OK, MessageBoxImage.Error));
            //    thread.Start();
            //}
        }

        private void saveSettings(string filePath)
        {

            int numCycles = (int)Math.Ceiling(double.Parse(numCyclesInput.Text));
            int numDays = (int)Math.Ceiling(double.Parse(numDaysInput.Text));
            ApplicationSettings appSettings = new ApplicationSettings();

            for(int ii = 0; ii< numCycles; ii++)
            {
                appSettings.targetMileage.Add(mileageList[ii].Text);
                appSettings.actualMileage.Add(actualMileageList[ii].Text);
            }

            appSettings.targetRaceDay = (DateTime)targetRaceDateInput.SelectedDate;
            appSettings.numberOfCycles = numCyclesInput.Text;
            appSettings.numDaysInCycle = numDaysInput.Text;
            appSettings.startingMileage = minMileageInput.Text;
            appSettings.endingMileage = maxMileageInput.Text;
            appSettings.cycleIncrease = cycleMileageIncrease.Text;
            appSettings.cycleDelta = cycleMileageDeltaInput.Text;
            appSettings.numCyclesReset = numCyclesReset.Text;

            for (int ii = 0; ii < numDays; ii++)
            {
                appSettings.dailyRunTypeList.Add(runInputList[ii].SelectedItem.ToString());
                appSettings.dailyWorkoutTypeList.Add(weightTrainInputList[ii].SelectedItem.ToString());
                appSettings.CT1TypeList.Add(crossTrainInputList1[ii].SelectedItem.ToString());
                appSettings.CT2TypeList.Add(crossTrainInputList2[ii].SelectedItem.ToString());
            }

            for (int ii = 0; ii < numCycles; ii++)
            {

                List<RunTypes> runTypesList = new List<RunTypes>();

                List<double> warmupDistance = new List<double>();
                List<Pace> warmupPace = new List<Pace>();
                List<Units> warmupUnits = new List<Units>();

                List<List<int>> numReps = new List<List<int>>();
                List<int> numSets = new List<int>();
                List<List<double>> repDistance = new List<List<double>>();
                List<List<Pace>> repPace = new List<List<Pace>>();
                List<List<Units>> repUnits = new List<List<Units>>();
                List<List<double>> repCoolDistance = new List<List<double>>();
                List<List<Pace>> repCoolPace = new List<List<Pace>>();
                List<List<Units>> repCoolUnits = new List<List<Units>>();

                List<double> coolDownDistance = new List<double>();
                List<Pace> coolDownPace = new List<Pace>();
                List<Units> coolDownUnits = new List<Units>();

                for (int jj = 0; jj < numDays; jj++)
                {
                    RunButton thisRunButton = runDefInputList[ii][jj];

                    runTypesList.Add(thisRunButton.getRunType());

                    warmupDistance.Add(thisRunButton.getWarmupDistance());
                    warmupPace.Add(thisRunButton.getWarmupPace());
                    warmupUnits.Add(thisRunButton.getWarmupUnits());

                    numSets.Add(thisRunButton.getNumberOfReps().Count);
                    numReps.Add(thisRunButton.getNumberOfReps());
                    repDistance.Add(thisRunButton.getRepDistance());
                    repUnits.Add(thisRunButton.getRepUnits());
                    repPace.Add(thisRunButton.getRepPace());
                    repCoolDistance.Add(thisRunButton.getRepCoolDistance());
                    repCoolUnits.Add(thisRunButton.getRepCoolUnits());
                    repCoolPace.Add(thisRunButton.getRepCoolPace());

                    coolDownDistance.Add(thisRunButton.getCoolDistance());
                    coolDownPace.Add(thisRunButton.getCoolPace());
                    coolDownUnits.Add(thisRunButton.getCoolUnits());
                }

                appSettings.runType.Add(runTypesList);

                appSettings.warmupDist.Add(warmupDistance);
                appSettings.warmupPace.Add(warmupPace);
                appSettings.warmupUnits.Add(warmupUnits);

                appSettings.numSets.Add(numSets);
                appSettings.numReps.Add(numReps);
                appSettings.repDistance.Add(repDistance);
                appSettings.repUnits.Add(repUnits);
                appSettings.repPace.Add(repPace);
                appSettings.repCoolDistance.Add(repCoolDistance);
                appSettings.repCoolUnits.Add(repCoolUnits);
                appSettings.repCoolPace.Add(repCoolPace);

                appSettings.coolDownDist.Add(coolDownDistance);
                appSettings.coolDownPace.Add(coolDownPace);
                appSettings.coolDownUnits.Add(coolDownUnits); 

            }

            appSettings.Save(filePath);

        }

        private void loadSettings(string filePath)
        {

            ApplicationSettings.Load(filePath);
            ApplicationSettings appSettings = ApplicationSettings.Instance;

            int numCycles = (int)double.Parse(appSettings.numberOfCycles);
            int numDays = (int)double.Parse(appSettings.numDaysInCycle);

            numCyclesInput.Text = appSettings.numberOfCycles;
            numDaysInput.Text = appSettings.numDaysInCycle;
            targetRaceDateInput.SelectedDate = appSettings.targetRaceDay;
            numCyclesInput.Text = appSettings.numberOfCycles;
            numDaysInput.Text = appSettings.numDaysInCycle;
            minMileageInput.Text = appSettings.startingMileage;
            maxMileageInput.Text = appSettings.endingMileage;
            cycleMileageIncrease.Text = appSettings.cycleIncrease;
            cycleMileageDeltaInput.Text = appSettings.cycleDelta;
            numCyclesReset.Text = appSettings.numCyclesReset;

            //create the mileage table
            mileageDefStackPanel.Children.Remove(mileageGrid);
            createMileageTable((int)double.Parse(numCyclesInput.Text));

            for (int ii = 0; ii < numCycles; ii++)
            {
                mileageList[ii].Text = appSettings.targetMileage[ii];
                actualMileageList[ii].Text = appSettings.actualMileage[ii];
            }

            //create run type selctions
            WorkoutDefStackPanel.Children.Remove(dayNumberGrid);
            WorkoutDefStackPanel.Children.Remove(runTypeGrid);
            WorkoutDefStackPanel.Children.Remove(weightTrainGrid);
            WorkoutDefStackPanel.Children.Remove(crossTrainGrid1);
            WorkoutDefStackPanel.Children.Remove(crossTrainGrid2);
            createRunTypeSelectorTable();

            //create run def grid
            runDefStackPanel.Children.Remove(runDefGrid);
            createRunDefinitionTable();


            for (int ii = 0; ii < numDays; ii++)
            {
                runInputList[ii].SelectedItem = appSettings.dailyRunTypeList[ii];
                weightTrainInputList[ii].SelectedItem = appSettings.dailyWorkoutTypeList[ii];
                crossTrainInputList1[ii].SelectedItem = appSettings.CT1TypeList[ii];
                crossTrainInputList2[ii].SelectedItem = appSettings.CT2TypeList[ii];
            }

            for (int ii = 0; ii < numCycles; ii++)
            {

                List<RunTypes> runTypesList = appSettings.runType[ii];

                List<double> warmupDistance = appSettings.warmupDist[ii];
                List<Pace> warmupPace = appSettings.warmupPace[ii];
                List<Units> warmupUnits = appSettings.warmupUnits[ii];

                List<List<int>> numReps = appSettings.numReps[ii];
                List<int> numSets = appSettings.numSets[ii];
                List<List<double>> repDistance = appSettings.repDistance[ii];
                List<List<Pace>> repPace = appSettings.repPace[ii];
                List<List<Units>> repUnits = appSettings.repUnits[ii];
                List<List<double>> repCoolDistance = appSettings.repCoolDistance[ii];
                List<List<Pace>> repCoolPace = appSettings.repCoolPace[ii];
                List<List<Units>> repCoolUnits = appSettings.repCoolUnits[ii];

                List<double> coolDownDistance = appSettings.coolDownDist[ii];
                List<Pace> coolDownPace = appSettings.coolDownPace[ii];
                List<Units> coolDownUnits = appSettings.coolDownUnits[ii];

                for (int jj = 0; jj < numDays; jj++)
                {
                    RunButton currentRunButton = runDefInputList[ii][jj];

                    //clear out old data
                    //currentRunButton = new RunButton();
                    currentRunButton.clearData();

                    //set run type
                    currentRunButton.setRunType(runTypesList[jj]);

                    //add warmup
                    currentRunButton.addWarmup(warmupDistance[jj], warmupUnits[jj], warmupPace[jj]);

                    //workout
                    for (int kk = 0; kk < numSets[jj]; kk++)
                    {
                        currentRunButton.addWorkout(numReps[jj][kk], repDistance[jj][kk], repUnits[jj][kk], repPace[jj][kk], repCoolDistance[jj][kk], repCoolUnits[jj][kk], repCoolPace[jj][kk]);
                    }

                    //add cooldown
                    currentRunButton.addCooldown(coolDownDistance[jj], coolDownUnits[jj], coolDownPace[jj]);

                    calculateTotalMiles();
                }



            }
        }
    }
}



