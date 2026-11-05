using Microsoft.Data.SqlClient;

namespace cursor_demo_api.Data;

public interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
}
