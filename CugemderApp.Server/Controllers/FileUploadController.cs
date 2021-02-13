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
using System.Globalization;
using System.Net.Mail;
using System.Net;

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
                .Include(c => c.GroupNavigation)
                .Include(c => c.PointsNavigation)
                .Include(c => c.GenderNavigation)
                .Include(c => c.RelationshipNavigation)
                .Include(c => c.LocatedCityNavigation)
                .Include(c => c.PositionNavigation)
                .Include(c => c.JobTitleNavigation)
                .ToListAsync();
            var jobreferences = await _context.JobReferences.ToListAsync();
            excel.WriteExcelFile(users, meetings, jobreferences, _environment.ContentRootPath);

            return Ok();
        }

    }




    public class ExcelGenerator
    {

        public void WriteExcelFile(List<AspNetUsers> users, List<MeetingPoints> meetings, List<JobReferences> jobReferences, string path)
        {
            CultureInfo trTR = new CultureInfo("tr-TR");
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


            var fileMeeting = Path.Combine(path, "Staticfiles", "Contents", $"Birebir Gorusme - {DateTime.Today.ToString("D", trTR)}.xlsx");
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileMeeting, SpreadsheetDocumentType.Workbook))
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

                    var meetingPointAverageList = meetings.Where(c => c.ReceiverUserId == userNamesWithID.Id).ToList();


                    for (int i = 0; i < criterias.Count(); i++)
                    {
                        Row row = new Row();
                        if (i == 0)
                        {
                            var meetingPointAverage = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.TotalPoints) : 0;

                            Cell name = new Cell(); name.DataType = CellValues.String; name.CellValue = new CellValue(userNamesWithID.Name); row.AppendChild(name);
                            Cell criteriaName = new Cell(); criteriaName.DataType = CellValues.String; criteriaName.CellValue = new CellValue(criterias[i]); row.AppendChild(criteriaName);
                            Cell averagePoint = new Cell(); averagePoint.DataType = CellValues.String; averagePoint.CellValue = new CellValue(string.Format("{0:p0}", meetingPointAverage/100)); row.AppendChild(averagePoint);

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

                                    Cell cell = new Cell(); cell.DataType = CellValues.String; cell.CellValue = new CellValue(string.Format("{0:p2}", averageofUser/100));
                                    row.AppendChild(cell);
                                }
                            }

                            sheetData.AppendChild(row);
                        }
                        else
                        {
                            Cell name = new Cell(); name.DataType = CellValues.String; name.CellValue = new CellValue(string.Empty); row.AppendChild(name);
                            Cell criteriaName = new Cell(); criteriaName.DataType = CellValues.String; criteriaName.CellValue = new CellValue(criterias[i]); row.AppendChild(criteriaName);
                            switch (i)
                            {
                                case 1:
                                    var point1 = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.Point1) : 0;
                                    Cell averagePoint1 = new Cell(); averagePoint1.DataType = CellValues.String; averagePoint1.CellValue = new CellValue(string.Format("{0} / 10", (int)point1));
                                    row.AppendChild(averagePoint1);
                                    break;
                                case 2:
                                    var point2 = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.Point2) : 0;
                                    Cell averagePoint2 = new Cell(); averagePoint2.DataType = CellValues.String; averagePoint2.CellValue = new CellValue(string.Format("{0} / 10", (int)point2));
                                    row.AppendChild(averagePoint2);
                                    break;
                                case 3:
                                    var point3 = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.Point3) : 0;
                                    Cell averagePoint3 = new Cell(); averagePoint3.DataType = CellValues.String; averagePoint3.CellValue = new CellValue(string.Format("{0} / 10", (int)point3));
                                    row.AppendChild(averagePoint3);
                                    break;
                                case 4:
                                    var point4 = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.Point4) : 0;
                                    Cell averagePoint4 = new Cell(); averagePoint4.DataType = CellValues.String; averagePoint4.CellValue = new CellValue(string.Format("{0} / 10", (int)point4));
                                    row.AppendChild(averagePoint4);
                                    break;
                                case 5:
                                    var point5 = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.Point5) : 0;
                                    Cell averagePoint5 = new Cell(); averagePoint5.DataType = CellValues.String; averagePoint5.CellValue = new CellValue(string.Format("{0} / 10", (int)point5));
                                    row.AppendChild(averagePoint5);
                                    break;
                                case 6:
                                    var point6 = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.Point6) : 0;
                                    Cell averagePoint6 = new Cell(); averagePoint6.DataType = CellValues.String; averagePoint6.CellValue = new CellValue(string.Format("{0} / 10", (int)point6));
                                    row.AppendChild(averagePoint6);
                                    break;
                                case 7:
                                    var point7 = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.Point7) : 0;
                                    Cell averagePoint7 = new Cell(); averagePoint7.DataType = CellValues.String; averagePoint7.CellValue = new CellValue(string.Format("{0} / 10", (int)point7));
                                    row.AppendChild(averagePoint7);
                                    break;
                                case 8:
                                    var point8 = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.Point8) : 0;
                                    Cell averagePoint8 = new Cell(); averagePoint8.DataType = CellValues.String; averagePoint8.CellValue = new CellValue(string.Format("{0} / 10", (int)point8));
                                    row.AppendChild(averagePoint8);
                                    break;
                                case 9:
                                    var point9 = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.Point9) : 0;
                                    Cell averagePoint9 = new Cell(); averagePoint9.DataType = CellValues.String; averagePoint9.CellValue = new CellValue(string.Format("{0} / 10", (int)point9));
                                    row.AppendChild(averagePoint9);
                                    break;
                                case 10:
                                    var point10 = meetingPointAverageList.Any() ? meetingPointAverageList.Average(c => c.Point10) : 0;
                                    Cell averagePoint10 = new Cell(); averagePoint10.DataType = CellValues.String; averagePoint10.CellValue = new CellValue(string.Format("{0} / 10", (int)point10));
                                    row.AppendChild(averagePoint10);
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
                                    var averageofUserList = meetingPointAverageList.Where(c => c.SenderUserId == UsersInRow.Id).ToList();

                                    switch (i)
                                    {
                                        case 1:
                                            var point1 = averageofUserList.Any() ? averageofUserList.Average(c => c.Point1) : 0;
                                            Cell averagePoint1 = new Cell(); averagePoint1.DataType = CellValues.String; averagePoint1.CellValue = new CellValue(string.Format("{0} / 10", point1));
                                            row.AppendChild(averagePoint1);
                                            break;
                                        case 2:
                                            var point2 = averageofUserList.Any() ? averageofUserList.Average(c => c.Point2) : 0;
                                            Cell averagePoint2 = new Cell(); averagePoint2.DataType = CellValues.String; averagePoint2.CellValue = new CellValue(string.Format("{0} / 10", point2));
                                            row.AppendChild(averagePoint2);
                                            break;
                                        case 3:
                                            var point3 = averageofUserList.Any() ? averageofUserList.Average(c => c.Point3) : 0;
                                            Cell averagePoint3 = new Cell(); averagePoint3.DataType = CellValues.String; averagePoint3.CellValue = new CellValue(string.Format("{0} / 10", point3));
                                            row.AppendChild(averagePoint3);
                                            break;
                                        case 4:
                                            var point4 = averageofUserList.Any() ? averageofUserList.Average(c => c.Point4) : 0;
                                            Cell averagePoint4 = new Cell(); averagePoint4.DataType = CellValues.String; averagePoint4.CellValue = new CellValue(string.Format("{0} / 10", point4));
                                            row.AppendChild(averagePoint4);
                                            break;
                                        case 5:
                                            var point5 = averageofUserList.Any() ? averageofUserList.Average(c => c.Point5) :0;
                                            Cell averagePoint5 = new Cell(); averagePoint5.DataType = CellValues.String; averagePoint5.CellValue = new CellValue(string.Format("{0} / 10", point5));
                                            row.AppendChild(averagePoint5);
                                            break;
                                        case 6:
                                            var point6 = averageofUserList.Any() ? averageofUserList.Average(c => c.Point6) : 0;
                                            Cell averagePoint6 = new Cell(); averagePoint6.DataType = CellValues.String; averagePoint6.CellValue = new CellValue(string.Format("{0} / 10", point6));
                                            row.AppendChild(averagePoint6);
                                            break;
                                        case 7:
                                            var point7 = averageofUserList.Any() ? averageofUserList.Average(c => c.Point7) : 0;
                                            Cell averagePoint7 = new Cell(); averagePoint7.DataType = CellValues.String; averagePoint7.CellValue = new CellValue(string.Format("{0} / 10", point7));
                                            row.AppendChild(averagePoint7);
                                            break;
                                        case 8:
                                            var point8 = averageofUserList.Any() ? averageofUserList.Average(c => c.Point8) : 0;
                                            Cell averagePoint8 = new Cell(); averagePoint8.DataType = CellValues.String; averagePoint8.CellValue = new CellValue(string.Format("{0} / 10", point8));
                                            row.AppendChild(averagePoint8);
                                            break;
                                        case 9:
                                            var point9 = averageofUserList.Any() ? averageofUserList.Average(c => c.Point9) : 0;
                                            Cell averagePoint9 = new Cell(); averagePoint9.DataType = CellValues.String; averagePoint9.CellValue = new CellValue(string.Format("{0} / 10", point9));
                                            row.AppendChild(averagePoint9);
                                            break;
                                        case 10:
                                            var point10 = averageofUserList.Any() ? averageofUserList.Average(c => c.Point10) : 0;
                                            Cell averagePoint10 = new Cell(); averagePoint10.DataType = CellValues.String; averagePoint10.CellValue = new CellValue(string.Format("{0} / 10", point10));
                                            row.AppendChild(averagePoint10);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }

                            sheetData.AppendChild(row);
                        }

                    }

                    Row empty = new Row();
                    sheetData.AppendChild(empty);
                }



                //Row headerRow = new Row();

                //List<String> columns = new List<string>();

                //foreach (System.Data.DataColumn column in table.Columns)
                //{
                //    columns.Add(column.ColumnName);

                //    Cell cell = new Cell();
                //    cell.DataType = CellValues.String;
                //    cell.CellValue = new CellValue(column.ColumnName);
                //    headerRow.AppendChild(cell);
                //}

                //sheetData.AppendChild(headerRow);

                //foreach (DataRow dsrow in table.Rows)
                //{
                //    Row newRow = new Row();
                //    foreach (String col in columns)
                //    {
                //        Cell cell = new Cell();
                //        cell.DataType = CellValues.String;
                //        cell.CellValue = new CellValue(dsrow[col].ToString());
                //        newRow.AppendChild(cell);
                //    }

                //    sheetData.AppendChild(newRow);
                //}

                workbookPart.Workbook.Save();
            }


            var fileUsers = Path.Combine(path, "Staticfiles", "Contents", $"Uye Listesi - {DateTime.Today.ToString("D", trTR)}.xlsx");
            // user list
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileUsers, SpreadsheetDocumentType.Workbook))
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
                headerCell.CellValue = new CellValue("BEEPORT ÜYE LİSTESİ");
                header.AppendChild(headerCell);
                sheetData.AppendChild(header);


                // FIXED SUBHEADERS
                Row subHeader = new Row();
                Cell userName = new Cell(); userName.DataType = CellValues.String; userName.CellValue = new CellValue("ADI SOYADI"); subHeader.AppendChild(userName);
                Cell email = new Cell(); email.DataType = CellValues.String; email.CellValue = new CellValue("E-MAIL"); subHeader.AppendChild(email);
                Cell phoneNo = new Cell(); phoneNo.DataType = CellValues.String; phoneNo.CellValue = new CellValue("TELEFON NUMARASI"); subHeader.AppendChild(phoneNo);
                Cell dateOfBirth = new Cell(); dateOfBirth.DataType = CellValues.String; dateOfBirth.CellValue = new CellValue("DOĞUM TARİHİ"); subHeader.AppendChild(dateOfBirth);
                Cell group = new Cell(); group.DataType = CellValues.String; group.CellValue = new CellValue("GRUBU"); subHeader.AppendChild(group);
                Cell job = new Cell(); job.DataType = CellValues.String; job.CellValue = new CellValue("MESLEĞİ"); subHeader.AppendChild(job);
                Cell company = new Cell(); company.DataType = CellValues.String; company.CellValue = new CellValue("FİRMASI"); subHeader.AppendChild(company);
                Cell gender = new Cell(); gender.DataType = CellValues.String; gender.CellValue = new CellValue("CİNSİYETİ"); subHeader.AppendChild(gender);
                Cell points = new Cell(); points.DataType = CellValues.String; points.CellValue = new CellValue("PUANI"); subHeader.AppendChild(points);
                Cell position = new Cell(); position.DataType = CellValues.String; position.CellValue = new CellValue("POZİSYONU"); subHeader.AppendChild(position);
                Cell summary = new Cell(); summary.DataType = CellValues.String; summary.CellValue = new CellValue("HAKKINDA"); subHeader.AppendChild(summary);
                Cell website = new Cell(); website.DataType = CellValues.String; website.CellValue = new CellValue("WEB SİTESİ"); subHeader.AppendChild(website);
                Cell year = new Cell(); year.DataType = CellValues.String; year.CellValue = new CellValue("YIL"); subHeader.AppendChild(year);
                Cell relation = new Cell(); relation.DataType = CellValues.String; relation.CellValue = new CellValue("MEDENİ HALİ"); subHeader.AppendChild(relation);
                Cell city = new Cell(); city.DataType = CellValues.String; city.CellValue = new CellValue("ŞEHRİ"); subHeader.AppendChild(city);
                Cell photo = new Cell(); photo.DataType = CellValues.String; photo.CellValue = new CellValue("PROFİL FOTOĞRAFI"); subHeader.AppendChild(photo);

                sheetData.AppendChild(subHeader);


                foreach (var user in users)
                {
                    Row headerRow = new Row();
                    Cell userNameCell = new Cell(); userNameCell.DataType = CellValues.String; userNameCell.CellValue = new CellValue(user.FirstName + " " + user.LastName ); headerRow.AppendChild(userNameCell);
                    Cell emailCell = new Cell(); emailCell.DataType = CellValues.String; emailCell.CellValue = new CellValue(user.Email); headerRow.AppendChild(emailCell);
                    Cell phoneNoCell = new Cell(); phoneNoCell.DataType = CellValues.String; phoneNoCell.CellValue = new CellValue(user.PhoneNumber); headerRow.AppendChild(phoneNoCell);
                    Cell dateOfBirthCell = new Cell(); dateOfBirthCell.DataType = CellValues.String; dateOfBirthCell.CellValue = new CellValue(user.DateOfBirth.HasValue? user.DateOfBirth.Value.ToString() : ""); headerRow.AppendChild(dateOfBirthCell);
                    Cell groupCell = new Cell(); groupCell.DataType = CellValues.String; groupCell.CellValue = new CellValue((user.GroupNavigation != null) ? user.GroupNavigation.GroupName : ""); headerRow.AppendChild(groupCell);
                    Cell jobCell = new Cell(); jobCell.DataType = CellValues.String; jobCell.CellValue = new CellValue((user.JobTitleNavigation != null) ? user.JobTitleNavigation.TitleName : ""); headerRow.AppendChild(jobCell);
                    Cell companyCell = new Cell(); companyCell.DataType = CellValues.String; companyCell.CellValue = new CellValue(string.IsNullOrEmpty(user.Company) ? "" : user.Company); headerRow.AppendChild(companyCell);
                    Cell genderCell = new Cell(); genderCell.DataType = CellValues.String; genderCell.CellValue = new CellValue((user.GenderNavigation != null) ? user.GenderNavigation.GenderName : ""); headerRow.AppendChild(genderCell);
                    Cell pointsCell = new Cell(); pointsCell.DataType = CellValues.String; pointsCell.CellValue = new CellValue((user.PointsNavigation != null) ? user.PointsNavigation.TotalPoints.ToString() : ""); headerRow.AppendChild(pointsCell);
                    Cell positionCell = new Cell(); positionCell.DataType = CellValues.String; positionCell.CellValue = new CellValue((user.PositionNavigation != null) ? user.PositionNavigation.Position : ""); headerRow.AppendChild(positionCell);
                    Cell summaryCell = new Cell(); summaryCell.DataType = CellValues.String; summaryCell.CellValue = new CellValue(UnicodeToEnglish(user.Summary)); headerRow.AppendChild(summaryCell);
                    Cell websiteCell = new Cell(); websiteCell.DataType = CellValues.String; websiteCell.CellValue = new CellValue(string.IsNullOrEmpty(user.Website) ? "" : user.Website ); headerRow.AppendChild(websiteCell);
                    Cell yearCell = new Cell(); yearCell.DataType = CellValues.String; yearCell.CellValue = new CellValue(user.Year.HasValue ? user.Year.Value.ToString() : ""); headerRow.AppendChild(yearCell);
                    Cell relationCell = new Cell(); relationCell.DataType = CellValues.String; relationCell.CellValue = new CellValue((user.GenderNavigation != null) ? user.GenderNavigation.GenderName : ""); headerRow.AppendChild(relationCell);
                    Cell cityCell = new Cell(); cityCell.DataType = CellValues.String; cityCell.CellValue = new CellValue((user.LocatedCityNavigation != null) ? user.LocatedCityNavigation.CityName : ""); headerRow.AppendChild(cityCell);
                    Cell photoCell = new Cell(); photoCell.DataType = CellValues.String; photoCell.CellValue = new CellValue(user.PhotoUrl); headerRow.AppendChild(photoCell);

                    sheetData.AppendChild(headerRow);
                }

                
                workbookPart.Workbook.Save();
            }


            var fileReference = Path.Combine(path, "Staticfiles", "Contents", $"Is Kisi Yonlendirme - {DateTime.Today.ToString("D", trTR)}.xlsx");
            // job references
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileReference, SpreadsheetDocumentType.Workbook))
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
                headerCell.CellValue = new CellValue("İŞ / KİŞİ YÖNLENDİRME LİSTESİ");
                header.AppendChild(headerCell);
                sheetData.AppendChild(header);


                // FIXED SUBHEADERS
                Row subHeader = new Row();
                Cell sender = new Cell(); sender.DataType = CellValues.String; sender.CellValue = new CellValue("KİM"); subHeader.AppendChild(sender);
                Cell receiver = new Cell(); receiver.DataType = CellValues.String; receiver.CellValue = new CellValue("KİMİ"); subHeader.AppendChild(receiver);
                Cell expert = new Cell(); expert.DataType = CellValues.String; expert.CellValue = new CellValue("KİME"); subHeader.AppendChild(expert);
                Cell phoneNo = new Cell(); phoneNo.DataType = CellValues.String; phoneNo.CellValue = new CellValue("İLETİŞİM NUMARASI"); subHeader.AppendChild(phoneNo);
                Cell isHappened = new Cell(); isHappened.DataType = CellValues.String; isHappened.CellValue = new CellValue("GÖRÜŞME OLDU MU?"); subHeader.AppendChild(isHappened);
                Cell isProductive = new Cell(); isProductive.DataType = CellValues.String; isProductive.CellValue = new CellValue("GÖRÜŞME OLUMLU GEÇTİ Mİ?"); subHeader.AppendChild(isProductive);

                sheetData.AppendChild(subHeader);


                foreach (var jobReference in jobReferences)
                {
                    var senderName = userNames.Any(c => c.Id == jobReference.ReferencerId) ? userNames.Where(c => c.Id == jobReference.ReferencerId).First().Name : "Silinmiş Kullanıcı";
                    var receivername = userNames.Any(c => c.Id == jobReference.ReferencedId) ? userNames.Where(c => c.Id == jobReference.ReferencedId).First().Name : "Silinmiş Kullanıcı";
                    Row headerRow = new Row();
                    Cell senderCell = new Cell(); senderCell.DataType = CellValues.String; senderCell.CellValue = new CellValue(senderName); headerRow.AppendChild(senderCell);
                    Cell receiverCell = new Cell(); receiverCell.DataType = CellValues.String; receiverCell.CellValue = new CellValue(jobReference.ExpertName); headerRow.AppendChild(receiverCell);
                    Cell expertCell = new Cell(); expertCell.DataType = CellValues.String; expertCell.CellValue = new CellValue(receivername); headerRow.AppendChild(expertCell);
                    Cell phoneNoCell = new Cell(); phoneNoCell.DataType = CellValues.String; phoneNoCell.CellValue = new CellValue(jobReference.ExpertContact); headerRow.AppendChild(phoneNoCell);
                    Cell isHappenedCell = new Cell(); isHappenedCell.DataType = CellValues.String; isHappenedCell.CellValue = new CellValue(jobReference.IsMeetingDone ? "EVET" : "HAYIR"); headerRow.AppendChild(isHappenedCell);
                    Cell isProductiveCell = new Cell(); isProductiveCell.DataType = CellValues.String; isProductiveCell.CellValue = new CellValue(jobReference.IsProductive.HasValue ? jobReference.IsProductive.Value ? "EVET" : "HAYIR" : ""); headerRow.AppendChild(isProductiveCell);
                   

                    sheetData.AppendChild(headerRow);
                }


                workbookPart.Workbook.Save();
            }


            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress($"bkaantaze@gmail.com"));
                message.From = new MailAddress("beeportsifre@gmail.com", "BeePort");
                message.Subject = "Veritabani bilgiler";
                message.Body = $"Bilgiler ektedir";
                message.IsBodyHtml = true;
                message.Attachments.Add(new Attachment(fileUsers));
                message.Attachments.Add(new Attachment(fileReference));
                message.Attachments.Add(new Attachment(fileMeeting));
                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("beeportsifre@gmail.com", "184589Be-");
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }


        public string UnicodeToEnglish(string summary)
        {
            if (!string.IsNullOrEmpty(summary))
            {
                summary = summary.Replace("&#199;", "Ç");
                summary = summary.Replace("&#286;", "Ğ");
                summary = summary.Replace("&#304;", "İ");
                summary = summary.Replace("&#214;", "Ö");
                summary = summary.Replace("&#350;", "Ş");
                summary = summary.Replace("&#220;", "Ü");
                summary = summary.Replace("&#231;", "ç");
                summary = summary.Replace("&#287;", "ğ");
                summary = summary.Replace("&#305;", "ı");
                summary = summary.Replace("&#246;", "ö");
                summary = summary.Replace("&#351;", "ş");
                summary = summary.Replace("&#252;", "ü");

                return summary;
            }

            return "";
        }
    }

    public class UserNamesWithID
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }

}
