using Microsoft.Office.Interop.Excel;
using OlQualityIndicatorManager.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlQualityIndicatorManager.Services.Exports
{
    public class QualityIndicatorExcelExporter
    {
        Workbook workbook;
        public void ExportAllQualityIndicators(List<OlGuideline> GuideLineList)
        {
            var excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible = true;

            workbook = excel.Workbooks.Add(Type.Missing);
            
            foreach (OlGuideline guideline in GuideLineList)
            {
                int count = workbook.Worksheets.Count;
                Worksheet sheet = (Worksheet)excel.Worksheets.Add(Type.Missing, workbook.Worksheets[count], Type.Missing, Type.Missing);
                sheet.Name = guideline.ShortTitle.Substring(0, Math.Min(guideline.ShortTitle.Length, 31));
                ExportQiToSheet(sheet, guideline.QualityIndicatorList);
            }

            workbook.SaveAs(@"c:\test\Qi-Liste.xlsx");
            workbook.Close();
            excel.Quit();
        }

        private void ExportQiToSheet(Worksheet sheet, List<OlSubsection> QualityIndicatorList)
        {
            sheet.Columns[1].ColumnWidth = 100;
            sheet.Columns[2].ColumnWidth = 100;
            sheet.Columns[3].ColumnWidth = 100;
            sheet.Columns[4].ColumnWidth = 25;
            sheet.Columns[5].ColumnWidth = 50;
            sheet.Columns[6].ColumnWidth = 50;

            Range firstRow = sheet.Rows[1];
            firstRow.Font.Bold = true;
            firstRow.Cells[1, 1] = "Titel";
            firstRow.Cells[1, 2] = "Zähler";
            firstRow.Cells[1, 3] = "Nenner";
            firstRow.Cells[1, 4] = "Referenzempfehlungen";
            firstRow.Cells[1, 5] = "Typ";
            firstRow.Cells[1, 6] = "Kategorie";

            for (int i = 0; i< QualityIndicatorList.Count; i++)
            {
                OlSubsection qi = QualityIndicatorList[i];
                Range row = sheet.Rows[i + 2];
                row.Cells[i + 1, 1].WrapText = true;
                row.Cells[i + 1, 1] = qi.Title;
                row.Cells[i + 1, 2].WrapText = true;
                row.Cells[i + 1, 2] = qi.Numerator;
                row.Cells[i + 1, 3].WrapText = true;
                row.Cells[i + 1, 3] = qi.Denominator;
                row.Cells[i + 1, 4].WrapText = true;
                row.Cells[i + 1, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                row.Cells[i + 1, 4] = string.Join(", ", qi.ReferenceRecommendationList.Select(item => item.Number));
            }
        }
    }
}
