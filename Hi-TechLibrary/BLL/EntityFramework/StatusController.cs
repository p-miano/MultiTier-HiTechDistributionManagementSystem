using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hi_TechLibrary.DAL.EntityFramework;

namespace Hi_TechLibrary.BLL.EntityFramework
{
    public class StatusController
    {
        private readonly StatusRepository statusRepository = new StatusRepository();

        public IEnumerable<Status> GetAllStatuses()
        {
            return statusRepository.GetAllStatuses();
        }

        public void AddStatus(Status status)
        {
            statusRepository.AddStatus(status);
        }

        public Status GetStatusById(int id)
        {
            return statusRepository.GetStatusById(id);
        }
    }
}
