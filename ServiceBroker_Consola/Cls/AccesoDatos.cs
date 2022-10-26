using System.Data;
using System.Data.SqlClient;

namespace ServiceBroker_Consola.Cls
{
  internal static class AccesoDatos
  {
    // Cadena de conexión a la BBDD SQL
    public static string cadenaConexion = @"Data Source=TOSBLAMA-L5\SQLSERVER;Initial Catalog=EPV;Integrated Security=true;";

    /// <summary>
    /// Ejecuta una consulta SQL en la base de datos, y devuelve los resultados obtenidos en un objeto DataTable.
    /// </summary>
    /// <param name="SQL"></param>
    /// <param name="parametros">Array de string con formato: nombre:valor</param>
    /// <returns>Devuelve un objeto DataTable con los resultados obtenidos tras ejecución de la consulta.</returns>
    public static DataTable GetDataTable(string SQL, string[] parametros)
    {
      try
      {
        SqlConnection conexion = new SqlConnection(cadenaConexion);
        SqlCommand comando = new SqlCommand(SQL, conexion);
        for (int i = 0; i < parametros.Length; i++)
          comando.Parameters.Add(new SqlParameter(parametros[i].Split(':')[0], parametros[i].Split(':')[1]));
        SqlDataAdapter da = new SqlDataAdapter(comando);
        DataSet ds = new DataSet();
        da.Fill(ds);
        conexion.Close();
        da.Dispose();
        conexion.Dispose();
        return ds.Tables[0];
      }
      catch (Exception)
      {
        throw;
      }
    }


    /// <summary>
    /// Ejecuta una consulta SQL en la base de datos, y devuelve los resultados obtenidos en un objeto DataTable.
    /// A diferencia de GetDataTable, este método es vulnerable a la inyección de dependencias, por lo que se recomienda usar sólo para procesos temporales internos.
    /// </summary>
    /// <param name="SQL"></param>
    /// <returns>Devuelve un DataTable con los resultados de la ejecución de la consulta.</returns>
    public static DataTable GetTmpDataTable(string SQL)
    {
      try
      {
        SqlConnection conexion = new SqlConnection(cadenaConexion);
        SqlDataAdapter comando = new SqlDataAdapter(SQL, conexion);
        DataSet ds = new DataSet();
        comando.Fill(ds);
        conexion.Close();
        comando.Dispose(); conexion.Dispose();
        return ds.Tables[0];
      }
      catch (Exception)
      {
        throw;
      }
    }



  }
}
