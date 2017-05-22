
using Rewarding.Models;
using System.Collections.Generic;
using System.Linq;

namespace Rewarding.Models
{
    public class TempChange
    {
        public List<TempData> changesCollection = new List<TempData>();
        
        public void AddItem(IEntity entity, string method)
        {
            changesCollection.Add(new TempData
                                {
                                    Entity = entity,
                                    Method = method
                                });
        }

        //public void Clear()
        //{
        //    changesCollection.Remove();
        //}

        public IEnumerable<TempData> Lines
        {
            get { return changesCollection; }
        }
    }

    public class TempData
    {
        public IEntity Entity { get; set; }
        public string Method { get; set; }
    }

}