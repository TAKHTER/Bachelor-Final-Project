using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class ClassRoom
    {
        [Key]
        public int ClassRoomId { set; get; }
        [Required(ErrorMessage = "Class room number required")]
        public string RoomNo { set; get; }
        public string Description { set; get; }
    }
}