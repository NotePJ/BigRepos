using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HCBPCoreUI_Backend.Models.Master
{
    [Table("HRB_MST_COST_CENTER")]
    public class HRB_MST_COST_CENTER
    {
        [Key]
        [Column("COST_ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CostId { get; set; }

        [Required]
        [Column("COMPANY_ID")]
        public int CompanyId { get; set; }

        [Required]
        [Column("COST_CENTER_CODE")]
        [StringLength(20)]
        public string CostCenterCode { get; set; } = string.Empty;

        [Column("COST_CENTER_NAME")]
        [StringLength(100)]
        public string? CostCenterName { get; set; }

        [Column("START_DATE")]
        public DateTime? StartDate { get; set; }

        [Column("END_DATE")]
        public DateTime? EndDate { get; set; }

        [Column("HR_EMP_CODE")]
        [StringLength(20)]
        public string? HrEmpCode { get; set; }

        [Column("IS_ACTIVE")]
        public bool? IsActive { get; set; } = true;

        [Column("UPDATED_BY")]
        [StringLength(50)]
        public string? UpdatedBy { get; set; }

        [Column("UPDATED_DATE")]
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
    }
}
