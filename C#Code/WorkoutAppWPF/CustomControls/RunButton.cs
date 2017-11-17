using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace WorkoutAppWPF.ActivityTypes
{
    class RunButton : Button
    {

        private RunTypes runType { get; set; }
        private double totalMileage { get; set; }
        private double easyMileage { get; set; }
        private double workoutMileage { get; set; }
        
        private List<int> numberOfReps { get; set; } = new List<int>();

        private double warmupDistance { get; set; } = 0;
        private Units warmupDistanceUnits { get; set; } = Units.Miles;
        private Pace warmupPace { get; set; } = Pace.Easy;

        private List<double> repDistance { get; set; } = new List<double>();
        private List<Units> repDistanceUnits { get; set; } = new List<Units>();
        private List<Pace> repPace { get; set; } = new List<Pace>();

        private List<double> repCoolDistance { get; set; } = new List<double>();
        private List<Units> repCoolDistanceUnits { get; set; } = new List<Units>();
        private List<Pace> repCoolPace { get; set; } = new List<Pace>();

        private double coolDistance { get; set; } = 0;
        private Units coolDistanceUnits { get; set; } = Units.Miles;
        private Pace coolPace { get; set; } = Pace.Easy;


        public void clearData()
        {


        numberOfReps =  new List<int>();

        warmupDistance  = 0;
        warmupDistanceUnits  = Units.Miles;
        warmupPace = Pace.Easy;

        repDistance  = new List<double>();
        repDistanceUnits  = new List<Units>();
        repPace  = new List<Pace>();

        repCoolDistance  = new List<double>();
        repCoolDistanceUnits  = new List<Units>();
        repCoolPace  = new List<Pace>();

        coolDistance  = 0;
        coolDistanceUnits  = Units.Miles;
        coolPace = Pace.Easy;
    }

        public void setRunType(RunTypes runType)
        {
            this.runType = runType;

            switch (runType)
            {
                case RunTypes.Rest:
                    this.Background = Brushes.LightBlue;
                    break;
                case RunTypes.Easy:
                    this.Background = Brushes.LightGreen;
                    break;
                case RunTypes.Long:
                    this.Background = Brushes.Green;
                    break;
                case RunTypes.Interval:
                    this.Background = Brushes.Orange;
                    break;
                case RunTypes.Tempo:
                    this.Background = Brushes.Orange;
                    break;
                default:
                    this.Background = Brushes.DarkGray;
                    break;
            }

            //update label
            if(this.runType == RunTypes.Rest)
            {
                this.Content = "Rest";
            }else
            {
                this.Content = this.runType.ToString() + " " + (Math.Round(totalMileage * 2) / 2).ToString();
            }
            
        }

        public void addWarmup(double distance, Units units, Pace pace)
        {
            this.warmupDistance = distance;
            this.warmupDistanceUnits = units;
            this.warmupPace = pace;

            calculateTotalMileage();
            calculateEasyMileage();
            calculateWorkoutMileage();
        }

        public void addWorkout(int numOfReps, double repDistance, Units repUnits, Pace repPace, double repCoolDistance, Units repCoolUnits, Pace repCoolPace)
        {
            this.repDistance.Add(repDistance);
            this.repDistanceUnits.Add(repUnits);
            this.repPace.Add(repPace);

            this.repCoolDistance.Add(repCoolDistance);
            this.repCoolDistanceUnits.Add(repCoolUnits);
            this.repCoolPace.Add(repCoolPace);
            this.numberOfReps.Add(numOfReps);

            calculateTotalMileage();
            calculateEasyMileage();
            calculateWorkoutMileage();

            //determine number of reps based on list length
            

        }

        public void addCooldown(double distance, Units units, Pace pace)
        {
            this.coolDistance = distance;
            this.coolDistanceUnits = units;
            this.coolPace = pace;

            calculateTotalMileage();
            calculateEasyMileage();
            calculateWorkoutMileage();
        }

        public RunTypes getRunType()
        {
            return this.runType;
        }

        public double getTotalMileage()
        {
            return this.totalMileage;
        }

        public double getEasyMileage()
        {
            return this.easyMileage;
        }

        public double getWorkoutMileage()
        {
            return this.workoutMileage;
        }

        //Warmup
        public double getWarmupDistance()
        {
            return this.warmupDistance;
        }

        public Units getWarmupUnits()
        {
            return this.warmupDistanceUnits;
        }

        public Pace getWarmupPace()
        {
            return this.warmupPace;
        }

        //Cool Down
        public double getCoolDistance()
        {
            return this.coolDistance;
        }

        public Units getCoolUnits()
        {
            return this.coolDistanceUnits;
        }

        public Pace getCoolPace()
        {
            return this.coolPace;
        }

        //Add workout rep
        public List<double> getRepDistance()
        {
            return this.repDistance;
        }

        public List<Units> getRepUnits()
        {
            return this.repDistanceUnits;
        }

        public List<Pace> getRepPace()
        {
            return this.repPace;
        }

        public List<double> getRepCoolDistance()
        {
            return this.repCoolDistance;
        }

        public List<Units> getRepCoolUnits()
        {
            return this.repCoolDistanceUnits;
        }

        public List<Pace> getRepCoolPace()
        {
            return this.repCoolPace;
        }

        public List<int> getNumberOfReps()
        {
            return this.numberOfReps;
        }

        private void calculateTotalMileage()
        {
            double totalMileage = 0;

            double warmDistance = convertToMiles(this.warmupDistance, this.warmupDistanceUnits);

            double repDistance = 0;
            for(int ii = 0; ii< this.repDistance.Count; ii++)
            {
                repDistance = repDistance + convertToMiles(this.repDistance[ii], this.repDistanceUnits[ii])*this.numberOfReps[ii];
            }

            double repCoolDistance = 0;
            for (int ii = 0; ii < this.repCoolDistance.Count; ii++)
            {
                repCoolDistance = repCoolDistance + convertToMiles(this.repCoolDistance[ii], this.repCoolDistanceUnits[ii]) * this.numberOfReps[ii];
            }

            double coolDistance = convertToMiles(this.coolDistance, this.coolDistanceUnits);

            totalMileage = warmupDistance + repDistance + repCoolDistance + coolDistance;

            this.totalMileage = totalMileage;

            //update label
            if (this.runType == RunTypes.Rest)
            {
                this.Content = "Rest";
            }
            else
            {
                this.Content = this.runType.ToString() + " " + (Math.Round(totalMileage * 2) / 2).ToString();
            }
        }

        private void calculateEasyMileage()
        {
            double totalMileage = 0;
            double warmDistance = 0;
            double repDistance = 0;
            double repCoolDistance = 0;
            double coolDistance = 0;

            if ((runType == RunTypes.Easy)|| (runType == RunTypes.Long))
            {
                warmDistance = convertToMiles(this.warmupDistance, this.warmupDistanceUnits);

                repDistance = 0;
                for (int ii = 0; ii < this.repDistance.Count; ii++)
                {
                    repDistance = repDistance + convertToMiles(this.repDistance[ii], this.repDistanceUnits[ii]) * this.numberOfReps[ii];
                }

                repCoolDistance = 0;
                for (int ii = 0; ii < this.repCoolDistance.Count; ii++)
                {
                    repCoolDistance = repCoolDistance + convertToMiles(this.repCoolDistance[ii], this.repCoolDistanceUnits[ii]) * this.numberOfReps[ii];
                }

                coolDistance = convertToMiles(this.coolDistance, this.coolDistanceUnits);

                totalMileage = warmupDistance + repDistance + repCoolDistance + coolDistance;
            }
            else if((runType == RunTypes.Tempo) || (runType == RunTypes.Interval) || (runType == RunTypes.Repition))
            {
                warmDistance = convertToMiles(this.warmupDistance, this.warmupDistanceUnits);

                repCoolDistance = 0;
                for (int ii = 0; ii < this.repCoolDistance.Count; ii++)
                {
                    repCoolDistance = repCoolDistance + convertToMiles(this.repCoolDistance[ii], this.repCoolDistanceUnits[ii]) * this.numberOfReps[ii];
                }

                coolDistance = convertToMiles(this.coolDistance, this.coolDistanceUnits);

                totalMileage = warmupDistance + repCoolDistance + coolDistance;
            }else
            {
                totalMileage = 0;
            }
            this.easyMileage = totalMileage;
        }

        private void calculateWorkoutMileage()
        {
            double totalMileage = 0;
            double repDistance = 0;

            if ((runType == RunTypes.Tempo) || (runType == RunTypes.Interval) || (runType == RunTypes.Repition))
            {
                repDistance = 0;
                for (int ii = 0; ii < this.repDistance.Count; ii++)
                {
                    repDistance = repDistance + convertToMiles(this.repDistance[ii], this.repDistanceUnits[ii]) * this.numberOfReps[ii];
                }

                totalMileage = repDistance;

            }
            else
            {
                totalMileage = 0;
            }

            this.workoutMileage = totalMileage;
        }

        private double convertToMiles(double distance, Units units)
        {
            double unitFactor = 1;
            double convertedMiles = 0;

            switch (units) {
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
