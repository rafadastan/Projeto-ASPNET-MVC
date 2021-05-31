using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoMVC01.Presentation.Models;
using ProjetoMVC01.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoMVC01.Presentation.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class RelatoriosController : Controller
    {
        private const string excel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private const string pdf = "application/pdf";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(RelatoriosViewModel model, 
            [FromServices] FornecedorRepository fornecedorRepository, 
            [FromServices] ProdutoRepository produtoRepository)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    //Relatório de fornecedores em formato excel
                    if(model.TipoRelatorio == 1 && model.FormatoRelatorio == 1)
                    {
                        var file = Reports.EXCEL.Reports
                            .FornecedorReport.GenerateReport(fornecedorRepository.GetAll());

                        DownloadFile(file, "fornecedores.xlsx", excel);
                    }
                    //Relatório de produtos em formato excel
                    else if (model.TipoRelatorio == 2 && model.FormatoRelatorio == 1)
                    {
                        var file = Reports.EXCEL.Reports
                            .ProdutoReport.GenerateReport(produtoRepository.GetAll());

                        DownloadFile(file, "produtos.xlsx", excel);
                    }
                    //Relatório de fornecedores em formato pdf
                    else if (model.TipoRelatorio == 1 && model.FormatoRelatorio == 2)
                    {
                        var file = Reports.PDF.Reports
                            .FornecedorReport.GenerateReport(fornecedorRepository.GetAll());

                        DownloadFile(file, "fornecedores.pdf", pdf);
                    }
                    //Relatório de produtos em formato pdf
                    else if (model.TipoRelatorio == 2 && model.FormatoRelatorio == 2)
                    {
                        var file = Reports.PDF.Reports
                            .ProdutoReport.GenerateReport(produtoRepository.GetAll());

                        DownloadFile(file, "produtos.pdf", pdf);
                    }
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = "Ocorreu um erro: " + e.Message;
                }
            }

            return View();
        }

        //método para realizar o download de um documento
        private void DownloadFile(byte[] file, string filename, string mimetype)
        {
            Response.Clear();
            Response.ContentType = mimetype; //tipo do arquivo do download
            Response.Headers.Add("content-disposition", "attachment; filename=" + filename);
            Response.Body.WriteAsync(file, 0, file.Length); //arquivo para download
            Response.Body.Flush(); //descarrega o conteudo em memoria
            Response.StatusCode = StatusCodes.Status200OK; //sucesso!
        }
    }
}
