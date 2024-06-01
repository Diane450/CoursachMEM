using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using coursach.Models;
using OfficeOpenXml;
using System.Data;
using System.Xml;

namespace courash.Helpers
{
    public class Report
    {
        public Document PdfDoc { get; set; }
        public List<Request> Data { get; set; }
    }
}
