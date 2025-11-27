using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage1._0.Vehicles
{
    internal class Motorcycle : Vehicle
    {
        public int CylinderVolume { get; set; }
        public Motorcycle(string regnr, string color, int weels, string model) : base(regnr, color, weels, model)
        {
        }

        public Motorcycle(string regnr, string color, int weels, string model, int cylinderVolume) : base(regnr, color, weels, model)
        {
            this.CylinderVolume = cylinderVolume;
        }

        protected override string GetVehicleInfo()
        {
            return base.GetVehicleInfo() + $", Cylindervolym : {CylinderVolume}";
        }
    }
}
