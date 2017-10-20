using System;
using System.Data;
using System.Data.CData.DynamicsCRM;
namespace CData.DynamicsCRM.demo
{
  public class Connection
  {
    public static DynamicsCRMConnection conn;
    public Connection ()
    {
    }

    public static void TestConnection(string connectionString) {
      DynamicsCRMConnection conn = new DynamicsCRMConnection (connectionString);
      conn.Open ();
      conn.TestConnection ();
    }

    public static void SetupConnection(string connectionString) {
      if (conn != null) conn = null;
      conn = new DynamicsCRMConnection (connectionString);
      conn.Open ();
    }

    public static string[] ListTablesViews() {
      if (!CheckConn ()) return null;
      DataTable table = new DataTable ();
      table = conn.GetSchema ("TABLES", null); 
      DataTable view = new DataTable ();
      view = conn.GetSchema ("VIEW", null); 
      string[] items = new string[table.Rows.Count + view.Rows.Count];
      for (int i = 0; i < table.Rows.Count; i++) {
        items[i] = table.Rows[i]["TABLE_NAME"].ToString();
      }
      for (int i = 0; i < view.Rows.Count; i++) {
        items[i + table.Rows.Count] = view.Rows[i]["TABLE_NAME"].ToString();
      }
      return items;
    }

    public static DataTable Query(string statement) {
      DataTable table = null;
      if (CheckConn ()) {
        table = new DataTable ();
        table.TableName = "XAMARIN Default Table Name";
        DynamicsCRMDataAdapter adapter = new DynamicsCRMDataAdapter (statement, conn);
        adapter.Fill (table);
      }
      return table;
    }

    public static int Execute(string statement) {
      if (CheckConn ()) {
        DynamicsCRMCommand command = conn.CreateCommand ();
        command.CommandText = statement;
        return command.ExecuteNonQuery ();
      }
      return 0;
    }

    public static bool CheckConn() {
      if (conn == null) return false;
      return true;
    }
  }
}