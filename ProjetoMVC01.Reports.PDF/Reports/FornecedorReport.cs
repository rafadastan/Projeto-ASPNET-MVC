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
    public class FornecedorReport
    {
        public static byte[] GenerateReport(List<Fornecedor> fornecedores)
        {
            MemoryStream memoryStream = new MemoryStream();
            var pdf = new PdfDocument(new PdfWriter(memoryStream));

            using (var document = new Document(pdf))
            {
                document.Add(new Paragraph("Relatório de Fornecedores"));

                var table = new Table(2); //numero de colunas

                table.AddHeaderCell("Nome do Fornecedor");
                table.AddHeaderCell("CNPJ");

                foreach (var item in fornecedores)
                {
                    table.AddCell(item.Nome);
                    table.AddCell(item.Cnpj);
                }

                document.Add(table);
            }

            return memoryStream.ToArray();
        }
    }
}
