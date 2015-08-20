using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;


namespace Conversion_App
{
    public class Calculations
    {
        public double Length_Calculation(double inputNum, int unitIndex1, int unitIndex2)
        {
            double answer = -1;

            // Conversions

            if (unitIndex1 == 0) // from inches
            {
                if (unitIndex2 == 0) // to inches
                    answer = inputNum;
                else if (unitIndex2 == 1) // to feet
                    answer = inputNum / 12;
                else if (unitIndex2 == 2) // to yards
                    answer = inputNum / 36;
                else if (unitIndex2 == 3) // to miles
                    answer = inputNum / 63360;
                else if (unitIndex2 == 4) // to millimeters
                    answer = inputNum * 25.4;
                else if (unitIndex2 == 5) // to centimeters
                    answer = inputNum * 2.54;
                else if (unitIndex2 == 6) // to meters
                    answer = inputNum * 0.0254;
                else if (unitIndex2 == 7) // to kilometers
                    answer = inputNum / 39370.1;
            }
            else if (unitIndex1 == 1) // from feet
            {
                if (unitIndex2 == 0) // to inches
                    answer = inputNum * 12;
                else if (unitIndex2 == 1) // to feet
                    answer = inputNum;
                else if (unitIndex2 == 2) // to yards
                    answer = inputNum / 3;
                else if (unitIndex2 == 3) // to miles
                    answer = inputNum / 5280;
                else if (unitIndex2 == 4) // to millimeters
                    answer = inputNum * 304.8;
                else if (unitIndex2 == 5) // to centimeters
                    answer = inputNum * 30.48;
                else if (unitIndex2 == 6) // to meters
                    answer = inputNum * 0.3048;
                else if (unitIndex2 == 7) // to kilometers
                    answer = inputNum / 3280.84;
            }
            else if (unitIndex1 == 2) // from yards
            {
                if (unitIndex2 == 0) // to inches
                    answer = inputNum * 36;
                else if (unitIndex2 == 1) // to feet
                    answer = inputNum * 3;
                else if (unitIndex2 == 2) // to yards
                    answer = inputNum;
                else if (unitIndex2 == 3) // to miles
                    answer = inputNum / 1760;
                else if (unitIndex2 == 4) // to millimeters
                    answer = inputNum * 914.4;
                else if (unitIndex2 == 5) // to centimeters
                    answer = inputNum * 91.44;
                else if (unitIndex2 == 6) // to meters
                    answer = inputNum * 0.9144;
                else if (unitIndex2 == 7) // to kilometers
                    answer = inputNum / 1093.61;
            }
            else if (unitIndex1 == 3) // from miles
            {
                if (unitIndex2 == 0) // to inches
                    answer = inputNum * 63360;
                else if (unitIndex2 == 1) // to feet
                    answer = inputNum * 5280;
                else if (unitIndex2 == 2) // to yards
                    answer = inputNum * 1760;
                else if (unitIndex2 == 3) // to miles
                    answer = inputNum;
                else if (unitIndex2 == 4) // to millimeters
                    answer = inputNum * 1609344.002;
                else if (unitIndex2 == 5) // to centimeters
                    answer = inputNum * 160934;
                else if (unitIndex2 == 6) // to meters
                    answer = inputNum * 1609.34;
                else if (unitIndex2 == 7) // to kilometers
                    answer = inputNum * 1.60934;
            }
            else if (unitIndex1 == 4) // from millimeters
            {
                if (unitIndex2 == 0) // to inches
                    answer = inputNum;
                else if (unitIndex2 == 1) // to feet
                    answer = inputNum;
                else if (unitIndex2 == 2) // to yards
                    answer = inputNum;
                else if (unitIndex2 == 3) // to miles
                    answer = inputNum;
                else if (unitIndex2 == 4) // to millimeters
                    answer = inputNum;
                else if (unitIndex2 == 5) // to centimeters
                    answer = inputNum;
                else if (unitIndex2 == 6) // to meters
                    answer = inputNum;
                else if (unitIndex2 == 7) // to kilometers
                    answer = inputNum;
            }
            else if (unitIndex1 == 5) // from centimeters
            {
                if (unitIndex2 == 0) // to inches
                    answer = inputNum / 2.54;
                else if (unitIndex2 == 1) // to feet
                    answer = inputNum / 30.48;
                else if (unitIndex2 == 2) // to yards
                    answer = inputNum / 91.44;
                else if (unitIndex2 == 3) // to miles
                    answer = inputNum / 160934;
                else if (unitIndex2 == 4) // to millimeters
                    answer = inputNum * 10;
                else if (unitIndex2 == 5) // to centimeters
                    answer = inputNum;
                else if (unitIndex2 == 6) // to meters
                    answer = inputNum * 0.01;
                else if (unitIndex2 == 7) // to kilometers
                    answer = inputNum * 0.00001;
            }
            else if (unitIndex1 == 6) // from meters
            {
                if (unitIndex2 == 0) // to inches
                    answer = inputNum * 39.3701;
                else if (unitIndex2 == 1) // to feet
                    answer = inputNum * 3.28084;
                else if (unitIndex2 == 2) // to yards
                    answer = inputNum * 1.09361;
                else if (unitIndex2 == 3) // to miles
                    answer = inputNum / 1609.34;
                else if (unitIndex2 == 4) // to millimeters
                    answer = inputNum * 1000;
                else if (unitIndex2 == 5) // to centimeters
                    answer = inputNum * 100;
                else if (unitIndex2 == 6) // to meters
                    answer = inputNum;
                else if (unitIndex2 == 7) // to kilometers
                    answer = inputNum * 0.001;
            }
            else if (unitIndex1 == 7) // from kilometers
            {
                if (unitIndex2 == 0) // to inches
                    answer = inputNum * 39370.1;
                else if (unitIndex2 == 1) // to feet
                    answer = inputNum * 3280.84;
                else if (unitIndex2 == 2) // to yards
                    answer = inputNum * 1093.61;
                else if (unitIndex2 == 3) // to miles
                    answer = inputNum * 0.621371;
                else if (unitIndex2 == 4) // to millimeters
                    answer = inputNum * 1000000;
                else if (unitIndex2 == 5) // to centimeters
                    answer = inputNum * 100000;
                else if (unitIndex2 == 6) // to meters
                    answer = inputNum * 1000;
                else if (unitIndex2 == 7) // to kilometers
                    answer = inputNum;
            }

            return (answer);
        }
    }
}
