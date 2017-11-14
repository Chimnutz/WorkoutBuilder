using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WorkoutApp.ActivityTypes
{
    class RunButton : Button
    {

        private RunTypes runType { get; set; }
        private double totalMileage { get; set; }
        private double easyMileage { get; set; }
        private double workoutMileage { get; set; }
        
        private int numberOfReps { get; set; }

        private double warmupDistance { get; set; }
        private Units warmupDistanceUnits { get; set; }
        private Pace warmupPace { get; set; }

        private List<double> repDistance { get; set; }
        private List<Units> repDistanceUnits { get; set; }
        private List<Pace> repPace { get; set; }

        private List<double> repCoolDistance { get; set; }
        private List<Units> repCoolDistanceUnits { get; set; }
        private List<Pace> repCoolPace { get; set; }

        private double coolDistance { get; set; }
        private Units coolDistanceUnits { get; set; }
        private Pace coolPace { get; set; }

        public void setRunType(RunTypes runType)
        {
            this.runType = runType;
        }

        public void addWarmup(double distance, Units units, Pace pace)
        {
            this.warmupDistance = distance;
            this.warmupDistanceUnits = units;
            this.warmupPace = pace;
        }

        public void addWorkout(double distance, Units units, Pace pace)
        {
            this.repDistance.Add(distance);
            this.repDistanceUnits.Add(units);
            this.repPace.Add(pace);

            //determine number of reps based on list length
            this.numberOfReps = this.repDistance.Count;

        }

        //Add workout cooldown
        public void addRepCool(double distance, Units units, Pace pace)
        {
            this.repCoolDistance.Add(distance);
            this.repCoolDistanceUnits.Add(units);
            this.repCoolPace.Add(pace);
        }

        public void addCooldown(double distance, Units units, Pace pace)
        {
            this.coolDistance = distance;
            this.coolDistanceUnits = units;
            this.coolPace = pace;
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


        private void calculateTotalMileage()
        {
            double totalMileage = 0;

            double warmDistance = convertToMiles(this.warmupDistance, this.warmupDistanceUnits);

            double repDistance = 0;
            for(int ii = 0; ii< this.repDistance.Count; ii++)
            {
                repDistance = repDistance + convertToMiles(this.repDistance[ii], this.repDistanceUnits[ii]);
            }

            double repCoolDistance = 0;
            for (int ii = 0; ii < this.repDistance.Count; ii++)
            {
                repCoolDistance = repCoolDistance + convertToMiles(this.repCoolDistance[ii], this.repCoolDistanceUnits[ii]);
            }

            double coolDistance = convertToMiles(this.coolDistance, this.coolDistanceUnits);

            totalMileage = warmupDistance + repDistance + repCoolDistance + coolDistance;

            this.totalMileage = totalMileage;

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
                    repDistance = repDistance + convertToMiles(this.repDistance[ii], this.repDistanceUnits[ii]);
                }

                repCoolDistance = 0;
                for (int ii = 0; ii < this.repDistance.Count; ii++)
                {
                    repCoolDistance = repCoolDistance + convertToMiles(this.repCoolDistance[ii], this.repCoolDistanceUnits[ii]);
                }

                coolDistance = convertToMiles(this.coolDistance, this.coolDistanceUnits);

                totalMileage = warmupDistance + repDistance + repCoolDistance + coolDistance;
            }
            else if((runType == RunTypes.Tempo) || (runType == RunTypes.Interval) || (runType == RunTypes.Repition))
            {
                warmDistance = convertToMiles(this.warmupDistance, this.warmupDistanceUnits);

                repCoolDistance = 0;
                for (int ii = 0; ii < this.repDistance.Count; ii++)
                {
                    repCoolDistance = repCoolDistance + convertToMiles(this.repCoolDistance[ii], this.repCoolDistanceUnits[ii]);
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
                    repDistance = repDistance + convertToMiles(this.repDistance[ii], this.repDistanceUnits[ii]);
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
