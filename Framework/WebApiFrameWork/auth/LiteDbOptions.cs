
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiFrameWork.auth
{
    public class LiteDbOptions
    {
        public string DatabaseLocation { get; set; }
        public string SecretKey_Recaptcha { get; set; }
    }
}