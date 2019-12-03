using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarksJewelersFtpData
{
    class ImageMaker
    {
        public string ImageMake(string shape)
        {
            if (shape.Equals("BR") || shape.Equals("R89") || shape.Equals("Round") || shape.Equals("OM"))
            {
                return "http://efilestorage.com/scefiles/Altaresh91_gmail.com/Diamonds/Round.jpg";
                //return "http://efilestorage.com/masterfilestorage/Images/ShapesforMJ/diamond-shapes-round.jpg";
            }
            else if (shape.Equals("CU") || shape.Equals("Cushion") || shape.Equals("CMB"))
            {
                return "http://efilestorage.com/scefiles/Altaresh91_gmail.com/Diamonds/Cushion.jpg";
                //return "http://efilestorage.com/masterfilestorage/Images/ShapesforMJ/diamond-shape-cushion.jpg";
            }
            else if (shape.Equals("EM") || shape.Equals("EME") || shape.Equals("Emerald") || shape.Equals("EC") || shape.Equals("SQEM"))
            {
                return "http://efilestorage.com/scefiles/Altaresh91_gmail.com/Diamonds/Emerald.jpg";
                //return "http://efilestorage.com/masterfilestorage/Images/ShapesforMJ/diamond-shapes-emerald.jpg";
            }
            else if (shape.Equals("OV") || shape.Equals("Oval") || shape.Equals("OB"))
            {
                return "http://efilestorage.com/scefiles/Altaresh91_gmail.com/Diamonds/Oval.jpg";
                //return "http://efilestorage.com/masterfilestorage/Images/ShapesforMJ/diamond-shapes-oval.jpg";
            }
            else if (shape.Equals("PC") || shape.Equals("Princess") || shape.Equals("PR"))
            {
                return "http://efilestorage.com/scefiles/Altaresh91_gmail.com/Diamonds/Princess.jpg";
                //return "http://efilestorage.com/masterfilestorage/Images/ShapesforMJ/diamond-shapes-princess.jpg";
            }
            else if (shape.Equals("PS") || shape.Equals("Pear") || shape.Equals("PB") || shape.Equals("PMB"))
            {
                return "http://efilestorage.com/scefiles/Altaresh91_gmail.com/Diamonds/Pear.jpg";
                //return "http://efilestorage.com/masterfilestorage/Images/ShapesforMJ/diamond-shapes-pear.jpg";

            }
            else if (shape.Equals("RAD") || shape.Equals("Radiant") || shape.Equals("RA"))
            {
                return "http://efilestorage.com/scefiles/Altaresh91_gmail.com/Diamonds/Radiant.jpg";
                //return "http://efilestorage.com/masterfilestorage/Images/ShapesforMJ/diamond-shapes-radiant.jpg";

            }
            else if (shape.Equals("AS") || shape.Equals("Asscher"))
            {
                return "http://efilestorage.com/scefiles/Altaresh91_gmail.com/Diamonds/Ascher.jpg";
                //return "http://efilestorage.com/masterfilestorage/Images/ShapesforMJ/diamond-shapes-assher.jpg";

            }
            else if (shape.Equals("HT") || shape.Equals("Heart") || shape.Equals("HS"))
            {
                return "http://efilestorage.com/scefiles/Altaresh91_gmail.com/Diamonds/Heart.jpg";
                //return "http://efilestorage.com/masterfilestorage/Images/ShapesforMJ/diamond-shapes-heart.jpg";

            }
            else if (shape.Equals("MS") || shape.Equals("Marquise") || shape.Equals("MQ"))
            {
                return "http://efilestorage.com/scefiles/Altaresh91_gmail.com/Diamonds/MQ.jpg";
                //return "http://efilestorage.com/masterfilestorage/Images/ShapesforMJ/diamond-shapes-marquise.jpg";

            }
            else return "";
        }
        public string ShapeShare(string name)
        {
            if (name.Equals("RD") || name.Equals("BR") || name.Equals("R89") || name.Equals("Round") || name.Equals("OM"))
            {
                name = "ROUND";

            }
            else if (name.Equals("CU") || name.Equals("Cushion") || name.Equals("CMB"))
            {
                name = "CUSHION";

            }
            else if (name.Equals("OV") || name.Equals("Oval") || name.Equals("OB"))
            {
                name = "OVAL";

            }
            else if (name.Equals("RAD") || name.Equals("Radiant") || name.Equals("RA"))
            {
                name = "RADIANT";

            }
            else if (name.Equals("AS") || name.Equals("Asscher"))
            {
                name = "ASSCHER";

            }
            else if (name.Equals("HT") || name.Equals("Heart") || name.Equals("HS"))
            {
                name = "HEART";
            }
            else if (name.Equals("PC") || name.Equals("Princess") || name.Equals("PR"))
            {
                name = "PRINCESS";

            }
            else if (name.Equals("EM") || name.Equals("EME") || name.Equals("Emerald") || name.Equals("EC") || name.Equals("SQEM"))
            {
                name = "EMERALD";

            }
            else if (name.Equals("PS") || name.Equals("Pear") || name.Equals("PB") || name.Equals("PMB"))
            {
                name = "PEAR";

            }
            else if (name.Equals("MS") || name.Equals("MQ") || name.Equals("MR") || name.Equals("Marquise"))
            {
                name = "MARQUISE";

            }
            else name = "";
            return name;

        }
        public string CostRange (double cost)
        {
            if (cost <2000)
            {
                return "Below $ 2000";
            }
            else if (cost >= 2000 && cost < 10000)
            {
                return "$2 000 - $10 000";
            }
            else if (cost >= 10000 && cost < 30000)
            {
                return "$10 000 - $30 000";
            }
            else if (cost >= 30000 && cost < 60000)
            {
                return "$30 000 - $60 000";
            }
            else if (cost >= 60000 && cost < 100000)
            {
                return "$60 000 - $100 000";
            }
            else if (cost >= 100000)
            {
                return "$100 000 +";
            }
            else return "";
        }
    }
}
