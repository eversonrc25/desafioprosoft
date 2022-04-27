using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MiniFrameWork.Dados;
 
using Microsoft.AspNetCore.Authorization;
using MiniFrameWork.Camadas;
using System.Security.Claims;

namespace WebApiFrameWork.util
{
    
    [Authorize("Bearer")]
    public class ApiBaseController: Controller


    {


        protected IDatabaseMF banco =  new MSSQLDatabaseFactory();
        protected  IConfiguration _configuration;
        protected System.Security.Claims.ClaimsPrincipal userlogado = null;

        public ApiBaseController(IConfiguration configuration)
        {
            _configuration = configuration;
          //  banco.CreateDatabaseDynamic(_configuration.GetConnectionString("bancosSQL"), "System.Data.SqlClient");
        }

         public void setBancoUsuario( System.Security.Claims.ClaimsPrincipal userlogado ) {
            this.userlogado = userlogado;
            //String bancoStr = userlogado.Claims.Where( x=>x.Type.Equals("EMPRESA")).Select( x=> x.Value).First<String>();
            //  banco.CreateDatabaseDynamic( String.Format(_configuration.GetConnectionString("bancosSQL"), bancoStr), "System.Data.SqlClient");
           banco.CreateDatabaseDynamic(_configuration.GetConnectionString("bancosSql"), "System.Data.SqlClient");
        }

        public void setBancoUsuario( string bancoStr ) {
            banco.CreateDatabaseDynamic( String.Format(_configuration.GetConnectionString("bancosSql"), bancoStr), "System.Data.SqlClient");

        }

   


 

        protected override void Dispose(bool disposing)
        {
            if (banco.BancoDados != null)
            {
                banco.BancoDados.Close();
            }
            base.Dispose(disposing);
        }


        protected bool isTransacao = false;

        virtual protected void setBanco()
        {
            banco.CreateDatabase("SISTE", "Oracle.DataAccess.Client");
        }


        virtual protected void setBanco(String strconn)
        {
            banco.CreateDatabaseDynamic(strconn, "Oracle.DataAccess.Client");
        }


        public void CloseDatabase()
        {
            if (banco != null)
            {
                banco.CloseDatabase();
                banco = null;
            }

        }





    }
}
