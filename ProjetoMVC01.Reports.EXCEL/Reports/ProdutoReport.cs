using OfficeOpenXml;
using ProjetoMVC01.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoMVC01.Reports.EXCEL.Reports
{
    public class ProdutoReport
    {
        public static byte[] GenerateReport(List<Produto> produtos)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var excelPackage = new ExcelPackage())
            {
                var sheet = excelPackage.Workbook.Worksheets.Add("Produtos");

                sheet.Cells["A1"].Value = "Relatório de Produtos";

                sheet.Cells["A3"].Value = "Nome do Produto";
                sheet.Cells["B3"].Value = "Preço";
                sheet.Cells["C3"].Value = "Quantidade";
                sheet.Cells["D3"].Value = "Total";
                sheet.Cells["E3"].Value = "Descrição";
                sheet.Cells["F3"].Value = "Nome do Fornecedor";
                sheet.Cells["G3"].Value = "CNPJ";

                var linha = 4;
                foreach (var item in produtos)
                {
                    sheet.Cells[$"A{linha}"].Value = item.Nome;
                    sheet.Cells[$"B{linha}"].Value = item.Preco;
                    sheet.Cells[$"C{linha}"].Value = item.Quantidade;
                    sheet.Cells[$"D{linha}"].Formula = $"=B{linha}*C{linha}";
                    sheet.Cells[$"E{linha}"].Value = item.Descricao;
                    sheet.Cells[$"F{linha}"].Value = item.Fornecedor.Nome;
                    sheet.Cells[$"G{linha}"].Value = item.Fornecedor.Cnpj;

                    linha++;
                }

                sheet.Cells["A:AZ"].AutoFitColumns();

                //excelPackage.Save("senha");

                return excelPackage.GetAsByteArray();
            }
        }
    }
}
