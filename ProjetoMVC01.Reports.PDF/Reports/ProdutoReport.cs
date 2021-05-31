using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using ProjetoMVC01.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProjetoMVC01.Reports.PDF.Reports
{
    public class ProdutoReport
    {
        public static byte[] GenerateReport(List<Produto> produtos)
        {
            MemoryStream memoryStream = new MemoryStream();
            var pdf = new PdfDocument(new PdfWriter(memoryStream));

            using (var document = new Document(pdf))
            {
                document.Add(new Paragraph("Relatório de Produtos"));

                var table = new Table(6); //número de colunas

                table.AddHeaderCell("Nome do produto");
                table.AddHeaderCell("Preço");
                table.AddHeaderCell("Quantidade");
                table.AddHeaderCell("Descrição");
                table.AddHeaderCell("Fornecedor");
                table.AddHeaderCell("CNPJ");

                foreach (var item in produtos)
                {
                    table.AddCell(item.Nome);
                    table.AddCell(item.Preco.ToString("c"));
                    table.AddCell(item.Quantidade.ToString());
                    table.AddCell(item.Descricao);
                    table.AddCell(item.Fornecedor.Nome);
                    table.AddCell(item.Fornecedor.Cnpj);
                }

                document.Add(table);
            }

            return memoryStream.ToArray();
        }
    }
}
