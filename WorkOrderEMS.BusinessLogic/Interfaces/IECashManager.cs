using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ECash_M;
namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface IECashManager
    {
        //List<System.Web.Mvc.SelectListItem> vendorList();commented by bhushan dod on 07/08/2015 for vendor list according to location
        List<System.Web.Mvc.SelectListItem> vendorList(long LocationId);
        VendorMaster_M getVendorDetails(long vendorId);
        string getVendorName(int vendorId);
        //List<System.Web.Mvc.SelectListItem> getVehicleListForDropDown(int vendorId);


        /// <summary>
        /// Get all vehicle registered under location .
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="LocationID"></param>
        /// <returns></returns>
        List<System.Web.Mvc.SelectListItem> getVehicleListForDropDown(long vendorId, long LocationID);

        VehicleModel getVehicleDetail(long vehicleId);

        object getTransactionDetails(string TransactionName, long LocaitonId);



        /// <summary>
        /// Created by Vijay Sahu
        /// Not in use.
        /// </summary>
        /// <param name="objPara"></param>
        /// <param name="objMain"></param>
        /// <param name="TransactionName"></param>
        /// <returns></returns>
        int ECashPaymentSave(WorkOrderEMS.Models.ECash_M.ECashPayment obj);

        /// <summary>
        /// Created by Vijay Sahu
        /// This function is used for saving all payment transaction record.
        /// </summary>
        /// <param name="objPara"></param>
        /// <param name="objMain"></param>
        /// <param name="TransactionName"></param>
        /// <returns></returns>
        WorkOrderEMS.Helper.Result SaveECashTransaction(List<TransactionSave_M> objPara, SpecialNotes_M objMain, string TransactionName, long LocaitonId, long? VehicleId);

        /// <summary>
        /// Created by Vijay Sahu
        /// This function is used for getting all driver details..
        /// </summary>
        /// <param name="objPara"></param>
        /// <param name="objMain"></param>
        /// <param name="TransactionName"></param>
        /// <returns></returns>
        WorkOrderEMS.Models.UserModels.DriverModel_M getDriverDetails(long driverId);



        /// <summary>
        /// created by vijay sahu on 28 march 2015
        /// get list of transaction caetegory for binding dropdown list in ECash Renwal page.
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> GetTransactionCategory();

        /// <summary>
        /// Created by vijay sahu on 28 march 2015
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Tuple<int, string> ECashRenewal(string obj);

        /// <summary>
        /// created by vijay sahu  on 31 march 2015
        /// Description:- This procedure is used for getting the number of all transaction list that had been assigned to location. 
        /// </summary>
        /// <returns></returns>
        List<WorkOrderEMS.Models.ECash_M.GetAllAssignedTransaction_M> getAllAssignedTransaction();

        /// <summary>
        /// Created by vijay sahu on 31 march 2015
        /// This function is used for updating the transaction amount based on Location
        ///  </summary>
        /// <param name="CategoryId"></param>
        /// <param name="Amount"></param>
        Tuple<byte> updateEcashAmount(long CategoryId, decimal Amount);


        /// <summary>
        /// Created by vijay sahu on 31 march 2015
        /// This function is used for getting all assigned category to given locaitonId only.
        /// </summary>
        List<WorkOrderEMS.Models.ECash_M.GetAllAssignedTransaction_M> getECashCategoryForLocationId(long LocationId);

        /// <summary>
        /// Deleted assigned trasaction mapping from table.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        byte deleteTransactionById(int id);

        dynamic getTransactionDetailsBasedOnVehicleType(string TransactionName, long? vehicleType);

    }
}
