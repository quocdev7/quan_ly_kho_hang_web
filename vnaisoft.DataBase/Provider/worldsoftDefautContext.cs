using Microsoft.EntityFrameworkCore;
using System;using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vnaisoft.DataBase.Function;
using vnaisoft.DataBase.System;

namespace vnaisoft.DataBase.Provider
{
    public partial class vnaisoftDefautContext : DbContext
    {
        public vnaisoftDefautContext(DbContextOptions<vnaisoftDefautContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //systemTableBuilder(modelBuilder);
            //db_syncTableBuilder(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
          
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        [DbFunction("fn_remove_unicode")]
        public  string Fn_remove_unicode(string strInput)
        {
            throw new NotImplementedException();
        }

        [DbFunction("Fn_remove_unicode", "dbo")]
        public  string FN_ENCRYPT(string strInput)
        {
            throw new NotImplementedException();
        }
        [DbFunction("Fn_status_finish", "dbo")]
        public int Fn_status_finish(string id)
        {
            throw new NotImplementedException();
        }

        [DbFunction("Fn_approval_last_date_action", "dbo")]
        public DateTime Fn_approval_last_date_action (string id)
        {
            throw new NotImplementedException();
        }

        [DbFunction("Fn_check_finish_approval", "dbo")]
        public bool Fn_check_finish_approval(string id)
        {
            throw new NotImplementedException();
        }
        [DbFunction("Fn_check_finish_approval_to_next_step", "dbo")]
        public bool Fn_check_finish_approval_to_next_step(string id)
        {
            throw new NotImplementedException();
        }

        [DbFunction("Fn_check_valid_approval", "dbo")]
        public bool Fn_check_valid_approval(string id, int? status_del)
        {
            throw new NotImplementedException();
        }
        


        //public List<Fn_get_sys_approval> Fn_get_sys_approval(string id_approval)
        //{
        //    // Initialization.  
        //    // Processing.  
        //      string sqlQuery = "select * from  [dbo].[Fn_get_sys_approval] ('" + id_approval + "')";

        //     var lst =  this.Query<Fn_get_sys_approval>().FromSqlRaw(sqlQuery).ToList();
            
        //    // Info.  
        //    return lst;
        //}
    }
}
