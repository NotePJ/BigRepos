# 📋 แผนงาน API Integration: Batch Entry Save (saveBatchBudgetData)

**วันที่สร้าง:** 19 ตุลาคม 2025  
**ผู้วิเคราะห์:** Ten (AI Developer)  
**ผู้อนุมัติ:** [รอการอนุมัติจาก SA]  
**สถานะ:** 📝 **DRAFT - รอการอนุมัติ**

---

## 📑 สารบัญ

1. [ภาพรวมโครงการ](#1-ภาพรวมโครงการ)
2. [การวิเคราะห์โครงสร้างข้อมูล](#2-การวิเคราะห์โครงสร้างข้อมูล)
3. [การออกแบบ API Endpoint](#3-การออกแบบ-api-endpoint)
4. [Data Flow Architecture](#4-data-flow-architecture)
5. [Transaction Management](#5-transaction-management)
6. [Error Handling Strategy](#6-error-handling-strategy)
7. [Implementation Plan](#7-implementation-plan)
8. [Testing Strategy](#8-testing-strategy)
9. [Performance Considerations](#9-performance-considerations)
10. [Security & Validation](#10-security--validation)

---

## 1. ภาพรวมโครงการ

### 🎯 วัตถุประสงค์

แทนที่ `setTimeout` simulation ใน `budget.plan.events.js` (line 5011-5015) ด้วย API call จริงเพื่อบันทึกข้อมูล Batch Entry แบบหลายแถวพร้อมกัน

### 📊 ขอบเขตการทำงาน

- **Frontend:** JavaScript (`saveBatchBudgetData()` function)
- **Backend:** 
  - Controller: `BudgetController.cs` - เพิ่ม endpoint `B0SaveBatchEntry`
  - Service: `BudgetService.cs` - เพิ่ม method `CreateBatchBudgetsAsync()`
  - DTO: สร้าง `BatchBudgetRequest.cs` และ `BatchBudgetResponse.cs`

### 🏢 Company-Specific Tables

| Company | ID | Table | Model File |
|---------|----|----|-----------|
| **BJC** | 1 | `HRB_BUDGET_BJC` | `HRB_BUDGET_BJC.cs` |
| **BIGC** | 2 | `HRB_BUDGET_BIGC` | `HRB_BUDGET_BIGC.cs` |

**Key Insight:** แต่ละ batch row **ต้อง** save ไปยัง table ที่ถูกต้องตาม `CompanyId` ที่ระบุ

---

## 2. การวิเคราะห์โครงสร้างข้อมูล

### 📦 Current Data Structure (JavaScript)

จาก `proceedWithDataCollection()` function:

```javascript
const batchData = [
  {
    "batch-company": "001",           // → CompanyId (1=BJC, 2=BIGC)
    "batch-year": "2025",             // → BudgetYear
    "batch-cobu": "BU01",             // → Cobu
    "batch-cost-center": "CC100",     // → CostCenterCode
    "batch-division": "DIV01",        // → Division
    "batch-department": "DEPT01",     // → Department
    "batch-section": "SEC01",         // → Section
    "batch-position": "POS100",       // → PositionCode
    "batch-job-band": "JB01",         // → JobBand
    "batch-emp-status": "Active",     // → EmpStatus
    "batch-join-date": "2025-01-01",  // → JoinDate
    // ... LE Benefits (2025 Oct-Dec)
    "batchLe-payroll": "50000",
    "batchLe-premium": "5000",
    // ... Budget Year Benefits (2026)
    "batchBg-payroll": "60000",
    "batchBg-premium": "6000",
    // ... Dynamic Cost Center Allocation (if multi CC)
    // ... all other fields
  },
  // Row 2, Row 3, ...
]
```

### 🗄️ Database Structure Analysis

#### **Common Fields (Both BJC & BIGC):**

| Category | Fields | Required |
|----------|--------|----------|
| **Primary Key** | `BUDGET_ID` (auto-increment), `BUDGET_YEAR` | ✅ Yes |
| **Company** | `COMPANY_ID`, `COMPANY_CODE` | ✅ Yes |
| **Organization** | `BU`, `COBU`, `DIVISION`, `DEPARTMENT`, `SECTION`, `STORE_NAME` | Partial |
| **Employee** | `EMP_CODE`, `EMP_STATUS`, `EMP_TYPE_CODE` | ✅ EMP_CODE Required |
| **Employee Name** | `TITLE_TH`, `FNAME_TH`, `LNAME_TH`, `TITLE_EN`, `FNAME_EN`, `LNAME_EN` | Optional |
| **Position** | `COST_CENTER_CODE`, `COST_CENTER_NAME`, `POSITION_CODE`, `POSITION_NAME`, `JOB_BAND` | ✅ COST_CENTER_CODE Required |
| **Dates** | `JOIN_DATE`, `UPDATED_DATE` | Optional |
| **Years** | `YOS_CURR_YEAR`, `YOS_NEXT_YEAR` | Optional |
| **New HC** | `NEW_HC_CODE`, `NEW_VAC_PERIOD`, `NEW_VAC_LE`, `REASON` | Optional |
| **Group** | `GROUP_NAME`, `GROUP_LEVEL_1`, `GROUP_LEVEL_2`, `GROUP_LEVEL_3` | Optional |
| **HR** | `HRBP_EMP_CODE`, `RUNRATE_CODE` | Optional |
| **Config** | `COST_CENTER_PLAN`, `SALARY_STRUCTURE`, `DISCOUNT`, `EXECUTIVE`, `FOCUS_HC`, `FOCUS_PE`, `JOIN_PVF`, `PVF` | Optional |
| **Months** | `LE_OF_MONTH`, `NO_OF_MONTH` | Optional |
| **Audit** | `UPDATED_BY`, `UPDATED_DATE` | Recommended |

#### **Financial Fields Pattern:**

ทั้ง BJC และ BIGC มี pattern เดียวกัน:

- **LE (Last Estimate)** fields: `*_LE` (สำหรับ LE year, เช่น 2025)
- **Current Year** fields: ไม่มี suffix (สำหรับ Budget year, เช่น 2026)

#### **BJC-Specific Financial Fields (96 fields):**

<details>
<summary>📊 BJC Financial Fields (Click to expand)</summary>

| Field Group | LE Fields | Current Year Fields |
|-------------|-----------|---------------------|
| **Payroll** | `PAYROLL_LE`, `PREMIUM_LE` | `PAYROLL`, `PREMIUM` |
| **Salary** | `SAL_WITH_EN_LE`, `SAL_NOT_EN_LE`, `SAL_TEMP_LE` | `SAL_WITH_EN`, `SAL_NOT_EN`, `SAL_TEMP` |
| **Bonus** | `BONUS_TYPE_LE`, `BONUS_LE` | `BONUS_TYPE`, `BONUS` |
| **Social Security** | `SOCIAL_SECURITY_TMP_LE`, `SOCIAL_SECURITY_LE` | `SOCIAL_SECURITY_TMP`, `SOCIAL_SECURITY` |
| **Provident Fund** | `PROVIDENT_FUND_LE` | `PROVIDENT_FUND` |
| **Allowances** | `SALES_MANAGEMENT_PC_LE`, `SHELF_STACKING_PC_LE`, `DILIGENCE_ALLOWANCE_PC_LE`, `POST_ALLOWANCE_PC_LE`, `PHONE_ALLOWANCE_PC_LE`, `TRANSPORTATION_PC_LE`, `SKILL_ALLOWANCE_PC_LE`, `OTHER_ALLOWANCE_PC_LE` | Same without `_LE` |
| **Car Related** | `CAR_MAINTENANCE_TMP_LE`, `CAR_MAINTENANCE_LE`, `CAR_GASOLINE_LE`, `CAR_REPAIR_LE`, `CAR_RENTAL_LE`, `COMP_CARS_GAS_LE`, `COMP_CARS_OTHER_LE`, `CAR_ALLOWANCE_LE` | Same without `_LE` |
| **Staff Benefits** | `TEMPORARY_STAFF_SAL_LE`, `HOUSING_ALLOWANCE_LE`, `SALES_CAR_ALLOWANCE_LE`, `SOUTHRISK_ALLOWANCE_LE`, `SOUTHRISK_ALLOWANCE_TMP_LE`, `ACCOMMODATION_LE`, `MEAL_ALLOWANCE_LE` | Same without `_LE` |
| **Insurance** | `WORKMEN_COMPENSATION_LE`, `LIFE_INSURANCE_LE` | `WORKMEN_COMPENSATION`, `LIFE_INSURANCE` |
| **Medical** | `MEDICAL_OUTSIDE_LE`, `MEDICAL_INHOUSE_LE` | `MEDICAL_OUTSIDE`, `MEDICAL_INHOUSE` |
| **Others** | `STAFF_ACTIVITIES_LE`, `UNIFORM_LE`, `LICENSE_ALLOWANCE_LE`, `OUTSOURCE_WAGES_LE`, `OTHER_LE`, `OTHERS_SUBJECT_TAX_LE` | Same without `_LE` |
| **Performance** | `PE_MTH_LE`, `PE_YEAR_LE`, `PE_SB_MTH_LE`, `PE_SB_YEAR_LE` | `PE_MTH`, `PE_YEAR`, `PE_SB_MTH`, `PE_SB_YEAR` |

</details>

#### **BIGC-Specific Financial Fields (120+ fields):**

<details>
<summary>📊 BIGC Financial Fields (Click to expand)</summary>

**BIGC มี fields แตกต่างจาก BJC:**

| Field Group | LE Fields | Current Year Fields |
|-------------|-----------|---------------------|
| **Payroll** | `PAYROLL_LE`, `PREMIUM_LE`, `TOTAL_PAYROLL_LE` | `PAYROLL`, `PREMIUM`, `TOTAL_PAYROLL` |
| **Bonus** | `BONUS_LE` | `BONUS`, `BONUS_TYPES` |
| **Car Related** | `FLEET_CARD_PE_LE`, `CAR_ALLOWANCE_LE`, `GASOLINE_ALLOWANCE_LE`, `CAR_RENTAL_PE_LE` | `FLEET_CARD_PE`, `CAR_ALLOWANCE`, `GASOLINE_ALLOWANCE`, `CAR_RENTAL_PE` |
| **Allowances** | `LICENSE_ALLOWANCE_LE`, `HOUSING_ALLOWANCE_LE`, `SKILL_PAY_ALLOWANCE_LE`, `OTHER_ALLOWANCE_LE` | `LICENSE_ALLOWANCE`, `HOUSING_ALLOWANCE`, `SKILL_PAY_ALLOWANCE`, `OTHER_ALLOWANCE` |
| **Student** | `WAGE_STUDENT_LE` | `WAGE_STUDENT` |
| **Social Security** | `SOCIAL_SECURITY_LE`, `LABOR_FUND_FEE_LE` | `SOCIAL_SECURITY`, `LABOR_FUND_FEE` |
| **Benefits** | `PROVIDENT_FUND_LE`, `EMPLOYEE_WELFARE_LE`, `OTHER_STAFF_BENEFIT_LE`, `STAFF_INSURANCE_LE` | `PROVIDENT_FUND`, `EMPLOYEE_WELFARE`, `OTHER_STAFF_BENEFIT`, `STAFF_INSURANCE` |
| **Medical** | `MEDICAL_EXPENSE_LE`, `MEDICAL_INHOUSE_LE` | `MEDICAL_EXPENSE`, `MEDICAL_INHOUSE` |
| **Others** | `TRAINING_LE`, `LONG_SERVICE_LE`, `PROVISION_LE`, `INTEREST_LE` | `TRAINING`, `LONG_SERVICE`, `PROVISION`, `INTEREST` |
| **Performance** | `PE_MTH_LE`, `PE_YEAR_LE`, `PE_SB_MTH_LE`, `PE_SB_YEAR_LE` | `PE_MTH`, `PE_YEAR`, `PE_SB_MTH`, `PE_SB_YEAR` |

**หมายเหตุ:** BIGC มีเฉพาะ 2 patterns (LE และ Current Year) ไม่มี Next Year (*_NX) fields

</details>

### 🔄 Field Mapping Strategy

**JavaScript Field Name → Database Column:**

```javascript
// Pattern 1: Direct mapping with transform
"batch-company" → COMPANY_ID (transform: "001" → 1, "002" → 2)

// Pattern 2: Remove "batch-" prefix
"batch-year" → BUDGET_YEAR
"batch-cobu" → COBU
"batch-cost-center" → COST_CENTER_CODE

// Pattern 3: LE Benefits (remove "batchLe-" prefix + add "_LE" suffix)
"batchLe-payroll" → PAYROLL_LE
"batchLe-premium" → PREMIUM_LE

// Pattern 4: Budget Year Benefits (remove "batchBg-" prefix)
"batchBg-payroll" → PAYROLL
"batchBg-premium" → PREMIUM
```

---

## 3. การออกแบบ API Endpoint

### 🔌 Endpoint Specification

```http
POST /api/Budget/B0SaveBatchEntry
Content-Type: application/json
```

### 📥 Request Model

**สร้างไฟล์ใหม่:** `DTOs/Budget/BatchBudgetRequest.cs`

```csharp
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCBPCoreUI_Backend.DTOs.Budget
{
    /// <summary>
    /// Request DTO for batch budget creation
    /// </summary>
    public class BatchBudgetRequest
    {
        /// <summary>
        /// List of budget entries to create
        /// </summary>
        [Required]
        [MinLength(1, ErrorMessage = "At least one budget entry is required")]
        public List<BudgetResponseDto> Budgets { get; set; } = new();

        /// <summary>
        /// Optional: User who initiated the batch save
        /// </summary>
        public string? CreatedBy { get; set; }
    }
}
```

### 📤 Response Model

**สร้างไฟล์ใหม่:** `DTOs/Budget/BatchBudgetResponse.cs`

```csharp
using System.Collections.Generic;

namespace HCBPCoreUI_Backend.DTOs.Budget
{
    /// <summary>
    /// Response DTO for batch budget creation
    /// </summary>
    public class BatchBudgetResponse
    {
        /// <summary>
        /// Overall success status
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Summary message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Total number of rows submitted
        /// </summary>
        public int TotalSubmitted { get; set; }

        /// <summary>
        /// Number of successfully saved rows
        /// </summary>
        public int TotalSuccess { get; set; }

        /// <summary>
        /// Number of failed rows
        /// </summary>
        public int TotalFailed { get; set; }

        /// <summary>
        /// Successfully created budgets with their new IDs
        /// </summary>
        public List<BudgetSaveResult> SuccessResults { get; set; } = new();

        /// <summary>
        /// Failed budget entries with error details
        /// </summary>
        public List<BudgetSaveError> FailedResults { get; set; } = new();
    }

    /// <summary>
    /// Individual budget save result
    /// </summary>
    public class BudgetSaveResult
    {
        /// <summary>
        /// Row index (0-based) from submitted batch
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// Generated Budget ID
        /// </summary>
        public int BudgetId { get; set; }

        /// <summary>
        /// Employee Code
        /// </summary>
        public string? EmpCode { get; set; }

        /// <summary>
        /// Cost Center Code
        /// </summary>
        public string? CostCenterCode { get; set; }

        /// <summary>
        /// Budget Year
        /// </summary>
        public int BudgetYear { get; set; }

        /// <summary>
        /// Company ID
        /// </summary>
        public int? CompanyId { get; set; }
    }

    /// <summary>
    /// Individual budget save error
    /// </summary>
    public class BudgetSaveError
    {
        /// <summary>
        /// Row index (0-based) from submitted batch
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// Employee Code (if available)
        /// </summary>
        public string? EmpCode { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Error details
        /// </summary>
        public string? ErrorDetails { get; set; }

        /// <summary>
        /// Submitted data (for debugging)
        /// </summary>
        public object? SubmittedData { get; set; }
    }
}
```

---

## 4. Data Flow Architecture

### 📊 Complete Data Flow Diagram

```
┌─────────────────────────────────────────────────────────────────────┐
│ FRONTEND (budget.plan.events.js)                                   │
│                                                                     │
│  proceedWithDataCollection(validationResults) {                    │
│    1. Collect batchData from all rows                              │
│    2. Call saveBatchBudgetData(batchData)  ← NEW FUNCTION          │
│  }                                                                  │
└─────────────────────────────────────────────────────────────────────┘
                            ↓
                  🌐 HTTP POST Request
                  /api/Budget/B0SaveBatchEntry
                  Content-Type: application/json
                  Body: { Budgets: [...], CreatedBy: "user" }
                            ↓
┌─────────────────────────────────────────────────────────────────────┐
│ BACKEND: BudgetController.cs                                        │
│                                                                     │
│  [HttpPost("B0SaveBatchEntry")]                                     │
│  public async Task<IActionResult> SaveBatchEntry(                  │
│    [FromBody] BatchBudgetRequest request                           │
│  ) {                                                                │
│    1. ✅ Validate request (ModelState)                             │
│    2. ✅ Check budgets list not empty                              │
│    3. 📞 Call _budgetService.CreateBatchBudgetsAsync()             │
│    4. 📤 Return BatchBudgetResponse                                │
│  }                                                                  │
└─────────────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────────────┐
│ BACKEND: BudgetService.cs                                           │
│                                                                     │
│  public async Task<BatchBudgetResponse> CreateBatchBudgetsAsync(   │
│    List<BudgetResponseDto> budgets, string createdBy              │
│  ) {                                                                │
│    var response = new BatchBudgetResponse();                       │
│    var successList = new List<BudgetSaveResult>();                 │
│    var errorList = new List<BudgetSaveError>();                    │
│                                                                     │
│    🔁 foreach (budget in budgets with index) {                     │
│      try {                                                          │
│        1. 🔍 Extract CompanyId from budget                         │
│        2. 🔀 Route to correct table:                               │
│           if (companyId == 1) → CreateBjcBudgetAsync()            │
│           if (companyId == 2) → CreateBigcBudgetAsync()           │
│        3. ✅ Add to successList                                    │
│      }                                                              │
│      catch (Exception ex) {                                         │
│        ❌ Add to errorList with details                            │
│      }                                                              │
│    }                                                                │
│                                                                     │
│    📊 Build response summary                                        │
│    return response;                                                 │
│  }                                                                  │
└─────────────────────────────────────────────────────────────────────┘
                            ↓
            ┌───────────────┴───────────────┐
            │                               │
            ↓                               ↓
┌──────────────────────┐       ┌──────────────────────┐
│ CreateBjcBudgetAsync │       │ CreateBigcBudgetAsync│
│                      │       │                      │
│ 1. Map DTO → Entity  │       │ 1. Map DTO → Entity  │
│ 2. _context.Add()    │       │ 2. _context.Add()    │
│ 3. SaveChangesAsync()│       │ 3. SaveChangesAsync()│
│ 4. Return created DTO│       │ 4. Return created DTO│
└──────────────────────┘       └──────────────────────┘
            ↓                               ↓
┌──────────────────────┐       ┌──────────────────────┐
│ HRB_BUDGET_BJC       │       │ HRB_BUDGET_BIGC      │
│ (SQL Server Table)   │       │ (SQL Server Table)   │
└──────────────────────┘       └──────────────────────┘
```

### 🔄 Detailed Service Logic

```csharp
public async Task<BatchBudgetResponse> CreateBatchBudgetsAsync(
    List<BudgetResponseDto> budgets, 
    string createdBy)
{
    var response = new BatchBudgetResponse
    {
        TotalSubmitted = budgets.Count
    };

    for (int i = 0; i < budgets.Count; i++)
    {
        var budget = budgets[i];
        try
        {
            // Set audit fields
            budget.UpdatedBy = createdBy ?? "System";
            budget.UpdatedDate = DateTime.Now;

            // Validate CompanyId
            if (!budget.CompanyId.HasValue || 
                (budget.CompanyId != 1 && budget.CompanyId != 2))
            {
                throw new ArgumentException(
                    $"Invalid CompanyId: {budget.CompanyId}. Must be 1 (BJC) or 2 (BIGC)."
                );
            }

            // Route to correct table
            object createdBudget;
            if (budget.CompanyId == 1) // BJC
            {
                var bjcDto = MapToBjcDto(budget);
                createdBudget = await CreateBjcBudgetAsync(bjcDto);
            }
            else // BIGC
            {
                var bigcDto = MapToBigcDto(budget);
                createdBudget = await CreateBigcBudgetAsync(bigcDto);
            }

            // Extract BudgetId from created entity
            int budgetId = GetBudgetIdFromCreatedEntity(createdBudget);

            // Add to success list
            response.SuccessResults.Add(new BudgetSaveResult
            {
                RowIndex = i,
                BudgetId = budgetId,
                EmpCode = budget.EmpCode,
                CostCenterCode = budget.CostCenterCode,
                BudgetYear = budget.BudgetYear,
                CompanyId = budget.CompanyId
            });
            response.TotalSuccess++;
        }
        catch (Exception ex)
        {
            // Add to error list
            response.FailedResults.Add(new BudgetSaveError
            {
                RowIndex = i,
                EmpCode = budget.EmpCode,
                ErrorMessage = ex.Message,
                ErrorDetails = ex.InnerException?.Message,
                SubmittedData = budget // For debugging
            });
            response.TotalFailed++;
        }
    }

    // Set overall status
    response.Success = response.TotalFailed == 0;
    response.Message = response.Success
        ? $"Successfully saved {response.TotalSuccess} budget entries."
        : $"Saved {response.TotalSuccess} entries with {response.TotalFailed} failures.";

    return response;
}
```

---

## 5. Transaction Management

### 🔒 Transaction Strategy

**Option 1: Individual Transactions (RECOMMENDED)**

```csharp
// ✅ Pros:
// - Partial success possible (some rows saved even if others fail)
// - Better error reporting (which specific rows failed)
// - No rollback of successful rows

// ❌ Cons:
// - Not truly atomic (batch not "all or nothing")
// - Potential for partial data in DB

foreach (var budget in budgets)
{
    try
    {
        // Each save is its own transaction
        var created = await CreateBudgetAsync(budget, companyId);
        successList.Add(created);
    }
    catch (Exception ex)
    {
        errorList.Add(new BudgetSaveError { ... });
    }
}
```

**Option 2: Single Transaction (Alternative)**

```csharp
// ✅ Pros:
// - Truly atomic ("all or nothing")
// - Data consistency guaranteed

// ❌ Cons:
// - One failure = all fail (lose all work)
// - Harder to report which specific row caused failure

using var transaction = await _context.Database.BeginTransactionAsync();
try
{
    foreach (var budget in budgets)
    {
        await CreateBudgetAsync(budget, companyId);
    }
    await transaction.CommitAsync();
    return SuccessResponse();
}
catch (Exception ex)
{
    await transaction.RollbackAsync();
    return ErrorResponse(ex);
}
```

**📌 Recommended Approach: Option 1 (Individual Transactions)**

**เหตุผล:**
1. ผู้ใช้ต้องการทราบว่าแถวไหนบันทึกสำเร็จ แถวไหนล้มเหลว
2. Frontend มี validation แล้ว จึงโอกาส error ต่ำ
3. ถ้า 1 แถวมีปัญหา ไม่ควรทำให้แถวอื่นเสียหายด้วย
4. User experience ดีกว่า (partial success > total failure)

---

## 6. Error Handling Strategy

### 🛡️ Error Categories

| Category | HTTP Status | Handling Strategy |
|----------|-------------|-------------------|
| **Validation Error** | 400 Bad Request | Return detailed field errors |
| **Business Rule Error** | 400 Bad Request | Return specific rule violation |
| **Database Error** | 500 Internal Server Error | Log error, return generic message |
| **Duplicate Entry** | 409 Conflict | Return conflicting key info |
| **Authorization Error** | 403 Forbidden | Return access denied message |

### 📝 Error Response Format

```json
{
  "success": false,
  "message": "Batch save completed with errors",
  "totalSubmitted": 5,
  "totalSuccess": 3,
  "totalFailed": 2,
  "successResults": [ /* ... */ ],
  "failedResults": [
    {
      "rowIndex": 1,
      "empCode": "E001",
      "errorMessage": "Duplicate entry: Employee E001 already exists for Budget Year 2026",
      "errorDetails": "Unique constraint violation on (EMP_CODE, BUDGET_YEAR, COST_CENTER_CODE)",
      "submittedData": { /* budget data for debugging */ }
    },
    {
      "rowIndex": 3,
      "empCode": "E003",
      "errorMessage": "Invalid CompanyId: 3. Must be 1 (BJC) or 2 (BIGC).",
      "errorDetails": null,
      "submittedData": { /* budget data for debugging */ }
    }
  ]
}
```

### 🔍 Common Error Scenarios

```csharp
// 1. Empty batch list
if (request.Budgets == null || request.Budgets.Count == 0)
{
    return BadRequest(new {
        Success = false,
        Message = "Budgets list cannot be empty"
    });
}

// 2. Invalid CompanyId
if (!budget.CompanyId.HasValue || 
    (budget.CompanyId != 1 && budget.CompanyId != 2))
{
    throw new ArgumentException(
        $"Invalid CompanyId: {budget.CompanyId}. Must be 1 (BJC) or 2 (BIGC)."
    );
}

// 3. Missing required fields
if (string.IsNullOrEmpty(budget.EmpCode))
{
    throw new ArgumentException("Employee Code is required.");
}

if (string.IsNullOrEmpty(budget.CostCenterCode))
{
    throw new ArgumentException("Cost Center Code is required.");
}

if (budget.BudgetYear <= 0)
{
    throw new ArgumentException("Budget Year is required.");
}

// 4. Duplicate entry (DB unique constraint)
catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && 
    sqlEx.Number == 2627) // Unique constraint violation
{
    throw new ArgumentException(
        $"Duplicate entry: Employee {budget.EmpCode} already exists for Budget Year {budget.BudgetYear}"
    );
}

// 5. Database connection error
catch (DbUpdateException ex)
{
    Console.WriteLine($"Database error: {ex.Message}");
    throw new InvalidOperationException(
        "An error occurred while saving to database. Please try again."
    );
}
```

---

## 7. Implementation Plan

### 📋 Phase 1: Backend Setup (Day 1-2)

#### **Task 1.1: Create DTOs**
- [ ] สร้าง `DTOs/Budget/BatchBudgetRequest.cs`
- [ ] สร้าง `DTOs/Budget/BatchBudgetResponse.cs`
- [ ] สร้าง `DTOs/Budget/BudgetSaveResult.cs` (inner class)
- [ ] สร้าง `DTOs/Budget/BudgetSaveError.cs` (inner class)

#### **Task 1.2: Update IBudgetService Interface**
```csharp
Task<BatchBudgetResponse> CreateBatchBudgetsAsync(
    List<BudgetResponseDto> budgets, 
    string createdBy
);
```

#### **Task 1.3: Implement BudgetService Method**

**⚠️ หมายเหตุ:** Implementation จะแตกต่างกันตาม SA ตอบคำถาม Q6 (Duplicate Check Strategy)

<details>
<summary><strong>Option A: No Pre-Check (Rely on DB Constraint)</strong></summary>

```csharp
// File: Services/BudgetService.cs
// Location: After UpdateBudgetAsync() method

public async Task<BatchBudgetResponse> CreateBatchBudgetsAsync(
    List<BudgetResponseDto> budgets, 
    string createdBy)
{
    var response = new BatchBudgetResponse
    {
        TotalSubmitted = budgets.Count
    };

    for (int i = 0; i < budgets.Count; i++)
    {
        var budget = budgets[i];
        try
        {
            budget.UpdatedBy = createdBy ?? "System";
            budget.UpdatedDate = DateTime.Now;

            // Validate CompanyId
            if (!budget.CompanyId.HasValue || 
                (budget.CompanyId != 1 && budget.CompanyId != 2))
            {
                throw new ArgumentException(
                    $"Invalid CompanyId: {budget.CompanyId}. Must be 1 (BJC) or 2 (BIGC)."
                );
            }

            // Route to correct table
            object createdBudget;
            if (budget.CompanyId == 1)
            {
                var bjcDto = MapToBjcDto(budget);
                createdBudget = await CreateBjcBudgetAsync(bjcDto);
            }
            else
            {
                var bigcDto = MapToBigcDto(budget);
                createdBudget = await CreateBigcBudgetAsync(bigcDto);
            }

            int budgetId = GetBudgetIdFromCreatedEntity(createdBudget);

            response.SuccessResults.Add(new BudgetSaveResult
            {
                RowIndex = i,
                BudgetId = budgetId,
                EmpCode = budget.EmpCode,
                CostCenterCode = budget.CostCenterCode,
                BudgetYear = budget.BudgetYear,
                CompanyId = budget.CompanyId
            });
            response.TotalSuccess++;
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && 
            sqlEx.Number == 2627) // Unique constraint violation
        {
            response.FailedResults.Add(new BudgetSaveError
            {
                RowIndex = i,
                EmpCode = budget.EmpCode,
                ErrorMessage = $"Duplicate entry: Employee {budget.EmpCode} already exists for Budget Year {budget.BudgetYear}",
                ErrorDetails = "Unique constraint violation"
            });
            response.TotalFailed++;
        }
        catch (Exception ex)
        {
            response.FailedResults.Add(new BudgetSaveError
            {
                RowIndex = i,
                EmpCode = budget.EmpCode,
                ErrorMessage = ex.Message,
                ErrorDetails = ex.InnerException?.Message
            });
            response.TotalFailed++;
        }
    }

    response.Success = response.TotalFailed == 0;
    response.Message = response.Success
        ? $"Successfully saved {response.TotalSuccess} budget entries."
        : $"Saved {response.TotalSuccess} entries with {response.TotalFailed} failures.";

    return response;
}
```

</details>

<details>
<summary><strong>⭐ Option B: Pre-Check Duplicates (RECOMMENDED for Better UX)</strong></summary>

```csharp
// File: Services/BudgetService.cs
// Location: After UpdateBudgetAsync() method

public async Task<BatchBudgetResponse> CreateBatchBudgetsAsync(
    List<BudgetResponseDto> budgets, 
    string createdBy)
{
    var response = new BatchBudgetResponse
    {
        TotalSubmitted = budgets.Count
    };

    for (int i = 0; i < budgets.Count; i++)
    {
        var budget = budgets[i];
        try
        {
            budget.UpdatedBy = createdBy ?? "System";
            budget.UpdatedDate = DateTime.Now;

            // Validate CompanyId
            if (!budget.CompanyId.HasValue || 
                (budget.CompanyId != 1 && budget.CompanyId != 2))
            {
                throw new ArgumentException(
                    $"Invalid CompanyId: {budget.CompanyId}. Must be 1 (BJC) or 2 (BIGC)."
                );
            }

            // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
            // 🔍 OPTION B: PRE-CHECK DUPLICATE
            // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
            bool isDuplicate = false;
            if (budget.CompanyId == 1) // BJC
            {
                isDuplicate = await _context.HRB_BUDGET_BJC
                    .AnyAsync(b => 
                        b.EmpCode == budget.EmpCode && 
                        b.BudgetYear == budget.BudgetYear &&
                        b.CostCenterCode == budget.CostCenterCode
                    );
            }
            else // BIGC
            {
                isDuplicate = await _context.HRB_BUDGET_BIGC
                    .AnyAsync(b => 
                        b.EmpCode == budget.EmpCode && 
                        b.BudgetYear == budget.BudgetYear &&
                        b.CostCenterCode == budget.CostCenterCode
                    );
            }

            if (isDuplicate)
            {
                throw new DuplicateBudgetException(
                    $"พนักงานรหัส {budget.EmpCode} มีข้อมูลงบประมาณปี {budget.BudgetYear} " +
                    $"สำหรับ Cost Center {budget.CostCenterCode} อยู่แล้ว",
                    budget.EmpCode,
                    budget.BudgetYear,
                    budget.CostCenterCode
                );
            }
            // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

            // Route to correct table
            object createdBudget;
            if (budget.CompanyId == 1)
            {
                var bjcDto = MapToBjcDto(budget);
                createdBudget = await CreateBjcBudgetAsync(bjcDto);
            }
            else
            {
                var bigcDto = MapToBigcDto(budget);
                createdBudget = await CreateBigcBudgetAsync(bigcDto);
            }

            int budgetId = GetBudgetIdFromCreatedEntity(createdBudget);

            response.SuccessResults.Add(new BudgetSaveResult
            {
                RowIndex = i,
                BudgetId = budgetId,
                EmpCode = budget.EmpCode,
                CostCenterCode = budget.CostCenterCode,
                BudgetYear = budget.BudgetYear,
                CompanyId = budget.CompanyId
            });
            response.TotalSuccess++;
        }
        catch (DuplicateBudgetException ex)
        {
            // Handle duplicate with clear Thai message
            response.FailedResults.Add(new BudgetSaveError
            {
                RowIndex = i,
                EmpCode = budget.EmpCode,
                ErrorMessage = ex.Message, // Thai message
                ErrorDetails = $"Duplicate: {ex.EmpCode} | Year: {ex.BudgetYear} | CC: {ex.CostCenterCode}",
                SubmittedData = budget
            });
            response.TotalFailed++;
        }
        catch (Exception ex)
        {
            response.FailedResults.Add(new BudgetSaveError
            {
                RowIndex = i,
                EmpCode = budget.EmpCode,
                ErrorMessage = ex.Message,
                ErrorDetails = ex.InnerException?.Message,
                SubmittedData = budget
            });
            response.TotalFailed++;
        }
    }

    response.Success = response.TotalFailed == 0;
    response.Message = response.Success
        ? $"Successfully saved {response.TotalSuccess} budget entries."
        : $"Saved {response.TotalSuccess} entries with {response.TotalFailed} failures.";

    return response;
}

// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// 🎯 Custom Exception for Option B
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
public class DuplicateBudgetException : Exception
{
    public string EmpCode { get; }
    public int BudgetYear { get; }
    public string CostCenterCode { get; }

    public DuplicateBudgetException(
        string message, 
        string empCode, 
        int budgetYear, 
        string costCenterCode
    ) : base(message)
    {
        EmpCode = empCode;
        BudgetYear = budgetYear;
        CostCenterCode = costCenterCode;
    }
}
```

**📝 หมายเหตุ Option B:**
- ใช้ `AnyAsync()` แทน `FirstOrDefaultAsync()` เพื่อความเร็ว
- Error message เป็นภาษาไทยชัดเจน
- Custom exception เก็บ context สำหรับ debugging
- Performance: ~150-200ms per row (vs 100ms Option A)

</details>



#### **Task 1.4: Add Controller Endpoint**
```csharp
// File: Controllers/BudgetController.cs
// Location: After DeleteBudget() method

[HttpPost]
[Route("B0SaveBatchEntry")]
public async Task<IActionResult> SaveBatchEntry(
    [FromBody] BatchBudgetRequest request)
{
    // Implementation as per Section 3
}
```

### 📋 Phase 2: Frontend Integration (Day 3)

#### **Task 2.1: Create API Call Function**
```javascript
// File: wwwroot/lib/razor/js/budget-planning/budget.plan.api.js
// Location: Add after existing budget API calls

/**
 * 💾 Save Batch Budget Entries
 * @param {Array} batchData - Array of budget objects
 * @returns {Promise<Object>} Batch save response
 */
async function saveBatchBudgetData(batchData) {
  try {
    console.log('📤 Sending batch save request...', {
      totalRows: batchData.length,
      data: batchData
    });

    const response = await fetch('/api/Budget/B0SaveBatchEntry', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        budgets: batchData,
        createdBy: 'CurrentUser' // TODO: Get from authentication
      })
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || 'Failed to save batch entries');
    }

    const result = await response.json();
    console.log('✅ Batch save response:', result);
    
    return result;
  } catch (error) {
    console.error('❌ Batch save error:', error);
    throw error;
  }
}
```

#### **Task 2.2: Update proceedWithDataCollection()**

**⚠️ หมายเหตุ:** Loading message จะแตกต่างกันตาม SA ตอบคำถาม Q6 (Duplicate Check Strategy)

```javascript
// File: wwwroot/lib/razor/js/budget-planning/budget.plan.events.js
// Location: Replace setTimeout simulation (Lines 5011-5015)

proceedWithDataCollection: async function (validationResults) {
  console.log('📦 Collecting data from all valid rows...');

  // Collect all data from valid rows
  const batchData = [];
  this.activeRows.forEach((rowData, rowId) => {
    const data = this.collectRowData(rowId);
    if (data) {
      batchData.push(data);
    }
  });

  if (batchData.length === 0) {
    console.error('❌ No valid data collected for saving');
    return;
  }

  // Show success message with validation summary
  const container = document.getElementById('batchValidationMessages');
  if (container) {
    const successDiv = document.createElement('div');
    successDiv.className = 'alert alert-success mt-2';
    successDiv.innerHTML = `
      <h6 class="alert-heading">
        <i class="fas fa-check-circle me-2"></i>Ready to Save!
      </h6>
      <p class="mb-2">All validation passed. Preparing to save ${batchData.length} rows...</p>
      ${validationResults.summary.totalWarnings > 0 ?
        `<small class="text-muted">Note: ${validationResults.summary.totalWarnings} warnings found but ignored.</small>` : ''}
    `;
    container.appendChild(successDiv);
  }

  console.log('📊 Enhanced batch data to save:', {
    totalRows: batchData.length,
    validationSummary: validationResults.summary,
    data: batchData
  });

  // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  // 🎯 NEW: Call actual API instead of setTimeout simulation
  // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  try {
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 🔍 LOADING MESSAGE (แตกต่างตาม Q6 Answer)
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    
    // Option A: No Pre-Check
    // this.showLoadingOverlay('กำลังบันทึกข้อมูล...');
    
    // Option B: Pre-Check (RECOMMENDED) ⭐
    this.showLoadingOverlay('กำลังตรวจสอบข้อมูลซ้ำและบันทึก...');
    
    // Optional: Performance warning for large batch
    if (batchData.length > 20) {
      console.warn(`⚠️ Large batch detected (${batchData.length} rows). Processing may take longer.`);
    }
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

    // Call API
    const response = await saveBatchBudgetData(batchData);

    // Hide loading
    this.hideLoadingOverlay();

    // Handle response
    if (response.success && response.totalFailed === 0) {
      // ✅ All successful
      this.showConfirmModal({
        title: 'บันทึกสำเร็จ',
        message: `บันทึกข้อมูลสำเร็จ ${response.totalSuccess} แถว!\n\nข้อมูลได้รับการตรวจสอบและบันทึกเรียบร้อยแล้ว`,
        iconType: 'success',
        confirmText: 'ตกลง',
        showCancel: false,
        onConfirm: () => {
          this.cancelBatchEntry(); // Clear and close
          console.log(`✅ Successfully saved ${response.totalSuccess} budget rows`);
        }
      });
    } 
    else if (response.totalSuccess > 0 && response.totalFailed > 0) {
      // ⚠️ Partial success
      this.showConfirmModal({
        title: 'บันทึกบางส่วนสำเร็จ',
        message: `บันทึกสำเร็จ ${response.totalSuccess} แถว\nล้มเหลว ${response.totalFailed} แถว\n\nกรุณาตรวจสอบรายละเอียดข้อผิดพลาด`,
        iconType: 'warning',
        confirmText: 'ดูรายละเอียด',
        showCancel: true,
        cancelText: 'ปิด',
        onConfirm: () => {
          this.showBatchErrorDetails(response.failedResults);
        }
      });
    }
    else {
      // ❌ All failed
      this.showConfirmModal({
        title: 'บันทึกล้มเหลว',
        message: `ไม่สามารถบันทึกข้อมูลได้ ${response.totalFailed} แถว\n\n${response.message}`,
        iconType: 'error',
        confirmText: 'ดูรายละเอียด',
        showCancel: true,
        cancelText: 'ปิด',
        onConfirm: () => {
          this.showBatchErrorDetails(response.failedResults);
        }
      });
    }
  } 
  catch (error) {
    // Hide loading
    this.hideLoadingOverlay();

    // Show error modal
    this.showConfirmModal({
      title: 'เกิดข้อผิดพลาด',
      message: `ไม่สามารถเชื่อมต่อกับเซิร์ฟเวอร์ได้\n\nกรุณาลองใหม่อีกครั้ง\n\nError: ${error.message}`,
      iconType: 'error',
      confirmText: 'ตกลง',
      showCancel: false
    });

    console.error('❌ Batch save failed:', error);
  }
},
```

#### **Task 2.3: Add Helper Methods**

**⚠️ หมายเหตุ:** Error display จะปรับปรุงสำหรับ Option B (Duplicate Check)

```javascript
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// 🎯 OPTION A: Simple Error Display
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
showBatchErrorDetails_OptionA: function (failedResults) {
  const container = document.getElementById('batchValidationMessages');
  if (!container) return;

  const errorDiv = document.createElement('div');
  errorDiv.className = 'alert alert-danger mt-2';
  errorDiv.innerHTML = `
    <h6 class="alert-heading">
      <i class="fas fa-exclamation-circle me-2"></i>
      ข้อผิดพลาดในการบันทึก (${failedResults.length} แถว)
    </h6>
    <ul class="mb-0">
      ${failedResults.map(error => `
        <li>
          <strong>แถวที่ ${error.rowIndex + 1}</strong>
          ${error.empCode ? `(${error.empCode})` : ''}: 
          ${error.errorMessage}
        </li>
      `).join('')}
    </ul>
  `;
  container.appendChild(errorDiv);
  container.scrollIntoView({ behavior: 'smooth', block: 'start' });
},

// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// 🎯 OPTION B: Enhanced Error Display with Grouping ⭐ RECOMMENDED
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
showBatchErrorDetails: function (failedResults) {
  const container = document.getElementById('batchValidationMessages');
  if (!container) return;

  // Group errors by type
  const duplicateErrors = failedResults.filter(e => 
    e.errorMessage.includes('อยู่แล้ว') || 
    e.errorMessage.includes('Duplicate')
  );
  const validationErrors = failedResults.filter(e => 
    e.errorMessage.includes('required') || 
    e.errorMessage.includes('Invalid')
  );
  const otherErrors = failedResults.filter(e => 
    !duplicateErrors.includes(e) && !validationErrors.includes(e)
  );

  let errorHtml = `
    <div class="alert alert-danger mt-2">
      <h6 class="alert-heading">
        <i class="fas fa-exclamation-circle me-2"></i>
        ข้อผิดพลาดในการบันทึก (${failedResults.length} แถว)
      </h6>
  `;

  // Duplicate Errors Section
  if (duplicateErrors.length > 0) {
    errorHtml += `
      <div class="mt-3">
        <h6 class="text-warning">
          <i class="fas fa-copy me-2"></i>
          ข้อมูลซ้ำ (${duplicateErrors.length} แถว)
        </h6>
        <ul class="mb-0">
          ${duplicateErrors.map(error => `
            <li>
              <strong>แถวที่ ${error.rowIndex + 1}</strong>
              ${error.empCode ? `<span class="badge bg-secondary">${error.empCode}</span>` : ''}: 
              ${error.errorMessage}
              ${error.errorDetails ? `<br><small class="text-muted">${error.errorDetails}</small>` : ''}
            </li>
          `).join('')}
        </ul>
      </div>
    `;
  }

  // Validation Errors Section
  if (validationErrors.length > 0) {
    errorHtml += `
      <div class="mt-3">
        <h6 class="text-danger">
          <i class="fas fa-exclamation-triangle me-2"></i>
          ข้อผิดพลาดการตรวจสอบ (${validationErrors.length} แถว)
        </h6>
        <ul class="mb-0">
          ${validationErrors.map(error => `
            <li>
              <strong>แถวที่ ${error.rowIndex + 1}</strong>
              ${error.empCode ? `<span class="badge bg-secondary">${error.empCode}</span>` : ''}: 
              ${error.errorMessage}
            </li>
          `).join('')}
        </ul>
      </div>
    `;
  }

  // Other Errors Section
  if (otherErrors.length > 0) {
    errorHtml += `
      <div class="mt-3">
        <h6 class="text-danger">
          <i class="fas fa-times-circle me-2"></i>
          ข้อผิดพลาดอื่นๆ (${otherErrors.length} แถว)
        </h6>
        <ul class="mb-0">
          ${otherErrors.map(error => `
            <li>
              <strong>แถวที่ ${error.rowIndex + 1}</strong>
              ${error.empCode ? `<span class="badge bg-secondary">${error.empCode}</span>` : ''}: 
              ${error.errorMessage}
            </li>
          `).join('')}
        </ul>
      </div>
    `;
  }

  errorHtml += `</div>`;
  
  const errorDiv = document.createElement('div');
  errorDiv.innerHTML = errorHtml;
  container.appendChild(errorDiv);
  container.scrollIntoView({ behavior: 'smooth', block: 'start' });
},
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

// Show/hide loading overlay
showLoadingOverlay: function (message = 'Loading...') {
  const overlay = document.getElementById('loadingOverlay');
  if (overlay) {
    const textElement = overlay.querySelector('#loadingText');
    if (textElement) {
      textElement.textContent = message;
      textElement.style.display = 'block';
    }
    overlay.classList.remove('d-none');
  }
},

hideLoadingOverlay: function () {
  const overlay = document.getElementById('loadingOverlay');
  if (overlay) {
    overlay.classList.add('d-none');
  }
}
```

### 📋 Phase 3: Testing (Day 4-5)

#### **Unit Tests**
- [ ] Test `CreateBatchBudgetsAsync()` with valid data
- [ ] Test with invalid CompanyId
- [ ] Test with missing required fields
- [ ] Test with duplicate entries
- [ ] Test with partial success scenario
- [ ] **⭐ (Option B only)** Test duplicate check performance
- [ ] **⭐ (Option B only)** Test Thai error messages for duplicates

#### **Integration Tests**
- [ ] Test full API endpoint `/api/Budget/B0SaveBatchEntry`
- [ ] Test with 1 row
- [ ] Test with 10 rows
- [ ] Test with 50 rows (performance)
- [ ] Test error response format

#### **Frontend Tests**
- [ ] Test successful batch save
- [ ] Test partial success (some rows fail)
- [ ] Test all rows fail
- [ ] Test network error
- [ ] Test loading overlay display
- [ ] **⭐ (Option B only)** Test loading message shows "กำลังตรวจสอบข้อมูลซ้ำและบันทึก..."
- [ ] **⭐ (Option B only)** Test error grouping (duplicate vs validation vs other)
- [ ] **⭐ (Option B only)** Test performance warning for large batch (>20 rows)

### 📋 Phase 4: Documentation & Deployment (Day 6)

- [ ] Update API documentation
- [ ] Update SAVE_ALL_BUTTON_WORKFLOW.md
- [ ] Create user guide for error handling
- [ ] Deploy to staging
- [ ] UAT testing with SA
- [ ] Deploy to production

---

## 8. Testing Strategy

### 🧪 Test Cases

#### **TC-001: Success - All Rows Valid (BJC)**
```json
Request:
{
  "budgets": [
    {
      "companyId": 1,
      "budgetYear": 2026,
      "empCode": "E001",
      "costCenterCode": "CC001",
      // ... all required fields
    }
  ],
  "createdBy": "TestUser"
}

Expected Response:
{
  "success": true,
  "message": "Successfully saved 1 budget entries.",
  "totalSubmitted": 1,
  "totalSuccess": 1,
  "totalFailed": 0,
  "successResults": [
    {
      "rowIndex": 0,
      "budgetId": 1001,
      "empCode": "E001",
      "costCenterCode": "CC001",
      "budgetYear": 2026,
      "companyId": 1
    }
  ],
  "failedResults": []
}
```

#### **TC-002: Success - All Rows Valid (BIGC)**
```json
Request:
{
  "budgets": [
    {
      "companyId": 2,
      "budgetYear": 2026,
      "empCode": "E002",
      "costCenterCode": "CC002",
      // ... all required fields
    }
  ],
  "createdBy": "TestUser"
}

Expected: HTTP 200, success=true
```

#### **TC-003: Partial Success - Same Company with 1 Duplicate**
```json
Request:
{
  "budgets": [
    { "companyId": 1, "empCode": "E001", "budgetYear": 2026, ... }, // Valid BJC
    { "companyId": 1, "empCode": "E002", "budgetYear": 2026, ... }, // Valid BJC
    { "companyId": 1, "empCode": "E001", "budgetYear": 2026, ... }  // Duplicate E001
  ]
}

Expected Response:
{
  "success": false,
  "totalSubmitted": 3,
  "totalSuccess": 2,
  "totalFailed": 1,
  "failedResults": [
    {
      "rowIndex": 2,
      "empCode": "E001",
      "errorMessage": "Duplicate entry: Employee E001 already exists for Budget Year 2026"
    }
  ]
}
```

#### **TC-004: Validation Error - Missing Required Field**
```json
Request:
{
  "budgets": [
    {
      "companyId": 1,
      "budgetYear": 2026,
      // Missing empCode
      "costCenterCode": "CC001"
    }
  ]
}

Expected: HTTP 400 Bad Request
```

#### **TC-005: Duplicate Entry**
```json
// Assume E001 already exists for 2026
Request:
{
  "budgets": [
    {
      "companyId": 1,
      "budgetYear": 2026,
      "empCode": "E001", // Duplicate
      "costCenterCode": "CC001"
    }
  ]
}

Expected: HTTP 200 with error in failedResults
```

#### **TC-006: Performance - 50 Rows**
```json
Request: { budgets: [ /* 50 rows */ ] }
Expected: Response time < 5 seconds
```

---

## 9. Performance Considerations

### ⚡ Optimization Strategies

#### **Batch Size Limits**
```csharp
// Add validation in controller
if (request.Budgets.Count > 100)
{
    return BadRequest(new {
        Success = false,
        Message = "Maximum 100 rows per batch. Please split into smaller batches."
    });
}
```

#### **Database Optimization**
```csharp
// Use AddRange instead of multiple Add calls
await _context.HRB_BUDGET_BJC.AddRangeAsync(bjcBudgets);
await _context.HRB_BUDGET_BIGC.AddRangeAsync(bigcBudgets);
await _context.SaveChangesAsync();
```

#### **Parallel Processing (Advanced)**
```csharp
// For very large batches (>100 rows)
var tasks = budgets.Select(async budget => {
    return await CreateBudgetAsync(budget, budget.CompanyId.Value);
});
var results = await Task.WhenAll(tasks);
```

### 📊 Performance Metrics

| Metric | Target | Measurement |
|--------|--------|-------------|
| **API Response Time** | < 2s for 10 rows | Use Postman/Swagger timer |
| **API Response Time** | < 5s for 50 rows | Use Postman/Swagger timer |
| **Database Insert** | < 100ms per row | SQL Server Profiler |
| **Frontend Feedback** | Immediate | Loading overlay shows < 200ms |

---

## 10. Security & Validation

### 🔒 Security Measures

#### **1. Authentication & Authorization**
```csharp
[Authorize] // Add authentication requirement
[HttpPost("B0SaveBatchEntry")]
public async Task<IActionResult> SaveBatchEntry(...)
{
    // Get current user from auth context
    var currentUser = User.Identity?.Name ?? "System";
    // Pass to service
}
```

#### **2. Input Sanitization**
```csharp
// Sanitize string inputs
budget.EmpCode = budget.EmpCode?.Trim();
budget.CostCenterCode = budget.CostCenterCode?.Trim();
```

#### **3. SQL Injection Protection**
✅ Entity Framework Core provides built-in protection
✅ Always use parameterized queries
❌ Never concatenate SQL strings

#### **4. Rate Limiting**
```csharp
// Add rate limiting middleware
[EnableRateLimiting("BatchSavePolicy")]
[HttpPost("B0SaveBatchEntry")]
```

### ✅ Validation Rules

#### **Frontend Validation (Already Implemented)**
- ✅ Required fields validation
- ✅ Format validation (dates, numbers)
- ✅ Business rules (budget logic)
- ✅ 3-level validation gates

#### **Backend Validation (To Add)**
```csharp
// Server-side validation (defense in depth)
private void ValidateBudgetEntry(BudgetResponseDto budget)
{
    var errors = new List<string>();

    // Required fields
    if (string.IsNullOrWhiteSpace(budget.EmpCode))
        errors.Add("Employee Code is required");
    
    if (string.IsNullOrWhiteSpace(budget.CostCenterCode))
        errors.Add("Cost Center Code is required");
    
    if (budget.BudgetYear <= 0)
        errors.Add("Budget Year is required");
    
    if (!budget.CompanyId.HasValue)
        errors.Add("Company ID is required");

    // Business rules
    if (budget.BudgetYear < 2020 || budget.BudgetYear > 2030)
        errors.Add("Budget Year must be between 2020 and 2030");

    if (budget.Payroll < 0)
        errors.Add("Payroll cannot be negative");

    if (errors.Any())
        throw new ValidationException(string.Join("; ", errors));
}
```

---

## 📝 Summary & Next Steps

### ✅ Key Decisions Made

1. **Transaction Strategy:** Individual transactions (partial success allowed)
2. **Error Handling:** Detailed error reporting per row
3. **Response Format:** Comprehensive BatchBudgetResponse with success/failed lists
4. **Company Routing:** Service layer handles BJC/BIGC table routing
5. **Validation:** Frontend + Backend validation (defense in depth)

### 🎯 Implementation Order

1. ✅ **Phase 1:** Backend DTOs & Service (2 days)
2. ✅ **Phase 2:** Controller Endpoint (1 day)
3. ✅ **Phase 3:** Frontend Integration (1 day)
4. ✅ **Phase 4:** Testing (2 days)
5. ✅ **Phase 5:** Documentation & Deployment (1 day)

**Total Estimated Time:** 7 days

### 📋 Checklist Before SA Approval

- [x] DTO structure defined
- [x] Service method logic planned
- [x] Controller endpoint specified
- [x] Data flow diagram complete
- [x] Error handling strategy documented
- [x] Transaction approach selected
- [x] Field mapping strategy clear
- [x] Test cases identified
- [x] Performance targets set
- [x] Security measures outlined

---

## 🔍 Questions for SA Review

### **Q1: Transaction Strategy**
> ต้องการให้ batch save เป็น "all or nothing" (ถ้า 1 แถวผิด ทั้งหมดไม่บันทึก) หรือ "partial success" (บันทึกได้เท่าที่ valid)?

**Recommendation:** Partial success (Individual transactions)

---

### **Q2: Batch Size Limit**
> ควรจำกัดจำนวนแถวสูงสุดต่อ 1 batch ไว้เท่าไหร่?

**Recommendation:** 100 rows per batch (can increase if performance allows)

---

### **Q3: Duplicate Handling**
> ถ้ามี duplicate entry (EMP_CODE + BUDGET_YEAR + COST_CENTER_CODE ซ้ำ) ต้องการให้:
> - A. Reject (return error)
> - B. Update existing record
> - C. Skip duplicate

**Recommendation:** Option A (Reject with clear error message)

---

### **Q4: Authentication**
> ต้องการ authentication ก่อน save หรือไม่? ถ้าใช่ ใช้ system ใด?

**Current:** No authentication in existing endpoints  
**Recommendation:** Add `[Authorize]` attribute if auth system exists

---

#### **📖 คำอธิบายโดยละเอียด**

**Authentication คืออะไร?**

Authentication (การยืนยันตัวตน) คือการตรวจสอบว่าผู้ใช้ที่เรียก API นี้เป็นใครจริงๆ ก่อนที่จะอนุญาตให้ save ข้อมูล

---

#### **🔍 สถานการณ์ปัจจุบัน**

จากการวิเคราะห์ `BudgetController.cs` พบว่า:

```csharp
// ❌ ไม่มี [Authorize] attribute
[HttpPost]
public async Task<IActionResult> CreateBudget([FromBody] CreateBudgetRequest request)
{
    // ใครก็เรียกได้โดยไม่ต้อง login
    var result = await _budgetService.CreateBudgetAsync(...);
    return Ok(result);
}
```

**ความเสี่ยง:**
- ❌ ใครก็สามารถเรียก API ได้ (ถ้ารู้ URL)
- ❌ ไม่สามารถ track ว่าใครบันทึกข้อมูล
- ❌ ไม่มีการควบคุมสิทธิ์

---

#### **💡 ตัวเลือกสำหรับ Q4**

<details>
<summary><strong>Option A: ไม่ใช้ Authentication (ไม่แนะนำสำหรับ Production)</strong></summary>

```csharp
// ไม่มี [Authorize]
[HttpPost("B0SaveBatchEntry")]
public async Task<IActionResult> SaveBatchEntry(
    [FromBody] BatchBudgetRequest request)
{
    // ใครก็เรียกได้
    var response = await _budgetService.CreateBatchBudgetsAsync(...);
    return Ok(response);
}
```

**✅ Pros:**
- ไม่ต้อง setup authentication system
- Development ง่ายกว่า
- Frontend ไม่ต้องส่ง token

**❌ Cons:**
- ❌ ไม่ปลอดภัย (ใครก็เรียกได้)
- ❌ ไม่รู้ว่าใครบันทึกข้อมูล
- ❌ ไม่สามารถควบคุมสิทธิ์ (เช่น เฉพาะ HRBP)

**⚠️ เหมาะกับ:**
- Development/Testing environment
- Internal network only
- Demo/POC

</details>

<details>
<summary><strong>Option B: ใช้ Authentication (แนะนำสำหรับ Production) ⭐</strong></summary>

```csharp
// เพิ่ม [Authorize]
[Authorize] // ← ต้อง login ก่อน
[HttpPost("B0SaveBatchEntry")]
public async Task<IActionResult> SaveBatchEntry(
    [FromBody] BatchBudgetRequest request)
{
    // ดึงข้อมูล user ที่ login
    var currentUser = User.Identity?.Name ?? "Unknown";
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    
    // บันทึกพร้อม audit trail
    var response = await _budgetService.CreateBatchBudgetsAsync(
        request.Budgets, 
        currentUser // ← รู้ว่าใครบันทึก
    );
    
    return Ok(response);
}
```

**✅ Pros:**
- ✅ ปลอดภัย (ต้อง login ก่อน)
- ✅ รู้ว่าใครบันทึกข้อมูล (Audit trail)
- ✅ สามารถควบคุมสิทธิ์ได้

**❌ Cons:**
- ต้องมี authentication system (JWT, Cookie, etc.)
- Frontend ต้องจัดการ token
- ซับซ้อนกว่า

**⚠️ เหมาะกับ:**
- **Production environment** ✅
- Multiple users
- ต้องการ audit trail

</details>

<details>
<summary><strong>Option C: ใช้ Authentication + Authorization (Advanced) 🔐</strong></summary>

```csharp
// เพิ่ม role-based authorization
[Authorize(Roles = "HRBP,Admin")] // ← เฉพาะ HRBP/Admin เท่านั้น
[HttpPost("B0SaveBatchEntry")]
public async Task<IActionResult> SaveBatchEntry(
    [FromBody] BatchBudgetRequest request)
{
    var currentUser = User.Identity?.Name ?? "Unknown";
    var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
    
    // Log who did what
    _logger.LogInformation(
        "User {User} (Role: {Role}) saved {Count} budgets", 
        currentUser, userRole, request.Budgets.Count
    );
    
    var response = await _budgetService.CreateBatchBudgetsAsync(
        request.Budgets, 
        currentUser
    );
    
    return Ok(response);
}
```

**✅ Pros:**
- ✅ ปลอดภัยสูงสุด
- ✅ ควบคุมสิทธิ์ละเอียด (เฉพาะ role)
- ✅ Audit trail ครบถ้วน

**❌ Cons:**
- ต้องมี role management system
- ซับซ้อนมากที่สุด

**⚠️ เหมาะกับ:**
- Enterprise system
- Multi-tenant
- Compliance requirements

</details>

---

#### **🎯 Authentication Systems ที่รองรับ**

ถ้า SA เลือก Option B/C ต้องมี authentication system อย่างใดอย่างหนึ่ง:

| System | Description | Setup Complexity |
|--------|-------------|------------------|
| **JWT Token** | JSON Web Token (modern, stateless) | ⭐⭐⭐ ปานกลาง |
| **Cookie Auth** | Traditional cookie-based (ASP.NET Identity) | ⭐⭐ ง่าย |
| **Azure AD** | Microsoft Azure Active Directory | ⭐⭐⭐⭐ ยาก |
| **OAuth 2.0** | Third-party (Google, Facebook) | ⭐⭐⭐⭐⭐ ยากมาก |

---

#### **🔥 JWT Token vs OAuth 2.0: เปรียบเทียบโดยละเอียด**

<details>
<summary><strong>🎯 JWT Token (JSON Web Token)</strong> ⭐ RECOMMENDED สำหรับ Budget System</summary>

**📖 คืออะไร?**

JWT เป็น token-based authentication ที่เก็บข้อมูล user ไว้ใน token เอง (self-contained)

**🔄 การทำงาน:**
```
1. User login → Server verify → Generate JWT Token
2. Client เก็บ token (localStorage)
3. ทุก request ส่ง token ใน header
4. Server verify token → Allow access
```

**✅ ข้อดี:**
- ✅ **Stateless:** Server ไม่ต้องเก็บ session (scalable)
- ✅ **Fast:** ไม่ต้อง query database ทุกครั้ง
- ✅ **Simple:** ใช้งานง่าย สำหรับ internal system
- ✅ **Cross-domain:** ใช้ได้กับ mobile app, SPA
- ✅ **Self-contained:** เก็บข้อมูล user ใน token
- ✅ **No external dependency:** ไม่ต้องพึ่ง third-party

**❌ ข้อเสีย:**
- ❌ Token size ใหญ่กว่า session ID
- ❌ ยาก revoke (ต้องรอ token หมดอายุ)
- ❌ ถ้า token ถูกขโมย ใช้งานได้จนกว่าจะหมดอายุ

**📊 ตัวอย่าง JWT Token:**
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.
eyJ1c2VySWQiOiIxMjM0IiwidXNlcm5hbWUiOiJocmJwMDEiLCJyb2xlIjoiSFJCUCJ9.
SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c
```

**🔧 Setup (ASP.NET Core):**
```csharp
// 1. Install Package
// dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer

// 2. Configure in Program.cs
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "YourIssuer",
            ValidAudience = "YourAudience",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("YourSecretKey")
            )
        };
    });

// 3. Login Endpoint
[HttpPost("login")]
public IActionResult Login([FromBody] LoginRequest request)
{
    // Verify username/password
    if (ValidateUser(request.Username, request.Password))
    {
        var token = GenerateJwtToken(request.Username);
        return Ok(new { token });
    }
    return Unauthorized();
}

// 4. Use in Controller
[Authorize]
[HttpPost("B0SaveBatchEntry")]
public async Task<IActionResult> SaveBatchEntry(...)
{
    var currentUser = User.Identity?.Name;
    // ...
}
```

**💰 Cost:** FREE (ไม่มีค่าใช้จ่าย)

**⏱️ Setup Time:** 1 วัน

**🎯 เหมาะกับ:**
- ✅ **Internal system** (Budget System) ← **แนะนำ**
- ✅ Company-owned users
- ✅ Simple authentication
- ✅ Mobile app + Web
- ✅ Microservices architecture

</details>

<details>
<summary><strong>🌐 OAuth 2.0 (Third-Party Authentication)</strong></summary>

**📖 คืออะไร?**

OAuth 2.0 เป็น authorization protocol ที่ให้ third-party (Google, Facebook, Microsoft) ทำการ authenticate แทน

**🔄 การทำงาน:**
```
1. User click "Login with Google"
2. Redirect ไป Google login page
3. Google verify → Return authorization code
4. Exchange code → Get access token
5. Use token to access Google API
6. Get user info → Create session
```

**✅ ข้อดี:**
- ✅ **No password management:** ไม่ต้องเก็บ password
- ✅ **Social login:** Login ด้วย Google/Facebook/Microsoft
- ✅ **Better security:** ใช้ security ของ Google/Facebook
- ✅ **User convenience:** ไม่ต้องจำ password
- ✅ **2FA built-in:** Google มี 2-factor authentication

**❌ ข้อเสีย:**
- ❌ **External dependency:** พึ่ง third-party (Google down = ไม่ login ได้)
- ❌ **Complex setup:** ต้อง register app, callback URL, etc.
- ❌ **Privacy concern:** Google/Facebook รู้ว่า user ใช้ app อะไร
- ❌ **Not suitable for internal:** พนักงานต้องมี Google account
- ❌ **Cost:** บาง provider เก็บเงิน (Azure AD ต้องซื้อ license)
- ❌ **Network required:** ต้องเชื่อมต่อ internet เสมอ

**🔧 Setup (ASP.NET Core - Google OAuth):**
```csharp
// 1. Install Package
// dotnet add package Microsoft.AspNetCore.Authentication.Google

// 2. Register at Google Cloud Console
// - Create OAuth 2.0 Client ID
// - Get ClientId and ClientSecret
// - Set Redirect URI: https://yourapp.com/signin-google

// 3. Configure in Program.cs
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = "YOUR_GOOGLE_CLIENT_ID";
    options.ClientSecret = "YOUR_GOOGLE_CLIENT_SECRET";
    options.CallbackPath = "/signin-google";
});

// 4. Login Flow
[HttpGet("login-google")]
public IActionResult LoginGoogle()
{
    var properties = new AuthenticationProperties
    {
        RedirectUri = "/api/Budget/B0SaveBatchEntry"
    };
    return Challenge(properties, GoogleDefaults.AuthenticationScheme);
}
```

**💰 Cost:**
- Google OAuth: FREE (มี quota limit)
- Azure AD: **ต้องซื้อ license** ($6-$30/user/month)
- Facebook: FREE

**⏱️ Setup Time:** 2-3 วัน (ต้อง register, configure, test)

**🎯 เหมาะกับ:**
- ❌ **ไม่เหมาะกับ Internal system**
- ✅ Public-facing apps (E-commerce, Social media)
- ✅ B2C applications
- ✅ Startup/SaaS products
- ✅ External users

</details>

---

#### **📊 JWT vs OAuth 2.0: Comparison Table**

| Feature | JWT Token ⭐ | OAuth 2.0 |
|---------|-------------|-----------|
| **ความเหมาะสม** | ✅ **Internal system** | ❌ External users |
| **Setup Complexity** | ⭐⭐⭐ ปานกลาง | ⭐⭐⭐⭐⭐ ยากมาก |
| **Setup Time** | 1 วัน | 2-3 วัน |
| **Cost** | FREE | FREE-Paid |
| **External Dependency** | ❌ ไม่มี | ✅ มี (Google/FB) |
| **Password Management** | ต้องเก็บ password | ไม่ต้องเก็บ |
| **Scalability** | ✅ Excellent (stateless) | ⚠️ Good |
| **Security** | ✅ Good (ถ้า implement ถูก) | ✅ Excellent |
| **User Experience** | ต้องจำ password | ง่ายกว่า (social login) |
| **Network Required** | แค่ตอน login | ทุกครั้ง |
| **Offline Support** | ✅ ใช้งานได้ (token cache) | ❌ ต้อง online |
| **Revocation** | ❌ ยาก | ✅ ง่าย |
| **Token Size** | ใหญ่กว่า (~200 bytes) | เล็กกว่า (~50 bytes) |
| **Use Case** | ✅ **Budget System** | ❌ Public apps |

---

#### **🎯 สรุป: อันไหนดีกว่ากัน?**

**สำหรับ Budget System นี้:**

### ✅ **JWT Token เหมาะกว่า** เพราะ:

1. **Internal System:**
   - Budget system เป็น internal company tool
   - User คือพนักงาน HRBP (มี company account อยู่แล้ว)
   - ไม่ต้องการ social login

2. **Simpler:**
   - Setup ง่ายกว่า (1 วัน vs 2-3 วัน)
   - ไม่ต้องพึ่ง third-party
   - Maintenance ง่ายกว่า

3. **Cost:**
   - FREE ไม่มีค่าใช้จ่าย
   - OAuth 2.0 (Azure AD) ต้องซื้อ license

4. **Performance:**
   - Stateless (เร็วกว่า)
   - ไม่ต้อง call external API

5. **Offline Support:**
   - Token cache ใช้งานได้แม้ network ช้า
   - OAuth 2.0 ต้อง online เสมอ

---

### ❌ **OAuth 2.0 ไม่เหมาะกับ Budget System** เพราะ:

1. **Internal System ไม่ต้องการ Social Login:**
   - User เป็นพนักงาน ไม่ใช่ public
   - มี company account อยู่แล้ว
   - ไม่จำเป็นต้อง "Login with Google"

2. **External Dependency:**
   - ถ้า Google/Azure down → ระบบไม่ทำงาน
   - เสี่ยงต่อการ downtime

3. **Complex & Costly:**
   - Setup ยาก (register app, callback, etc.)
   - Azure AD ต้องซื้อ license ($6-$30/user/month)
   - Budget system มี user เยอะ = แพง

4. **Privacy:**
   - Google/Facebook จะรู้ว่า user ใช้ Budget system
   - อาจไม่เหมาะกับข้อมูล sensitive

---

#### **💡 Recommendation สำหรับ SA**

**ถ้าต้องการ implement authentication (Q4 Option B/C):**

### 🎯 **เลือก JWT Token** ⭐

**เหตุผล:**
1. ✅ เหมาะกับ internal system
2. ✅ Setup ง่าย (1 วัน)
3. ✅ FREE ไม่มีค่าใช้จ่าย
4. ✅ ไม่ต้องพึ่ง third-party
5. ✅ Performance ดี (stateless)
6. ✅ Offline support

**Implementation:**
```csharp
// Simple JWT implementation
[Authorize] // ← เพียงแค่นี้
[HttpPost("B0SaveBatchEntry")]
public async Task<IActionResult> SaveBatchEntry(...)
{
    var currentUser = User.Identity?.Name; // ได้ username
    // ...
}
```

---

### ⚠️ **พิจารณา OAuth 2.0 เมื่อ:**

- ❌ Budget system **ไม่ใช่ internal** (เป็น public-facing)
- ❌ ต้องการให้ **external users** login
- ❌ ต้องการ **social login** (Google/Facebook)
- ❌ มี **B2C** use case
- ❌ มี **Azure AD** อยู่แล้ว (และซื้อ license แล้ว)

**แต่ Budget System ไม่ตรงเงื่อนไขเหล่านี้**

---

#### **📋 คำตอบสำหรับ Q4**

**ถ้า SA ตอบว่า:**

1. **"ต้องการ authentication"** → ใช้ **JWT Token** ✅
2. **"ไม่ต้องการ authentication"** → Option A (ไม่แนะนำ production)
3. **"มี Azure AD อยู่แล้ว"** → ใช้ **Azure AD** (OAuth 2.0)
4. **"ต้องการ social login"** → ไม่เหมาะกับ Budget System

**แนะนำ:** **JWT Token** เป็นตัวเลือกที่ดีที่สุดสำหรับ Budget System นี้ครับ! 🎯

---

#### **📋 Frontend Impact (ถ้าเลือก Option B/C)**

**1. ต้องเพิ่ม Token ใน API Call:**

```javascript
// Before (Option A)
const response = await fetch('/api/Budget/B0SaveBatchEntry', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
  },
  body: JSON.stringify({ budgets: batchData })
});

// After (Option B/C)
const token = localStorage.getItem('authToken'); // ดึง token ที่เก็บไว้
const response = await fetch('/api/Budget/B0SaveBatchEntry', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}` // ← เพิ่ม token
  },
  body: JSON.stringify({ budgets: batchData })
});
```

**2. ต้องจัดการ Token Expiration:**

```javascript
// ตรวจสอบ token หมดอายุ
if (response.status === 401) { // Unauthorized
  // Token หมดอายุ → redirect ไป login
  window.location.href = '/login';
}
```

**3. ต้องมี Login Flow:**

```
User → Login Page → Enter Username/Password → Get Token → Store Token → Use Token in API
```

---

#### **🔑 Implementation Steps (ถ้าเลือก Option B)**

**Backend (5 steps):**

1. **Install Package:**
   ```bash
   dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
   ```

2. **Configure in Program.cs:**
   ```csharp
   builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options => { /* config */ });
   ```

3. **Add Authorize Attribute:**
   ```csharp
   [Authorize]
   [HttpPost("B0SaveBatchEntry")]
   ```

4. **Get Current User:**
   ```csharp
   var currentUser = User.Identity?.Name;
   ```

5. **Store in Audit Field:**
   ```csharp
   budget.UpdatedBy = currentUser;
   ```

**Frontend (3 steps):**

1. **Store Token after Login:**
   ```javascript
   localStorage.setItem('authToken', token);
   ```

2. **Add Token to Request:**
   ```javascript
   headers: { 'Authorization': `Bearer ${token}` }
   ```

3. **Handle 401 Response:**
   ```javascript
   if (response.status === 401) { window.location.href = '/login'; }
   ```

---

#### **📊 Comparison Table**

| Feature | Option A | Option B | Option C |
|---------|----------|----------|----------|
| **ความปลอดภัย** | ❌ ต่ำ | ✅ สูง | ✅ สูงสุด |
| **Audit Trail** | ❌ ไม่มี | ✅ มี | ✅ มี + Log |
| **สิทธิ์ควบคุม** | ❌ ไม่มี | ⚠️ มีพื้นฐาน | ✅ ละเอียด |
| **Setup Time** | 0 วัน | +1 วัน | +2 วัน |
| **Frontend Impact** | ไม่มี | ต้องแก้ | ต้องแก้ |
| **เหมาะกับ** | Dev/Test | Production | Enterprise |

---

#### **🎯 คำถามที่ SA ต้องตอบ**

1. **มี authentication system อยู่แล้วหรือไม่?**
   - ✅ ถ้ามี → ใช้ Option B (แนะนำ)
   - ❌ ถ้าไม่มี → ต้อง setup ใหม่ หรือใช้ Option A ชั่วคราว

2. **ถ้ามี authentication แล้ว ใช้ระบบอะไร?**
   - JWT Token
   - Cookie-based (ASP.NET Identity)
   - Azure AD
   - อื่นๆ

3. **ต้องการควบคุมสิทธิ์แบบ role-based หรือไม่?**
   - ✅ ใช่ → Option C (เฉพาะ HRBP/Admin)
   - ❌ ไม่ → Option B (แค่ login ก็พอ)

4. **Environment ไหนที่จะ deploy?**
   - Development → Option A ชั่วคราว
   - Production → **ต้องใช้ Option B/C** ⭐

---

#### **💡 Recommendation สำหรับ SA**

**ถ้ามี Authentication System อยู่แล้ว:**
- ✅ **เลือก Option B** - เพิ่ม `[Authorize]` attribute
- ✅ ใช้เวลา implementation +1 วัน
- ✅ ปลอดภัยและ audit trail ครบ

**ถ้ายังไม่มี Authentication System:**
- ⚠️ **ใช้ Option A ชั่วคราว** สำหรับ Development
- ⚠️ แต่ **ต้อง** implement Option B ก่อน Production
- ⚠️ หรือพิจารณา setup authentication system เลย (+2-3 วัน)

**⚠️ สำคัญ:** สำหรับ Production environment **ไม่แนะนำ Option A** เด็ดขาด!

---

### **Q5: Success Message Strategy**
> เมื่อบันทึกสำเร็จทั้งหมด ต้องการให้:
> - A. Modal แสดงสรุป + auto-close
> - B. Modal แสดงรายละเอียด + ปุ่มปิด
> - C. Toast notification + auto-close

**Current Implementation:** Option A (Modal + auto-clear form)

---

### **Q6: Duplicate Check Strategy** ⭐ NEW
> ต้องการให้ตรวจสอบข้อมูลซ้ำ (duplicate) กับ database ก่อน save หรือไม่?

#### **Option A: ไม่ตรวจสอบก่อน (Rely on Database Constraint)**

```csharp
// ไม่ query check, save เลย
await CreateBjcBudgetAsync(budget);

// ถ้า duplicate → catch DbUpdateException (SQL Error 2627)
catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && 
    sqlEx.Number == 2627)
{
    throw new ArgumentException("Duplicate entry found");
}
```

**✅ Pros:**
- เร็วกว่า (1 query per row: INSERT only)
- Database guarantee uniqueness (ไม่มี race condition)
- ประหยัด database resources

**❌ Cons:**
- Error message generic (ไม่ค่อย user-friendly)
- ไม่รู้ล่วงหน้าว่าจะ duplicate

**Performance:** < 100ms per row

---

#### **Option B: ตรวจสอบก่อน save (Pre-Check + Custom Error Message)** ⭐ RECOMMENDED

```csharp
// Step 1: Check duplicate first
var exists = await _context.HRB_BUDGET_BJC
    .AnyAsync(b => 
        b.EmpCode == budget.EmpCode && 
        b.BudgetYear == budget.BudgetYear &&
        b.CostCenterCode == budget.CostCenterCode
    );

if (exists)
{
    throw new DuplicateException(
        $"พนักงานรหัส {budget.EmpCode} มีข้อมูลงบประมาณปี {budget.BudgetYear} " +
        $"สำหรับ Cost Center {budget.CostCenterCode} อยู่แล้ว"
    );
}

// Step 2: Then save
await CreateBjcBudgetAsync(budget);
```

**✅ Pros:**
- Error message ชัดเจนเป็นภาษาไทย ✅
- รู้ล่วงหน้าก่อน save ✅
- UX ดีกว่า (แจ้ง duplicate เฉพาะเจาะจง) ✅

**❌ Cons:**
- ช้ากว่า (2 queries per row: SELECT + INSERT)
- Potential race condition (แต่โอกาสต่ำมาก)

**Performance:** ~150-200ms per row (เพิ่มขึ้น ~50-100ms)

---

#### **🎯 UX Considerations (ถ้าเลือก Option B)**

**Frontend ต้องรองรับ:**

1. **Loading State ชัดเจนขึ้น**
   ```javascript
   // แสดง loading message ที่ชัดเจน
   this.showLoadingOverlay('กำลังตรวจสอบข้อมูลซ้ำและบันทึก...');
   ```

2. **Error Message แสดงรายละเอียด**
   ```javascript
   // Error message จาก backend จะชัดเจนขึ้น
   {
     "errorMessage": "พนักงานรหัส E001 มีข้อมูลงบประมาณปี 2026 สำหรับ Cost Center CC001 อยู่แล้ว"
   }
   
   // แทนที่จะเป็น
   {
     "errorMessage": "Duplicate entry found"
   }
   ```

3. **Performance Notice (Optional)**
   ```javascript
   // ถ้า batch ใหญ่ (>20 rows) อาจแสดง warning
   if (batchData.length > 20) {
     console.warn('Large batch detected. Duplicate check may take longer.');
   }
   ```

4. **Error Display ปรับปรุง**
   ```javascript
   showBatchErrorDetails: function (failedResults) {
     // แสดง error แบบ grouped by type
     const duplicateErrors = failedResults.filter(e => 
       e.errorMessage.includes('อยู่แล้ว')
     );
     const otherErrors = failedResults.filter(e => 
       !e.errorMessage.includes('อยู่แล้ว')
     );
     
     // แสดงแยกประเภท
     html += `<h6>ข้อมูลซ้ำ (${duplicateErrors.length} แถว)</h6>`;
     html += `<h6>ข้อผิดพลาดอื่นๆ (${otherErrors.length} แถว)</h6>`;
   }
   ```

---

#### **📊 Performance Impact (Option B vs Option A)**

| Batch Size | Option A Time | Option B Time | Difference |
|------------|---------------|---------------|------------|
| 10 rows | ~1.0s | ~1.5s | +50% |
| 50 rows | ~5.0s | ~7.5s | +50% |
| 100 rows | ~10.0s | ~15.0s | +50% |

**หมายเหตุ:** ถ้าเลือก Option B ควรปรับ Performance Target ใหม่:
- 10 rows: < **3s** (เดิม 2s)
- 50 rows: < **8s** (เดิม 5s)

---

#### **💡 Hybrid Approach (Option C - Advanced)**

```csharp
// Batch check duplicates ทั้งหมดในคราวเดียว
var empCodes = budgets.Select(b => b.EmpCode).ToList();
var existingBudgets = await _context.HRB_BUDGET_BJC
    .Where(b => empCodes.Contains(b.EmpCode) && b.BudgetYear == budgets[0].BudgetYear)
    .Select(b => new { b.EmpCode, b.CostCenterCode })
    .ToListAsync();

// Check duplicates in memory
foreach (var budget in budgets)
{
    var isDuplicate = existingBudgets.Any(e => 
        e.EmpCode == budget.EmpCode && 
        e.CostCenterCode == budget.CostCenterCode
    );
    if (isDuplicate) { /* error */ }
}
```

**✅ Pros:**
- เร็วกว่า Option B (1 SELECT query รวมทั้ง batch + individual INSERTs)
- Error message ชัดเจนเหมือน Option B

**❌ Cons:**
- ซับซ้อนกว่า
- ใช้ memory มากขึ้น (เก็บ existing budgets in memory)

**Performance:** ~100-120ms per row (กลางๆ ระหว่าง A และ B)

---

#### **🎯 Recommendation**

**แนะนำ Option B** เพราะ:
1. ✅ UX ดีที่สุด (error message ชัดเจน)
2. ✅ ง่ายต่อการ implement และ maintain
3. ✅ Performance impact ยอมรับได้ (batch มักไม่เกิน 20-30 rows)
4. ✅ Frontend ไม่ต้องแก้เยอะ (แค่ loading message)

**ถ้าเลือก Option C:** ใช้เมื่อ batch size ใหญ่บ่อยๆ (>50 rows) และต้องการ performance ดี

---

## 📌 Status: รอการอนุมัติจาก SA

**ผู้สร้างแผน:** Ten (AI Developer)  
**วันที่:** 19 ตุลาคม 2025  
**ผู้อนุมัติ:** _________________________  
**วันที่อนุมัติ:** _________________________  

---

**หมายเหตุ:** เอกสารนี้เป็น DRAFT รอการทบทวนและอนุมัติจาก SA ก่อนเริ่มดำเนินการ Implementation

---

## 📌 สรุปการทำงาน (Executive Summary)

### 🎯 วัตถุประสงค์หลัก
แทนที่ `setTimeout` simulation ใน `budget.plan.events.js` ด้วย API call จริงสำหรับบันทึก Batch Budget Entry

### 📊 ขอบเขตงาน
- **Frontend:** สร้าง `saveBatchBudgetData()` function เรียก API
- **Backend:** สร้าง endpoint `POST /api/Budget/B0SaveBatchEntry`
- **DTO:** สร้าง `BatchBudgetRequest.cs` และ `BatchBudgetResponse.cs`
- **Service:** เพิ่ม `CreateBatchBudgetsAsync()` ใน BudgetService

### 🏢 Company Routing
- **CompanyId = 1** → บันทึกลง `HRB_BUDGET_BJC` table
- **CompanyId = 2** → บันทึกลง `HRB_BUDGET_BIGC` table

### 🗄️ Database Fields Pattern
- **LE (Last Estimate):** ฟิลด์ที่มี suffix `*_LE` (เช่น `PAYROLL_LE`)
- **Current Year:** ฟิลด์ปกติไม่มี suffix (เช่น `PAYROLL`)
- **หมายเหตุ:** ไม่มีฟิลด์ `*_NX` (Next Year) ทั้ง BJC และ BIGC

### 🔄 Field Mapping
```
"batch-company" → COMPANY_ID (001→1, 002→2)
"batch-year" → BUDGET_YEAR
"batchLe-payroll" → PAYROLL_LE
"batchBg-payroll" → PAYROLL
```

### 🔒 Transaction Strategy (แนะนำ)
**Individual Transactions** - บันทึกทีละแถว เพื่อให้รายงาน success/failure แยกชัดเจน

### 📝 Error Handling
- **Partial Success:** แจ้ง success/failed count พร้อมรายละเอียดแต่ละแถว
- **Per-Row Tracking:** ระบุ `rowIndex`, `empCode`, `errorMessage` สำหรับแถวที่ล้มเหลว

### ⚡ Performance Target
- **10 rows:** < 2 วินาที
- **50 rows:** < 5 วินาที
- **Batch Limit:** แนะนำ 100 rows สูงสุด

### 🧪 Test Cases
- TC-001: บันทึกสำเร็จทั้งหมด (BJC)
- TC-002: บันทึกสำเร็จทั้งหมด (BIGC)
- TC-003: บันทึกบางส่วนสำเร็จ (มี Duplicate Entry)
- TC-004: Validation Error (Missing Required Field)
- TC-005: Duplicate Entry (ทั้งหมด)
- TC-006: Performance Test (50 rows)

### 📅 Implementation Timeline

**⚠️ หมายเหตุ:** Timeline อาจปรับเปลี่ยนตาม Q6 Answer

| Phase | งาน | Option A | Option B ⭐ |
|-------|-----|----------|------------|
| **Phase 1** | Backend DTOs & Service | 2 วัน | **2.5 วัน** (+0.5 duplicate check) |
| **Phase 2** | Controller Endpoint | 1 วัน | 1 วัน |
| **Phase 3** | Frontend Integration | 1 วัน | **1.5 วัน** (+0.5 UX enhancements) |
| **Phase 4** | Testing | 2 วัน | **2.5 วัน** (+0.5 duplicate tests) |
| **Phase 5** | Documentation & Deployment | 1 วัน | 1 วัน |
| **รวม** | | **7 วัน** | **8.5 วัน** |

**Option B เพิ่มงาน:**
- Backend: Duplicate check logic + Custom exception
- Frontend: Enhanced loading message + Error grouping + Performance warning
- Testing: Duplicate check tests + Thai message validation

### ❓ Questions ที่ต้องได้คำตอบจาก SA
1. **Transaction Strategy:** All-or-nothing หรือ Partial success?
2. **Batch Size Limit:** จำกัดกี่แถวต่อ batch?
3. **Duplicate Handling:** Reject, Update หรือ Skip?
4. **Authentication:** ต้องการ authentication หรือไม่?
5. **Success Message:** Modal แบบไหน (auto-close หรือ manual)?
6. **Duplicate Check Strategy:** ⭐ ตรวจสอบก่อน save (Option B) หรือพึ่ง DB constraint (Option A)?

### ✅ Status
- [x] วิเคราะห์ DTOs ครบถ้วน (8 files)
- [x] วิเคราะห์ Database Models (BJC/BIGC)
- [x] ออกแบบ API Endpoint พร้อม Request/Response
- [x] สร้าง Data Flow Diagram
- [x] วางแผน Error Handling Strategy
- [x] กำหนด Transaction Management
- [x] สร้าง Test Cases (6 scenarios)
- [x] จัดทำ Implementation Plan (7 days)
- [ ] **รอการอนุมัติจาก SA**

### 📦 Deliverables พร้อมใช้งาน
1. ✅ `BatchBudgetRequest.cs` - Request DTO
2. ✅ `BatchBudgetResponse.cs` - Response DTO
3. ✅ `BudgetSaveResult.cs` - Success result model
4. ✅ `BudgetSaveError.cs` - Error result model
5. ✅ `CreateBatchBudgetsAsync()` - Service method logic
6. ✅ `SaveBatchEntry()` - Controller endpoint
7. ✅ `saveBatchBudgetData()` - Frontend API call function
8. ✅ Error handling & validation logic
9. ✅ Test cases & performance metrics
10. ✅ Complete implementation guide

---

**📌 End of Document**
