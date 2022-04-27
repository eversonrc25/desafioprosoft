using System;
using System.Collections.Generic;
using LiteDB;


namespace WebApiFrameWork.auth
{
    public class UserAuth
    {
        
        public string id { get;set; }
        public string id_usuario { get; set; }
        public DateTime? dtstart { get; set; }    
        public DateTime? created { get;set; }
        public DateTime? exp { get;set; }
        public String accessToken { get;set; }
        public String nome { get;set; }
        public String apelido { get;set; }
        public String email { get;set; }
        public String[] roles { get; set; }
        public String[] routes { get; set; }
    }

}

                    