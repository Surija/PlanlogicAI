using AutoMapper;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using V = DocumentFormat.OpenXml.Vml;
using W = DocumentFormat.OpenXml.Wordprocessing;
using Wp = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using Wps = DocumentFormat.OpenXml.Office2010.Word.DrawingShape;
using Wp14 = DocumentFormat.OpenXml.Office2010.Word.Drawing;
using A14 = DocumentFormat.OpenXml.Office2010.Drawing;
using Ovml = DocumentFormat.OpenXml.Vml.Office;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlanlogicAI.Data;
using PlanlogicAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Extensions.Configuration;

namespace PlanlogicAI.Controllers
{
    [Route("/api/insuranceDocumentGenerator")]
    public class InsuranceDocumentController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;

        public IConfiguration Configuration { get; }


        public InsuranceDocumentController(StrategyOptimizerPrototypeContext context,IMapper mapper, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.Configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateDocument([FromBody] InsuranceDocumentDetails documentDetails)
        {
            try
            {

                //string filePath = Environment.CurrentDirectory + @"\\Documents\\" + documentDetails[0].clientDetails.FamilyName.ToString().Trim() + "," + documentDetails[0].clientDetails.ClientName.ToString().Trim() + "-" + documentDetails[0].clientDetails.ClientId.ToString().Trim() + ".docx";
                MemoryStream ms = new MemoryStream();
                string _storageConnection = Configuration.GetConnectionString("StorageConnectionString");
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_storageConnection);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("plstorage");

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });
                }
                string filePath = documentDetails.clientDetails.FamilyName.ToString().Trim() + "," + documentDetails.clientDetails.ClientName.ToString().Trim() + "-" + documentDetails.clientDetails.ClientId.ToString().Trim() + "-Insurance.docx";

                CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(filePath);
                using (WordprocessingDocument package = WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document))
                {

                    MainDocumentPart mainPart = package.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());
                    SectionProperties sectionProperties1 = new SectionProperties() { RsidRPr = "008818E9", RsidR = "00543A0D" };
                    FooterReference footerReference1 = new FooterReference() { Type = HeaderFooterValues.Default, Id = "rId8" };

                    sectionProperties1.Append(footerReference1);

                    body.Append(sectionProperties1);





                    NumberingDefinitionsPart numberinfPart = mainPart.AddNewPart<NumberingDefinitionsPart>("numbering");
                    Numbering element = new Numbering(new AbstractNum(new Level(new NumberingFormat() { Val = NumberFormatValues.Bullet }, new LevelText() { Val = "•" }, new LevelJustification { Val = LevelJustificationValues.Left }, new ParagraphProperties(new Indentation { Left = "720", Hanging = "360" })) { LevelIndex = 0 }) { AbstractNumberId = 0 }, new NumberingInstance(new AbstractNumId() { Val = 0 }) { NumberID = 1 });
                    element.Save(numberinfPart);

                    Color orange = new Color() { Val = "ED7D27" };

                    //Your Risk Protection

                    var clientCurrentInsurance = documentDetails.currentInsurance.Where(x => x.Owner == "Client");
                    var partnerCurrentInsurance = documentDetails.currentInsurance.Where(x => x.Owner == "Partner");

                    if (clientCurrentInsurance.Count() > 0 || partnerCurrentInsurance.Count() > 0)
                    {
                        if (clientCurrentInsurance.Count() > 0)
                        {
                            AddRiskProtection(body, documentDetails.clientDetails, clientCurrentInsurance, orange, 0);
                        }

                        if (partnerCurrentInsurance.Count() > 0)
                        {
                            AddRiskProtection(body, documentDetails.clientDetails, partnerCurrentInsurance, orange , 1);
                        }
                    }

                    Paragraph break2 = new Paragraph(new Run(new Break() { Type = BreakValues.Page }));
                    body.Append(break2);

                    //Paragraph pr0 = new Paragraph();
                    //add(pr0);
                    //body.Append(pr0);

                    GenerateBody(body);
                    Run br1 = new Run(new Break());
                    Paragraph pr1 = new Paragraph();
                    pr1.Append(new OpenXmlElement[] { br1 });
                    body.Append(pr1);

                    AddPara1(body);
                    //Run br2 = new Run(new Break());
                    Paragraph pr2 = new Paragraph();
                    //pr2.Append(new OpenXmlElement[] { br2 });
                    body.Append(pr2);
                    AddPara2(body);

                    //Your Insurance Needs

                    Paragraph break3 = new Paragraph(new Run(new Break() { Type = BreakValues.Page }));
                    body.Append(break3);

                    var clientNeedsAnalysis = documentDetails.needsAnalysis.Where(x => x.Owner == "Client");
                    var partnerNeedsAnalysis = documentDetails.needsAnalysis.Where(x => x.Owner == "Partner");

                    if(clientNeedsAnalysis.Count() > 0 || partnerNeedsAnalysis.Count() > 0)
                    {
                        Paragraph InsuranceNeeds = new Paragraph(new Run(new RunProperties(new Color() { Val = "ED7D27" }, new FontSize { Val = "40" }), new Text("Your Insurance Needs")));
                        body.Append(InsuranceNeeds);

                      
                        if (clientNeedsAnalysis.Count() > 0)
                        {
                            AddInsuranceNeeds(body, clientNeedsAnalysis.ToArray(), orange , documentDetails.clientDetails.ClientName);
                        }

                        if (partnerNeedsAnalysis.Count() > 0 && documentDetails.clientDetails.MaritalStatus != "S")
                        {
                            AddInsuranceNeeds(body, partnerNeedsAnalysis.ToArray(), orange, documentDetails.clientDetails.PartnerName);
                        }

                    }


                    //Recommended Insurance
                    body.AppendChild(break3.CloneNode(true));
                    var clientProposedInsurance = documentDetails.proposedInsurance.Where(x => x.Owner == "Client");
                    var partnerProposedInsurance = documentDetails.proposedInsurance.Where(x => x.Owner == "Partner");

                    if (clientProposedInsurance.Count() > 0 || partnerProposedInsurance.Count() > 0)
                    {
                        if (clientProposedInsurance.Count() > 0)
                        {
                            AddRecommendedInsurance(body, documentDetails.clientDetails, clientProposedInsurance, orange, 0);
                        }

                        if (partnerProposedInsurance.Count() > 0)
                        {
                            AddRecommendedInsurance(body, documentDetails.clientDetails, partnerProposedInsurance, orange, 1);
                        }


                        AddStandardText(body,orange);
                    }

                    //Insurance ROP

                    AddReplacementOfProduct(body, documentDetails.currentInsurance, documentDetails.proposedInsurance, documentDetails.clientDetails, orange);

                    Paragraph break4 = new Paragraph(new Run(new Break() { Type = BreakValues.Page }));
                    body.Append(break4);


                    AddImplementation(body,documentDetails.proposedInsurance, documentDetails.clientDetails, orange);

                    //Paragraph break4 = new Paragraph(new Run(new Break() { Type = BreakValues.Page }));
                    //body.Append(break4);


                    //if (clientProposedInsurance.Count() > 0 || partnerProposedInsurance.Count() > 0)
                    //{
                    //    Paragraph InsuranceNeeds = new Paragraph(new Run(new RunProperties(new Color() { Val = "ED7D27" }, new FontSize { Val = "40" }), new Text("Insurance replacement of product")));
                    //    body.Append(InsuranceNeeds);


                    //    if (clientNeedsAnalysis.Count() > 0)
                    //    {
                    //        AddInsuranceNeeds(body, clientNeedsAnalysis.ToArray(), orange, documentDetails.clientDetails.ClientName);
                    //    }

                    //    if (partnerNeedsAnalysis.Count() > 0 && documentDetails.clientDetails.MaritalStatus != "S")
                    //    {
                    //        AddInsuranceNeeds(body, partnerNeedsAnalysis.ToArray(), orange, documentDetails.clientDetails.PartnerName);
                    //    }

                    //}

                    //Footer
                    var t = mainPart.Document.Body.Descendants<Table>().ToList();
                    var p = mainPart.Document.Body.Descendants<Paragraph>().ToList();
                    AlterTableType(t, p, package);

                    FooterPart footerPart = mainPart.AddNewPart<FooterPart>();

                    string footerPartId = mainPart.GetIdOfPart(footerPart);
                    GeneratePartContent(footerPart);

                    IEnumerable<SectionProperties> sections = mainPart.Document.Body.Elements<SectionProperties>();
                    foreach (var section in sections)
                    {
                        section.RemoveAllChildren<FooterReference>();
                        section.PrependChild<FooterReference>(new FooterReference() { Id = footerPartId });
                    }


                    package.MainDocumentPart.Document.Save();
                    package.Close();

                }


                ms.Position = 0;
                await blockBlob.UploadFromStreamAsync(ms);


                return File(ms.ToArray(), "application/octet-stream", "test");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void AddImplementation(Body body, ProposedInsuranceViewModel[] proposedInsurance, BasicDetails clientDetails, Color orange)
        {
            
            var implementation = new List<ProposedInsuranceViewModel>();
            var ongoing = new List<ProposedInsuranceViewModel>();

            foreach (ProposedInsuranceViewModel pI in proposedInsurance)
            {
                if (pI.Implementation.Commission > 0)
                {
                    implementation.Add(pI);
                }
                if (pI.Ongoing.Commission > 0)
                {
                    ongoing.Add(pI);
                }
            }

            if(implementation.Count() > 0 || ongoing.Count() > 0)
            {
               
                if (implementation.Any())
                    {

                    Paragraph s1 = body.AppendChild(new Paragraph());
                    Run r1 = s1.AppendChild(new Run());
                    RunProperties runProperties1 = r1.AppendChild(new RunProperties(new Bold(), new RunFonts { Ascii = "Verdana" }, new FontSize { Val = new StringValue("20") }));
                    Color black = new Color() { Val = "000000" };
                    runProperties1.AppendChild(orange.CloneNode(true));
                    r1.AppendChild(new Text("Implementation"));

                    Paragraph p1 = body.AppendChild(new Paragraph(new Run(new RunProperties(new Color() { Val = "000000" }), new Text("If you wish to proceed with our advice, the following table shows the costs that apply to implement our recommendations."))));

                    //New Table

                    Table table = body.AppendChild(new Table());
                        TableProperties tableProp = new TableProperties();
                        TableStyle tableStyle = new TableStyle() { Val = "TableGrid" };

                        TableWidth tableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };

                        tableProp.Append(tableStyle, tableWidth);
                        table.AppendChild(tableProp);


                        ////Header
                        TableRow type = new TableRow();

                        TableCellProperties hcp = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                        TableCellProperties hcp1 = new TableCellProperties(new TableCellWidth { Width = "2390", Type = TableWidthUnitValues.Auto }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                        TableCellProperties hcp2 = new TableCellProperties(new TableCellWidth { Width = "2390", Type = TableWidthUnitValues.Auto }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });

                        ParagraphProperties hpp = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });
                        ParagraphProperties hpp1 = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });

                        Justification centerJustify = new Justification() { Val = JustificationValues.Center };
                        hpp1.AppendChild((Justification)centerJustify.CloneNode(true));

                        TableCellBorders hcb = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 20, Color = "ED7D27" } };

                        Shading hs1 = new Shading() { Color = "auto", Fill = "393939", Val = ShadingPatternValues.Clear };
                        TableCellMargin hcm1 = new TableCellMargin() { RightMargin = new RightMargin() { Width = "50", Type = TableWidthUnitValues.Dxa }, LeftMargin = new LeftMargin() { Width = "50", Type = TableWidthUnitValues.Dxa } };

                        hcp.Append(hcm1, hs1, hcb);
                        hcp1.Append((TableCellMargin)hcm1.CloneNode(true), (Shading)hs1.CloneNode(true), (TableCellBorders)hcb.CloneNode(true));
                        hcp2.Append((TableCellMargin)hcm1.CloneNode(true), (Shading)hs1.CloneNode(true));

                        TableCell blank = new TableCell();
                        blank.AppendChild((ParagraphProperties)hpp.CloneNode(true));
                        blank.Append((TableCellProperties)hcp2.CloneNode(true));
                        blank.Append(new Paragraph(new Run(new RunProperties(new Bold(), new FontSize { Val = new StringValue("16") }), new Text("Description"))));
                        type.Append(blank);


                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                            h.Append((TableCellProperties)hcp2.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(new Bold(), new FontSize { Val = new StringValue("16") }), new Text("Cost to you"))));
                            type.Append(h);
                      
                            TableCell h1 = new TableCell();
                            h1.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                            h1.Append((TableCellProperties)hcp2.CloneNode(true));
                            h1.Append(new Paragraph(new Run(new RunProperties(new Bold(), new FontSize { Val = new StringValue("16") }), new Text("Calculation of amounts received by us"))));
                            type.Append(h1);

                            TableCell h2 = new TableCell();
                            h2.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                            h2.Append((TableCellProperties)hcp2.CloneNode(true));
                            h2.Append(new Paragraph(new Run(new RunProperties(new Bold(), new FontSize { Val = new StringValue("16") }), new Text("RI Advice Group"))));
                            type.Append(h2);

                            TableCell h3 = new TableCell();
                            h3.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                            h3.Append((TableCellProperties)hcp2.CloneNode(true));
                            h3.Append(new Paragraph(new Run(new RunProperties(new Bold(), new FontSize { Val = new StringValue("16") }), new Text("Practice"))));
                            type.Append(h3);

                            TableCell h4 = new TableCell();
                            h4.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                            h4.Append((TableCellProperties)hcp2.CloneNode(true));
                            h4.Append(new Paragraph(new Run(new RunProperties(new Bold(), new FontSize { Val = new StringValue("16") }), new Text("Adviser"))));
                            type.Append(h4);

                            table.Append(type);


                        //Body

                        TableCellProperties tcp1 = new TableCellProperties(new TableCellWidth { Width = "2390", Type = TableWidthUnitValues.Auto }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                        TableCellProperties tcp = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                        TableCellProperties tcpN = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });

                        TableCellBorders tcbL = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, RightBorder = new RightBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };
                        TableCellBorders tcbR = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, LeftBorder = new LeftBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };
                        TableCellBorders tcbN = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };

                        TableCellMargin tcm = new TableCellMargin() { RightMargin = new RightMargin() { Width = "50", Type = TableWidthUnitValues.Dxa }, LeftMargin = new LeftMargin() { Width = "50", Type = TableWidthUnitValues.Dxa } };

                        ParagraphProperties pp = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });
                        ParagraphProperties pp1 = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });

                      
                    pp1.AppendChild((Justification)centerJustify.CloneNode(true));
                    tcp.Append(tcbL, tcm);
                        tcpN.Append((TableCellBorders)tcbN.CloneNode(true), (TableCellMargin)tcm.CloneNode(true));

                    decimal totalFeeCost = 0;
                    decimal totalAdviceGroup = 0;
                    decimal totalPractice = 0;

                        foreach (ProposedInsuranceViewModel proposed in implementation)
                        {

                        TableRow heading = new TableRow();
                        TableCell bACell = new TableCell();
                        TableCellProperties tableCellProperties9 = new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }, new GridSpan() { Val = 6 });
                        tableCellProperties9.Append((TableCellBorders)tcbN.CloneNode(true), (TableCellMargin)tcm.CloneNode(true));
                        bACell.Append(tableCellProperties9);
                        bACell.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        bACell.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(proposed.Owner == "Client" ? clientDetails.ClientName + " - " + proposed.Provider : clientDetails.PartnerName + " - " + proposed.Provider))));
                        heading.Append(bACell);
                        table.Append(heading);

                        TableRow impDetails = new TableRow();

                        TableCell tableCell1 = new TableCell();
                        tableCell1.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tableCell1.Append((TableCellProperties)tcp.CloneNode(true));
                        tableCell1.Append(new Paragraph(new Run(new RunProperties(), new Text("Premium paid by you your super fund to insurer. Commission paid by insurer to us, after implementation of strategy"))));
                        impDetails.Append(tableCell1);

                        var feeSum = proposed.FeeDetails.Sum(x => x.Amount);
                        //verify if error
                        var adviceGroup = feeSum * (proposed.Implementation.Commission  / 100 ) * ( proposed.Implementation.Riadvice/100);
                        var practice = feeSum * (proposed.Implementation.Commission / 100) * (proposed.Implementation.Practice / 100);
                        var adviser = proposed.Implementation.Adviser == 0 ? "See note above" : string.Format("{0:n0}",feeSum * (proposed.Implementation.Commission / 100) * (proposed.Implementation.Adviser / 100));


                        totalFeeCost += feeSum;
                        totalAdviceGroup += adviceGroup;
                        totalPractice += practice;


                        TableCell tableCell2 = new TableCell();
                        tableCell2.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell2.Append((TableCellProperties)tcp.CloneNode(true));
                        tableCell2.Append(new Paragraph(new Run(new RunProperties(), new Text(string.Format("{0:n0}",feeSum)))));
                        impDetails.Append(tableCell2);

                        TableCell tableCell3 = new TableCell();
                        tableCell3.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell3.Append((TableCellProperties)tcp.CloneNode(true));
                        tableCell3.Append(new Paragraph(new Run(new RunProperties(new FontSize { Val = new StringValue("16") }), new Text( proposed.Implementation.Commission + "% of commissionable premium"))));
                        impDetails.Append(tableCell3);

                       

                        TableCell tableCell4 = new TableCell();
                        tableCell4.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell4.Append((TableCellProperties)tcp.CloneNode(true));
                        tableCell4.Append(new Paragraph(new Run(new RunProperties(), new Text(string.Format("{0:n0}", adviceGroup)))));
                        impDetails.Append(tableCell4);

                        TableCell tableCell5 = new TableCell();
                        tableCell5.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell5.Append((TableCellProperties)tcp.CloneNode(true));
                        tableCell5.Append(new Paragraph(new Run(new RunProperties(), new Text(string.Format("{0:n0}", practice)))));
                        impDetails.Append(tableCell5);

                        TableCell tableCell6 = new TableCell();
                        tableCell6.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell6.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell6.Append(new Paragraph(new Run(new RunProperties(), new Text(adviser))));
                        impDetails.Append(tableCell6);

                        table.Append(impDetails);


                    }


                    TableRow totalRow = new TableRow();

                    TableCell total1 = new TableCell();
                    total1.AppendChild((ParagraphProperties)pp.CloneNode(true));
                    total1.Append((TableCellProperties)tcp.CloneNode(true));
                    total1.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Totals"))));
                    totalRow.Append(total1);

                    TableCell total2 = new TableCell();
                    total2.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                    total2.Append((TableCellProperties)tcp.CloneNode(true));
                    total2.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("$" + string.Format("{0:n0}", totalFeeCost)))));
                    totalRow.Append(total2);

                    TableCell total3 = new TableCell();
                    total3.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                    total3.Append((TableCellProperties)tcp.CloneNode(true));
                    total3.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(""))));
                    totalRow.Append(total3);

                    TableCell total4 = new TableCell();
                    total4.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                    total4.Append((TableCellProperties)tcp.CloneNode(true));
                    total4.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("$" + string.Format("{0:n0}", totalAdviceGroup)))));
                    totalRow.Append(total4);

                    TableCell total5 = new TableCell();
                    total5.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                    total5.Append((TableCellProperties)tcp.CloneNode(true));
                    total5.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("$" + string.Format("{0:n0}", totalPractice)))));
                    totalRow.Append(total5);

                    TableCell total6 = new TableCell();
                    total6.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                    total6.Append((TableCellProperties)tcpN.CloneNode(true));
                    total6.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(""))));
                    totalRow.Append(total6);


                    table.Append(totalRow);
                }

                if (ongoing.Any())
                {

                    Run br2 = new Run(new Break());
                    Paragraph pr2 = new Paragraph();
                    pr2.Append(new OpenXmlElement[] { br2 });
                    body.Append(pr2);

                    Paragraph s1 = body.AppendChild(new Paragraph());
                    Run r1 = s1.AppendChild(new Run());
                    RunProperties runProperties1 = r1.AppendChild(new RunProperties(new Bold(), new RunFonts { Ascii = "Verdana" }, new FontSize { Val = new StringValue("20") }));
                    Color black = new Color() { Val = "000000" };
                    runProperties1.AppendChild(orange.CloneNode(true));
                    r1.AppendChild(new Text("Ongoing advice"));

                    Paragraph p1 = body.AppendChild(new Paragraph(new Run(new RunProperties(new Color() { Val = "000000" }), new Text("The following table shows the ongoing costs. All fees are shown as annual amounts."))));

                    //New Table

                    Table table = body.AppendChild(new Table());
                    TableProperties tableProp = new TableProperties();
                    TableStyle tableStyle = new TableStyle() { Val = "TableGrid" };

                    TableWidth tableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };

                    tableProp.Append(tableStyle, tableWidth);
                    table.AppendChild(tableProp);


                    ////Header
                    TableRow type = new TableRow();

                    TableCellProperties hcp = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                    TableCellProperties hcp1 = new TableCellProperties(new TableCellWidth { Width = "2390", Type = TableWidthUnitValues.Auto }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                    TableCellProperties hcp2 = new TableCellProperties(new TableCellWidth { Width = "2390", Type = TableWidthUnitValues.Auto }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });

                    ParagraphProperties hpp = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });
                    ParagraphProperties hpp1 = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });

                    Justification centerJustify = new Justification() { Val = JustificationValues.Center };
                    hpp1.AppendChild((Justification)centerJustify.CloneNode(true));

                    TableCellBorders hcb = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 20, Color = "ED7D27" } };

                    Shading hs1 = new Shading() { Color = "auto", Fill = "393939", Val = ShadingPatternValues.Clear };
                    TableCellMargin hcm1 = new TableCellMargin() { RightMargin = new RightMargin() { Width = "50", Type = TableWidthUnitValues.Dxa }, LeftMargin = new LeftMargin() { Width = "50", Type = TableWidthUnitValues.Dxa } };

                    hcp.Append(hcm1, hs1, hcb);
                    hcp1.Append((TableCellMargin)hcm1.CloneNode(true), (Shading)hs1.CloneNode(true), (TableCellBorders)hcb.CloneNode(true));
                    hcp2.Append((TableCellMargin)hcm1.CloneNode(true), (Shading)hs1.CloneNode(true));

                    TableCell blank = new TableCell();
                    blank.AppendChild((ParagraphProperties)hpp.CloneNode(true));
                    blank.Append((TableCellProperties)hcp2.CloneNode(true));
                    blank.Append(new Paragraph(new Run(new RunProperties(new Bold(), new FontSize { Val = new StringValue("16") }), new Text("Description"))));
                    type.Append(blank);


                    TableCell h = new TableCell();
                    h.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                    h.Append((TableCellProperties)hcp2.CloneNode(true));
                    h.Append(new Paragraph(new Run(new RunProperties(new Bold(), new FontSize { Val = new StringValue("16") }), new Text("Cost to you"))));
                    type.Append(h);

                    TableCell h1 = new TableCell();
                    h1.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                    h1.Append((TableCellProperties)hcp2.CloneNode(true));
                    h1.Append(new Paragraph(new Run(new RunProperties(new Bold(), new FontSize { Val = new StringValue("16") }), new Text("Calculation of amounts received by us"))));
                    type.Append(h1);

                    TableCell h2 = new TableCell();
                    h2.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                    h2.Append((TableCellProperties)hcp2.CloneNode(true));
                    h2.Append(new Paragraph(new Run(new RunProperties(new Bold(), new FontSize { Val = new StringValue("16") }), new Text("RI Advice Group"))));
                    type.Append(h2);

                    TableCell h3 = new TableCell();
                    h3.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                    h3.Append((TableCellProperties)hcp2.CloneNode(true));
                    h3.Append(new Paragraph(new Run(new RunProperties(new Bold(), new FontSize { Val = new StringValue("16") }), new Text("Practice"))));
                    type.Append(h3);

                    TableCell h4 = new TableCell();
                    h4.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                    h4.Append((TableCellProperties)hcp2.CloneNode(true));
                    h4.Append(new Paragraph(new Run(new RunProperties(new Bold(), new FontSize { Val = new StringValue("16") }), new Text("Adviser"))));
                    type.Append(h4);

                    table.Append(type);


                    //Body

                    TableCellProperties tcp1 = new TableCellProperties(new TableCellWidth { Width = "2390", Type = TableWidthUnitValues.Auto }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                    TableCellProperties tcp = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                    TableCellProperties tcpN = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });

                    TableCellBorders tcbL = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, RightBorder = new RightBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };
                    TableCellBorders tcbR = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, LeftBorder = new LeftBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };
                    TableCellBorders tcbN = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };

                    TableCellMargin tcm = new TableCellMargin() { RightMargin = new RightMargin() { Width = "50", Type = TableWidthUnitValues.Dxa }, LeftMargin = new LeftMargin() { Width = "50", Type = TableWidthUnitValues.Dxa } };

                    ParagraphProperties pp = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });
                    ParagraphProperties pp1 = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });


                    pp1.AppendChild((Justification)centerJustify.CloneNode(true));
                    tcp.Append(tcbL, tcm);
                    tcpN.Append((TableCellBorders)tcbN.CloneNode(true), (TableCellMargin)tcm.CloneNode(true));

                    decimal totalFeeCost = 0;
                    decimal totalAdviceGroup = 0;
                    decimal totalPractice = 0;

                    foreach (ProposedInsuranceViewModel proposed in ongoing)
                    {

                        TableRow heading = new TableRow();
                        TableCell bACell = new TableCell();
                        TableCellProperties tableCellProperties9 = new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }, new GridSpan() { Val = 6 });
                        tableCellProperties9.Append((TableCellBorders)tcbN.CloneNode(true), (TableCellMargin)tcm.CloneNode(true));
                        bACell.Append(tableCellProperties9);
                        bACell.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        bACell.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(proposed.Owner == "Client" ? clientDetails.ClientName + " - " + proposed.Provider : clientDetails.PartnerName + " - " + proposed.Provider))));
                        heading.Append(bACell);
                        table.Append(heading);

                        TableRow impDetails = new TableRow();

                        TableCell tableCell1 = new TableCell();
                        tableCell1.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tableCell1.Append((TableCellProperties)tcp.CloneNode(true));
                        tableCell1.Append(new Paragraph(new Run(new RunProperties(), new Text("Premium paid by you your super fund to insurer. Commission paid by insurer to us, after implementation of strategy"))));
                        impDetails.Append(tableCell1);

                        var feeSum = proposed.FeeDetails.Sum(x => x.Amount);
                        //verify if error
                        var adviceGroup = feeSum * (proposed.Implementation.Commission / 100) * (proposed.Implementation.Riadvice / 100);
                        var practice = feeSum * (proposed.Implementation.Commission / 100) * (proposed.Implementation.Practice / 100);
                        var adviser = proposed.Implementation.Adviser == 0 ? "See note above" : string.Format("{0:n0}", feeSum * (proposed.Implementation.Commission / 100) * (proposed.Implementation.Adviser / 100));


                        totalFeeCost += feeSum;
                        totalAdviceGroup += adviceGroup;
                        totalPractice += practice;


                        TableCell tableCell2 = new TableCell();
                        tableCell2.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell2.Append((TableCellProperties)tcp.CloneNode(true));
                        tableCell2.Append(new Paragraph(new Run(new RunProperties(), new Text(string.Format("{0:n0}", feeSum)))));
                        impDetails.Append(tableCell2);

                        TableCell tableCell3 = new TableCell();
                        tableCell3.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell3.Append((TableCellProperties)tcp.CloneNode(true));
                        tableCell3.Append(new Paragraph(new Run(new RunProperties(new FontSize { Val = new StringValue("16") }), new Text(proposed.Implementation.Commission + "% of commissionable premium"))));
                        impDetails.Append(tableCell3);



                        TableCell tableCell4 = new TableCell();
                        tableCell4.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell4.Append((TableCellProperties)tcp.CloneNode(true));
                        tableCell4.Append(new Paragraph(new Run(new RunProperties(), new Text(string.Format("{0:n0}", adviceGroup)))));
                        impDetails.Append(tableCell4);

                        TableCell tableCell5 = new TableCell();
                        tableCell5.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell5.Append((TableCellProperties)tcp.CloneNode(true));
                        tableCell5.Append(new Paragraph(new Run(new RunProperties(), new Text(string.Format("{0:n0}", practice)))));
                        impDetails.Append(tableCell5);

                        TableCell tableCell6 = new TableCell();
                        tableCell6.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell6.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell6.Append(new Paragraph(new Run(new RunProperties(), new Text(adviser))));
                        impDetails.Append(tableCell6);

                        table.Append(impDetails);


                    }


                    TableRow totalRow = new TableRow();

                    TableCell total1 = new TableCell();
                    total1.AppendChild((ParagraphProperties)pp.CloneNode(true));
                    total1.Append((TableCellProperties)tcp.CloneNode(true));
                    total1.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Totals"))));
                    totalRow.Append(total1);

                    TableCell total2 = new TableCell();
                    total2.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                    total2.Append((TableCellProperties)tcp.CloneNode(true));
                    total2.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("$" + string.Format("{0:n0}", totalFeeCost)))));
                    totalRow.Append(total2);

                    TableCell total3 = new TableCell();
                    total3.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                    total3.Append((TableCellProperties)tcp.CloneNode(true));
                    total3.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(""))));
                    totalRow.Append(total3);

                    TableCell total4 = new TableCell();
                    total4.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                    total4.Append((TableCellProperties)tcp.CloneNode(true));
                    total4.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("$" + string.Format("{0:n0}", totalAdviceGroup)))));
                    totalRow.Append(total4);

                    TableCell total5 = new TableCell();
                    total5.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                    total5.Append((TableCellProperties)tcp.CloneNode(true));
                    total5.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("$" + string.Format("{0:n0}", totalPractice)))));
                    totalRow.Append(total5);

                    TableCell total6 = new TableCell();
                    total6.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                    total6.Append((TableCellProperties)tcpN.CloneNode(true));
                    total6.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(""))));
                    totalRow.Append(total6);


                    table.Append(totalRow);
                }
            }


        }

        private void AddStandardText(Body body, Color orange)
        {
            Paragraph s1 = body.AppendChild(new Paragraph());
            Run r1 = s1.AppendChild(new Run());
            RunProperties runProperties1 = r1.AppendChild(new RunProperties(new Bold(), new RunFonts { Ascii = "Verdana" }, new FontSize { Val = new StringValue("20") }));
            Color black = new Color() { Val = "000000" };
            runProperties1.AppendChild(orange.CloneNode(true));
            r1.AppendChild(new Text("Other important information"));

            Paragraph p1 = body.AppendChild(new Paragraph(new Run(new RunProperties(new Color() { Val = "000000" }), new Text("The recommended insurance cover may have exclusions applied, be loaded (higher premiums) or be declined depending on the outcome of underwriting."))));
            Paragraph p2 = body.AppendChild(new Paragraph(new Run(new RunProperties(new Color() { Val = "000000" }), new Text("A death benefit will not be paid in the first 13 months of your life policy in the event of death by suicide."))));
            Paragraph p3 = body.AppendChild(new Paragraph(new Run(new RunProperties(new Color() { Val = "000000" }), new Text("Your policy has a cooling off period which allows you to cancel your cover and receive a refund of premiums paid. Please refer to the PDS for details."))));


            Paragraph s2 = body.AppendChild(new Paragraph());
            Run r2 = s2.AppendChild(new Run());
            RunProperties runProperties2 = r2.AppendChild(new RunProperties(new Bold(), new RunFonts { Ascii = "Verdana" }, new FontSize { Val = new StringValue("20") }));
          
            runProperties2.AppendChild(orange.CloneNode(true));
            r2.AppendChild(new Text("Your duty of disclosure"));

            Paragraph p5 = body.AppendChild(new Paragraph(new Run(new RunProperties(new Color() { Val = "000000" }), new Text("Before you apply for, extend, renew, vary or reinstate your insurance, it is important that you understand your duty of disclosure. The key elements are:"))));

            Paragraph commentary = body.AppendChild(new Paragraph(new ParagraphProperties(new ParagraphStyleId() { Val = "ListParagraph" }, new NumberingProperties(new NumberingLevelReference() { Val = 0 }, new NumberingId() { Val = 1 })), new Run(new RunProperties(), new Text("You have a duty under law to disclose to the insurer every matter you know or could reasonably be expected to know that is relevant to their decision to accept your insurance application."))));
            Paragraph commentary1 = body.AppendChild(new Paragraph(new ParagraphProperties(new ParagraphStyleId() { Val = "ListParagraph" }, new NumberingProperties(new NumberingLevelReference() { Val = 0 }, new NumberingId() { Val = 1 })), new Run(new RunProperties(), new Text("You must provide all the information requested by the insurer. Your duty could also extend beyond the questions in the insurance application form."))));
            Paragraph commentary2 = body.AppendChild(new Paragraph(new ParagraphProperties(new ParagraphStyleId() { Val = "ListParagraph" }, new NumberingProperties(new NumberingLevelReference() { Val = 0 }, new NumberingId() { Val = 1 })), new Run(new RunProperties(), new Text("Your duty continues until the insurance application is accepted. You will need to disclose any relevant changes in your circumstances that occur before the insurance policy is issued."))));
            Paragraph commentary3 = body.AppendChild(new Paragraph(new ParagraphProperties(new ParagraphStyleId() { Val = "ListParagraph" }, new NumberingProperties(new NumberingLevelReference() { Val = 0 }, new NumberingId() { Val = 1 })), new Run(new RunProperties(), new Text("If you do not comply with your duty of disclosure, in some circumstances, your insurance may be avoided or varied, or future claims may be affected or denied. "))));

            Paragraph p6 = body.AppendChild(new Paragraph(new Run(new RunProperties(new Color() { Val = "000000" }), new Text("Please refer to the relevant PDS for full details about your duty of disclosure."))));

            //Paragraph br = body.AppendChild(new Paragraph(new Run(new Break())));

            Paragraph s3 = body.AppendChild(new Paragraph());
            Run r3 = s3.AppendChild(new Run());
            RunProperties runProperties3 = r3.AppendChild(new RunProperties(new Bold(), new RunFonts { Ascii = "Verdana" }, new FontSize { Val = new StringValue("20") }));
           
            runProperties3.AppendChild(orange.CloneNode(true));
            r3.AppendChild(new Text("Product Disclosure Statement of retained policies"));

            Color red = new Color() { Val = "FF0000" };

            Paragraph p7 = new Paragraph(new Run(new RunProperties(new Color() { Val = "000000" }), new Text("Please read the following Product Disclosure Statement (PDS) for further information on your retained insurance ")));
            p7.Append(new Run(new RunProperties((Color)red.CloneNode(true)), new Text { Text = " [policy/ies:] ", Space = SpaceProcessingModeValues.Preserve }));
            body.Append(p7);

            Paragraph commentary4 = body.AppendChild(new Paragraph(new ParagraphProperties(new ParagraphStyleId() { Val = "ListParagraph" }, new NumberingProperties(new NumberingLevelReference() { Val = 0 }, new NumberingId() { Val = 1 })), new Run(new RunProperties((Color)red.CloneNode(true)), new Text("[PDS name here]"))));

        }

        private void AddStandardTextROP(Body body, Color orange)
        {
            Paragraph s1 = body.AppendChild(new Paragraph());
            Run r1 = s1.AppendChild(new Run());
            RunProperties runProperties1 = r1.AppendChild(new RunProperties(new Bold(), new RunFonts { Ascii = "Verdana" }, new FontSize { Val = new StringValue("20") }));
            Color black = new Color() { Val = "000000" };
            runProperties1.AppendChild(orange.CloneNode(true));
            Color red = new Color() { Val = "FF0000" };
            r1.AppendChild(new Text("Product replacement recommendations"));


            Paragraph commentary = body.AppendChild(new Paragraph(new ParagraphProperties(new ParagraphStyleId() { Val = "ListParagraph" }, new NumberingProperties(new NumberingLevelReference() { Val = 0 }, new NumberingId() { Val = 1 })), new Run(new RunProperties(), new Text("Based on our research and your needs and objectives, our recommendation is to replace your current insurance product."))));
            Paragraph commentary1 = body.AppendChild(new Paragraph(new ParagraphProperties(new ParagraphStyleId() { Val = "ListParagraph" }, new NumberingProperties(new NumberingLevelReference() { Val = 0 }, new NumberingId() { Val = 1 })), new Run(new RunProperties((Color)red.CloneNode(true)), new Text("[Recommended product]"))));
            commentary1.Append(new Run(new RunProperties(), new Text { Text = " was deemed appropriate for you because: ", Space = SpaceProcessingModeValues.Preserve }));

            Paragraph commentary2 = body.AppendChild(new Paragraph(new ParagraphProperties(new ParagraphStyleId() { Val = "ListParagraph" }, new Indentation() { Left = "1080" }, new NumberingProperties(new NumberingLevelReference() { Val = 0 }, new NumberingId() { Val = 1 })), new Run(new RunProperties((Color)red.CloneNode(true)), new Text("[If superior features]"))));
            commentary2.Append(new Run(new RunProperties(), new Text { Text = " It provides you with additional features. ", Space = SpaceProcessingModeValues.Preserve }));

            Paragraph commentary3 = body.AppendChild(new Paragraph(new ParagraphProperties(new ParagraphStyleId() { Val = "ListParagraph" }, new Indentation() { Left = "1080" }, new NumberingProperties(new NumberingLevelReference() { Val = 0 }, new NumberingId() { Val = 1 })), new Run(new RunProperties((Color)red.CloneNode(true)), new Text("[If lower premiums]"))));
            commentary3.Append(new Run(new RunProperties(), new Text { Text = " You will benefit from lower premiums. ", Space = SpaceProcessingModeValues.Preserve }));


            Paragraph commentary6 = body.AppendChild(new Paragraph(new ParagraphProperties(new ParagraphStyleId() { Val = "ListParagraph" }, new NumberingProperties(new NumberingLevelReference() { Val = 0 }, new NumberingId() { Val = 1 })), new Run(new RunProperties(), new Text("In making this recommendation, we compared "))));
            commentary6.Append(new Run(new RunProperties((Color)red.CloneNode(true)), new Text { Text = " recommended product ", Space = SpaceProcessingModeValues.Preserve }));
            commentary6.Append(new Run(new RunProperties(), new Text { Text = "to the following other products in the market: ", Space = SpaceProcessingModeValues.Preserve }));

            Paragraph commentary7 = body.AppendChild(new Paragraph(new ParagraphProperties(new ParagraphStyleId() { Val = "ListParagraph" }, new Indentation() { Left = "1080" }, new NumberingProperties(new NumberingLevelReference() { Val = 0 }, new NumberingId() { Val = 1 })), new Run(new RunProperties((Color)red.CloneNode(true)), new Text("[Alternative product 1]"))));
            commentary7.Append(new Run(new RunProperties(), new Text { Text = " could ", Space = SpaceProcessingModeValues.Preserve }));
            commentary7.Append(new Run(new RunProperties((Color)red.CloneNode(true)), new Text { Text = "[save you $XXX in annual costs and you would benefit from XXX features.]  ", Space = SpaceProcessingModeValues.Preserve }));
            commentary7.Append(new Run(new RunProperties(), new Text { Text = "However we disregarded this product because ", Space = SpaceProcessingModeValues.Preserve }));
            commentary7.Append(new Run(new RunProperties((Color)red.CloneNode(true)), new Text { Text = "[you specifically stated you wanted the XXX product.] ", Space = SpaceProcessingModeValues.Preserve }));

            Paragraph commentary8 = body.AppendChild(new Paragraph(new ParagraphProperties(new ParagraphStyleId() { Val = "ListParagraph" }, new Indentation() { Left = "1080" }, new NumberingProperties(new NumberingLevelReference() { Val = 0 }, new NumberingId() { Val = 1 })), new Run(new RunProperties((Color)red.CloneNode(true)), new Text("[Alternative product 2]"))));
            commentary8.Append(new Run(new RunProperties(), new Text { Text = " offers ", Space = SpaceProcessingModeValues.Preserve }));
            commentary8.Append(new Run(new RunProperties((Color)red.CloneNode(true)), new Text { Text = "[XXX features which could benefit you]", Space = SpaceProcessingModeValues.Preserve }));
            commentary8.Append(new Run(new RunProperties(), new Text { Text = ", however we disregarded this product because ", Space = SpaceProcessingModeValues.Preserve }));
            commentary8.Append(new Run(new RunProperties((Color)red.CloneNode(true)), new Text { Text = "[the annual costs are around $XXX more than XXX recommended product and this is more than you want to pay.] ", Space = SpaceProcessingModeValues.Preserve }));


            Paragraph s3 = body.AppendChild(new Paragraph());
            Run r3 = s3.AppendChild(new Run());
            RunProperties runProperties3 = r3.AppendChild(new RunProperties(new Bold(), new RunFonts { Ascii = "Verdana" }, new FontSize { Val = new StringValue("20") }));

           // runProperties3.AppendChild(orange.CloneNode(true));
            r3.AppendChild(new Text("Important note:"));

            Paragraph commentary4 = body.AppendChild(new Paragraph(new ParagraphProperties(new ParagraphStyleId() { Val = "ListParagraph" }, new NumberingProperties(new NumberingLevelReference() { Val = 0 }, new NumberingId() { Val = 1 })), new Run(new RunProperties(), new Text("You should maintain your existing cover until the new policy is established, and all waiting periods are served, to ensure you have continuous cover. This could mean you are paying multiple premiums for a period of time. Once your new policy is in place and all waiting periods have been satisfied, the old policy can be cancelled."))));
            Paragraph commentary5 = new Paragraph(new ParagraphProperties(new ParagraphStyleId() { Val = "ListParagraph" }, new NumberingProperties(new NumberingLevelReference() { Val = 0 }, new NumberingId() { Val = 1 })), new Run(new RunProperties(), new Text("A policy that has been in force for three years or longer may have a greater level of protection against avoidance by an insurer if the duty of disclosure is breached or innocent misrepresentation occurs.")));
            commentary5.Append(new Run(new RunProperties(new Bold()), new Text { Text = " Cancelling your existing cover to take out new cover will restart this three year period where the insurer could avoid your policy if innocent non-disclosure or misrepresentation occurs.", Space = SpaceProcessingModeValues.Preserve }));
            body.Append(commentary5);
        }

        private void AddRecommendedInsurance(Body body, BasicDetails clientDetails, IEnumerable<ProposedInsuranceViewModel> clientProposedInsurance, Color orange, int v)
        {
            Paragraph break3 = new Paragraph(new Run(new Break() { Type = BreakValues.Page }));

            Paragraph s1 = body.AppendChild(new Paragraph());
            Run r1 = s1.AppendChild(new Run());
            RunProperties runProperties1 = r1.AppendChild(new RunProperties(new Bold(), new RunFonts { Ascii = "Verdana" }, new FontSize { Val = new StringValue("20") }));
            Color black = new Color() { Val = "000000" };
            runProperties1.AppendChild(orange.CloneNode(true));

            if (v == 0)
            {
                r1.AppendChild(new Text("Recommended Insurance cover for " + clientDetails.ClientName));
            }
            else
            {
                r1.AppendChild(new Text("Recommended Insurance cover for " + clientDetails.PartnerName));
            }


            foreach (ProposedInsuranceViewModel proposed in clientProposedInsurance)
            //New Table
            { 
                Table table = body.AppendChild(new Table());
                TableProperties tableProp = new TableProperties();
                TableStyle tableStyle = new TableStyle() { Val = "TableGrid" };

                TableWidth tableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };

                tableProp.Append(tableStyle, tableWidth);
                table.AppendChild(tableProp);


                ////Header
                TableRow header = new TableRow();
                TableCell h1 = new TableCell();

                TableCellProperties hcp = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                TableCellProperties hcp1 = new TableCellProperties(new TableCellWidth { Width = "2390", Type = TableWidthUnitValues.Auto }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });

                ParagraphProperties hppLeft = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });
                ParagraphProperties hppCenter = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });
                ParagraphProperties hppRight = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });

                Justification centerJustify = new Justification() { Val = JustificationValues.Center };
                hppCenter.AppendChild((Justification)centerJustify.CloneNode(true));

                Justification rightJustify = new Justification() { Val = JustificationValues.Right };
                hppRight.AppendChild((Justification)rightJustify.CloneNode(true));

                Justification leftJustify = new Justification() { Val = JustificationValues.Left };

                TableCellBorders hcb = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 20, Color = "ED7D27" } };

                Shading hs1 = new Shading() { Color = "auto", Fill = "393939", Val = ShadingPatternValues.Clear };
                TableCellMargin hcm1 = new TableCellMargin() { RightMargin = new RightMargin() { Width = "50", Type = TableWidthUnitValues.Dxa }, LeftMargin = new LeftMargin() { Width = "50", Type = TableWidthUnitValues.Dxa } };

                hcp.Append(hcm1, hs1, hcb);
                //hcp1.Append((TableCellMargin)hcm1.CloneNode(true), (Shading)hs1.CloneNode(true), (TableCellBorders)hcb.CloneNode(true));
                h1.Append(hcp);

                h1.AppendChild((ParagraphProperties)hppLeft.CloneNode(true));

                var heading = "";
                if(proposed.ReplacementId == 0)
                {
                    heading = "Purchase";
                }
                else
                {
                    heading = "";
                }

                h1.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(heading))));

                TableCell h2 = new TableCell();
                h2.AppendChild((ParagraphProperties)hppCenter.CloneNode(true));
                h2.Append((TableCellProperties)hcp.CloneNode(true));
                h2.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(proposed.Provider))));

                TableCell h3 = new TableCell();
                h3.AppendChild((ParagraphProperties)hppCenter.CloneNode(true));
                h3.Append((TableCellProperties)hcp.CloneNode(true));
                h3.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(""))));
                header.Append(h1, h2, h3);

                table.Append(header);
                //Body
                TableCellProperties tcp1 = new TableCellProperties(new TableCellWidth { Width = "2390", Type = TableWidthUnitValues.Auto }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                TableCellProperties tcp = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                TableCellProperties tcpN = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });

                TableCellBorders tcbL = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, RightBorder = new RightBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };
                TableCellBorders tcbR = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, LeftBorder = new LeftBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };
                TableCellBorders tcbN = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };

                TableCellMargin tcm = new TableCellMargin() { RightMargin = new RightMargin() { Width = "50", Type = TableWidthUnitValues.Dxa }, LeftMargin = new LeftMargin() { Width = "50", Type = TableWidthUnitValues.Dxa } };

                ParagraphProperties pp = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "0" });
                ParagraphProperties pp1 = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "0" });
                ParagraphProperties pp2 = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "0" });

                pp2.AppendChild((Justification)rightJustify.CloneNode(true));
                pp.AppendChild((Justification)leftJustify.CloneNode(true));
                tcp.Append(tcbL, tcm);
                tcpN.Append((TableCellBorders)tcbN.CloneNode(true), (TableCellMargin)tcm.CloneNode(true));


                if (proposed.LifeCover.Count() > 0)
                {

                    TableRow mtr = new TableRow();
                    TableCell mtc1 = new TableCell();

                    tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                    mtc1.Append((TableCellProperties)tcp.CloneNode(true));
                    mtc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                    mtc1.Append(new Paragraph(new Run(new Text(""))));

                    TableCell mtc2 = new TableCell();
                    mtc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                   mtc2.Append((TableCellProperties)tcp1.CloneNode(true));               
                    mtc2.Append(new Paragraph(new Run(new Run(new RunProperties(new Bold()), new Text("Cover Type")))));
                  
                    TableCell mtc3 = new TableCell();
                    mtc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                    mtc3.Append((TableCellProperties)tcpN.CloneNode(true));
                    mtc3.Append(new Paragraph(new Run(new Run(new RunProperties(new Bold()), new Text("Life")))));

                    mtr.Append(mtc1, mtc2, mtc3);

                    table.AppendChild(mtr);

                    if(proposed.LifeCover.First().PolicyOwner != "")
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Policy Owner"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text(proposed.LifeCover.First().PolicyOwner == "Client" ? clientDetails.ClientName : (proposed.LifeCover.First().PolicyOwner == "Partner" ? clientDetails.PartnerName : "Super Fund")))));
                        
                       

                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }

                    if (proposed.LifeCover.First().BenefitAmount != 0)
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Benefit Amount"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text( "$" + String.Format("{0:n}", proposed.LifeCover.First().BenefitAmount.ToString())))));

                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }

                    if (proposed.LifeCover.First().PremiumType != "")
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Premium Type"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text(proposed.LifeCover.First().PremiumType))));

                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }
                    if (proposed.LifeCover.First().WithinSuper < 2)
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Super"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text(proposed.LifeCover.First().WithinSuper == 1 ? "Yes" : "No"))));

                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }
                }

                if (proposed.TpdCover.Count() > 0)
                {

                    TableRow mtr = new TableRow();
                    TableCell mtc1 = new TableCell();

                    tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                    mtc1.Append((TableCellProperties)tcp.CloneNode(true));
                    mtc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                    mtc1.Append(new Paragraph(new Run(new Text(""))));

                    TableCell mtc2 = new TableCell();
                    mtc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                    mtc2.Append((TableCellProperties)tcp1.CloneNode(true));
                    mtc2.Append(new Paragraph(new Run(new Run(new RunProperties(new Bold()), new Text("Cover Type")))));

                    TableCell mtc3 = new TableCell();
                    mtc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                    mtc3.Append((TableCellProperties)tcpN.CloneNode(true));
                    mtc3.Append(new Paragraph(new Run(new Run(new RunProperties(new Bold()), new Text("TPD")))));

                    mtr.Append(mtc1, mtc2, mtc3);

                    table.AppendChild(mtr);

                    if (proposed.TpdCover.First().PolicyOwner != "")
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Policy Owner"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text(proposed.TpdCover.First().PolicyOwner == "Client" ? clientDetails.ClientName : (proposed.TpdCover.First().PolicyOwner == "Partner" ? clientDetails.PartnerName : "Super Fund")))));


                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }

                    if (proposed.TpdCover.First().BenefitAmount != 0)
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Benefit Amount"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text("$" + String.Format("{0:n}", proposed.TpdCover.First().BenefitAmount.ToString())))));

                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }

                    if (proposed.TpdCover.First().Definition != "")
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Definition"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text(proposed.TpdCover.First().Definition))));

                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }
                    if (proposed.TpdCover.First().StandaloneOrLinked != "")
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Stand Alone / Linked"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text(proposed.TpdCover.First().StandaloneOrLinked))));

                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }
                    if (proposed.TpdCover.First().PremiumType != "")
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Premium Type"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text(proposed.TpdCover.First().PremiumType))));

                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }
                    if (proposed.TpdCover.First().WithinSuper != "")
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Super"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text(proposed.TpdCover.First().WithinSuper))));

                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }
                }

                if (proposed.TraumaCover.Count() > 0)
                {

                    TableRow mtr = new TableRow();
                    TableCell mtc1 = new TableCell();

                    tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                    mtc1.Append((TableCellProperties)tcp.CloneNode(true));
                    mtc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                    mtc1.Append(new Paragraph(new Run(new Text(""))));

                    TableCell mtc2 = new TableCell();
                    mtc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                    mtc2.Append((TableCellProperties)tcp1.CloneNode(true));
                    mtc2.Append(new Paragraph(new Run(new Run(new RunProperties(new Bold()), new Text("Cover Type")))));

                    TableCell mtc3 = new TableCell();
                    mtc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                    mtc3.Append((TableCellProperties)tcpN.CloneNode(true));
                    mtc3.Append(new Paragraph(new Run(new Run(new RunProperties(new Bold()), new Text("Trauma")))));

                    mtr.Append(mtc1, mtc2, mtc3);

                    table.AppendChild(mtr);

                    if (proposed.TraumaCover.First().PolicyOwner != "")
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Policy Owner"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text(proposed.TraumaCover.First().PolicyOwner == "Client" ? clientDetails.ClientName : (proposed.TraumaCover.First().PolicyOwner == "Partner" ? clientDetails.PartnerName : "Super Fund")))));


                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }

                    if (proposed.TraumaCover.First().BenefitAmount != 0)
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Benefit Amount"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text("$" + String.Format("{0:n}", proposed.TraumaCover.First().BenefitAmount.ToString())))));

                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }

                    
                    if (proposed.TraumaCover.First().StandaloneOrLinked != "")
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Stand Alone / Linked"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text(proposed.TraumaCover.First().StandaloneOrLinked))));

                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }
                    if (proposed.TraumaCover.First().PremiumType != "")
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Premium Type"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text(proposed.TraumaCover.First().PremiumType))));

                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }
                    if (proposed.TraumaCover.First().WithinSuper != "")
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Super"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text(proposed.TraumaCover.First().WithinSuper))));

                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }
                }

                if (proposed.IncomeCover.Count() > 0)
                {

                    TableRow mtr = new TableRow();
                    TableCell mtc1 = new TableCell();

                    tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                    mtc1.Append((TableCellProperties)tcp.CloneNode(true));
                    mtc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                    mtc1.Append(new Paragraph(new Run(new Text(""))));

                    TableCell mtc2 = new TableCell();
                    mtc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                    mtc2.Append((TableCellProperties)tcp1.CloneNode(true));
                    mtc2.Append(new Paragraph(new Run(new Run(new RunProperties(new Bold()), new Text("Cover Type")))));

                    TableCell mtc3 = new TableCell();
                    mtc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                    mtc3.Append((TableCellProperties)tcpN.CloneNode(true));
                    mtc3.Append(new Paragraph(new Run(new Run(new RunProperties(new Bold()), new Text("Income Protection")))));

                    mtr.Append(mtc1, mtc2, mtc3);

                    table.AppendChild(mtr);

                    if (proposed.IncomeCover.First().PolicyOwner != "")
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Policy Owner"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text(proposed.IncomeCover.First().PolicyOwner == "Client" ? clientDetails.ClientName : (proposed.IncomeCover.First().PolicyOwner == "Partner" ? clientDetails.PartnerName : "Super Fund")))));


                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }

                    if (proposed.IncomeCover.First().MonthlyBenefitAmount != 0)
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Benefit Amount"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text("$" + String.Format("{0:n}", proposed.IncomeCover.First().MonthlyBenefitAmount.ToString())))));

                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }

                    if (proposed.IncomeCover.First().Definition != "")
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Definition"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text(proposed.IncomeCover.First().Definition))));

                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }
                    if (proposed.IncomeCover.First().WaitingPeriod != "")
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Waiting Period"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text(proposed.IncomeCover.First().WaitingPeriod))));

                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }
                    if (proposed.IncomeCover.First().BenefitPeriod != "")
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Benefit Period"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text(proposed.IncomeCover.First().BenefitPeriod))));

                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }
                    if (proposed.IncomeCover.First().PremiumType != "")
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Premium Type"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text(proposed.IncomeCover.First().PremiumType))));

                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }
                    if (proposed.IncomeCover.First().WithinSuper < 2)
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();

                        tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                        tc1.Append((TableCellProperties)tcp.CloneNode(true));
                        tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tc1.Append(new Paragraph(new Run(new Text(""))));

                        TableCell tc2 = new TableCell();
                        tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                        tc2.Append(new Paragraph(new Run(new Text("Super"))));

                        TableCell tc3 = new TableCell();
                        tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        tc3.Append((TableCellProperties)tcpN.CloneNode(true));
                        tc3.Append(new Paragraph(new Run(new Text(proposed.IncomeCover.First().WithinSuper == 1 ? "Yes" : "No"))));

                        tr.Append(tc1, tc2, tc3);

                        table.AppendChild(tr);
                    }
                }


                Run linebreak = new Run(new Break());
                Paragraph productCost = new Paragraph();
                productCost.Append(new OpenXmlElement[] { (Run)linebreak.CloneNode(true), new Run(new RunProperties(new Bold()), new Text("Please note the following: ")) });
                body.Append(productCost);

            }
        }

        public void add(Paragraph body)
        {
            AlternateContentChoice alternateContentChoice1 = new AlternateContentChoice() { Requires = "wps" };

            W.Drawing drawing1 = new W.Drawing();

            Wp.Anchor anchor1 = new Wp.Anchor() { DistanceFromTop = (UInt32Value)0U, DistanceFromBottom = (UInt32Value)0U, DistanceFromLeft = (UInt32Value)114935U, DistanceFromRight = (UInt32Value)114935U, SimplePos = false, RelativeHeight = (UInt32Value)251676672U, BehindDoc = true, Locked = false, LayoutInCell = true, AllowOverlap = true, EditId = "3A313D92", AnchorId = "5438FE6E" };
            Wp.SimplePosition simplePosition1 = new Wp.SimplePosition() { X = 0L, Y = 0L };

            Wp.HorizontalPosition horizontalPosition1 = new Wp.HorizontalPosition() { RelativeFrom = Wp.HorizontalRelativePositionValues.Column };
            Wp.PositionOffset positionOffset1 = new Wp.PositionOffset();
            positionOffset1.Text = "6030595";

            horizontalPosition1.Append(positionOffset1);

            Wp.VerticalPosition verticalPosition1 = new Wp.VerticalPosition() { RelativeFrom = Wp.VerticalRelativePositionValues.Paragraph };
            Wp.PositionOffset positionOffset2 = new Wp.PositionOffset();
            positionOffset2.Text = "97155";

            verticalPosition1.Append(positionOffset2);
            Wp.Extent extent1 = new Wp.Extent() { Cx = 35560L, Cy = 13970L };
            Wp.EffectExtent effectExtent1 = new Wp.EffectExtent() { LeftEdge = 8255L, TopEdge = 5715L, RightEdge = 3810L, BottomEdge = 8890L };
            Wp.WrapNone wrapNone1 = new Wp.WrapNone();
            Wp.DocProperties docProperties1 = new Wp.DocProperties() { Id = (UInt32Value)92U, Name = "Text Box 92" };

            Wp.NonVisualGraphicFrameDrawingProperties nonVisualGraphicFrameDrawingProperties1 = new Wp.NonVisualGraphicFrameDrawingProperties();

            A.GraphicFrameLocks graphicFrameLocks1 = new A.GraphicFrameLocks();
            graphicFrameLocks1.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            nonVisualGraphicFrameDrawingProperties1.Append(graphicFrameLocks1);

            A.Graphic graphic1 = new A.Graphic();
            graphic1.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            A.GraphicData graphicData1 = new A.GraphicData() { Uri = "http://schemas.microsoft.com/office/word/2010/wordprocessingShape" };

            Wps.WordprocessingShape wordprocessingShape1 = new Wps.WordprocessingShape();

            Wps.NonVisualDrawingShapeProperties nonVisualDrawingShapeProperties1 = new Wps.NonVisualDrawingShapeProperties() { TextBox = true };
            A.ShapeLocks shapeLocks1 = new A.ShapeLocks() { NoChangeArrowheads = true };

            nonVisualDrawingShapeProperties1.Append(shapeLocks1);

            Wps.ShapeProperties shapeProperties1 = new Wps.ShapeProperties() { BlackWhiteMode = A.BlackWhiteModeValues.Auto };

            A.Transform2D transform2D1 = new A.Transform2D();
            A.Offset offset1 = new A.Offset() { X = 0L, Y = 0L };
            A.Extents extents1 = new A.Extents() { Cx = 35560L, Cy = 13970L };

            transform2D1.Append(offset1);
            transform2D1.Append(extents1);

            A.PresetGeometry presetGeometry1 = new A.PresetGeometry() { Preset = A.ShapeTypeValues.Rectangle };
            A.AdjustValueList adjustValueList1 = new A.AdjustValueList();

            presetGeometry1.Append(adjustValueList1);

            A.SolidFill solidFill1 = new A.SolidFill();

            A.RgbColorModelHex rgbColorModelHex1 = new A.RgbColorModelHex() { Val = "FFFFFF" };
            A.Alpha alpha1 = new A.Alpha() { Val = 0 };

            rgbColorModelHex1.Append(alpha1);

            solidFill1.Append(rgbColorModelHex1);

            A.Outline outline1 = new A.Outline();
            A.NoFill noFill1 = new A.NoFill();

            outline1.Append(noFill1);

            A.ShapePropertiesExtensionList shapePropertiesExtensionList1 = new A.ShapePropertiesExtensionList();

            A.ShapePropertiesExtension shapePropertiesExtension1 = new A.ShapePropertiesExtension() { Uri = "{91240B29-F687-4F45-9708-019B960494DF}" };

            A14.HiddenLineProperties hiddenLineProperties1 = new A14.HiddenLineProperties() { Width = 9525 };
            hiddenLineProperties1.AddNamespaceDeclaration("a14", "http://schemas.microsoft.com/office/drawing/2010/main");

            A.SolidFill solidFill2 = new A.SolidFill();
            A.RgbColorModelHex rgbColorModelHex2 = new A.RgbColorModelHex() { Val = "000000" };

            solidFill2.Append(rgbColorModelHex2);
            A.Miter miter1 = new A.Miter() { Limit = 800000 };
            A.HeadEnd headEnd1 = new A.HeadEnd();
            A.TailEnd tailEnd1 = new A.TailEnd();

            hiddenLineProperties1.Append(solidFill2);
            hiddenLineProperties1.Append(miter1);
            hiddenLineProperties1.Append(headEnd1);
            hiddenLineProperties1.Append(tailEnd1);

            shapePropertiesExtension1.Append(hiddenLineProperties1);

            shapePropertiesExtensionList1.Append(shapePropertiesExtension1);

            shapeProperties1.Append(transform2D1);
            shapeProperties1.Append(presetGeometry1);
            shapeProperties1.Append(solidFill1);
            shapeProperties1.Append(outline1);
            shapeProperties1.Append(shapePropertiesExtensionList1);

            Wps.TextBoxInfo2 textBoxInfo21 = new Wps.TextBoxInfo2();

            W.TextBoxContent textBoxContent1 = new W.TextBoxContent();

            W.Paragraph paragraph1 = new W.Paragraph() { RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "04CE03A0", TextId = "77777777" };

            W.ParagraphProperties paragraphProperties1 = new W.ParagraphProperties();
            W.ParagraphStyleId paragraphStyleId1 = new W.ParagraphStyleId() { Val = "BodyText" };

            W.ParagraphMarkRunProperties paragraphMarkRunProperties1 = new W.ParagraphMarkRunProperties();
            W.RunFonts runFonts1 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold1 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript1 = new W.FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties1.Append(runFonts1);
            paragraphMarkRunProperties1.Append(bold1);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript1);

            paragraphProperties1.Append(paragraphStyleId1);
            paragraphProperties1.Append(paragraphMarkRunProperties1);

            W.Run run1 = new W.Run();

            W.RunProperties runProperties1 = new W.RunProperties();
            W.RunFonts runFonts2 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold2 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript2 = new W.FontSizeComplexScript() { Val = "20" };

            runProperties1.Append(runFonts2);
            runProperties1.Append(bold2);
            runProperties1.Append(fontSizeComplexScript2);
            W.Text text1 = new W.Text() { Space = SpaceProcessingModeValues.Preserve };
            text1.Text = " ";

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            W.Paragraph paragraph2 = new W.Paragraph() { RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "6E6DB8AF", TextId = "77777777" };

            W.ParagraphProperties paragraphProperties2 = new W.ParagraphProperties();
            W.ParagraphStyleId paragraphStyleId2 = new W.ParagraphStyleId() { Val = "BodyText" };

            W.ParagraphMarkRunProperties paragraphMarkRunProperties2 = new W.ParagraphMarkRunProperties();
            W.RunFonts runFonts3 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold3 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript3 = new W.FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties2.Append(runFonts3);
            paragraphMarkRunProperties2.Append(bold3);
            paragraphMarkRunProperties2.Append(fontSizeComplexScript3);

            paragraphProperties2.Append(paragraphStyleId2);
            paragraphProperties2.Append(paragraphMarkRunProperties2);

            W.Run run2 = new W.Run();

            W.RunProperties runProperties2 = new W.RunProperties();
            W.RunFonts runFonts4 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold4 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript4 = new W.FontSizeComplexScript() { Val = "20" };

            runProperties2.Append(runFonts4);
            runProperties2.Append(bold4);
            runProperties2.Append(fontSizeComplexScript4);
            W.Text text2 = new W.Text() { Space = SpaceProcessingModeValues.Preserve };
            text2.Text = " ";

            run2.Append(runProperties2);
            run2.Append(text2);

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run2);

            W.Paragraph paragraph3 = new W.Paragraph() { RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "02530E2F", TextId = "77777777" };

            W.ParagraphProperties paragraphProperties3 = new W.ParagraphProperties();
            W.ParagraphStyleId paragraphStyleId3 = new W.ParagraphStyleId() { Val = "BodyText" };

            W.ParagraphMarkRunProperties paragraphMarkRunProperties3 = new W.ParagraphMarkRunProperties();
            W.RunFonts runFonts5 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold5 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript5 = new W.FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties3.Append(runFonts5);
            paragraphMarkRunProperties3.Append(bold5);
            paragraphMarkRunProperties3.Append(fontSizeComplexScript5);

            paragraphProperties3.Append(paragraphStyleId3);
            paragraphProperties3.Append(paragraphMarkRunProperties3);

            W.Run run3 = new W.Run();

            W.RunProperties runProperties3 = new W.RunProperties();
            W.RunFonts runFonts6 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold6 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript6 = new W.FontSizeComplexScript() { Val = "20" };

            runProperties3.Append(runFonts6);
            runProperties3.Append(bold6);
            runProperties3.Append(fontSizeComplexScript6);
            W.Text text3 = new W.Text() { Space = SpaceProcessingModeValues.Preserve };
            text3.Text = " ";

            run3.Append(runProperties3);
            run3.Append(text3);

            paragraph3.Append(paragraphProperties3);
            paragraph3.Append(run3);

            W.Paragraph paragraph4 = new W.Paragraph() { RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "4753D73F", TextId = "77777777" };

            W.ParagraphProperties paragraphProperties4 = new W.ParagraphProperties();
            W.ParagraphStyleId paragraphStyleId4 = new W.ParagraphStyleId() { Val = "BodyText" };

            W.ParagraphMarkRunProperties paragraphMarkRunProperties4 = new W.ParagraphMarkRunProperties();
            W.RunFonts runFonts7 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold7 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript7 = new W.FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties4.Append(runFonts7);
            paragraphMarkRunProperties4.Append(bold7);
            paragraphMarkRunProperties4.Append(fontSizeComplexScript7);

            paragraphProperties4.Append(paragraphStyleId4);
            paragraphProperties4.Append(paragraphMarkRunProperties4);

            paragraph4.Append(paragraphProperties4);

            W.Paragraph paragraph5 = new W.Paragraph() { RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "7675723D", TextId = "77777777" };

            W.ParagraphProperties paragraphProperties5 = new W.ParagraphProperties();
            W.ParagraphStyleId paragraphStyleId5 = new W.ParagraphStyleId() { Val = "BodyText" };

            W.ParagraphMarkRunProperties paragraphMarkRunProperties5 = new W.ParagraphMarkRunProperties();
            W.RunFonts runFonts8 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold8 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript8 = new W.FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties5.Append(runFonts8);
            paragraphMarkRunProperties5.Append(bold8);
            paragraphMarkRunProperties5.Append(fontSizeComplexScript8);

            paragraphProperties5.Append(paragraphStyleId5);
            paragraphProperties5.Append(paragraphMarkRunProperties5);

            W.Run run4 = new W.Run();

            W.RunProperties runProperties4 = new W.RunProperties();
            W.RunFonts runFonts9 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold9 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript9 = new W.FontSizeComplexScript() { Val = "20" };

            runProperties4.Append(runFonts9);
            runProperties4.Append(bold9);
            runProperties4.Append(fontSizeComplexScript9);
            W.Text text4 = new W.Text() { Space = SpaceProcessingModeValues.Preserve };
            text4.Text = " ";

            run4.Append(runProperties4);
            run4.Append(text4);

            paragraph5.Append(paragraphProperties5);
            paragraph5.Append(run4);

            W.Paragraph paragraph6 = new W.Paragraph() { RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "12AC83A7", TextId = "77777777" };

            W.ParagraphProperties paragraphProperties6 = new W.ParagraphProperties();
            W.ParagraphStyleId paragraphStyleId6 = new W.ParagraphStyleId() { Val = "BodyText" };

            W.ParagraphMarkRunProperties paragraphMarkRunProperties6 = new W.ParagraphMarkRunProperties();
            W.RunFonts runFonts10 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold10 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript10 = new W.FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties6.Append(runFonts10);
            paragraphMarkRunProperties6.Append(bold10);
            paragraphMarkRunProperties6.Append(fontSizeComplexScript10);

            paragraphProperties6.Append(paragraphStyleId6);
            paragraphProperties6.Append(paragraphMarkRunProperties6);

            W.Run run5 = new W.Run();

            W.RunProperties runProperties5 = new W.RunProperties();
            W.RunFonts runFonts11 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold11 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript11 = new W.FontSizeComplexScript() { Val = "20" };

            runProperties5.Append(runFonts11);
            runProperties5.Append(bold11);
            runProperties5.Append(fontSizeComplexScript11);
            W.Text text5 = new W.Text() { Space = SpaceProcessingModeValues.Preserve };
            text5.Text = " ";

            run5.Append(runProperties5);
            run5.Append(text5);

            paragraph6.Append(paragraphProperties6);
            paragraph6.Append(run5);

            W.Paragraph paragraph7 = new W.Paragraph() { RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "7F4349F2", TextId = "77777777" };

            W.ParagraphProperties paragraphProperties7 = new W.ParagraphProperties();
            W.ParagraphStyleId paragraphStyleId7 = new W.ParagraphStyleId() { Val = "BodyText" };

            W.ParagraphMarkRunProperties paragraphMarkRunProperties7 = new W.ParagraphMarkRunProperties();
            W.RunFonts runFonts12 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold12 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript12 = new W.FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties7.Append(runFonts12);
            paragraphMarkRunProperties7.Append(bold12);
            paragraphMarkRunProperties7.Append(fontSizeComplexScript12);

            paragraphProperties7.Append(paragraphStyleId7);
            paragraphProperties7.Append(paragraphMarkRunProperties7);

            W.Run run6 = new W.Run();

            W.RunProperties runProperties6 = new W.RunProperties();
            W.RunFonts runFonts13 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold13 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript13 = new W.FontSizeComplexScript() { Val = "20" };

            runProperties6.Append(runFonts13);
            runProperties6.Append(bold13);
            runProperties6.Append(fontSizeComplexScript13);
            W.Text text6 = new W.Text() { Space = SpaceProcessingModeValues.Preserve };
            text6.Text = " ";

            run6.Append(runProperties6);
            run6.Append(text6);

            paragraph7.Append(paragraphProperties7);
            paragraph7.Append(run6);

            W.Paragraph paragraph8 = new W.Paragraph() { RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "5378D687", TextId = "77777777" };

            W.ParagraphProperties paragraphProperties8 = new W.ParagraphProperties();
            W.ParagraphStyleId paragraphStyleId8 = new W.ParagraphStyleId() { Val = "BodyText" };

            W.ParagraphMarkRunProperties paragraphMarkRunProperties8 = new W.ParagraphMarkRunProperties();
            W.RunFonts runFonts14 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold14 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript14 = new W.FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties8.Append(runFonts14);
            paragraphMarkRunProperties8.Append(bold14);
            paragraphMarkRunProperties8.Append(fontSizeComplexScript14);

            paragraphProperties8.Append(paragraphStyleId8);
            paragraphProperties8.Append(paragraphMarkRunProperties8);

            W.Run run7 = new W.Run();

            W.RunProperties runProperties7 = new W.RunProperties();
            W.RunFonts runFonts15 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold15 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript15 = new W.FontSizeComplexScript() { Val = "20" };

            runProperties7.Append(runFonts15);
            runProperties7.Append(bold15);
            runProperties7.Append(fontSizeComplexScript15);
            W.Text text7 = new W.Text() { Space = SpaceProcessingModeValues.Preserve };
            text7.Text = " ";

            run7.Append(runProperties7);
            run7.Append(text7);

            paragraph8.Append(paragraphProperties8);
            paragraph8.Append(run7);

            W.Paragraph paragraph9 = new W.Paragraph() { RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "5DCADE92", TextId = "77777777" };

            W.ParagraphProperties paragraphProperties9 = new W.ParagraphProperties();
            W.ParagraphStyleId paragraphStyleId9 = new W.ParagraphStyleId() { Val = "BodyText" };

            W.ParagraphMarkRunProperties paragraphMarkRunProperties9 = new W.ParagraphMarkRunProperties();
            W.RunFonts runFonts16 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold16 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript16 = new W.FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties9.Append(runFonts16);
            paragraphMarkRunProperties9.Append(bold16);
            paragraphMarkRunProperties9.Append(fontSizeComplexScript16);

            paragraphProperties9.Append(paragraphStyleId9);
            paragraphProperties9.Append(paragraphMarkRunProperties9);

            W.Run run8 = new W.Run();

            W.RunProperties runProperties8 = new W.RunProperties();
            W.RunFonts runFonts17 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold17 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript17 = new W.FontSizeComplexScript() { Val = "20" };

            runProperties8.Append(runFonts17);
            runProperties8.Append(bold17);
            runProperties8.Append(fontSizeComplexScript17);
            W.Text text8 = new W.Text() { Space = SpaceProcessingModeValues.Preserve };
            text8.Text = " ";

            run8.Append(runProperties8);
            run8.Append(text8);

            paragraph9.Append(paragraphProperties9);
            paragraph9.Append(run8);

            W.Paragraph paragraph10 = new W.Paragraph() { RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "4AC96599", TextId = "77777777" };

            W.ParagraphProperties paragraphProperties10 = new W.ParagraphProperties();
            W.ParagraphStyleId paragraphStyleId10 = new W.ParagraphStyleId() { Val = "BodyText" };

            W.ParagraphMarkRunProperties paragraphMarkRunProperties10 = new W.ParagraphMarkRunProperties();
            W.RunFonts runFonts18 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold18 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript18 = new W.FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties10.Append(runFonts18);
            paragraphMarkRunProperties10.Append(bold18);
            paragraphMarkRunProperties10.Append(fontSizeComplexScript18);

            paragraphProperties10.Append(paragraphStyleId10);
            paragraphProperties10.Append(paragraphMarkRunProperties10);

            W.Run run9 = new W.Run();

            W.RunProperties runProperties9 = new W.RunProperties();
            W.RunFonts runFonts19 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold19 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript19 = new W.FontSizeComplexScript() { Val = "20" };

            runProperties9.Append(runFonts19);
            runProperties9.Append(bold19);
            runProperties9.Append(fontSizeComplexScript19);
            W.Text text9 = new W.Text() { Space = SpaceProcessingModeValues.Preserve };
            text9.Text = " ";

            run9.Append(runProperties9);
            run9.Append(text9);

            paragraph10.Append(paragraphProperties10);
            paragraph10.Append(run9);

            W.Paragraph paragraph11 = new W.Paragraph() { RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "2B4B8144", TextId = "77777777" };

            W.ParagraphProperties paragraphProperties11 = new W.ParagraphProperties();
            W.ParagraphStyleId paragraphStyleId11 = new W.ParagraphStyleId() { Val = "BodyText" };

            W.ParagraphMarkRunProperties paragraphMarkRunProperties11 = new W.ParagraphMarkRunProperties();
            W.RunFonts runFonts20 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold20 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript20 = new W.FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties11.Append(runFonts20);
            paragraphMarkRunProperties11.Append(bold20);
            paragraphMarkRunProperties11.Append(fontSizeComplexScript20);

            paragraphProperties11.Append(paragraphStyleId11);
            paragraphProperties11.Append(paragraphMarkRunProperties11);

            W.Run run10 = new W.Run();

            W.RunProperties runProperties10 = new W.RunProperties();
            W.RunFonts runFonts21 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold21 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript21 = new W.FontSizeComplexScript() { Val = "20" };

            runProperties10.Append(runFonts21);
            runProperties10.Append(bold21);
            runProperties10.Append(fontSizeComplexScript21);
            W.Text text10 = new W.Text() { Space = SpaceProcessingModeValues.Preserve };
            text10.Text = " ";

            run10.Append(runProperties10);
            run10.Append(text10);

            paragraph11.Append(paragraphProperties11);
            paragraph11.Append(run10);

            W.Paragraph paragraph12 = new W.Paragraph() { RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "0AAAC9A6", TextId = "77777777" };

            W.ParagraphProperties paragraphProperties12 = new W.ParagraphProperties();
            W.ParagraphStyleId paragraphStyleId12 = new W.ParagraphStyleId() { Val = "BodyText" };

            W.ParagraphMarkRunProperties paragraphMarkRunProperties12 = new W.ParagraphMarkRunProperties();
            W.RunFonts runFonts22 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold22 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript22 = new W.FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties12.Append(runFonts22);
            paragraphMarkRunProperties12.Append(bold22);
            paragraphMarkRunProperties12.Append(fontSizeComplexScript22);

            paragraphProperties12.Append(paragraphStyleId12);
            paragraphProperties12.Append(paragraphMarkRunProperties12);

            W.Run run11 = new W.Run();

            W.RunProperties runProperties11 = new W.RunProperties();
            W.RunFonts runFonts23 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold23 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript23 = new W.FontSizeComplexScript() { Val = "20" };

            runProperties11.Append(runFonts23);
            runProperties11.Append(bold23);
            runProperties11.Append(fontSizeComplexScript23);
            W.Text text11 = new W.Text() { Space = SpaceProcessingModeValues.Preserve };
            text11.Text = " ";

            run11.Append(runProperties11);
            run11.Append(text11);

            paragraph12.Append(paragraphProperties12);
            paragraph12.Append(run11);

            W.Paragraph paragraph13 = new W.Paragraph() { RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "5506A4C5", TextId = "77777777" };

            W.ParagraphProperties paragraphProperties13 = new W.ParagraphProperties();
            W.ParagraphStyleId paragraphStyleId13 = new W.ParagraphStyleId() { Val = "BodyText" };

            W.ParagraphMarkRunProperties paragraphMarkRunProperties13 = new W.ParagraphMarkRunProperties();
            W.RunFonts runFonts24 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold24 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript24 = new W.FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties13.Append(runFonts24);
            paragraphMarkRunProperties13.Append(bold24);
            paragraphMarkRunProperties13.Append(fontSizeComplexScript24);

            paragraphProperties13.Append(paragraphStyleId13);
            paragraphProperties13.Append(paragraphMarkRunProperties13);

            W.Run run12 = new W.Run();

            W.RunProperties runProperties12 = new W.RunProperties();
            W.RunFonts runFonts25 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold25 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript25 = new W.FontSizeComplexScript() { Val = "20" };

            runProperties12.Append(runFonts25);
            runProperties12.Append(bold25);
            runProperties12.Append(fontSizeComplexScript25);
            W.Text text12 = new W.Text() { Space = SpaceProcessingModeValues.Preserve };
            text12.Text = " ";

            run12.Append(runProperties12);
            run12.Append(text12);

            paragraph13.Append(paragraphProperties13);
            paragraph13.Append(run12);

            W.Paragraph paragraph14 = new W.Paragraph() { RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "7AEBEDA2", TextId = "77777777" };

            W.ParagraphProperties paragraphProperties14 = new W.ParagraphProperties();
            W.ParagraphStyleId paragraphStyleId14 = new W.ParagraphStyleId() { Val = "BodyText" };

            W.ParagraphMarkRunProperties paragraphMarkRunProperties14 = new W.ParagraphMarkRunProperties();
            W.RunFonts runFonts26 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold26 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript26 = new W.FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties14.Append(runFonts26);
            paragraphMarkRunProperties14.Append(bold26);
            paragraphMarkRunProperties14.Append(fontSizeComplexScript26);

            paragraphProperties14.Append(paragraphStyleId14);
            paragraphProperties14.Append(paragraphMarkRunProperties14);

            W.Run run13 = new W.Run();

            W.RunProperties runProperties13 = new W.RunProperties();
            W.RunFonts runFonts27 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold27 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript27 = new W.FontSizeComplexScript() { Val = "20" };

            runProperties13.Append(runFonts27);
            runProperties13.Append(bold27);
            runProperties13.Append(fontSizeComplexScript27);
            W.Text text13 = new W.Text() { Space = SpaceProcessingModeValues.Preserve };
            text13.Text = " ";

            run13.Append(runProperties13);
            run13.Append(text13);

            paragraph14.Append(paragraphProperties14);
            paragraph14.Append(run13);

            W.Paragraph paragraph15 = new W.Paragraph() { RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "48085869", TextId = "77777777" };

            W.ParagraphProperties paragraphProperties15 = new W.ParagraphProperties();
            W.ParagraphStyleId paragraphStyleId15 = new W.ParagraphStyleId() { Val = "BodyText" };

            W.ParagraphMarkRunProperties paragraphMarkRunProperties15 = new W.ParagraphMarkRunProperties();
            W.RunFonts runFonts28 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold28 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript28 = new W.FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties15.Append(runFonts28);
            paragraphMarkRunProperties15.Append(bold28);
            paragraphMarkRunProperties15.Append(fontSizeComplexScript28);

            paragraphProperties15.Append(paragraphStyleId15);
            paragraphProperties15.Append(paragraphMarkRunProperties15);

            paragraph15.Append(paragraphProperties15);

            W.Paragraph paragraph16 = new W.Paragraph() { RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "03349807", TextId = "77777777" };

            W.ParagraphProperties paragraphProperties16 = new W.ParagraphProperties();
            W.ParagraphStyleId paragraphStyleId16 = new W.ParagraphStyleId() { Val = "BodyText" };

            W.ParagraphMarkRunProperties paragraphMarkRunProperties16 = new W.ParagraphMarkRunProperties();
            W.RunFonts runFonts29 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold29 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript29 = new W.FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties16.Append(runFonts29);
            paragraphMarkRunProperties16.Append(bold29);
            paragraphMarkRunProperties16.Append(fontSizeComplexScript29);

            paragraphProperties16.Append(paragraphStyleId16);
            paragraphProperties16.Append(paragraphMarkRunProperties16);

            W.Run run14 = new W.Run();

            W.RunProperties runProperties14 = new W.RunProperties();
            W.RunFonts runFonts30 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold30 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript30 = new W.FontSizeComplexScript() { Val = "20" };

            runProperties14.Append(runFonts30);
            runProperties14.Append(bold30);
            runProperties14.Append(fontSizeComplexScript30);
            W.Text text14 = new W.Text() { Space = SpaceProcessingModeValues.Preserve };
            text14.Text = " ";

            run14.Append(runProperties14);
            run14.Append(text14);

            paragraph16.Append(paragraphProperties16);
            paragraph16.Append(run14);

            W.Paragraph paragraph17 = new W.Paragraph() { RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "13B8FCE4", TextId = "77777777" };

            W.ParagraphProperties paragraphProperties17 = new W.ParagraphProperties();
            W.ParagraphStyleId paragraphStyleId17 = new W.ParagraphStyleId() { Val = "BodyText" };

            W.ParagraphMarkRunProperties paragraphMarkRunProperties17 = new W.ParagraphMarkRunProperties();
            W.RunFonts runFonts31 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold31 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript31 = new W.FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties17.Append(runFonts31);
            paragraphMarkRunProperties17.Append(bold31);
            paragraphMarkRunProperties17.Append(fontSizeComplexScript31);

            paragraphProperties17.Append(paragraphStyleId17);
            paragraphProperties17.Append(paragraphMarkRunProperties17);

            paragraph17.Append(paragraphProperties17);

            W.Paragraph paragraph18 = new W.Paragraph() { RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "3ACA8116", TextId = "77777777" };

            W.ParagraphProperties paragraphProperties18 = new W.ParagraphProperties();
            W.ParagraphStyleId paragraphStyleId18 = new W.ParagraphStyleId() { Val = "BodyText" };

            W.ParagraphMarkRunProperties paragraphMarkRunProperties18 = new W.ParagraphMarkRunProperties();
            W.RunFonts runFonts32 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold32 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript32 = new W.FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties18.Append(runFonts32);
            paragraphMarkRunProperties18.Append(bold32);
            paragraphMarkRunProperties18.Append(fontSizeComplexScript32);

            paragraphProperties18.Append(paragraphStyleId18);
            paragraphProperties18.Append(paragraphMarkRunProperties18);

            W.Run run15 = new W.Run();

            W.RunProperties runProperties15 = new W.RunProperties();
            W.RunFonts runFonts33 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold33 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript33 = new W.FontSizeComplexScript() { Val = "20" };

            runProperties15.Append(runFonts33);
            runProperties15.Append(bold33);
            runProperties15.Append(fontSizeComplexScript33);
            W.Text text15 = new W.Text() { Space = SpaceProcessingModeValues.Preserve };
            text15.Text = " ";

            run15.Append(runProperties15);
            run15.Append(text15);

            paragraph18.Append(paragraphProperties18);
            paragraph18.Append(run15);

            W.Paragraph paragraph19 = new W.Paragraph() { RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "05C6F3B3", TextId = "77777777" };

            W.ParagraphProperties paragraphProperties19 = new W.ParagraphProperties();
            W.ParagraphStyleId paragraphStyleId19 = new W.ParagraphStyleId() { Val = "BodyText" };

            W.ParagraphMarkRunProperties paragraphMarkRunProperties19 = new W.ParagraphMarkRunProperties();
            W.RunFonts runFonts34 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold34 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript34 = new W.FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties19.Append(runFonts34);
            paragraphMarkRunProperties19.Append(bold34);
            paragraphMarkRunProperties19.Append(fontSizeComplexScript34);

            paragraphProperties19.Append(paragraphStyleId19);
            paragraphProperties19.Append(paragraphMarkRunProperties19);

            W.Run run16 = new W.Run();

            W.RunProperties runProperties16 = new W.RunProperties();
            W.RunFonts runFonts35 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold35 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript35 = new W.FontSizeComplexScript() { Val = "20" };

            runProperties16.Append(runFonts35);
            runProperties16.Append(bold35);
            runProperties16.Append(fontSizeComplexScript35);
            W.Text text16 = new W.Text() { Space = SpaceProcessingModeValues.Preserve };
            text16.Text = " ";

            run16.Append(runProperties16);
            run16.Append(text16);

            paragraph19.Append(paragraphProperties19);
            paragraph19.Append(run16);

            W.Paragraph paragraph20 = new W.Paragraph() { RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "3C7FD12D", TextId = "77777777" };

            W.ParagraphProperties paragraphProperties20 = new W.ParagraphProperties();
            W.ParagraphStyleId paragraphStyleId20 = new W.ParagraphStyleId() { Val = "BodyText" };

            W.ParagraphMarkRunProperties paragraphMarkRunProperties20 = new W.ParagraphMarkRunProperties();
            W.RunFonts runFonts36 = new W.RunFonts() { ComplexScript = "Calibri" };
            W.Bold bold36 = new W.Bold();
            W.FontSizeComplexScript fontSizeComplexScript36 = new W.FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties20.Append(runFonts36);
            paragraphMarkRunProperties20.Append(bold36);
            paragraphMarkRunProperties20.Append(fontSizeComplexScript36);

            paragraphProperties20.Append(paragraphStyleId20);
            paragraphProperties20.Append(paragraphMarkRunProperties20);

            paragraph20.Append(paragraphProperties20);

            textBoxContent1.Append(paragraph1);
            textBoxContent1.Append(paragraph2);
            textBoxContent1.Append(paragraph3);
            textBoxContent1.Append(paragraph4);
            textBoxContent1.Append(paragraph5);
            textBoxContent1.Append(paragraph6);
            textBoxContent1.Append(paragraph7);
            textBoxContent1.Append(paragraph8);
            textBoxContent1.Append(paragraph9);
            textBoxContent1.Append(paragraph10);
            textBoxContent1.Append(paragraph11);
            textBoxContent1.Append(paragraph12);
            textBoxContent1.Append(paragraph13);
            textBoxContent1.Append(paragraph14);
            textBoxContent1.Append(paragraph15);
            textBoxContent1.Append(paragraph16);
            textBoxContent1.Append(paragraph17);
            textBoxContent1.Append(paragraph18);
            textBoxContent1.Append(paragraph19);
            textBoxContent1.Append(paragraph20);

            textBoxInfo21.Append(textBoxContent1);

            Wps.TextBodyProperties textBodyProperties1 = new Wps.TextBodyProperties() { Rotation = 0, Vertical = A.TextVerticalValues.Horizontal, Wrap = A.TextWrappingValues.Square, LeftInset = 0, TopInset = 0, RightInset = 0, BottomInset = 0, Anchor = A.TextAnchoringTypeValues.Top, AnchorCenter = false, UpRight = true };
            A.NoAutoFit noAutoFit1 = new A.NoAutoFit();

            textBodyProperties1.Append(noAutoFit1);

            wordprocessingShape1.Append(nonVisualDrawingShapeProperties1);
            wordprocessingShape1.Append(shapeProperties1);
            wordprocessingShape1.Append(textBoxInfo21);
            wordprocessingShape1.Append(textBodyProperties1);

            graphicData1.Append(wordprocessingShape1);

            graphic1.Append(graphicData1);

            Wp14.RelativeWidth relativeWidth1 = new Wp14.RelativeWidth() { ObjectId = Wp14.SizeRelativeHorizontallyValues.Page };
            Wp14.PercentageWidth percentageWidth1 = new Wp14.PercentageWidth();
            percentageWidth1.Text = "0";

            relativeWidth1.Append(percentageWidth1);

            Wp14.RelativeHeight relativeHeight1 = new Wp14.RelativeHeight() { RelativeFrom = Wp14.SizeRelativeVerticallyValues.Page };
            Wp14.PercentageHeight percentageHeight1 = new Wp14.PercentageHeight();
            percentageHeight1.Text = "0";

            relativeHeight1.Append(percentageHeight1);

            anchor1.Append(simplePosition1);
            anchor1.Append(horizontalPosition1);
            anchor1.Append(verticalPosition1);
            anchor1.Append(extent1);
            anchor1.Append(effectExtent1);
            anchor1.Append(wrapNone1);
            anchor1.Append(docProperties1);
            anchor1.Append(nonVisualGraphicFrameDrawingProperties1);
            anchor1.Append(graphic1);
            anchor1.Append(relativeWidth1);
            anchor1.Append(relativeHeight1);

            drawing1.Append(anchor1);

            //alternateContentChoice1.Append(drawing1);
            //body.Append(alternateContentChoice1);
            //body.Append(drawing1);
            Run run = new Run();
            run.AppendChild(drawing1);

            body.Append(run);

        }

        public void GenerateBody(Body body)
        {
            Table table1 = new Table();

            TableProperties tableProperties1 = new TableProperties();
            TableWidth tableWidth1 = new TableWidth() { Width = "11319", Type = TableWidthUnitValues.Dxa };
            TableIndentation tableIndentation1 = new TableIndentation() { Width = -1310, Type = TableWidthUnitValues.Dxa };
            //  TableLayout tableLayout1 = new TableLayout() { Type = TableLayoutValues.Fixed };
            TableLook tableLook1 = new TableLook() { Val = "0000", FirstRow = false, LastRow = false, FirstColumn = false, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = false };

            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableIndentation1);
            //  tableProperties1.Append(tableLayout1);
            tableProperties1.Append(tableLook1);

            TableGrid tableGrid1 = new TableGrid();
            GridColumn gridColumn1 = new GridColumn() { Width = "10448" };
            GridColumn gridColumn2 = new GridColumn() { Width = "871" };

            tableGrid1.Append(gridColumn1);
            tableGrid1.Append(gridColumn2);

            TableRow tableRow1 = new TableRow() { RsidTableRowMarkRevision = "00682DA0", RsidTableRowAddition = "00682DA0", RsidTableRowProperties = "00682DA0", ParagraphId = "70C1D861", TextId = "77777777" };

        

            TableRowProperties tableRowProperties1 = new TableRowProperties();
            TableRowHeight tableRowHeight1 = new TableRowHeight() { Val = (UInt32Value)4933U, HeightType = HeightRuleValues.Exact };

            tableRowProperties1.Append(tableRowHeight1);

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "10448", Type = TableWidthUnitValues.Dxa };
            Shading shading1 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "F58426" };
            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Bottom };

            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(shading1);
            tableCellProperties1.Append(tableCellVerticalAlignment1);

            Paragraph paragraph1 = new Paragraph() { RsidParagraphMarkRevision = "00682DA0", RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "28145B75", TextId = "77777777" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            SuppressAutoHyphens suppressAutoHyphens1 = new SuppressAutoHyphens();
            SpacingBetweenLines spacingBetweenLines1 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Indentation indentation1 = new Indentation() { Left = "1168" };

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            RunFonts runFonts1 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Verdana" };
            Color color1 = new Color() { Val = "FFFFFF" };
            FontSize fontSize1 = new FontSize() { Val = "60" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "60" };
            Languages languages1 = new Languages() { Val = "en-AU", EastAsia = "en-AU" };

            paragraphMarkRunProperties1.Append(runFonts1);
            paragraphMarkRunProperties1.Append(color1);
            paragraphMarkRunProperties1.Append(fontSize1);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript1);
            paragraphMarkRunProperties1.Append(languages1);

            paragraphProperties1.Append(suppressAutoHyphens1);
            paragraphProperties1.Append(spacingBetweenLines1);
            paragraphProperties1.Append(indentation1);
            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run() { RsidRunProperties = "00682DA0" };

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts2 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Verdana" };
            Color color2 = new Color() { Val = "FFFFFF" };
            FontSize fontSize2 = new FontSize() { Val = "60" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "60" };
            Languages languages2 = new Languages() { Val = "en-AU", EastAsia = "en-AU" };

            runProperties1.Append(runFonts2);
            runProperties1.Append(color2);
            runProperties1.Append(fontSize2);
            runProperties1.Append(fontSizeComplexScript2);
            runProperties1.Append(languages2);
            Text text1 = new Text();
            text1.Text = "Insurance";

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            Paragraph paragraph2 = new Paragraph() { RsidParagraphMarkRevision = "00682DA0", RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "30056DAC", TextId = "77777777" };

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();
            SuppressAutoHyphens suppressAutoHyphens2 = new SuppressAutoHyphens();
            SpacingBetweenLines spacingBetweenLines2 = new SpacingBetweenLines() { After = "200", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Indentation indentation2 = new Indentation() { Left = "1168" };

            ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
            RunFonts runFonts3 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Verdana" };
            Color color3 = new Color() { Val = "000000" };
            FontSize fontSize3 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript3 = new FontSizeComplexScript() { Val = "20" };
            Languages languages3 = new Languages() { Val = "en-AU", EastAsia = "en-AU" };

            paragraphMarkRunProperties2.Append(runFonts3);
            paragraphMarkRunProperties2.Append(color3);
            paragraphMarkRunProperties2.Append(fontSize3);
            paragraphMarkRunProperties2.Append(fontSizeComplexScript3);
            paragraphMarkRunProperties2.Append(languages3);

            paragraphProperties2.Append(suppressAutoHyphens2);
            paragraphProperties2.Append(spacingBetweenLines2);
            paragraphProperties2.Append(indentation2);
            paragraphProperties2.Append(paragraphMarkRunProperties2);

            Run run2 = new Run() { RsidRunProperties = "00682DA0" };

            RunProperties runProperties2 = new RunProperties();
            RunFonts runFonts4 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Verdana" };
            Color color4 = new Color() { Val = "FFFFFF" };
            FontSize fontSize4 = new FontSize() { Val = "60" };
            FontSizeComplexScript fontSizeComplexScript4 = new FontSizeComplexScript() { Val = "60" };
            Languages languages4 = new Languages() { Val = "en-AU", EastAsia = "en-AU" };

            runProperties2.Append(runFonts4);
            runProperties2.Append(color4);
            runProperties2.Append(fontSize4);
            runProperties2.Append(fontSizeComplexScript4);
            runProperties2.Append(languages4);
            Text text2 = new Text();
            text2.Text = "recommendations";

            run2.Append(runProperties2);
            run2.Append(text2);

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run2);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);
            tableCell1.Append(paragraph2);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "871", Type = TableWidthUnitValues.Dxa };
            Shading shading2 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "F58426" };

            tableCellProperties2.Append(tableCellWidth2);
            tableCellProperties2.Append(shading2);

            Paragraph paragraph3 = new Paragraph() { RsidParagraphMarkRevision = "00682DA0", RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "5B4D34FA", TextId = "77777777" };

            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            SuppressAutoHyphens suppressAutoHyphens3 = new SuppressAutoHyphens();
            SnapToGrid snapToGrid1 = new SnapToGrid() { Val = false };
            SpacingBetweenLines spacingBetweenLines3 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            ParagraphMarkRunProperties paragraphMarkRunProperties3 = new ParagraphMarkRunProperties();
            RunFonts runFonts5 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Verdana" };
            Color color5 = new Color() { Val = "000000" };
            FontSize fontSize5 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript5 = new FontSizeComplexScript() { Val = "20" };
            Languages languages5 = new Languages() { Val = "en-AU", EastAsia = "en-AU" };

            paragraphMarkRunProperties3.Append(runFonts5);
            paragraphMarkRunProperties3.Append(color5);
            paragraphMarkRunProperties3.Append(fontSize5);
            paragraphMarkRunProperties3.Append(fontSizeComplexScript5);
            paragraphMarkRunProperties3.Append(languages5);

            paragraphProperties3.Append(suppressAutoHyphens3);
            paragraphProperties3.Append(snapToGrid1);
            paragraphProperties3.Append(spacingBetweenLines3);
            paragraphProperties3.Append(paragraphMarkRunProperties3);

            paragraph3.Append(paragraphProperties3);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph3);
            BookmarkStart bookmarkStart1 = new BookmarkStart() { Name = "_GoBack", Id = "0" };
            BookmarkEnd bookmarkEnd1 = new BookmarkEnd() { Id = "0" };

            tableRow1.Append(tableRowProperties1);
            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);
            tableRow1.Append(bookmarkStart1);
            tableRow1.Append(bookmarkEnd1);

            TableRow tableRow2 = new TableRow() { RsidTableRowMarkRevision = "00682DA0", RsidTableRowAddition = "00682DA0", RsidTableRowProperties = "00682DA0", ParagraphId = "3AE95C2D", TextId = "77777777" };

            TableRowProperties tableRowProperties2 = new TableRowProperties();
            TableRowHeight tableRowHeight2 = new TableRowHeight() { Val = (UInt32Value)312U, HeightType = HeightRuleValues.Exact };

            tableRowProperties2.Append(tableRowHeight2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "10448", Type = TableWidthUnitValues.Dxa };
            Shading shading3 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "5F6062" };

            tableCellProperties3.Append(tableCellWidth3);
            tableCellProperties3.Append(shading3);

            Paragraph paragraph4 = new Paragraph() { RsidParagraphMarkRevision = "00682DA0", RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "57970676", TextId = "77777777" };

            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            SuppressAutoHyphens suppressAutoHyphens4 = new SuppressAutoHyphens();
            SnapToGrid snapToGrid2 = new SnapToGrid() { Val = false };
            SpacingBetweenLines spacingBetweenLines4 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            ParagraphMarkRunProperties paragraphMarkRunProperties4 = new ParagraphMarkRunProperties();
            RunFonts runFonts6 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Verdana" };
            Color color6 = new Color() { Val = "000000" };
            FontSize fontSize6 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript6 = new FontSizeComplexScript() { Val = "20" };
            Languages languages6 = new Languages() { Val = "en-AU", EastAsia = "en-AU" };

            paragraphMarkRunProperties4.Append(runFonts6);
            paragraphMarkRunProperties4.Append(color6);
            paragraphMarkRunProperties4.Append(fontSize6);
            paragraphMarkRunProperties4.Append(fontSizeComplexScript6);
            paragraphMarkRunProperties4.Append(languages6);

            paragraphProperties4.Append(suppressAutoHyphens4);
            paragraphProperties4.Append(snapToGrid2);
            paragraphProperties4.Append(spacingBetweenLines4);
            paragraphProperties4.Append(paragraphMarkRunProperties4);

            paragraph4.Append(paragraphProperties4);

            tableCell3.Append(tableCellProperties3);
            tableCell3.Append(paragraph4);

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "871", Type = TableWidthUnitValues.Dxa };
            Shading shading4 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "F58426" };

            tableCellProperties4.Append(tableCellWidth4);
            tableCellProperties4.Append(shading4);

            Paragraph paragraph5 = new Paragraph() { RsidParagraphMarkRevision = "00682DA0", RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "57DFB535", TextId = "77777777" };

            ParagraphProperties paragraphProperties5 = new ParagraphProperties();
            SuppressAutoHyphens suppressAutoHyphens5 = new SuppressAutoHyphens();
            SnapToGrid snapToGrid3 = new SnapToGrid() { Val = false };
            SpacingBetweenLines spacingBetweenLines5 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            ParagraphMarkRunProperties paragraphMarkRunProperties5 = new ParagraphMarkRunProperties();
            RunFonts runFonts7 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Verdana" };
            Color color7 = new Color() { Val = "000000" };
            FontSize fontSize7 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript7 = new FontSizeComplexScript() { Val = "20" };
            Languages languages7 = new Languages() { Val = "en-AU", EastAsia = "en-AU" };

            paragraphMarkRunProperties5.Append(runFonts7);
            paragraphMarkRunProperties5.Append(color7);
            paragraphMarkRunProperties5.Append(fontSize7);
            paragraphMarkRunProperties5.Append(fontSizeComplexScript7);
            paragraphMarkRunProperties5.Append(languages7);

            paragraphProperties5.Append(suppressAutoHyphens5);
            paragraphProperties5.Append(snapToGrid3);
            paragraphProperties5.Append(spacingBetweenLines5);
            paragraphProperties5.Append(paragraphMarkRunProperties5);

            paragraph5.Append(paragraphProperties5);

            tableCell4.Append(tableCellProperties4);
            tableCell4.Append(paragraph5);

            tableRow2.Append(tableRowProperties2);
            tableRow2.Append(tableCell3);
            tableRow2.Append(tableCell4);

            table1.Append(tableProperties1);
            table1.Append(tableGrid1);
            table1.Append(tableRow1);
            table1.Append(tableRow2);
            body.Append(table1);
        }

        private void AddPara1(Body body)
        {

            Paragraph paragraph1 = new Paragraph() { RsidParagraphMarkRevision = "00682DA0", RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "7DADCC70", TextId = "77777777" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            SuppressAutoHyphens suppressAutoHyphens1 = new SuppressAutoHyphens();
            SpacingBetweenLines spacingBetweenLines1 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Indentation indentation1 = new Indentation() { Left = "-112", Right = "162" };
            Justification justification1 = new Justification() { Val = JustificationValues.Both };

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            RunFonts runFonts1 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Segoe UI Light" };
            Color color1 = new Color() { Val = "9FA1A4" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "18" };
            Languages languages1 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            paragraphMarkRunProperties1.Append(runFonts1);
            paragraphMarkRunProperties1.Append(color1);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript1);
            paragraphMarkRunProperties1.Append(languages1);

            paragraphProperties1.Append(suppressAutoHyphens1);
            paragraphProperties1.Append(spacingBetweenLines1);
            paragraphProperties1.Append(indentation1);
            paragraphProperties1.Append(justification1);
            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run() { RsidRunProperties = "00682DA0" };

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts2 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Segoe UI Light" };
            Color color2 = new Color() { Val = "9FA1A4" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "22" };
            Languages languages2 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            runProperties1.Append(runFonts2);
            runProperties1.Append(color2);
            runProperties1.Append(fontSizeComplexScript2);
            runProperties1.Append(languages2);
            Text text1 = new Text() { Space = SpaceProcessingModeValues.Preserve };
            text1.Text = "When choosing an insurance product to suit your needs, we research the costs and features of various products.  ";

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);
            body.Append(paragraph1);
        }
        private void AddPara2(Body body)
        {
            Paragraph paragraph1 = new Paragraph() { RsidParagraphMarkRevision = "00682DA0", RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "12F282DF", TextId = "77777777" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            SuppressAutoHyphens suppressAutoHyphens1 = new SuppressAutoHyphens();
            SpacingBetweenLines spacingBetweenLines1 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Indentation indentation1 = new Indentation() { Left = "-112", Right = "162" };
            Justification justification1 = new Justification() { Val = JustificationValues.Both };

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            RunFonts runFonts1 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Segoe UI Light" };
            Color color1 = new Color() { Val = "9FA1A4" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "18" };
            Languages languages1 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            paragraphMarkRunProperties1.Append(runFonts1);
            paragraphMarkRunProperties1.Append(color1);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript1);
            paragraphMarkRunProperties1.Append(languages1);

            paragraphProperties1.Append(suppressAutoHyphens1);
            paragraphProperties1.Append(spacingBetweenLines1);
            paragraphProperties1.Append(indentation1);
            paragraphProperties1.Append(justification1);
            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run() { RsidRunProperties = "00682DA0" };

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts2 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Segoe UI Light" };
            Color color2 = new Color() { Val = "9FA1A4" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "18" };
            Languages languages2 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            runProperties1.Append(runFonts2);
            runProperties1.Append(color2);
            runProperties1.Append(fontSizeComplexScript2);
            runProperties1.Append(languages2);
            Text text1 = new Text();
            text1.Text = "In preparing your advice our research focuses on the features and benefits that are important to your situation.";

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);
            body.Append(paragraph1);
        }
        private static void AlterTableType(List<Table> t, List<Paragraph> p, WordprocessingDocument document)
        {




            foreach (Table table in t)
            {
                var tableProp = table.Descendants<TableProperties>().ToList().FirstOrDefault();
                if (tableProp != null)
                {
                    TableCellMarginDefault tableCellMarginDefault1 = new TableCellMarginDefault();
                    TopMargin topMargin1 = new TopMargin() { Width = "15", Type = TableWidthUnitValues.Dxa };
                    TableCellLeftMargin tableCellLeftMargin1 = new TableCellLeftMargin() { Width = 85, Type = TableWidthValues.Dxa };
                    BottomMargin bottomMargin1 = new BottomMargin() { Width = "15", Type = TableWidthUnitValues.Dxa };
                    TableCellRightMargin tableCellRightMargin1 = new TableCellRightMargin() { Width = 85, Type = TableWidthValues.Dxa };
                    //TableLayout tl = new TableLayout() { Type = TableLayoutValues.Fixed };
                    //tableProp.Append(tl);

                    tableCellMarginDefault1.Append(topMargin1);
                    tableCellMarginDefault1.Append(tableCellLeftMargin1);
                    tableCellMarginDefault1.Append(bottomMargin1);
                    tableCellMarginDefault1.Append(tableCellRightMargin1);

                    var tableMarg = tableProp.Descendants<TableCellMarginDefault>().ToList().FirstOrDefault();
                    if (tableMarg != null)
                    {
                        tableProp.ReplaceChild<TableCellMarginDefault>(tableCellMarginDefault1, tableMarg);
                    }
                    else
                    {

                        tableProp.Append(tableCellMarginDefault1);
                    }
                }
                else
                {
                    TableProperties tableProperties1 = new TableProperties();
                    TableCellMarginDefault tableCellMarginDefault1 = new TableCellMarginDefault();
                    TopMargin topMargin1 = new TopMargin() { Width = "15", Type = TableWidthUnitValues.Dxa };
                    TableCellLeftMargin tableCellLeftMargin1 = new TableCellLeftMargin() { Width = 85, Type = TableWidthValues.Dxa };
                    BottomMargin bottomMargin1 = new BottomMargin() { Width = "15", Type = TableWidthUnitValues.Dxa };
                    TableCellRightMargin tableCellRightMargin1 = new TableCellRightMargin() { Width = 85, Type = TableWidthValues.Dxa };
                    //TableLayout tl = new TableLayout() { Type = TableLayoutValues.Fixed };

                    tableCellMarginDefault1.Append(topMargin1);
                    tableCellMarginDefault1.Append(tableCellLeftMargin1);
                    tableCellMarginDefault1.Append(bottomMargin1);
                    tableCellMarginDefault1.Append(tableCellRightMargin1);
                    tableProperties1.Append(tableCellMarginDefault1);
                    // tableProperties1.Append(tl);
                }


                foreach (TableRow row in table.Descendants<TableRow>())
                {

                    var subRun = row.Descendants<Paragraph>().ToList();
                    foreach (Paragraph run in subRun)
                    {
                        var subRnProp = run.Descendants<ParagraphProperties>().ToList().FirstOrDefault();
                        // SpacingBetweenLines spacing = new SpacingBetweenLines() { LineRule = LineSpacingRuleValues.Auto, Before = "6", After = "6" };
                        SpacingBetweenLines spacingBetweenLines1 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

                        if (subRnProp != null)
                        {
                            var space = subRnProp.Descendants<SpacingBetweenLines>().FirstOrDefault();
                            if (space != null)
                            {
                                subRnProp.ReplaceChild<SpacingBetweenLines>(spacingBetweenLines1, space);
                            }
                            else
                            {
                                subRnProp.AppendChild<SpacingBetweenLines>(spacingBetweenLines1);
                                //run.AppendChild<RunProperties>(subRnProp);
                            }


                        }
                        else
                        {
                            var tmpSubRunProp = new ParagraphProperties();
                            tmpSubRunProp.AppendChild<SpacingBetweenLines>(spacingBetweenLines1);
                            run.AppendChild<ParagraphProperties>(tmpSubRunProp);
                        }




                    }



                    var tableRowProp = row.Descendants<TableRowProperties>().ToList().FirstOrDefault();
                    if (tableRowProp != null)
                    {
                        TableRowHeight tableRowHeight1 = new TableRowHeight() { Val = (UInt32Value)312U };
                        var tableP = tableRowProp.Descendants<TableRowHeight>().ToList().FirstOrDefault();
                        if (tableP != null)
                        {
                            tableRowProp.ReplaceChild<TableRowHeight>(tableRowHeight1, tableP);
                        }
                        else
                        {

                            tableRowProp.Append(tableRowHeight1);
                        }
                    }
                    else
                    {
                        TableRowProperties trp = new TableRowProperties();
                        // CantSplit split = new CantSplit();
                        TableRowHeight tableRowHeight1 = new TableRowHeight() { Val = (UInt32Value)312U };
                        // trp.Append(split);
                        trp.Append(tableRowHeight1);
                        row.AppendChild(trp);
                    }
                }
            }



            foreach (Paragraph para in p)
            {


                ParagraphProperties spacingProp = para.Descendants<ParagraphProperties>().ToList().FirstOrDefault();
                SpacingBetweenLines spacingBetweenLines1 = new SpacingBetweenLines() { Before = "120", After = "120", Line = "240", LineRule = LineSpacingRuleValues.Auto };
                Justification justification1 = new Justification() { Val = JustificationValues.Both };

                ParagraphProperties pp = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "0" });



                if (spacingProp != null)
                {
                    var sp = spacingProp.Descendants<SpacingBetweenLines>().FirstOrDefault();
                    if (sp == null)
                    {
                        spacingProp.Append((SpacingBetweenLines)spacingBetweenLines1.CloneNode(true));
                    }

                    var js = spacingProp.Descendants<Justification>().FirstOrDefault();
                    if (js == null)
                    {
                        spacingProp.Append((Justification)justification1.CloneNode(true));
                    }

                }
                else
                {

                    ParagraphProperties tmpSubRunProp = new ParagraphProperties();
                    tmpSubRunProp.AppendChild((SpacingBetweenLines)spacingBetweenLines1.CloneNode(true));
                    tmpSubRunProp.AppendChild((Justification)justification1.CloneNode(true));

                    para.PrependChild<ParagraphProperties>((ParagraphProperties)tmpSubRunProp.CloneNode(true));
                }

                var subRun = para.Descendants<Run>().ToList();
                foreach (Run run in subRun)
                {
                    var subRnProp = run.Descendants<RunProperties>().ToList().FirstOrDefault();
                    var newFont = new RunFonts();
                    newFont.Ascii = "Verdana";
                    newFont.EastAsia = "Verdana";

                    var fontSize = new FontSize();
                    fontSize.Val = "18";

                    if (subRnProp != null)
                    {
                        var font = subRnProp.Descendants<RunFonts>().FirstOrDefault();
                        if (font != null)
                        {
                            subRnProp.ReplaceChild<RunFonts>(newFont, font);
                        }
                        else
                        {
                            subRnProp.PrependChild<RunFonts>(newFont);
                            //run.AppendChild<RunProperties>(subRnProp);
                        }

                        var fonts = subRnProp.Descendants<FontSize>().FirstOrDefault();
                        if (fonts != null)
                        {
                            // subRnProp.ReplaceChild<FontSize>(fontSize, fonts);
                        }
                        else
                        {
                            subRnProp.PrependChild<FontSize>(fontSize);
                            //run.AppendChild<RunProperties>(subRnProp);
                        }

                    }
                    else
                    {
                        var tmpSubRunProp = new RunProperties();
                        tmpSubRunProp.AppendChild<RunFonts>(newFont);
                        tmpSubRunProp.AppendChild<FontSize>(fontSize);
                        run.PrependChild<RunProperties>(tmpSubRunProp);
                    }




                }
            }




        }
        private void GeneratePartContent(FooterPart part)
        {
            Footer footer1 = new Footer() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "w14 w15 wp14" } };
            footer1.AddNamespaceDeclaration("wpc", "http://schemas.microsoft.com/office/word/2010/wordprocessingCanvas");
            footer1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            footer1.AddNamespaceDeclaration("o", "urn:schemas-microsoft-com:office:office");
            footer1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            footer1.AddNamespaceDeclaration("m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
            footer1.AddNamespaceDeclaration("v", "urn:schemas-microsoft-com:vml");
            footer1.AddNamespaceDeclaration("wp14", "http://schemas.microsoft.com/office/word/2010/wordprocessingDrawing");
            footer1.AddNamespaceDeclaration("wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing");
            footer1.AddNamespaceDeclaration("w10", "urn:schemas-microsoft-com:office:word");
            footer1.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            footer1.AddNamespaceDeclaration("w14", "http://schemas.microsoft.com/office/word/2010/wordml");
            footer1.AddNamespaceDeclaration("w15", "http://schemas.microsoft.com/office/word/2012/wordml");
            footer1.AddNamespaceDeclaration("wpg", "http://schemas.microsoft.com/office/word/2010/wordprocessingGroup");
            footer1.AddNamespaceDeclaration("wpi", "http://schemas.microsoft.com/office/word/2010/wordprocessingInk");
            footer1.AddNamespaceDeclaration("wne", "http://schemas.microsoft.com/office/word/2006/wordml");
            footer1.AddNamespaceDeclaration("wps", "http://schemas.microsoft.com/office/word/2010/wordprocessingShape");

            Paragraph paragraph1 = new Paragraph() { RsidParagraphAddition = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "332566C5", TextId = "77777777" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            FontSize fontSize1 = new FontSize() { Val = "16" };

            paragraphMarkRunProperties1.Append(fontSize1);

            paragraphProperties1.Append(paragraphMarkRunProperties1);

            paragraph1.Append(paragraphProperties1);

            Table table1 = new Table();

            TableProperties tableProperties1 = new TableProperties();
            TableWidth tableWidth1 = new TableWidth() { Width = "0", Type = TableWidthUnitValues.Auto };
            TableIndentation tableIndentation1 = new TableIndentation() { Width = -318, Type = TableWidthUnitValues.Dxa };
            //  TableLayout tableLayout1 = new TableLayout() { Type = TableLayoutValues.Fixed };

            TableCellMarginDefault tableCellMarginDefault1 = new TableCellMarginDefault();
            TableCellRightMargin tableCellRightMargin1 = new TableCellRightMargin() { Width = 0, Type = TableWidthValues.Dxa };

            tableCellMarginDefault1.Append(tableCellRightMargin1);
            TableLook tableLook1 = new TableLook() { Val = "0000", FirstRow = false, LastRow = false, FirstColumn = false, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = false };

            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableIndentation1);
            // tableProperties1.Append(tableLayout1);
            tableProperties1.Append(tableCellMarginDefault1);
            tableProperties1.Append(tableLook1);

            TableGrid tableGrid1 = new TableGrid();
            GridColumn gridColumn1 = new GridColumn() { Width = "568" };
            GridColumn gridColumn2 = new GridColumn() { Width = "8081" };
            GridColumn gridColumn3 = new GridColumn() { Width = "1415" };

            tableGrid1.Append(gridColumn1);
            tableGrid1.Append(gridColumn2);
            tableGrid1.Append(gridColumn3);

            TableRow tableRow1 = new TableRow() { RsidTableRowMarkRevision = "00682DA0", RsidTableRowAddition = "00682DA0", RsidTableRowProperties = "00F76E86", ParagraphId = "08E0D757", TextId = "77777777" };

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "568", Type = TableWidthUnitValues.Dxa };
            Shading shading1 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "auto" };
            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Bottom };

            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(shading1);
            tableCellProperties1.Append(tableCellVerticalAlignment1);

            Paragraph paragraph2 = new Paragraph() { RsidParagraphMarkRevision = "00682DA0", RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "28E68A06", TextId = "77777777" };

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();

            Tabs tabs1 = new Tabs();
            TabStop tabStop1 = new TabStop() { Val = TabStopValues.Center, Position = 4513 };
            TabStop tabStop2 = new TabStop() { Val = TabStopValues.Right, Position = 9026 };

            tabs1.Append(tabStop1);
            tabs1.Append(tabStop2);
            SuppressAutoHyphens suppressAutoHyphens1 = new SuppressAutoHyphens();
            SpacingBetweenLines spacingBetweenLines1 = new SpacingBetweenLines() { After = "0", Line = "100", LineRule = LineSpacingRuleValues.AtLeast };
            Justification justification1 = new Justification() { Val = JustificationValues.Right };

            ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
            RunFonts runFonts1 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Verdana" };
            FontSize fontSize2 = new FontSize() { Val = "16" };
            Languages languages1 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            paragraphMarkRunProperties2.Append(runFonts1);
            paragraphMarkRunProperties2.Append(fontSize2);
            paragraphMarkRunProperties2.Append(languages1);

            paragraphProperties2.Append(tabs1);
            paragraphProperties2.Append(suppressAutoHyphens1);
            paragraphProperties2.Append(spacingBetweenLines1);
            paragraphProperties2.Append(justification1);
            paragraphProperties2.Append(paragraphMarkRunProperties2);

            Run run1 = new Run() { RsidRunProperties = "00682DA0" };

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts2 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Calibri" };
            Color color1 = new Color() { Val = "F58426" };
            FontSize fontSize3 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "18" };
            Languages languages2 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            runProperties1.Append(runFonts2);
            runProperties1.Append(color1);
            runProperties1.Append(fontSize3);
            runProperties1.Append(fontSizeComplexScript1);
            runProperties1.Append(languages2);
            FieldChar fieldChar1 = new FieldChar() { FieldCharType = FieldCharValues.Begin };

            run1.Append(runProperties1);
            run1.Append(fieldChar1);

            Run run2 = new Run() { RsidRunProperties = "00682DA0" };

            RunProperties runProperties2 = new RunProperties();
            RunFonts runFonts3 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Calibri" };
            Color color2 = new Color() { Val = "F58426" };
            FontSize fontSize4 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "18" };
            Languages languages3 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            runProperties2.Append(runFonts3);
            runProperties2.Append(color2);
            runProperties2.Append(fontSize4);
            runProperties2.Append(fontSizeComplexScript2);
            runProperties2.Append(languages3);
            FieldCode fieldCode1 = new FieldCode() { Space = SpaceProcessingModeValues.Preserve };
            fieldCode1.Text = " PAGE ";

            run2.Append(runProperties2);
            run2.Append(fieldCode1);

            Run run3 = new Run() { RsidRunProperties = "00682DA0" };

            RunProperties runProperties3 = new RunProperties();
            RunFonts runFonts4 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Calibri" };
            Color color3 = new Color() { Val = "F58426" };
            FontSize fontSize5 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript3 = new FontSizeComplexScript() { Val = "18" };
            Languages languages4 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            runProperties3.Append(runFonts4);
            runProperties3.Append(color3);
            runProperties3.Append(fontSize5);
            runProperties3.Append(fontSizeComplexScript3);
            runProperties3.Append(languages4);
            FieldChar fieldChar2 = new FieldChar() { FieldCharType = FieldCharValues.Separate };

            run3.Append(runProperties3);
            run3.Append(fieldChar2);

            Run run4 = new Run() { RsidRunAddition = "008818E9" };

            RunProperties runProperties4 = new RunProperties();
            RunFonts runFonts5 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Calibri" };
            NoProof noProof1 = new NoProof();
            Color color4 = new Color() { Val = "F58426" };
            FontSize fontSize6 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript4 = new FontSizeComplexScript() { Val = "18" };
            Languages languages5 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            runProperties4.Append(runFonts5);
            runProperties4.Append(noProof1);
            runProperties4.Append(color4);
            runProperties4.Append(fontSize6);
            runProperties4.Append(fontSizeComplexScript4);
            runProperties4.Append(languages5);
            Text text1 = new Text();
            text1.Text = "1";

            run4.Append(runProperties4);
            run4.Append(text1);

            Run run5 = new Run() { RsidRunProperties = "00682DA0" };

            RunProperties runProperties5 = new RunProperties();
            RunFonts runFonts6 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Calibri" };
            Color color5 = new Color() { Val = "F58426" };
            FontSize fontSize7 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript5 = new FontSizeComplexScript() { Val = "18" };
            Languages languages6 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            runProperties5.Append(runFonts6);
            runProperties5.Append(color5);
            runProperties5.Append(fontSize7);
            runProperties5.Append(fontSizeComplexScript5);
            runProperties5.Append(languages6);
            FieldChar fieldChar3 = new FieldChar() { FieldCharType = FieldCharValues.End };

            run5.Append(runProperties5);
            run5.Append(fieldChar3);

            Run run6 = new Run() { RsidRunProperties = "00682DA0" };

            RunProperties runProperties6 = new RunProperties();
            RunFonts runFonts7 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Verdana", ComplexScript = "Verdana" };
            Color color6 = new Color() { Val = "72CDF4" };
            FontSize fontSize8 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript6 = new FontSizeComplexScript() { Val = "18" };
            Languages languages7 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            runProperties6.Append(runFonts7);
            runProperties6.Append(color6);
            runProperties6.Append(fontSize8);
            runProperties6.Append(fontSizeComplexScript6);
            runProperties6.Append(languages7);
            Text text2 = new Text() { Space = SpaceProcessingModeValues.Preserve };
            text2.Text = " ";

            run6.Append(runProperties6);
            run6.Append(text2);

            Run run7 = new Run() { RsidRunProperties = "00682DA0" };

            RunProperties runProperties7 = new RunProperties();
            RunFonts runFonts8 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Calibri" };
            Color color7 = new Color() { Val = "808080" };
            FontSize fontSize9 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript7 = new FontSizeComplexScript() { Val = "18" };
            Languages languages8 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            runProperties7.Append(runFonts8);
            runProperties7.Append(color7);
            runProperties7.Append(fontSize9);
            runProperties7.Append(fontSizeComplexScript7);
            runProperties7.Append(languages8);
            Text text3 = new Text();
            text3.Text = "|";

            run7.Append(runProperties7);
            run7.Append(text3);

            Run run8 = new Run() { RsidRunProperties = "00682DA0" };

            RunProperties runProperties8 = new RunProperties();
            RunFonts runFonts9 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Calibri" };
            Color color8 = new Color() { Val = "72CDF4" };
            FontSize fontSize10 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript8 = new FontSizeComplexScript() { Val = "18" };
            Languages languages9 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            runProperties8.Append(runFonts9);
            runProperties8.Append(color8);
            runProperties8.Append(fontSize10);
            runProperties8.Append(fontSizeComplexScript8);
            runProperties8.Append(languages9);
            Text text4 = new Text() { Space = SpaceProcessingModeValues.Preserve };
            text4.Text = " ";

            run8.Append(runProperties8);
            run8.Append(text4);

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run1);
            paragraph2.Append(run2);
            paragraph2.Append(run3);
            paragraph2.Append(run4);
            paragraph2.Append(run5);
            paragraph2.Append(run6);
            paragraph2.Append(run7);
            paragraph2.Append(run8);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph2);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "8081", Type = TableWidthUnitValues.Dxa };
            Shading shading2 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "auto" };
            TableCellVerticalAlignment tableCellVerticalAlignment2 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Bottom };

            tableCellProperties2.Append(tableCellWidth2);
            tableCellProperties2.Append(shading2);
            tableCellProperties2.Append(tableCellVerticalAlignment2);

            Paragraph paragraph3 = new Paragraph() { RsidParagraphMarkRevision = "00682DA0", RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "2F49C805", TextId = "74311658" };

            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            SuppressAutoHyphens suppressAutoHyphens2 = new SuppressAutoHyphens();
            SpacingBetweenLines spacingBetweenLines2 = new SpacingBetweenLines() { After = "40", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            ParagraphMarkRunProperties paragraphMarkRunProperties3 = new ParagraphMarkRunProperties();
            RunFonts runFonts10 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Verdana" };
            Color color9 = new Color() { Val = "FF0000" };
            FontSize fontSize11 = new FontSize() { Val = "16" };
            Languages languages10 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            paragraphMarkRunProperties3.Append(runFonts10);
            paragraphMarkRunProperties3.Append(color9);
            paragraphMarkRunProperties3.Append(fontSize11);
            paragraphMarkRunProperties3.Append(languages10);

            paragraphProperties3.Append(suppressAutoHyphens2);
            paragraphProperties3.Append(spacingBetweenLines2);
            paragraphProperties3.Append(paragraphMarkRunProperties3);

            Run run9 = new Run() { RsidRunProperties = "00682DA0" };

            RunProperties runProperties9 = new RunProperties();
            RunFonts runFonts11 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Verdana" };
            FontSize fontSize12 = new FontSize() { Val = "16" };
            Languages languages11 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            runProperties9.Append(runFonts11);
            runProperties9.Append(fontSize12);
            runProperties9.Append(languages11);
            Text text5 = new Text() { Space = SpaceProcessingModeValues.Preserve };
            text5.Text = "Confidential Statement of Advice Prepared for :";

            run9.Append(runProperties9);
            run9.Append(text5);

            Run run10 = new Run() { RsidRunProperties = "00682DA0" };

            RunProperties runProperties10 = new RunProperties();
            RunFonts runFonts12 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Verdana" };
            Color color10 = new Color() { Val = "FF0000" };
            FontSize fontSize13 = new FontSize() { Val = "16" };
            Languages languages12 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            runProperties10.Append(runFonts12);
            runProperties10.Append(color10);
            runProperties10.Append(fontSize13);
            runProperties10.Append(languages12);
            Text text6 = new Text();
            text6.Text = "[Client/Partner Name]";

            run10.Append(runProperties10);
            run10.Append(text6);

            paragraph3.Append(paragraphProperties3);
            paragraph3.Append(run9);
            paragraph3.Append(run10);

            Paragraph paragraph4 = new Paragraph() { RsidParagraphMarkRevision = "00682DA0", RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "3C8B385D", TextId = "77777777" };

            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            SuppressAutoHyphens suppressAutoHyphens3 = new SuppressAutoHyphens();
            SpacingBetweenLines spacingBetweenLines3 = new SpacingBetweenLines() { After = "40", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            ParagraphMarkRunProperties paragraphMarkRunProperties4 = new ParagraphMarkRunProperties();
            RunFonts runFonts13 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Verdana" };
            FontSize fontSize14 = new FontSize() { Val = "14" };
            Languages languages13 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            paragraphMarkRunProperties4.Append(runFonts13);
            paragraphMarkRunProperties4.Append(fontSize14);
            paragraphMarkRunProperties4.Append(languages13);

            paragraphProperties4.Append(suppressAutoHyphens3);
            paragraphProperties4.Append(spacingBetweenLines3);
            paragraphProperties4.Append(paragraphMarkRunProperties4);

            Run run11 = new Run() { RsidRunProperties = "00682DA0" };

            RunProperties runProperties11 = new RunProperties();
            RunFonts runFonts14 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Verdana" };
            FontSize fontSize15 = new FontSize() { Val = "16" };
            Languages languages14 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            runProperties11.Append(runFonts14);
            runProperties11.Append(fontSize15);
            runProperties11.Append(languages14);
            Text text7 = new Text() { Space = SpaceProcessingModeValues.Preserve };
            text7.Text = "Provided by: ";

            run11.Append(runProperties11);
            run11.Append(text7);

            Run run12 = new Run() { RsidRunProperties = "00682DA0" };

            RunProperties runProperties12 = new RunProperties();
            RunFonts runFonts15 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Verdana" };
            Color color11 = new Color() { Val = "FF0000" };
            FontSize fontSize16 = new FontSize() { Val = "16" };
            Languages languages15 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            runProperties12.Append(runFonts15);
            runProperties12.Append(color11);
            runProperties12.Append(fontSize16);
            runProperties12.Append(languages15);
            Text text8 = new Text();
            text8.Text = "[Practice]";

            run12.Append(runProperties12);
            run12.Append(text8);

            Run run13 = new Run() { RsidRunProperties = "00682DA0" };

            RunProperties runProperties13 = new RunProperties();
            RunFonts runFonts16 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Verdana" };
            FontSize fontSize17 = new FontSize() { Val = "16" };
            Languages languages16 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            runProperties13.Append(runFonts16);
            runProperties13.Append(fontSize17);
            runProperties13.Append(languages16);
            Text text9 = new Text();
            text9.Text = ", a Corporate Authorised Representative of RI Advice Group Pty Ltd";

            run13.Append(runProperties13);
            run13.Append(text9);

            paragraph4.Append(paragraphProperties4);
            paragraph4.Append(run11);
            paragraph4.Append(run12);
            paragraph4.Append(run13);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph3);
            tableCell2.Append(paragraph4);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "1415", Type = TableWidthUnitValues.Dxa };
            Shading shading3 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "auto" };
            TableCellVerticalAlignment tableCellVerticalAlignment3 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Bottom };

            tableCellProperties3.Append(tableCellWidth3);
            tableCellProperties3.Append(shading3);
            tableCellProperties3.Append(tableCellVerticalAlignment3);

            Paragraph paragraph5 = new Paragraph() { RsidParagraphMarkRevision = "00682DA0", RsidParagraphAddition = "00682DA0", RsidParagraphProperties = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "43FE2BBD", TextId = "77777777" };

            ParagraphProperties paragraphProperties5 = new ParagraphProperties();

            Tabs tabs2 = new Tabs();
            TabStop tabStop3 = new TabStop() { Val = TabStopValues.Left, Position = 7080 };

            tabs2.Append(tabStop3);
            SuppressAutoHyphens suppressAutoHyphens4 = new SuppressAutoHyphens();
            SnapToGrid snapToGrid1 = new SnapToGrid() { Val = false };
            SpacingBetweenLines spacingBetweenLines4 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Justification justification2 = new Justification() { Val = JustificationValues.Right };

            ParagraphMarkRunProperties paragraphMarkRunProperties5 = new ParagraphMarkRunProperties();
            RunFonts runFonts17 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana", EastAsia = "Calibri", ComplexScript = "Verdana" };
            FontSize fontSize18 = new FontSize() { Val = "14" };
            Languages languages17 = new Languages() { Val = "en-AU", EastAsia = "zh-CN" };

            paragraphMarkRunProperties5.Append(runFonts17);
            paragraphMarkRunProperties5.Append(fontSize18);
            paragraphMarkRunProperties5.Append(languages17);

            paragraphProperties5.Append(tabs2);
            paragraphProperties5.Append(suppressAutoHyphens4);
            paragraphProperties5.Append(snapToGrid1);
            paragraphProperties5.Append(spacingBetweenLines4);
            paragraphProperties5.Append(justification2);
            paragraphProperties5.Append(paragraphMarkRunProperties5);

            paragraph5.Append(paragraphProperties5);

            tableCell3.Append(tableCellProperties3);
            tableCell3.Append(paragraph5);

            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);
            tableRow1.Append(tableCell3);

            table1.Append(tableProperties1);
            table1.Append(tableGrid1);
            table1.Append(tableRow1);

            Paragraph paragraph6 = new Paragraph() { RsidParagraphAddition = "00682DA0", RsidRunAdditionDefault = "00682DA0", ParagraphId = "08103970", TextId = "77777777" };

            ParagraphProperties paragraphProperties6 = new ParagraphProperties();
            ParagraphStyleId paragraphStyleId1 = new ParagraphStyleId() { Val = "Footer" };

            Tabs tabs3 = new Tabs();
            TabStop tabStop4 = new TabStop() { Val = TabStopValues.Left, Position = 3433 };

            tabs3.Append(tabStop4);

            paragraphProperties6.Append(paragraphStyleId1);
            paragraphProperties6.Append(tabs3);

            Run run14 = new Run();

            RunProperties runProperties14 = new RunProperties();
            FontSize fontSize19 = new FontSize() { Val = "2" };
            FontSizeComplexScript fontSizeComplexScript9 = new FontSizeComplexScript() { Val = "2" };

            runProperties14.Append(fontSize19);
            runProperties14.Append(fontSizeComplexScript9);
            TabChar tabChar1 = new TabChar();

            run14.Append(runProperties14);
            run14.Append(tabChar1);

            paragraph6.Append(paragraphProperties6);
            paragraph6.Append(run14);

            footer1.Append(paragraph1);
            footer1.Append(table1);
            footer1.Append(paragraph6);

            part.Footer = footer1;
        }
        public void AddRiskProtection(Body body, BasicDetails details, IEnumerable<CurrentInsuranceViewModel> currentInsurance, Color orange, int type)
        {


            if (details != null && currentInsurance.Count() > 0)
            {

                Paragraph s1 = body.AppendChild(new Paragraph());
                Run r1 = s1.AppendChild(new Run());
                RunProperties runProperties1 = r1.AppendChild(new RunProperties(new Bold(), new RunFonts { Ascii = "Verdana" }, new FontSize { Val = new StringValue("20") }));
                Color black = new Color() { Val = "000000" };
                runProperties1.AppendChild(orange.CloneNode(true));

                if (type == 0)
                {
                    r1.AppendChild(new Text("Risk protection cover for " + details.ClientName));
                }
                else
                {
                    r1.AppendChild(new Text("Risk protection cover for " + details.PartnerName));
                }


                foreach (CurrentInsuranceViewModel current in currentInsurance)
                {
                    //New Table

                    Table table = body.AppendChild(new Table());
                    TableProperties tableProp = new TableProperties();
                    TableStyle tableStyle = new TableStyle() { Val = "TableGrid" };

                    TableWidth tableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };

                    tableProp.Append(tableStyle, tableWidth);
                    table.AppendChild(tableProp);


                    ////Header
                    TableRow header = new TableRow();
                    TableCell h1 = new TableCell();

                    TableCellProperties hcp = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                    TableCellProperties hcp1 = new TableCellProperties(new TableCellWidth { Width = "2390", Type = TableWidthUnitValues.Auto }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });

                    ParagraphProperties hpp = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });
                    Justification leftJustify = new Justification() { Val = JustificationValues.Left };
                    Justification centerJustify = new Justification() { Val = JustificationValues.Center };
                    hpp.AppendChild((Justification)leftJustify.CloneNode(true));

                    TableCellBorders hcb = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 20, Color = "ED7D27" } };

                    Shading hs1 = new Shading() { Color = "auto", Fill = "393939", Val = ShadingPatternValues.Clear };
                    TableCellMargin hcm1 = new TableCellMargin() { RightMargin = new RightMargin() { Width = "50", Type = TableWidthUnitValues.Dxa }, LeftMargin = new LeftMargin() { Width = "50", Type = TableWidthUnitValues.Dxa } };

                    hcp.Append(hcm1, hs1, hcb);
                    hcp1.Append((TableCellMargin)hcm1.CloneNode(true), (Shading)hs1.CloneNode(true), (TableCellBorders)hcb.CloneNode(true));
                    h1.Append(hcp);

                    var coverType = "";
                    List<string> policyOwner = new List<string>();
                    if (current.LifeCover.Any())
                    {
                        coverType += "/Life";
                        policyOwner.Add(current.LifeCover[0].PolicyOwner);
                    }
                    if (current.TpdCover.Any())
                    {
                        coverType += "/TPD";
                        policyOwner.Add(current.TpdCover[0].PolicyOwner);
                    }
                    if (current.TraumaCover.Any())
                    {
                        coverType += "/Trauma";
                        policyOwner.Add(current.TraumaCover[0].PolicyOwner);
                    }
                    if (current.IncomeCover.Any())
                    {
                        coverType += "/Income";
                        policyOwner.Add(current.IncomeCover[0].PolicyOwner);
                    }

                    h1.AppendChild((ParagraphProperties)hpp.CloneNode(true));
                    h1.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(current.Provider + (coverType != ""  ? " - " + coverType.Substring(1) + " Cover "  : "" )))));


                    TableCell h2 = new TableCell();
                    h2.AppendChild((ParagraphProperties)hpp.CloneNode(true));
                    h2.Append((TableCellProperties)hcp1.CloneNode(true));
                    h2.Append(new Paragraph(new Run(new Text(""))));
                    header.Append(h1, h2);


                    table.Append(header);
                    ////Body

                    //TableCellProperties tcp1 = new TableCellProperties(new TableCellWidth { Width = "2390", Type = TableWidthUnitValues.Auto }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                    TableCellProperties tcp = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                    TableCellProperties tcpN = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                    TableCellProperties tcpNone = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });


                    TableCellBorders tcbL = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, RightBorder = new RightBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };
                    TableCellBorders tcbR = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, LeftBorder = new LeftBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };
                    TableCellBorders tcbN = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };

                    TableCellMargin tcm = new TableCellMargin() { RightMargin = new RightMargin() { Width = "50", Type = TableWidthUnitValues.Dxa }, LeftMargin = new LeftMargin() { Width = "50", Type = TableWidthUnitValues.Dxa } };

                    ParagraphProperties pp1 = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "0" });
                    ParagraphProperties pp = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "0" });
                    pp.AppendChild((Justification)centerJustify.CloneNode(true));
                    tcp.Append(tcbL, tcm);
                    tcpN.Append((TableCellBorders)tcbR.CloneNode(true), (TableCellMargin)tcm.CloneNode(true));
                    tcpNone.Append((TableCellBorders)tcbN.CloneNode(true), (TableCellMargin)tcm.CloneNode(true));
                    //Policy Owner
                    if (current.Owner != "")
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell((TableCellProperties)tcp.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text("Policy Owner"))));
                        if (policyOwner.All(x => x == policyOwner.First()))
                        {
                            TableCell tc2 = new TableCell((TableCellProperties)tcpN.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text(policyOwner.First() == "superFund" ? "Super Fund" : (policyOwner.First() == "client" ? details.ClientName : details.PartnerName)))));
                            tr.Append(tc1, tc2);
                        }
                        else
                        {
                            TableCell tc2 = new TableCell((TableCellProperties)tcpN.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text("Super Fund") ,new Break(), type == 0 ? new Text(details.ClientName) : new Text(details.PartnerName))));
                            tr.Append(tc1, tc2);
                        }
                           
                        table.AppendChild(tr);
                    }
                    //Premium
                    if (current.FeeDetails.Count() > 0)
                    {
                        decimal sum = 0;
                        foreach (CurrentFeeDetailsViewModel fee in current.FeeDetails)
                        {

                            if (fee.FeeType == "premium")
                            {
                                if (fee.Frequency == "yearly")
                                {
                                    sum += fee.Amount;
                                }
                                else
                                {
                                    sum += (fee.Amount * 12);
                                }
                            }
                        }

                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell((TableCellProperties)tcp.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text("Premium"))));
                        TableCell tc2 = new TableCell((TableCellProperties)tcpN.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text("$" + String.Format("{0:n}", sum.ToString()) + " per annum"))));
                        tr.Append(tc1, tc2);
                        table.AppendChild(tr);
                    }
                    //Life
                    if (current.LifeCover.Count() > 0)
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell((TableCellProperties)tcp.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(new Bold()), new Text("Life Cover Benefit"))));
                        TableCell tc2 = new TableCell((TableCellProperties)tcpN.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(new Bold()), new Text("$" + String.Format("{0:n}", current.LifeCover[0].BenefitAmount.ToString())))));
                        tr.Append(tc1, tc2);
                        table.AppendChild(tr);

                        TableRow tr1 = new TableRow();
                        TableCell tc11 = new TableCell((TableCellProperties)tcp.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text("Within Super"))));
                        TableCell tc21 = new TableCell((TableCellProperties)tcpN.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text(current.LifeCover[0].WithinSuper == 1 ? "Yes" : "No"))));
                        tr1.Append(tc11, tc21);
                        table.AppendChild(tr1);
                    }
                    //TPD
                    if (current.TpdCover.Count() > 0)
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell((TableCellProperties)tcp.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(new Bold()), new Text("TPD Cover Benefit"))));
                        TableCell tc2 = new TableCell((TableCellProperties)tcpN.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(new Bold()), new Text("$" + current.TpdCover[0].BenefitAmount.ToString()))));
                        tr.Append(tc1, tc2);
                        table.AppendChild(tr);

                        TableRow tr2 = new TableRow();
                        TableCell tc12 = new TableCell((TableCellProperties)tcp.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text("Definition"))));
                        TableCell tc22 = new TableCell((TableCellProperties)tcpN.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text(current.TpdCover[0].Definition))));
                        tr2.Append(tc12, tc22);
                        table.AppendChild(tr2);

                        TableRow tr1 = new TableRow();
                        TableCell tc11 = new TableCell((TableCellProperties)tcp.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text("Within Super"))));
                        TableCell tc21 = new TableCell((TableCellProperties)tcpN.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text(current.TpdCover[0].WithinSuper))));
                        tr1.Append(tc11, tc21);
                        table.AppendChild(tr1);
                    }
                    //Trauma
                    if (current.TraumaCover.Count() > 0)
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell((TableCellProperties)tcp.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(new Bold()), new Text("Trauma Cover Benefit"))));
                        TableCell tc2 = new TableCell((TableCellProperties)tcpN.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(new Bold()), new Text("$" + current.TraumaCover[0].BenefitAmount.ToString()))));
                        tr.Append(tc1, tc2);
                        table.AppendChild(tr);

                        TableRow tr1 = new TableRow();
                        TableCell tc11 = new TableCell((TableCellProperties)tcp.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text("Within Super"))));
                        TableCell tc21 = new TableCell((TableCellProperties)tcpN.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text(current.TraumaCover[0].WithinSuper))));
                        tr1.Append(tc11, tc21);
                        table.AppendChild(tr1);
                    }
                    //Income
                    if (current.IncomeCover.Count() > 0)
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell((TableCellProperties)tcp.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(new Bold()), new Text("Income Cover Benefit"))));
                        TableCell tc2 = new TableCell((TableCellProperties)tcpN.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(new Bold()), new Text("$" + current.IncomeCover[0].MonthlyBenefitAmount.ToString() + " per month"))));
                        tr.Append(tc1, tc2);
                        table.AppendChild(tr);

                        TableRow tr3 = new TableRow();
                        TableCell tc13 = new TableCell((TableCellProperties)tcp.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text("Premium Type"))));
                        TableCell tc23 = new TableCell((TableCellProperties)tcpN.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text(current.IncomeCover[0].PremiumType))));
                        tr3.Append(tc13, tc23);
                        table.AppendChild(tr3);

                        TableRow tr2 = new TableRow();
                        TableCell tc12 = new TableCell((TableCellProperties)tcp.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text("Definition"))));
                        TableCell tc22 = new TableCell((TableCellProperties)tcpN.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text(current.IncomeCover[0].Definition))));
                        tr2.Append(tc12, tc22);
                        table.AppendChild(tr2);

                        TableRow tr4 = new TableRow();
                        TableCell tc14 = new TableCell((TableCellProperties)tcp.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text("Benefit Period"))));
                        TableCell tc24 = new TableCell((TableCellProperties)tcpN.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text(current.IncomeCover[0].BenefitPeriod))));
                        tr4.Append(tc14, tc24);
                        table.AppendChild(tr4);

                        TableRow tr5 = new TableRow();
                        TableCell tc15 = new TableCell((TableCellProperties)tcp.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text("Waiting Period"))));
                        TableCell tc25 = new TableCell((TableCellProperties)tcpN.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text(current.IncomeCover[0].WaitingPeriod))));
                        tr5.Append(tc15, tc25);
                        table.AppendChild(tr5);

                        TableRow tr1 = new TableRow();
                        TableCell tc11 = new TableCell((TableCellProperties)tcp.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text("Within Super"))));
                        TableCell tc21 = new TableCell((TableCellProperties)tcpN.CloneNode(true), (ParagraphProperties)pp1.CloneNode(true), new Paragraph(new Run(new RunProperties(), new Text(current.IncomeCover[0].WithinSuper == 1 ? "Yes" : "No"))));
                        tr1.Append(tc11, tc21);
                        table.AppendChild(tr1);
                    }

                    Run linebreak = new Run(new Break());
                    Paragraph paragraph = new Paragraph();
                    paragraph.Append(new OpenXmlElement[] { linebreak });
                    body.Append(paragraph);

                }
            }

        }
        public void AddInsuranceNeeds(Body body, NeedsAnalysisViewModel[] details, Color orange, string name)
        {

            Paragraph p1 = body.AppendChild(new Paragraph(new Run(new RunProperties(new Color() { Val = "000000" }), new Text("Our insurance recommendations are based on our analysis of your circumstances and financial situation. The following table illustrates your insurance needs."))));

            Run linebreak2 = new Run(new Break());

            if (details.Length > 0)
            {
                //New Table

                Table table = body.AppendChild(new Table());
                TableProperties tableProp = new TableProperties();
                TableStyle tableStyle = new TableStyle() { Val = "TableGrid" };

                TableWidth tableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };

                tableProp.Append(tableStyle, tableWidth);
                table.AppendChild(tableProp);


                ////Header
                TableRow header = new TableRow();
                TableCell h1 = new TableCell();

                TableCellProperties hcp = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                TableCellProperties hcp1 = new TableCellProperties(new TableCellWidth { Width = "2390", Type = TableWidthUnitValues.Auto }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                ParagraphProperties hpp = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });

                ParagraphProperties hpp1 = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });
                Justification centerJustify = new Justification() { Val = JustificationValues.Center };
                Justification rightJustify = new Justification() { Val = JustificationValues.Right };

                hpp1.AppendChild((Justification)centerJustify.CloneNode(true));

                TableCellBorders hcb = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 20, Color = "ED7D27" } };

                Shading hs1 = new Shading() { Color = "auto", Fill = "393939", Val = ShadingPatternValues.Clear };
                TableCellMargin hcm1 = new TableCellMargin() { RightMargin = new RightMargin() { Width = "50", Type = TableWidthUnitValues.Dxa }, LeftMargin = new LeftMargin() { Width = "50", Type = TableWidthUnitValues.Dxa } };

                hcp.Append(hcm1, hs1, hcb);
                hcp1.Append((TableCellMargin)hcm1.CloneNode(true), (Shading)hs1.CloneNode(true), (TableCellBorders)hcb.CloneNode(true));
                h1.Append(hcp);
                h1.AppendChild((ParagraphProperties)hpp.CloneNode(true));
                h1.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(name))));

                TableCell h2 = new TableCell();
                h2.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                h2.Append((TableCellProperties)hcp1.CloneNode(true));
                h2.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Life"))));

                TableCell h3 = new TableCell();
                h3.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                h3.Append((TableCellProperties)hcp1.CloneNode(true));
                h3.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("TPD"))));

                TableCell h4 = new TableCell();
                h4.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                h4.Append((TableCellProperties)hcp1.CloneNode(true));
                h4.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Trauma"))));

                TableCell h5 = new TableCell();
                h5.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                h5.Append((TableCellProperties)hcp1.CloneNode(true));
                h5.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Income Protection"))));
                header.Append(h1, h2, h3, h4, h5);

                table.Append(header);
                //Body

                TableCellProperties tcp1 = new TableCellProperties(new TableCellWidth { Width = "2390", Type = TableWidthUnitValues.Auto }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                TableCellProperties tcp = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                TableCellProperties tcpN = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });

                TableCellBorders tcbL = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, RightBorder = new RightBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };
                TableCellBorders tcbR = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, LeftBorder = new LeftBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };
                TableCellBorders tcbN = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };

                TableCellMargin tcm = new TableCellMargin() { RightMargin = new RightMargin() { Width = "50", Type = TableWidthUnitValues.Dxa }, LeftMargin = new LeftMargin() { Width = "50", Type = TableWidthUnitValues.Dxa } };

                ParagraphProperties pp = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "0" });
                ParagraphProperties pp1 = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "0" });

                pp.AppendChild((Justification)centerJustify.CloneNode(true));
                tcp.Append(tcbL, tcm);
                tcpN.Append((TableCellBorders)tcbN.CloneNode(true), (TableCellMargin)tcm.CloneNode(true));

                TableRow defAsset = new TableRow();
                TableCell dCell = new TableCell();
                dCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                dCell.Append((TableCellProperties)tcpN.CloneNode(true));
                dCell.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Capital Requirements"))));
                defAsset.Append(dCell);
                table.AppendChild(defAsset);


                TotalNeedsAnalysisData total = new TotalNeedsAnalysisData();
                foreach (NeedsAnalysisViewModel needsAnalysis in details)
                {
                    TableRow tr = new TableRow();
                    TableCell tc1 = new TableCell();

                    tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)rightJustify.CloneNode(true));
                    tc1.Append((TableCellProperties)tcp.CloneNode(true));

                    var desc = "";
                    if (needsAnalysis.Description == "Children/s education expenses")
                    {
                        desc = "Amount needed for children/s education expenses";
                    }
                    else if (needsAnalysis.Description == "For how long that income needs to be replaced")
                    {
                        desc = "For how long that income needs to be replaced";
                    }
                    else if(needsAnalysis.Description == "Funeral expenses")
                    {
                        desc = "Amount needed for funeral expenses";
                    }
                    else if(needsAnalysis.Description == "Medical expenses")
                    {
                        desc = "Amount needed for medical expenses";
                    }
                    else if (needsAnalysis.Description == "Waiting period")
                    {
                        desc = "Number of weeks you could sustain without income (waiting period)";
                    }
                    else if (needsAnalysis.Description == "Benefit period")
                    {
                        desc = "How long would you like the benefits to go for (benefit period)";
                    }
                    else
                    {
                        desc = needsAnalysis.Description;
                    }

                    tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                    if(needsAnalysis.Description == "Total Cover Recommended")
                    {
                        tc1.Append(new Paragraph(new Run(new RunProperties(new Bold()),new Text(desc))));
                    }
                    else
                    {
                        tc1.Append(new Paragraph(new Run(new Text(desc))));
                    }

                    TableCell tc2 = new TableCell();
                    tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                    tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                    if (needsAnalysis.Description == "Total Cover Recommended")
                    {
                        tc2.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(needsAnalysis.Life == "0" ? "-" : "$" + String.Format("{0:n0}", Convert.ToInt32(needsAnalysis.Life))))));
                    }
                    else
                    {
                        if (needsAnalysis.Description != "For how long that income needs to be replaced")
                        {
                            int number;
                            if (Int32.TryParse(needsAnalysis.Life, out number))
                            {
                                total.Life += Convert.ToInt32(needsAnalysis.Life);
                            }
                           
                        }
                        tc2.Append(new Paragraph(new Run(new Text(needsAnalysis.Life == "0" ? "-" : "$" + String.Format("{0:n0}", Convert.ToInt32(needsAnalysis.Life))))));
                    }

                    TableCell tc3 = new TableCell();
                    tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                    tc3.Append((TableCellProperties)tcp1.CloneNode(true));
                    if (needsAnalysis.Description == "Total Cover Recommended")
                    {
                        tc3.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(needsAnalysis.Tpd == "0" ? "-" : "$" + String.Format("{0:n0}", Convert.ToInt32(needsAnalysis.Tpd))))));
                    }
                    else
                    {
                        if (needsAnalysis.Description != "For how long that income needs to be replaced")
                        {
                            int number;
                            if (Int32.TryParse(needsAnalysis.Tpd, out number))
                            {
                                total.Tpd += Convert.ToInt32(needsAnalysis.Tpd);
                            }

                        }
                        tc3.Append(new Paragraph(new Run(new Text(needsAnalysis.Tpd == "0" ? "-" : "$" + String.Format("{0:n0}", Convert.ToInt32(needsAnalysis.Tpd))))));
                    }



                    TableCell tc4 = new TableCell();
                    tc4.AppendChild((ParagraphProperties)pp.CloneNode(true));
                    tc4.Append((TableCellProperties)tcp1.CloneNode(true));
                    if (needsAnalysis.Description == "Total Cover Recommended")
                    {
                        tc4.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(needsAnalysis.Trauma == "0" ? "-" : "$" + String.Format("{0:n0}", Convert.ToInt32(needsAnalysis.Trauma))))));
                    }
                    else
                    {
                        if (needsAnalysis.Description != "For how long that income needs to be replaced")
                        {
                            int number;
                            if (Int32.TryParse(needsAnalysis.Trauma, out number))
                            {
                                total.Trauma += Convert.ToInt32(needsAnalysis.Trauma);
                            }
                        }
                        tc4.Append(new Paragraph(new Run(new Text(needsAnalysis.Trauma == "0" ? "-" : "$" + String.Format("{0:n0}", Convert.ToInt32(needsAnalysis.Trauma))))));
                    }



                    TableCell tc5 = new TableCell();
                    TableCellProperties tcp2 = new TableCellProperties(new TableCellWidth { Width = "750", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                    tcp2.Append((TableCellBorders)tcbR.CloneNode(true), (Justification)centerJustify.CloneNode(true));
                    tc5.Append(tcp2);
                    tc5.AppendChild((ParagraphProperties)pp.CloneNode(true));
                    if (needsAnalysis.Description == "Total Cover Recommended")
                    {
                        int number;
                        if (Int32.TryParse(needsAnalysis.IncomeProtection, out number))
                        {
                            tc5.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(needsAnalysis.IncomeProtection == "0" ? "-" : "$" + String.Format("{0:n0}", Convert.ToInt32(needsAnalysis.IncomeProtection))))));
                        }
                        else
                        {
                            tc5.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(needsAnalysis.IncomeProtection == "0" ? "-" : "$" + String.Format("{0:n0}", needsAnalysis.IncomeProtection))))); 
                        }
                    }
                    else
                    { 
                        int number;
                        if (Int32.TryParse(needsAnalysis.IncomeProtection, out number))
                        {
                            if (needsAnalysis.Description != "For how long that income needs to be replaced")
                            {
                                total.IncomeProtection += Convert.ToInt32(needsAnalysis.IncomeProtection);
                            }
                                tc5.Append(new Paragraph(new Run(new Text(needsAnalysis.IncomeProtection == "0" ? "-" : "$" + String.Format("{0:n0}", Convert.ToInt32(needsAnalysis.IncomeProtection))))));
                        }
                        else
                        {
                            tc5.Append(new Paragraph(new Run(new Text(needsAnalysis.IncomeProtection == "0" ? "-" : "$" + String.Format("{0:n0}", needsAnalysis.IncomeProtection)))));
                        }
                    }
                    tr.Append(tc1, tc2, tc3, tc4, tc5);

                    table.AppendChild(tr);

                }

                //Total
                TableRow totalRow = new TableRow();
                TableCell total_tc1 = new TableCell();

                tcp1.Append((TableCellBorders)tcbL.CloneNode(true), (TableCellMargin)tcm.CloneNode(true), (Justification)rightJustify.CloneNode(true));
                total_tc1.Append((TableCellProperties)tcp.CloneNode(true));
                total_tc1.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                total_tc1.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Total Cover Required"))));

                TableCell total_tc2 = new TableCell();
                total_tc2.AppendChild((ParagraphProperties)pp.CloneNode(true));
                total_tc2.Append((TableCellProperties)tcp1.CloneNode(true));
                total_tc2.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(total.Life == 0 ? "-" : "$" + String.Format("{0:n0}", total.Life)))));

                TableCell total_tc3 = new TableCell();
                total_tc3.AppendChild((ParagraphProperties)pp.CloneNode(true));
                total_tc3.Append((TableCellProperties)tcp1.CloneNode(true));
                total_tc3.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(total.Tpd == 0 ? "-" : "$" + String.Format("{0:n0}", total.Tpd)))));

                TableCell total_tc4 = new TableCell();
                total_tc4.AppendChild((ParagraphProperties)pp.CloneNode(true));
                total_tc4.Append((TableCellProperties)tcp1.CloneNode(true));
                total_tc4.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(total.Trauma == 0 ? "-" : "$" + String.Format("{0:n0}", total.Trauma)))));

                TableCell total_tc5 = new TableCell();
                total_tc5.AppendChild((ParagraphProperties)pp.CloneNode(true));
                total_tc5.Append((TableCellProperties)tcp1.CloneNode(true));
                total_tc5.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(total.IncomeProtection == 0 ? "-" : "$" + String.Format("{0:n0}", total.IncomeProtection)))));

                totalRow.Append(total_tc1, total_tc2, total_tc3, total_tc4, total_tc5);
                table.AppendChild(totalRow);

                Run linebreak = new Run(new Break());
                Paragraph blank = new Paragraph();
                blank.Append(new OpenXmlElement[] { (Run)linebreak.CloneNode(true) });
                body.Append(blank);


            }


        }

        private void AddReplacementOfProduct(Body body, CurrentInsuranceViewModel[] currentInsurances, ProposedInsuranceViewModel[] proposedInsurances, BasicDetails clientDetails, Color orange)
        {

            var clientInsurances = proposedInsurances.Where(c => c.Owner == "Client").ToArray();
            var partnerInsurances = proposedInsurances.Where(c => c.Owner == "Partner").ToArray();
           

            if (clientInsurances.Length > 0 || partnerInsurances.Length > 0 )
            {
                Paragraph break4 = new Paragraph(new Run(new Break() { Type = BreakValues.Page }));
                body.Append(break4);
                Paragraph ProductReplacement = new Paragraph(new Run(new RunProperties(new FontSize { Val = "40" }, new Color() { Val = "ED7D27" }), new Text("Insurance replacement of product")));
                body.Append(ProductReplacement);

                Paragraph replacement = new Paragraph();
                replacement.Append(new OpenXmlElement[] { new Run(new RunProperties(), new Text("We have recommended replacing your existing insurance. The table below compares your existing insurance with our recommended cover based on our research. ")) });
                body.Append(replacement);
            }

            //Client
            if (clientInsurances != null && clientInsurances.Length > 0)
            {              
                var proposedProducts = new List<ProposedInsuranceViewModel>();

                foreach (ProposedInsuranceViewModel pI in clientInsurances)
                {
                  
                    if (pI.Replacement.Any())
                    {
                        proposedProducts.Add(pI);
                    }
                }
    
                var existingProducts = new List<CurrentInsuranceViewModel>();
             
                foreach (ProposedInsuranceViewModel pp in proposedProducts)
                {
                    foreach (InsuranceReplacementViewModel iR in pp.Replacement)
                    {
                        var current = currentInsurances.Where(a => a.RecId == iR.CurrentId).FirstOrDefault();
                        var existing = existingProducts.Where(a => a.RecId == current.RecId);
                        if(existing.Count() == 0)
                        {
                            existingProducts.Add(current);
                        }

                        
                    }
                }
           
                if (proposedProducts.Any() || existingProducts.Any())
                {

                    Paragraph s2 = body.AppendChild(new Paragraph(new Run(new RunProperties(new Bold(), new FontSize { Val = "24" }, new Color() { Val = "ED7D27" }), new Text("Comparison costs for " + clientDetails.ClientName))));

                    //New Table

                    Table table = body.AppendChild(new Table());
                    TableProperties tableProp = new TableProperties();
                    TableStyle tableStyle = new TableStyle() { Val = "TableGrid" };

                    TableWidth tableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };

                    tableProp.Append(tableStyle, tableWidth);
                    table.AppendChild(tableProp);


                    ////Header
                    TableRow header = new TableRow();
                    TableRow type = new TableRow();
                    TableCell h1 = new TableCell();

                    TableCellProperties hcp = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                    TableCellProperties hcp1 = new TableCellProperties(new TableCellWidth { Width = "2390", Type = TableWidthUnitValues.Auto }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                    TableCellProperties hcp2 = new TableCellProperties(new TableCellWidth { Width = "2390", Type = TableWidthUnitValues.Auto }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });

                    ParagraphProperties hpp = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });
                    ParagraphProperties hpp1 = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });

                    Justification centerJustify = new Justification() { Val = JustificationValues.Center };
                    hpp1.AppendChild((Justification)centerJustify.CloneNode(true));

                    TableCellBorders hcb = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 20, Color = "ED7D27" } };

                    Shading hs1 = new Shading() { Color = "auto", Fill = "393939", Val = ShadingPatternValues.Clear };
                    TableCellMargin hcm1 = new TableCellMargin() { RightMargin = new RightMargin() { Width = "50", Type = TableWidthUnitValues.Dxa }, LeftMargin = new LeftMargin() { Width = "50", Type = TableWidthUnitValues.Dxa } };

                    hcp.Append(hcm1, hs1, hcb);
                    hcp1.Append((TableCellMargin)hcm1.CloneNode(true), (Shading)hs1.CloneNode(true), (TableCellBorders)hcb.CloneNode(true));
                    hcp2.Append((TableCellMargin)hcm1.CloneNode(true), (Shading)hs1.CloneNode(true));

                    TableCell blank = new TableCell();
                    blank.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                    blank.Append((TableCellProperties)hcp2.CloneNode(true));
                    blank.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(""))));
                    type.Append(blank);

                    foreach (CurrentInsuranceViewModel existing in existingProducts)
                    {
                        TableCell h = new TableCell();
                        h.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                        h.Append((TableCellProperties)hcp2.CloneNode(true));
                        h.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Existing"))));
                        type.Append(h);
                    }
                    foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                    {
                        TableCell h = new TableCell();
                        h.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                        h.Append((TableCellProperties)hcp2.CloneNode(true));
                        h.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Proposed"))));
                        type.Append(h);
                    }
                 
                    table.Append(type);



                    h1.Append(hcp);

                    h1.AppendChild((ParagraphProperties)hpp.CloneNode(true));
                    h1.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(""))));
                    header.Append(h1);

                    foreach (CurrentInsuranceViewModel existing in existingProducts)
                    {
                        TableCell h = new TableCell();
                        h.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                        h.Append((TableCellProperties)hcp.CloneNode(true));
                        h.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(existing.Provider))));
                        header.Append(h);
                    }
                    foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                    {
                        TableCell h = new TableCell();
                        h.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                        h.Append((TableCellProperties)hcp.CloneNode(true));
                        h.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(proposed.Provider))));
                        header.Append(h);
                    }

                    table.Append(header);



                    //Body

                    TableCellProperties tcp1 = new TableCellProperties(new TableCellWidth { Width = "2390", Type = TableWidthUnitValues.Auto }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                    TableCellProperties tcp = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                    TableCellProperties tcpN = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });

                    TableCellBorders tcbL = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, RightBorder = new RightBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };
                    TableCellBorders tcbR = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, LeftBorder = new LeftBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };
                    TableCellBorders tcbN = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };

                    TableCellMargin tcm = new TableCellMargin() { RightMargin = new RightMargin() { Width = "50", Type = TableWidthUnitValues.Dxa }, LeftMargin = new LeftMargin() { Width = "50", Type = TableWidthUnitValues.Dxa } };

                    ParagraphProperties pp = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });
                    ParagraphProperties pp1 = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });

                    pp.AppendChild((Justification)centerJustify.CloneNode(true));
                    tcp.Append(tcbR, tcm);
                    tcpN.Append((TableCellBorders)tcbN.CloneNode(true), (TableCellMargin)tcm.CloneNode(true));

                    //Underwriter
                    TableRow Underwriter = new TableRow();
                    TableCell dCell = new TableCell();
                    dCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                    dCell.Append((TableCellProperties)tcpN.CloneNode(true));
                    dCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Underwriter"))));
                    Underwriter.Append(dCell);

                    foreach (CurrentInsuranceViewModel existing in existingProducts)
                    {
                        TableCell h = new TableCell();
                        h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        h.Append((TableCellProperties)tcp.CloneNode(true));
                        h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.Provider))));
                        Underwriter.Append(h);
                    }
                    foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                    {
                        TableCell h = new TableCell();
                        h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        h.Append((TableCellProperties)tcp.CloneNode(true));
                        h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.Provider))));
                        Underwriter.Append(h);
                    }
                    table.Append(Underwriter);

                    //Life Insured
                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Life Insured"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.Owner))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.Owner))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }
                    var rowLength = existingProducts.Count() + proposedProducts.Count() + 1;

                    //Policy Owner
                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Policy Owner"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text("Super Fund/Client"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text("Super Fund/Client"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    //Benefit Amount
                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {                         
                        TableRow BenefitAmount = new TableRow();
                        TableCell bACell = new TableCell();
                        TableCellProperties tableCellProperties9 = new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center } , new GridSpan() { Val = rowLength });
                        bACell.Append(tableCellProperties9);
                        bACell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        bACell.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Benefit Amounts (lump sum)"))));
                        BenefitAmount.Append(bACell);
                        table.Append(BenefitAmount);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Life Insurance"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.LifeCover.Any() ? "$ " + String.Format("{0:n0}", existing.LifeCover[0].BenefitAmount) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.LifeCover.Any() ? "$ " + String.Format("{0:n0}", proposed.LifeCover[0].BenefitAmount) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("TPD Insurance"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TpdCover.Any() ? "$ " + String.Format("{0:n0}", existing.TpdCover[0].BenefitAmount) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TpdCover.Any() ? "$ " + String.Format("{0:n0}", proposed.TpdCover[0].BenefitAmount) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Trauma Insurance"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TraumaCover.Any() ? "$ " + String.Format("{0:n0}", existing.TraumaCover[0].BenefitAmount) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TraumaCover.Any() ? "$ " + String.Format("{0:n0}", proposed.TraumaCover[0].BenefitAmount) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Income Protection"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.IncomeCover.Any() ? "$ " + String.Format("{0:n0}", existing.IncomeCover[0].MonthlyBenefitAmount) + " per month" : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.IncomeCover.Any() ? "$ " + String.Format("{0:n0}", proposed.IncomeCover[0].MonthlyBenefitAmount) + " per month" : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    //Premium Costs
                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {                       
                        TableRow PremiumCosts = new TableRow();
                        TableCell bACell = new TableCell();
                        TableCellProperties tableCellProperties9 = new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }, new GridSpan() { Val = rowLength });
                        bACell.Append(tableCellProperties9);
                        bACell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        bACell.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Premium Costs (estimated first year)"))));
                        PremiumCosts.Append(bACell);
                        table.Append(PremiumCosts);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Premium Amount"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.FeeDetails.Where( item => item.FeeType == "premium").Any() ? "$ " + String.Format("{0:n0}", existing.FeeDetails.Where(item => item.FeeType == "premium").Sum(item => item.Amount)) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.FeeDetails.Where(item => item.FeeType == "premium").Any() ? "$ " + String.Format("{0:n0}", proposed.FeeDetails.Where(item => item.FeeType == "premium").Sum(item => item.Amount)) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Policy Fee"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.FeeDetails.Where(item => item.FeeType == "policyFee").Any() ? "$ " + String.Format("{0:n0}", existing.FeeDetails.Where(item => item.FeeType == "policyFee").Sum(item => item.Amount)) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.FeeDetails.Where(item => item.FeeType == "policyFee").Any() ? "$ " + String.Format("{0:n0}", proposed.FeeDetails.Where(item => item.FeeType == "policyFee").Sum(item => item.Amount)) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("QLD Stamp Duty"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.FeeDetails.Where(item => item.FeeType == "stampDuty").Any() ? "$ " + String.Format("{0:n0}", existing.FeeDetails.Where(item => item.FeeType == "stampDuty").Sum(item => item.Amount)) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.FeeDetails.Where(item => item.FeeType == "stampDuty").Any() ? "$ " + String.Format("{0:n0}", proposed.FeeDetails.Where(item => item.FeeType == "stampDuty").Sum(item => item.Amount)) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Other"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.FeeDetails.Where(item => item.FeeType == "other").Any() ? "$ " + String.Format("{0:n0}", existing.FeeDetails.Where(item => item.FeeType == "other").Sum(item => item.Amount)) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.FeeDetails.Where(item => item.FeeType == "other").Any() ? "$ " + String.Format("{0:n0}", proposed.FeeDetails.Where(item => item.FeeType == "other").Sum(item => item.Amount)) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    //foreach (RopcurrentProducts cp in list)
                    //    //            {
                    //    //                var current = currentAssests.Where(a => a.id == cp.RecId).FirstOrDefault();
                    //    //                sum += current.feeDisplay.Sum(item => item.val);
                    //    //            }

                    //    //            var display = "";
                    //    //            var value = Math.Round(proposed.feeDisplay.Sum(item => item.val)) - Math.Round(sum);
                    //    //            if (value > 0)
                    //    //            {
                    //    //                display = " Additional $" + String.Format("{0:n0}", Math.Abs(value));
                    //    //            }


                    //Life Benefits
                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow LifeBenefits = new TableRow();
                        TableCell bACell = new TableCell();
                        TableCellProperties tableCellProperties9 = new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }, new GridSpan() { Val = rowLength });
                        bACell.Append(tableCellProperties9);
                        bACell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        bACell.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Life Benefits"))));
                        LifeBenefits.Append(bACell);
                        table.Append(LifeBenefits);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Future Insurability"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.LifeCover.Any() ? (existing.LifeCover[0].FutureInsurability == 1 ? "Yes" : "No" ) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.LifeCover.Any() ? (proposed.LifeCover[0].FutureInsurability == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Terminal Illness"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.LifeCover.Any() ? (existing.LifeCover[0].TerminalIllness == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.LifeCover.Any() ? (proposed.LifeCover[0].TerminalIllness == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    //TPD Benefits
                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow TPDBenefits = new TableRow();
                        TableCell bACell = new TableCell();
                        TableCellProperties tableCellProperties9 = new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }, new GridSpan() { Val = rowLength });
                        bACell.Append(tableCellProperties9);
                        bACell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        bACell.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("TPD Benefits"))));
                        TPDBenefits.Append(bACell);
                        table.Append(TPDBenefits);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Definition"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TpdCover.Any() ? (existing.TpdCover[0].Definition == "" ? "-" : existing.TpdCover[0].Definition) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TpdCover.Any() ? (proposed.TpdCover[0].Definition == "" ? "-" : proposed.TpdCover[0].Definition) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Disability Term"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TpdCover.Any() ? (existing.TpdCover[0].DisabilityTerm == "" ? "-" : existing.TpdCover[0].DisabilityTerm) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TpdCover.Any() ? (proposed.TpdCover[0].DisabilityTerm == "" ? "-" : proposed.TpdCover[0].DisabilityTerm) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Double TPD"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TpdCover.Any() ? (existing.TpdCover[0].DoubleTpd == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TpdCover.Any() ? (proposed.TpdCover[0].DoubleTpd == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Waiver of Premium"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TpdCover.Any() ? (existing.TpdCover[0].WaiverOfPremium == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TpdCover.Any() ? (proposed.TpdCover[0].WaiverOfPremium == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Future Insurability"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TpdCover.Any() ? (existing.TpdCover[0].FutureInsurability == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TpdCover.Any() ? (proposed.TpdCover[0].FutureInsurability == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    //Trauma Benefits
                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow TraumaBenefits = new TableRow();
                        TableCell bACell = new TableCell();
                        TableCellProperties tableCellProperties9 = new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }, new GridSpan() { Val = rowLength });
                        bACell.Append(tableCellProperties9);
                        bACell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        bACell.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Trauma Benefits"))));
                        TraumaBenefits.Append(bACell);
                        table.Append(TraumaBenefits);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Reinstatement"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TraumaCover.Any() ? (existing.TraumaCover[0].Reinstatement == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TraumaCover.Any() ? (proposed.TraumaCover[0].Reinstatement == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Double Trauma"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TraumaCover.Any() ? (existing.TraumaCover[0].DoubleTrauma == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TraumaCover.Any() ? (proposed.TraumaCover[0].DoubleTrauma == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Child Trauma"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TraumaCover.Any() ? (existing.TraumaCover[0].ChildTrauma == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TraumaCover.Any() ? (proposed.TraumaCover[0].ChildTrauma == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }

                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Waiver of Premium"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TraumaCover.Any() ? (existing.TraumaCover[0].WaiverOfPremium == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TraumaCover.Any() ? (proposed.TraumaCover[0].WaiverOfPremium == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }

                        table.Append(tbl);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Reinstatement"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TraumaCover.Any() ? (existing.TraumaCover[0].Reinstatement == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TraumaCover.Any() ? (proposed.TraumaCover[0].Reinstatement == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }

                        table.Append(tbl);
                    }

                    //Income Benefits
                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow IncomeBenefits = new TableRow();
                        TableCell bACell = new TableCell();
                        TableCellProperties tableCellProperties9 = new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }, new GridSpan() { Val = rowLength });
                        bACell.Append(tableCellProperties9);
                        bACell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        bACell.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Income Protection Benefits"))));
                        IncomeBenefits.Append(bACell);
                        table.Append(IncomeBenefits);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Definition"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.IncomeCover.Any() ? (existing.IncomeCover[0].Definition == "" ? "-" : existing.IncomeCover[0].Definition) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.IncomeCover.Any() ? (proposed.IncomeCover[0].Definition == "" ? "-" : proposed.IncomeCover[0].Definition) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Waiting Period"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.IncomeCover.Any() ? (existing.IncomeCover[0].WaitingPeriod == "" ? "-" : existing.IncomeCover[0].WaitingPeriod) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.IncomeCover.Any() ? (proposed.IncomeCover[0].WaitingPeriod == "" ? "-" : proposed.IncomeCover[0].WaitingPeriod) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Benefit Period"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.IncomeCover.Any() ? (existing.IncomeCover[0].BenefitPeriod == "" ? "-" : existing.IncomeCover[0].BenefitPeriod) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.IncomeCover.Any() ? (proposed.IncomeCover[0].BenefitPeriod == "" ? "-" : proposed.IncomeCover[0].BenefitPeriod) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Claims Indexation"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.IncomeCover.Any() ? (existing.IncomeCover[0].ClaimsIndexation == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.IncomeCover.Any() ? (proposed.IncomeCover[0].ClaimsIndexation == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Critical Conditions Cover"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.IncomeCover.Any() ? (existing.IncomeCover[0].CriticalConditionsCover == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.IncomeCover.Any() ? (proposed.IncomeCover[0].CriticalConditionsCover == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                       if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Accident Benefit"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.IncomeCover.Any() ? (existing.IncomeCover[0].AccidentBenefit == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.IncomeCover.Any() ? (proposed.IncomeCover[0].AccidentBenefit == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }
                }
           
            }

            //Partner
            if (partnerInsurances != null && partnerInsurances.Length > 0)
            {
                var proposedProducts = new List<ProposedInsuranceViewModel>();

                foreach (ProposedInsuranceViewModel pI in partnerInsurances)
                {

                    if (pI.Replacement.Any())
                    {
                        proposedProducts.Add(pI);
                    }
                }

                var existingProducts = new List<CurrentInsuranceViewModel>();

                foreach (ProposedInsuranceViewModel pp in proposedProducts)
                {
                    foreach (InsuranceReplacementViewModel iR in pp.Replacement)
                    {
                        var current = currentInsurances.Where(a => a.RecId == iR.CurrentId).FirstOrDefault();
                        var existing = existingProducts.Where(a => a.RecId == current.RecId);
                        if (existing.Count() == 0)
                        {
                            existingProducts.Add(current);
                        }


                    }
                }

                if (proposedProducts.Any() || existingProducts.Any())
                {

                    Paragraph s2 = body.AppendChild(new Paragraph(new Run(new RunProperties(new Bold(), new FontSize { Val = "24" }, new Color() { Val = "ED7D27" }), new Text("Comparison costs for " + clientDetails.ClientName))));

                    //New Table

                    Table table = body.AppendChild(new Table());
                    TableProperties tableProp = new TableProperties();
                    TableStyle tableStyle = new TableStyle() { Val = "TableGrid" };

                    TableWidth tableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };

                    tableProp.Append(tableStyle, tableWidth);
                    table.AppendChild(tableProp);


                    ////Header
                    TableRow header = new TableRow();
                    TableRow type = new TableRow();
                    TableCell h1 = new TableCell();

                    TableCellProperties hcp = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                    TableCellProperties hcp1 = new TableCellProperties(new TableCellWidth { Width = "2390", Type = TableWidthUnitValues.Auto }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                    TableCellProperties hcp2 = new TableCellProperties(new TableCellWidth { Width = "2390", Type = TableWidthUnitValues.Auto }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });

                    ParagraphProperties hpp = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });
                    ParagraphProperties hpp1 = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });

                    Justification centerJustify = new Justification() { Val = JustificationValues.Center };
                    hpp1.AppendChild((Justification)centerJustify.CloneNode(true));

                    TableCellBorders hcb = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 20, Color = "ED7D27" } };

                    Shading hs1 = new Shading() { Color = "auto", Fill = "393939", Val = ShadingPatternValues.Clear };
                    TableCellMargin hcm1 = new TableCellMargin() { RightMargin = new RightMargin() { Width = "50", Type = TableWidthUnitValues.Dxa }, LeftMargin = new LeftMargin() { Width = "50", Type = TableWidthUnitValues.Dxa } };

                    hcp.Append(hcm1, hs1, hcb);
                    hcp1.Append((TableCellMargin)hcm1.CloneNode(true), (Shading)hs1.CloneNode(true), (TableCellBorders)hcb.CloneNode(true));
                    hcp2.Append((TableCellMargin)hcm1.CloneNode(true), (Shading)hs1.CloneNode(true));

                    TableCell blank = new TableCell();
                    blank.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                    blank.Append((TableCellProperties)hcp2.CloneNode(true));
                    blank.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(""))));
                    type.Append(blank);

                    foreach (CurrentInsuranceViewModel existing in existingProducts)
                    {
                        TableCell h = new TableCell();
                        h.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                        h.Append((TableCellProperties)hcp2.CloneNode(true));
                        h.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Existing"))));
                        type.Append(h);
                    }
                    foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                    {
                        TableCell h = new TableCell();
                        h.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                        h.Append((TableCellProperties)hcp2.CloneNode(true));
                        h.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Proposed"))));
                        type.Append(h);
                    }

                    table.Append(type);



                    h1.Append(hcp);

                    h1.AppendChild((ParagraphProperties)hpp.CloneNode(true));
                    h1.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(""))));
                    header.Append(h1);

                    foreach (CurrentInsuranceViewModel existing in existingProducts)
                    {
                        TableCell h = new TableCell();
                        h.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                        h.Append((TableCellProperties)hcp.CloneNode(true));
                        h.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(existing.Provider))));
                        header.Append(h);
                    }
                    foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                    {
                        TableCell h = new TableCell();
                        h.AppendChild((ParagraphProperties)hpp1.CloneNode(true));
                        h.Append((TableCellProperties)hcp.CloneNode(true));
                        h.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text(proposed.Provider))));
                        header.Append(h);
                    }

                    table.Append(header);



                    //Body

                    TableCellProperties tcp1 = new TableCellProperties(new TableCellWidth { Width = "2390", Type = TableWidthUnitValues.Auto }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                    TableCellProperties tcp = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
                    TableCellProperties tcpN = new TableCellProperties(new TableCellWidth { Width = "2000", Type = TableWidthUnitValues.Pct }, new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });

                    TableCellBorders tcbL = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, RightBorder = new RightBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };
                    TableCellBorders tcbR = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, LeftBorder = new LeftBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };
                    TableCellBorders tcbN = new TableCellBorders() { BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" }, TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 1, Color = "D3D3D3" } };

                    TableCellMargin tcm = new TableCellMargin() { RightMargin = new RightMargin() { Width = "50", Type = TableWidthUnitValues.Dxa }, LeftMargin = new LeftMargin() { Width = "50", Type = TableWidthUnitValues.Dxa } };

                    ParagraphProperties pp = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });
                    ParagraphProperties pp1 = new ParagraphProperties(new ParagraphStyleId() { Val = "No Spacing" }, new SpacingBetweenLines() { After = "5" });

                    pp.AppendChild((Justification)centerJustify.CloneNode(true));
                    tcp.Append(tcbR, tcm);
                    tcpN.Append((TableCellBorders)tcbN.CloneNode(true), (TableCellMargin)tcm.CloneNode(true));

                    //Underwriter
                    TableRow Underwriter = new TableRow();
                    TableCell dCell = new TableCell();
                    dCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                    dCell.Append((TableCellProperties)tcpN.CloneNode(true));
                    dCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Underwriter"))));
                    Underwriter.Append(dCell);

                    foreach (CurrentInsuranceViewModel existing in existingProducts)
                    {
                        TableCell h = new TableCell();
                        h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        h.Append((TableCellProperties)tcp.CloneNode(true));
                        h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.Provider))));
                        Underwriter.Append(h);
                    }
                    foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                    {
                        TableCell h = new TableCell();
                        h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                        h.Append((TableCellProperties)tcp.CloneNode(true));
                        h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.Provider))));
                        Underwriter.Append(h);
                    }
                    table.Append(Underwriter);

                    //Life Insured
                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Life Insured"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.Owner))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.Owner))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }
                    var rowLength = existingProducts.Count() + proposedProducts.Count() + 1;

                    //Policy Owner
                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Policy Owner"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text("Super Fund/Client"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text("Super Fund/Client"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    //Benefit Amount
                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow BenefitAmount = new TableRow();
                        TableCell bACell = new TableCell();
                        TableCellProperties tableCellProperties9 = new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }, new GridSpan() { Val = rowLength });
                        bACell.Append(tableCellProperties9);
                        bACell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        bACell.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Benefit Amounts (lump sum)"))));
                        BenefitAmount.Append(bACell);
                        table.Append(BenefitAmount);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Life Insurance"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.LifeCover.Any() ? "$ " + String.Format("{0:n0}", existing.LifeCover[0].BenefitAmount) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.LifeCover.Any() ? "$ " + String.Format("{0:n0}", proposed.LifeCover[0].BenefitAmount) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("TPD Insurance"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TpdCover.Any() ? "$ " + String.Format("{0:n0}", existing.TpdCover[0].BenefitAmount) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TpdCover.Any() ? "$ " + String.Format("{0:n0}", proposed.TpdCover[0].BenefitAmount) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Trauma Insurance"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TraumaCover.Any() ? "$ " + String.Format("{0:n0}", existing.TraumaCover[0].BenefitAmount) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TraumaCover.Any() ? "$ " + String.Format("{0:n0}", proposed.TraumaCover[0].BenefitAmount) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Income Protection"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.IncomeCover.Any() ? "$ " + String.Format("{0:n0}", existing.IncomeCover[0].MonthlyBenefitAmount) + " per month" : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.IncomeCover.Any() ? "$ " + String.Format("{0:n0}", proposed.IncomeCover[0].MonthlyBenefitAmount) + " per month" : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    //Premium Costs
                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow PremiumCosts = new TableRow();
                        TableCell bACell = new TableCell();
                        TableCellProperties tableCellProperties9 = new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }, new GridSpan() { Val = rowLength });
                        bACell.Append(tableCellProperties9);
                        bACell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        bACell.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Premium Costs (estimated first year)"))));
                        PremiumCosts.Append(bACell);
                        table.Append(PremiumCosts);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Premium Amount"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.FeeDetails.Where(item => item.FeeType == "premium").Any() ? "$ " + String.Format("{0:n0}", existing.FeeDetails.Where(item => item.FeeType == "premium").Sum(item => item.Amount)) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.FeeDetails.Where(item => item.FeeType == "premium").Any() ? "$ " + String.Format("{0:n0}", proposed.FeeDetails.Where(item => item.FeeType == "premium").Sum(item => item.Amount)) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Policy Fee"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.FeeDetails.Where(item => item.FeeType == "policyFee").Any() ? "$ " + String.Format("{0:n0}", existing.FeeDetails.Where(item => item.FeeType == "policyFee").Sum(item => item.Amount)) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.FeeDetails.Where(item => item.FeeType == "policyFee").Any() ? "$ " + String.Format("{0:n0}", proposed.FeeDetails.Where(item => item.FeeType == "policyFee").Sum(item => item.Amount)) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("QLD Stamp Duty"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.FeeDetails.Where(item => item.FeeType == "stampDuty").Any() ? "$ " + String.Format("{0:n0}", existing.FeeDetails.Where(item => item.FeeType == "stampDuty").Sum(item => item.Amount)) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.FeeDetails.Where(item => item.FeeType == "stampDuty").Any() ? "$ " + String.Format("{0:n0}", proposed.FeeDetails.Where(item => item.FeeType == "stampDuty").Sum(item => item.Amount)) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Other"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.FeeDetails.Where(item => item.FeeType == "other").Any() ? "$ " + String.Format("{0:n0}", existing.FeeDetails.Where(item => item.FeeType == "other").Sum(item => item.Amount)) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.FeeDetails.Where(item => item.FeeType == "other").Any() ? "$ " + String.Format("{0:n0}", proposed.FeeDetails.Where(item => item.FeeType == "other").Sum(item => item.Amount)) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    //foreach (RopcurrentProducts cp in list)
                    //    //            {
                    //    //                var current = currentAssests.Where(a => a.id == cp.RecId).FirstOrDefault();
                    //    //                sum += current.feeDisplay.Sum(item => item.val);
                    //    //            }

                    //    //            var display = "";
                    //    //            var value = Math.Round(proposed.feeDisplay.Sum(item => item.val)) - Math.Round(sum);
                    //    //            if (value > 0)
                    //    //            {
                    //    //                display = " Additional $" + String.Format("{0:n0}", Math.Abs(value));
                    //    //            }


                    //Life Benefits
                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow LifeBenefits = new TableRow();
                        TableCell bACell = new TableCell();
                        TableCellProperties tableCellProperties9 = new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }, new GridSpan() { Val = rowLength });
                        bACell.Append(tableCellProperties9);
                        bACell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        bACell.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Life Benefits"))));
                        LifeBenefits.Append(bACell);
                        table.Append(LifeBenefits);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Future Insurability"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.LifeCover.Any() ? (existing.LifeCover[0].FutureInsurability == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.LifeCover.Any() ? (proposed.LifeCover[0].FutureInsurability == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Terminal Illness"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.LifeCover.Any() ? (existing.LifeCover[0].TerminalIllness == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.LifeCover.Any() ? (proposed.LifeCover[0].TerminalIllness == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    //TPD Benefits
                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow TPDBenefits = new TableRow();
                        TableCell bACell = new TableCell();
                        TableCellProperties tableCellProperties9 = new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }, new GridSpan() { Val = rowLength });
                        bACell.Append(tableCellProperties9);
                        bACell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        bACell.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("TPD Benefits"))));
                        TPDBenefits.Append(bACell);
                        table.Append(TPDBenefits);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Definition"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TpdCover.Any() ? (existing.TpdCover[0].Definition == "" ? "-" : existing.TpdCover[0].Definition) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TpdCover.Any() ? (proposed.TpdCover[0].Definition == "" ? "-" : proposed.TpdCover[0].Definition) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Disability Term"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TpdCover.Any() ? (existing.TpdCover[0].DisabilityTerm == "" ? "-" : existing.TpdCover[0].DisabilityTerm) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TpdCover.Any() ? (proposed.TpdCover[0].DisabilityTerm == "" ? "-" : proposed.TpdCover[0].DisabilityTerm) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Double TPD"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TpdCover.Any() ? (existing.TpdCover[0].DoubleTpd == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TpdCover.Any() ? (proposed.TpdCover[0].DoubleTpd == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Waiver of Premium"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TpdCover.Any() ? (existing.TpdCover[0].WaiverOfPremium == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TpdCover.Any() ? (proposed.TpdCover[0].WaiverOfPremium == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Future Insurability"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TpdCover.Any() ? (existing.TpdCover[0].FutureInsurability == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TpdCover.Any() ? (proposed.TpdCover[0].FutureInsurability == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    //Trauma Benefits
                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow TraumaBenefits = new TableRow();
                        TableCell bACell = new TableCell();
                        TableCellProperties tableCellProperties9 = new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }, new GridSpan() { Val = rowLength });
                        bACell.Append(tableCellProperties9);
                        bACell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        bACell.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Trauma Benefits"))));
                        TraumaBenefits.Append(bACell);
                        table.Append(TraumaBenefits);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Reinstatement"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TraumaCover.Any() ? (existing.TraumaCover[0].Reinstatement == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TraumaCover.Any() ? (proposed.TraumaCover[0].Reinstatement == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Double Trauma"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TraumaCover.Any() ? (existing.TraumaCover[0].DoubleTrauma == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TraumaCover.Any() ? (proposed.TraumaCover[0].DoubleTrauma == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Child Trauma"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TraumaCover.Any() ? (existing.TraumaCover[0].ChildTrauma == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TraumaCover.Any() ? (proposed.TraumaCover[0].ChildTrauma == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }

                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Waiver of Premium"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TraumaCover.Any() ? (existing.TraumaCover[0].WaiverOfPremium == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TraumaCover.Any() ? (proposed.TraumaCover[0].WaiverOfPremium == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }

                        table.Append(tbl);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Reinstatement"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.TraumaCover.Any() ? (existing.TraumaCover[0].Reinstatement == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.TraumaCover.Any() ? (proposed.TraumaCover[0].Reinstatement == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }

                        table.Append(tbl);
                    }

                    //Income Benefits
                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow IncomeBenefits = new TableRow();
                        TableCell bACell = new TableCell();
                        TableCellProperties tableCellProperties9 = new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }, new GridSpan() { Val = rowLength });
                        bACell.Append(tableCellProperties9);
                        bACell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        bACell.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Income Protection Benefits"))));
                        IncomeBenefits.Append(bACell);
                        table.Append(IncomeBenefits);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Definition"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.IncomeCover.Any() ? (existing.IncomeCover[0].Definition == "" ? "-" : existing.IncomeCover[0].Definition) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.IncomeCover.Any() ? (proposed.IncomeCover[0].Definition == "" ? "-" : proposed.IncomeCover[0].Definition) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Waiting Period"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.IncomeCover.Any() ? (existing.IncomeCover[0].WaitingPeriod == "" ? "-" : existing.IncomeCover[0].WaitingPeriod) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.IncomeCover.Any() ? (proposed.IncomeCover[0].WaitingPeriod == "" ? "-" : proposed.IncomeCover[0].WaitingPeriod) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Benefit Period"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.IncomeCover.Any() ? (existing.IncomeCover[0].BenefitPeriod == "" ? "-" : existing.IncomeCover[0].BenefitPeriod) : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.IncomeCover.Any() ? (proposed.IncomeCover[0].BenefitPeriod == "" ? "-" : proposed.IncomeCover[0].BenefitPeriod) : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Claims Indexation"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.IncomeCover.Any() ? (existing.IncomeCover[0].ClaimsIndexation == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.IncomeCover.Any() ? (proposed.IncomeCover[0].ClaimsIndexation == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Critical Conditions Cover"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.IncomeCover.Any() ? (existing.IncomeCover[0].CriticalConditionsCover == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.IncomeCover.Any() ? (proposed.IncomeCover[0].CriticalConditionsCover == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }

                    if (existingProducts.Count() > 0 || proposedProducts.Count() > 0)
                    {
                        TableRow tbl = new TableRow();
                        TableCell tableCell = new TableCell();
                        tableCell.AppendChild((ParagraphProperties)pp1.CloneNode(true));
                        tableCell.Append((TableCellProperties)tcpN.CloneNode(true));
                        tableCell.Append(new Paragraph(new Run(new RunProperties(), new Text("Accident Benefit"))));
                        tbl.Append(tableCell);

                        foreach (CurrentInsuranceViewModel existing in existingProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(existing.IncomeCover.Any() ? (existing.IncomeCover[0].AccidentBenefit == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        foreach (ProposedInsuranceViewModel proposed in proposedProducts)
                        {
                            TableCell h = new TableCell();
                            h.AppendChild((ParagraphProperties)pp.CloneNode(true));
                            h.Append((TableCellProperties)tcp.CloneNode(true));
                            h.Append(new Paragraph(new Run(new RunProperties(), new Text(proposed.IncomeCover.Any() ? (proposed.IncomeCover[0].AccidentBenefit == 1 ? "Yes" : "No") : "-"))));
                            tbl.Append(h);
                        }
                        table.Append(tbl);
                    }
                }

            }

            if (clientInsurances.Length > 0 || partnerInsurances.Length > 0)
            {
                AddStandardTextROP(body, orange);
            }
        }

   

    }
}
