using MiniFrameWork.Camadas;
using MiniFrameWork.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesafioTecnicoProsoft.Camadas.Entidades;
///using DesafioTecnicoProsoft.Camadas.Entidades.Auxiliares;
using DesafioTecnicoProsoft.Camadas.Dados;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;

namespace DesafioTecnicoProsoft.Camadas.Negocio
{

    public class NDev : NBase<Dev, DDev>
    {
        public NDev() : base() { }
        public NDev(IDatabaseMF _databasemf) : base(_databasemf) { }


        public override long? Insert(Dev entidade)
        {
            try
            {
                entidade.createdAt = DateTime.Now;
                var retornoapi = ConectApi("", "POST", entidade);
                entidade = retornoapi[0];
                    return base.Insert(entidade);
               

                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public override long? Update(Dev entidade)
        {
            try
            {
                var retornoapi =  ConectApi(entidade.id, "PUT", entidade);
                
                    entidade = retornoapi[0];
                    return base.Update(entidade);
                


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

           
        }
        public void PopulaBase()
        {


            try { 
                DDev _objeto = new DDev();
                _objeto.databasemf = this.databasemf;
                QueryMF queryMf = _objeto.getQueryMF<Dev>("FILTRO", (IEntityBase)new Dev());
                List<Dev> retorno = new List<Dev>();
                retorno = _objeto.GetListaByFilter<Dev>(queryMf);
                if (retorno.Count <= 0)
                {



                    var retornoapi = ConectApi("", "GET", new Dev());
                    

                        foreach (Dev item in retornoapi)
                        {


                            if (!item.email.Contains("@prosoft.com.br"))
                            {


                                string[] subs = item.email.Split("@");
                                subs[1] = "prosoft.com.br";
                                item.email = subs[1] + subs[2];
                            }
                         queryMf = _objeto.getQueryMF<Dev>("insert", item);
                        _objeto.Insert<Dev>(queryMf);


                        }



                    
                }
            }catch (Exception e)
            {
                throw new Exception(e.Message);
            }




        }


        private List<Dev> ConectApi(string querystring, string metodo, Dev entidade)
        {
            using (var cliente = new HttpClient())
            {
                string uribase = "https://61a170e06c3b400017e69d00.mockapi.io/DevTest/Dev";
                cliente.BaseAddress = new Uri(uribase) ;
                cliente.DefaultRequestHeaders.Accept.Clear();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // HttpResponseMessage response = new HttpResponseMessage();
                List<Dev> lista = new List<Dev>();


                if (String.Equals(metodo, "GET"))
                {
                    cliente.BaseAddress = new Uri(uribase);
                    using (var response = cliente.GetAsync(querystring).Result)
                    {


                        if (response.IsSuccessStatusCode)
                        {

                           string retorno =  response.Content.ReadAsStringAsync().Result;
                            lista = JsonConvert.DeserializeObject<List<Dev>>(retorno);

                           
                            
                        }

                    }
                }
                else if (String.Equals(metodo, "POST"))
                {
                    cliente.BaseAddress = new Uri(uribase);
                    using (var response = cliente.PostAsJsonAsync(querystring, entidade).Result)
                    {

                        if (response.IsSuccessStatusCode)
                        {

                             string retorno =  response.Content.ReadAsStringAsync().Result;
                            
                              lista.Add(JsonConvert.DeserializeObject<Dev>(retorno));

                        }
                    }

                }
                else if (String.Equals(metodo, "PUT"))
                {
                    cliente.BaseAddress = new Uri(uribase + "/"+ entidade.id);
                    using (var response = cliente.PutAsJsonAsync<Dev>(querystring, entidade).Result)
                    {

                        if (response.IsSuccessStatusCode)
                        {
                            string retorno =  response.Content.ReadAsStringAsync().Result;
                            lista.Add(JsonConvert.DeserializeObject<Dev>(retorno));



                        }
                    }
                }
                return lista;

            }


        }




    }
}
