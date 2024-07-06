using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;

namespace apartmentManager
{
     class koneksyon
    {
        SqlConnection conn;

        public SqlConnection getCon() {
            
            conn = new SqlConnection("Data Source=LAPTOP-EK7LC1R2;Initial Catalog=apartmentmanagement;Integrated Security=True"); 
            return conn;
        }
    }
}
