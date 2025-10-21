
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
// TODO: Uncomment when JWT Authentication is ready
// using System.Security.Claims;
using HCBPCoreUI_Backend.Models;
using HCBPCoreUI_Backend.DTOs.Budget;
using HCBPCoreUI_Backend.Services;

namespace HCBPCoreUI_Backend.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class BudgetController : Controller
  {
    private readonly IBudgetService _budgetService;

    public BudgetController(IBudgetService budgetService)
    {
      _budgetService = budgetService;
    }

    // GET: /Budget/GetBudgets
    [HttpGet]
    [Route("B0Budgets")]
    public async Task<IActionResult> GetBudgets(
      [FromQuery] string? companyID,
      [FromQuery] string? coBu,
      [FromQuery] string? budgetYear,
      [FromQuery] string? costCenterCode,
      [FromQuery] string? divisionCode,
      [FromQuery] string? departmentCode,
      [FromQuery] string? sectionCode,
      [FromQuery] string? compStoreCode,
      [FromQuery] string? empStatus)
    {
      var filter = new BudgetFilterDto
      {
        CompanyID = companyID ?? "1",
        BudgetYear = budgetYear,
        Cobu = coBu,
        CostCenterCode = costCenterCode,
        Division = divisionCode,
        Department = departmentCode,
        Section = sectionCode,
        StoreName = compStoreCode,
        EmpStatus = empStatus
      };

      if (!filter.IsValid())
      {
        return BadRequest(filter.GetValidationError());
      }

      var list = await _budgetService.GetBudgetsAsync(filter);
      return Ok(list);
    }

    // GET: /Budget/GetDistinctCompanies
    [HttpGet]
    [Route("B0Companies")]
    public async Task<IActionResult> GetDistinctCompanies()
    {
      var distinctCompanies = await _budgetService.GetDistinctCompaniesAsync();
      return Ok(distinctCompanies);
    }

    // GET: /Budget/GetDistinctCoBU
    [HttpGet]
    [Route("B0CoBU")]
    public async Task<IActionResult> GetDistinctCoBU([FromQuery] string? companyID)
    {
      var distinctCoBU = await _budgetService.GetDistinctCoBUAsync(companyID);
      return Ok(distinctCoBU);
    }

    // GET: /Budget/GetDistinctPositions
    [HttpGet]
    [Route("B0Positions")]
    public async Task<IActionResult> GetDistinctPositions(
      [FromQuery] string? companyID,
      [FromQuery] string? coBu,
      [FromQuery] string? budgetYear,
      [FromQuery] string? costCenterCode,
      [FromQuery] string? divisionCode,
      [FromQuery] string? departmentCode,
      [FromQuery] string? sectionCode,
      [FromQuery] string? compStoreCode,
      [FromQuery] string? empStatus)
    {
      var filter = new BudgetFilterDto
      {
        CompanyID = companyID ?? "1",
        BudgetYear = budgetYear,
        Cobu = coBu,
        CostCenterCode = costCenterCode,
        Division = divisionCode,
        Department = departmentCode,
        Section = sectionCode,
        StoreName = compStoreCode,
        EmpStatus = empStatus
      };
      var distinctPositions = await _budgetService.GetDistinctPositionsAsync(filter);
      return Ok(distinctPositions);
    }

    // GET: /Budget/GetDistinctDivisions
    [HttpGet]
    [Route("B0Divisions")]
    public async Task<IActionResult> GetDistinctDivisions(
      [FromQuery] string? companyID,
      [FromQuery] string? coBu,
      [FromQuery] string? budgetYear,
      [FromQuery] string? costCenterCode)
    {
      var filter = new BudgetFilterDto
      {
        CompanyID = companyID ?? "1",
        BudgetYear = budgetYear,
        Cobu = coBu,
        CostCenterCode = costCenterCode
      };
      var distinctDivision = await _budgetService.GetDistinctDivisionsAsync(filter);
      return Ok(distinctDivision);
    }

    // GET: /Budget/GetDistinctDepartments
    [HttpGet]
    [Route("B0Departments")]
    public async Task<IActionResult> GetDistinctDepartments(
      [FromQuery] string? companyID,
      [FromQuery] string? coBu,
      [FromQuery] string? budgetYear,
      [FromQuery] string? costCenterCode,
      [FromQuery] string? divisionCode)
    {
      var filter = new BudgetFilterDto
      {
        CompanyID = companyID ?? "1",
        BudgetYear = budgetYear,
        Cobu = coBu,
        CostCenterCode = costCenterCode,
        Division = divisionCode
      };
      var distinctDepartments = await _budgetService.GetDistinctDepartmentsAsync(filter);
      return Ok(distinctDepartments);
    }

    // GET: /Budget/GetDistinctSections
    [HttpGet]
    [Route("B0Sections")]
    public async Task<IActionResult> GetDistinctSections(
      [FromQuery] string? companyID,
      [FromQuery] string? coBu,
      [FromQuery] string? budgetYear,
      [FromQuery] string? costCenterCode,
      [FromQuery] string? divisionCode,
      [FromQuery] string? departmentCode)
    {
      var filter = new BudgetFilterDto
      {
        CompanyID = companyID ?? "1",
        BudgetYear = budgetYear,
        Cobu = coBu,
        CostCenterCode = costCenterCode,
        Division = divisionCode,
        Department = departmentCode
      };
      var distinctSections = await _budgetService.GetDistinctSectionsAsync(filter);
      return Ok(distinctSections);
    }

    // GET: /Budget/GetDistinctStoreNames
    [HttpGet]
    [Route("B0StoreNames")]
    public async Task<IActionResult> GetDistinctStoreNames(
      [FromQuery] string? companyID,
      [FromQuery] string? cobu,
      [FromQuery] string? budgetYear,
      [FromQuery] string? costCenterCode,
      [FromQuery] string? divisionCode,
      [FromQuery] string? departmentCode,
      [FromQuery] string? sectionCode)
    {
      var filter = new BudgetFilterDto
      {
        CompanyID = companyID ?? "1",
        BudgetYear = budgetYear,
        Cobu = cobu,
        CostCenterCode = costCenterCode,
        Division = divisionCode,
        Department = departmentCode,
        Section = sectionCode
      };
      var distinctStoreNames = await _budgetService.GetDistinctStoreNamesAsync(filter);
      return Ok(distinctStoreNames);
    }

    // GET: /Budget/GetDistinctCostCenters
    [HttpGet]
    [Route("B0CostCenters")]
    public async Task<IActionResult> GetDistinctCostCenters(
      [FromQuery] string? companyID,
      [FromQuery] string? coBu,
      [FromQuery] string? budgetYear)
    {
      var filter = new BudgetFilterDto
      {
        CompanyID = companyID ?? "1",
        BudgetYear = budgetYear,
        Cobu = coBu
      };
      var distinctCostCenters = await _budgetService.GetDistinctCostCentersAsync(filter);
      return Ok(distinctCostCenters);
    }

    // GET: /Budget/GetDistinctBudgetYears
    [HttpGet]
    [Route("B0BudgetYears")]
    public async Task<IActionResult> GetDistinctBudgetYears(
      [FromQuery] string? companyID)
    {
      var distinctYears = await _budgetService.GetDistinctBudgetYearsAsync(companyID);
      return Ok(distinctYears);
    }

    // GET: /Budget/GetDistinctEmpStatus
    [HttpGet]
    [Route("B0EmpStatuses")]
    public async Task<IActionResult> GetDistinctEmpStatuses(
      [FromQuery] string? companyID,
      [FromQuery] string? coBu,
      [FromQuery] string? budgetYear,
      [FromQuery] string? costCenterCode,
      [FromQuery] string? divisionCode,
      [FromQuery] string? departmentCode,
      [FromQuery] string? sectionCode,
      [FromQuery] string? compStoreCode)
    {
      var filter = new BudgetFilterDto
      {
        CompanyID = companyID ?? "1",
        BudgetYear = budgetYear,
        Cobu = coBu,
        CostCenterCode = costCenterCode,
        Division = divisionCode,
        Department = departmentCode,
        Section = sectionCode,
        StoreName = compStoreCode
      };
      var distinctEmpStatuses = await _budgetService.GetDistinctEmpStatusesAsync(filter);
      return Ok(distinctEmpStatuses);
    }

    // GET: /Budget/GetDistinctJobBand
    [HttpGet]
    [Route("B0JobBands")]
    public async Task<IActionResult> GetDistinctJobBands(
      [FromQuery] string? companyID,
      [FromQuery] string? cobu,
      [FromQuery] string? budgetYear,
      [FromQuery] string? costCenterCode,
      [FromQuery] string? divisionCode,
      [FromQuery] string? departmentCode,
      [FromQuery] string? sectionCode,
      [FromQuery] string? compStoreCode,
      [FromQuery] string? empStatus,
      [FromQuery] string? positionCode)
    {
      var filter = new BudgetFilterDto
      {
        CompanyID = companyID ?? "1",
        BudgetYear = budgetYear,
        CostCenterCode = costCenterCode,
        Division = divisionCode,
        Department = departmentCode,
        Section = sectionCode,
        StoreName = compStoreCode,
        EmpStatus = empStatus,
        PositionCode = positionCode
      };
      var distinctJobBands = await _budgetService.GetDistinctJobBandsAsync(filter);
      return Ok(distinctJobBands);
    }

    // POST: /Budget/CreateBudget
    [HttpPost]
    [Route("CreateBudget")]
    public async Task<IActionResult> CreateBudget([FromBody] CreateBudgetRequest request)
    {
      try
      {
        if (request == null || request.Budget == null)
        {
          return BadRequest("Budget data is required.");
        }

        if (request.CompanyId <= 0)
        {
          return BadRequest("Valid Company ID is required.");
        }

        // Validate required fields based on company
        if (string.IsNullOrEmpty(request.Budget.EmpCode))
        {
          return BadRequest("Employee Code is required.");
        }

        if (string.IsNullOrEmpty(request.Budget.CostCenterCode))
        {
          return BadRequest("Cost Center Code is required.");
        }

        if (request.Budget.BudgetYear <= 0)
        {
          return BadRequest("Budget Year is required.");
        }

        // Set company-specific defaults
        request.Budget.CompanyId = request.CompanyId;
        request.Budget.UpdatedBy = "System"; // TODO: Get from authentication
        request.Budget.UpdatedDate = DateTime.Now;

        var createdBudget = await _budgetService.CreateBudgetAsync(request.Budget, request.CompanyId);

        return Ok(new {
          Success = true,
          Message = "Budget created successfully.",
          Data = createdBudget
        });
      }
      catch (ArgumentException ex)
      {
        return BadRequest(new {
          Success = false,
          Message = ex.Message
        });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new {
          Success = false,
          Message = "An error occurred while creating the budget.",
          Error = ex.Message
        });
      }
    }

    // PUT: /Budget/UpdateBudget/{id}
    [HttpPut]
    [Route("UpdateBudget/{id}")]
    public async Task<IActionResult> UpdateBudget(int id, [FromBody] UpdateBudgetRequest request)
    {
      try
      {
        if (request == null || request.Budget == null)
        {
          return BadRequest("Budget data is required.");
        }

        if (request.CompanyId <= 0)
        {
          return BadRequest("Valid Company ID is required.");
        }

        // Update audit fields
        request.Budget.UpdatedBy = "System"; // TODO: Get from authentication
        request.Budget.UpdatedDate = DateTime.Now;

        var updatedBudget = await _budgetService.UpdateBudgetAsync(id, request.Budget, request.CompanyId);

        if (updatedBudget == null)
        {
          return NotFound(new {
            Success = false,
            Message = "Budget not found."
          });
        }

        return Ok(new {
          Success = true,
          Message = "Budget updated successfully.",
          Data = updatedBudget
        });
      }
      catch (ArgumentException ex)
      {
        return BadRequest(new {
          Success = false,
          Message = ex.Message
        });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new {
          Success = false,
          Message = "An error occurred while updating the budget.",
          Error = ex.Message
        });
      }
    }

    // DELETE: /Budget/DeleteBudget/{id}
    [HttpDelete]
    [Route("DeleteBudget/{id}")]
    public async Task<IActionResult> DeleteBudget(int id, [FromQuery] int companyId)
    {
      try
      {
        if (companyId <= 0)
        {
          return BadRequest("Valid Company ID is required.");
        }

        var deleted = await _budgetService.DeleteBudgetAsync(id, companyId);

        if (!deleted)
        {
          return NotFound(new {
            Success = false,
            Message = "Budget not found."
          });
        }

        return Ok(new {
          Success = true,
          Message = "Budget deleted successfully."
        });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new {
          Success = false,
          Message = "An error occurred while deleting the budget.",
          Error = ex.Message
        });
      }
    }

    // GET: /Budget/GetBudget/{id}
    [HttpGet]
    [Route("GetBudget/{id}")]
    public async Task<IActionResult> GetBudget(int id, [FromQuery] int companyId)
    {
      try
      {
        if (companyId <= 0)
        {
          return BadRequest("Valid Company ID is required.");
        }

        var budget = await _budgetService.GetBudgetByIdAsync(id, companyId);

        if (budget == null)
        {
          return NotFound(new {
            Success = false,
            Message = "Budget not found."
          });
        }

        return Ok(budget);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new {
          Success = false,
          Message = "An error occurred while retrieving the budget.",
          Error = ex.Message
        });
      }
    }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 🎯 BATCH ENTRY SAVE - SA APPROVED IMPLEMENTATION
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    // SA Answers Applied:
    // - Q1: All or nothing (TransactionScope)
    // - Q2: ไม่จำกัดจำนวนแถว
    // - Q3: Reject duplicate
    // - Q4: JWT Token (Option 1 - Comment ไว้ก่อน)
    // - Q5: Modal + auto-close
    // - Q6: Pre-check duplicate
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

    /// <summary>
    /// POST: /api/Budget/B0SaveBatchEntry
    /// บันทึก Batch Budget Entries พร้อมกันหลายแถว
    ///
    /// Q4 Option 1: JWT Authentication (Comment ไว้ก่อน)
    /// - ตอนนี้ใช้ "DevUser" ชั่วคราวสำหรับ Development/Testing
    /// - เมื่อ JWT Auth เสร็จ: Uncomment [Authorize] และ User.Identity?.Name
    /// </summary>
    // TODO: Uncomment when JWT Authentication is ready
    // [Authorize]
    [HttpPost]
    [Route("B0SaveBatchEntry")]
    public async Task<IActionResult> SaveBatchEntry([FromBody] BatchBudgetRequest request)
    {
      try
      {
        // 🔍 DEBUG: Log incoming request
        Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        Console.WriteLine("🔍 SaveBatchEntry - Incoming Request");
        Console.WriteLine($"   Request is null: {request == null}");
        if (request != null)
        {
          Console.WriteLine($"   Budgets is null: {request.Budgets == null}");
          Console.WriteLine($"   Budgets count: {request.Budgets?.Count ?? 0}");
          Console.WriteLine($"   CreatedBy: '{request.CreatedBy}'");

          // 🆕 LOG ALL FIELDS ของแต่ละ Budget Row (Updated: 2025-10-20)
          if (request.Budgets != null && request.Budgets.Count > 0)
          {
            Console.WriteLine("\n📊 BUDGET DATA DETAILS:");
            for (int i = 0; i < request.Budgets.Count; i++)
            {
              var budget = request.Budgets[i];
              Console.WriteLine($"\n   ╔═══════════════════════════════════════════════════════════");
              Console.WriteLine($"   ║ ROW {i + 1} - BASIC FIELDS");
              Console.WriteLine($"   ╠═══════════════════════════════════════════════════════════");
              Console.WriteLine($"   ║ CompanyId: {budget.CompanyId}");
              Console.WriteLine($"   ║ CompanyCode: '{budget.CompanyCode}'");
              Console.WriteLine($"   ║ BudgetYear: {budget.BudgetYear}");
              Console.WriteLine($"   ║ BudgetYearLe: {budget.BudgetYearLe}");
              Console.WriteLine($"   ║ EmpCode: '{budget.EmpCode}'");
              Console.WriteLine($"   ║ EmpStatus: '{budget.EmpStatus}'");
              Console.WriteLine($"   ║ EmpTypeCode: '{budget.EmpTypeCode}'");
              Console.WriteLine($"   ║ ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
              Console.WriteLine($"   ║ CostCenterCode: '{budget.CostCenterCode}'");
              Console.WriteLine($"   ║ CostCenterName: '{budget.CostCenterName}'");
              Console.WriteLine($"   ║ Cobu: '{budget.Cobu}'");
              Console.WriteLine($"   ║ Bu: '{budget.Bu}'");
              Console.WriteLine($"   ║ Division: '{budget.Division}'");
              Console.WriteLine($"   ║ Department: '{budget.Department}'");
              Console.WriteLine($"   ║ Section: '{budget.Section}'");
              Console.WriteLine($"   ║ StoreName: '{budget.StoreName}'");
              Console.WriteLine($"   ║ ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
              Console.WriteLine($"   ║ PositionCode: '{budget.PositionCode}'");
              Console.WriteLine($"   ║ PositionName: '{budget.PositionName}'");
              Console.WriteLine($"   ║ JobBand: '{budget.JobBand}'");
              Console.WriteLine($"   ║ ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
              Console.WriteLine($"   ║ NewHcCode: '{budget.NewHcCode}'");
              Console.WriteLine($"   ║ NewVacPeriod: '{budget.NewVacPeriod}'");
              Console.WriteLine($"   ║ NewVacLe: '{budget.NewVacLe}'");
              Console.WriteLine($"   ║ LeOfMonth: {budget.LeOfMonth}");
              Console.WriteLine($"   ║ NoOfMonth: {budget.NoOfMonth}");
              Console.WriteLine($"   ║ ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
              Console.WriteLine($"   ║ CostCenterPlan: '{budget.CostCenterPlan}'");
              Console.WriteLine($"   ║ SalaryStructure: '{budget.SalaryStructure}'");
              Console.WriteLine($"   ║ RunrateCode: '{budget.RunrateCode}'");
              Console.WriteLine($"   ║ Reason: '{budget.Reason}'");
              Console.WriteLine($"   ║");
              Console.WriteLine($"   ╠═══════════════════════════════════════════════════════════");
              Console.WriteLine($"   ║ BENEFITS FIELDS - Salary Components");
              Console.WriteLine($"   ╠═══════════════════════════════════════════════════════════");
              Console.WriteLine($"   ║ SalWithEn: {budget.SalWithEn}");
              Console.WriteLine($"   ║ SalWithEnLe: {budget.SalWithEnLe}");
              Console.WriteLine($"   ║ SalNotEn: {budget.SalNotEn}");
              Console.WriteLine($"   ║ SalNotEnLe: {budget.SalNotEnLe}");
              Console.WriteLine($"   ║ WageStudent: {budget.WageStudent}");
              Console.WriteLine($"   ║ WageStudentLe: {budget.WageStudentLe}");
              Console.WriteLine($"   ║");
              Console.WriteLine($"   ╠═══════════════════════════════════════════════════════════");
              Console.WriteLine($"   ║ BENEFITS FIELDS - Bonus");
              Console.WriteLine($"   ╠═══════════════════════════════════════════════════════════");
              Console.WriteLine($"   ║ Bonus: {budget.Bonus}");
              Console.WriteLine($"   ║ BonusLe: {budget.BonusLe}");
              Console.WriteLine($"   ║ BonusType: '{budget.BonusType}' (BJC)");
              Console.WriteLine($"   ║ BonusTypeLe: '{budget.BonusTypeLe}' (BJC)");
              Console.WriteLine($"   ║ BonusTypes: '{budget.BonusTypes}' (BIGC)");
              Console.WriteLine($"   ║");
              Console.WriteLine($"   ╠═══════════════════════════════════════════════════════════");
              Console.WriteLine($"   ║ BENEFITS FIELDS - Performance Components");
              Console.WriteLine($"   ╠═══════════════════════════════════════════════════════════");
              Console.WriteLine($"   ║ SalesManagementPc: {budget.SalesManagementPc}");
              Console.WriteLine($"   ║ DiligenceAllowancePc: {budget.DiligenceAllowancePc}");
              Console.WriteLine($"   ║ PostAllowancePc: {budget.PostAllowancePc}");
              Console.WriteLine($"   ║ PhoneAllowancePc: {budget.PhoneAllowancePc}");
              Console.WriteLine($"   ║ TransportationPc: {budget.TransportationPc}");
              Console.WriteLine($"   ║");
              Console.WriteLine($"   ╠═══════════════════════════════════════════════════════════");
              Console.WriteLine($"   ║ BENEFITS FIELDS - Allowances & Benefits");
              Console.WriteLine($"   ╠═══════════════════════════════════════════════════════════");
              Console.WriteLine($"   ║ FleetCardPe: {budget.FleetCardPe}");
              Console.WriteLine($"   ║ FleetCardPeLe: {budget.FleetCardPeLe}");
              Console.WriteLine($"   ║ SkillPayAllowance: {budget.SkillPayAllowance}");
              Console.WriteLine($"   ║ SkillPayAllowanceLe: {budget.SkillPayAllowanceLe}");
              Console.WriteLine($"   ║ MedicalOutside: {budget.MedicalOutside}");
              Console.WriteLine($"   ║ MedicalOutsideLe: {budget.MedicalOutsideLe}");
              Console.WriteLine($"   ║ Uniform: {budget.Uniform}");
              Console.WriteLine($"   ║ UniformLe: {budget.UniformLe}");
              Console.WriteLine($"   ║ LifeInsurance: {budget.LifeInsurance}");
              Console.WriteLine($"   ║ LifeInsuranceLe: {budget.LifeInsuranceLe}");
              Console.WriteLine($"   ║");
              Console.WriteLine($"   ╠═══════════════════════════════════════════════════════════");
              Console.WriteLine($"   ║ BENEFITS FIELDS - Totals");
              Console.WriteLine($"   ╠═══════════════════════════════════════════════════════════");
              Console.WriteLine($"   ║ TotalPayroll: {budget.TotalPayroll}");
              Console.WriteLine($"   ║ TotalPayrollLe: {budget.TotalPayrollLe}");
              Console.WriteLine($"   ║ PeSbMth: {budget.PeSbMth}");
              Console.WriteLine($"   ║ PeSbMthLe: {budget.PeSbMthLe}");
              Console.WriteLine($"   ║ PeSbYear: {budget.PeSbYear}");
              Console.WriteLine($"   ║ PeSbYearLe: {budget.PeSbYearLe}");
              Console.WriteLine($"   ║");
              Console.WriteLine($"   ╠═══════════════════════════════════════════════════════════");
              Console.WriteLine($"   ║ 1-CHARACTER FIELDS");
              Console.WriteLine($"   ╠═══════════════════════════════════════════════════════════");
              Console.WriteLine($"   ║ JoinPvf: '{budget.JoinPvf}' (Length: {budget.JoinPvf?.Length ?? 0})");
              Console.WriteLine($"   ║ Pvf: '{budget.Pvf}' (Length: {budget.Pvf?.Length ?? 0})");
              Console.WriteLine($"   ║ FocusHc: '{budget.FocusHc}' (Length: {budget.FocusHc?.Length ?? 0})");
              Console.WriteLine($"   ║ FocusPe: '{budget.FocusPe}' (Length: {budget.FocusPe?.Length ?? 0})");
              Console.WriteLine($"   ║ Discount: '{budget.Discount}' (Length: {budget.Discount?.Length ?? 0})");
              Console.WriteLine($"   ║ Executive: '{budget.Executive}' (Length: {budget.Executive?.Length ?? 0})");
              Console.WriteLine($"   ║");
              Console.WriteLine($"   ╠═══════════════════════════════════════════════════════════");
              Console.WriteLine($"   ║ AUDIT FIELDS");
              Console.WriteLine($"   ╠═══════════════════════════════════════════════════════════");
              Console.WriteLine($"   ║ UpdatedBy: '{budget.UpdatedBy}'");
              Console.WriteLine($"   ║ UpdatedDate: {budget.UpdatedDate:yyyy-MM-dd HH:mm:ss}");
              Console.WriteLine($"   ╚═══════════════════════════════════════════════════════════");
            }
          }
        }
        Console.WriteLine($"   ModelState.IsValid: {ModelState.IsValid}");
        if (!ModelState.IsValid)
        {
          Console.WriteLine("   ModelState Errors:");
          foreach (var key in ModelState.Keys)
          {
            var errors = ModelState[key]?.Errors;
            if (errors != null && errors.Count > 0)
            {
              Console.WriteLine($"     [{key}]:");
              foreach (var error in errors)
              {
                Console.WriteLine($"       - {error.ErrorMessage}");
                if (!string.IsNullOrEmpty(error.Exception?.Message))
                {
                  Console.WriteLine($"         Exception: {error.Exception.Message}");
                }
              }
            }
          }
        }
        Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

        // ===== Validation =====
        if (!ModelState.IsValid)
        {
          var errors = ModelState.Values
              .SelectMany(v => v.Errors)
              .Select(e => e.ErrorMessage)
              .ToList();

          return BadRequest(new {
            Success = false,
            Message = "Invalid request data",
            Errors = errors,
            ModelStateKeys = ModelState.Keys.ToList() // เพิ่ม keys เพื่อดู property ที่มีปัญหา
          });
        }

        if (request == null || request.Budgets == null || request.Budgets.Count == 0)
        {
          return BadRequest(new {
            Success = false,
            Message = "Budgets list cannot be empty"
          });
        }

        // Q2: ไม่จำกัดจำนวนแถว (No batch size limit)
        // Note: สามารถเพิ่ม validation limit ได้ถ้า SA ต้องการ เช่น:
        // if (request.Budgets.Count > 100) {
        //   return BadRequest(new { Message = "Cannot exceed 100 rows per batch" });
        // }

        // ===== Q4: Authentication (Option 1 - Comment ไว้ก่อน) =====
        // TODO: Uncomment when JWT Authentication is ready
        // var currentUser = User.Identity?.Name ?? "System";
        // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Temporary for Development/Testing
        var currentUser = "DevUser"; // ← Remove when uncomment above

        // ===== Call Service =====
        var response = await _budgetService.CreateBatchBudgetsAsync(
            request.Budgets,
            currentUser
        );

        // ===== Return Response =====
        if (response.Success)
        {
          return Ok(response); // 200 OK with success details
        }
        else
        {
          // Q1: All or nothing - ถ้า error จะ rollback ทั้งหมด
          return BadRequest(response); // 400 Bad Request with error details
        }
      }
      catch (Exception ex)
      {
        // Log error
        Console.WriteLine($"Error in SaveBatchEntry: {ex.Message}");
        Console.WriteLine($"Stack Trace: {ex.StackTrace}");

        return StatusCode(500, new {
          Success = false,
          Message = "เกิดข้อผิดพลาดในการบันทึกข้อมูล",
          Error = ex.Message
        });
      }
    }
  }
}
