using System.Collections.Generic;

namespace Domain.Constants
{
    public static class BusinessConstants
    {
        public static List<ISOClass> AtRestISOClassTypes = new List<ISOClass>(){

            new ISOClass(){
                ClassName="ISO Class-5",
                PointMicron="3520",
                OneMicron="832",
                FiveMicron="20"
            },
             new ISOClass(){
                ClassName="ISO Class-6",
                PointMicron="35200",
                OneMicron="8320",
                FiveMicron="293"
            },
             new ISOClass(){
                ClassName="ISO Class-7",
                PointMicron="352000",
                OneMicron="83200",
                FiveMicron="2900"
            },
             new ISOClass(){
                ClassName="ISO Class-8",
                PointMicron="3520000",
                OneMicron="832000",
                FiveMicron="29000"
            },
             new ISOClass(){
                ClassName="ISO Class-9",
                PointMicron="35200000",
                OneMicron="8320000",
                FiveMicron="290000"
            }
        };
        public static List<ISOClass> InOperationISOClassTypes = new List<ISOClass>(){
               new ISOClass(){
                ClassName="ISO Class-5",
                PointMicron="3520",
                OneMicron="832",
                FiveMicron="20"
            },
             new ISOClass(){
                ClassName="ISO Class-6",
                PointMicron="35200",
                OneMicron="8320",
                FiveMicron="293"
            },
             new ISOClass(){
                ClassName="ISO Class-7",
                PointMicron="352000",
                OneMicron="83200",
                FiveMicron="2900"
            },
             new ISOClass(){
                ClassName="ISO Class-8",
                PointMicron="Not Defined",
                OneMicron="Not Defined",
                FiveMicron="Not Defined"
            },
             new ISOClass(){
                ClassName="ISO Class-9",
                PointMicron="Not Defined",
                OneMicron="Not Defined",
                FiveMicron="Not Defined"
            }
        };
    }

    public class ISOClass
    {
        public string ClassName { get; set; }
        public string OneMicron { get; set; }
        public string PointMicron { get; set; }
        public string FiveMicron { get; set; }
    }
}
