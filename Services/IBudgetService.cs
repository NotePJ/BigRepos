using System.Collections.Generic;
using System.Threading.Tasks;
using HCBPCoreUI_Backend.DTOs.Budget;
using HCBPCoreUI_Backend.DTOs.Employee;

namespace HCBPCoreUI_Backend.Services
{
  public interface IBudgetService
  {
    // ===== Company-Specific API Methods (Frontend ใช้งาน - Company-specific DTOs) =====
    Task<object> GetBudgetsAsync(BudgetFilterDto filter); // Returns List<BudgetBjcDto> or List<BudgetBigcDto> based on CompanyID
    Task<object?> GetBudgetByIdAsync(int budgetId, int companyId); // Returns BudgetBjcDto or BudgetBigcDto based on companyId
    Task<object> CreateBudgetAsync(object budget, int companyId); // Accepts BudgetBjcDto or BudgetBigcDto based on companyId
    Task<object?> UpdateBudgetAsync(int budgetId, object budget, int companyId); // Accepts BudgetBjcDto or BudgetBigcDto based on companyId
    Task<bool> DeleteBudgetAsync(int budgetId, int companyId);

    // ===== Legacy Unified API Methods (Backward compatibility - BudgetResponseDto) =====
    Task<List<BudgetResponseDto>> GetBudgetsUnifiedAsync(BudgetFilterDto filter);
    Task<BudgetResponseDto?> GetBudgetByIdUnifiedAsync(int budgetId, int companyId);
    Task<BudgetResponseDto> CreateBudgetUnifiedAsync(BudgetResponseDto budget, int companyId);
    Task<BudgetResponseDto?> UpdateBudgetUnifiedAsync(int budgetId, BudgetResponseDto budget, int companyId);
    Task<bool> DeleteBudgetUnifiedAsync(int budgetId, int companyId);

    // ===== Company-Specific Internal Methods (Strategy Pattern) =====
    Task<List<BudgetBjcDto>> GetBjcBudgetsAsync(BudgetFilterDto filter);
    Task<List<BudgetBigcDto>> GetBigcBudgetsAsync(BudgetFilterDto filter);
    Task<BudgetBjcDto?> GetBjcBudgetByIdAsync(int budgetId);
    Task<BudgetBigcDto?> GetBigcBudgetByIdAsync(int budgetId);
    Task<BudgetBjcDto> CreateBjcBudgetAsync(BudgetBjcDto budget);
    Task<BudgetBigcDto> CreateBigcBudgetAsync(BudgetBigcDto budget);
    Task<BudgetBjcDto?> UpdateBjcBudgetAsync(int budgetId, BudgetBjcDto budget);
    Task<BudgetBigcDto?> UpdateBigcBudgetAsync(int budgetId, BudgetBigcDto budget);
    Task<bool> DeleteBjcBudgetAsync(int budgetId);
    Task<bool> DeleteBigcBudgetAsync(int budgetId);

    // ===== Mapping Methods (Response ↔ Company-Specific DTOs) =====
    BudgetResponseDto MapToResponseDto(BudgetBjcDto bjcDto);
    BudgetResponseDto MapToResponseDto(BudgetBigcDto bigcDto);
    BudgetBjcDto MapToBjcDto(BudgetResponseDto responseDto);
    BudgetBigcDto MapToBigcDto(BudgetResponseDto responseDto);

    // ===== Pagination & Count =====
    Task<int> GetBudgetCountAsync(BudgetFilterDto filter);

    // ===== Dropdown/Filter Support Methods (รองรับ CompanyID logic) =====
    Task<List<object>> GetDistinctCompaniesAsync();
    Task<List<string?>> GetDistinctCoBUAsync(string? companyID);
    Task<List<object>> GetDistinctPositionsAsync(BudgetFilterDto filter);
    Task<List<string?>> GetDistinctDivisionsAsync(BudgetFilterDto filter);
    Task<List<string?>> GetDistinctDepartmentsAsync(BudgetFilterDto filter);
    Task<List<string?>> GetDistinctSectionsAsync(BudgetFilterDto filter);
    Task<List<string?>> GetDistinctStoreNamesAsync(BudgetFilterDto filter);
    Task<List<object>> GetDistinctCostCentersAsync(BudgetFilterDto filter);
    Task<List<int?>> GetDistinctBudgetYearsAsync(string? companyID);
    Task<List<string?>> GetDistinctEmpStatusesAsync(BudgetFilterDto filter);
    Task<List<string?>> GetDistinctJobBandsAsync(BudgetFilterDto filter);

    // ===== Additional Helper Methods =====
    Task<List<string?>> GetDistinctRunrateCodesAsync(string? companyID);
    Task<List<string?>> GetDistinctHrbpEmpCodesAsync(string? companyID);

    // ===== Summary/Statistics Methods =====
    Task<decimal> GetTotalPayrollAsync(BudgetFilterDto filter);
    Task<decimal> GetTotalBonusAsync(BudgetFilterDto filter);
    Task<int> GetEmployeeCountAsync(BudgetFilterDto filter);

    // ===== Employee Expenses (Existing from old interface) =====
    Task<List<EmployeeExpenseDto>> GetEmployeeExpensesAsync();

    // ===== Batch Entry Save (NEW - SA Approved Implementation) =====
    /// <summary>
    /// บันทึก Batch Budget Entries พร้อมกันหลายแถว
    ///
    /// SA Answers Applied:
    /// - Q1: All or nothing (TransactionScope) - ถ้า 1 แถว error จะ rollback ทั้งหมด
    /// - Q2: ไม่จำกัดจำนวนแถว
    /// - Q3: Reject duplicate
    /// - Q6: Pre-check duplicate ก่อน save
    /// </summary>
    /// <param name="budgets">รายการ Budget ที่ต้องการบันทึก</param>
    /// <param name="createdBy">ผู้ใช้ที่ทำการบันทึก (จาก JWT Token หรือ "DevUser" สำหรับ Dev)</param>
    /// <returns>BatchBudgetResponse พร้อมรายละเอียด success/failed</returns>
    Task<BatchBudgetResponse> CreateBatchBudgetsAsync(List<BudgetResponseDto> budgets, string createdBy);
  }
}
