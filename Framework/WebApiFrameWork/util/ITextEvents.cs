using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApiFrameWork.util
{

    public class ITextEvents : PdfPageEventHelper
    {
        public ITextEvents(string header) { _header = header; }
        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate headerTemplate, footerTemplate;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;

        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;


        #region Fields
        private string _header;
        #endregion

        #region Properties
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        #endregion


        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                headerTemplate = cb.CreateTemplate(400, 150);
                footerTemplate = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {
                System.Console.WriteLine(de);
            }
            catch (System.IO.IOException ioe)
            {
                System.Console.WriteLine(ioe);
            }
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            Font baseFontNormal = FontFactory.GetFont(FontFactory.HELVETICA, 12f, Font.NORMAL, BaseColor.Black);

            Font baseFontBig = FontFactory.GetFont(FontFactory.TIMES, 10f, Font.NORMAL, BaseColor.Black);
            Font time = FontFactory.GetFont(FontFactory.HELVETICA, 10f, Font.NORMAL);
            var arq = Directory.GetCurrentDirectory() + @"\APP_DATA\logo-cetrel-header.PNG";
            var logo = Image.GetInstance(arq);
            logo.ScaleToFit(140f, 52f);
            //document.Add(new Phrase(Environment.NewLine));
            //var p1Header = new Phrase("\"Sistema de Evasores\"", baseFontNormal);
            var p1Header = new Phrase(" Portal dos Fornecedores - " + _header, baseFontNormal);

            //Create PdfTable object
            var pdfTab = new PdfPTable(3);
            float[] width = { 100f, 320f, 100f };
            pdfTab.SetWidths(width);
            pdfTab.TotalWidth = 520f;
            pdfTab.LockedWidth = true;
            //We will have to create separate cells to include image logo and 2 separate strings
            //Row 1
            var pdfCell1 = new PdfPCell(logo);
            var pdfCell2 = new PdfPCell(p1Header);
            var text = "Página " + writer.PageNumber + " de ";
            //string

            //Add paging to header
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 8);
                cb.SetTextMatrix(document.PageSize.GetRight(100), document.PageSize.GetTop(45));
                cb.ShowText(text);
                cb.EndText();
                float len = bf.GetWidthPoint(text, 8);
                //Adds "12" in Page 1 of 12
                cb.AddTemplate(headerTemplate, document.PageSize.GetRight(100) + len, document.PageSize.GetTop(45));
            }
            //Add paging to footer
            {
                //var leftCol = new Paragraph("Mukesh Salaria\n" + "Software Engineer", time);
                //var rightCol = new Paragraph("LearnShareCorner.com\n" + "Techical Blog", time);
                //var phone = new Paragraph("Phone +91-9814268272", time);
                var address = new Paragraph("      Portal dos Fornecedores\n" + "            Cetrel", time);
                //var fax = new Paragraph("mukeshsalaria01@gmail.com", time);

                //leftCol.Alignment = Element.ALIGN_LEFT;
                //rightCol.Alignment = Element.ALIGN_RIGHT;
                //fax.Alignment = Element.ALIGN_RIGHT;
                //phone.Alignment = Element.ALIGN_LEFT;
                address.Alignment = Element.ALIGN_CENTER;

                var footerTbl = new PdfPTable(3) { TotalWidth = 520f, HorizontalAlignment = Element.ALIGN_CENTER, LockedWidth = true };
                float[] widths = { 150f, 220f, 150f };
                footerTbl.SetWidths(widths);
                //var footerCell1 = new PdfPCell(leftCol);
                //var footerCell2 = new PdfPCell();
                //var footerCell3 = new PdfPCell(rightCol);
                var sep = new PdfPCell();
                //var footerCell4 = new PdfPCell(phone);
                var footerCell5 = new PdfPCell(address);
                //var footerCell6 = new PdfPCell(fax);


                //footerCell1.Border = 0;
                //footerCell2.Border = 0;
                //footerCell3.Border = 0;
                //footerCell4.Border = 0;
                footerCell5.Border = 0;
                //footerCell6.Border = 0;
                //footerCell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                sep.Border = 0;
                sep.FixedHeight = 10f;
                //footerCell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                //footerCell6.PaddingLeft = 0;
                sep.Colspan = 3;

                // footerTbl.AddCell(footerCell1);
                //footerTbl.AddCell(footerCell2);
                //footerTbl.AddCell(footerCell3);
                footerTbl.AddCell(sep);
                //footerTbl.AddCell(footerCell4);
                footerTbl.AddCell(footerCell5);
                //footerTbl.AddCell(footerCell6);
                footerTbl.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin, writer.DirectContent);
            }
            //Row 2
            // PdfPCell pdfCell4 = new PdfPCell(new Phrase("No job is so urgent that it cannot be done safely", baseFontNormal));
            //Row 3

            var pdfCell3 = new PdfPCell(new Phrase("Data:" + PrintTime.ToShortDateString(), baseFontBig));
            var pdfCell4 = new PdfPCell();
            //var pdfCell5 = new PdfPCell(new Phrase("TIME:" + string.Format("{0:t}", DateTime.Now), baseFontBig));
            var pdfCell5 = new PdfPCell(new Phrase(""));

            //set the alignment of all three cells and set border to 0
            pdfCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell2.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell3.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell5.HorizontalAlignment = Element.ALIGN_RIGHT;

            //pdfCell2.VerticalAlignment = Element.ALIGN_BOTTOM;

            //pdfCell1.Colspan = 3;
            //pdfCell2.Colspan = 3;
            //pdfCell1.PaddingTop = 15f;
            pdfCell2.PaddingLeft = 45f;
            pdfCell2.PaddingTop = 10f;
            //pdfCell3.PaddingLeft = 45f;
            pdfCell3.PaddingTop = 10f;
            pdfCell5.PaddingTop = 9f;

            pdfCell1.Border = 0;
            pdfCell2.Border = 0;
            pdfCell3.Border = 0;
            pdfCell4.Border = 0;
            pdfCell5.Border = 0;

            //add all three cells into PdfTable
            pdfTab.AddCell(pdfCell1);
            pdfTab.AddCell(pdfCell2);
            pdfTab.AddCell(pdfCell3);
            //pdfTab.AddCell(pdfCell4);
            //pdfTab.AddCell(pdfCell5);

            pdfTab.TotalWidth = 520f;
            pdfTab.LockedWidth = true;
            //pdfTab.TotalWidth = document.PageSize.Width;
            //pdfTab.WidthPercentage = 100;

            //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
            //first param is start row. -1 indicates there is no end row and all the rows to be included to write
            //Third and fourth param is x and y position to start writing
            pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 30, writer.DirectContent);
            //set pdfContent value

            //Move the pointer and draw line to separate header section from rest of page
            cb.MoveTo(40, document.PageSize.Height - 100);
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 100);
            cb.Stroke();

            //Move the pointer and draw line to separate footer section from rest of page
            cb.MoveTo(30, document.PageSize.GetBottom(50));
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
            cb.Stroke();
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            headerTemplate.BeginText();
            headerTemplate.SetFontAndSize(bf, 12);
            headerTemplate.SetTextMatrix(0, 0);
            headerTemplate.ShowText((writer.PageNumber - 1).ToString());
            headerTemplate.EndText();

            footerTemplate.BeginText();
            footerTemplate.SetFontAndSize(bf, 12);
            footerTemplate.SetTextMatrix(0, 0);
            footerTemplate.ShowText((writer.PageNumber - 1).ToString());
            footerTemplate.EndText();
        }
    }

    public class PDFFooter : PdfPageEventHelper
    {

        //Image imgfoot = Image.getInstance("FooterImage.jpg");
        Image imgfoot = Image.GetInstance(Directory.GetCurrentDirectory() + @"\APP_DATA\logo-cetrel-header.PNG");
        Image imghead = Image.GetInstance(Directory.GetCurrentDirectory() + @"\APP_DATA\logo-cetrel-header.PNG");

        // write on top of document
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {

            base.OnOpenDocument(writer, document);
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            tabFot.SpacingAfter = 10F;
            PdfPCell cell;
            tabFot.TotalWidth = 300F;
            cell = new PdfPCell(new Phrase("Header"));
            tabFot.AddCell(cell);
            tabFot.WriteSelectedRows(0, -1, 150, document.Top, writer.DirectContent);
        }

        // write on start of each page
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            //imghead.SetAbsolutePosition(0, 0);
            //imgfoot.SetAbsolutePosition(0, 0);
            //PdfContentByte cbhead = writer.DirectContent;
            //PdfTemplate tp = cbhead.CreateTemplate(600, 250);
            //tp.AddImage(imghead);

            //PdfContentByte cbfoot = writer.DirectContent;
            //PdfTemplate tpl = cbfoot.CreateTemplate(600, 250);
            //tpl.AddImage(imgfoot);

            //cbhead.AddTemplate(tp, 0, 715);
            //cbfoot.AddTemplate(tpl, 0, 0);

            //Phrase headPhraseImg = new Phrase(cbhead + "",
            //FontFactory.GetFont(FontFactory.TIMES_ROMAN, 7,
            //iTextSharp.text.Font.NORMAL));
            //            Phrase footPhraseImg = new Phrase(cbfoot + "",
            //FontFactory.GetFont(FontFactory.TIMES_ROMAN, 7,
            //iTextSharp.text.Font.NORMAL));

            //HeaderFooter header = new HeaderFooter(headPhraseImg, true);
            //HeaderFooter footer = new HeaderFooter(footPhraseImg, true);


            base.OnStartPage(writer, document);
        }

        // write on end of each page
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            PdfPCell cell;
            tabFot.TotalWidth = 300F;

            cell = new PdfPCell(new Phrase("Footer"));
            tabFot.AddCell(cell);
            tabFot.WriteSelectedRows(0, -1, 150, document.Bottom, writer.DirectContent);
        }

        //write on close of document
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }
    }

    public class ITextEventsRelatorios : PdfPageEventHelper
    {
        public ITextEventsRelatorios(string header) { _header = header; }
        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate headerTemplate, footerTemplate;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;

        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;


        #region Fields
        private string _header;
        #endregion

        #region Properties
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        #endregion


        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                headerTemplate = cb.CreateTemplate(50, 50);
                footerTemplate = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {
                System.Console.WriteLine(de);
            }
            catch (System.IO.IOException ioe)
            {
                System.Console.WriteLine(ioe);
            }
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            Font baseFontNormal = FontFactory.GetFont(FontFactory.HELVETICA, 12f, Font.NORMAL, BaseColor.Black);

            Font baseFontBig = FontFactory.GetFont(FontFactory.TIMES, 10f, Font.NORMAL, BaseColor.Black);
            Font time = FontFactory.GetFont(FontFactory.HELVETICA, 10f, Font.NORMAL);

            var logo = Image.GetInstance(Directory.GetCurrentDirectory() + @"\APP_DATA\logo-cetrel-header.PNG");
            logo.ScaleToFit(140f, 52f);
            //document.Add(new Phrase(Environment.NewLine));
            //var p1Header = new Phrase("\"Sistema de Evasores\"", baseFontNormal);
            var p1Header = new Phrase(_header, baseFontNormal);

            //Create PdfTable object
            var pdfTab = new PdfPTable(3);
            float[] width = { 100f, 320f, 100f };
            pdfTab.SetWidths(width);
            pdfTab.TotalWidth = 520f;
            pdfTab.LockedWidth = true;
            //We will have to create separate cells to include image logo and 2 separate strings
            //Row 1
            var pdfCell1 = new PdfPCell(logo);
            var pdfCell2 = new PdfPCell(p1Header);
            var text = "Página " + writer.PageNumber + " de ";
            //string

            //Add paging to header
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 8);
                cb.SetTextMatrix(document.PageSize.GetRight(100), document.PageSize.GetTop(45));
                cb.ShowText(text);
                cb.EndText();
                float len = bf.GetWidthPoint(text, 8);
                //Adds "12" in Page 1 of 12
                cb.AddTemplate(headerTemplate, document.PageSize.GetRight(100) + len, document.PageSize.GetTop(45));
            }
            //Add paging to footer
            {
                //var leftCol = new Paragraph("Mukesh Salaria\n" + "Software Engineer", time);
                //var rightCol = new Paragraph("LearnShareCorner.com\n" + "Techical Blog", time);
                //var phone = new Paragraph("Phone +91-9814268272", time);
                var address = new Paragraph("      Portal dos Fornecedores\n" + "            Cetrel", time);
                //var fax = new Paragraph("mukeshsalaria01@gmail.com", time);

                //leftCol.Alignment = Element.ALIGN_LEFT;
                //rightCol.Alignment = Element.ALIGN_RIGHT;
                //fax.Alignment = Element.ALIGN_RIGHT;
                //phone.Alignment = Element.ALIGN_LEFT;
                address.Alignment = Element.ALIGN_CENTER;

                var footerTbl = new PdfPTable(3) { TotalWidth = 520f, HorizontalAlignment = Element.ALIGN_CENTER, LockedWidth = true };
                float[] widths = { 150f, 220f, 150f };
                footerTbl.SetWidths(widths);
                //var footerCell1 = new PdfPCell(leftCol);
                //var footerCell2 = new PdfPCell();
                //var footerCell3 = new PdfPCell(rightCol);
                var sep = new PdfPCell();
                //var footerCell4 = new PdfPCell(phone);
                var footerCell5 = new PdfPCell(address);
                //var footerCell6 = new PdfPCell(fax);


                //footerCell1.Border = 0;
                //footerCell2.Border = 0;
                //footerCell3.Border = 0;
                //footerCell4.Border = 0;
                footerCell5.Border = 0;
                //footerCell6.Border = 0;
                //footerCell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                sep.Border = 0;
                sep.FixedHeight = 10f;
                //footerCell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                //footerCell6.PaddingLeft = 0;
                sep.Colspan = 3;

                // footerTbl.AddCell(footerCell1);
                //footerTbl.AddCell(footerCell2);
                //footerTbl.AddCell(footerCell3);
                footerTbl.AddCell(sep);
                //footerTbl.AddCell(footerCell4);
                footerTbl.AddCell(footerCell5);
                //footerTbl.AddCell(footerCell6);
                footerTbl.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin, writer.DirectContent);
            }
            //Row 2
            // PdfPCell pdfCell4 = new PdfPCell(new Phrase("No job is so urgent that it cannot be done safely", baseFontNormal));
            //Row 3

            var pdfCell3 = new PdfPCell(new Phrase(""));
            var pdfCell4 = new PdfPCell();
            //var pdfCell5 = new PdfPCell(new Phrase("TIME:" + string.Format("{0:t}", DateTime.Now), baseFontBig));
            var pdfCell5 = new PdfPCell(new Phrase(""));

            //set the alignment of all three cells and set border to 0
            pdfCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell2.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell3.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell5.HorizontalAlignment = Element.ALIGN_RIGHT;

            //pdfCell2.VerticalAlignment = Element.ALIGN_BOTTOM;

            //pdfCell1.Colspan = 3;
            //pdfCell2.Colspan = 3;
            //pdfCell1.PaddingTop = 15f;
            pdfCell2.PaddingLeft = 45f;
            pdfCell2.PaddingTop = 10f;
            //pdfCell3.PaddingLeft = 45f;
            pdfCell3.PaddingTop = 10f;
            pdfCell5.PaddingTop = 9f;

            pdfCell1.Border = 0;
            pdfCell2.Border = 0;
            pdfCell3.Border = 0;
            pdfCell4.Border = 0;
            pdfCell5.Border = 0;

            //add all three cells into PdfTable
            pdfTab.AddCell(pdfCell1);
            pdfTab.AddCell(pdfCell2);
            pdfTab.AddCell(pdfCell3);
            //pdfTab.AddCell(pdfCell4);
            //pdfTab.AddCell(pdfCell5);

            pdfTab.TotalWidth = 520f;
            pdfTab.LockedWidth = true;
            //pdfTab.TotalWidth = document.PageSize.Width;
            //pdfTab.WidthPercentage = 100;

            //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
            //first param is start row. -1 indicates there is no end row and all the rows to be included to write
            //Third and fourth param is x and y position to start writing
            pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 30, writer.DirectContent);
            //set pdfContent value

            //Move the pointer and draw line to separate header section from rest of page
            cb.MoveTo(40, document.PageSize.Height - 100);
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 100);
            cb.Stroke();

            //Move the pointer and draw line to separate footer section from rest of page
            cb.MoveTo(30, document.PageSize.GetBottom(50));
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
            cb.Stroke();
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            headerTemplate.BeginText();
            headerTemplate.SetFontAndSize(bf, 12);
            headerTemplate.SetTextMatrix(0, 0);
            headerTemplate.ShowText((writer.PageNumber).ToString());
            headerTemplate.EndText();

            footerTemplate.BeginText();
            footerTemplate.SetFontAndSize(bf, 12);
            footerTemplate.SetTextMatrix(0, 0);
            footerTemplate.ShowText((writer.PageNumber).ToString());
            footerTemplate.EndText();
        }
    }

}
