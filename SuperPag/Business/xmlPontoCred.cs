using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using SuperPag.Business.Messages;

namespace SuperPag.Business
{
    public class xmlPontoCred
    {
        /// <summary>
        /// isere o xml.
        /// </summary>
        /// <param name="ObjMxmlPontoCred"></param>
        public static void InsertXmlPontoCred(MxmlPontoCred ObjMxmlPontoCred)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_InsertXmlPontoCred");

            db.AddInParameter(dbCommand, "@Id", DbType.Guid, ObjMxmlPontoCred.Id);
            db.AddInParameter(dbCommand, "@xml", DbType.String, ObjMxmlPontoCred.Xml);
            db.AddInParameter(dbCommand, "@Userid", DbType.Guid, ObjMxmlPontoCred.Userid);
            db.AddInParameter(dbCommand, "@Data", DbType.DateTime, ObjMxmlPontoCred.Data);

            db.ExecuteNonQuery(dbCommand);
        }

        public static void UpdateXmlPontoCred(Guid Id)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_UpdateXmlPontoCred");

            db.AddInParameter(dbCommand, "@Id", DbType.Guid, Id);

            db.ExecuteNonQuery(dbCommand);
        }
    }
}
