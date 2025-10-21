using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HCBPCoreUI_Backend.Models.Employee
{
    [Table("HRB_EMP_EXPENSE_DATA", Schema = "HRBUDGET")]
    [Index(nameof(ExpenseId), nameof(EmpId), Name = "IX_HRB_EMP_EXPENSE_DATA")]
    public class HRB_EMP_EXPENSE_DATA
    {
        [Key]
        [Column("EXPENSE_ID")]
        public int ExpenseId { get; set; }
        [Key]
        [Column("EMP_ID")]
        public int EmpId { get; set; }
        [Column("COST_CENTER_CODE")]
        public string? CostCenterCode { get; set; }
        [Column("GL_CODE")]
        public string? GlCode { get; set; }
        [Column("AMOUNT")]
        public decimal? Amount { get; set; }
        [Column("EXPENSE_YEAR")]
        public int? ExpenseYear { get; set; }
        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }
        [Column("UPDATED_BY")]
        public string? UpdatedBy { get; set; }
        [Column("UPDATED_DATE")]
        public DateTime? UpdatedDate { get; set; }
    }
}
