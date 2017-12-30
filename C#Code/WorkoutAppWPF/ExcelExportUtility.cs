using System;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using Excel = Microsoft.Office.Interop.Excel;

namespace WorkoutAppWPF
{
    public class ExcelExportUtility

    {
        private Excel.Application oXL;
        private Excel._Workbook oWB;

        public void createWorkbook()
        {
            //Load resources
            Assembly assembly;
            assembly = Assembly.GetExecutingAssembly();

            //Start Excel and get Application object.
            oXL = new Excel.Application();
            oXL.DisplayAlerts = false;

            //create stream from resource
            Stream excelStream = assembly.GetManifestResourceStream("WorkoutAppWPF.ExportTemplate.xlsx");

            //create local path string so copy excel template to
            string currentDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ExcelUtility";

            // Determine whether the directory exists.
            if (!Directory.Exists(currentDir))
            {
                DirectoryInfo di = Directory.CreateDirectory(currentDir);
            }

            //create full file path
            string excelTemplatePath = currentDir + "\\.ExportTemplate.xlsx";

            //create new file
            var fileStream = File.Create(excelTemplatePath);
            System.Diagnostics.Debug.Write("Writing Template: " + excelTemplatePath + System.Environment.NewLine);

            //write the file and close the filestream 
            excelStream.CopyTo(fileStream);
            fileStream.Close();

            //Get a new workbook form resource template.
            oWB = (Excel._Workbook)(oXL.Workbooks.Add(excelTemplatePath));

        }


        public void saveWorkbook(String fileName)
        {
            //save workbook
            oWB.SaveAs(fileName);

            //close workbook
            oWB.Close();
            oXL.Quit();
        }

        public void renameWorksheet(String worksheetName, int idx)
        {
            //Get current workbook sheets
            Excel.Sheets xlSheets = oWB.Sheets as Excel.Sheets;

            //The first argument below inserts the new worksheet as the first one
            Excel.Worksheet xlNewSheet = (Excel.Worksheet)xlSheets[idx];
            xlNewSheet.Name = worksheetName;
        }

        public void addWorksheet(String worksheetName, int idx)
        {
            //Get current workbook sheets
            Excel.Sheets xlSheets = oWB.Sheets as Excel.Sheets;

            //The first argument below inserts the new worksheet as the first one
            Excel.Worksheet xlNewSheet = (Excel.Worksheet)xlSheets.Add(xlSheets[idx], Type.Missing, Type.Missing, Type.Missing);
            xlNewSheet.Name = worksheetName;
        }

        public void writeSingleCell(int row, int col, object value)
        {
            //Get active workbook sheet
            Excel.Worksheet oSheet = (Excel.Worksheet)oWB.ActiveSheet;

            //write value to the cell.
            oSheet.Cells[row, col] = value;
        }

        public void writeSingleCell(int row, int col, object value, int sheetIdx)
        {
            //Get active workbook sheet
            Excel.Worksheet oSheet = (Excel.Worksheet)oWB.Sheets[sheetIdx];

            //write value to the cell.
            oSheet.Cells[row, col] = value;
        }

        public void writeSingleCell(int row, int col, double value, int sheetIdx)
        {
            //Get active workbook sheet
            Excel.Worksheet oSheet = (Excel.Worksheet)oWB.Sheets[sheetIdx];

            //write value to the cell.
            oSheet.Cells[row, col] = value;
        }

        public void writeSingleCell(int row, int col, string value, int sheetIdx)
        {
            //Get active workbook sheet
            Excel.Worksheet oSheet = (Excel.Worksheet)oWB.Sheets[sheetIdx];

            //write value to the cell.
            oSheet.Cells[row, col] = value;
        }

        public void writeCellRange(object data, string rangeStart, string rangeStop)
        {
            //Get active workbook sheet
            Excel.Worksheet oSheet = (Excel.Worksheet)oWB.ActiveSheet;

            Excel.Range oRng = oSheet.get_Range(rangeStart, rangeStop);

            //write value to the cell.
            oRng.Value = data;
        }

        public void writeCellRange(object data, string rangeStart, string rangeStop, int sheetIdx)
        {
            //Get active workbook sheet
            Excel.Worksheet oSheet = (Excel.Worksheet)oWB.Sheets[sheetIdx];

            Excel.Range oRng = oSheet.get_Range(rangeStart, rangeStop);

            //write value to the cell.
            oRng.Value = data;
        }

        public void writeHeader(string[,] header)
        {
            for (int ii = 0; ii < header.GetLength(0); ii++)
            {
                for (int jj = 0; jj < header.GetLength(0); jj++)
                {

                    writeSingleCell(ii + 1, jj + 1, header[ii, jj]);
                }
            }
        }

        public void writeData(int row, double[] data, int sheetIdx)
        {
            for (int ii = 0; ii < data.GetLength(0); ii++)
            {

                writeSingleCell(row, ii + 1, data[ii]);

            }
        }

        public void writeData(int row, string[] data, int sheetIdx)
        {
            for (int ii = 0; ii < data.GetLength(0); ii++)
            {

                writeSingleCell(row, ii + 1, data[ii]);

            }
        }

        public void setCellColor(int row, int col, SolidColorBrush brush, int sheetIdx)
        {
            //Get active workbook sheet
            Excel.Worksheet oSheet = (Excel.Worksheet)oWB.Sheets[sheetIdx];

            //write value to the cell
            oSheet.Cells[row, col].Interior.Color = System.Drawing.ColorTranslator.ToOle(WpfBrushToDrawingColor(brush));

        }

        public void setRowHeight(int row, int sheetIdx , int height)
        {

            //Get active workbook sheet
            Excel.Worksheet oSheet = (Excel.Worksheet)oWB.Sheets[sheetIdx];

            oSheet.Cells[row, 1].rowHeight = height;

        }


        private System.Drawing.Color WpfBrushToDrawingColor(System.Windows.Media.SolidColorBrush br)
        {
            return System.Drawing.Color.FromArgb(
                br.Color.A,
                br.Color.R,
                br.Color.G,
                br.Color.B);
        }

        public void setThickBorderOutline(int rowStart, int colStart, int rowEnd, int colEnd, int sheetIdx)
        {

            string rangeStart = GetExcelColumnName(colStart).ToString() + rowStart;
            string rangeStop = GetExcelColumnName(colEnd).ToString() + rowEnd;


            //Get active workbook sheet
            Excel.Worksheet oSheet = (Excel.Worksheet)oWB.Sheets[sheetIdx];

            Excel.Range oRng = oSheet.get_Range(rangeStart, rangeStop);

            oRng.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);
        }

        public void setNormalBorderOutline(int rowStart, int colStart, int rowEnd, int colEnd, int sheetIdx)
        {

            string rangeStart = GetExcelColumnName(colStart).ToString() + rowStart;
            string rangeStop = GetExcelColumnName(colEnd).ToString() + rowEnd;


            //Get active workbook sheet
            Excel.Worksheet oSheet = (Excel.Worksheet)oWB.Sheets[sheetIdx];

            Excel.Range oRng = oSheet.get_Range(rangeStart, rangeStop);

            oRng.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThin);
        }

        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
    }
}
