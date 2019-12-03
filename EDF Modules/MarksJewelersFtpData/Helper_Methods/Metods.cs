using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarksJewelersFtpData.Helper_Methods
{
    class Metods
    {
        public static string ClarityHardCode(string clarity)
        {
            if (clarity.Equals("SI3") || clarity.Equals("I2"))
            {
                return "I1";
            }

            return clarity;
        }
        public static string ColorHardCodeRARIAM(string color)
        {
            //if (color.Equals("FC"))
            //{
              //  return "M";
            //}

            switch (color)
            {
                case "E":
                    return "E";
                case "D":
                    return "D";
                case "F":
                    return "F";
                case "G":
                    return "G";
                case "H":
                    return "H";
                case "I":
                    return "I";
                case "J":
                    return "J";
                case "K":
                    return "K";
                case "L":
                    return "L";
                case "M":
                    return "M";

                default:
                    return "not much";
            }

            
        }
        public static string ColorHardCode(string color)
        {
            //if (color.Equals("FC"))
            //{
            //  return "M";
            //}

            switch (color)
            {
                case "E":
                    return "E";
                case "D":
                    return "D";
                case "F":
                    return "F";
                case "G":
                    return "G";
                case "H":
                    return "H";
                case "I":
                    return "I";
                case "J":
                    return "J";
                case "K":
                    return "K";
                case "L":
                    return "L";
                case "M":
                    return "M";

                default:
                    return "M";
            }


        }
        public static string ColorHardCodeSaharAtid(string color)
        {
            //if (color.Equals("FC"))
            //{
            //  return "M";
            //}

            switch (color)
            {
                case "E":
                    return "E Color ";
                case "D":
                    return "D Color ";
                case "F":
                    return "F Color ";
                case "G":
                    return "G Color ";
                case "H":
                    return "H Color ";
                case "I":
                    return "I Color ";
                case "J":
                    return "J Color ";
                case "K":
                    return "K Color ";
                case "L":
                    return "L Color ";
                case "M":
                    return "M Color ";

                default:
                    return "M Color ";
            }


        }
    }
}
