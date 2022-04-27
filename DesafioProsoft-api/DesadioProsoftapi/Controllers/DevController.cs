using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using DesafioTecnicoProsoft.Camadas.Dados;
using DesafioTecnicoProsoft.Camadas.Entidades;
//using DesafioTecnicoProsoft.Camadas.Entidades.Auxiliares;
using DesafioTecnicoProsoft.Camadas.Negocio;
using WebApiFrameWork.util;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;

using System.Web;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;



namespace DesafioTecnicoProsoft.Controllers
{

    public class DevController : ApiBASEEntity2Controller<Dev, NDev, DDev>
    {

        public DevController(IConfiguration configuration) : base(configuration)
        {
            this.nameId = "id";
        }

   
       

        #region MetodosMaster
        [HttpGet]
        [AllowAnonymous]
        [Route("api/dev")]
        public  Task<IActionResult> Get() => this.GetFiltroPagAsync(HttpContext.Request.QueryString.Value);

        public override async Task<IActionResult> GetFiltroPagAsync(string modelParams)
        {


            try
            {

                this.setBancoUsuario(User);
                IDictionary<string, string> dictionaryRequest = HttpUtility.ParseQueryString(modelParams).ToDictionary();
                               
                Dev model = JsonConvert.DeserializeObject<Dev>(JsonConvert.SerializeObject(dictionaryRequest, Formatting.Indented));

                model = OnTrataModelAfterRoute("GETFILTROASYNC", model, dictionaryRequest);
                model = addPaginacao(model, dictionaryRequest);

                Tuple<List<Dev>, Int64> retorno = null;

                Int64 total_registro = 0;
                await Task.Run(() =>
                {
                    _nNegocio.PopulaBase();
                    retorno = _nNegocio.GetListaByFilterWithPagination<Dev>(model);
                    total_registro = retorno.Item2;

                });

                return Ok(new RetornoREST<Dev>() { error = 0, mensagem = MENSAGEM_PADRAO_SUCESSO, dados = retorno.Item1, total_registro = total_registro });

            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoSingleREST<Dev>() { error = 1, mensagem = this.trataMensagemError(ex, string.Empty), dados = null });
            }

                      



        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/dev/{id}")]
        public Task<IActionResult> GetSingle(String id) => this.GetAsync(id);

        [HttpPost]
        [AllowAnonymous]
        [Route("api/dev")]
        public IActionResult Insert([FromBody] Dev modelParams) =>this.Inserir(modelParams);
     

        [HttpPut]
        [Route("api/dev/{id}")]
        [AllowAnonymous]
        public IActionResult Update(String id, [FromBody] Dev modelParams) => this.Alterar(id, modelParams);

        [HttpDelete]
        [Route("api/dev/{id}")]
        [AllowAnonymous]
        public Task<IActionResult> DeleteByID(string id) => this.DeleteAsync(id, 1);
        #endregion MetodosMaster
        /*
        [HttpGet]
        [AllowAnonymous]
        [Route("api/dev/populaBase")]
        public async Task<IActionResult> getPopulaBase()
        {
            try
            {
                this.setBancoUsuario(User);
                List<Dev> retorno = new List<Dev>();
                await Task.Run(() =>
                {
                   var retorno = _nNegocio.PopulaBase();

                });
                return Ok(new RetornoREST<Dev>() { error = 0, mensagem = "MENSAGEM.SUCESSO", dados = retorno });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new RetornoSingleREST<Dev>() { error = 1, mensagem = this.trataMensagemError(ex, string.Empty), dados = null });
            }
        }*/


    }
}

