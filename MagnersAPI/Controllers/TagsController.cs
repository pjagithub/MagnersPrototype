using Magners.Models;
using Magners.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Magners.Controllers
{
    public class TagsController : ApiController
    {
        private IEFRepository repository = new EFRepository();

        /// <summary>
        /// Return all Tags in JSON format
        /// // GET: /Api/Tags
        /// </summary>
        /// <returns></returns>
        public IQueryable<Tag> GetAllTags()
        {
            return repository.GetAllTags();
        }
    }
}
