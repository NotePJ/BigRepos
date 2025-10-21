using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HCBPCoreUI_Backend.Models.PE
{
    [Table("HRB_PE_COMPONENT", Schema = "HRBUDGET")]
    [Index(nameof(Id), nameof(PeComId), nameof(CompanyId), Name = "IX_HRB_PE_COMPONENT")]
    public class HRB_PE_COMPONENT
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Key]
        [Column("PE_COM_ID")]
        public string? PeComId { get; set; }
        [Key]
        [Column("COMPANY_ID")]
        public int CompanyId { get; set; }
        [Column("COMPANY_NAME")]
        public string? CompanyName { get; set; }
        [Column("PE_MONTH")]
        public int? PeMonth { get; set; }
        [Column("PE_YEAR")]
        public string? PeYear { get; set; }
        [Column("COST_CENTER_CODE")]
        public string? CostCenterCode { get; set; }
        [Column("COST_CENTER_NAME")]
        public string? CostCenterName { get; set; }
        [Column("DIVISION")]
        public string? Division { get; set; }
        [Column("DEPARTMENT")]
        public string? Department { get; set; }
        [Column("SECTION")]
        public string? Section { get; set; }
        [Column("GROUP_DATA")]
        public string? GroupData { get; set; }
        [Column("B0_HC")]
        public int? B0Hc { get; set; }
        [Column("B0_BASE_WAGE")]
        public string? B0BaseWage { get; set; }
        [Column("MOVE_IN_HC")]
        public int? MoveInHc { get; set; }
        [Column("MOVE_IN_BASE_WAGE")]
        public string? MoveInBaseWage { get; set; }
        [Column("MOVE_OUT_HC")]
        public int? MoveOutHc { get; set; }
        [Column("MOVE_OUT_BASE_WAGE")]
        public string? MoveOutBaseWage { get; set; }
        [Column("ADDITIONAL_HC")]
        public int? AdditionalHc { get; set; }
        [Column("ADDITIONAL_BASE_WAGE")]
        public string? AdditionalBaseWage { get; set; }
        [Column("CUT_HC")]
        public int? CutHc { get; set; }
        [Column("CUT_BASE_WAGE")]
        public string? CutBaseWage { get; set; }
        [Column("ACC_MOVE_IN_HC")]
        public int? AccMoveInHc { get; set; }
        [Column("ACC_MOVE_IN_BASE_WAGE")]
        public string? AccMoveInBaseWage { get; set; }
        [Column("ACC_MOVE_OUT_HC")]
        public int? AccMoveOutHc { get; set; }
        [Column("ACC_MOVE_OUT_BASE_WAGE")]
        public string? AccMoveOutBaseWage { get; set; }
        [Column("ACC_ADD_HC")]
        public int? AccAddHc { get; set; }
        [Column("ACC_ADD_BASE_WAGE")]
        public string? AccAddBaseWage { get; set; }
        [Column("ACC_CUT_HC")]
        public int? AccCutHc { get; set; }
        [Column("ACC_CUT_BASE_WAGE")]
        public string? AccCutBaseWage { get; set; }
        [Column("B1_HC")]
        public int? B1Hc { get; set; }
        [Column("B1_BASE_WAGE")]
        public string? B1BaseWage { get; set; }
        [Column("ACTUAL_HC")]
        public int? ActualHc { get; set; }
        [Column("ACTUAL_BASE_WAGE_PREMIUM")]
        public string? ActualBaseWagePremium { get; set; }
        [Column("DIFF_B0_HC")]
        public int? DiffB0Hc { get; set; }
        [Column("DIFF_B0_BASE_WAGE_PREMIUM")]
        public string? DiffB0BaseWagePremium { get; set; }
        [Column("DIFF_B1_HC")]
        public int? DiffB1Hc { get; set; }
        [Column("DIFF_B1_BASE_WAGE_PREMIUM")]
        public string? DiffB1BaseWagePremium { get; set; }
        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }
        [Column("UPDATED_BY")]
        public string? UpdatedBy { get; set; }
        [Column("UPDATED_DATE")]
        public DateTime? UpdatedDate { get; set; }
    }
}
