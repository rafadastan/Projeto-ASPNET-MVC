using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoMVC01.Domain.Entities;
using ProjetoMVC01.Presentation.Models;
using ProjetoMVC01.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoMVC01.Presentation.Controllers
{
    [Authorize]
    public class FornecedorController : Controller
    {
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost] //qualifica o método para receber o SUBMIT do formulário
        public IActionResult Cadastro(FornecedorCadastroViewModel model,
            //parametro que receber a instancia da classe FornecedorRepository
            //definida no Startup do projeto (services.AddTransient)
            [FromServices] FornecedorRepository fornecedorRepository)
        {
            //verificar se todos os campos da classe model
            //passaram nas regras de validação..
            if(ModelState.IsValid)
            {
                try
                {
                    var fornecedor = new Fornecedor();

                    fornecedor.Nome = model.Nome;
                    fornecedor.Cnpj = model.Cnpj;

                    //cadastrando o fornecedor..
                    fornecedorRepository.Create(fornecedor);

                    //gerando uma mensagem na página..
                    TempData["MensagemSucesso"] = $"Fornecedor '{fornecedor.Nome}', cadastrado com sucesso.";

                    //limpar os campos do formulário..
                    ModelState.Clear();
                }
                catch(Exception e)
                {
                    //gerando uma mensagem na página..
                    TempData["MensagemErro"] = "Ocorreu um erro: " + e.Message;
                }
            }
            else
            {
                //gerando uma mensagem na página..
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário.";
            }

            return View();
        }

        public IActionResult Consulta()
        {
            return View();
        }

        [HttpPost] //qualifica o método para receber o SUBMIT do formulário
        public IActionResult Consulta(FornecedorConsultaViewModel model,
            [FromServices] FornecedorRepository fornecedorRepository)
        {
            //verificar se existem erros de validação nos campos..
            if(ModelState.IsValid)
            {
                try
                {
                    //executar a consulta de fornecedores por nome e
                    //armazenar o resultado da consulta na classe model
                    model.Fornecedores = fornecedorRepository.GetByNome(model.Nome);

                    //verificando se algum registro foi encontrado
                    if (model.Fornecedores.Count > 0)
                    {
                        TempData["MensagemSucesso"] = $"Consulta realizada com sucesso. {model.Fornecedores.Count} fornecedore(s) obtido(s).";
                    }
                    else
                    {
                        TempData["MensagemAlerta"] = "Nenhum resultado foi encontrado para a busca realizada.";
                    }
                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = "Ocorreu um erro: " + e.Message;
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário.";
            }

            //enviando o objeto 'model' de volta para a página
            return View(model);
        }

        //método para receber a requisição de exclusão..
        public IActionResult Exclusao(Guid id, //Fornecedor/Exclusao?id=?
            [FromServices] FornecedorRepository fornecedorRepository) 
        {
            try
            {
                //resgatar o fornecedor no banco de dados pelo id..
                var fornecedor = fornecedorRepository.GetById(id);

                //verificando se o fornecedor foi encontrado na base de dados..
                if(fornecedor != null)
                {
                    fornecedorRepository.Delete(fornecedor);
                    TempData["MensagemSucesso"] = $"Fornecedor '{fornecedor.Nome}' excluído com sucesso.";
                }
                else
                {
                    TempData["MensagemAlerta"] = "Fornecedor não foi encontrado.";
                }
            }
            catch(Exception e)
            {
                TempData["MensagemErro"] = "Ocorreu um erro: " + e.Message;
            }

            //retornando para a página de consulta
            return View("Consulta");
        }

        //método para abrir a página de edição de fornecedor..
        public IActionResult Edicao(Guid id, //Fornecedor/Edicao?id=?
            [FromServices] FornecedorRepository fornecedorRepository)
        {
            var model = new FornecedorEdicaoViewModel();

            try
            {
                //buscar o fornecedor no banco de dados atraves do id..
                var fornecedor = fornecedorRepository.GetById(id);

                if(fornecedor != null) //verificando se o fornecedor foi encontrado..
                {
                    model.IdFornecedor = fornecedor.IdFornecedor;
                    model.Nome = fornecedor.Nome;
                    model.Cnpj = fornecedor.Cnpj;
                }
                else
                {
                    TempData["MensagemAlerta"] = "Fornecedor não foi encontrado.";
                }
            }
            catch(Exception e)
            {
                TempData["MensagemErro"] = "Ocorreu um erro: " + e.Message;
            }

            return View(model);
        }

        //método para receber o submit do formulário de edição de fornecedor..
        [HttpPost]
        public IActionResult Edicao(FornecedorEdicaoViewModel model, 
            [FromServices] FornecedorRepository fornecedorRepository)
        {
            //verificar se todos os campos da classe model
            //passaram nas regras de validação..
            if (ModelState.IsValid)
            {
                try
                {
                    var fornecedor = fornecedorRepository.GetById(model.IdFornecedor);

                    fornecedor.IdFornecedor = model.IdFornecedor;
                    fornecedor.Nome = model.Nome;
                    fornecedor.Cnpj = model.Cnpj;

                    //atualizando o fornecedor..
                    fornecedorRepository.Update(fornecedor);

                    //gerando uma mensagem na página..
                    TempData["MensagemSucesso"] = $"Fornecedor '{fornecedor.Nome}', atualizado com sucesso.";
                }
                catch (Exception e)
                {
                    //gerando uma mensagem na página..
                    TempData["MensagemErro"] = "Ocorreu um erro: " + e.Message;
                }
            }
            else
            {
                //gerando uma mensagem na página..
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário.";
            }

            return View();
        }

        /*
         * JsonResult -> métodos que são executados atraves de funções JavaScript
         */
        public JsonResult ObterDadosGrafico([FromServices] FornecedorRepository fornecedorRepository)
        {
            try
            {
                //executando a consulta no banco de dados e obtendo os somatorios de
                //quantidade de produtos para cada fornecedor (DTO)
                var qtdProdutosFornecedor = fornecedorRepository.GroupByQtdProdutos();

                var model = new List<GraficosViewModel>();
                foreach (var item in qtdProdutosFornecedor)
                {
                    model.Add(new GraficosViewModel
                    {
                        Name = item.Fornecedor, //nome do fornecedor
                        Data = new List<decimal>() { item.QuantidadeProdutos } //qtd de produtos
                    }); ;
                }

                return Json(model); //retornando para o javascript..
            }
            catch(Exception e)
            {
                //gerando uma resposta de erro para o javascript
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return Json(e.Message);
            }
        }

    }
}
