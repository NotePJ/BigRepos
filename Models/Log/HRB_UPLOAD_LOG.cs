using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HCBPCoreUI_Backend.Models.Log
{
    [Table("HRB_UPLOAD_LOG", Schema = "HRBUDGET")]
    [Index(nameof(Id), nameof(Seq), Name = "IX_HRB_UPLOAD_LOG")]
    public class HRB_UPLOAD_LOG
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Key]
        [Column("SEQ")]
        public int Seq { get; set; }
        [Column("FILE_NAME")]
        public string? FileName { get; set; }
        [Column("FILE_SIZE")]
        public string? FileSize { get; set; }
        [Column("FILE_DATA")]
        public byte[]? FileData { get; set; }
        [Column("UPLOADED_BY")]
        public string? UploadedBy { get; set; }
        [Column("UPLOADED_DATE")]
        public DateTime? UploadedDate { get; set; }
    }
}
