using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class AllEntitiesContext:DbContext
    {
        public DbSet<Department> Departments { set; get; }
        public DbSet<Teacher> Teachers { set; get; }
        public DbSet<Semester> Semesters { set; get; }
        public DbSet<ClassRoom> ClassRooms { set; get; }
        public DbSet<Course> Courses { set; get; }
        public DbSet<Day> Days { set; get; }
        public DbSet<RoutineType> RoutineTypes { set; get; }

        public DbSet<Routine> Routines { get; set; }
        public DbSet<EveRoutine> EveRoutines{ get; set; }
        public DbSet<TempRoutine> TempRoutines { get; set; }

        public DbSet<RoutineBackupModel> RoutineBackupModels { get; set; }
        public DbSet<EveRoutineBackupModel> EveRoutineBackupModels { get; set; }
        public DbSet<TempRoutineBackupModel> TempRoutineBackupModels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Routine>()
                    .HasRequired(c => c.Semester)
                    .WithMany()
                    .HasForeignKey(c => c.RoutineSemesterId)
                    .WillCascadeOnDelete(false);
            modelBuilder.Entity<Routine>()
                    .HasRequired(c => c.Course)
                    .WithMany()
                    .HasForeignKey(c => c.RoutineCourseId)
                    .WillCascadeOnDelete(false);
            modelBuilder.Entity<Routine>()
                   .HasRequired(c => c.ClassRoom)
                   .WithMany()
                   .HasForeignKey(c => c.RoutineClassroomId)
                   .WillCascadeOnDelete(false);
            modelBuilder.Entity<Routine>()
                  .HasRequired(c => c.Teacher)
                  .WithMany()
                  .HasForeignKey(c => c.RoutineTeacherId)
                  .WillCascadeOnDelete(false);
            modelBuilder.Entity<Routine>()
                  .HasRequired(c => c.Department)
                  .WithMany()
                  .HasForeignKey(c => c.RoutineDepartmentId)
                  .WillCascadeOnDelete(false);
            modelBuilder.Entity<Routine>()
                  .HasRequired(c => c.RoutineType)
                  .WithMany()
                  .HasForeignKey(c => c.RoutineTypeId)
                  .WillCascadeOnDelete(false);


            modelBuilder.Entity<RoutineBackupModel>()
                    .HasRequired(c => c.Semester)
                    .WithMany()
                    .HasForeignKey(c => c.RoutineSemesterId)
                    .WillCascadeOnDelete(false);
            modelBuilder.Entity<RoutineBackupModel>()
                    .HasRequired(c => c.Course)
                    .WithMany()
                    .HasForeignKey(c => c.RoutineCourseId)
                    .WillCascadeOnDelete(false);
            modelBuilder.Entity<RoutineBackupModel>()
                   .HasRequired(c => c.ClassRoom)
                   .WithMany()
                   .HasForeignKey(c => c.RoutineClassroomId)
                   .WillCascadeOnDelete(false);
            modelBuilder.Entity<RoutineBackupModel>()
                  .HasRequired(c => c.Teacher)
                  .WithMany()
                  .HasForeignKey(c => c.RoutineTeacherId)
                  .WillCascadeOnDelete(false);
            modelBuilder.Entity<RoutineBackupModel>()
                  .HasRequired(c => c.Department)
                  .WithMany()
                  .HasForeignKey(c => c.RoutineDepartmentId)
                  .WillCascadeOnDelete(false);
            modelBuilder.Entity<RoutineBackupModel>()
                  .HasRequired(c => c.RoutineType)
                  .WithMany()
                  .HasForeignKey(c => c.RoutineTypeId)
                  .WillCascadeOnDelete(false);


            modelBuilder.Entity<TempRoutineBackupModel>()
        .HasRequired(c => c.Semester)
        .WithMany()
        .HasForeignKey(c => c.RoutineSemesterId)
        .WillCascadeOnDelete(false);
            modelBuilder.Entity<TempRoutineBackupModel>()
                    .HasRequired(c => c.Course)
                    .WithMany()
                    .HasForeignKey(c => c.RoutineCourseId)
                    .WillCascadeOnDelete(false);
            modelBuilder.Entity<TempRoutineBackupModel>()
                   .HasRequired(c => c.ClassRoom)
                   .WithMany()
                   .HasForeignKey(c => c.RoutineClassroomId)
                   .WillCascadeOnDelete(false);
            modelBuilder.Entity<TempRoutineBackupModel>()
                  .HasRequired(c => c.Teacher)
                  .WithMany()
                  .HasForeignKey(c => c.RoutineTeacherId)
                  .WillCascadeOnDelete(false);
            modelBuilder.Entity<TempRoutineBackupModel>()
                  .HasRequired(c => c.Department)
                  .WithMany()
                  .HasForeignKey(c => c.RoutineDepartmentId)
                  .WillCascadeOnDelete(false);
            modelBuilder.Entity<TempRoutineBackupModel>()
                  .HasRequired(c => c.RoutineType)
                  .WithMany()
                  .HasForeignKey(c => c.RoutineTypeId)
                  .WillCascadeOnDelete(false);

        }
    }
}