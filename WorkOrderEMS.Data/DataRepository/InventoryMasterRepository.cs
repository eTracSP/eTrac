using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Data
{
    public class InventoryMasterRepository : BaseRepository<InventoryMaster>
    {
        workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();
        public List<InventoryMasterModelList> GetAllInventory(int? ProjectId, string OperationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, int? InventoryType, int? ItemOwnership, ObjectParameter paramTotalRecords)
        {
            List<InventoryMasterModelList> lstInventory = new List<InventoryMasterModelList>();
            try
            {

                //lstInventory = _workorderEMSEntities.SP_GetAllInventory(ProjectId, OperationName, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, InventoryType, ItemOwnership,paramTotalRecords).Select(i => new InventoryMasterModelList()
                //{
                //    InventoryID = i.InventoryID,
                //    ItemName = i.ItemName,
                //    ItemCode = i.ItemCode,
                //    ItemType = i.ItemType,
                //    ItemTypeName = i.ItemTypeName,
                //    Description = i.Description,
                //    Quantity = i.Quantity,
                //    LocationId = i.LocationId,
                //    AssignInventoryID = Convert.ToInt32(i.AssignInventoryID),
                //    AssignedUserID = Convert.ToInt32(i.AssignedUserID),
                //    IssueDate = Convert.ToDateTime(i.IssueDate),
                //    IssuedBy = Convert.ToInt32(i.IssuedBy),
                //    AssignedToName = i.AssignedToName,
                //    ReturnDate = i.ReturnDate
                //}).ToList();
                string result = _workorderEMSEntities.SP_GetAllInventory(ProjectId, OperationName, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, InventoryType, ItemOwnership, paramTotalRecords).FirstOrDefault();
                if (result != null)
                {
                    XDocument doc = XDocument.Parse(result);
                    string root = doc.Root.Name.LocalName;
                    if (root == "Inventory")
                    {
                        var xRow = doc.Root.Elements("row");
                        lstInventory = (from item in xRow
                                        select new InventoryMasterModelList()
                                        {
                                            InventoryID = (int)item.Element("InventoryID"),
                                            ItemName = (string)item.Element("ItemName"),
                                            ItemCode = (string)item.Element("ItemCode"),
                                            ItemType = (int)item.Element("ItemType"),
                                            ItemTypeName = (string)item.Element("ItemTypeName"),
                                            Description = (string)item.Element("Description"),
                                            Quantity = (long)item.Element("Quantity"),
                                            ItemOwnership = (int)item.Element("ItemOwnership"),
                                            LocationId = (int)item.Element("LocationId")
                                        }).ToList();
                    }
                    else
                    {
                        var xRow = doc.Root.Elements("row");

                        InventoryMasterModelList objD = null;
                        foreach (var item in xRow)
                        {
                            objD = new InventoryMasterModelList();
                            objD.InventoryID = item.Element("InventoryID") == null ? 0 : (int)item.Element("InventoryID");
                            objD.ItemName = (string)item.Element("ItemName");
                            objD.ItemCode = (string)item.Element("ItemCode");
                            objD.ItemType = item.Element("ItemType") == null ? 0 : (int)item.Element("ItemType");
                            objD.ItemTypeName = (string)item.Element("ItemTypeName");
                            objD.Description = (string)item.Element("Description");
                            objD.AssginedQuantity = item.Element("AssginedQuantity") == null ? 0 : (long)item.Element("AssginedQuantity");
                            objD.ItemOwnership = item.Element("ItemOwnership") == null ? 0 : (int)item.Element("ItemOwnership");
                            objD.LocationId = (int)item.Element("LocationId");
                            objD.AssignInventoryID = item.Element("AssignInventoryID") == null ? 0 : (int)item.Element("AssignInventoryID");
                            objD.AssignedUserID = item.Element("AssignedUserID") == null ? 0 : (int)item.Element("AssignedUserID");
                            objD.IssueDate = (DateTime)item.Element("IssueDate");
                            objD.IssuedBy = item.Element("IssuedBy") == null ? 0 : (int)item.Element("IssuedBy");
                            objD.AssignedToName = (string)item.Element("AssignedToName");
                            lstInventory.Add(objD);
                        }

                        //lstInventory = (from item in xRow
                        //                select new InventoryMasterModelList()
                        //                {
                        //                    InventoryID = (int)item.Element("InventoryID"),
                        //                    ItemName = (string)item.Element("ItemName"),
                        //                    ItemCode = (string)item.Element("ItemCode"),
                        //                    ItemType = (int)item.Element("ItemType"),
                        //                    ItemTypeName = (string)item.Element("ItemTypeName"),
                        //                    Description = (string)item.Element("Description"),
                        //                    AssginedQuantity = (int)item.Element("AssginedQuantity"),
                        //                    ItemOwnership = (int)item.Element("ItemOwnership"),
                        //                    LocationId = (int)item.Element("LocationId"),
                        //                    AssignInventoryID = (int)item.Element("AssignInventoryID"),
                        //                    AssignedUserID = (int)item.Element("AssignedUserID"),
                        //                    IssueDate = (DateTime)item.Element("IssueDate"),
                        //                    IssuedBy = (int)item.Element("IssuedBy"),
                        //                    AssignedToName = (string)item.Element("AssignedToName"),
                        //                }).ToList();
                    }
                }
                return lstInventory;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedDate>02/09/2015</CreatedDate>
        /// <Description>Get All Created Inventory list by LocationID</Description>
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public List<InventoryMasterModelList> GetAllInventoryForPDF(int? ProjectId, int? InventoryType, int? OwnershipType)
        {
            var lstInventory = new List<InventoryMasterModelList>();
            try
            {
                lstInventory = (from i in _workorderEMSEntities.InventoryMasters
                                join g in _workorderEMSEntities.GlobalCodes on i.ItemType equals g.GlobalCodeId
                                where i.LocationId == ProjectId
                                select new InventoryMasterModelList { InventoryID = i.InventoryID, ItemName = i.ItemName, ItemCode = i.ItemCode, CodeName = g.CodeName, Description = i.Description, Quantity = i.Quantity, CreatedOn = i.CreatedOn.ToClientTimeZoneinDateTime()}).ToList();

                return lstInventory;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
