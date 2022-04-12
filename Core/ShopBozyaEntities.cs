using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public partial class ShopBozyaEntities
    {
        private static ShopBozyaEntities _context;

        public static ShopBozyaEntities GetContext()
        {
            if (_context == null)
                _context = new ShopBozyaEntities();

            return _context;
        }
    }
}
