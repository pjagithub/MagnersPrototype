using Magners.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magners.Services
{
    public interface IEFRepository
    {
        Image GetImage(string tags);
        IQueryable<Tag> GetAllTags();
    }
}
