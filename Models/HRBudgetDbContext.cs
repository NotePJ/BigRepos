using Microsoft.EntityFrameworkCore;
using HCBPCoreUI_Backend.Models.Budget;
using HCBPCoreUI_Backend.Models.Config;
using HCBPCoreUI_Backend.Models.Master;
using HCBPCoreUI_Backend.Models.Cost;
using HCBPCoreUI_Backend.Models.Employee;
using HCBPCoreUI_Backend.Models.PE;
using HCBPCoreUI_Backend.Models.Log;

namespace HCBPCoreUI_Backend.Models
{
  public class HRBudgetDbContext : DbContext
  {
    public HRBudgetDbContext(DbContextOptions<HRBudgetDbContext> options) : base(options) { }

    // Budget
    public DbSet<HRB_BUDGET_BIGC> HRB_BUDGET_BIGC { get; set; }
    public DbSet<HRB_BUDGET_BJC> HRB_BUDGET_BJC { get; set; }

    // Config
    public DbSet<HRB_CONF_JB_BENEFITS> HRB_CONF_JB_BENEFITS { get; set; }
    public DbSet<HRB_MST_ITEM_CONFIG> HRB_MST_ITEM_CONFIG { get; set; }
    public DbSet<HRB_CONF_GROUP_RUNRATE> HRB_CONF_GROUP_RUNRATE { get; set; }
    public DbSet<HRB_CONF_FLEETCARD> HRB_CONF_FLEETCARD { get; set; }
    public DbSet<HRB_CONF_BUDGET_BONUS> HRB_CONF_BUDGET_BONUS { get; set; }
    public DbSet<HRB_CONF_SALARY_STRUCTURE> HRB_CONF_SALARY_STRUCTURE { get; set; }

    // Master
    public DbSet<HRB_MST_COMPANY> HRB_MST_COMPANY { get; set; }
    public DbSet<HRB_MST_COST_CENTER> HRB_MST_COST_CENTER { get; set; }
    public DbSet<HRB_MST_GL_ACCOUNT> HRB_MST_GL_ACCOUNT { get; set; }
    public DbSet<HRB_MST_JOB_BAND> HRB_MST_JOB_BAND { get; set; }
    public DbSet<HRB_MST_POSITION> HRB_MST_POSITION { get; set; }
    public DbSet<HRB_MST_STATUS> HRB_MST_STATUS { get; set; }

    // Cost
    public DbSet<HRB_COST_GROUP_MAPPING> HRB_COST_GROUP_MAPPING { get; set; }
    public DbSet<HRB_COST_GROUP_RUNRATE> HRB_COST_GROUP_RUNRATE { get; set; }

    // Employee
    public DbSet<HRB_EMPLOYEE_DATA> HRB_EMPLOYEE_DATA { get; set; }
    public DbSet<HRB_EMP_EXPENSE_DATA> HRB_EMP_EXPENSE_DATA { get; set; }
    public DbSet<HRB_EMP_EXPENSE_BIGC> HRB_EMP_EXPENSE_BIGC { get; set; }
    public DbSet<HRB_EMP_EXPENSE_BJC> HRB_EMP_EXPENSE_BJC { get; set; }
    public DbSet<HRB_CONF_HRBP> HRB_CONF_HRBP { get; set; }

    // PE
    public DbSet<HRB_PE_COMPONENT> HRB_PE_COMPONENT { get; set; }
    public DbSet<HRB_PE_MOVEMENT> HRB_PE_MOVEMENT { get; set; }

    // Log
    public DbSet<HRB_EMAIL_LOG> HRB_EMAIL_LOG { get; set; }
    public DbSet<HRB_UPLOAD_LOG> HRB_UPLOAD_LOG { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.HasDefaultSchema("dbo");

      // Budget - ตารางใหม่
      modelBuilder.Entity<HRB_BUDGET_BIGC>()
          .HasKey(b => b.BudgetId);
      modelBuilder.Entity<HRB_BUDGET_BJC>()
          .HasKey(b => b.BudgetId);

      // Config
      modelBuilder.Entity<HRB_CONF_JB_BENEFITS>()
          .HasKey(b => b.BenfId);
      modelBuilder.Entity<HRB_MST_ITEM_CONFIG>()
          .HasKey(i => i.ItemId);
      modelBuilder.Entity<HRB_CONF_GROUP_RUNRATE>()
          .HasKey(r => r.RunId);
      modelBuilder.Entity<HRB_CONF_FLEETCARD>()
          .HasKey(f => f.FcId);
      modelBuilder.Entity<HRB_CONF_BUDGET_BONUS>()
          .HasKey(b => b.BgrateId);
      modelBuilder.Entity<HRB_CONF_SALARY_STRUCTURE>()
          .HasKey(s => s.SsId);

      // Master
      modelBuilder.Entity<HRB_MST_COMPANY>()
          .HasKey(c => c.CompanyId);
      modelBuilder.Entity<HRB_MST_COST_CENTER>()
          .HasKey(cc => cc.CostId);
      modelBuilder.Entity<HRB_MST_GL_ACCOUNT>()
          .HasKey(gl => gl.GlId);
      modelBuilder.Entity<HRB_MST_JOB_BAND>()
          .HasKey(jb => jb.JbId);
      modelBuilder.Entity<HRB_MST_POSITION>()
          .HasKey(p => p.PosId);
      modelBuilder.Entity<HRB_MST_STATUS>()
          .HasKey(s => s.StatusId);

      // Cost
      modelBuilder.Entity<HRB_COST_GROUP_MAPPING>()
          .HasKey(mp => mp.GroupId);
      modelBuilder.Entity<HRB_COST_GROUP_RUNRATE>()
          .HasKey(r => r.MapId);

      // Employee
      modelBuilder.Entity<HRB_EMPLOYEE_DATA>()
          .HasKey(e => e.EmpId);
      modelBuilder.Entity<HRB_EMP_EXPENSE_DATA>()
          .HasKey(e => e.ExpenseId);
      modelBuilder.Entity<HRB_EMP_EXPENSE_BIGC>()
          .HasKey(e => e.ExpenseId);
      modelBuilder.Entity<HRB_EMP_EXPENSE_BJC>()
          .HasKey(e => e.ExpenseId);
      modelBuilder.Entity<HRB_CONF_HRBP>()
          .HasKey(h => h.HrbpId);

      // PE
      modelBuilder.Entity<HRB_PE_COMPONENT>()
          .HasKey(c => c.PeComId);
      modelBuilder.Entity<HRB_PE_MOVEMENT>()
          .HasKey(m => m.PeMovId);

      // Log
      modelBuilder.Entity<HRB_EMAIL_LOG>()
          .HasKey(l => l.EmailId);
      modelBuilder.Entity<HRB_UPLOAD_LOG>()
          .HasKey(l => l.Id);
    }
  }
}
