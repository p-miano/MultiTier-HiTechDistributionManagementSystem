using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hi_TechLibrary.DAL.EntityFramework;

namespace Hi_TechLibrary.BLL.EntityFramework
{
    public class PublisherController
    {
        private readonly PublisherRepository publisherRepository;

        public PublisherController()
        {
            publisherRepository = new PublisherRepository();
        }

        public IEnumerable<Publishers> GetAllPublishers() => publisherRepository.GetAllPublishers();
        public int AddPublisher(Publishers publisher)
        {
            publisherRepository.AddPublisher(publisher);
            return publisher.PublisherID;
        }
        public Publishers GetPublisherById(int id) => publisherRepository.GetPublisherById(id);
        public Publishers GetPublisherByName(string name) => publisherRepository.GetPublishByName(name);
        public bool Exists(string publisherName)
        {
            return publisherRepository.GetAllPublishers().Any(p => p.Name.Equals(publisherName, StringComparison.OrdinalIgnoreCase));
        }

    }
}
