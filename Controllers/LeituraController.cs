using Microsoft.AspNetCore.Mvc;
using TSEA.API.Models;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.IO;
using System.Linq;

[ApiController]
[Route("api/leituras")]
public class LeiturasController : ControllerBase
{
    private readonly AppDbContext _context;

    public LeiturasController(AppDbContext context)
    {
        _context = context;
    }

    // 📊 GET COM FILTRO (tela)
    [HttpGet]
    public IActionResult Get(DateTime? inicio, DateTime? fim, string? maquina)
    {
        var query = _context.Leituras.AsQueryable();

        if (inicio.HasValue)
            query = query.Where(x => x.Data >= inicio);

        if (fim.HasValue)
            query = query.Where(x => x.Data <= fim);

        if (!string.IsNullOrEmpty(maquina))
            query = query.Where(x => x.Maquina == maquina);

        return Ok(query.ToList());
    }

    // 📊 EXCEL COM FILTRO
    [HttpGet("export/excel")]
    public IActionResult ExportExcel(DateTime? inicio, DateTime? fim, string? maquina)
    {
        var query = _context.Leituras.AsQueryable();

        if (inicio.HasValue)
            query = query.Where(x => x.Data >= inicio);

        if (fim.HasValue)
            query = query.Where(x => x.Data <= fim);

        if (!string.IsNullOrEmpty(maquina))
            query = query.Where(x => x.Maquina == maquina);

        var dados = query.ToList();

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Leituras");

        ws.Cell(1, 1).Value = "Data";
        ws.Cell(1, 2).Value = "Temperatura";
        ws.Cell(1, 3).Value = "Vibração";
        ws.Cell(1, 4).Value = "Corrente";
        ws.Cell(1, 5).Value = "Máquina";

        ws.Range(1, 1, 1, 5).Style.Font.Bold = true;

        int row = 2;

        foreach (var d in dados)
        {
            var cell = ws.Cell(row, 1);
            cell.Value = d.Data;
            cell.Style.DateFormat.Format = "dd/MM/yyyy HH:mm";

            ws.Cell(row, 2).Value = d.Temperatura;
            ws.Cell(row, 3).Value = d.Vibracao;
            ws.Cell(row, 4).Value = d.Corrente;
            ws.Cell(row, 5).Value = d.Maquina;

            row++;
        }

        ws.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        wb.SaveAs(stream);

        return File(stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "relatorio.xlsx");
    }

    // 📄 PDF COM FILTRO
    [HttpGet("export/pdf")]
    public IActionResult ExportPdf(DateTime? inicio, DateTime? fim, string? maquina)
    {
        var query = _context.Leituras.AsQueryable();

        if (inicio.HasValue)
            query = query.Where(x => x.Data >= inicio);

        if (fim.HasValue)
            query = query.Where(x => x.Data <= fim);

        if (!string.IsNullOrEmpty(maquina))
            query = query.Where(x => x.Maquina == maquina);

        var dados = query.ToList();

        QuestPDF.Settings.License = LicenseType.Community;

        var pdf = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Header()
                    .Text("RELATÓRIO LEITURAS")
                    .FontSize(18)
                    .Bold();

                page.Content().Column(col =>
                {
                    col.Spacing(5);

                    col.Item().Text($"Gerado em: {DateTime.Now}");

                    col.Item().LineHorizontal(1);

                    foreach (var d in dados)
                    {
                        col.Item().Text(
                            $"{d.Data} | Temp: {d.Temperatura} | Vib: {d.Vibracao} | Corr: {d.Corrente} | {d.Maquina}"
                        );
                    }
                });
            });
        });

        using var stream = new MemoryStream();
        pdf.GeneratePdf(stream);

        return File(stream.ToArray(), "application/pdf", "relatorio.pdf");
    }
}