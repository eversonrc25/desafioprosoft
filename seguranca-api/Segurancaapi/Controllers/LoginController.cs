using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.IdentityModel.Tokens;
using MiniFrameWork.Dados;
using Segurancaapi.Camadas.Negocio;
using Segurancaapi.Camadas.Entidades;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;
using WebApiFrameWork.util;
using WebApiFrameWork.auth;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;
using MiniFrameWork.Util;
using System.Linq;
using MongoDB.Driver;
using Segurancaapi.Util;
using Segurancaapi.Camadas.Util;

namespace Segurancaapi.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {


        private IConfiguration _configuration;
        private readonly IUserAuthService _userAuthService;

        public LoginController(IConfiguration configuration, IUserAuthService userAuthService)
        {
            _configuration = configuration;
            _userAuthService = userAuthService;
        }




        [HttpPost]
        public async Task<IActionResult> login([FromBody] Usuario model,
                                                [FromServices] SigningConfigurations signingConfigurations,
                                                [FromServices] TokenConfigurations tokenConfigurations)
        {

            DateTime dataCriacao = new DateTime();
            DateTime dataExpiracao = new DateTime();
            Usuario usuario = new Usuario();
            String jtiCodigo = Guid.NewGuid().ToString("N");
            bool credenciaisValidas = false;
            string token = string.Empty;
            MongoDbContext dbContext = new MongoDbContext();
            var builder = Builders<Usuario>.Filter;
            var filter = builder.Exists("usuario", true);

            var senha = MD5Hash.CalculaHash(model.senha);

            if (!String.IsNullOrEmpty(model.usuario))
            {
                filter = filter & builder.Eq("usuario", model.usuario);
            }
            else
            {
                throw new Exception("Usuário deve ser informado.");
            }

            //var teste = MD5Hash.CalculaHash(model.senha);


            await Task.Run(() =>
                           {
                               usuario = dbContext.Usuario.Find(filter).FirstOrDefault();

                               if (usuario.senha == MD5Hash.CalculaHash(model.senha))
                               {


                                   List<Claim> claimsRoles = new List<Claim>();
                                   claimsRoles.Add(new Claim(ClaimTypes.NameIdentifier, usuario.nome.ToString()));
                                   claimsRoles.Add(new Claim(JwtRegisteredClaimNames.Jti, jtiCodigo));
                                   claimsRoles.Add(new Claim(JwtRegisteredClaimNames.UniqueName, model.usuario));

                                   ClaimsIdentity identity = new ClaimsIdentity(
                                           new GenericIdentity(model.usuario, "Login"),
                                               claimsRoles.ToArray()

                                    );

                                   dataCriacao = DateTime.Now;
                                   dataExpiracao = DateTime.Now.AddSeconds(tokenConfigurations.Seconds);

                                   var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenConfigurations.Secret));
                                     //    var jwt = new JwtSecurityToken(  issuer: tokenConfigurations.Issuer,
                                     //                                     audience: tokenConfigurations.Audience,
                                     //                                     claims: identity.Claims,
                                     //                                     notBefore: dataCriacao,
                                     //                                     Subject = identity,
                                     //                                     expires: dataExpiracao,
                                     //                                     signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                                     //    );
                                     var handler = new JwtSecurityTokenHandler();
                                   var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                                   {
                                       Issuer = tokenConfigurations.Issuer,
                                       Audience = tokenConfigurations.Audience,
                                       SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                                       Subject = identity,
                                       NotBefore = dataCriacao,
                                       Expires = dataExpiracao
                                   });

                                   token = handler.WriteToken(securityToken);




                               }
                           });

            if (usuario.senha == MD5Hash.CalculaHash(model.senha))
            {
                UserAuth userAuth = new UserAuth();
                userAuth.id = jtiCodigo;
                userAuth.created = dataCriacao;
                userAuth.exp = dataExpiracao;
                userAuth.accessToken = token;
                userAuth.nome = usuario.nome;
                userAuth.apelido = usuario.nome;
                userAuth.email = usuario.usuario;
                userAuth.id_usuario = usuario._id;

                if (model.CamposAuxiliares.ContainsKey("REGRAS"))
                {
                    userAuth.roles = (String[])model.CamposAuxiliares["REGRAS"];
                }
                if (model.CamposAuxiliares.ContainsKey("ROTAS"))
                {
                    userAuth.routes = (String[])model.CamposAuxiliares["ROTAS"]; ;
                }

                _userAuthService.Insert(userAuth);

                return Ok(userAuth);
            }

            else
                return Unauthorized();

        }

        /*
                [HttpGet]
                [AllowAnonymous]
                [Route("validatoken")]
                public IActionResult validatoken()
                {
                    try
                    {
                        IDictionary<string, string> dictionaryRequest = HttpUtility.ParseQueryString(HttpContext.Request.QueryString.Value.Substring(2)).ToDictionary();

                            Usuario model= new Usuario();
                            MongoDbContext dbContext = new MongoDbContext();
                            var builder = Builders<Usuario>.Filter;
                            var filter = builder.Exists("usuario", true);

                            var senha = MD5Hash.CalculaHash(model.senha);

                            if (!String.IsNullOrEmpty(model.usuario))
                            {
                                filter = filter & builder.Eq("usuario", model.usuario);
                            }
                            else
                            {
                                throw new Exception("Usuário deve ser informado.");
                            }



                        model.CamposAuxiliares = new Dictionary<string, object>();
                        model.CamposAuxiliares.Add("token", dictionaryRequest["token"]);
                        var retornoModel = _nNegocio.validaToken(model);
                        if (retornoModel.CamposAuxiliares["status"].ToString().Equals("0"))
                            throw new Exception("Token Invalido");

                        return Ok(new RetornoSingleREST<Usuario>() { error = 0, mensagem = "MENSAGEM.SUCESSO", dados = retornoModel });

                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new RetornoSingleREST<Usuario>() { error = 1, mensagem = ex.Message, dados = null });
                    }
                }




                [HttpPost]
                [AllowAnonymous]
                [Route("alterasenha")]
                public IActionResult alterasenha([FromBody] Usuario usuario)
                {
                    try
                    {
                        //IDictionary<string, string> dictionaryRequest = HttpUtility.ParseQueryString(HttpContext.Request.QueryString.Value.Substring(2)).ToDictionary();

                        this.setBancoUsuario("bancosSQL");

                        NUsuario _nNegocio = new NUsuario(this.banco);

                        //Usuario model = JsonConvert.DeserializeObject<Usuario>(JsonConvert.SerializeObject(dictionaryRequest, Formatting.Indented));
                        // usuario.CamposAuxiliares = new Dictionary<string, object>();
                        //  usuario.CamposAuxiliares.Add("token", HttpContext.Request.Query["token"].ToString());
                        //  usuario.CamposAuxiliares.Add("token", dictionaryRequest["token"]);
                        var retornoModel = _nNegocio.alteraSenha(usuario);

                        //  LoginController login = new LoginController(IConfiguration);



                        return Ok(new RetornoSingleREST<Usuario>() { error = 0, mensagem = "MENSAGEM.SUCESSO", dados = retornoModel });

                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new RetornoSingleREST<Usuario>() { error = 1, mensagem = ex.Message, dados = null });
                    }
                }

                */

    }







}
