using System;
using System.IO;
using System.Net.Http.Headers;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Data;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using CugemderApp.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CugemderApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IHostingEnvironment _environment;
        private readonly CugemderMobileAppDbContext _context;

        public FileUploadController(IHostingEnvironment environment, CugemderMobileAppDbContext context)
        { 
            _environment = environment;
            _context = context;
        }



        [HttpPost]
        public  IActionResult Post()
        {

            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("StaticFiles", "Contents");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(dbPath);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        [HttpPost]
        [Route("Photo")]
        public IActionResult PostPhoto()
        {

            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("StaticFiles", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(dbPath);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete]
        [Route("Photo/{filename}")]
        public IActionResult DeletePhoto(string filename)
        {

            try
            {
                var file = Path.Combine(_environment.ContentRootPath, "Staticfiles", "Images", filename);
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete]
        [Route("Document/{filename}")]
        public IActionResult DeleteDocument(string filename)
        {

            try
            {
                var file = Path.Combine(_environment.ContentRootPath, "Staticfiles", "Contents", filename);
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet]
        [Route("Document/Excel")]
        public async Task<IActionResult> Test()
        {
            ExcelGenerator excel = new ExcelGenerator();

            var meetings = await _context.MeetingPoints.ToListAsync();
            var users = await _context.AspNetUsers
                .Where(c => c.Points != null)
                .Where(c => c.Group != null)
                .ToListAsync();
            excel.WriteExcelFile(users, meetings);


            return Ok();
        }

    }




    public class ExcelGenerator
    {

        public void WriteExcelFile(List<AspNetUsers> users, List<MeetingPoints> meetings)
        {

            List<UserNamesWithID> userNames = new List<UserNamesWithID>();
            List<string> criterias = new List<string>
            {
                "Birebir Puan",
                "Toplantıya Katılım",
                "Toplantı Saatine Uyum",
                "Dress Code Uyumu",
                "Mikrofon / Kamera Kalitesi / Yönetimi",
                "Söz Kesme / Kesmeme",
                "Süreye Uyum",
                "Firmasını ve Yaptığı İşi Güzel Tanıtma",
                "Kendisini Güzel İfade Etme",
                "Sonuç Odaklılık",
                "Kişisel İzlenim"
            };

            foreach (var item in users)
            {
                userNames.Add(new UserNamesWithID { Id = item.Id, Name = item.FirstName + " " + item.LastName});
            }
            
            // Lets converts our object data to Datatable for a simplified logic.
            // Datatable is most easy way to deal with complex datatypes for easy reading and formatting. 
            System.Data.DataTable table = (System.Data.DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(users), (typeof(System.Data.DataTable)));

            using (SpreadsheetDocument document = SpreadsheetDocument.Create("C:\\Users\\Kaan\\Desktop\\TestNewData.xlsx", SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };

                sheets.Append(sheet);


                // ADD HEADER
                Row header = new Row();
                Cell headerCell = new Cell();
                headerCell.DataType = CellValues.String;
                headerCell.CellValue = new CellValue("BİREBİR GÖRÜŞME DEĞERLENDİRME FORMU");
                header.AppendChild(headerCell);
                sheetData.AppendChild(header);


                // FIXED SUBHEADERS
                Row subHeader = new Row();
                Cell usernameCell = new Cell();
                Cell criteria = new Cell();
                Cell average = new Cell();

                usernameCell.DataType = CellValues.String; usernameCell.CellValue = new CellValue("Kişi Adı"); subHeader.AppendChild(usernameCell);
                criteria.DataType = CellValues.String; criteria.CellValue = new CellValue("Kriter Adı"); subHeader.AppendChild(criteria);
                average.DataType = CellValues.String; average.CellValue = new CellValue("Genel Ortalama"); subHeader.AppendChild(average);



                // LIST OF USERS AS SUBHEADER

                foreach (var item in userNames)
                {
                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(item.Name);
                    subHeader.AppendChild(cell);
                }


                sheetData.AppendChild(subHeader);
                // END OF SUBHEADER


                foreach (var userNamesWithID in userNames)
                {

                    var meetingPointAverageList = meetings.Where(c => c.ReceiverUserId == userNamesWithID.Id);


                    for (int i = 0; i < criterias.Count(); i++)
                    {
                        Row row = new Row();
                        if (i == 0)
                        {
                            var meetingPointAverage = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.TotalPoints) : 0;

                            Cell name = new Cell(); name.DataType = CellValues.String; name.CellValue = new CellValue(userNamesWithID.Name); row.AppendChild(name);
                            Cell criteriaName = new Cell(); criteriaName.DataType = CellValues.String; criteriaName.CellValue = new CellValue(criterias[i]); row.AppendChild(criteriaName);
                            Cell averagePoint = new Cell(); averagePoint.DataType = CellValues.String; averagePoint.CellValue = new CellValue(string.Format("{0:p2}", meetingPointAverage));

                            foreach (var UsersInRow in userNames)
                            {
                                if(userNamesWithID.Id == UsersInRow.Id)
                                {
                                    Cell cell = new Cell(); cell.DataType = CellValues.String; cell.CellValue = new CellValue(string.Empty);
                                    row.AppendChild(cell);
                                }
                                else
                                {
                                    var averageofUserList = meetingPointAverageList.Where(c => c.SenderUserId == UsersInRow.Id);
                                    var averageofUser = averageofUserList.Any() ? averageofUserList.Average(c => c.TotalPoints) : 0;

                                    Cell cell = new Cell(); cell.DataType = CellValues.String; cell.CellValue = new CellValue(string.Format("{0:p2}", averageofUser));
                                    row.AppendChild(cell);
                                }
                            }

                            sheet.AppendChild(row);
                        }
                        else
                        {


                            Cell name = new Cell(); name.DataType = CellValues.String; name.CellValue = new CellValue(string.Empty); row.AppendChild(name);
                            Cell criteriaName = new Cell(); criteriaName.DataType = CellValues.String; criteriaName.CellValue = new CellValue(criterias[i]); row.AppendChild(criteriaName);
                            switch (i)
                            {
                                case 1:
                                    var point1 = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.point1) : 0;
                                    Cell averagePoint1 = new Cell(); averagePoint1.DataType = CellValues.String; averagePoint1.CellValue = new CellValue(string.Format("{0} / 10", point1));
                                    break;
                                case 2:
                                    var point2 = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.point2) : 0;
                                    Cell averagePoint2 = new Cell(); averagePoint2.DataType = CellValues.String; averagePoint2.CellValue = new CellValue(string.Format("{0} / 10", point2));
                                    break;
                                case 3:
                                    var point3 = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.point3) : 0;
                                    Cell averagePoint3 = new Cell(); averagePoint3.DataType = CellValues.String; averagePoint3.CellValue = new CellValue(string.Format("{0} / 10", point3));
                                    break;
                                case 4:
                                    var point4 = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.point4) : 0;
                                    Cell averagePoint4 = new Cell(); averagePoint4.DataType = CellValues.String; averagePoint4.CellValue = new CellValue(string.Format("{0} / 10", point4));
                                    break;
                                case 5:
                                    var point5 = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.point5) : 0;
                                    Cell averagePoint5 = new Cell(); averagePoint5.DataType = CellValues.String; averagePoint5.CellValue = new CellValue(string.Format("{0} / 10", point5));
                                    break;
                                case 6:
                                    var point6 = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.point6) : 0;
                                    Cell averagePoint6 = new Cell(); averagePoint6.DataType = CellValues.String; averagePoint6.CellValue = new CellValue(string.Format("{0} / 10", point6));
                                    break;
                                case 7:
                                    var point7 = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.point7) : 0;
                                    Cell averagePoint7 = new Cell(); averagePoint7.DataType = CellValues.String; averagePoint7.CellValue = new CellValue(string.Format("{0} / 10", point7));
                                    break;
                                case 8:
                                    var point8 = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.point8) : 0;
                                    Cell averagePoint8 = new Cell(); averagePoint8.DataType = CellValues.String; averagePoint8.CellValue = new CellValue(string.Format("{0} / 10", point8));
                                    break;
                                case 9:
                                    var point9 = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.point9) : 0;
                                    Cell averagePoint9 = new Cell(); averagePoint9.DataType = CellValues.String; averagePoint9.CellValue = new CellValue(string.Format("{0} / 10", point9));
                                    break;
                                case 10:
                                    var point10 = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.point10) : 0;
                                    Cell averagePoint10 = new Cell(); averagePoint10.DataType = CellValues.String; averagePoint10.CellValue = new CellValue(string.Format("{0} / 10", point10));
                                    break;
                                default:
                                    break;
                            }

                            foreach (var UsersInRow in userNames)
                            {
                                if (userNamesWithID.Id == UsersInRow.Id)
                                {
                                    Cell cell = new Cell(); cell.DataType = CellValues.String; cell.CellValue = new CellValue(string.Empty);
                                    row.AppendChild(cell);
                                }
                                else
                                {
                                    var averageofUserList = meetingPointAverageList.Where(c => c.SenderUserId == UsersInRow.Id);

                                    switch (i)
                                    {
                                        case 1:
                                            var point1 = averageofUserList.Any() ? averageofUserList.Average(c => c.point1) : 0;
                                            Cell averagePoint1 = new Cell(); averagePoint1.DataType = CellValues.String; averagePoint1.CellValue = new CellValue(string.Format("{0} / 10", point1));
                                            row.AppendChild(averagePoint1);
                                            break;
                                        case 2:
                                            var point2 = averageofUserList.Any() ? averageofUserList.Average(c => c.point2) : 0;
                                            Cell averagePoint2 = new Cell(); averagePoint2.DataType = CellValues.String; averagePoint2.CellValue = new CellValue(string.Format("{0} / 10", point2));
                                            row.AppendChild(averagePoint2);
                                            break;
                                        case 3:
                                            var point3 = averageofUserList.Any() ? averageofUserList.Average(c => c.point3) : 0;
                                            Cell averagePoint3 = new Cell(); averagePoint3.DataType = CellValues.String; averagePoint3.CellValue = new CellValue(string.Format("{0} / 10", point3));
                                            row.AppendChild(averagePoint3);
                                            break;
                                        case 4:
                                            var point4 = averageofUserList.Any() ? averageofUserList.Average(c => c.point4) : 0;
                                            Cell averagePoint4 = new Cell(); averagePoint4.DataType = CellValues.String; averagePoint4.CellValue = new CellValue(string.Format("{0} / 10", point4));
                                            row.AppendChild(averagePoint4);
                                            break;
                                        case 5:
                                            var point5 = averageofUserList.Any() ? averageofUserList.Average(c => c.point5) : 0;
                                            Cell averagePoint5 = new Cell(); averagePoint5.DataType = CellValues.String; averagePoint5.CellValue = new CellValue(string.Format("{0} / 10", point5));
                                            row.AppendChild(averagePoint5);
                                            break;
                                        case 6:
                                            var point6 = averageofUserList.Any() ? averageofUserList.Average(c => c.point6) : 0;
                                            Cell averagePoint6 = new Cell(); averagePoint6.DataType = CellValues.String; averagePoint6.CellValue = new CellValue(string.Format("{0} / 10", point6));
                                            row.AppendChild(averagePoint6);
                                            break;
                                        case 7:
                                            var point7 = averageofUserList.Any() ? averageofUserList.Average(c => c.point7) : 0;
                                            Cell averagePoint7 = new Cell(); averagePoint7.DataType = CellValues.String; averagePoint7.CellValue = new CellValue(string.Format("{0} / 10", point7));
                                            row.AppendChild(averagePoint7);
                                            break;
                                        case 8:
                                            var point8 = averageofUserList.Any() ? averageofUserList.Average(c => c.point8) : 0;
                                            Cell averagePoint8 = new Cell(); averagePoint8.DataType = CellValues.String; averagePoint8.CellValue = new CellValue(string.Format("{0} / 10", point8));
                                            row.AppendChild(averagePoint8);
                                            break;
                                        case 9:
                                            var point9 = averageofUserList.Any() ? averageofUserList.Average(c => c.point9) : 0;
                                            Cell averagePoint9 = new Cell(); averagePoint9.DataType = CellValues.String; averagePoint9.CellValue = new CellValue(string.Format("{0} / 10", point9));
                                            row.AppendChild(averagePoint9);
                                            break;
                                        case 10:
                                            var point10 = averageofUserList.Any() ? averageofUserList.Average(c => c.point10) : 0;
                                            Cell averagePoint10 = new Cell(); averagePoint10.DataType = CellValues.String; averagePoint10.CellValue = new CellValue(string.Format("{0} / 10", point10));
                                            row.AppendChild(averagePoint10);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }

                            sheet.AppendChild(row);

                        }
                    }
                }




                Row headerRow = new Row();

                List<String> columns = new List<string>();

                foreach (System.Data.DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);

                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(column.ColumnName);
                    headerRow.AppendChild(cell);
                }

                sheetData.AppendChild(headerRow);

                foreach (DataRow dsrow in table.Rows)
                {
                    Row newRow = new Row();
                    foreach (String col in columns)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(dsrow[col].ToString());
                        newRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(newRow);
                }

                workbookPart.Workbook.Save();
            }
        }

    }

    public class UserNamesWithID
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }

}
