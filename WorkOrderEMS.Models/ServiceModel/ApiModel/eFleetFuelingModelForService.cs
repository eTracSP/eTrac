using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.ServiceModel
{
    public class eFleetFuelingModelForService
    {
        public string ServiceAuthKey { get; set; }
        public long UserId { get; set; }
        public long VehicleID { get; set; }
        public string VehicleNumber { get; set; }
        public string QRCodeID { get; set; }
        public long LocationID { get; set; }
        public string Mileage { get; set; }
        public string CurrentFuel { get; set; }
        public long FuelType { get; set; }
        public string ReceiptNo { get; set; }
        public System.DateTime FuelingDate { get; set; }
        public Nullable<int> TankSize { get; set; }
        public decimal Gallons { get; set; }
        public decimal PricePerGallon { get; set; }
        public decimal Total { get; set; }
        public string GasStatioName { get; set; }
        public string CardNo { get; set; }
        public string DriverName { get; set; }
        public string OtherComment { get; set; }
        public string ActivityDetails { get; set; }
        public long TaskType { get; set; }
        public string LocationName { get; set; }
    }
}
