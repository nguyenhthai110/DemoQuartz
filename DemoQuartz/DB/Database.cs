using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoQuartz.DB
{
    public class Database : IDatabase
    {
        private readonly ILogger<Database> _logger;
        private readonly DbOptions _db;

        public Database(ILogger<Database> logger, DbOptions dbOptions)
        {
            _logger = logger;
            _db = dbOptions;
        }

        public List<string> LayDanhSachXn(List<string> DanhSachDaCo, int year, int xnCodeMin, int xnCodeMax, string systemWeb)
        {
            List<string> listReturn = new List<string>();
            DateTime date = new DateTime(year, 1, 1);

            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(_db.conn);
            try
            {
                string store = "Company_GetDownload";

                //store = String.Format("Company_GetDownload{0}", Config.IndexListXN);

                cmd = new SqlCommand(store, conn);
                cmd.Parameters.Add(new SqlParameter("@StartLogfileTime", date));
                cmd.Parameters.Add(new SqlParameter("@XNCodeMin", xnCodeMin));
                cmd.Parameters.Add(new SqlParameter("@XNCodeMax", xnCodeMax));

                SqlParameter param = new SqlParameter("@SystemWeb", systemWeb);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.String;
                cmd.Parameters.Add(param);

                cmd.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = cmd;
                da.Fill(dt);
                string TmpXN = string.Empty;
                int SoXN = 0;
                foreach (DataRow row in dt.Rows)
                {
                    TmpXN = row[0].ToString();
                    if (string.IsNullOrEmpty(TmpXN) == false && TmpXN.Contains(";") == false)
                    {
                        if (int.TryParse(TmpXN, out SoXN))
                        {
                            if (DanhSachDaCo.Contains(SoXN.ToString()) == false)
                                listReturn.Add(SoXN.ToString());
                        }
                    }
                }
            }
            catch
            {
                listReturn = new List<string>();
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }

            return listReturn;
        }
    }
}
