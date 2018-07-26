using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class EveRoutineBackupModel:RoutineBackupModel
    {
        public int BatchNumber { set; get; }
    }
}