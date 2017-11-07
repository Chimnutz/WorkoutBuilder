using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutApp.ActivityTypes
{
    class Run
    {

        private RunTypes runType { get; set; }
        private double totalMileage { get; set; }
        private double easyMileage { get; set; }
        private double workoutMileage { get; set; }
        
        private int numberOfReps { get; set; }

        private List<double> warmupDistance { get; set; }
        private List<Units> warmupDistanceUnits { get; set; }
        private List<Pace> warmupPace { get; set; }

        private List<double> repDistance { get; set; }
        private List<Units> repDistanceUnits { get; set; }
        private List<Pace> repPace { get; set; }

        private List<double> repCoolDistance { get; set; }
        private List<Units> repCoolDistanceUnits { get; set; }
        private List<Pace> repCoolPace { get; set; }

        private List<double> coolDistance { get; set; }
        private List<double> coolUnits { get; set; }
        private List<Pace> coolPace { get; set; }

    }
}
