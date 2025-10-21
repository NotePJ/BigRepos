using System;

namespace HCBPCoreUI_Backend.DTOs.Budget
{
    /// <summary>
    /// DTO สำหรับ Budget ของ BIGC - ตาม HRB_BUDGET_BIGC model
    /// </summary>
    public class BudgetBigcDto : BaseBudgetDto
    {
        public override string CompanyType => "BIGC";

        // ===== BIGC Specific Fields (not in base class) =====

        // ===== LE (Last Estimate) Fields =====
        public decimal? TotalPayrollLe { get; set; }
        public decimal? FleetCardPeLe { get; set; }
        public decimal? CarAllowanceLe { get; set; }
        public decimal? LicenseAllowanceLe { get; set; }
        public decimal? HousingAllowanceLe { get; set; }
        public decimal? GasolineAllowanceLe { get; set; }
        public decimal? WageStudentLe { get; set; }
        public decimal? CarRentalPeLe { get; set; }
        public decimal? SkillPayAllowanceLe { get; set; }
        public decimal? OtherAllowanceLe { get; set; }
        public decimal? SocialSecurityLe { get; set; }
        public decimal? LaborFundFeeLe { get; set; }
        public decimal? OtherStaffBenefitLe { get; set; }
        public decimal? ProvidentFundLe { get; set; }
        public decimal? EmployeeWelfareLe { get; set; }
        public decimal? ProvisionLe { get; set; }
        public decimal? InterestLe { get; set; }
        public decimal? StaffInsuranceLe { get; set; }
        public decimal? MedicalExpenseLe { get; set; }
        public decimal? MedicalInhouseLe { get; set; }
        public decimal? TrainingLe { get; set; }
        public decimal? LongServiceLe { get; set; }

        // ===== Budget Fields (Current Year) =====
        public decimal? TotalPayroll { get; set; }
        public string? BonusTypes { get; set; }
        public decimal? FleetCardPe { get; set; }
        public decimal? CarAllowance { get; set; }
        public decimal? LicenseAllowance { get; set; }
        public decimal? HousingAllowance { get; set; }
        public decimal? GasolineAllowance { get; set; }
        public decimal? WageStudent { get; set; }
        public decimal? CarRentalPe { get; set; }
        public decimal? SkillPayAllowance { get; set; }
        public decimal? OtherAllowance { get; set; }
        public decimal? SocialSecurity { get; set; }
        public decimal? LaborFundFee { get; set; }
        public decimal? OtherStaffBenefit { get; set; }
        public decimal? ProvidentFund { get; set; }
        public decimal? EmployeeWelfare { get; set; }
        public decimal? Provision { get; set; }
        public decimal? Interest { get; set; }
        public decimal? StaffInsurance { get; set; }
        public decimal? MedicalExpense { get; set; }
        public decimal? MedicalInhouse { get; set; }
        public decimal? Training { get; set; }
        public decimal? LongService { get; set; }

        // ===== BIGC Specific Organization =====
        public string? OrgUnitL1 { get; set; }            // Org Unit Level 1
        public string? OrgUnitL2 { get; set; }            // Org Unit Level 2
        public string? OrgUnitL3 { get; set; }            // Org Unit Level 3
        public string? OrgUnitL4 { get; set; }            // Org Unit Level 4
        public string? OrgUnitL5 { get; set; }            // Org Unit Level 5

        // ===== BIGC Allowances (Additional from base) =====
        public decimal? SkillPayNew { get; set; }         // เบี้ยเลี้ยงความชำนาญใหม่
        public decimal? SkillPayNewLe { get; set; }       // เบี้ยเลี้ยงความชำนาญใหม่ (LE)
        public decimal? SkillPayNewNx { get; set; }       // เบี้ยเลี้ยงความชำนาญใหม่ (Next Year)

        // ===== BIGC Performance Components =====
        public decimal? SalesAllowancePc { get; set; }    // PC เบี้ยเลี้ยงขาย
        public decimal? SalesAllowancePcLe { get; set; }  // PC เบี้ยเลี้ยงขาย (LE)
        public decimal? SalesAllowancePcNx { get; set; }  // PC เบี้ยเลี้ยงขาย (Next Year)
        public decimal? DiligenceAllowancePc { get; set; } // PC เบี้ยขยัน
        public decimal? DiligenceAllowancePcLe { get; set; } // PC เบี้ยขยัน (LE)
        public decimal? DiligenceAllowancePcNx { get; set; } // PC เบี้ยขยัน (Next Year)
        public decimal? RiskAllowancePc { get; set; }     // PC เบี้ยเสี่ยง
        public decimal? RiskAllowancePcLe { get; set; }   // PC เบี้ยเสี่ยง (LE)
        public decimal? RiskAllowancePcNx { get; set; }   // PC เบี้ยเสี่ยง (Next Year)
        public decimal? PostAllowancePc { get; set; }     // PC เบี้ยตำแหน่ง
        public decimal? PostAllowancePcLe { get; set; }   // PC เบี้ยตำแหน่ง (LE)
        public decimal? PostAllowancePcNx { get; set; }   // PC เบี้ยตำแหน่ง (Next Year)
        public decimal? PhoneAllowancePc { get; set; }    // PC เบี้ยโทรศัพท์
        public decimal? PhoneAllowancePcLe { get; set; }  // PC เบี้ยโทรศัพท์ (LE)
        public decimal? PhoneAllowancePcNx { get; set; }  // PC เบี้ยโทรศัพท์ (Next Year)
        public decimal? TransportationPc { get; set; }    // PC คมนาคม
        public decimal? TransportationPcLe { get; set; }  // PC คมนาคม (LE)
        public decimal? TransportationPcNx { get; set; }  // PC คมนาคม (Next Year)
        public decimal? SafetyShoesPc { get; set; }       // PC รองเท้านิรภัย
        public decimal? SafetyShoesPcLe { get; set; }     // PC รองเท้านิรภัย (LE)
        public decimal? SafetyShoesPcNx { get; set; }     // PC รองเท้านิรภัย (Next Year)

        // ===== BIGC Social Security & Benefits =====
        public decimal? SocialSecurityTmp { get; set; }   // ประกันสังคมชั่วคราว
        public decimal? SocialSecurityTmpLe { get; set; } // ประกันสังคมชั่วคราว (LE)
        public decimal? SocialSecurityTmpNx { get; set; } // ประกันสังคมชั่วคราว (Next Year)
        public decimal? WorkmenCompensation { get; set; } // เงินทดแทน
        public decimal? WorkmenCompensationLe { get; set; } // เงินทดแทน (LE)
        public decimal? WorkmenCompensationNx { get; set; } // เงินทดแทน (Next Year)
        public decimal? Welfare { get; set; }             // สวัสดิการ
        public decimal? WelfareLe { get; set; }           // สวัสดิการ (LE)
        public decimal? WelfareNx { get; set; }           // สวัสดิการ (Next Year)
        public decimal? VacationPay { get; set; }         // ค่าจ้างวันหยุด
        public decimal? VacationPayLe { get; set; }       // ค่าจ้างวันหยุด (LE)
        public decimal? VacationPayNx { get; set; }       // ค่าจ้างวันหยุด (Next Year)
        public decimal? SeverancePay { get; set; }        // เงินชดเชย
        public decimal? SeverancePayLe { get; set; }      // เงินชดเชย (LE)
        public decimal? SeverancePayNx { get; set; }      // เงินชดเชย (Next Year)

        // ===== BIGC Car & Transportation =====
        public decimal? FleetCard { get; set; }           // Fleet Card
        public decimal? FleetCardLe { get; set; }         // Fleet Card (LE)
        public decimal? FleetCardNx { get; set; }         // Fleet Card (Next Year)
        public decimal? CarMaintenance { get; set; }      // ค่าบำรุงรักษารถ
        public decimal? CarMaintenanceLe { get; set; }    // ค่าบำรุงรักษารถ (LE)
        public decimal? CarMaintenanceNx { get; set; }    // ค่าบำรุงรักษารถ (Next Year)
        public decimal? CompCarsGas { get; set; }         // น้ำมันรถบริษัท
        public decimal? CompCarsGasLe { get; set; }       // น้ำมันรถบริษัท (LE)
        public decimal? CompCarsGasNx { get; set; }       // น้ำมันรถบริษัท (Next Year)
        public decimal? CarGasoline { get; set; }         // ค่าน้ำมันรถ
        public decimal? CarGasolineLe { get; set; }       // ค่าน้ำมันรถ (LE)
        public decimal? CarGasolineNx { get; set; }       // ค่าน้ำมันรถ (Next Year)
        public decimal? CarRepair { get; set; }           // ค่าซ่อมรถ
        public decimal? CarRepairLe { get; set; }         // ค่าซ่อมรถ (LE)
        public decimal? CarRepairNx { get; set; }         // ค่าซ่อมรถ (Next Year)

        // ===== BIGC Training & Development =====
        public decimal? TrainingCost { get; set; }        // ค่าฝึกอบรม
        public decimal? TrainingCostLe { get; set; }      // ค่าฝึกอบรม (LE)
        public decimal? TrainingCostNx { get; set; }      // ค่าฝึกอบรม (Next Year)
        public decimal? SeminarTraining { get; set; }     // สัมมนาฝึกอบรม
        public decimal? SeminarTrainingLe { get; set; }   // สัมมนาฝึกอบรม (LE)
        public decimal? SeminarTrainingNx { get; set; }   // สัมมนาฝึกอบรม (Next Year)

        // ===== BIGC Medical & Health =====
        public decimal? MedicalOutside { get; set; }      // ค่ารักษาพยาบาลนอกสถานที่
        public decimal? MedicalOutsideLe { get; set; }    // ค่ารักษาพยาบาลนอกสถานที่ (LE)
        public decimal? MedicalOutsideNx { get; set; }    // ค่ารักษาพยาบาลนอกสถานที่ (Next Year)
        public decimal? HealthCheckup { get; set; }       // ตรวจสุขภาพ
        public decimal? HealthCheckupLe { get; set; }     // ตรวจสุขภาพ (LE)
        public decimal? HealthCheckupNx { get; set; }     // ตรวจสุขภาพ (Next Year)

        // ===== BIGC Staff Activities & Equipment =====
        public decimal? StaffActivities { get; set; }     // กิจกรรมพนักงาน
        public decimal? StaffActivitiesLe { get; set; }   // กิจกรรมพนักงาน (LE)
        public decimal? StaffActivitiesNx { get; set; }   // กิจกรรมพนักงาน (Next Year)
        public decimal? Uniform { get; set; }             // เครื่องแบบ
        public decimal? UniformLe { get; set; }           // เครื่องแบบ (LE)
        public decimal? UniformNx { get; set; }           // เครื่องแบบ (Next Year)
        public decimal? LifeInsurance { get; set; }       // ประกันชีวิต
        public decimal? LifeInsuranceLe { get; set; }     // ประกันชีวิต (LE)
        public decimal? LifeInsuranceNx { get; set; }     // ประกันชีวิต (Next Year)

        // ===== BIGC Outsource & Contract =====
        public decimal? OutsourceWages { get; set; }      // ค่าจ้าง Outsource
        public decimal? OutsourceWagesLe { get; set; }    // ค่าจ้าง Outsource (LE)
        public decimal? OutsourceWagesNx { get; set; }    // ค่าจ้าง Outsource (Next Year)
        public decimal? ContractorFee { get; set; }       // ค่าจ้างผู้รับเหมา
        public decimal? ContractorFeeLe { get; set; }     // ค่าจ้างผู้รับเหมา (LE)
        public decimal? ContractorFeeNx { get; set; }     // ค่าจ้างผู้รับเหมา (Next Year)

        // ===== BIGC Accommodation & Travel =====
        public decimal? Accommodation { get; set; }       // ค่าที่พัก
        public decimal? AccommodationLe { get; set; }     // ค่าที่พัก (LE)
        public decimal? AccommodationNx { get; set; }     // ค่าที่พัก (Next Year)
        public decimal? TravelExpense { get; set; }       // ค่าเดินทาง
        public decimal? TravelExpenseLe { get; set; }     // ค่าเดินทาง (LE)
        public decimal? TravelExpenseNx { get; set; }     // ค่าเดินทาง (Next Year)
        public decimal? MealAllowance { get; set; }       // เบี้ยเลี้ยงอาหาร
        public decimal? MealAllowanceLe { get; set; }     // เบี้ยเลี้ยงอาหาร (LE)
        public decimal? MealAllowanceNx { get; set; }     // เบี้ยเลี้ยงอาหาร (Next Year)

        // ===== BIGC Tax & Others =====
        public decimal? OthersSubjectTax { get; set; }    // อื่นๆ ที่ต้องเสียภาษี
        public decimal? OthersSubjectTaxLe { get; set; }  // อื่นๆ ที่ต้องเสียภาษี (LE)
        public decimal? OthersSubjectTaxNx { get; set; }  // อื่นๆ ที่ต้องเสียภาษี (Next Year)
        public decimal? Other { get; set; }               // อื่นๆ
        public decimal? OtherLe { get; set; }             // อื่นๆ (LE)
        public decimal? OtherNx { get; set; }             // อื่นๆ (Next Year)

        // ===== BIGC Equipment & Tools =====
        public decimal? OfficeSupplies { get; set; }      // เครื่องเขียนสำนักงาน
        public decimal? OfficeSuppliesLe { get; set; }    // เครื่องเขียนสำนักงาน (LE)
        public decimal? OfficeSuppliesNx { get; set; }    // เครื่องเขียนสำนักงาน (Next Year)
        public decimal? ComputerEquipment { get; set; }   // อุปกรณ์คอมพิวเตอร์
        public decimal? ComputerEquipmentLe { get; set; } // อุปกรณ์คอมพิวเตอร์ (LE)
        public decimal? ComputerEquipmentNx { get; set; } // อุปกรณ์คอมพิวเตอร์ (Next Year)

        // ===== BIGC Communication =====
        public decimal? Communication { get; set; }       // ค่าสื่อสาร
        public decimal? CommunicationLe { get; set; }     // ค่าสื่อสาร (LE)
        public decimal? CommunicationNx { get; set; }     // ค่าสื่อสาร (Next Year)
        public decimal? InternetPhone { get; set; }       // อินเทอร์เน็ต/โทรศัพท์
        public decimal? InternetPhoneLe { get; set; }     // อินเทอร์เน็ต/โทรศัพท์ (LE)
        public decimal? InternetPhoneNx { get; set; }     // อินเทอร์เน็ต/โทรศัพท์ (Next Year)

        // ===== BIGC Additional Fields from Entity =====
        public string? OrgCode { get; set; }              // Organization Code
        public int? EstHcEoy { get; set; }                // Estimated Headcount End of Year
        public int? EstHcEoyLe { get; set; }              // Estimated Headcount EOY (LE)
        public int? BudgetHcEoy { get; set; }             // Budget Headcount End of Year
        public decimal? TotalCostEoy { get; set; }        // Total Cost End of Year
        public decimal? TotalCostEoyLe { get; set; }      // Total Cost EOY (LE)
    }
}
