using System.Configuration;
using System.Data;
using AutoMapper;
using Dapper;
using Monitoring.CrossCutting;
using Monitoring.CrossCutting.Configuration;
using MySql.Data.MySqlClient;

namespace Data.Repositories
{
    public class ConnectionBase
	{
        public IMapper Mapper { get; private set; }

        public ConnectionBase()
        {
            var mapperConfig = new MapperConfiguration(cfg => { });
            Mapper = mapperConfig.CreateMapper();
        }


        public IDbConnection Connection
        {
            get
            {
                return new MySqlConnection(ObterConexao());
            }
        }

        public IEnumerable<dynamic> Get(string sql, Dictionary<string, object> parameters)
        {
            return ExecuteSql(sql.ToString(), parameters);
        }

        private string ObterConexao()
        {
            return AppConfiguration.GetConfiguration(ConfigurationEnum.ConfigurationName.ConnectionStringDataBase);
        }
         
        private IEnumerable<dynamic> ExecuteSql(string sql, Dictionary<string, object> parameters )
        {
            var dynamicParameters = this.MapperToDynamicParameters(parameters);

            using (var conexao = this.Connection)
            {
                try
                {
                    conexao.Open();
                    var resultado = conexao.Query(sql.ToString(), dynamicParameters);
                    return resultado;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    conexao.Close();
                }

            }
        }

        private DynamicParameters MapperToDynamicParameters(Dictionary<string, object> parameters)
        {
            var dbArgs = new DynamicParameters();

            if (parameters != null)
            {
                foreach (var pair in parameters) dbArgs.Add(pair.Key, pair.Value);
            }

            return dbArgs;
        }
    }
}

