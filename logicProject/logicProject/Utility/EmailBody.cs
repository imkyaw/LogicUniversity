using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace logicProject.Utility
{
    public class EmailBody
    {
        public static string CollectionSubject = "A new collection point has been set.";
        public static string CollectionBody = "New collection point has been updated. Please check before you delivered to us.";
        public static string RepresentativeSubject = "You have been selected as representative.";
        public static string RepresentatvieBody = "You are selected as a representative. Please take a look at where to collect the" +
            " stationary items and check the disbursement if need.";
        public static string HeadSubject = "You have been appointed as your department head.";
        public static string HeadBody = "You have been selected as department head. Please login here: http://127.0.0.1:64451/logic/Department to manage the order requests. Your authorization will start on ";
    }
}