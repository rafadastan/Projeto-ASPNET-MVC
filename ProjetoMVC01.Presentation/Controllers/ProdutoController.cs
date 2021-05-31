using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoMVC01.Domain.Entities;
using ProjetoMVC01.Presentation.Models;
using ProjetoMVC01.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoMVC01.Presentation.Controllers
{
    [Authorize]
    public class ProdutoController : Controller
    {
        //atributo utilizado para capturar a referencia do caminho da pasta /wwwroot
        private readonly IHostingEnvironment _hostingEnvironment;

        //construtor para inicialização do atributo
        public ProdutoController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        //método executado quando a página é carregada..
        public IActionResult Cadastro([FromServices] FornecedorRepository fornecedorRepository)
        {
            var model = new ProdutoCadastroViewModel();

            model.ListagemFornecedores = ObterFornecedores(fornecedorRepository);
            return View(model);
        }

        [HttpPost] //receber os dados enviados pelo formulário (SUBMIT)
        public IActionResult Cadastro(ProdutoCadastroViewModel model, 
            [FromServices] FornecedorRepository fornecedorRepository,
            [FromServices] ProdutoRepository produtoRepository)
        {
            //verificar se todos os campos do formulário passaram nas regras de validação..
            if(ModelState.IsValid)
            {
                try
                {
                    var produto = new Produto();

                    produto.Nome = model.Nome;
                    produto.Descricao = model.Descricao;
                    produto.Preco = Convert.ToDecimal(model.Preco);
                    produto.Quantidade = Convert.ToInt32(model.Quantidade);
                    produto.IdFornecedor = (Guid)model.IdFornecedor;
                    produto.Foto = "/imagens/foto.jpg";

                    //verificar se algum arquivo foi enviado para upload..
                    var files = Request.Form.Files; //resgatando os arquivos enviados..

                    //varrendo os arquivos enviados..
                    foreach (var item in files)
                    {
                        //obter a extensão do arquivo..
                        var extensao = Path.GetExtension(item.FileName).ToLower();

                        //verificando a extensão do arquivo (.jpg, .jpeg, .png)
                        if (extensao.Equals(".jpg") || extensao.Equals(".jpeg") || extensao.Equals(".png"))
                        {
                            //gerando o nome da foto do produto..
                            produto.Foto = "/imagens/" + Guid.NewGuid() + extensao;

                            //pegando o caminho da pasta /wwwroot
                            var path = _hostingEnvironment.WebRootPath;

                            //fazendo o upload da foto..
                            using (var stream = new FileStream(path + produto.Foto, FileMode.Create))
                            {
                                item.CopyTo(stream); //upload do arquivo!
                            }
                        }
                        else
                        {
                            TempData["MensagemAlerta"] = "O formato de imagem enviado não é válido. Atualize os dados do produto com uma nova foto.";
                        }
                    }

                    produtoRepository.Create(produto);

                    TempData["MensagemSucesso"] = $"Produto '{produto.Nome}', cadastrado com sucesso.";

                    model = new ProdutoCadastroViewModel();
                    ModelState.Clear();
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

            model.ListagemFornecedores = ObterFornecedores(fornecedorRepository);
            return View(model);
        }

        public IActionResult Consulta()
        {
            return View();
        }

        [HttpPost] //recebe o SUBMIT POST do formulário
        public IActionResult Consulta(ProdutoConsultaViewModel model, 
            [FromServices] ProdutoRepository produtoRepository)
        {
            //verificar se existem erros de validação nos campos..
            if (ModelState.IsValid)
            {
                try
                {
                    //executar a consulta de produtos por nome e
                    //armazenar o resultado da consulta na classe model
                    model.Produtos = produtoRepository.GetByNome(model.Nome);

                    //verificando se algum registro foi encontrado
                    if (model.Produtos.Count > 0)
                    {
                        TempData["MensagemSucesso"] = $"Consulta realizada com sucesso. {model.Produtos.Count} produto(s) obtido(s).";
                    }
                    else
                    {
                        TempData["MensagemAlerta"] = "Nenhum resultado foi encontrado para a busca realizada.";
                    }
                }
                catch (Exception e)
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

        //método para processar a requisição de exclusão
        public IActionResult Exclusao(Guid id, 
            [FromServices] ProdutoRepository produtoRepository)
        {
            try
            {
                //consultar o produto no banco de dados atraves do id..
                var produto = produtoRepository.GetById(id);

                //verificar se o produto foi encontrado
                if(produto != null)
                {
                    //excluindo o produto..
                    produtoRepository.Delete(produto);
                    
                    //excluindo a foto do produto..
                    if (!produto.Foto.Contains("imagens/foto.jpg"))
                        System.IO.File.Delete(_hostingEnvironment.WebRootPath + produto.Foto);

                    TempData["MensagemSucesso"] = $"Produto '{produto.Nome}' excluído com sucesso.";
                }
                else
                {
                    TempData["MensagemAlerta"] = "Produto não encontrado.";
                }
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Ocorreu um erro: " + e.Message;
            }

            //retornar para a página de consulta..
            return View("Consulta");
        }

        public IActionResult Edicao(Guid id, 
            [FromServices] FornecedorRepository fornecedorRepository, 
            [FromServices] ProdutoRepository produtoRepository)
        {
            var model = new ProdutoEdicaoViewModel();

            try
            {
                //buscar o produto baseado no id..
                var produto = produtoRepository.GetById(id);

                if(produto != null) //verificando se o produto foi encontrado..
                {
                    //transferir os dados do produto para o objeto model..
                    model.IdProduto = produto.IdProduto;
                    model.Nome = produto.Nome;
                    model.Preco = produto.Preco;
                    model.Quantidade = produto.Quantidade;
                    model.Descricao = produto.Descricao;
                    model.IdFornecedor = produto.IdFornecedor;
                }
                else
                {
                    TempData["MensagemAlerta"] = "Produto não encontrado.";
                }
            }
            catch(Exception e)
            {
                TempData["MensagemErro"] = "Ocorreu um erro: " + e.Message;
            }

            model.ListagemFornecedores = ObterFornecedores(fornecedorRepository);
            return View(model);
        }

        [HttpPost] //receber os dados enviados pelo formulário (SUBMIT)
        public IActionResult Edicao(ProdutoEdicaoViewModel model,
            [FromServices] FornecedorRepository fornecedorRepository,
            [FromServices] ProdutoRepository produtoRepository)
        {
            //verificar se todos os campos do formulário passaram nas regras de validação..
            if (ModelState.IsValid)
            {
                try
                {
                    var produto = produtoRepository.GetById(model.IdProduto);

                    produto.Nome = model.Nome;
                    produto.Descricao = model.Descricao;
                    produto.Preco = Convert.ToDecimal(model.Preco);
                    produto.Quantidade = Convert.ToInt32(model.Quantidade);
                    produto.IdFornecedor = (Guid)model.IdFornecedor;

                    var files = Request.Form.Files; 

                    foreach (var item in files)
                    {
                        var extensao = Path.GetExtension(item.FileName).ToLower();
                        if (extensao.Equals(".jpg") || extensao.Equals(".jpeg") || extensao.Equals(".png"))
                        {                           
                            var path = _hostingEnvironment.WebRootPath;

                            if(produto.Foto.Contains("imagens/foto.jpg"))
                                produto.Foto = "/imagens/" + Guid.NewGuid() + extensao;

                            using (var stream = new FileStream(path + produto.Foto, FileMode.Create))
                            {
                                item.CopyTo(stream); 
                            }
                        }
                        else
                        {
                            TempData["MensagemAlerta"] = "O formato de imagem enviado não é válido. Atualize os dados do produto com uma nova foto.";
                        }
                    }

                    produtoRepository.Update(produto);

                    TempData["MensagemSucesso"] = $"Produto '{produto.Nome}', atualizado com sucesso.";
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = "Ocorreu um erro: " + e.Message;
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário.";
            }

            model.ListagemFornecedores = ObterFornecedores(fornecedorRepository);
            return View(model);
        }

        #region Carregamento de Fornecedores

        private List<SelectListItem> ObterFornecedores(FornecedorRepository fornecedorRepository)
        {
            var listagemFornecedores = new List<SelectListItem>();

            try
            {
                foreach (var item in fornecedorRepository.GetAll())
                {
                    var selectListItem = new SelectListItem
                    {
                        Value = item.IdFornecedor.ToString(),
                        Text = $"{item.Cnpj} - {item.Nome.ToUpper()}"
                    };

                    listagemFornecedores.Add(selectListItem);
                }
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            return listagemFornecedores;
        }

        #endregion
    }
}
