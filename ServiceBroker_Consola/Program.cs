// Ejemplo que muestra como usa ServiceBroker de SQL Server.
// Puede utilizarse en un servicio para disparar procesos automáticos (por ejemplo crear una cola de tareas que genere PDFs, envie email, etc)
// También se puede usar para lanzar notificaciones por WebSocket (basado en tablas)


// Usamos nuestra clase de ServiceBroker
using ServiceBroker_Consola.Cls;

// Cadena de conexión (recomendable sacar de fichero de configuración)
AccesoDatos.cadenaConexion = @"Data Source=TOSBLAMA-L5\SQLSERVER;Initial Catalog=EPV;Integrated Security=true;MultipleActiveResultSets=true;";

// Instanciamos e inicializamos nuestra clase (El Id de usuario fijo es sólo a efectos informativos, en aplicación real sacarlo del usuario logado)
var sb = new ServiceBrokerSQL(AccesoDatos.cadenaConexion, "SELECT IdAlerta FROM [dbo].[Alertas] WHERE IdUsuario=1", "ESCUCHAR_NUMERO_ALERTAS");
sb.OnMensajeRecibido += new ServiceBrokerSQL.MensajeRecibido(sbColaTareas_InformacionRecibida);
sb.IniciarEscucha();

Console.Clear();
Console.WriteLine("Esperando instrucciones");
Thread.Sleep(Timeout.Infinite);
sb.DetenerEscucha();


// Controlador del evento MensajeRecibido de ServiceBrokerSQL. El parámetro nombreMensaje se usa a modo de demostración de como devolver parámetros en el evento.
void sbColaTareas_InformacionRecibida(object sender, string nombreMensaje)
{
  // Procesamos el mensaje recibido
  System.Data.DataTable dt = AccesoDatos.GetTmpDataTable("SELECT COUNT(IdAlerta) as NumeroAlertas FROM dbo.Alertas WHERE IdUsuario=1");
  int numeroAlertas = Convert.ToInt32(dt.Rows[0]["NumeroAlertas"]);

  Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} Ha llegado un cambio en {nombreMensaje}, ahora tienes {numeroAlertas} alertas");

  // Volvemos a inicializar la escucha
  sb.IniciarEscucha();
}


