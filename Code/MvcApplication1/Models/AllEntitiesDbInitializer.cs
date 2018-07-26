using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MvcApplication1.Models
{
    public class AllEntitiesDbInitializer : DropCreateDatabaseIfModelChanges<AllEntitiesContext>
    {
            protected override void Seed(AllEntitiesContext context)
            {
                //Department Sample Data
                new List<Department>
                {
                    new Department {Code = "CSE", Name = "Computer Scirnce And Engineering"},
                    new Department {Code = "EEE", Name = "Electrical And Electronics Engineering"},
                    new Department {Code = "BBA", Name = " Bachelor of Business Administration "},
                    new Department {Code = "TE", Name = "Textile Engineering"},
                    new Department{Code = "Unknown", Name = "Unknown"},
                }.ForEach(department => context.Departments.Add(department));
                MembershipCreateStatus createStatus;
                new List<RegisterModel>
                {
                    
                    new RegisterModel{UserName="a",Password="123456",ConfirmPassword="123456",Email="a@a.com"},
                    new RegisterModel{UserName="b",Password="123456",ConfirmPassword="123456",Email="b@b.com"},
                }.ForEach(RegisterModel => Membership.CreateUser(RegisterModel.UserName, RegisterModel.Password, RegisterModel.Email, null, null, true, null, out createStatus));

                Roles.CreateRole("Admin");
                Roles.CreateRole("Teacher");
                Roles.AddUserToRole("a", "Admin");
                Roles.AddUserToRole("b", "Teacher");
                new List<Day>
                   {
                       
                       new Day { DayName = "Saturday"},
                       new Day { DayName = "Sunday"},
                       new Day { DayName = "Monday"},
                       new Day { DayName = "Tuesday"},
                       new Day { DayName = "Wednesday"},
                       new Day { DayName = "Thrusday"},
                       new Day { DayName = "Friday"},

                   }.ForEach(day => context.Days.Add(day));

                new List<ClassRoom>
                   {
                       new ClassRoom { RoomNo = "504 AB",Description="Admin Building"},
                      // new ClassRoom { RoomNo = "505 AB",Description="Admin Building"},
                       new ClassRoom {RoomNo = "604 AB",Description="Admin Building"},
                       //new ClassRoom { RoomNo = "404",Description="Main Building"},
                       //new ClassRoom { RoomNo = "302 PP",Description="Prince Plaza"},
                       //new ClassRoom { RoomNo = "303 PP",Description="Prince Plaza"},
                   }.ForEach(classRoom => context.ClassRooms.Add(classRoom));

                //Semester Sample Data
                new List<Semester>
                {
                    new Semester {Name = "Summer"},
                    new Semester {Name = "Spring"},
                    new Semester {Name = "Fall"},
                }.ForEach(sems=>context.Semesters.Add(sems)); 
             
                new List<RoutineType>
                {
                    new RoutineType {TypeName="Day"},
                     new RoutineType {TypeName="Evening"},
                     new RoutineType {TypeName="Temporary"}
                }.ForEach(type=>context.RoutineTypes.Add(type));
            }
        }

    }
