using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text.RegularExpressions;

namespace MvcApplication1.Controllers
{
    public class UploadController : Controller
    {
        
        private AllEntitiesContext db = new AllEntitiesContext();
        public ParseHelper aParseHelper = new ParseHelper();
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch
            {
                ViewBag.M = "Unable to load Index UploadController";
                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(HttpPostedFileBase csvFile)
        {
            try
            {
                List<string[]> timeList = new List<string[]>();
                if (csvFile != null)
                {
                    List<Routine> routineList = new List<Routine>();
                    Routine aRoutine = new Routine();

                    StreamReader CsvReader = new StreamReader(csvFile.InputStream);
                    string inputLine = "";
                    var values = new List<string>();
                    var values1 = new List<string>();
                    while ((inputLine = CsvReader.ReadLine()) != null)
                    {
                        values.Add(inputLine.Trim());
                        values1.Add(inputLine.Trim().Replace(",", "").Replace(" ", ""));
                    }
                    CsvReader.Close();
                    char[] semester = new char[8];
                    char[] year = new char[5];
                    char[] dept = new char[10];
                    char[] RoutineType = new char[10];
                    //string day;
                    if (values1[1].StartsWith("Semester"))
                    {
                        int y = values1[1].Length - 4 - 9;
                        values1[1].CopyTo(9, semester, 0, y);
                        y += 9;
                        values1[1].CopyTo(y, year, 0, 4);
                        aRoutine.Year = new string(year);
                        int semesterFound = db.Semesters.Where(a => a.Name == new string(semester)).Count();
                        if (semesterFound < 1) { db.Semesters.Add(new Semester { Name = new string(semester) }); }
                        string semname = new string(semester);
                        aRoutine.RoutineSemesterId = db.Semesters.Where(a => a.Name == semname).Select(a => a.SemesterId).FirstOrDefault();
                        if (values1[3].StartsWith("Effective"))
                        {
                            string deptcode = null;
                            string[] delimeter1 = new string[1];
                            delimeter1[0] = ",";
                            string[] word = values[3].Split(delimeter1, StringSplitOptions.None);
                            foreach (var a in word)
                            {
                                if (a.StartsWith("CLASS ROUTINE FOR"))
                                {
                                    y = a.LastIndexOf("PROG") - 19;
                                    a.CopyTo(18, dept, 0, y);
                                    for (int i = 0; dept[i] != '\0'; i++)
                                    {
                                        deptcode += dept[i];
                                    }
                                    // deptcode = new string(dept);
                                    int x = a.LastIndexOf("(") + 1;
                                    y = a.LastIndexOf(")") - x;
                                    a.CopyTo(x, RoutineType, 0, y);
                                    if (db.Departments.Where(b => b.Code == deptcode).Count() < 1)
                                    {
                                        Department aDepartment = new Department();
                                        aDepartment.Code = new string(dept);
                                        aDepartment.Name = "Unknown";
                                        db.Departments.Add(aDepartment);
                                        db.SaveChanges();
                                    }
                                    break;
                                }
                            }

                            aRoutine.RoutineDepartmentId = db.Departments.Where(a => a.Code == deptcode).Select(a => a.DepartmentId).FirstOrDefault();

                            string RT = null;
                            for (int i = 0; RoutineType[i] != '\0'; i++)
                            {
                                RT += RoutineType[i];
                            }
                            //RT = Convert.ToString(RoutineType);
                            int RoutineId = db.RoutineTypes.Where(a => a.TypeName == RT).Select(a => a.RoutineTypeId).FirstOrDefault();
                            if (db.Routines.Where(a => (a.RoutineSemesterId == aRoutine.RoutineSemesterId) && (a.Year == aRoutine.Year) && (a.RoutineDepartmentId == aRoutine.RoutineDepartmentId) && (a.RoutineTypeId == RoutineId)).Select(a => a.RoutineId).Count() > 10)
                            {
                                //ViewBag.Messsage("This Routine already exists. You can not upload The same again.");
                                ViewData["adata"] = "This Routine already exists. You can not upload The same again.";
                                return View("UploadComplete");
                            }
                            else
                            {
                                if ((RT == "Day") || (RT == "DAY"))
                                {
                                    word = null;
                                    word = values[6].Split(delimeter1, StringSplitOptions.None);
                                    timeList = aParseHelper.GetTime(word);
                                    aRoutine.RoutineTypeId = db.RoutineTypes.Where(a => a.TypeName == "Day").Select(a => a.RoutineTypeId).FirstOrDefault();
                                    int count = values.Count;
                                    for (int i = 5; i < count; i++)
                                    {

                                        if (values1[5].ToString().EndsWith("day"))
                                        {
                                            aRoutine.Day = values1[i].ToString();
                                            i += 3;
                                            while (values1[i].ToString() != "" && i < count)
                                            {
                                                word = null;
                                                word = values[i].Split(delimeter1, StringSplitOptions.None);
                                                if (word[0].StartsWith("L") || word[0].StartsWith("1") || word[0].StartsWith("2") || word[0].StartsWith("3") || word[0].StartsWith("4") || word[0].StartsWith("5") || word[0].StartsWith("6") || word[0].StartsWith("7") || word[0].StartsWith("8"))
                                                {
                                                    int timecount = 0;
                                                    for (int k = 0; k < (timeList.Count*3); k += 3)
                                                    {
                                                        Routine routine = new Routine();
                                                        routine.RoutineTypeId = aRoutine.RoutineTypeId;
                                                        routine.RoutineSemesterId = aRoutine.RoutineSemesterId;
                                                        routine.RoutineDepartmentId = aRoutine.RoutineDepartmentId;
                                                        routine.Year = aRoutine.Year;
                                                        routine.Day = aRoutine.Day;


                                                        routine.Time_From = Convert.ToDateTime(timeList[timecount][0]);
                                                        routine.Time_To = Convert.ToDateTime(timeList[timecount][1]);
                                                        timecount += 1;
                                                        
                                                        //if (k < 3)
                                                        //{
                                                        //    routine.Time_From = Convert.ToDateTime(timeList[0][0]);
                                                        //    routine.Time_To = Convert.ToDateTime(timeList[0][1]);
                                                        //}
                                                        //else if (k < 6)
                                                        //{
                                                        //    routine.Time_From = Convert.ToDateTime(timeList[1][0]);
                                                        //    routine.Time_To = Convert.ToDateTime(timeList[1][1]);
                                                        //}
                                                        //else if (k < 9)
                                                        //{
                                                        //    routine.Time_From = Convert.ToDateTime(timeList[2][0]);
                                                        //    routine.Time_To = Convert.ToDateTime(timeList[2][1]);
                                                        //}
                                                        //else if (k < 12)
                                                        //{
                                                        //    routine.Time_From = Convert.ToDateTime(timeList[3][0]);
                                                        //    routine.Time_To = Convert.ToDateTime(timeList[3][1]);
                                                        //}
                                                        //else if (k < 15)
                                                        //{
                                                        //    routine.Time_From = Convert.ToDateTime(timeList[4][0]);
                                                        //    routine.Time_To = Convert.ToDateTime(timeList[4][1]);
                                                        //}
                                                        //else
                                                        //{
                                                        //    routine.Time_From = Convert.ToDateTime(timeList[5][0]);
                                                        //    routine.Time_To = Convert.ToDateTime(timeList[5][1]);
                                                        //}
                                                        //******************************************************

                                                        string roomNo = word[k];
                                                        roomNo = Regex.Replace(roomNo, @"\s", "");
                                                        roomNo = Regex.Replace(roomNo, @" ", "");
                                                        roomNo = Regex.Replace(roomNo, @"-", "");
                                                        routine.RoutineClassroomId = aParseHelper.GetRoutineClassroomId(roomNo);

                                                        //******************************************************

                                                        string astring = word[k + 1];
                                                        string corscode = Regex.Replace(astring, @"\s", "");
                                                        corscode = Regex.Replace(corscode, @" ", "");
                                                        corscode = Regex.Replace(corscode, @"-", "");
                                                        routine.RoutineCourseId = aParseHelper.GetRoutineCourseId(corscode, deptcode);

                                                        routine.Section = aParseHelper.GetSection(corscode);

                                                        //********************************************

                                                        string teacherIni = word[k + 2];
                                                        routine.RoutineTeacherId = aParseHelper.GetTeacherId(teacherIni);
                                                        routineList.Add(routine);
                                                        db.Routines.Add(routine);
                                                        db.SaveChanges();

                                                    }
                                                }
                                                i++;
                                            }
                                        }

                                    }
                                    ViewBag.Message = "CSV has been parsed Successfully";
                                    return View("UploadComplete");
                                }
                                else if (RT == "Evening" || RT == "EVENING")
                                {
                                    EveRoutine aEveRoutine = new EveRoutine();

                                    aRoutine.RoutineTypeId = db.RoutineTypes.Where(a => a.TypeName == "Evening").Select(a => a.RoutineTypeId).FirstOrDefault();
                                    //List<string[]> timeList = new List<string[]>();
                                    timeList = null;
                                    try
                                    {
                                        word = null;
                                        word = values[5].Split(delimeter1, StringSplitOptions.None);
                                        timeList = aParseHelper.GetTime(word);
                                    }
                                    catch
                                    {
                                        ViewBag.Message = "CSV File Format Does not meet the specification";
                                        return View("UploadComplete");
                                    }
                                    int count = values.Count;
                                    for (int i = 7; i < count; i++)
                                    {
                                        word = null;
                                        word = values[i].Split(delimeter1, StringSplitOptions.None);

                                        if (word[0].EndsWith("day"))
                                        {
                                            aRoutine.Day = word[0];
                                            while (values1[i].ToString() != "" && i < count)
                                            {
                                                word = null;
                                                word = values[i].Split(delimeter1, StringSplitOptions.None);
                                                if (word[1].StartsWith("L") || word[1].StartsWith("1") || word[1].StartsWith("2") || word[1].StartsWith("3") || word[1].StartsWith("4") || word[1].StartsWith("5") || word[1].StartsWith("6") || word[1].StartsWith("7") || word[1].StartsWith("8"))
                                                {
                                                    int timecount = 0;
                                                    for (int k = 1; k < (timeList.Count * 5); k += 5)
                                                    {
                                                        if (word[k] != "")
                                                        {
                                                            Routine routine = new Routine();
                                                            aEveRoutine.RoutineTypeId = aRoutine.RoutineTypeId;
                                                            aEveRoutine.RoutineSemesterId = aRoutine.RoutineSemesterId;
                                                            aEveRoutine.RoutineDepartmentId = aRoutine.RoutineDepartmentId;
                                                            aEveRoutine.Year = aRoutine.Year;
                                                            aEveRoutine.Day = aRoutine.Day;

                                                            //*************************************************

                                                            aEveRoutine.Time_From = Convert.ToDateTime(timeList[timecount][0]);
                                                            aEveRoutine.Time_To = Convert.ToDateTime(timeList[timecount][1]);
                                                            timecount += 1;
                                                            
                                                            //if (k < 5)
                                                            //{
                                                            //    aEveRoutine.Time_From = Convert.ToDateTime(timeList[0][0]);
                                                            //    aEveRoutine.Time_To = Convert.ToDateTime(timeList[0][1]);
                                                            //}
                                                            //else
                                                            //{
                                                            //    aEveRoutine.Time_From = Convert.ToDateTime(timeList[1][0]);
                                                            //    aEveRoutine.Time_To = Convert.ToDateTime(timeList[1][1]);
                                                            //}

                                                            //*************************************************
                                                            string roomNo = word[k];
                                                            roomNo = Regex.Replace(roomNo, @"\s", "");
                                                            roomNo = Regex.Replace(roomNo, @" ", "");
                                                            roomNo = Regex.Replace(roomNo, @"-", "");
                                                            aEveRoutine.RoutineClassroomId = aParseHelper.GetRoutineClassroomId(roomNo);

                                                            //*************************************************

                                                            string astring = word[k + 1];
                                                            string corscode = Regex.Replace(astring, @"\s", "");
                                                            corscode = Regex.Replace(corscode, @" ", "");
                                                            corscode = Regex.Replace(corscode, @"-", "");
                                                            //corscode = word[k + 1];
                                                            string corsname = word[k + 2];
                                                            aEveRoutine.RoutineCourseId = aParseHelper.GetRoutineCourseId(corscode, deptcode, corsname);

                                                            aEveRoutine.Section = aParseHelper.GetSection(corscode);
                                                            //*************************************************
                                                            if (word[k + 3] != "")
                                                            {
                                                                aEveRoutine.BatchNumber = Convert.ToInt16(word[k + 3]);
                                                            }
                                                            else
                                                            {
                                                                aEveRoutine.BatchNumber = 0;
                                                            }
                                                            string teacherIni = word[k + 4];
                                                            aEveRoutine.RoutineTeacherId = aParseHelper.GetTeacherId(teacherIni);
                                                            routineList.Add(routine);
                                                            db.EveRoutines.Add(aEveRoutine);
                                                            db.SaveChanges();

                                                        }
                                                    }
                                                }

                                                i++;
                                            }
                                        }
                                        else
                                        {
                                            if (values1[i] == "")
                                            {
                                                i++;
                                            }
                                            else if (values1[i].EndsWith("0"))
                                            {
                                                word = null;
                                                word = values[i].Split(delimeter1, StringSplitOptions.None);
                                                timeList = aParseHelper.GetTime(word);
                                                //i++;
                                            }
                                            else
                                            {
                                                ViewBag.Message = "CSV File Format Does not meet the specification2";
                                                return View("UploadComplete");
                                            }
                                        }
                                    }

                                    //word = null;
                                    //word = values[7].Split(delimeter1, StringSplitOptions.None);
                                    //word = null;
                                    //word = values[8].Split(delimeter1, StringSplitOptions.None);
                                    ViewBag.Message = "CSV has been parsed Successfully";
                                    return View("UploadComplete");
                                }
                                else
                                {
                                    ViewBag.Message = "CSV File Format Does not meet the specification3";
                                    return View("UploadComplete");
                                }
                            }
                        }
                        else
                        {
                            ViewBag.Message = "CSV File Format Does not meet the specification5";
                            return View("UploadComplete");
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Only CSV file with specified format is allowed to upload";
                        return View("UploadComplete");
                    }
                }
                ViewBag.message = "File can not be empty";
                return View("UploadComplete");

            }
            catch
            {
                ViewBag.M1="Only CSV file is allowed to upload";
                return View("UploadComplete");
            }
        }
        }
}
