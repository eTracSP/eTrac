using System;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public class AddressManager : IAddressManager
    {
        AddressMasterRepository objAddressMasterRepository = new AddressMasterRepository();
        AddressMaster _AddressMaster = null;

        /// <summary>SaveAdress
        /// 
        /// </summary>
        /// <param name="objAddressModel"></param>
        public void SaveAddress(AddressModel objAddressModel)
        {
            try
            {
                if (objAddressModel.StateId == 0 || objAddressModel.CountryId == 0) { throw new Exception("Country / State cannot be blank"); }
                if (objAddressModel.AddressMasterId == 0)
                {
                    objAddressModel.CreatedBy = objAddressModel.CreatedBy;
                    objAddressModel.CreatedDate = DateTime.UtcNow;
                    objAddressModel.IsDeleted = false;
                    _AddressMaster = new AddressMaster();
                    AutoMapper.Mapper.CreateMap<AddressModel, AddressMaster>();
                    _AddressMaster = AutoMapper.Mapper.Map(objAddressModel, _AddressMaster);
                    objAddressMasterRepository.Add(_AddressMaster);
                }
                else
                {
                    var data = objAddressMasterRepository.GetSingleOrDefault(l => l.AddressMasterId == objAddressModel.AddressMasterId);
                    if (data != null)
                    {
                        data.Address1 = objAddressModel.Address1;
                        data.Address2 = objAddressModel.Address2;
                        data.CountryId = objAddressModel.CountryId;
                        data.StateId = objAddressModel.StateId;
                        data.City = objAddressModel.City;
                        data.ZipCode = objAddressModel.ZipCode;
                        data.Phone = objAddressModel.Phone;
                        data.Mobile = objAddressModel.Mobile;
                        data.ModifiedBy = 1;
                        data.ModifiedDate = DateTime.UtcNow;
                        data.IsDeleted = false;
                        objAddressMasterRepository.SaveChanges();
                    }
                }
            }
            catch (Exception)
            { throw; }
        }
    }
}
