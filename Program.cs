using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace XMLReadWrite
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("C:\\xml\\in\\DeformationModelPrefData.xml");
            XmlNodeList userNodes = xmlDoc.SelectNodes("//DeformationModelPrefData/DeformationModel");
            foreach (XmlNode DeformationModel in userNodes)
            {

                int defModId = int.Parse(DeformationModel.Attributes["defModId"].Value);

                foreach(XmlNode faultSectionDefModelData in DeformationModel.ChildNodes)
                {
                    int faultSectionId = int.Parse(faultSectionDefModelData.Attributes["faultSectionId"].Value);
                    decimal slipRate = 0;
                    decimal slipRateStdDev = 0;
                    decimal aseismicSlip = 0;
                    if (faultSectionDefModelData.Attributes["slipRate"].Value!= "NaN")
                        slipRate = decimal.Parse(faultSectionDefModelData.Attributes["slipRate"].Value);
                    if (faultSectionDefModelData.Attributes["slipRateStdDev"].Value != "NaN")
                        slipRateStdDev = decimal.Parse(faultSectionDefModelData.Attributes["slipRateStdDev"].Value);
                    if (faultSectionDefModelData.Attributes["aseismicSlip"].Value != "NaN")
                        aseismicSlip = decimal.Parse(faultSectionDefModelData.Attributes["aseismicSlip"].Value);
                    var defModelList = findAttempt("82", faultSectionId.ToString());

                    var x = defModelList.ToList();


                    if (x != null && x.Count > 0)
                    {

                        //faultSectionDefModelData.Attributes["faultSectionId"].Value = faultSectionId.ToString();
                        //faultSectionDefModelData.Attributes["slipRate"].Value = slipRate.ToString();
                        //faultSectionDefModelData.Attributes["slipRateStdDev"].Value = slipRateStdDev.ToString();
                        //faultSectionDefModelData.Attributes["aseismicSlip"].Value = aseismicSlip.ToString();

                        //faultSectionDefModelData.Attributes["faultSectionId"].Value = x[0];
                        faultSectionDefModelData.Attributes["slipRate"].Value = x[0].slipRate;
                        faultSectionDefModelData.Attributes["slipRateStdDev"].Value = x[0].slipRateStdDev;
                        faultSectionDefModelData.Attributes["aseismicSlip"].Value = x[0].aseismicSlip;
                    }





                    
                }



                //userNode.Attributes["defModId"].Value = (age + 1).ToString();
                

            }
            xmlDoc.Save("C:\\xml\\out\\DeformationModelPrefDataOutput.xml");
        }


        public static IEnumerable<FaultSectionDefModelData> findAttempt(string defModId, string faultSectionId)
        {
            var csvData = File.ReadAllLines("C:\\xml\\csv\\DeformationModelPrefData.csv");

            var defModelList = from defModelData in csvData
                            where defModelData.StartsWith(defModId+","+ faultSectionId)
                            let data = defModelData.Split(',')
                            select new FaultSectionDefModelData()
                            {
                                faultSectionId = data[1],
                                slipRate = data[2],
                                aseismicSlip = data[4],
                                slipRateStdDev = data[3]
                            } ;

            return defModelList;



        }


        public class FaultSectionDefModelData
        {
            public string faultSectionId { get; set; }
            public string slipRate { get; set; }
            public string aseismicSlip { get; set; }
            public string slipRateStdDev { get; set; }
    }
    }
}
