using Magners.Models;
using Magners.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Magners.Controllers
{
    public class ImageController : ApiController
    {
        private IEFRepository repository = new EFRepository();

        /// <summary>
        /// Return an Image object in JSON format
        /// // GET: /Api/Image/tags=TAGS,TAGS
        /// </summary>
        /// <param name="requestTags"></param>
        /// <returns></returns>
        public Image GetImage(string tags)
        {
            return repository.GetImage(tags);
        }
    }
}
