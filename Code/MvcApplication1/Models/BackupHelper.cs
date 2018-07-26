using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class BackupHelper
    {
        public TempRoutineBackupModel GetTempRoputineBackupData(TempRoutine temproutine)
        {
            TempRoutineBackupModel aTempRoutineBackupModel = new TempRoutineBackupModel();
            aTempRoutineBackupModel.Day = temproutine.Day;
            aTempRoutineBackupModel.Section = temproutine.Section;
            aTempRoutineBackupModel.Year = temproutine.Year;
            aTempRoutineBackupModel.Time_From = temproutine.Time_From;
            aTempRoutineBackupModel.Time_To = temproutine.Time_To;
            aTempRoutineBackupModel.TempRoutineDate = temproutine.TempRoutineDate;
            aTempRoutineBackupModel.username = temproutine.UserName;

            aTempRoutineBackupModel.RoutineClassroomId  = temproutine.RoutineClassroomId;
            aTempRoutineBackupModel.RoutineCourseId     = temproutine.RoutineCourseId;
            aTempRoutineBackupModel.RoutineDepartmentId = temproutine.RoutineDepartmentId;
            aTempRoutineBackupModel.RoutineSemesterId   = temproutine.RoutineSemesterId;
            aTempRoutineBackupModel.RoutineTeacherId    = temproutine.RoutineTeacherId;
            aTempRoutineBackupModel.RoutineTypeId       = temproutine.RoutineTypeId;
            return aTempRoutineBackupModel;
        }


        public EveRoutineBackupModel GetEveRoutineBackup(EveRoutine everoutine,string username)
        {
            EveRoutineBackupModel aEveRoutineBackupModel = new EveRoutineBackupModel();

            aEveRoutineBackupModel.BatchNumber = everoutine.BatchNumber;
            aEveRoutineBackupModel.Day = everoutine.Day;
            aEveRoutineBackupModel.Section = everoutine.Section;
            aEveRoutineBackupModel.Year = everoutine.Year;
            aEveRoutineBackupModel.Time_From = everoutine.Time_From;
            aEveRoutineBackupModel.Time_To = everoutine.Time_To;
            aEveRoutineBackupModel.username = username;
            aEveRoutineBackupModel.RoutineClassroomId = everoutine.RoutineClassroomId;
            aEveRoutineBackupModel.RoutineCourseId = everoutine.RoutineCourseId;
            aEveRoutineBackupModel.RoutineDepartmentId = everoutine.RoutineDepartmentId;
            aEveRoutineBackupModel.RoutineSemesterId = everoutine.RoutineSemesterId;
            aEveRoutineBackupModel.RoutineTeacherId = everoutine.RoutineTeacherId; 
            aEveRoutineBackupModel.RoutineTypeId = everoutine.RoutineTypeId; 
            
            return aEveRoutineBackupModel;
        }

        public RoutineBackupModel GetRoutineBackup(Routine routine, string username)
        {
            RoutineBackupModel aRoutineBackupModel = new RoutineBackupModel();

            aRoutineBackupModel.Day = routine.Day;
            aRoutineBackupModel.Section = routine.Section;
            aRoutineBackupModel.Year = routine.Year;
            aRoutineBackupModel.Time_From = routine.Time_From;
            aRoutineBackupModel.Time_To = routine.Time_To;
            aRoutineBackupModel.username = username;
            aRoutineBackupModel.RoutineClassroomId = routine.RoutineClassroomId;
            aRoutineBackupModel.RoutineCourseId = routine.RoutineCourseId;
            aRoutineBackupModel.RoutineDepartmentId =routine.RoutineDepartmentId;
            aRoutineBackupModel.RoutineSemesterId = routine.RoutineSemesterId;
            aRoutineBackupModel.RoutineTeacherId = routine.RoutineTeacherId;
            aRoutineBackupModel.RoutineTypeId = routine.RoutineTypeId;

            return aRoutineBackupModel;
        }

    }
}