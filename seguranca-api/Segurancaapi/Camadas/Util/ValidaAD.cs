using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;

namespace SGR.Infra.Util
{
    public static class ValidaAD
    {

        static public bool IsAuthenticated(string Dominio, string username, string password)
        {
            bool retorno = false;
            try
            {
                //string _path = ConfigurationSettings.AppSettings["dominioAD"];
                PrincipalContext pricontext = new PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain, Dominio);


                //ldap://cn=users,dc=adn,dc=com,dc=br 

                //string container = "CN=servfile01, OU=domain controllers, DC=adn, DC=com, DC=br.";
                //  PrincipalContext lp = new PrincipalContext(
                //                        ContextType.ApplicationDirectory,
                //                        "LDAP://adn/CN=Computers,DC=adn,DC=com,DC=br");



                //PrincipalContext lp = new PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain, Dominio);
                retorno = pricontext.ValidateCredentials(username, password);
            }
            catch (System.Exception e)
            {
                return false;
            }

            return retorno;
        }

        static public bool IsAuthenticated(string Dominio, string username)
        {
            try
            {
                //PrincipalContext lp = new PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain, Dominio);
                //UserPrincipal luser = UserPrincipal.FindByIdentity(lp, username);
                //if (luser == null)
                //    return false;
            }

            catch (System.Exception)
            {
                return false;
            }

            return true;
        }


    }
}
