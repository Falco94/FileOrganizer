using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.IService
{
    public interface IProvideExtensions
    {
        IEnumerable<Models.Extension> GetExtensions();
    }
}
