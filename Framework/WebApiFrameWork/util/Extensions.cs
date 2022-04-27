
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace WebApiFrameWork.util
{


    public static class Extensions
    {

        public static IDictionary<string, string> ToDictionary(this NameValueCollection collection)
        {

            //collection.AllKeys

            return collection.Cast<string>().ToDictionary(k => k, v => collection[v]);
        }

    }

}