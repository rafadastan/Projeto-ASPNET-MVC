using OfficeOpenXml;
using ProjetoMVC01.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoMVC01.Reports.EXCEL.Reports
{
    public class FornecedorReport
    {
        public static byte[] GenerateReport(List<Fornecedor> fornecedores)
        {
            //definindo o tipo de licença utilizado no EpPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //iniciar o desenvolvimento do relatorio..
            using (var excelPackage = new ExcelPackage())
            {
                //criando a planilha
                var sheet = excelPackage.Workbook.Worksheets.Add("Fornecedores");

                //escrevendo o conteudo
                sheet.Cells["A1"].Value = "Relatorio de Fornecedores";

                sheet.Cells["A3"].Value = "Nome do Fornecedor";
                sheet.Cells["B3"].Value = "CNPJ";

                var linha = 4;
                foreach (var item in fornecedores)
                {
                    sheet.Cells[$"A{linha}"].Value = item.Nome;
                    sheet.Cells[$"B{linha}"].Value = item.Cnpj;

                    linha++;
                }

                //ajustando a largura das colunas
                sheet.Cells["A:AZ"].AutoFitColumns();

                //retornando o conteudo da planilha em byte[]
                return excelPackage.GetAsByteArray();
            }
        }
    }
}
