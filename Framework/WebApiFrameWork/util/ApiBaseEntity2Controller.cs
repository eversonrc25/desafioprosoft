using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MiniFrameWork.Camadas;
using MiniFrameWork.Dados;
using MiniFrameWork.Util;
using Newtonsoft.Json;
using System.Data;
using ClosedXML.Excel;
using System.Security.Claims;

namespace WebApiFrameWork.util
{

    [EnableCors("MyPolicy")]
    [Authorize("Bearer")]
    public abstract class ApiBASEEntity2Controller<E, N, D> : ApiBaseController
    where E : EntityBase, new()
    where D : DBase<E>, new()
    where N : NBase<E, D>, new()
    {

        protected const string MENSAGEM_PADRAO_SUCESSO = "Operação efetuda com Sucesso !";
        protected const string MENSAGEM_PADRAO_POST = "Registro Cadastrado com Sucesso !";
        protected const string MENSAGEM_PADRAO_PUT = "Registro Alterado com Sucesso !";
        protected const string MENSAGEM_PADRAO_DELETE = "Registro Ativado/Inativado com Sucesso !";

        public N _nNegocio = new N();
        protected string nameId = "";
        protected string usuarioFieldEdicao = "usua_nr_edicao";
         protected string usuarioFieldCreate = "usua_nr_cadastro";


      virtual protected   string getIdUser<T>(  ) {
          string idUsuario = string.Empty;
          try {
            var identity = userlogado.Identity  as ClaimsIdentity;
            idUsuario = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
          } catch (Exception ex ) {}
            
          return idUsuario;
        }


        virtual protected E addPaginacao(E model, IDictionary<String, String> dictionary)
        {
            return this.addPaginacao<E>(model, dictionary);
        }

        virtual protected M addPaginacao<M>(M model, IDictionary<String, String> dictionary) where M : EntityBase, new()
        {
            if (model.CamposAuxiliares == null)
                model.CamposAuxiliares = new Dictionary<string, object>();

            model.CamposAuxiliares.Add("PAG_C", Convert.ToInt64(dictionary["PAG_C"]));
            model.CamposAuxiliares.Add("QTD_I", Convert.ToInt64(dictionary["QTD_I"]));

            return model;
        }

        virtual protected E OnTrataModelAfterRoute(string router, E model, IDictionary<String, String> dictionary)
        {

            return this.OnTrataModelAfterRoute<E>(router, model, dictionary);
        }

        virtual protected M OnTrataModelAfterRoute<M>(string router, M model, IDictionary<String, String> dictionary) where M : EntityBase, new()
        {

            return model;
        }

        public ApiBASEEntity2Controller(IConfiguration configuration) : base(configuration)
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
                this.setBancoUsuario(User);
                model = JsonConvert.DeserializeObject<E>(JsonConvert.SerializeObject(modelParams, Formatting.Indented));
                Int64? id = _nNegocio.Insert(model);
                Reflector.SetProperty(model, this.nameId, (Int32?)id);

                return Ok(new RetornoSingleREST<E>() { error = 0, mensagem = MENSAGEM_PADRAO_POST, dados = model });
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



        virtual public IActionResult Alterar(string id, dynamic modelParams)
        {
            E model = null;
            try
            {
                this.setBancoUsuario(User);
                model = JsonConvert.DeserializeObject<E>(JsonConvert.SerializeObject(modelParams, Formatting.Indented));
                Reflector.SetProperty(model, this.nameId, id);
                _nNegocio.Update(model);

                return Ok(new RetornoSingleREST<E>() { error = 0, mensagem = MENSAGEM_PADRAO_PUT, dados = model });

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

        virtual public async Task<IActionResult> AlterarAsync(string id, dynamic modelParams)
        {
            E model = null;
            try
            {
                this.setBancoUsuario(User);
                model = JsonConvert.DeserializeObject<E>(JsonConvert.SerializeObject(modelParams, Formatting.Indented));
                Reflector.SetProperty(model, this.nameId, id);
                Reflector.SetProperty(model, this.usuarioFieldEdicao, this.getIdUser<String>());

                await Task.Run(() =>
                {
                    _nNegocio.Update(model);
                });

                return Ok(new RetornoSingleREST<E>() { error = 0, mensagem = MENSAGEM_PADRAO_PUT, dados = model });

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
                this.setBancoUsuario(User);
                IDictionary<string, string> dictionaryRequest = HttpUtility.ParseQueryString(modelParams).ToDictionary();

                E model = JsonConvert.DeserializeObject<E>(JsonConvert.SerializeObject(dictionaryRequest, Formatting.Indented));
                model = OnTrataModelAfterRoute("GETFILTRO", model, dictionaryRequest);

                List<E> retornoJson = null;
                retornoJson = _nNegocio.GetListaByFilter(model);

                return Ok(new RetornoREST<E>() { error = 0, mensagem = "MENSAGEM.SUCESSO", dados = retornoJson });

            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoSingleREST<E>() { error = 1, mensagem = this.trataMensagemError(ex, string.Empty), dados = null });
            }
        }

        virtual public async Task<IActionResult> GetFiltroAsync(string modelParams)
        {
            try
            {
                this.setBancoUsuario(User);
                IDictionary<string, string> dictionaryRequest = HttpUtility.ParseQueryString(modelParams).ToDictionary();

                E model = JsonConvert.DeserializeObject<E>(JsonConvert.SerializeObject(dictionaryRequest, Formatting.Indented));
                model = OnTrataModelAfterRoute("GETFILTROASYNC", model, dictionaryRequest);
                // model = addPaginacao(model, dictionaryRequest);
                List<E> retornoJson = null;
                Int64 total_registro = 0;
                await Task.Run(() =>
                {
                    retornoJson = _nNegocio.GetListaByFilter(model);
                    /*   if (retornoJson.Count > 0)
                       {
                           total_registro = Convert.ToInt64(retornoJson[0].CamposAuxiliares["qtdtotal"]);
                       }*/
                });

                return Ok(new RetornoREST<E>() { error = 0, mensagem = MENSAGEM_PADRAO_SUCESSO, dados = retornoJson, total_registro = total_registro });

            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoSingleREST<E>() { error = 1, mensagem = this.trataMensagemError(ex, string.Empty), dados = null });
            }
        }


        virtual public IActionResult Get(string id)
        {
            try
            {
                this.setBancoUsuario(User);
                E model = new E();
                Reflector.SetProperty(model, this.nameId, id);

                return Ok(new RetornoSingleREST<E>() { error = 0, mensagem = "MENSAGEM.SUCESSO", dados = _nNegocio.GetEntidadeByFilter(model) });

            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoSingleREST<E>() { error = 1, mensagem = this.trataMensagemError(ex, string.Empty), dados = null });
            }
        }

        virtual public IActionResult Delete(string id)
        {
            try
            {
                this.setBancoUsuario(User);
                E _model = new E();
                Reflector.SetProperty(_model, this.nameId, id);
                _model = _nNegocio.GetEntidadeByFilter(_model); // temporario
                return Ok(new RetornoSingleREST<E>() { error = 0, mensagem = MENSAGEM_PADRAO_DELETE, dados = _model });

            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoSingleREST<E>() { error = 1, mensagem = ex.Message, dados = null });

            }
        }

        virtual public IActionResult Delete(string id, int idUsuario)
        {
            try
            {
                this.setBancoUsuario(User);
                E _model = new E();
                Reflector.SetProperty(_model, this.nameId, id);
                Reflector.SetProperty(_model, this.usuarioFieldEdicao, this.getIdUser<String>());

                long? retorno = _nNegocio.Delete(_model);

                return Ok(new RetornoSingleREST<E>() { error = 0, mensagem = MENSAGEM_PADRAO_DELETE, dados = _model });

            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoSingleREST<E>() { error = 1, mensagem = ex.Message, dados = null });

            } 
        }

        virtual public async Task<IActionResult> DeleteAsync(string id, int idUsuario)
        {
            try
            {
                this.setBancoUsuario(User);
                E _model = new E();
                Reflector.SetProperty(_model, this.nameId, id);
                Reflector.SetProperty(_model, this.usuarioFieldEdicao, this.getIdUser<String>());

                long? retorno = null;
                await Task.Run(() =>
                {
                    retorno = _nNegocio.Delete(_model);
                });

                return Ok(new RetornoSingleREST<E>() { error = 0, mensagem = MENSAGEM_PADRAO_DELETE, dados = _model });

            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoSingleREST<E>() { error = 1, mensagem = ex.Message, dados = null });

            }
        }



        #region GetFiltroPagAsync
        virtual public async Task<IActionResult> GetFiltroPagAsync(string modelParams)
        {
            try
            {

                this.setBancoUsuario(User);
                IDictionary<string, string> dictionaryRequest = HttpUtility.ParseQueryString(modelParams).ToDictionary();

                E model = JsonConvert.DeserializeObject<E>(JsonConvert.SerializeObject(dictionaryRequest, Formatting.Indented));

                model = OnTrataModelAfterRoute("GETFILTROASYNC", model, dictionaryRequest);
                model = addPaginacao(model, dictionaryRequest);

                Tuple<List<E>, Int64> retorno = null;

                Int64 total_registro = 0;
                await Task.Run(() =>
                {
                    retorno = _nNegocio.GetListaByFilterWithPagination<E>(model);
                    total_registro = retorno.Item2;

                });

                return Ok(new RetornoREST<E>() { error = 0, mensagem = MENSAGEM_PADRAO_SUCESSO, dados = retorno.Item1, total_registro = total_registro });

            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoSingleREST<E>() { error = 1, mensagem = this.trataMensagemError(ex, string.Empty), dados = null });
            }
        }

        virtual public async Task<IActionResult> GetFiltroPagAsyncChild<M, W, T>(string id, string modelParams)
                                                                            where M : EntityBase, new()
                                                                            where W : DBase<M>, new()
                                                                            where T : NBase<M, W>, new()
        {

            try
            {

                this.setBancoUsuario(User);
                IDictionary<string, string> dictionaryRequest = HttpUtility.ParseQueryString(modelParams).ToDictionary();

                M model = JsonConvert.DeserializeObject<M>(JsonConvert.SerializeObject(dictionaryRequest, Formatting.Indented));
                Reflector.SetProperty(model, this.nameId, id);
                model = OnTrataModelAfterRoute<M>("GETFILTROASYNC", model, dictionaryRequest);
                model = addPaginacao<M>(model, dictionaryRequest);

                Tuple<List<M>, Int64> retorno = null;
                T _nNegocioChild = new T();
                _nNegocioChild.databasemf = this.banco;
                Int64 total_registro = 0;
                await Task.Run(() =>
                {
                    retorno = _nNegocioChild.GetListaByFilterWithPagination<M>(model);
                    total_registro = retorno.Item2;

                });

                return Ok(new RetornoREST<M>() { error = 0, mensagem = MENSAGEM_PADRAO_SUCESSO, dados = retorno.Item1, total_registro = total_registro });

            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoSingleREST<M>() { error = 1, mensagem = this.trataMensagemError(ex, string.Empty), dados = null });
            }
        }

        virtual public async Task<IActionResult> GetFiltroPagAsync<M>(string modelParams) where M : EntityBase, new()
        {
            try
            {

                this.setBancoUsuario(User);
                IDictionary<string, string> dictionaryRequest = HttpUtility.ParseQueryString(modelParams).ToDictionary();

                E model = JsonConvert.DeserializeObject<E>(JsonConvert.SerializeObject(dictionaryRequest, Formatting.Indented));

                model = OnTrataModelAfterRoute("GETFILTROASYNC", model, dictionaryRequest);
                model = addPaginacao(model, dictionaryRequest);

                Tuple<List<M>, Int64> retorno = null;

                Int64 total_registro = 0;
                await Task.Run(() =>
                {
                    retorno = _nNegocio.GetListaByFilterWithPagination<M>(model);
                    total_registro = retorno.Item2;

                });

                return Ok(new RetornoREST<M>() { error = 0, mensagem = MENSAGEM_PADRAO_SUCESSO, dados = retorno.Item1, total_registro = total_registro });

            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoSingleREST<M>() { error = 1, mensagem = this.trataMensagemError(ex, string.Empty), dados = null });
            }
        }

        #endregion

        #region GetAsync
        virtual public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                this.setBancoUsuario(User);
                E model = new E();
                Reflector.SetProperty(model, this.nameId, id);
                E retornoJson = null;
                await Task.Run(() =>
                {
                    retornoJson = _nNegocio.GetEntidadeByFilter(model);
                });

                return Ok(new RetornoSingleREST<E>() { error = 0, mensagem = MENSAGEM_PADRAO_SUCESSO, dados = retornoJson });

            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoSingleREST<E>() { error = 1, mensagem = this.trataMensagemError(ex, string.Empty), dados = null });
            }
        }

        virtual public async Task<IActionResult> GetAsync<M>(string id) where M : EntityBase
        {
            try
            {
                this.setBancoUsuario(User);
                E model = new E();
                Reflector.SetProperty(model, this.nameId, id);
                M retornoJson = null;
                await Task.Run(() =>
                {
                    retornoJson = _nNegocio.GetEntidadeByFilter<M>(model);
                });

                return Ok(new RetornoSingleREST<M>() { error = 0, mensagem = MENSAGEM_PADRAO_SUCESSO, dados = retornoJson });

            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoSingleREST<M>() { error = 1, mensagem = this.trataMensagemError(ex, string.Empty), dados = null });
            }
        }


        virtual public async Task<IActionResult> GetAsyncChild<M, W, T>(string idParent, string nameidChild, string idChild, string modelParams) where M : EntityBase, new()
                                                                                 where W : DBase<M>, new()
                                                                                 where T : NBase<M, W>, new()

        {
            try
            {
                this.setBancoUsuario(User);
                M model = new M();
                T _nNegocioChild = new T();

                _nNegocioChild.databasemf = this.banco;
                Reflector.SetProperty(model, this.nameId, idParent);
                Reflector.SetProperty(model, nameidChild, idChild);
                M retornoJson = null;
                await Task.Run(() =>
                {
                    retornoJson = _nNegocioChild.GetEntidadeByFilter<M>(model);
                });

                return Ok(new RetornoSingleREST<M>() { error = 0, mensagem = MENSAGEM_PADRAO_SUCESSO, dados = retornoJson });

            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoSingleREST<M>() { error = 1, mensagem = this.trataMensagemError(ex, string.Empty), dados = null });
            }
        }

        #endregion

        #region InserirAsync
        virtual public async Task<IActionResult> InserirAsync(dynamic modelParams)
        {

            return await this.InserirAsync<Int32>(modelParams);
        }
        virtual public async Task<IActionResult> InserirAsync<T>(dynamic modelParams)
        {
            E model = null;
            try
            {
                this.setBancoUsuario(User);
                model = JsonConvert.DeserializeObject<E>(JsonConvert.SerializeObject(modelParams, Formatting.Indented));
                Reflector.SetProperty(model, this.usuarioFieldCreate, this.getIdUser<String>());

                //T id = new T();
                await Task.Run(() =>
                {
                    var id = _nNegocio.Insert<T>(model);
                    Reflector.SetProperty(model, this.nameId, (T)id);
                });

                return Ok(new RetornoSingleREST<E>() { error = 0, mensagem = MENSAGEM_PADRAO_POST, dados = model });
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

        virtual public async Task<IActionResult> InserirAsync<T, M, W, Z>(dynamic modelParams, string nameIdChild) where M : EntityBase, new()
                                                                         where W : DBase<M>, new()
                                                                         where Z : NBase<M, W>, new()
        {
            M model = null;
            try
            {
                this.setBancoUsuario(User);
                model = JsonConvert.DeserializeObject<M>(JsonConvert.SerializeObject(modelParams, Formatting.Indented));

                Z _nNegocioChild = new Z();
                _nNegocioChild.databasemf = this.banco;
                await Task.Run(() =>
                {
                    var id = _nNegocioChild.Insert<T>(model);
                    Reflector.SetProperty(model, nameIdChild, (T)id);
                });

                return Ok(new RetornoSingleREST<M>() { error = 0, mensagem = MENSAGEM_PADRAO_POST, dados = model });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ORA-00001: restrição exclusiva"))
                {

                    return BadRequest(new RetornoSingleREST<M>() { error = 1, mensagem = this.trataMensagemError(ex, string.Empty), dados = null });

                }
                else
                {
                    return BadRequest(new RetornoSingleREST<M>() { error = 1, mensagem = ex.Message, dados = null });
                }

            }
        }

         virtual public async Task<IActionResult> InsertCrypto< M, W, Z>(dynamic modelParams, string nameIdChild) where M : EntityBase, new()
                                                                         where W : DBase<M>, new()
                                                                         where Z : NBase<M, W>, new()
        {
            M model = null;
            try
            {
                this.setBancoUsuario(User);
                model = JsonConvert.DeserializeObject<M>(JsonConvert.SerializeObject(modelParams, Formatting.Indented));
                String idUser = this.getIdUser<String>();
                Reflector.SetProperty(model, this.usuarioFieldEdicao, idUser);
                Reflector.SetProperty(model, this.usuarioFieldCreate, idUser);
                Z _nNegocioChild = new Z();
                _nNegocioChild.databasemf = this.banco;
                await Task.Run(() =>
                {

                     var id = _nNegocioChild.InsertCrypto(model);
                    Reflector.SetProperty(model, nameIdChild,  id);
                });

                return Ok(new RetornoSingleREST<M>() { error = 0, mensagem = MENSAGEM_PADRAO_POST, dados = model });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ORA-00001: restrição exclusiva"))
                {

                    return BadRequest(new RetornoSingleREST<M>() { error = 1, mensagem = this.trataMensagemError(ex, string.Empty), dados = null });

                }
                else
                {
                    return BadRequest(new RetornoSingleREST<M>() { error = 1, mensagem = ex.Message, dados = null });
                }

            }
        }


         virtual public async Task<IActionResult> InsertCrypto(dynamic modelParams)
        {
            E model = null;
            try
            {
                this.setBancoUsuario(User);
                model = JsonConvert.DeserializeObject<E>(JsonConvert.SerializeObject(modelParams, Formatting.Indented));
                String idUser = this.getIdUser<String>();
                Reflector.SetProperty(model, this.usuarioFieldEdicao, idUser);
                Reflector.SetProperty(model, this.usuarioFieldCreate, idUser);
                //T id = new T();
                await Task.Run(() =>
                {
                    var id = _nNegocio.InsertCrypto(model);
                    Reflector.SetProperty(model, this.nameId,  id);
                });

                return Ok(new RetornoSingleREST<E>() { error = 0, mensagem = MENSAGEM_PADRAO_POST, dados = model });
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

        virtual public async Task<IActionResult> AlterarAsync<M, W, Z>(string id, dynamic modelParams, string nameIdChild) where M : EntityBase, new()
                                                           where W : DBase<M>, new()
                                                           where Z : NBase<M, W>, new()
        {
            M model = null;
            try
            {
                this.setBancoUsuario(User);

                Z _nNegocioChild = new Z();
                _nNegocioChild.databasemf = this.banco;
                model = JsonConvert.DeserializeObject<M>(JsonConvert.SerializeObject(modelParams, Formatting.Indented));
                Reflector.SetProperty(model, nameIdChild, id);

                await Task.Run(() =>
                {
                    _nNegocioChild.Update(model);
                });

                return Ok(new RetornoSingleREST<M>() { error = 0, mensagem = MENSAGEM_PADRAO_PUT, dados = model });

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ORA-00001: restrição exclusiva"))
                {
                    return BadRequest(new RetornoSingleREST<M>() { error = 1, mensagem = this.trataMensagemError(ex, string.Empty), dados = null });

                }
                else
                {
                    return BadRequest(new RetornoSingleREST<M>() { error = 1, mensagem = ex.Message, dados = null });
                }

            }
        }


        virtual public async Task<IActionResult> DeleteAsync<M, W, Z>(string id, string nameIdChild) where M : EntityBase, new()
                                                                where W : DBase<M>, new()
                                                                where Z : NBase<M, W>, new()
        {
            try
            {
                this.setBancoUsuario(User);
                M _model = new M();
                Z _nNegocioChild = new Z();
                _nNegocioChild.databasemf = this.banco;
                Reflector.SetProperty(_model, nameIdChild, id);
                Reflector.SetProperty(_model, this.usuarioFieldEdicao, this.getIdUser<String>());

                long? retorno = null;
                await Task.Run(() =>
                {
                    retorno = _nNegocioChild.Delete(_model);
                });

                return Ok(new RetornoSingleREST<M>() { error = 0, mensagem = MENSAGEM_PADRAO_DELETE, dados = _model });

            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoSingleREST<M>() { error = 1, mensagem = ex.Message, dados = null });

            }
        }
        #endregion


        #region exportExcelAsync
        virtual public async Task<IActionResult> exportExcelAsync(
            string modelParams, String[] coluna, String[] titulo, Int16[] tamanho,
            string tituloRelatorio, string nome)
        {

            string caminhoArq =
           $"{nome}_{DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss")}.xlsx";
            string caminhoArqCotacoes =
           "resources/temp/" + caminhoArq;
            try
            {
                this.setBancoUsuario(User);
                IDictionary<string, string> dictionaryRequest = HttpUtility.ParseQueryString(modelParams).ToDictionary();

                E model = JsonConvert.DeserializeObject<E>(JsonConvert.SerializeObject(dictionaryRequest, Formatting.Indented));
                model = OnTrataModelAfterRoute("EXPORTEXCELASYNC", model, dictionaryRequest);

                await Task.Run(() =>
                {
                    DataTable data = _nNegocio.GetDadosDataTable(model, "FILTRO");
                    //retornoJson = _nNegocio.GetEntidadeByFilter(model);

                    System.IO.File.Copy("resources/modelo/modelorelatorio.xlsx", caminhoArqCotacoes);

                    using (var workbook = new XLWorkbook(caminhoArqCotacoes))
                    {
                        var worksheet = workbook.Worksheets.Worksheet("dados");

                        worksheet.Cell(1, 1).Value = System.DateTime.Now.ToString("dd/MM/yyyy");
                        worksheet.Cell(3, 1).Value = tituloRelatorio;

                        worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(1, titulo.Length)).Merge();
                        worksheet.Range(worksheet.Cell(2, 1), worksheet.Cell(2, titulo.Length)).Merge();
                        worksheet.Range(worksheet.Cell(3, 1), worksheet.Cell(3, titulo.Length)).Merge();
                        worksheet.Range(worksheet.Cell(5, 1), worksheet.Cell(5, titulo.Length)).Style.Fill.BackgroundColor = XLColor.Black;
                        worksheet.Range(worksheet.Cell(5, 1), worksheet.Cell(5, titulo.Length)).Style.Font.Bold = true;
                        worksheet.Range(worksheet.Cell(5, 1), worksheet.Cell(5, titulo.Length)).Style.Font.FontColor = XLColor.White;




                        int linhaTitulo = 5;
                        int colPosicao = 1;
                        foreach (String item in titulo)
                        {
                            worksheet.Column(colPosicao).Width = tamanho[colPosicao - 1];
                            worksheet.Cell(linhaTitulo, colPosicao).Value = item;
                            worksheet.Cell(linhaTitulo, colPosicao++).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);


                        }
                        int linha = 6;
                        foreach (DataRow row in data.Rows)
                        {
                            colPosicao = 1;
                            foreach (String item in coluna)
                            {
                                worksheet.Cell(linha, colPosicao).Value = row[item];


                                colPosicao++;

                            }
                            linha++;

                        }
                        workbook.Save();



                    }

                });

                var stream = System.IO.File.OpenRead(caminhoArqCotacoes);
                Response.Headers.Add("Content-Disposition", $"attachment; filename={caminhoArq}");
                return new FileStreamResult(stream, "application/xlsx");


            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoSingleREST<E>() { error = 1, mensagem = this.trataMensagemError(ex, string.Empty), dados = null });
            }
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }


        virtual public async Task<IActionResult> exportExcelDataTableAsync<M>(
           List<M> data, String[] coluna, String[] titulo, Int16[] tamanho, String[] tipo,
           string tituloRelatorio, string nome, String modelo, bool imprimeTitulo, int linhaDados) where M : EntityBase
        {

            string caminhoArq =
           $"{nome}_{DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss")}.xlsx";
            string caminhoArqCotacoes =
           "resources/temp/" + caminhoArq;
            try
            {


                await Task.Run(() =>
                {

                    System.IO.File.Copy("resources/modelo/" + modelo, caminhoArqCotacoes);

                    using (var workbook = new XLWorkbook(caminhoArqCotacoes))
                    {
                        var worksheet = workbook.Worksheets.Worksheet("dados");

                        worksheet.Cell(1, 1).Value = System.DateTime.Now.ToString("dd/MM/yyyy");
                        worksheet.Cell(1, 1).Style.Font.Bold = true;
                        worksheet.Cell(3, 1).Value = tituloRelatorio;

                        worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(1, titulo.Length)).Merge();
                        worksheet.Range(worksheet.Cell(2, 1), worksheet.Cell(2, titulo.Length)).Merge();
                        worksheet.Range(worksheet.Cell(3, 1), worksheet.Cell(3, titulo.Length)).Merge();
                        worksheet.Range(worksheet.Cell(5, 1), worksheet.Cell(5, titulo.Length)).Style.Fill.BackgroundColor = XLColor.Black;
                        worksheet.Range(worksheet.Cell(5, 1), worksheet.Cell(5, titulo.Length)).Style.Font.Bold = true;
                        worksheet.Range(worksheet.Cell(5, 1), worksheet.Cell(5, titulo.Length)).Style.Font.FontColor = XLColor.White;
                        int linhaTitulo = 5;
                        int colPosicao = 1;
                        if (imprimeTitulo)
                        {
                            foreach (String item in titulo)
                            {
                                worksheet.Column(colPosicao).Width = tamanho[colPosicao - 1];
                                worksheet.Cell(linhaTitulo, colPosicao).Value = item;
                                worksheet.Cell(linhaTitulo, colPosicao++).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            }

                        }
                        int linha = linhaDados;
                        // foreach (DataRow row in data.Rows)
                        // {
                        //     colPosicao = 1;
                        //     foreach (String item in coluna)
                        //     {
                        //         worksheet.Cell(linha, colPosicao).Value = row[item];
                        //         if ( tipo[colPosicao -1 ] == "VALOR") {
                        //            worksheet.Cell(linha, colPosicao).Style.NumberFormat.Format ="_ * # ##0.00_ ;_ * -# ##0.00_ ;_ * \"-\"??_ ;_ @_ ";
                        //         } else if ( tipo[colPosicao -1 ] == "DATA") {
                        //            worksheet.Cell(linha, colPosicao).Style.NumberFormat.Format = "dd/mm/yyyy";;
                        //         }

                        //          colPosicao++;
                        //     }
                        //     linha++;

                        // }

                        foreach (M row in data)
                        {
                            colPosicao = 1;
                            foreach (String item in coluna)
                            {
                                if (tipo[colPosicao - 1] == "TEXT")
                                {
                                    worksheet.Cell(linha, colPosicao).SetValue<string>(Convert.ToString(GetPropValue(row, item)));
                                }
                                else
                                {
                                    worksheet.Cell(linha, colPosicao).Value = GetPropValue(row, item);
                                }

                                if (tipo[colPosicao - 1] == "VALOR")
                                {
                                    worksheet.Cell(linha, colPosicao).Style.NumberFormat.Format = "_ * # ##0.00_ ;_ * -# ##0.00_ ;_ * \"-\"??_ ;_ @_ ";
                                }  
                                else if (tipo[colPosicao - 1] == "NUMERO")
                                {
                                    worksheet.Cell(linha, colPosicao).Style.NumberFormat.Format = "0"; ;
                                }
                                else if (tipo[colPosicao - 1] == "DATA")
                                {
                                    worksheet.Cell(linha, colPosicao).Style.NumberFormat.Format = "dd/mm/yyyy"; ;
                                }
                                else if (tipo[colPosicao - 1] == "TEXT")
                                {

                                    worksheet.Cell(linha, colPosicao).DataType = XLDataType.Text;
                                }




                                colPosicao++;
                            }
                            linha++;

                        }
                        workbook.Save();



                    }

                });

                // var stream = System.IO.File.OpenRead(caminhoArqCotacoes);
                // Response.Headers.Add("Content-Disposition", $"attachment; filename={caminhoArq}");
                // return new FileStreamResult(stream, "application/xlsx");

                return Ok( new { file = caminhoArq  });

            }
            catch (Exception ex)
            {
                return BadRequest(new RetornoSingleREST<E>() { error = 1, mensagem = this.trataMensagemError(ex, string.Empty), dados = null });
            }
        }
        #endregion



    }
}