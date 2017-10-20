using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;



namespace WindowsFormsApplication1
{
    public partial class WorkoutAppGUI : Form
    {
        double[] xData;
        double[] yDataTarget;
        double[] yDataTempo;
        double[] yDataSpeed;
        double[] yDataLong;
        double[] yDataEasy;

        public WorkoutAppGUI()
        {
            InitializeComponent();

            //Initial Paramters
            numDaysNumInput.Value = 9;
            numWeeksNumInput.Value = 16;
            numCyclesNumInput.Value = numWeeksNumInput.Value * 7 / numDaysNumInput.Value;

            cycleMileageStartNumInput.Value = 10;
            cycleMileageEndNumInput.Value = 45;
            cycleMileageDeltaNumInput.Value = 2;
            numSSDelta.Value = 2;
            numSSPeriod.Value = 1;

            numOfLongRunLevels.Value = 3;
            numMinWeeklyPct.Value = 20;
            numMaxWeeklyPct.Value = 35;

            numMinWeeklyTempo.Value = 10;
            numMaxWeeklyTempo.Value = 14;

            createWorkoputBuilderTable();
            createCycleMileageTable();
            createSpeedWorkoutTable();
            createTempoWorkoutTable();
            createLongRunLevelTable();
            createLongRunTable();
            createEasyRunTable();
            plotMileageData();

            numDaysNumInput.ValueChanged += new EventHandler(numDays_ValueChanged);
            numWeeksNumInput.ValueChanged += new EventHandler(numWeeks_ValueChanged);
            numCyclesNumInput.ValueChanged += new EventHandler(numCycles_ValueChanged);
            cycleMileageStartNumInput.ValueChanged += new EventHandler(cycleMileageStartNumInput_ValueChanged);
            cycleMileageEndNumInput.ValueChanged += new EventHandler(cycleMileageEndNumInput_ValueChanged);
            cycleMileageDeltaNumInput.ValueChanged += new EventHandler(cycleMileageRatioNumInput_ValueChanged);
            numSSDelta.ValueChanged += new EventHandler(numSSDelta_ValueChanged);
            numSSPeriod.ValueChanged += new EventHandler(numSSPeriod_ValueChanged);
            numOfLongRunLevels.ValueChanged += new EventHandler(numOfLongRunLevels_ValueChanged);
            numMinWeeklyPct.ValueChanged += new EventHandler(numMinWeeklyPct_ValueChanged);
            numMaxWeeklyPct.ValueChanged += new EventHandler(numMaxWeeklyPct_ValueChanged);

        }



        
        #region Workout Builder Table
        private void createWorkoputBuilderTable()
        {
            //set top panel autoscroll to true
            cyclePanel.AutoScroll = true;

            //format header panel
            workoutBuilderHeader.Controls.Clear();
            workoutBuilderHeader.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            workoutBuilderHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            workoutBuilderHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            workoutBuilderHeader.Controls.Add(new Label() { Text = "Day", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, 0);
            workoutBuilderHeader.Controls.Add(new Label() { Text = "Run", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 1, 0);
            workoutBuilderHeader.Controls.Add(new Label() { Text = "Gym Workout", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 2, 0);

            //format data table
            workoutBuilderTable.Controls.Clear();
            workoutBuilderTable.AutoSize = true;
            workoutBuilderTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            workoutBuilderTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            workoutBuilderTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            workoutBuilderTable.ColumnCount = 3;
            workoutBuilderTable.RowCount = (int)Math.Ceiling(numCyclesNumInput.Value);


            //create and add controls
            for (int ii = 0; ii < (int)numDaysNumInput.Value; ii++)
            {
                ComboBox runCmbBox = new ComboBox();
                runCmbBox.Items.Add("Easy Run");
                runCmbBox.Items.Add("Long Run");
                runCmbBox.Items.Add("Speed Workout");
                runCmbBox.Items.Add("Tempo");
                runCmbBox.Items.Add("Rest");
                runCmbBox.Anchor = AnchorStyles.None;
                runCmbBox.Dock = DockStyle.Fill;
                runCmbBox.FlatStyle = FlatStyle.Flat;
                runCmbBox.DropDownStyle = ComboBoxStyle.DropDownList;
                runCmbBox.SelectedValueChanged += new EventHandler(runComboBox_ValueChanged);


                ComboBox gymCmbBox = new ComboBox();
                gymCmbBox.Items.Add("Legs");
                gymCmbBox.Items.Add("Chest");
                gymCmbBox.Items.Add("Shoulders");
                gymCmbBox.Items.Add("Back");
                gymCmbBox.Items.Add("Abs");
                gymCmbBox.Items.Add("Rest");
                gymCmbBox.Anchor = AnchorStyles.None;
                gymCmbBox.Dock = DockStyle.Fill;
                gymCmbBox.FlatStyle = FlatStyle.Flat;
                gymCmbBox.DropDownStyle = ComboBoxStyle.DropDownList;
                gymCmbBox.SelectedValueChanged += new EventHandler(gymComboBox_ValueChanged);


                workoutBuilderTable.Controls.Add(new Label() { Text = (ii + 1).ToString(), Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, ii);
                workoutBuilderTable.Controls.Add(runCmbBox, 1, ii);
                workoutBuilderTable.Controls.Add(gymCmbBox, 2, ii);
                gymCmbBox.Text = "Rest";
                runCmbBox.SelectedIndex = runCmbBox.FindStringExact("Rest");

            }

            //set table selection to predefined templates
            initTableSelection((int)numDaysNumInput.Value);
        }

        private void initTableSelection(int numRows)
        {
            switch (numRows)
            {

                case 7:

                    workoutBuilderTable.GetControlFromPosition(1, 0).Text = "Easy Run";
                    workoutBuilderTable.GetControlFromPosition(1, 1).Text = "Speed Workout";
                    workoutBuilderTable.GetControlFromPosition(1, 2).Text = "Rest";
                    workoutBuilderTable.GetControlFromPosition(1, 3).Text = "Tempo";
                    workoutBuilderTable.GetControlFromPosition(1, 4).Text = "Easy Run";
                    workoutBuilderTable.GetControlFromPosition(1, 5).Text = "Long Run";
                    workoutBuilderTable.GetControlFromPosition(1, 6).Text = "Easy Run";

                    workoutBuilderTable.GetControlFromPosition(2, 0).Text = "Chest";
                    workoutBuilderTable.GetControlFromPosition(2, 1).Text = "Back";
                    workoutBuilderTable.GetControlFromPosition(2, 2).Text = "Legs";
                    workoutBuilderTable.GetControlFromPosition(2, 3).Text = "Abs";
                    workoutBuilderTable.GetControlFromPosition(2, 4).Text = "Shoulders";
                    workoutBuilderTable.GetControlFromPosition(2, 5).Text = "Rest";
                    workoutBuilderTable.GetControlFromPosition(2, 6).Text = "Rest";

                    break;

                case 8:

                    workoutBuilderTable.GetControlFromPosition(1, 0).Text = "Easy Run";
                    workoutBuilderTable.GetControlFromPosition(1, 1).Text = "Speed Workout";
                    workoutBuilderTable.GetControlFromPosition(1, 2).Text = "Rest";
                    workoutBuilderTable.GetControlFromPosition(1, 3).Text = "Tempo";
                    workoutBuilderTable.GetControlFromPosition(1, 4).Text = "Rest";
                    workoutBuilderTable.GetControlFromPosition(1, 5).Text = "Easy Run";
                    workoutBuilderTable.GetControlFromPosition(1, 6).Text = "Long Run";
                    workoutBuilderTable.GetControlFromPosition(1, 7).Text = "Easy Run";

                    workoutBuilderTable.GetControlFromPosition(2, 0).Text = "Chest";
                    workoutBuilderTable.GetControlFromPosition(2, 1).Text = "Back";
                    workoutBuilderTable.GetControlFromPosition(2, 2).Text = "Legs";
                    workoutBuilderTable.GetControlFromPosition(2, 3).Text = "Rest";
                    workoutBuilderTable.GetControlFromPosition(2, 4).Text = "Abs";
                    workoutBuilderTable.GetControlFromPosition(2, 5).Text = "Shoulders";
                    workoutBuilderTable.GetControlFromPosition(2, 6).Text = "Rest";
                    workoutBuilderTable.GetControlFromPosition(2, 7).Text = "Rest";

                    break;

                case 9:

                    workoutBuilderTable.GetControlFromPosition(1, 0).Text = "Long Run";
                    workoutBuilderTable.GetControlFromPosition(1, 1).Text = "Easy Run";
                    workoutBuilderTable.GetControlFromPosition(1, 2).Text = "Rest";
                    workoutBuilderTable.GetControlFromPosition(1, 3).Text = "Tempo";
                    workoutBuilderTable.GetControlFromPosition(1, 4).Text = "Easy Run";
                    workoutBuilderTable.GetControlFromPosition(1, 5).Text = "Rest";
                    workoutBuilderTable.GetControlFromPosition(1, 6).Text = "Easy Run";
                    workoutBuilderTable.GetControlFromPosition(1, 7).Text = "Speed Workout";
                    workoutBuilderTable.GetControlFromPosition(1, 8).Text = "Rest";

                    workoutBuilderTable.GetControlFromPosition(2, 0).Text = "Rest";
                    workoutBuilderTable.GetControlFromPosition(2, 1).Text = "Abs";
                    workoutBuilderTable.GetControlFromPosition(2, 2).Text = "Chest";
                    workoutBuilderTable.GetControlFromPosition(2, 3).Text = "Rest";
                    workoutBuilderTable.GetControlFromPosition(2, 4).Text = "Back";
                    workoutBuilderTable.GetControlFromPosition(2, 5).Text = "Legs";
                    workoutBuilderTable.GetControlFromPosition(2, 6).Text = "Shoulders";
                    workoutBuilderTable.GetControlFromPosition(2, 7).Text = "Rest";
                    workoutBuilderTable.GetControlFromPosition(2, 8).Text = "Rest";

                    break;

                default:
                    break;
            }
        }
        #endregion
        #region Cycle Mileage Table
        private void createCycleMileageTable()
        {
            Boolean mileageExceeded = false;
            weeklyMileagePanel.AutoScroll = true;

            cycleMileageHeader.Controls.Clear();
            cycleMileageHeader.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            cycleMileageHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            cycleMileageHeader.Controls.Add(new Label() { Text = "Cycle", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, 0);
            cycleMileageHeader.Controls.Add(new Label() { Text = "Milage", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 1, 0);

            cycleMileageTable.Controls.Clear();
            cycleMileageTable.AutoSize = true;
            cycleMileageTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            cycleMileageTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            cycleMileageTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            cycleMileageTable.ColumnCount = 3;
            cycleMileageTable.RowCount = (int)Math.Ceiling(numCyclesNumInput.Value);


            double mileageDelta = (double)cycleMileageDeltaNumInput.Value;
            double doubleCycleMileage;
            double maxMileage = (double)cycleMileageEndNumInput.Value;
            int periodCnt = 1;

            double sign = -1;
            double lastMileageTgt = (double)cycleMileageStartNumInput.Value;

            for (int ii = 0; ii < (int)Math.Ceiling(numCyclesNumInput.Value); ii++)
            {

                if (ii == 0)
                {
                    doubleCycleMileage = (double)cycleMileageStartNumInput.Value;
                    
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
                        if ((int)numSSPeriod.Value > 0)
                        {
                            doubleCycleMileage = lastMileageTgt + (double)numSSDelta.Value * sign;
                            periodCnt++;

                            if (periodCnt > (int)numSSPeriod.Value)
                            {
                                sign = sign * -1;
                                periodCnt = 1;
                            }
                        }else
                        {
                            doubleCycleMileage = lastMileageTgt;
                        }

                    }
                }

                lastMileageTgt = doubleCycleMileage;

                PictureBox picture = new PictureBox{ Name = "pictureBox" + ii,Size = new Size(200, 20),Visible = true,Anchor = AnchorStyles.None,};
                int ratio = 0;

                if (maxMileage > 0)
                {
                    ratio = (int)Math.Ceiling(doubleCycleMileage / maxMileage * 200);
                }else
                {
                    ratio = 0;
                }

                if (doubleCycleMileage < 0)
                {
                    doubleCycleMileage = 0;
                }

                picture.Paint += new PaintEventHandler((sender, e) => cycleMileageLevel_Paint(sender, e,ratio));

                NumericUpDown cycleMileage = new NumericUpDown() { Value = (decimal)Math.Ceiling(doubleCycleMileage), Anchor = AnchorStyles.None, Dock = DockStyle.Fill };
                cycleMileage.ValueChanged += new EventHandler(cycleMileage_ValueChanged);

                cycleMileageTable.Controls.Add(new Label() { Text = (ii+1).ToString(), Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, ii);
                cycleMileageTable.Controls.Add(cycleMileage, 1, ii);
                cycleMileageTable.Controls.Add(picture, 2, ii);
                

            }
        }

        private void updateCycleMileageTable()
        {
            double mileageDelta = (double)cycleMileageDeltaNumInput.Value;

            if(Math.Ceiling(numCyclesNumInput.Value) < cycleMileageTable.RowCount)
            {

                
                for(int ii = (int)Math.Ceiling(numCyclesNumInput.Value); ii < cycleMileageTable.RowCount; ii++)
                {
                    for (int jj = 0; jj < cycleMileageTable.ColumnCount; jj++)
                    {
                        Control Control = cycleMileageTable.GetControlFromPosition(jj, ii);
                        cycleMileageTable.Controls.Remove(Control);
                    }

                } 

                cycleMileageTable.RowCount = (int)Math.Ceiling(numCyclesNumInput.Value);


            }
            else if(numCyclesNumInput.Value > cycleMileageTable.RowCount)
            {
                int previousRowCount = cycleMileageTable.RowCount;
                cycleMileageTable.RowCount = (int)Math.Ceiling(numCyclesNumInput.Value);
                for (int ii = previousRowCount; ii < (int)Math.Ceiling(numCyclesNumInput.Value); ii++)
                {

                    double doubleCycleMileage = (double)cycleMileageStartNumInput.Value + mileageDelta * (ii);
                    double maxMileage = (double)cycleMileageEndNumInput.Value;
                    if (doubleCycleMileage > maxMileage)
                    {
                        doubleCycleMileage = maxMileage;
                    }

                    PictureBox picture = new PictureBox { Name = "pictureBox" + ii, Size = new Size(200, 20), Visible = true, Anchor = AnchorStyles.None, };
                    picture.Paint += new PaintEventHandler((sender, e) => cycleMileageLevel_Paint(sender, e, (int)Math.Ceiling(doubleCycleMileage / maxMileage * 200)));

                    NumericUpDown cycleMileage = new NumericUpDown() { Value = (decimal)Math.Ceiling(doubleCycleMileage), Anchor = AnchorStyles.None, Dock = DockStyle.Fill };
                    cycleMileage.ValueChanged += new EventHandler(cycleMileage_ValueChanged);

                    cycleMileageTable.Controls.Add(new Label() { Text = (ii + 1).ToString(), Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, ii);
                    cycleMileageTable.Controls.Add(cycleMileage, 1, ii);
                    cycleMileageTable.Controls.Add(picture, 2, ii);


                }
            } 

        }


#endregion
        #region Speed Work Table
        private void createSpeedWorkoutTable()
        {
            //set auto scroll to true
            speedWorkoutTablePanel.AutoScroll = true;

            //clear out existing controls and create header
            speedWorkoutTableHeader.Controls.Clear();
            speedWorkoutTableHeader.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            speedWorkoutTableHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            speedWorkoutTableHeader.Controls.Add(new Label() { Text = "Cycle", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, 0);
            speedWorkoutTableHeader.Controls.Add(new Label() { Text = "Warmup", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 1, 0);
            speedWorkoutTableHeader.Controls.Add(new Label() { Text = "Units", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 2, 0);
            speedWorkoutTableHeader.Controls.Add(new Label() { Text = "Reps", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 3, 0);
            speedWorkoutTableHeader.Controls.Add(new Label() { Text = "Interval", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 4, 0);
            speedWorkoutTableHeader.Controls.Add(new Label() { Text = "Units", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 5, 0);
            speedWorkoutTableHeader.Controls.Add(new Label() { Text = "Pace", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 6, 0);
            speedWorkoutTableHeader.Controls.Add(new Label() { Text = "Rest", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 7, 0);
            speedWorkoutTableHeader.Controls.Add(new Label() { Text = "Units", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 8, 0);
            speedWorkoutTableHeader.Controls.Add(new Label() { Text = "Cooldown", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 9, 0);
            speedWorkoutTableHeader.Controls.Add(new Label() { Text = "Units", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 10, 0);
            speedWorkoutTableHeader.Controls.Add(new Label() { Text = "Total Miles", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 11, 0);

            //clear out controls from table and intilize to correct size 
            speedWorkoutTable.Controls.Clear();
            speedWorkoutTable.AutoSize = true;
            speedWorkoutTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            speedWorkoutTable.ColumnCount = 12;
            speedWorkoutTable.RowCount = (int)Math.Ceiling(numCyclesNumInput.Value);

            //build the table
            for (int ii = 0; ii < (int)Math.Ceiling(numCyclesNumInput.Value); ii++)
            {
                addNewSpeedWorkoutRow(ii);
            }
        }

        private void addNewSpeedWorkoutRow(int rowNumber)
        {

            //create the warmup input
            NumericUpDown warmupDist = new NumericUpDown() { Maximum = 100000, Minimum = 0, DecimalPlaces = 1, Value = (decimal)1.5, Anchor = AnchorStyles.None, Dock = DockStyle.Fill };
            warmupDist.ValueChanged += new EventHandler(speedWorkout_ValueChanged);

            //create the warmup units
            ComboBox warmUnits = new ComboBox();
            warmUnits.Items.Add("Miles");
            warmUnits.Items.Add("Meters");
            warmUnits.Anchor = AnchorStyles.None;
            warmUnits.Dock = DockStyle.Fill;
            warmUnits.Text = "Miles";
            warmUnits.DropDownStyle = ComboBoxStyle.DropDownList;
            warmUnits.FlatStyle = FlatStyle.Flat;
            warmUnits.SelectedValueChanged += new EventHandler(speedWorkout_ValueChanged);

            //create the number of reps
            NumericUpDown reps = new NumericUpDown() { Maximum = 100000, Minimum = 0, DecimalPlaces = 0, Value = (decimal)12, Anchor = AnchorStyles.None, Dock = DockStyle.Fill };
            reps.ValueChanged += new EventHandler(speedWorkout_ValueChanged);

            //create the interval distance input
            NumericUpDown intervalDist = new NumericUpDown() { Maximum = 100000, Minimum = 0, DecimalPlaces = 1, Value = (decimal)400.0, Anchor = AnchorStyles.None, Dock = DockStyle.Fill };
            intervalDist.ValueChanged += new EventHandler(speedWorkout_ValueChanged);

            //create the interval units input
            ComboBox intervalUnits = new ComboBox();
            intervalUnits.Items.Add("Miles");
            intervalUnits.Items.Add("Meters");
            intervalUnits.Anchor = AnchorStyles.None;
            intervalUnits.Dock = DockStyle.Fill;
            intervalUnits.Text = "Meters";
            intervalUnits.DropDownStyle = ComboBoxStyle.DropDownList;
            intervalUnits.FlatStyle = FlatStyle.Flat;
            intervalUnits.SelectedValueChanged += new EventHandler(speedWorkout_ValueChanged);

            //create the interval pace input
            ComboBox intervalPace = new ComboBox();
            intervalPace.Items.Add("5K");
            intervalPace.Items.Add("10K");
            intervalPace.Anchor = AnchorStyles.None;
            intervalPace.Dock = DockStyle.Fill;
            intervalPace.Text = "5K";
            intervalPace.DropDownStyle = ComboBoxStyle.DropDownList;
            intervalPace.FlatStyle = FlatStyle.Flat;
            intervalPace.SelectedValueChanged += new EventHandler(speedWorkout_ValueChanged);

            //create the rest distacne input
            NumericUpDown restDist = new NumericUpDown() { Maximum = 100000, Minimum = 0, DecimalPlaces = 1, Value = (decimal)400.0, Anchor = AnchorStyles.None, Dock = DockStyle.Fill };
            restDist.ValueChanged += new EventHandler(speedWorkout_ValueChanged);

            //create the rest units input
            ComboBox restUnits = new ComboBox();
            restUnits.Items.Add("Miles");
            restUnits.Items.Add("Meters");
            restUnits.Anchor = AnchorStyles.None;
            restUnits.Dock = DockStyle.Fill;
            restUnits.Text = "Meters";
            restUnits.DropDownStyle = ComboBoxStyle.DropDownList;
            restUnits.FlatStyle = FlatStyle.Flat;
            restUnits.SelectedValueChanged += new EventHandler(speedWorkout_ValueChanged);

            //create the cooldown distance input
            NumericUpDown coolDist = new NumericUpDown() { Maximum = 100000, Minimum = 0, DecimalPlaces = 1, Value = (decimal)1.5, Anchor = AnchorStyles.None, Dock = DockStyle.Fill };
            coolDist.ValueChanged += new EventHandler(speedWorkout_ValueChanged);

            //create the cooldown units input
            ComboBox coolUnits = new ComboBox();
            coolUnits.Items.Add("Miles");
            coolUnits.Items.Add("Meters");
            coolUnits.Anchor = AnchorStyles.None;
            coolUnits.Dock = DockStyle.Fill;
            coolUnits.Text = "Miles";
            coolUnits.DropDownStyle = ComboBoxStyle.DropDownList;
            coolUnits.FlatStyle = FlatStyle.Flat;
            coolUnits.SelectedValueChanged += new EventHandler(speedWorkout_ValueChanged);

            //add controls to the table
            speedWorkoutTable.Controls.Add(new Label() { Text = (rowNumber + 1).ToString(), Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, rowNumber);
            speedWorkoutTable.Controls.Add(warmupDist, 1, rowNumber);
            speedWorkoutTable.Controls.Add(warmUnits, 2, rowNumber);
            speedWorkoutTable.Controls.Add(reps, 3, rowNumber);
            speedWorkoutTable.Controls.Add(intervalDist, 4, rowNumber);
            speedWorkoutTable.Controls.Add(intervalUnits, 5, rowNumber);
            speedWorkoutTable.Controls.Add(intervalPace, 6, rowNumber);
            speedWorkoutTable.Controls.Add(restDist, 7, rowNumber);
            speedWorkoutTable.Controls.Add(restUnits, 8, rowNumber);
            speedWorkoutTable.Controls.Add(coolDist, 9, rowNumber);
            speedWorkoutTable.Controls.Add(coolUnits, 10, rowNumber);

            //calculate the total distance based on the units
            double warmDistance;
            double intervalDistance;
            double restDistance;
            double coolDistance;

            if (warmUnits.Text == "Miles")
            {
                warmDistance = (double)warmupDist.Value;
            }
            else
            {
                warmDistance = (double)warmupDist.Value * 0.000621371;
            }

            if (intervalUnits.Text == "Miles")
            {
                intervalDistance = (double)intervalDist.Value;
            }
            else
            {
                intervalDistance = (double)intervalDist.Value * 0.000621371;
            }

            if (restUnits.Text == "Miles")
            {
                restDistance = (double)restDist.Value;
            }
            else
            {
                restDistance = (double)restDist.Value * 0.000621371;
            }

            if (coolUnits.Text == "Miles")
            {
                coolDistance = (double)coolDist.Value;
            }
            else
            {
                coolDistance = (double)coolDist.Value * 0.000621371;
            }

            double totalDistance = warmDistance + intervalDistance * (double)reps.Value + restDistance * ((double)reps.Value - 1.0) + coolDistance;

            //add the total distance control
            speedWorkoutTable.Controls.Add(new Label() { Text = (Math.Round(totalDistance * 10) / 10).ToString(), Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 11, rowNumber);
        }

        private void updateSpeedWorkoutTable()
        {

            // if row count is decreasing then remove rows
            if (Math.Ceiling(numCyclesNumInput.Value) < speedWorkoutTable.RowCount)
            {
                //loop through rown
                for (int ii = (int)Math.Ceiling(numCyclesNumInput.Value); ii < speedWorkoutTable.RowCount; ii++)
                {
                    //remove all controls in row
                    for (int jj = 0; jj < speedWorkoutTable.ColumnCount; jj++)
                    {
                        Control Control = speedWorkoutTable.GetControlFromPosition(jj, ii);
                        speedWorkoutTable.Controls.Remove(Control);
                    }

                }

                //update teh row count
                speedWorkoutTable.RowCount = (int)Math.Ceiling(numCyclesNumInput.Value);

            }
            //else add new rows to the form
            else if ((int)Math.Ceiling(numCyclesNumInput.Value) > speedWorkoutTable.RowCount)
            {
                int previousRowCount = speedWorkoutTable.RowCount;
                speedWorkoutTable.RowCount = (int)Math.Ceiling(numCyclesNumInput.Value);
                for (int ii = previousRowCount; ii < (int)Math.Ceiling(numCyclesNumInput.Value); ii++)
                {
                    //add new speed workout row
                    addNewSpeedWorkoutRow(ii);

                }
            }

        }


        
#endregion
        #region Longrun Table

        private void createLongRunLevelTable()
        {


            longRunLevelTable.Controls.Clear();
            longRunLevelTable.AutoSize = true;
            longRunLevelTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            longRunLevelTable.ColumnCount = 2;
            longRunLevelTable.RowCount = (int)numOfLongRunLevels.Value + 1;


            longRunLevelTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            longRunLevelTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            longRunLevelTable.Controls.Add(new Label() { Text = "Level", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, 0);
            longRunLevelTable.Controls.Add(new Label() { Text = "Mileage", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 1, 0);

            int numLevels = (int)numOfLongRunLevels.Value;

            NumericUpDown numUpDown1 = new NumericUpDown();
            NumericUpDown numUpDown2 = new NumericUpDown();
            NumericUpDown numUpDown3 = new NumericUpDown();
            NumericUpDown numUpDown4 = new NumericUpDown();


            numUpDown1.Anchor = AnchorStyles.None;
            numUpDown2.Anchor = AnchorStyles.None;
            numUpDown3.Anchor = AnchorStyles.None;
            numUpDown4.Anchor = AnchorStyles.None;

            numUpDown1.Dock = DockStyle.Fill;
            numUpDown2.Dock = DockStyle.Fill;
            numUpDown3.Dock = DockStyle.Fill;
            numUpDown4.Dock = DockStyle.Fill;
           

            switch (numLevels)
            {
                case 1:
                    numUpDown1.Value = 12;
                    longRunLevelTable.Controls.Add(new Label() { Text = "1", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, 1);
                    longRunLevelTable.Controls.Add(numUpDown1, 1, 1);
                    break;
                case 2:
                    numUpDown1.Value = 10;
                    numUpDown2.Value = 12;
                    longRunLevelTable.Controls.Add(new Label() { Text = "1", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, 1);
                    longRunLevelTable.Controls.Add(numUpDown1, 1, 1);
                    longRunLevelTable.Controls.Add(new Label() { Text = "2", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, 2);
                    longRunLevelTable.Controls.Add(numUpDown2, 1, 2);
                    break;
                case 3:
                    numUpDown1.Value = 10;
                    numUpDown2.Value = 12;
                    numUpDown3.Value = 14;

                    longRunLevelTable.Controls.Add(new Label() { Text = "1", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, 1);
                    longRunLevelTable.Controls.Add(numUpDown1, 1, 1);
                    longRunLevelTable.Controls.Add(new Label() { Text = "2", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, 2);
                    longRunLevelTable.Controls.Add(numUpDown2, 1, 2);
                    longRunLevelTable.Controls.Add(new Label() { Text = "3", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, 3);
                    longRunLevelTable.Controls.Add(numUpDown3, 1, 3);
                    break;
                case 4:
                    numUpDown1.Value = 8;
                    numUpDown2.Value = 10;
                    numUpDown3.Value = 12;
                    numUpDown4.Value = 14;

                    longRunLevelTable.Controls.Add(new Label() { Text = "1", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, 1);
                    longRunLevelTable.Controls.Add(numUpDown1 , 1, 1);
                    longRunLevelTable.Controls.Add(new Label() { Text = "2", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, 2);
                    longRunLevelTable.Controls.Add(numUpDown2 , 1, 2);
                    longRunLevelTable.Controls.Add(new Label() { Text = "3", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, 3);
                    longRunLevelTable.Controls.Add(numUpDown3 , 1, 3);
                    longRunLevelTable.Controls.Add(new Label() { Text = "4", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, 4);
                    longRunLevelTable.Controls.Add(numUpDown4 , 1, 4);
                    break;
            }
            numUpDown1.ValueChanged += new EventHandler(longRunLevel_ValueChanged);
            numUpDown2.ValueChanged += new EventHandler(longRunLevel_ValueChanged);
            numUpDown3.ValueChanged += new EventHandler(longRunLevel_ValueChanged);
            numUpDown4.ValueChanged += new EventHandler(longRunLevel_ValueChanged);
        }


        private void createLongRunTable()
        {
            //set teh autoscroll property to true
            longRunTablePanel.AutoScroll = true;

            //clear out all existing controls
            longRunCycleTableHeader.Controls.Clear();

            //create and format header
            longRunCycleTableHeader.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            longRunCycleTableHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            longRunCycleTableHeader.Controls.Add(new Label() { Text = "Cycle", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, 0);
            longRunCycleTableHeader.Controls.Add(new Label() { Text = "Milage", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 1, 0);
            longRunCycleTableHeader.Controls.Add(new Label() { Text = "% Total", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 2, 0);

            //clear out all existing controls and format table
            longRunCycleTable.Controls.Clear();
            longRunCycleTable.AutoSize = true;
            longRunCycleTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            longRunCycleTable.ColumnCount = 4;
            longRunCycleTable.RowCount = (int)Math.Ceiling(numCyclesNumInput.Value);

            //set index to zero
            int longRunLevelIndex = 0;



            //determine maximum cycle mileage
            decimal maxMileage = 0;
            for (int ii = 0; ii < numCyclesNumInput.Value; ii++)
            {
                NumericUpDown numTotalCycleMilage = (NumericUpDown)cycleMileageTable.GetControlFromPosition(1, ii);

                if (numTotalCycleMilage.Value > maxMileage)
                {
                    maxMileage = numTotalCycleMilage.Value;
                }
            }

            //calculate cycle factor to nomalize cyle mileage to week
            double cycleFactor = 7 / (double)numDaysNumInput.Value;

            //determine the maximum amount of long run miles
            decimal maxLongRunMiles = 0;
            int numLevels = (int)Math.Ceiling(numOfLongRunLevels.Value);
            for (int jj = 0; jj < numLevels; jj++)
            {
                NumericUpDown thisNumericUpDown = (NumericUpDown)longRunLevelTable.GetControlFromPosition(1, jj + 1);
                if (thisNumericUpDown.Value > maxLongRunMiles)
                {
                    maxLongRunMiles = thisNumericUpDown.Value;
                }

            }

            //build the table
            for (int ii = 0; ii < numCyclesNumInput.Value; ii++)
            {
                //get teh desired level of long run miles to add to the table
                NumericUpDown numLongRunLevelMiles = (NumericUpDown)longRunLevelTable.GetControlFromPosition(1, (longRunLevelIndex + 1));
                double longRunMiles = (double)numLongRunLevelMiles.Value;

                //get the total cycle mileage and calculate the percentage of miles
                NumericUpDown numTotalCycleMilage = (NumericUpDown)cycleMileageTable.GetControlFromPosition(1, ii);
                double longRunPercentage = longRunMiles / ((double)numTotalCycleMilage.Value* cycleFactor )* 100;

                //saturate the percentage of miles at the min and max
                if(longRunPercentage< (double)numMinWeeklyPct.Value)
                {
                    longRunPercentage = (double)numMinWeeklyPct.Value;
                    longRunMiles = (double)numTotalCycleMilage.Value * cycleFactor * longRunPercentage / 100;

                }
                else if (longRunPercentage > (double)numMaxWeeklyPct.Value)
                {
                    longRunPercentage = (double)numMaxWeeklyPct.Value;
                    longRunMiles = (double)numTotalCycleMilage.Value * cycleFactor * longRunPercentage / 100;

                }

                //create new bar picture to display level
                PictureBox picture = new PictureBox { Name = "pictureBox" + ii, Size = new Size(200, 20), Visible = true, Anchor = AnchorStyles.None, };
                int ratio = (int)Math.Ceiling(longRunMiles / (double)maxLongRunMiles * 200);
                picture.Paint += new PaintEventHandler((sender, e) => longRunLevel_Paint(sender, e, ratio, Color.Green));

                //create new user input contol
                NumericUpDown numLongRunMiles = new NumericUpDown() { Value = (decimal)longRunMiles, Anchor = AnchorStyles.None, Dock = DockStyle.Fill };
                numLongRunMiles.ValueChanged += new EventHandler(numLongRunMiles_ValueChanged);

                //add all controls to the table
                longRunCycleTable.Controls.Add(new Label() { Text = (ii + 1).ToString(), Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, ii);
                longRunCycleTable.Controls.Add(numLongRunMiles, 1, ii);
                longRunCycleTable.Controls.Add(new Label() { Text = (Math.Round(longRunPercentage*10)/10).ToString(), Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 2, ii);
                longRunCycleTable.Controls.Add(picture, 3, ii);

                //increment the long run level index
                longRunLevelIndex++;
                if (longRunLevelIndex > (numLevels - 1))
                {
                    longRunLevelIndex = 0;
                }

            }
        }

        private void updateLongRunTable()
        {

            //get the number of levels in the table
            int numLevels = (int)Math.Ceiling(numOfLongRunLevels.Value);

            //if number of levels is less than the current row count remove row
            if (Math.Ceiling(numCyclesNumInput.Value) < longRunCycleTable.RowCount)
            {
                //loop through rows that need to be removed
                for (int ii = (int)Math.Ceiling(numCyclesNumInput.Value); ii < longRunCycleTable.RowCount; ii++)
                {
                    //get all the controls in this row and remove them
                    for (int jj = 0; jj < longRunCycleTable.ColumnCount; jj++)
                    {
                        Control Control = longRunCycleTable.GetControlFromPosition(jj, ii);
                        longRunCycleTable.Controls.Remove(Control);
                    }

                }

                //update teh row count
                longRunCycleTable.RowCount = (int)Math.Ceiling(numCyclesNumInput.Value);


            }
            //need to add row
            else if ((int)Math.Ceiling(numCyclesNumInput.Value) > longRunCycleTable.RowCount)
            {
                //get the current row count
                int previousRowCount = longRunCycleTable.RowCount;

                //update teh row count to new level
                longRunCycleTable.RowCount = (int)Math.Ceiling(numCyclesNumInput.Value);

                //find the maximum number of long run miles
                decimal maxLongRunMiles = 0;
                for (int ii = 0; ii < numLevels; ii++)
                {
                    NumericUpDown thisNumericUpDown = (NumericUpDown)longRunCycleTable.GetControlFromPosition(1, ii);
                    decimal longRunMiles = thisNumericUpDown.Value;
                    if (longRunMiles > maxLongRunMiles)
                    {
                        maxLongRunMiles = longRunMiles;
                    }
                }

                //find the cycle factor to normalize weekly mileage
                double cycleFactor = 7 / (double)numDaysNumInput.Value;

                //add new rows
                for (int ii = previousRowCount; ii < (int)Math.Ceiling(numCyclesNumInput.Value); ii++)
                {
                    //get desired long run value from table at index 1
                    NumericUpDown numLongRunLevelMiles = (NumericUpDown)longRunLevelTable.GetControlFromPosition(1, 1);
                    double longRunMiles = (double)numLongRunLevelMiles.Value;

                    //get the total cycle mileage for this cycle
                    NumericUpDown numTotalCycleMilage = (NumericUpDown)cycleMileageTable.GetControlFromPosition(1, ii);

                    //limit long run mileage to bounds that are in the table
                    double longRunPercentage = longRunMiles / ((double)numTotalCycleMilage.Value * cycleFactor) * 100;
                    if (longRunPercentage < (double)numMinWeeklyPct.Value)
                    {
                        longRunPercentage = (double)numMinWeeklyPct.Value;
                        longRunMiles = (double)numTotalCycleMilage.Value * cycleFactor * longRunPercentage / 100;

                    }
                    else if (longRunPercentage > (double)numMaxWeeklyPct.Value)
                    {
                        longRunPercentage = (double)numMaxWeeklyPct.Value;
                        longRunMiles = (double)numTotalCycleMilage.Value * cycleFactor * longRunPercentage / 100;

                    }

                    //create new bar picture
                    PictureBox picture = new PictureBox { Name = "pictureBox" + ii, Size = new Size(200, 20), Visible = true, Anchor = AnchorStyles.None, };
                    int ratio = (int)Math.Ceiling(longRunMiles / (double)maxLongRunMiles * 200);
                    picture.Paint += new PaintEventHandler((sender, e) => longRunLevel_Paint(sender, e, ratio, Color.Green));

                    //create new input control for level
                    NumericUpDown numLongRunMiles = new NumericUpDown() { Value = (decimal)longRunMiles, Anchor = AnchorStyles.None, Dock = DockStyle.Fill };
                    numLongRunMiles.ValueChanged += new EventHandler(numLongRunMiles_ValueChanged);

                    //add the new controls to the table
                    longRunCycleTable.Controls.Add(new Label() { Text = (ii + 1).ToString(), Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, ii);
                    longRunCycleTable.Controls.Add(numLongRunMiles, 1, ii);
                    longRunCycleTable.Controls.Add(new Label() { Text = (Math.Round(longRunPercentage * 10) / 10).ToString(), Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 2, ii);
                    longRunCycleTable.Controls.Add(picture, 3, ii);
                }
            }

        }
        #endregion

        #region Tempo Table
        private void createTempoWorkoutTable()
        {
            //set auto scroll to true
            tempoWorkoutPanel.AutoScroll = true;

            //clear out existing controls and create header
            tempoTableHeader.Controls.Clear();
            tempoTableHeader.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tempoTableHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tempoTableHeader.Controls.Add(new Label() { Text = "Cycle", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, 0);
            tempoTableHeader.Controls.Add(new Label() { Text = "Warmup", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 1, 0);
            tempoTableHeader.Controls.Add(new Label() { Text = "Units", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 2, 0);
            tempoTableHeader.Controls.Add(new Label() { Text = "Tempo Distance", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 3, 0);
            tempoTableHeader.Controls.Add(new Label() { Text = "Units", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 4, 0);
            tempoTableHeader.Controls.Add(new Label() { Text = "Cooldown", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 5, 0);
            tempoTableHeader.Controls.Add(new Label() { Text = "Units", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 6, 0);
            tempoTableHeader.Controls.Add(new Label() { Text = "Total Miles", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 7, 0);
            tempoTableHeader.Controls.Add(new Label() { Text = "% Total @ Pace", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 8, 0);

            //clear out controls from table and intilize to correct size 
            tempoWorkoutTable.Controls.Clear();
            tempoWorkoutTable.AutoSize = true;
            tempoWorkoutTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tempoWorkoutTable.ColumnCount = 10;
            tempoWorkoutTable.RowCount = (int)Math.Ceiling(numCyclesNumInput.Value);

            //build the table
            for (int ii = 0; ii < (int)Math.Ceiling(numCyclesNumInput.Value); ii++)
            {
                addNewTempoWorkoutRow(ii);
            }

        }

        private void addNewTempoWorkoutRow(int rowNumber)
        {
            //TODO fix this so that it is calculated from table
            double maxIntervalMiles = 7;

            //calculate tempo range
            double cycleFactor = 7 / (double)numDaysNumInput.Value;
            double cycleMileage = (double)((NumericUpDown)cycleMileageTable.GetControlFromPosition(1, rowNumber)).Value;
            double minPercentage = (double)numMinWeeklyTempo.Value;
            double maxPercentage = (double)numMaxWeeklyTempo.Value;
            double averagePercentage = (minPercentage + maxPercentage) / 2;

            double desiredTempoMiles = Math.Round(cycleFactor * cycleMileage * averagePercentage / 100*2)/2;

            //create the warmup input
            NumericUpDown warmupDist = new NumericUpDown() { Maximum = 100000, Minimum = 0, DecimalPlaces = 1, Value = (decimal)1.5, Anchor = AnchorStyles.None, Dock = DockStyle.Fill };
            warmupDist.ValueChanged += new EventHandler(tempoWorkout_ValueChanged);

            //create the warmup units
            ComboBox warmUnits = new ComboBox();
            warmUnits.Items.Add("Miles");
            warmUnits.Items.Add("Meters");
            warmUnits.Anchor = AnchorStyles.None;
            warmUnits.Dock = DockStyle.Fill;
            warmUnits.Text = "Miles";
            warmUnits.DropDownStyle = ComboBoxStyle.DropDownList;
            warmUnits.FlatStyle = FlatStyle.Flat;
            warmUnits.SelectedValueChanged += new EventHandler(tempoWorkout_ValueChanged);


            //create the tempo distance input
            NumericUpDown tempoDist = new NumericUpDown() { Maximum = 100000, Minimum = 0, DecimalPlaces = 1, Value = (decimal)desiredTempoMiles, Anchor = AnchorStyles.None, Dock = DockStyle.Fill };
            tempoDist.ValueChanged += new EventHandler(tempoWorkout_ValueChanged);

            //create the interval units input
            ComboBox tempoUnits = new ComboBox();
            tempoUnits.Items.Add("Miles");
            tempoUnits.Items.Add("Meters");
            tempoUnits.Anchor = AnchorStyles.None;
            tempoUnits.Dock = DockStyle.Fill;
            tempoUnits.Text = "Miles";
            tempoUnits.DropDownStyle = ComboBoxStyle.DropDownList;
            tempoUnits.FlatStyle = FlatStyle.Flat;
            tempoUnits.SelectedValueChanged += new EventHandler(tempoWorkout_ValueChanged);

            //create the cooldown distance input
            NumericUpDown coolDist = new NumericUpDown() { Maximum = 100000, Minimum = 0, DecimalPlaces = 1, Value = (decimal)1.5, Anchor = AnchorStyles.None, Dock = DockStyle.Fill };
            coolDist.ValueChanged += new EventHandler(tempoWorkout_ValueChanged);

            //create the cooldown units input
            ComboBox coolUnits = new ComboBox();
            coolUnits.Items.Add("Miles");
            coolUnits.Items.Add("Meters");
            coolUnits.Anchor = AnchorStyles.None;
            coolUnits.Dock = DockStyle.Fill;
            coolUnits.Text = "Miles";
            coolUnits.SelectedValueChanged += new EventHandler(tempoWorkout_ValueChanged);
            coolUnits.DropDownStyle = ComboBoxStyle.DropDownList;
            coolUnits.FlatStyle = FlatStyle.Flat;

            //add controls to the table
            tempoWorkoutTable.Controls.Add(new Label() { Text = (rowNumber + 1).ToString(), Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, rowNumber);
            tempoWorkoutTable.Controls.Add(warmupDist, 1, rowNumber);
            tempoWorkoutTable.Controls.Add(warmUnits, 2, rowNumber);
            tempoWorkoutTable.Controls.Add(tempoDist, 3, rowNumber);
            tempoWorkoutTable.Controls.Add(tempoUnits, 4, rowNumber);
            tempoWorkoutTable.Controls.Add(coolDist, 5, rowNumber);
            tempoWorkoutTable.Controls.Add(coolUnits, 6, rowNumber);

            //calculate the total distance based on the units
            double warmDistance;
            double tempoDistance;
            double coolDistance;

            if (warmUnits.Text == "Miles")
            {
                warmDistance = (double)warmupDist.Value;
            }
            else
            {
                warmDistance = (double)warmupDist.Value * 0.000621371;
            }

            if (tempoUnits.Text == "Miles")
            {
                tempoDistance = (double)tempoDist.Value;
            }
            else
            {
                tempoDistance = (double)tempoDist.Value * 0.000621371;
            }

            if (coolUnits.Text == "Miles")
            {
                coolDistance = (double)coolDist.Value;
            }
            else
            {
                coolDistance = (double)coolDist.Value * 0.000621371;
            }

            double totalDistance = warmDistance + tempoDistance  + coolDistance;

            //add the total distance control
            tempoWorkoutTable.Controls.Add(new Label() { Text = (Math.Round(totalDistance * 10) / 10).ToString(), Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 7, rowNumber);

            //add percentage distance to table
            tempoWorkoutTable.Controls.Add(new Label() { Text = (Math.Round(tempoDistance / (cycleFactor * cycleMileage)*100 * 10) / 10).ToString(), Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 8, rowNumber);

            //create new bar picture to display level
            PictureBox picture = new PictureBox { Name = "pictureBox" + rowNumber, Size = new Size(200, 20), Visible = true, Anchor = AnchorStyles.None };
            int ratio = (int)Math.Ceiling(tempoDistance / maxIntervalMiles * 200);
            picture.Paint += new PaintEventHandler((sender, e) => longRunLevel_Paint(sender, e, ratio, Color.Orange));
            tempoWorkoutTable.Controls.Add(picture, 9, rowNumber);

        }

        private void updateTempoTable()
        {

            // if row count is decreasing then remove rows
            if (Math.Ceiling(numCyclesNumInput.Value) < tempoWorkoutTable.RowCount)
            {
                //loop through rown
                for (int ii = (int)Math.Ceiling(numCyclesNumInput.Value); ii < tempoWorkoutTable.RowCount; ii++)
                {
                    //remove all controls in row
                    for (int jj = 0; jj < tempoWorkoutTable.ColumnCount; jj++)
                    {
                        Control Control = tempoWorkoutTable.GetControlFromPosition(jj, ii);
                        tempoWorkoutTable.Controls.Remove(Control);
                    }

                }

                //update teh row count
                tempoWorkoutTable.RowCount = (int)Math.Ceiling(numCyclesNumInput.Value);

            }
            //else add new rows to the form
            else if ((int)Math.Ceiling(numCyclesNumInput.Value) > tempoWorkoutTable.RowCount)
            {
                int previousRowCount = tempoWorkoutTable.RowCount;
                tempoWorkoutTable.RowCount = (int)Math.Ceiling(numCyclesNumInput.Value);
                for (int ii = previousRowCount; ii < (int)Math.Ceiling(numCyclesNumInput.Value); ii++)
                {
                    //add new speed workout row
                    addNewTempoWorkoutRow(ii);

                }
            }

        }

        #endregion

        #region Easy Table
        private void createEasyRunTable()
        {
            //determine number of days
            int numberOfDaysInCycle = (int)Math.Ceiling(numDaysNumInput.Value);

            //set auto scroll to true
            easyRunPanel.AutoScroll = true;

            //clear out existing controls and create header
            easyRunTableHeader.Controls.Clear();
            easyRunTableHeader.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            easyRunTableHeader.AutoSize = true;
            easyRunTableHeader.ColumnCount = (numberOfDaysInCycle*2)+1;
            easyRunTableHeader.RowCount = 1;
            easyRunTableHeader.ColumnStyles.Clear();
            easyRunTableHeader.RowStyles.Clear();

            easyRunTableHeader.RowStyles.Add(new RowStyle(SizeType.Absolute));
            easyRunTableHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute));
            easyRunTableHeader.ColumnStyles[0].Width = 50;
            easyRunTableHeader.RowStyles[0].Height = 30;
            easyRunTableHeader.Controls.Add(new Label() { Text = "Cycle", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, 0);

            //add a column for each day
            int columnCount = 0;

            //add a control for each day
            while (columnCount < (numberOfDaysInCycle * 2))
            {

                easyRunTableHeader.RowStyles.Add(new RowStyle(SizeType.Absolute));
                easyRunTableHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute));
                easyRunTableHeader.Controls.Add(new Label() { Text = "Day "+ (columnCount/2 + 1).ToString(), Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, columnCount + 1, 0);
                easyRunTableHeader.ColumnStyles[columnCount + 1].Width = 80;

                easyRunTableHeader.RowStyles.Add(new RowStyle(SizeType.Absolute));
                easyRunTableHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute));
                easyRunTableHeader.Controls.Add(new Label() { Text = "", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, columnCount + 2, 0);
                easyRunTableHeader.ColumnStyles[columnCount + 2].Width = 50;

                columnCount = columnCount + 2;

            }


            //clear out controls from table and intilize to correct size 
            easyRunTable.Controls.Clear();
            easyRunTable.AutoSize = true;
            easyRunTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            easyRunTable.ColumnCount = numberOfDaysInCycle*2+1;
            easyRunTable.RowCount = (int)Math.Ceiling(numCyclesNumInput.Value);
            easyRunTable.ColumnStyles.Clear();
            easyRunTable.RowStyles.Clear();

            //build the table
            for (int ii = 0; ii < (int)Math.Ceiling(numCyclesNumInput.Value); ii++)
            {
                addNewEasyRunRow(ii);
            }

        }

        private void addNewEasyRunRow(int rowNumber)
        {

            //determine number of days
            int numberOfDaysInCycle = (int)Math.Ceiling(numDaysNumInput.Value);

            //add cycle number to table
            easyRunTable.RowStyles.Add(new RowStyle(SizeType.Absolute));
            easyRunTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute));
            easyRunTable.RowStyles[0].Height = 30;
            easyRunTable.ColumnStyles[0].Width = 50;
            easyRunTable.Controls.Add(new Label() { Text = (rowNumber+1).ToString(), Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill } , 0, rowNumber);

            int columnCount = 0;

            //add a control for each day
            int easyCount = 0;
            List<int> easyIndex = new List<int>();
            List<double> previousMileage = new List<double>();
            double totalUsedDistance = 0;
            double lastDistance = 0;

            //loop through the days to create contols to figure out how many miles are availible for easy running
            while (columnCount < (numberOfDaysInCycle * 2))
            {
                //create new combo box to select run type
                ComboBox dailyControl = new ComboBox();
                dailyControl.Items.Add("Easy Run");
                dailyControl.Items.Add("Long Run");
                dailyControl.Items.Add("Tempo");
                dailyControl.Items.Add("Speed Workout");
                dailyControl.Items.Add("Rest");
                dailyControl.Anchor = AnchorStyles.None;
                dailyControl.Dock = DockStyle.Fill;
                dailyControl.DropDownStyle = ComboBoxStyle.DropDownList;
                dailyControl.FlatStyle = FlatStyle.Flat;

                //create new numerical input to edit data
                NumericUpDown thisNumericUpDown = new NumericUpDown();

                //add event handler for daily control
                dailyControl.SelectedValueChanged += new EventHandler(tempoWorkout_ValueChanged);
                dailyControl.SelectedValueChanged += new EventHandler(runComboBox_ValueChanged);

                //get teh desired run type for this day
                String runThisDay = ((ComboBox)workoutBuilderTable.GetControlFromPosition(1, columnCount/2)).Text;

                //memory to store the distance of this run
                decimal thisDistance = 0;

                //get the distance from the correct table based on the type of run
                switch (runThisDay){
                    case "Easy Run":
                        dailyControl.Text = "Easy Run";
                        dailyControl.BackColor = Color.LightGreen;
                        easyCount++; //increment the number of easy days
                        easyIndex.Add(columnCount+2); //add the index of the easy day
                        previousMileage.Add(lastDistance); //store the mileage of the day before teh easy day
                        break;
                    case "Long Run":
                        dailyControl.Text = "Long Run";
                        dailyControl.BackColor = Color.Green;
                        thisDistance = ((NumericUpDown)longRunCycleTable.GetControlFromPosition(1, rowNumber)).Value;
                        thisNumericUpDown.Value = thisDistance;
                        thisNumericUpDown.Enabled = false;
                        lastDistance = (double)thisDistance; //store this daistance for the future
                        break;
                    case "Tempo":
                        dailyControl.Text = "Tempo";
                        dailyControl.BackColor = Color.Orange;
                        thisDistance = Convert.ToDecimal(((Label)tempoWorkoutTable.GetControlFromPosition(7, rowNumber)).Text);
                        thisNumericUpDown.Value = thisDistance;
                        thisNumericUpDown.Enabled = false;
                        lastDistance = (double)thisDistance; //store this daistance for the future
                        break;
                    case "Speed Workout":
                        dailyControl.Text = "Speed Workout";
                        dailyControl.BackColor = Color.Orange;
                        thisDistance = Convert.ToDecimal(((Label)speedWorkoutTable.GetControlFromPosition(11, rowNumber)).Text);
                        thisNumericUpDown.Value = thisDistance;
                        thisNumericUpDown.Enabled = false;
                        lastDistance = (double)thisDistance; //store this daistance for the future
                        break;
                    case "Rest":
                        dailyControl.Text = "Rest";
                        dailyControl.BackColor = Color.LightBlue;
                        thisNumericUpDown.Value = 0;
                        thisNumericUpDown.Enabled = false;
                        lastDistance = 0; //store this daistance for the future
                        break;
                            
                }
                //accumulate the total distance
                totalUsedDistance = totalUsedDistance + (double)thisDistance;

                //add combo box to panel
                easyRunTable.RowStyles.Add(new RowStyle(SizeType.Absolute));
                easyRunTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute));
                easyRunTable.Controls.Add(dailyControl, columnCount + 1, rowNumber);
                easyRunTable.ColumnStyles[columnCount + 1].SizeType = SizeType.Absolute;
                easyRunTable.ColumnStyles[columnCount + 1].Width = 80;
                easyRunTable.RowStyles[columnCount + 1].Height = 30;

                //add numerical input to panel
                easyRunTable.RowStyles.Add(new RowStyle(SizeType.Absolute));
                easyRunTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute));
                easyRunTable.Controls.Add(thisNumericUpDown, columnCount + 2, rowNumber);
                easyRunTable.ColumnStyles[columnCount + 2].SizeType = SizeType.Absolute;
                easyRunTable.ColumnStyles[columnCount + 2].Width = 50;
                easyRunTable.RowStyles[columnCount + 2].Height = 30;

                //increment counter
                columnCount = columnCount + 2;
            }

            //figure out easy miles
            double desiredCycleMiles = (double)((NumericUpDown)cycleMileageTable.GetControlFromPosition(1,rowNumber)).Value;
            double remainingMiles = desiredCycleMiles - totalUsedDistance;
            double dailyMiles = Math.Floor(remainingMiles / easyCount);
            int remainder = (int)Math.Ceiling(remainingMiles % easyCount);

            //sort easy days based on how hard the previous day was
            var sorted = previousMileage
                .Select((x, i) => new KeyValuePair<double, int>(x, i))
                .OrderBy(x => x.Key)
                .ToList();

            List<double> sortedValue = sorted.Select(x => x.Key).ToList();
            List<int> idx = sorted.Select(x => x.Value).ToList();

            //divide up easy miles
            double[] easyMiles = new double[idx.Count];
            int lastIndex = 0;

            //add extra miles to easist days
            for (int ii = 0; ii<remainder; ii++)
            {
                easyMiles[ii] = dailyMiles + 1;
                lastIndex = ii;

            }

            //use base miles for harder days
            for (int ii = lastIndex+1; ii < idx.Count; ii++)
            {
                easyMiles[ii] = dailyMiles;
            }


            //set the value of the easy controls
            for (int ii = 0; ii < idx.Count; ii++)
            {
                int thisControlIndex = easyIndex[idx[ii]];
                ((NumericUpDown)easyRunTable.GetControlFromPosition(thisControlIndex, rowNumber)).Value = Math.Max((decimal)easyMiles[ii],0);
            }

        }

        private void updateEasyRunTable()
        {

            // if row count is decreasing then remove rows
            if (Math.Ceiling(numCyclesNumInput.Value) < easyRunTable.RowCount)
            {
                //loop through rown
                for (int ii = (int)Math.Ceiling(numCyclesNumInput.Value); ii < easyRunTable.RowCount; ii++)
                {
                    //remove all controls in row
                    for (int jj = 0; jj < easyRunTable.ColumnCount; jj++)
                    {
                        Control Control = easyRunTable.GetControlFromPosition(jj, ii);
                        easyRunTable.Controls.Remove(Control);
                    }

                }

                //update teh row count
                easyRunTable.RowCount = (int)Math.Ceiling(numCyclesNumInput.Value);

            }
            //else add new rows to the form
            else if ((int)Math.Ceiling(numCyclesNumInput.Value) > easyRunTable.RowCount)
            {
                int previousRowCount = easyRunTable.RowCount;
                easyRunTable.RowCount = (int)Math.Ceiling(numCyclesNumInput.Value);
                for (int ii = previousRowCount; ii < (int)Math.Ceiling(numCyclesNumInput.Value); ii++)
                {
                    //add new speed workout row
                    addNewEasyRunRow(ii);
                }
            }

        }
        #endregion


        #region Change Listeners

        private void numDays_ValueChanged(object sender, EventArgs e)
        {
            //recreate workoutbuilder table
            createWorkoputBuilderTable();

            //change number of cycles but leave number of weeks alone
            numCyclesNumInput.Value = numWeeksNumInput.Value * 7 / numDaysNumInput.Value;
        }

        private void numCycles_ValueChanged(object sender, EventArgs e)
        {
            //temporarly disable num weeks change listener
            numWeeksNumInput.ValueChanged -= new EventHandler(numWeeks_ValueChanged);

            //update number of weeks
            numWeeksNumInput.Value = numCyclesNumInput.Value * numDaysNumInput.Value / 7;

            //update tables
            updateCycleMileageTable();
            updateSpeedWorkoutTable();
            updateLongRunTable();
            updateTempoTable();
            updateEasyRunTable();
            refreshMileageData();

            //reenable change listener
            numWeeksNumInput.ValueChanged += new EventHandler(numWeeks_ValueChanged);

        }

        private void numWeeks_ValueChanged(object sender, EventArgs e)
        {
            //temporarly disable num cycles change listener
            numCyclesNumInput.ValueChanged -= new EventHandler(numCycles_ValueChanged);

            //update number of cycles
            numCyclesNumInput.Value = numWeeksNumInput.Value * 7 / numDaysNumInput.Value;

            //update tables
            updateCycleMileageTable();
            updateSpeedWorkoutTable();
            updateLongRunTable();
            updateTempoTable();
            updateEasyRunTable();
            refreshMileageData();

            //reenable change listener
            numCyclesNumInput.ValueChanged += new EventHandler(numCycles_ValueChanged);
        }

        private void gymComboBox_ValueChanged(object sender, EventArgs e)
        {
            ComboBox gymCmbBox = (ComboBox)sender;
            string selectedString = (string)gymCmbBox.SelectedItem;

            switch (selectedString)
            {
                case "Rest":
                    gymCmbBox.BackColor = Color.LightBlue;
                    break;
                case "Shoulders":
                    gymCmbBox.BackColor = Color.Yellow;
                    break;
                case "Abs":
                    gymCmbBox.BackColor = Color.Yellow;
                    break;
                case "Chest":
                    gymCmbBox.BackColor = Color.Yellow;
                    break;
                case "Back":
                    gymCmbBox.BackColor = Color.Yellow;
                    break;
                case "Legs":
                    gymCmbBox.BackColor = Color.Yellow;
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
                    runCmbBox.BackColor = Color.LightBlue;
                    break;
                case "Easy Run":
                    runCmbBox.BackColor = Color.LightGreen;
                    break;
                case "Long Run":
                    runCmbBox.BackColor = Color.Green;
                    break;
                case "Tempo":
                    runCmbBox.BackColor = Color.Orange;
                    break;
                case "Speed Workout":
                    runCmbBox.BackColor = Color.Orange;
                    break;
                default:
                    break;


            }
        }

        private void cycleMileageStartNumInput_ValueChanged(object sender, EventArgs e)
        {
            //create cycle milage table
            createCycleMileageTable();

            //refresh mileage plot
            refreshMileageData();
            
            //refresh the long run percentage and bar image
            refreshLongRunTable();

            //refresh tempo table
            refreshTempoTable();
        }

        private void cycleMileageEndNumInput_ValueChanged(object sender, EventArgs e)
        {
            //create cycle milage table
            createCycleMileageTable();

            //refresh mileage plot
            refreshMileageData();

            //refresh the long run percentage and bar image
            refreshLongRunTable();

            //refresh tempo table
            refreshTempoTable();
        }

        private void cycleMileageRatioNumInput_ValueChanged(object sender, EventArgs e)
        {
            //create cycle milage table
            createCycleMileageTable();

            //refresh mileage plot
            refreshMileageData();

            //refresh the long run percentage and bar image
            refreshLongRunTable();

            //refresh tempo table
            refreshTempoTable();
        }

        private void numSSDelta_ValueChanged(object sender, EventArgs e)
        {
            //create cycle milage table
            createCycleMileageTable();

            //refresh mileage plot
            refreshMileageData();

            //refresh the long run percentage and bar image
            refreshLongRunTable();

            //refresh tempo table
            refreshTempoTable();
        }

        private void numSSPeriod_ValueChanged(object sender, EventArgs e)
        {
            //create cycle milage table
            createCycleMileageTable();

            //refresh mileage plot
            refreshMileageData();

            //refresh the long run percentage and bar image
            refreshLongRunTable();

            //refresh tempo table
            refreshTempoTable();
        }

        private void cycleMileage_ValueChanged(object sender, EventArgs e)
        {
            //TODO: create refresh method and tie it to create method to make sure intial levels are set correctly

            //get active control
            Control thisControl = (Control)sender;

            //get position of control
            TableLayoutPanelCellPosition thisPosition = cycleMileageTable.GetPositionFromControl(thisControl);

            //get and remove bar chart from this row
            Control barPicture = cycleMileageTable.GetControlFromPosition(thisPosition.Column + 1, thisPosition.Row);
            cycleMileageTable.Controls.Remove(barPicture);

            //get  the mileage target for this cycle
            double doubleCycleMileage = (double)((NumericUpDown)thisControl).Value;

            //get maximum mileage and the mileage target for this cycle
            double maxCycleMiles = 0;
            for (int ii = 0; ii < Math.Ceiling(numCyclesNumInput.Value); ii++)
            {
                NumericUpDown thisNumericUpDown = (NumericUpDown)cycleMileageTable.GetControlFromPosition(1, ii);
                double cycleMiles = (double)thisNumericUpDown.Value;
                if (cycleMiles > maxCycleMiles)
                {
                    maxCycleMiles = cycleMiles;
                }
            }

            //create new picture box for this control and add to the table
            PictureBox picture = new PictureBox { Size = new Size(200, 20), Visible = true, Anchor = AnchorStyles.None, };
            picture.Paint += new PaintEventHandler((paintsender, arg) => cycleMileageLevel_Paint(paintsender, arg, (int)Math.Ceiling(doubleCycleMileage / maxCycleMiles * 200)));
            cycleMileageTable.Controls.Add(picture, thisPosition.Column + 1, thisPosition.Row);

            //update the rest of the mileage controls
            for (int ii = 0; ii < Math.Ceiling(numCyclesNumInput.Value); ii++)
            {
                //get the current control
                NumericUpDown thisNumericUpDown = (NumericUpDown)cycleMileageTable.GetControlFromPosition(1, ii);
                double thisCycleMileage = (double)thisNumericUpDown.Value;

                //get the coresponding bar chart and remove it
                Control thisBarControl = cycleMileageTable.GetControlFromPosition(thisPosition.Column + 1, ii);
                cycleMileageTable.Controls.Remove(thisBarControl);

                //create a new bar chart and add it to the diagram
                PictureBox thisPicture = new PictureBox { Size = new Size(200, 20), Visible = true, Anchor = AnchorStyles.None, };
                thisPicture.Paint += new PaintEventHandler((paintsender, arg) => cycleMileageLevel_Paint(paintsender, arg, (int)Math.Ceiling(thisCycleMileage / maxCycleMiles * 200)));
                cycleMileageTable.Controls.Add(thisPicture, thisPosition.Column + 1, ii);

            }

            //refresh mileage plot
            refreshMileageData();

            //refresh the long run percentage and bar image
            refreshLongRunTable();

            //refresh tempo table
            refreshTempoTable();

        }


        private void numLongRunMiles_ValueChanged(object sender, EventArgs e)
        {
            //refresh the long run percentage and bar image
            refreshLongRunTable();
        }

        private void refreshLongRunTable()
        {

            //find max value of long run miles
            int numLevels = (int)Math.Ceiling(numCyclesNumInput.Value);
            double maxLongRunMiles = 0;
            for (int ii = 0; ii < numLevels; ii++)
            {
                NumericUpDown thisNumericUpDown = (NumericUpDown)longRunCycleTable.GetControlFromPosition(1, ii);
                double longRunMiles = (double)thisNumericUpDown.Value;
                if (longRunMiles > maxLongRunMiles)
                {
                    maxLongRunMiles = longRunMiles;
                }
            }

            //refresh the level of the rest of the bar controls
            for (int ii = 0; ii < numLevels; ii++)
            {

                //get the mileage of this level
                NumericUpDown thisNumericUpDown = (NumericUpDown)longRunCycleTable.GetControlFromPosition(1, ii);
                double thisLongRunCycleMileage = (double)((NumericUpDown)thisNumericUpDown).Value;

                //get the coresponding percentage label and level
                Label thisPercentageLabel = (Label)longRunCycleTable.GetControlFromPosition(2, ii);

                //get coresponding cycle mileage
                NumericUpDown numCycleMileage = (NumericUpDown)cycleMileageTable.GetControlFromPosition(1, ii);

                //calculate cycle factor to normalize cycle mileage to weekly mileage 
                double cycleFactor = 7 / (double)numDaysNumInput.Value;

                //get percentage label and update text with new value
                double percentageOfWeeklyMiles = Math.Round(thisLongRunCycleMileage / (double)numCycleMileage.Value / cycleFactor * 1000) / 10;
                thisPercentageLabel.Text = percentageOfWeeklyMiles.ToString();

                //set bar color based on weekly percentage
                Color thisBarColor;
                if ((percentageOfWeeklyMiles > (double)numMaxWeeklyPct.Value) || (percentageOfWeeklyMiles < (double)numMinWeeklyPct.Value))
                {
                    thisBarColor = Color.Red;
                }
                else
                {
                    thisBarColor = Color.Green;
                }

                //get the and remove bar picture control
                Control thisBarControl = longRunCycleTable.GetControlFromPosition(3, ii);
                longRunCycleTable.Controls.Remove(thisBarControl);

                //create a new bar picture and add it to the table
                PictureBox thisPicture = new PictureBox { Size = new Size(200, 20), Visible = true, Anchor = AnchorStyles.None, };
                thisPicture.Paint += new PaintEventHandler((paintsender, arg) => longRunLevel_Paint(paintsender, arg, (int)Math.Ceiling(thisLongRunCycleMileage / maxLongRunMiles * 200), thisBarColor));
                longRunCycleTable.Controls.Add(thisPicture, 3, ii);
            }
        }

        


        private void cycleMileageLevel_Paint(object sender, PaintEventArgs e, int size)
        {
            Rectangle ee = new Rectangle(0, 0, size, 20);

            using (Brush thisBrush = new SolidBrush(Color.Blue))
            {
                e.Graphics.FillRectangle(thisBrush, ee);
            }
        }

        private void longRunLevel_Paint(object sender, PaintEventArgs e, int size, Color barColor)
        {
            Rectangle ee = new Rectangle(0, 0, size, 20);

            using (Brush thisBrush = new SolidBrush(barColor))
            {
                e.Graphics.FillRectangle(thisBrush, ee);
            }
        }

        private void speedWorkout_ValueChanged(object sender, EventArgs e)
        {
            //loop through table and update total distance values
            for (int ii = 0; ii < speedWorkoutTable.RowCount; ii++)
            {

                double warmDistance;
                double intervalDistance;
                double restDistance;
                double coolDistance;

                //warmup distance
                String warmUnits = speedWorkoutTable.GetControlFromPosition(2, ii).Text;
                double warmupDist = (double)((NumericUpDown)speedWorkoutTable.GetControlFromPosition(1, ii)).Value;

                //interval distance
                String intervalUnits = speedWorkoutTable.GetControlFromPosition(5, ii).Text;
                double intervalDist = (double)((NumericUpDown)speedWorkoutTable.GetControlFromPosition(4, ii)).Value;

                //rest distance
                String restUnits = speedWorkoutTable.GetControlFromPosition(8, ii).Text;
                double restDist = (double)((NumericUpDown)speedWorkoutTable.GetControlFromPosition(7, ii)).Value;

                //cooldown distance
                String coolUnits = speedWorkoutTable.GetControlFromPosition(10, ii).Text;
                double coolDist = (double)((NumericUpDown)speedWorkoutTable.GetControlFromPosition(9, ii)).Value;

                //number of reps
                int reps = (int)((NumericUpDown)speedWorkoutTable.GetControlFromPosition(3, ii)).Value;

                //convert based on units
                if (warmUnits == "Miles")
                {
                    warmDistance = warmupDist;
                }
                else
                {
                    warmDistance = warmupDist * 0.000621371;
                }

                if (intervalUnits == "Miles")
                {
                    intervalDistance = intervalDist;
                }
                else
                {
                    intervalDistance = intervalDist * 0.000621371;
                }

                if (restUnits == "Miles")
                {
                    restDistance = restDist;
                }
                else
                {
                    restDistance = restDist * 0.000621371;
                }

                if (coolUnits == "Miles")
                {
                    coolDistance = coolDist;
                }
                else
                {
                    coolDistance = coolDist * 0.000621371;
                }

                //calculate total distance
                double totalDistance = warmDistance + intervalDistance * (double)reps + restDistance * ((double)reps - 1.0) + coolDistance;

                //update table
                speedWorkoutTable.GetControlFromPosition(11, ii).Text = (Math.Round(totalDistance * 10) / 10).ToString();
            }
        }

        private void tempoWorkout_ValueChanged(object sender, EventArgs e)
        {
            refreshTempoTable();
        }

        private void refreshTempoTable()
        {

            //find max value of tempo miles
            int numLevels = (int)Math.Ceiling(numCyclesNumInput.Value);
            double maxTempoMiles = 0;
            for (int ii = 0; ii < numLevels; ii++)
            {
                NumericUpDown thisNumericUpDown = (NumericUpDown)tempoWorkoutTable.GetControlFromPosition(3, ii);
                double tempoMiles = (double)thisNumericUpDown.Value;
                if (tempoMiles > maxTempoMiles)
                {
                    maxTempoMiles = tempoMiles;
                }
            }

            //refresh the level of the rest of the bar controls
            for (int ii = 0; ii < numLevels; ii++)
            {

                //get the mileage of this level
                NumericUpDown thisNumericUpDown = (NumericUpDown)tempoWorkoutTable.GetControlFromPosition(3, ii);
                double thisTempoWorkoutMileage = (double)((NumericUpDown)thisNumericUpDown).Value;

                //get coresponding cycle mileage
                NumericUpDown numCycleMileage = (NumericUpDown)cycleMileageTable.GetControlFromPosition(1, ii);

                //calculate cycle factor to normalize cycle mileage to weekly mileage 
                double cycleFactor = 7 / (double)numDaysNumInput.Value;

                //get percentage label and update text with new value
                double percentageOfWeeklyMiles = Math.Round(thisTempoWorkoutMileage / (double)numCycleMileage.Value / cycleFactor * 1000) / 10;

                //set bar color based on weekly percentage
                Color thisBarColor;
                if ((percentageOfWeeklyMiles > (double)numMaxWeeklyTempo.Value) || (percentageOfWeeklyMiles < (double)numMinWeeklyTempo.Value))
                {
                    thisBarColor = Color.Red;
                }
                else
                {
                    thisBarColor = Color.Orange;
                }

                //get the and remove bar picture control
                Control thisBarControl = tempoWorkoutTable.GetControlFromPosition(9, ii);
                tempoWorkoutTable.Controls.Remove(thisBarControl);

                //create a new bar picture and add it to the table
                PictureBox thisPicture = new PictureBox { Size = new Size(200, 20), Visible = true, Anchor = AnchorStyles.None, };
                thisPicture.Paint += new PaintEventHandler((paintsender, arg) => longRunLevel_Paint(paintsender, arg, (int)Math.Ceiling(thisTempoWorkoutMileage / maxTempoMiles * 200), thisBarColor));
                tempoWorkoutTable.Controls.Add(thisPicture, 9, ii);

                double warmDistance;
                double tempoDistance;
                double coolDistance;

                //warmup distance
                String warmUnits = tempoWorkoutTable.GetControlFromPosition(2, ii).Text;
                double warmupDist = (double)((NumericUpDown)tempoWorkoutTable.GetControlFromPosition(1, ii)).Value;

                //tempo distance
                String intervalUnits = tempoWorkoutTable.GetControlFromPosition(4, ii).Text;
                double intervalDist = (double)((NumericUpDown)tempoWorkoutTable.GetControlFromPosition(3, ii)).Value;

                //cooldown distance
                String coolUnits = tempoWorkoutTable.GetControlFromPosition(6, ii).Text;
                double coolDist = (double)((NumericUpDown)tempoWorkoutTable.GetControlFromPosition(5, ii)).Value;


                //convert based on units
                if (warmUnits == "Miles")
                {
                    warmDistance = warmupDist;
                }
                else
                {
                    warmDistance = warmupDist * 0.000621371;
                }

                if (intervalUnits == "Miles")
                {
                    tempoDistance = intervalDist;
                }
                else
                {
                    tempoDistance = intervalDist * 0.000621371;
                }


                if (coolUnits == "Miles")
                {
                    coolDistance = coolDist;
                }
                else
                {
                    coolDistance = coolDist * 0.000621371;
                }

                //calculate total distance
                double totalDistance = warmDistance + tempoDistance + coolDistance;

                //update table
                tempoWorkoutTable.GetControlFromPosition(7, ii).Text = (Math.Round(totalDistance * 10) / 10).ToString();
                tempoWorkoutTable.GetControlFromPosition(8, ii).Text = (Math.Round(tempoDistance / (cycleFactor * (double)numCycleMileage.Value) * 100 * 10) / 10).ToString();

            }
        }


        private void numOfLongRunLevels_ValueChanged(object sender, EventArgs e)
        {
            //update long run level table
            createLongRunLevelTable();
        }

        public void longRunLevel_ValueChanged(object sender, EventArgs e)
        {
            //Do nothing
        }

        private void numMinWeeklyPct_ValueChanged(object sender, EventArgs e)
        {
            //refresh the long run table
            refreshLongRunTable();
        }

        private void numMaxWeeklyPct_ValueChanged(object sender, EventArgs e)
        {
            //refresh the long run table
            refreshLongRunTable();
        }

        private void btnGenerateTable_Click(object sender, EventArgs e)
        {
            //recreate long run table
            createLongRunTable();
        }

        private void btnGenerateTempoTable_Click(object sender, EventArgs e)
        {
            //recreate tempo table
            createTempoWorkoutTable();
        }

        private void btnGenEasyTable_Click(object sender, EventArgs e)
        {
            //recreate easy table
            createEasyRunTable();
        }

        #endregion
        #region Mileage Plot
        public void plotMileageData()
        {


            int numCycles = (int)Math.Ceiling(numCyclesNumInput.Value);
            xData = new double[numCycles];
            yDataTarget = new double[numCycles];
            yDataTempo = new double[numCycles];
            yDataSpeed = new double[numCycles];
            yDataLong = new double[numCycles];
            yDataEasy = new double[numCycles];

            mileageChart.Series.Clear();

            for (int ii = 0; ii < numCycles; ii++)
            {
                xData[ii] = ii + 1;
                yDataTarget[ii] = (double)((NumericUpDown)cycleMileageTable.GetControlFromPosition(1, ii)).Value;
                yDataTempo[ii] = (double)Convert.ToDouble(((Label)tempoWorkoutTable.GetControlFromPosition(7, ii)).Text);
                yDataSpeed[ii] = (double)Convert.ToDouble(((Label)speedWorkoutTable.GetControlFromPosition(11, ii)).Text);
                yDataLong[ii] = (double)((NumericUpDown)longRunCycleTable.GetControlFromPosition(1, ii)).Value;
                yDataEasy[ii] = 10;
            }

            //Vertical bar chart
            //create another area and add it to the chart
            ChartArea area = new ChartArea("Mileage Data");
            area.AxisX.Title = "Cycle #";
            area.AxisY.Title = "Mileage";
            mileageChart.ChartAreas.Add(area);

            //Create the series using just the y data
            Series targetBarSeries = new Series();
            targetBarSeries.Points.DataBindY(yDataTarget);
            targetBarSeries.Color = Color.Blue;
            targetBarSeries.MarkerSize = 10;
            targetBarSeries.ChartType = SeriesChartType.Point;
            targetBarSeries.ChartArea = "Mileage Data";

            Series tempoBarSeries = new Series();
            tempoBarSeries.Points.DataBindY(yDataTempo);
            tempoBarSeries.Color = Color.Orange;
            tempoBarSeries.ChartType = SeriesChartType.StackedColumn;
            tempoBarSeries.ChartArea = "Mileage Data";

            Series speedBarSeries = new Series();
            speedBarSeries.Points.DataBindY(yDataSpeed);
            speedBarSeries.Color = Color.Orange;
            speedBarSeries.ChartType = SeriesChartType.StackedColumn;
            speedBarSeries.ChartArea = "Mileage Data";

            Series longBarSeries = new Series();
            longBarSeries.Points.DataBindY(yDataLong);
            longBarSeries.Color = Color.DarkGreen;
            longBarSeries.ChartType = SeriesChartType.StackedColumn;
            longBarSeries.ChartArea = "Mileage Data";

            Series easyBarSeries = new Series();
            easyBarSeries.Points.DataBindY(yDataEasy);
            easyBarSeries.Color = Color.LightGreen;
            easyBarSeries.ChartType = SeriesChartType.StackedColumn;
            easyBarSeries.ChartArea = "Mileage Data";


            //Add the series to the chart
           
            mileageChart.Series.Add(tempoBarSeries);
            mileageChart.Series.Add(speedBarSeries);
            mileageChart.Series.Add(longBarSeries);
            mileageChart.Series.Add(easyBarSeries);


            mileageChart.Series.Add(targetBarSeries);
        }

        public void refreshMileageData()
        {

            int numCycles = (int)Math.Ceiling(numCyclesNumInput.Value);
            xData = new double[numCycles];
            yDataTarget = new double[numCycles];
            yDataTempo = new double[numCycles];
            yDataSpeed = new double[numCycles];
            yDataLong = new double[numCycles];
            yDataEasy = new double[numCycles];

            mileageChart.Series.Clear();

            for (int ii = 0; ii < numCycles; ii++)
            {
                xData[ii] = ii + 1;
                yDataTarget[ii] = (double)((NumericUpDown)cycleMileageTable.GetControlFromPosition(1, ii)).Value;
                yDataTempo[ii] = (double)Convert.ToDouble(((Label)tempoWorkoutTable.GetControlFromPosition(7, ii)).Text);
                yDataSpeed[ii] = (double)Convert.ToDouble(((Label)speedWorkoutTable.GetControlFromPosition(11, ii)).Text);
                yDataLong[ii] = (double)((NumericUpDown)longRunCycleTable.GetControlFromPosition(1, ii)).Value;
                yDataEasy[ii] = 10;
            }

            //Vertical bar chart
            //create another area and add it to the chart
            ChartArea area = new ChartArea("Mileage Data");
            area.AxisX.Title = "Cycle #";
            area.AxisY.Title = "Mileage";
            mileageChart.ChartAreas.Clear();
            mileageChart.ChartAreas.Add(area);

            //Create the series using just the y data
            Series targetBarSeries = new Series();
            targetBarSeries.Points.DataBindY(yDataTarget);
            targetBarSeries.Color = Color.Blue;
            targetBarSeries.MarkerSize = 10;
            targetBarSeries.ChartType = SeriesChartType.Point;
            targetBarSeries.ChartArea = "Mileage Data";

            Series tempoBarSeries = new Series();
            tempoBarSeries.Points.DataBindY(yDataTempo);
            tempoBarSeries.Color = Color.Orange;
            tempoBarSeries.ChartType = SeriesChartType.StackedColumn;
            tempoBarSeries.ChartArea = "Mileage Data";

            Series speedBarSeries = new Series();
            speedBarSeries.Points.DataBindY(yDataSpeed);
            speedBarSeries.Color = Color.Orange;
            speedBarSeries.ChartType = SeriesChartType.StackedColumn;
            speedBarSeries.ChartArea = "Mileage Data";

            Series longBarSeries = new Series();
            longBarSeries.Points.DataBindY(yDataLong);
            longBarSeries.Color = Color.DarkGreen;
            longBarSeries.ChartType = SeriesChartType.StackedColumn;
            longBarSeries.ChartArea = "Mileage Data";

            Series easyBarSeries = new Series();
            easyBarSeries.Points.DataBindY(yDataEasy);
            easyBarSeries.Color = Color.LightGreen;
            easyBarSeries.ChartType = SeriesChartType.StackedColumn;
            easyBarSeries.ChartArea = "Mileage Data";


            //Add the series to the chart

            mileageChart.Series.Add(tempoBarSeries);
            mileageChart.Series.Add(speedBarSeries);
            mileageChart.Series.Add(longBarSeries);
            mileageChart.Series.Add(easyBarSeries);


            mileageChart.Series.Add(targetBarSeries);

        }


        #endregion


    }
}




