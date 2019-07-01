using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace KnowledgeMVCSite.Util
{
    public class PreviewTool
    {

        #region 预览Excel
        /// <summary>
        /// 预览Excel
        /// </summary>
        public string PreviewExcel(string physicalPath,  HttpServerUtilityBase Server)
        {
            Microsoft.Office.Interop.Excel.Application application = null;
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            application = new Microsoft.Office.Interop.Excel.Application();
            application.Visible = false;
            application.DisplayAlerts = false;
            workbook = application.Workbooks.Open(physicalPath,ReadOnly:true);
            //Save Excel to Html
            object format = Microsoft.Office.Interop.Excel.XlFileFormat.xlHtml;
            string htmlName = Path.GetFileNameWithoutExtension(physicalPath) + ".html";
            String outputFile = Path.GetDirectoryName(physicalPath) + "\\" + htmlName;
            workbook.SaveAs(outputFile,FileFormat:format);
            workbook.Close();
            application.Quit();
            return htmlName;
        }
        #endregion
        #region 预览Word
        /// <summary>
        /// 预览Word
        /// </summary>
        public string PreviewWord(string physicalPath,  HttpServerUtilityBase Server)
        {
            Microsoft.Office.Interop.Word._Application application = null;
            Microsoft.Office.Interop.Word._Document doc = null;
            application = new Microsoft.Office.Interop.Word.Application();
            object missing = Type.Missing;
            object trueObject = true;
            application.Visible = false;
            application.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            doc = application.Documents.Open(physicalPath, missing, trueObject, missing, missing, missing,
              missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            //Save Excel to Html
            object format = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatHTML;
            string htmlName = Path.GetFileNameWithoutExtension(physicalPath) + ".html";
            String outputFile = Path.GetDirectoryName(physicalPath) + "\\" + htmlName;
            doc.SaveAs(outputFile, format, missing, missing, missing,
                     missing, XlSaveAsAccessMode.xlNoChange, missing,
                     missing, missing, missing, missing);
            doc.Close();
            application.Quit();
            return  htmlName;
        }
        #endregion
        #region 预览Txt
        /// <summary>
        /// 预览Txt
        /// </summary>
        public string PreviewTxt(string physicalPath)
        {
            return physicalPath;
        }
        #endregion
        #region 预览Pdf
        /// <summary>
        /// 预览Pdf
        /// </summary>
        public string PreviewPdf(string physicalPath)
        {
            return physicalPath;
        }
        #endregion
    }

}