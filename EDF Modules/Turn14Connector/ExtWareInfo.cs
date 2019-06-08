using System.Collections.Generic;
using System.Linq;
using Turn14Connector.DataItems;
using Turn14Connector.Helpers;
using WheelsScraper;

namespace Turn14Connector
{
    public class ExtWareInfo : WareInfo
    {

        #region Constructors

        public ExtWareInfo() { }

        public ExtWareInfo(string title, string description, string partNumber, string manufacturerPartNumber, string brand,
            string mainCategory, string subCategory, string upc, string length, string height, string width, string weight,
            int stock, int manufactorerStock, double msrp, double jobber, double webPrice, double costPrice, string generalImages,
            string attachments, string fitments, string sceProdId, string sceProductType, string scePartNumber)
        {
            Title = title;
            Description = description;
            Brand = brand;
            PartNumber = !string.IsNullOrEmpty(partNumber) ? $"@{partNumber}" : string.Empty;
            ManufacturerNumber = !string.IsNullOrEmpty(manufacturerPartNumber) ? $"@{manufacturerPartNumber}" : string.Empty;
            MainCategory = mainCategory;
            SubCategory = subCategory;
            Upc = !string.IsNullOrEmpty(upc) ? $"@{upc}" : string.Empty;
            Length = length;
            Weight = weight;
            Height = height;
            Width = width;
            Stock = stock;
            ManufacturerStock = manufactorerStock;
            Msrp = msrp;
            WebPrice = webPrice;
            CostPrice = costPrice;
            Jober = jobber;
            GeneralImages = generalImages;
            Attachments = attachments;
            Fitments = !string.IsNullOrEmpty(fitments) ? $"@{fitments}" : string.Empty;

            ProdId = sceProdId;
            ProductType = sceProductType;
            ScePartNumber = !string.IsNullOrEmpty(scePartNumber) ? $"@{scePartNumber}" : string.Empty;
        }

        #endregion

        #region Properties

        public string Title { get; set; }
        public string MainCategory { get; set; }
        public string SubCategory { get; set; }
        public string Upc { get; set; }
        public double Msrp { get; set; }
        public double WebPrice { get; set; }
        public double CostPrice { get; set; }
        public double Jober { get; set; }
        public string Length { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Weight { get; set; }
        public string GeneralImages { get; set; }
        public string Attachments { get; set; }
        public string Fitments { get; set; }
        public int Stock { get; set; }
        public int ManufacturerStock { get; set; }

        public string ProdId { get; set; }
        public string ProductType { get; set; }
        public string ScePartNumber { get; set; }

        public string Make { get; set; }
        public string Model { get; set; }
        public string Submodel { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string Engine { get; set; }
        public int YearId { get; set; }
        public int VehicleId { get; set; }

        #endregion

        #region Grouping Without submodels

        //public static List<ExtWareInfo> GroupWares(List<ExtWareInfo> waresForGroup)
        //{
        //    List<ExtWareInfo> groupedWares = new List<ExtWareInfo>();

        //    var newWares = waresForGroup.GroupBy(x => new
        //    {
        //        ((ExtWareInfo)x).PartNumber,
        //        ((ExtWareInfo)x).Brand,
        //        ((ExtWareInfo)x).Make,
        //        ((ExtWareInfo)x).Model,
        //        ((ExtWareInfo)x).Submodel,
        //        ((ExtWareInfo)x).Engine
        //    }).Select(g => CreateCopyWare(g.First())).ToList();

        //    string filePath = FileHelper.CreateScrapeFile(FileHelper.GetSettingsPath("Turn14ScrapeInfoWitout.csv"), newWares);

        //    #region Old

        //    //var newWares = waresForGroup.GroupBy(x => new
        //    //{
        //    //    ((ExtWareInfo)x).PartNumber,
        //    //    ((ExtWareInfo)x).Brand,
        //    //    ((ExtWareInfo)x).Make,
        //    //    ((ExtWareInfo)x).Model,
        //    //    ((ExtWareInfo)x).Submodel,
        //    //    ((ExtWareInfo)x).Engine
        //    //}).Select(g => new ExtWareInfo
        //    //{
        //    //    Title = g.First().Title,
        //    //    Description = g.First().Description,
        //    //    Brand = g.First().Brand,
        //    //    PartNumber = g.First().PartNumber,
        //    //    ManufacturerNumber = g.First().ManufacturerNumber,
        //    //    MainCategory = g.First().MainCategory,
        //    //    SubCategory = g.First().SubCategory,
        //    //    Upc = g.First().Upc,
        //    //    Length = g.First().Length,
        //    //    Weight = g.First().Weight,
        //    //    Height = g.First().Height,
        //    //    Width = g.First().Width,
        //    //    Stock = g.First().Stock,
        //    //    ManufacturerStock = g.First().ManufacturerStock,
        //    //    Msrp = g.First().Msrp,
        //    //    WebPrice = g.First().WebPrice,
        //    //    CostPrice = g.First().CostPrice,
        //    //    Jober = g.First().Jober,
        //    //    GeneralImages = g.First().GeneralImages,
        //    //    Attachments = g.First().Attachments,
        //    //    Fitments = g.First().Fitments,
        //    //    ProdId = g.First().ProdId,
        //    //    ProductType = g.First().ProductType,
        //    //    ScePartNumber = g.First().ScePartNumber,

        //    //    Make = g.First().Make,
        //    //    Model = g.First().Model,
        //    //    Submodel = g.First().Submodel,
        //    //    Engine = g.First().Engine,
        //    //    YearId = g.First().YearId,
        //    //}).ToList();

        //    #endregion


        //    waresForGroup = waresForGroup.OrderBy(i => i.YearId).ToList();

        //    foreach (ExtWareInfo newWare in newWares)
        //    {
        //        var firstOldWare = waresForGroup.First(i => i.Make == newWare.Make && i.Model == newWare.Model && i.Engine == newWare.Engine);
        //        var lastOldWare = waresForGroup.Last(i => i.Make == newWare.Make && i.Model == newWare.Model && i.Engine == newWare.Engine);

        //        newWare.StartYear = firstOldWare.YearId;
        //        newWare.EndYear = lastOldWare.YearId;

        //        groupedWares.Add(CreateCopyWare(newWare));
        //    }

        //    return groupedWares;
        //}


        #endregion

        public static List<ExtWareInfo> GroupWares(List<ExtWareInfo> waresForGroup)
        {
            //FileHelper.CreateScrapeFile(FileHelper.GetSettingsPath("Turn14ScrapeHondaBeforeGroup.csv"), waresForGroup);

            waresForGroup = waresForGroup.OrderBy(i => i.YearId).ToList();

            //FileHelper.CreateScrapeFile(FileHelper.GetSettingsPath("Turn14ScrapeHondaAfterSort.csv"), waresForGroup);

            var withoutYearsGroup = waresForGroup.GroupBy(x => new
            {
                ((ExtWareInfo) x).PartNumber,
                ((ExtWareInfo) x).Brand,
                ((ExtWareInfo) x).Make,
                ((ExtWareInfo) x).Model,
                ((ExtWareInfo) x).Submodel,
                ((ExtWareInfo) x).Engine
            });
            
            List<ExtWareInfo> yearsGroup = new List<ExtWareInfo>();
            foreach (var groupInfo in withoutYearsGroup)
            {
                var orderedGroupInfo = groupInfo.OrderBy(i => i.YearId);
                //TODO: check if we have some skippedYearInfo

                ExtWareInfo orderedGroupInfoFirstWare = orderedGroupInfo.First();

                var firstOldWare = orderedGroupInfo.First(i => i.Make == orderedGroupInfoFirstWare.Make && i.Model == orderedGroupInfoFirstWare.Model && i.Engine == orderedGroupInfoFirstWare.Engine && i.Submodel == orderedGroupInfoFirstWare.Submodel);
                var lastOldWare = orderedGroupInfo.Last(i => i.Make == orderedGroupInfoFirstWare.Make && i.Model == orderedGroupInfoFirstWare.Model && i.Engine == orderedGroupInfoFirstWare.Engine && i.Submodel == orderedGroupInfoFirstWare.Submodel);

                orderedGroupInfoFirstWare.StartYear = firstOldWare.YearId;
                orderedGroupInfoFirstWare.EndYear = lastOldWare.YearId;

                yearsGroup.Add(orderedGroupInfoFirstWare);
            }

            //FileHelper.CreateScrapeFile(FileHelper.GetSettingsPath("Turn14ScrapeHondaYearsGroup.csv"), yearsGroup);

            var withoutSubmodelGroup = yearsGroup.GroupBy(x => new
            {
                ((ExtWareInfo) x).Make,
                ((ExtWareInfo) x).Model,
                ((ExtWareInfo) x).Engine,
                ((ExtWareInfo) x).StartYear,
                ((ExtWareInfo) x).EndYear
            });

            List<ExtWareInfo> submodelsGroup = new List<ExtWareInfo>();

            foreach (var groupInfo in withoutSubmodelGroup)
            {
                ExtWareInfo firstWare = groupInfo.First();
                string submodels = string.Empty;

                foreach (ExtWareInfo extWareInfo in groupInfo )
                {
                    submodels += extWareInfo.Submodel + ",";
                }

                submodels = submodels.TrimEnd(',');

                firstWare.Submodel = submodels;

                submodelsGroup.Add(firstWare);
            }

            //FileHelper.CreateScrapeFile(FileHelper.GetSettingsPath("Turn14ScrapeHondaSubmodelsGroup.csv"), submodelsGroup);

            return submodelsGroup;
        }

        public static List<ExtWareInfo> ProcessingFitments(ExtWareInfo ware, List<VehicleInfoForFitments> fitments)
        {
            List<ExtWareInfo> fitmentsWares = new List<ExtWareInfo>();

            if (!string.IsNullOrEmpty(ware.Fitments))
            {
                var trimedWare = ware.Fitments.TrimStart('@');
                string[] fitmentArray = trimedWare.Split(',');

                foreach (string vehicleId in fitmentArray )
                {
                    foreach (VehicleInfoForFitments fitmentInfo in fitments )
                    {
                        if (vehicleId == fitmentInfo.VehicleId)
                        {                     
                            ExtWareInfo copiedWare = CreateCopyWare(ware);

                            copiedWare.Make = fitmentInfo.MakeName;
                            copiedWare.Model = fitmentInfo.ModelName;
                            copiedWare.Submodel = fitmentInfo.SubModelName;
                            copiedWare.Engine = fitmentInfo.Engine;

                            int.TryParse(fitmentInfo.YearId, out int yearId);
                            copiedWare.YearId = yearId;

                            int.TryParse(vehicleId, out int intVehicleId);
                            copiedWare.VehicleId = intVehicleId;

                            fitmentsWares.Add(copiedWare);
                        }
                    }

                }
            }
            else
                fitmentsWares.Add(ware);

            return fitmentsWares;
        }

        public static ExtWareInfo CreateCopyWare(ExtWareInfo ware)
        {
            ExtWareInfo copiedWare = new ExtWareInfo();

            copiedWare.Title = ware.Title;
            copiedWare.Description = ware.Description;
            copiedWare.Brand = ware.Brand;
            copiedWare.PartNumber = ware.PartNumber;
            copiedWare.ManufacturerNumber = ware.ManufacturerNumber;
            copiedWare.MainCategory = ware.MainCategory;
            copiedWare.SubCategory = ware.SubCategory;
            copiedWare.Upc = ware.Upc;
            copiedWare.Length = ware.Length;
            copiedWare.Weight = ware.Weight;
            copiedWare.Height = ware.Height;
            copiedWare.Width = ware.Width;
            copiedWare.Stock = ware.Stock;
            copiedWare.ManufacturerStock = ware.ManufacturerStock;
            copiedWare.Msrp = ware.Msrp;
            copiedWare.WebPrice = ware.WebPrice;
            copiedWare.CostPrice = ware.CostPrice;
            copiedWare.Jober = ware.Jober;
            copiedWare.GeneralImages = ware.GeneralImages;
            copiedWare.Attachments = ware.Attachments;
            copiedWare.Fitments = ware.Fitments;

            copiedWare.ProdId = ware.ProdId;
            copiedWare.ProductType = ware.ProductType;
            copiedWare.ScePartNumber = ware.ScePartNumber;

            copiedWare.Make = ware.Make;
            copiedWare.Model = ware.Model;
            copiedWare.Submodel = ware.Submodel;
            copiedWare.Engine = ware.Engine;
            copiedWare.StartYear = ware.StartYear;
            copiedWare.EndYear = ware.EndYear;
            copiedWare.VehicleId = ware.VehicleId;
            copiedWare.YearId = ware.YearId;

            return copiedWare;
        }
    }
}
