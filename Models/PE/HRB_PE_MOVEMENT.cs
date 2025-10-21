using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HCBPCoreUI_Backend.Models.PE
{
    [Table("HRB_PE_MOVEMENT", Schema = "HRBUDGET")]
    [Index(nameof(Id), nameof(PeMovId), Name = "IX_HRB_PE_MOVEMENT")]
    public class HRB_PE_MOVEMENT
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Key]
        [Column("PE_MOV_ID")]
        public string? PeMovId { get; set; }
        [Column("SEQ")]
        public int? Seq { get; set; }
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
        [Column("MOVE_IN_COMP")]
        public string? MoveInComp { get; set; }
        [Column("MOVE_OUT_COMP")]
        public string? MoveOutComp { get; set; }
        [Column("MOVE_IN_MONTH")]
        public string? MoveInMonth { get; set; }
        [Column("MOVE_OUT_MONTH")]
        public string? MoveOutMonth { get; set; }
        [Column("MOVE_IN_YEAR")]
        public string? MoveInYear { get; set; }
        [Column("MOVE_OUT_YEAR")]
        public string? MoveOutYear { get; set; }
        [Column("MOVE_IN_COST_CENTER_CODE")]
        public string? MoveInCostCenterCode { get; set; }
        [Column("MOVE_OUT_COST_CENTER_CODE")]
        public string? MoveOutCostCenterCode { get; set; }
        [Column("MOVE_IN_DIV")]
        public string? MoveInDiv { get; set; }
        [Column("MOVE_OUT_DIV")]
        public string? MoveOutDiv { get; set; }
        [Column("MOVE_IN_DEPT")]
        public string? MoveInDept { get; set; }
        [Column("MOVE_OUT_DEPT")]
        public string? MoveOutDept { get; set; }
        [Column("MOVE_IN_SECT")]
        public string? MoveInSect { get; set; }
        [Column("MOVE_OUT_SECT")]
        public string? MoveOutSect { get; set; }
        [Column("FLAG_MOVE")]
        public string? FlagMove { get; set; }
        [Column("REMARK_MOVE")]
        public string? RemarkMove { get; set; }
        [Column("STATUS")]
        public string? Status { get; set; }
        [Column("UPDATED_BY")]
        public string? UpdatedBy { get; set; }
        [Column("UPDATED_DATE")]
        public DateTime? UpdatedDate { get; set; }
        [Column("APPROVED_BY")]
        public string? ApprovedBy { get; set; }
        [Column("APPROVED_DATE")]
        public DateTime? ApprovedDate { get; set; }
    }
}
