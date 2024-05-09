using System;

using System.Collections.Generic;
using System.Data.SQLite;



namespace JDBCUtilsModule {
    public static class DBUtils
    {
        private static SQLiteConnection instance = null; 
		
       public  static SQLiteConnection getConnection(IDictionary<string,string> props)
        {
            if (instance == null || instance.State == System.Data.ConnectionState.Closed)
            {
                instance = getNewConnection(props);

            }
            return instance;
           
        }

        private static SQLiteConnection getNewConnection(IDictionary<string,string> props)
        {
            instance = new SQLiteConnection(props["ConnectionString"]);
         
          // Open the connection:
          try
          {
              instance.Open();
          }
          catch (Exception ex)
          {
              // DB Exception message : {{ {ex.Message}}}";
          }
          return instance;
        }
    }
}