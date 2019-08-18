using logicProject.Models.DBContext;
using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace logicProject.Models.DAO
{
    public class CollectionPointDAO
    {
        private LogicEntities db;

        public CollectionPoint GetCollectionPoint(int id)
        {
            using (db = new LogicEntities())
            {
                return db.CollectionPoint.Find(id);
            }
        }
    }
}