using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MvcApplication1.Models
{
    public class ParseHelper
    {
        private AllEntitiesContext db = new AllEntitiesContext();
        public int GetRoutineClassroomId(string roomNo)
        {
            ClassRoom aClassRoom = new ClassRoom();
            aClassRoom.RoomNo = roomNo;
            if (aClassRoom.RoomNo[0] == 'L') { aClassRoom.Description = "Lab Room\n"; }
            if (aClassRoom.RoomNo.EndsWith("AB") == true) { aClassRoom.Description += "Admin Building\n"; }
            if (aClassRoom.RoomNo.EndsWith("PP") == true) { aClassRoom.Description += "Prince Plaza\n"; }
            if (aClassRoom.RoomNo.EndsWith("0") || aClassRoom.RoomNo.EndsWith("1") || aClassRoom.RoomNo.EndsWith("2") || aClassRoom.RoomNo.EndsWith("3") || aClassRoom.RoomNo.EndsWith("4") || aClassRoom.RoomNo.EndsWith("5") || aClassRoom.RoomNo.EndsWith("6") || aClassRoom.RoomNo.EndsWith("7") == true)
            { aClassRoom.Description += "Main Building"; }
            if (db.ClassRooms.Where(a => a.RoomNo == aClassRoom.RoomNo).Count() < 1)
            {
                db.ClassRooms.Add(aClassRoom);
                db.SaveChanges();
            }

            return db.ClassRooms.Where(a => a.RoomNo == aClassRoom.RoomNo).Select(a => a.ClassRoomId).FirstOrDefault(); ;
        }

        public int GetRoutineCourseId(string corscode, string deptcode)
        {
            Course corse = new Course();
            if (corscode == "") 
            {
                corse.Code = "---";
                if (db.Courses.Where(a => a.Code == corse.Code).Count() < 1)
                {
                    corse.CourseDeptId = db.Departments.Where(a => a.Code == deptcode).Select(a => a.DepartmentId).FirstOrDefault();
                    corse.Name = "---"; corse.Level = "0"; corse.Term = "0"; corse.Credit = "0"; db.Courses.Add(corse); db.SaveChanges();
                }
            }
            else
            {
                if (corscode.Length <= 6) { corse.Code = corscode; }
                else
                {
                    string code = null;
                    if (corscode.Length > 7 && corscode[6] == 'L')
                    {
                        code = corscode[0].ToString();
                        code += corscode[1].ToString() + corscode[2].ToString() + corscode[3].ToString() + corscode[4].ToString() + corscode[5].ToString() + corscode[6].ToString();
                        //corscode.CopyTo(0, coursecode, 0, 8); 
                    }
                    else
                    {
                        code = corscode[0].ToString();
                        code += corscode[1].ToString() + corscode[2].ToString() + corscode[3].ToString() + corscode[4].ToString() + corscode[5].ToString() ;
                        //corscode.CopyTo(0, coursecode, 0, 7);
                    }
                    corse.Code = code;
                }
                if (db.Courses.Where(a => a.Code == corse.Code).Count() < 1)
                {
                    corse.CourseDeptId = db.Departments.Where(a => a.Code == deptcode).Select(a => a.DepartmentId).FirstOrDefault();
                    corse.Name = "---"; corse.Level = "0"; corse.Term = "0"; corse.Credit = "0"; db.Courses.Add(corse); db.SaveChanges();
                }
            }
            return db.Courses.Where(a => a.Code == corse.Code).Select(a => a.CourseId).FirstOrDefault();

        }
        public int GetRoutineCourseId(string corscode, string deptcode, string corsname)
        {
            Course corse = new Course();
            if (corscode == "")
            {
                corse.Code = "---";
                if (db.Courses.Where(a => a.Code == corse.Code).Count() < 1)
                {
                    corse.CourseDeptId = db.Departments.Where(a => a.Code == deptcode).Select(a => a.DepartmentId).FirstOrDefault();
                    corse.Name = "---"; corse.Level = "0"; corse.Term = "0"; corse.Credit = "0"; db.Courses.Add(corse); db.SaveChanges();
                }
            }
            else
            {
                if (corscode.Length <= 7) { corse.Code = corscode; }
                else
                {
                    string code = null;
                    if (corscode.Length ==9 )
                    {
                        code = corscode[0].ToString();
                        code += corscode[1].ToString() + corscode[2].ToString() + corscode[3].ToString() + corscode[4].ToString() + corscode[5].ToString() ;
                    }
                    else
                    {
                        code = corscode[0].ToString();
                        code += corscode[1].ToString() + corscode[2].ToString() + corscode[3].ToString() + corscode[4].ToString() + corscode[5].ToString() + corscode[6].ToString();
                    }
                    corse.Code = code;
                }
                if (db.Courses.Where(a => a.Code == corse.Code).Count() < 1)
                {
                    corse.CourseDeptId = db.Departments.Where(a => a.Code == deptcode).Select(a => a.DepartmentId).FirstOrDefault();
                    corse.Name =corsname; corse.Level = "0"; corse.Term = "0"; corse.Credit = "0"; db.Courses.Add(corse); db.SaveChanges();
                }
            }
            return db.Courses.Where(a => a.Code == corse.Code).Select(a => a.CourseId).FirstOrDefault();


        }
        public string GetSection(string corscode)
        {
            string Section=null;
            if (corscode.Length > 7)
            {
                if (corscode[6] == '(') 
                {
                    Section = corscode[7].ToString();
                }
                else if ( corscode[7] == '(')
                {
                    Section = corscode[8].ToString();
                }
                else if (corscode.Length > 8 && corscode[8] == '(')
                {
                    Section = corscode[9].ToString();
                }
                else if (corscode.Length > 9 && corscode[9] == '(')
                {
                    Section = corscode[10].ToString();
                }
                else 
                {
                }
            }
            return Section;
        }

        public void createIdForTeacher(string teacherInitial)
        {
            try
            {
                MembershipCreateStatus createStatus;
                new List<RegisterModel>
                {
                    
                    new RegisterModel{UserName=teacherInitial,Password="123456",ConfirmPassword="123456",Email="a@a.com"},
                }.ForEach(RegisterModel => Membership.CreateUser(RegisterModel.UserName, RegisterModel.Password, RegisterModel.Email, null, null, true, null, out createStatus));

                Roles.AddUserToRole(teacherInitial, "Teacher");
            }
            catch { }
        }

        public int GetTeacherId(string teacherIni) 
        {
            Teacher aTeacher = new Teacher();
            if (teacherIni == "")
            {
                aTeacher.Name = "---";
                aTeacher.Designation = "Unknown";
                aTeacher.Initial = "---";
                aTeacher.TeacherDeptId = db.Departments.Where(a => a.Name == "Unknown").Select(a => a.DepartmentId).FirstOrDefault();
                if (db.Teachers.Where(a => a.Initial == "---").Count() < 1)
                {
                    db.Teachers.Add(aTeacher);
                    db.SaveChanges();
                }
                teacherIni = "---";
            }
            else
            {

                if (db.Teachers.Where(a => a.Initial == teacherIni).Count() < 1)
                {
                    createIdForTeacher(teacherIni);
                    aTeacher.Name = "---";
                    aTeacher.Designation = "Unknown";
                    aTeacher.Initial = teacherIni;
                    aTeacher.TeacherDeptId = db.Departments.Where(a => a.Name == "Unknown").Select(a => a.DepartmentId).FirstOrDefault();
                    db.Teachers.Add(aTeacher);
                    db.SaveChanges();
                }

            }
            return db.Teachers.Where(a => a.Initial == teacherIni).Select(a => a.TeacherId).FirstOrDefault();
        }

        public List<string[]> GetTime(string[] word)
        {
            string[] delimeter = new string[1];
            delimeter[0] = "-";
            string[] timelist = null;
            //string[] timelist1 = new string[2]; 
            List<string[]> alist = new List<string[]>();
            foreach (var a in word) 
            {
                if (a != "") 
                {
                    string[] timelist1 = new string[2];
                    timelist = a.Split(delimeter, StringSplitOptions.None);
                    string[] delimeter1=new string[1];
                    delimeter1[0] = ".";
                    if (timelist[0][2] == '.' || timelist[0][1] == '.')
                    {
                        string[] b = timelist[0].Split(delimeter1, StringSplitOptions.None);
                        timelist[0] = b[0] + ":" + b[1];
                        //if (timelist[1][2] == '.' || timelist[1][1] == '.')
                        //{
                        //    b = timelist[1].Split(delimeter1, StringSplitOptions.None);
                        //    timelist[1] = b[0] + ":" + b[1];
                        //}
                    }
                    if (timelist.Length == 2) 
                    {
                        if (timelist[1][2] == '.' || timelist[1][1] == '.')
                        {
                            string[] b = timelist[1].Split(delimeter1, StringSplitOptions.None);
                            timelist[1] = b[0] + ":" + b[1];
                        }
                    }

                    //
                    //************To change the time to pm ****************
                    //
                    delimeter1[0] = ":";
                    string[] newtime = timelist[0].Split(delimeter1, StringSplitOptions.None);
                    int newtime1 = Convert.ToInt16(newtime[0]);
                    if (newtime1 < 8)
                    {
                        newtime1 += 12;
                        newtime[0] = newtime1.ToString();
                    }
                    timelist1[0] = newtime[0] + ":" + newtime[1];

                    string[] newtimeagain = timelist[1].Split(delimeter1, StringSplitOptions.None);
                    newtime1 = Convert.ToInt16(newtimeagain[0]);
                    if (newtime1 < 8 || newtime1== 9)
                    {
                        newtime1 += 12;
                        newtimeagain[0] = newtime1.ToString();
                    }
                    timelist1[1] = newtimeagain[0] + ":" + newtimeagain[1];

                    alist.Add(timelist1);   
                }

            }
            return alist;
        }
    }
}