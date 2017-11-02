using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data
{
    public class ProjectRepository : BaseRepository<ProjectMaster>, IProjectRepository
    {
        workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();
        //Created by Gayatri Pal
        //on 26-Aug-2014
        //To get list of all Project
        public List<ProjectMasterListModel> GetAllProject(int ProjectID, string OperationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter paramTotalRecords)
        {
            try
            {
                List<ProjectMasterListModel> lstProject = _workorderEMSEntities.SP_GetAllProject(ProjectID, OperationName, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, paramTotalRecords).Select(p => new ProjectMasterListModel()
                {
                    ProjectID = p.ProjectID,
                    Location = p.LocationName,
                    LocationID = p.LocationID,
                    LocationName = p.LocationName,
                    ProjectCategory = p.ProjectCategory,
                    ProjectCategoryName = p.ProjectCategoryName,
                    Description = p.Description,
                    ProjectLogoName = p.ProjectLogoName,
                    ProjectServiceName = p.ServiceName,
                    ProjectServicesID = p.ProjectServices,
                    QRCID = p.QRCID

                }).ToList();

                return lstProject;
            }
            catch (Exception)
            {
                throw ;
            }
        }
    }
}
