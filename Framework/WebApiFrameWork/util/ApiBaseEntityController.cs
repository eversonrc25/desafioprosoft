using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Cors;
using MiniFrameWork.Camadas;
using MiniFrameWork.Util;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;


using MiniFrameWork.Dados;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.Collections.Specialized;
using System.ComponentModel;
//using SISTE.Util;
//using SISTE.Web.Models;


namespace WebApiFrameWork.util
{


    //[EnableCors(headers: "*", methods: "*")]
    public abstract class ApiBASEEntityController<E, N, D> : ApiBaseController
        where E : EntityBase, new()
        where D : DBase<E>, new()
        where N : NBase<E, D>, new()
    {


        N _nNegocio = new N();
        protected string id = "";


        virtual protected E OnTrataModelAfterRoute(string router, E model, IDictionary<String, String> dictionary)
        {

            return model;
        }
        public ApiBASEEntityController(IConfiguration configuration) : base(configuration)
        {
            _nNegocio.databasemf = this.banco;
        }

        #region
        virtual protected string trataMensagemError(Exception ex, string Tag)
        {
            return ex.Message;
        }
        #endregion


        virtual public IActionResult Inserir(dynamic modelParams)
        {
            E model = null;
            try
            {

                model = JsonConvert.DeserializeObject<E>(JsonConvert.SerializeObject(modelParams, Formatting.Indented));
                _nNegocio.Insert(model);
                return Ok(new RetornoSingleREST<E>() { error = 0, mensagem = "MENSAGEM.SUCESSO", dados = model });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ORA-00001: restrição exclusiva"))
                {

                    return BadRequest(new RetornoSingleREST<E>() { error = 1, mensagem = this.trataMensagemError(ex, string.Empty), dados = null });

                }
                else
                {
                    return BadRequest(new RetornoSingleREST<E>() { error = 1, mensagem = ex.Message, dados = null });
                }

            }
        }

        virtual public IActionResult Alterar(int id, dynamic modelParams)
        {
            E model = null;
            try
            {
                model = JsonConvert.DeserializeObject<E>(JsonConvert.SerializeObject(modelParams, Formatting.Indented));
                Reflector.SetProperty(model, this.id, id);
                _nNegocio.Update(model);

                return Ok(new RetornoSingleREST<E>() { error = 0, mensagem = "MENSAGEM.SUCESSO", dados = model });

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ORA-00001: restrição exclusiva"))
                {
                    return BadRequest(new RetornoSingleREST<E>() { error = 1, mensagem = this.trataMensagemError(ex, string.Empty), dados = null });

                }
                else
                {
                    return BadRequest(new RetornoSingleREST<E>() { error = 1, mensagem = ex.Message, dados = null });
                }

            }
        }

        virtual public IActionResult GetFiltro(string modelParams)
        {
            try
            {
                IDictionary<string, string> dictionaryRequest = HttpUtility.ParseQueryString(modelParams).ToDictionary();

                E model = JsonConvert.DeserializeObject<E>(JsonConvert.SerializeObject(dictionaryRequest, Formatting.Indented));
                model = OnTrataModelAfterRoute("GETFILTRO", model, dictionaryRequest);

                return Ok(new RetornoREST<E>() { error = 0, mensagem = "MENSAGEM.SUCESSO", dados = _nNegocio.GetListaByFilter(model) });

            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoSingleREST<E>() { error = 1, mensagem = this.trataMensagemError(ex, string.Empty), dados = null });
            }
        }

        virtual public IActionResult Get(int id)
        {
            try
            {
                E model = new E();
                Reflector.SetProperty(model, this.id, id);

                return Ok(new RetornoSingleREST<E>() { error = 0, mensagem = "MENSAGEM.SUCESSO", dados = _nNegocio.GetEntidadeByFilter(model) });


            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoSingleREST<E>() { error = 1, mensagem = this.trataMensagemError(ex, string.Empty), dados = null });
            }
        }

        virtual public IActionResult Delete(int id)
        {
            try
            {
                E _model = new E();
                Reflector.SetProperty(_model, this.id, id);
                // _nNegocio.Delete(_model);
                _model = _nNegocio.GetEntidadeByFilter(_model); // temporario
                return Ok(new RetornoSingleREST<E>() { error = 0, mensagem = "MENSAGEM.SUCESSO", dados = _model });

            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoSingleREST<E>() { error = 1, mensagem = ex.Message, dados = null });

            }
        }

    }
}