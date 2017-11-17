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
using WorkoutAppWPF.CustomControls;
using PlotTools = System.Windows.Forms.DataVisualization.Charting;

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
        Grid crossTrainGrid1;
        Grid crossTrainGrid2;
        Grid runDefGrid;

        List<TextBox> mileageList;
        List<ComboBox> crossTrainInputList1;
        List<ComboBox> crossTrainInputList2;
        List<ComboBox> runInputList;
        List<ComboBox> weightTrainInputList;
        List<List<RunButton>> runDefInputList;
        List<SetPanel> currentSetPanels = new List<SetPanel>();
        RunButton currentRunButton;

        double[] xData;
        double[] yDataTarget;
        double[] yDataTempo;
        double[] yDataSpeed;
        double[] yDataLong;
        double[] yDataEasy;
        PlotTools.Chart mileageChart;

        public MainWindowApp()
        {
            InitializeComponent();

            setDefaults();

            createMileageTable((int)double.Parse(numCyclesInput.Text));
            createRunTypeSelectorTable();

            createRunDefinitionTable();

            SetPanel setPanel = addSetPanel(1);
            currentSetPanels.Add(setPanel);

            TextBox thisTextBox = mileageList[0];


        }

        private void setDefaults()
        {
            numCyclesInput.TextChanged -= numCycles_Changed;

            numCyclesInput.Text = 16.ToString();
            numDaysInput.Text = 9.ToString();
            numWeeksInput.Text = (Math.Round(double.Parse(numCyclesInput.Text) * double.Parse(numDaysInput.Text) / 7 * 10) / 10).ToString();
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


            //WorkoutDefStackPanel.Children.Remove(dayNumerGrid);
            //WorkoutDefStackPanel.Children.Remove(runTypeGrid);
            //WorkoutDefStackPanel.Children.Remove(weightTrainGrid);
            //WorkoutDefStackPanel.Children.Remove(crossTrainGrid);
            //createRunTypeSelectorTable();

            //runDefStackPanel.Children.Remove(runDefGrid);
            //createRunDefinitionTable();

        }

        private void recalcRunTable_click(object sender, EventArgs e)
        {
            WorkoutDefStackPanel.Children.Remove(dayNumerGrid);
            WorkoutDefStackPanel.Children.Remove(runTypeGrid);
            WorkoutDefStackPanel.Children.Remove(weightTrainGrid);
            WorkoutDefStackPanel.Children.Remove(crossTrainGrid1);
            WorkoutDefStackPanel.Children.Remove(crossTrainGrid2);
            createRunTypeSelectorTable();

            runDefStackPanel.Children.Remove(runDefGrid);
            createRunDefinitionTable();

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

            dayNumerGrid = new Grid();
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

                dayNumerGrid.ColumnDefinitions.Add(gridCol1);
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

            WorkoutDefStackPanel.Children.Add(dayNumerGrid);
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
                runDefInputList.Add(cycleRunDefInputList);

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

                    crossTrainInputList1[0].SelectedItem = "Rest";
                    crossTrainInputList1[1].SelectedItem = "Rest";
                    crossTrainInputList1[2].SelectedItem = "Swim";
                    crossTrainInputList1[3].SelectedItem = "Bike";
                    crossTrainInputList1[4].SelectedItem = "Rest";
                    crossTrainInputList1[5].SelectedItem = "Bike";
                    crossTrainInputList1[6].SelectedItem = "Swim";
                    crossTrainInputList1[7].SelectedItem = "Rest";
                    crossTrainInputList1[7].SelectedItem = "Rest";

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


        #region Mileage Plot
        public void plotMileageData()
        {

            int numCycles = (int)Math.Ceiling(double.Parse(numCyclesInput.Text));
            int numDays = (int)Math.Ceiling(double.Parse(numDaysInput.Text)); 


            xData = new double[numCycles];
            yDataTarget = new double[numCycles];
            yDataTempo = new double[numCycles];
            yDataSpeed = new double[numCycles];
            yDataLong = new double[numCycles];
            yDataEasy = new double[numCycles];

            mileageChart.Series.Clear();

            for (int ii = 0; ii < numCycles; ii++)
            {
                double easyMiles = 0;
                double tempoMiles = 0;
                double longMiles = 0;
                double speedMiles = 0;
                for(int jj = 0; jj < numDays; jj++)
                {
                    RunButton thisRunButton = runDefInputList[ii][jj];
                    switch (thisRunButton.getRunType())
                    {
                        case (RunTypes.Easy):
                            easyMiles = thisRunButton.getTotalMileage();
                            break;
                        case (RunTypes.Long):
                            longMiles = thisRunButton.getTotalMileage();
                            break;
                        case (RunTypes.Tempo):
                            easyMiles = thisRunButton.getEasyMileage();
                            tempoMiles = thisRunButton.getWorkoutMileage();
                            break;
                        case (RunTypes.Interval):
                            easyMiles = thisRunButton.getEasyMileage();
                            speedMiles = thisRunButton.getWorkoutMileage();
                            break;
                    }
                   
                }


                xData[ii] = ii + 1;
                yDataTarget[ii] = double.Parse(mileageList[ii].Text);
                yDataTempo[ii] = tempoMiles;
                yDataSpeed[ii] = speedMiles;
                yDataLong[ii] = longMiles;
                yDataEasy[ii] = easyMiles;
            }

            //Vertical bar chart
            //create another area and add it to the chart
            PlotTools.ChartArea area = new PlotTools.ChartArea("Mileage Data");
            area.AxisX.Title = "Cycle #";
            area.AxisY.Title = "Mileage";
            mileageChart.ChartAreas.Add(area);

            //Create the series using just the y data
            PlotTools.Series targetBarSeries = new PlotTools.Series();
            targetBarSeries.Points.DataBindY(yDataTarget);
            //targetBarSeries.Color = Brushes.Blue;
            targetBarSeries.MarkerSize = 10;
            targetBarSeries.ChartType = PlotTools.SeriesChartType.Point;
            targetBarSeries.ChartArea = "Mileage Data";

            PlotTools.Series tempoBarSeries = new PlotTools.Series();
            tempoBarSeries.Points.DataBindY(yDataTempo);
            //tempoBarSeries.Color = Brushes.Orange;
            tempoBarSeries.ChartType = PlotTools.SeriesChartType.StackedColumn;
            tempoBarSeries.ChartArea = "Mileage Data";

            PlotTools.Series speedBarSeries = new PlotTools.Series();
            speedBarSeries.Points.DataBindY(yDataSpeed);
            //speedBarSeries.Color = Brushes.Orange;
            speedBarSeries.ChartType = PlotTools.SeriesChartType.StackedColumn;
            speedBarSeries.ChartArea = "Mileage Data";

            PlotTools.Series longBarSeries = new PlotTools.Series();
            longBarSeries.Points.DataBindY(yDataLong);
            //longBarSeries.Color = Brushes.DarkGreen;
            longBarSeries.ChartType = PlotTools.SeriesChartType.StackedColumn;
            longBarSeries.ChartArea = "Mileage Data";

            PlotTools.Series easyBarSeries = new PlotTools.Series();
            easyBarSeries.Points.DataBindY(yDataEasy);
            //easyBarSeries.Color = Brushes.LightGreen;
            easyBarSeries.ChartType = PlotTools.SeriesChartType.StackedColumn;
            easyBarSeries.ChartArea = "Mileage Data";


            //Add the series to the chart

            mileageChart.Series.Add(tempoBarSeries);
            mileageChart.Series.Add(speedBarSeries);
            mileageChart.Series.Add(longBarSeries);
            mileageChart.Series.Add(easyBarSeries);


            mileageChart.Series.Add(targetBarSeries);

            //chartStackPanel.Children.Add(mileageChart);
        }

        //public void refreshMileageData()
        //{

        //    int numCycles = (int)Math.Ceiling(numCyclesNumInput.Value);
        //    xData = new double[numCycles];
        //    yDataTarget = new double[numCycles];
        //    yDataTempo = new double[numCycles];
        //    yDataSpeed = new double[numCycles];
        //    yDataLong = new double[numCycles];
        //    yDataEasy = new double[numCycles];

        //    mileageChart.Series.Clear();

        //    for (int ii = 0; ii < numCycles; ii++)
        //    {
        //        xData[ii] = ii + 1;
        //        yDataTarget[ii] = (double)((NumericUpDown)cycleMileageTable.GetControlFromPosition(1, ii)).Value;
        //        yDataTempo[ii] = (double)Convert.ToDouble(((Label)tempoWorkoutTable.GetControlFromPosition(7, ii)).Text);
        //        yDataSpeed[ii] = (double)Convert.ToDouble(((Label)speedWorkoutTable.GetControlFromPosition(11, ii)).Text);
        //        yDataLong[ii] = (double)((NumericUpDown)longRunCycleTable.GetControlFromPosition(1, ii)).Value;
        //        yDataEasy[ii] = 10;
        //    }

        //    //Vertical bar chart
        //    //create another area and add it to the chart
        //    ChartArea area = new ChartArea("Mileage Data");
        //    area.AxisX.Title = "Cycle #";
        //    area.AxisY.Title = "Mileage";
        //    mileageChart.ChartAreas.Clear();
        //    mileageChart.ChartAreas.Add(area);

        //    //Create the series using just the y data
        //    Series targetBarSeries = new Series();
        //    targetBarSeries.Points.DataBindY(yDataTarget);
        //    targetBarSeries.Color = Color.Blue;
        //    targetBarSeries.MarkerSize = 10;
        //    targetBarSeries.ChartType = SeriesChartType.Point;
        //    targetBarSeries.ChartArea = "Mileage Data";

        //    Series tempoBarSeries = new Series();
        //    tempoBarSeries.Points.DataBindY(yDataTempo);
        //    tempoBarSeries.Color = Color.Orange;
        //    tempoBarSeries.ChartType = SeriesChartType.StackedColumn;
        //    tempoBarSeries.ChartArea = "Mileage Data";

        //    Series speedBarSeries = new Series();
        //    speedBarSeries.Points.DataBindY(yDataSpeed);
        //    speedBarSeries.Color = Color.Orange;
        //    speedBarSeries.ChartType = SeriesChartType.StackedColumn;
        //    speedBarSeries.ChartArea = "Mileage Data";

        //    Series longBarSeries = new Series();
        //    longBarSeries.Points.DataBindY(yDataLong);
        //    longBarSeries.Color = Color.DarkGreen;
        //    longBarSeries.ChartType = SeriesChartType.StackedColumn;
        //    longBarSeries.ChartArea = "Mileage Data";

        //    Series easyBarSeries = new Series();
        //    easyBarSeries.Points.DataBindY(yDataEasy);
        //    easyBarSeries.Color = Color.LightGreen;
        //    easyBarSeries.ChartType = SeriesChartType.StackedColumn;
        //    easyBarSeries.ChartArea = "Mileage Data";


        //    //Add the series to the chart

        //    mileageChart.Series.Add(tempoBarSeries);
        //    mileageChart.Series.Add(speedBarSeries);
        //    mileageChart.Series.Add(longBarSeries);
        //    mileageChart.Series.Add(easyBarSeries);


        //    mileageChart.Series.Add(targetBarSeries);

        //}


        #endregion
    }
}



