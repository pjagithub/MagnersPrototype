using Magners.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Magners.Services
{
    public class EFRepository : IEFRepository
    {
        MagnersContentEntities context = new MagnersContentEntities();

        /// <summary>
        /// Retrieves the name of the image by one or more input tags.
        /// If more than one image is returned from the database, it defaults to the first
        /// in the list.
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public Image GetImage(string tags)
        {            
            Image image = null;

            // create the sql parameter for all the request tags
            String tagsClause = GetTagsSQL(tags);

            // get an image from the database
            DataRow dr = GetImagesFromDB(tagsClause);
            
            // convert a DataTable row to an Image object and return
            string imageURL = ConfigurationManager.AppSettings["ImageURL"];           
            if (dr != null)
            {
                image = new Image
                    {
                        Id = dr.Field<int>("Id"),
                        FileName = imageURL + dr.Field<string>("FileName"),
                        Tags = dr.Field<string>("Tags")
                    };
            }

            if (image == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return image;
        }

        /// <summary>
        /// Take a comma-delimited tag list, convert to an array, and construct a string that 
        /// conforms to a SQL CONTAINS clause
        /// "SPRING,DAY" -> "'SPRING' and 'DAY'"
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        private String GetTagsSQL(string tags)
        {
            String tagsClause = "'";
            String[] requestTagsArray = tags.Split(',');
            int i = 0;
            foreach (var tagQuery in requestTagsArray)
            {
                i++;
                if (i > 1)
                {
                    tagsClause = tagsClause + " and '" + tagQuery.Trim() + "'";
                }
                else
                {
                    tagsClause = tagsClause + "'" + tagQuery.Trim() + "'";
                }
            }
            return tagsClause;
        }

        /// <summary>
        /// Execute SQL based on tags and return an image filename from the database
        /// </summary>
        /// <param name="tagsParam"></param>
        /// <returns></returns>
        private DataRow GetImagesFromDB(string tagsParam)
        {
            DataTable dt = new DataTable();

            string connString = ConfigurationManager.ConnectionStrings["MagnersImages"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string qry = "";
                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    qry = "select * from Images where contains(Tags, @tags)";
                    cmd.CommandText = qry;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@tags", tagsParam);

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    conn.Open();
                    adp.Fill(dt);
                }
            }

            // if more than one image is returned, return random one in the list to the client
            if (dt.Rows.Count > 1)
            {
                Random r = new Random();
                int i = r.Next(dt.Rows.Count);
                return dt.Rows[i];
            }
            else
            {
                return dt.Rows[0];
            }
        }

        public IQueryable<Tag> GetAllTags()
        {
            return context.Tags;
        }

        // -pja
        // Linq will not perform a sql 'contains' clause equivalent correctly.
        // Therefore tag options do not work so the sql option (above) has been implemented.

        //var image = images.FirstOrDefault();
        //var q = images.AsQueryable();

        //String[] requestTagsArray = requestTags.Split(','); //['SPRING','DAY]

        //foreach (var tagQuery in requestTagsArray)
        //{
        //    q = q.Where(i => i.Tags.Contains(tagQuery));
        //    image = q.FirstOrDefault();
        //}
        //if (image == null)
        //{
        //    throw new HttpResponseException(HttpStatusCode.NotFound);
        //}
        //return image;
    }            
}