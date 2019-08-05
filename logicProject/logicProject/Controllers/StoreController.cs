﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace logicProject.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store
        [Filter.StoreAuthorize]
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}