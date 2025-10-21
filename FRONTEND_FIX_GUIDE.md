# 🔧 Frontend Fix Guide - Field Name Mismatch Issue

## ปัญหา
Frontend ส่ง field names ที่ไม่ตรงกับ Backend DTO ทำให้ข้อมูลไม่ถูก save ลง database (ได้ค่า NULL)

---

## 🎯 Fields ที่ต้องแก้ไข (CRITICAL - ทำให้ได้ NULL)

### ไฟล์ที่ต้องแก้: `wwwroot/js/plan.event.js` หรือฟังก์ชัน `collectRowData()`

แก้ไขชื่อ field ใน JavaScript object ที่ส่งไปยัง Backend API:

```javascript
// ❌ เก่า (ผิด) - ทำให้ได้ NULL
const budgetData = {
  NewHC: "Yes",           // ❌ Backend ต้องการ: NewHcCode
  NewPeriod: "Jan",       // ❌ Backend ต้องการ: NewVacPeriod
  NewLEPeriod: "Non",     // ❌ Backend ต้องการ: NewVacLe
  LEnOfMonth: "12",       // ❌ Backend ต้องการ: LeOfMonth (e ตัวเล็ก)
  NOfMonth: "12",         // ❌ Backend ต้องการ: NoOfMonth (No สองตัว)
  HRBP: null,             // ❌ Backend ต้องการ: HrbpEmpCode
  Group: null,            // ❌ Backend ต้องการ: GroupName
  GroupDiv: null,         // ❌ Backend ต้องการ: GroupLevel1
  GroupDept: null,        // ❌ Backend ต้องการ: GroupLevel2
  // ... fields อื่นๆ
};

// ✅ ใหม่ (ถูก) - แก้แล้วจะ save ได้
const budgetData = {
  NewHcCode: "Yes",       // ✅ ตรงกับ Backend
  NewVacPeriod: "Jan",    // ✅ ตรงกับ Backend
  NewVacLe: "Non",        // ✅ ตรงกับ Backend
  LeOfMonth: "12",        // ✅ ตรงกับ Backend
  NoOfMonth: "12",        // ✅ ตรงกับ Backend
  HrbpEmpCode: null,      // ✅ ตรงกับ Backend
  GroupName: null,        // ✅ ตรงกับ Backend
  GroupLevel1: null,      // ✅ ตรงกับ Backend
  GroupLevel2: null,      // ✅ ตรงกับ Backend
  // ... fields อื่นๆ
};
```

---

## 📋 สรุปตาราง Field Name Mapping

| Frontend (เก่า - ผิด) | Backend DTO (ต้องการ) | ความสำคัญ | คำอธิบาย |
|----------------------|----------------------|-----------|----------|
| `NewHC` | `NewHcCode` | 🔴 CRITICAL | New HC Code (Yes/No) |
| `NewPeriod` | `NewVacPeriod` | 🔴 CRITICAL | New Vacancy Period (Jan/Feb/...) |
| `NewLEPeriod` | `NewVacLe` | 🔴 CRITICAL | New Vacancy LE Period |
| `LEnOfMonth` | `LeOfMonth` | 🟡 HIGH | LE Number of Months (E ตัวเล็ก) |
| `NOfMonth` | `NoOfMonth` | 🟡 HIGH | Number of Months (No สองตัว) |
| `HRBP` | `HrbpEmpCode` | 🟡 HIGH | HRBP Employee Code |
| `Group` | `GroupName` | 🟡 HIGH | Group Name |
| `GroupDiv` | `GroupLevel1` | 🟡 HIGH | Group Division (Level 1) |
| `GroupDept` | `GroupLevel2` | 🟡 HIGH | Group Department (Level 2) |

---

## 🔍 วิธีหา Code ที่ต้องแก้

### ขั้นตอนที่ 1: ค้นหาไฟล์ที่ส่งข้อมูล

```bash
# ค้นหาใน wwwroot/js/
grep -r "NewHC\|NewPeriod\|LEnOfMonth" wwwroot/js/

# หรือใช้ VS Code Search (Ctrl+Shift+F)
# ค้นหา: NewHC
```

### ขั้นตอนที่ 2: มองหาฟังก์ชันที่รวบรวมข้อมูล

ฟังก์ชันที่มักจะใช้:
- `collectRowData()`
- `prepareBudgetData()`
- `getFormData()`
- ฟังก์ชันที่ถูกเรียกจากปุ่ม "Save"

### ขั้นตอนที่ 3: ตัวอย่าง Code ที่ต้องแก้

**ตัวอย่างที่ 1: Object Literal**
```javascript
// ❌ ก่อนแก้
const data = {
  NewHC: row.newHC,
  NewPeriod: row.newVacPeriod,
  NewLEPeriod: row.newVacLe
};

// ✅ หลังแก้
const data = {
  NewHcCode: row.newHC,
  NewVacPeriod: row.newVacPeriod,
  NewVacLe: row.newVacLe
};
```

**ตัวอย่างที่ 2: Dynamic Property Assignment**
```javascript
// ❌ ก่อนแก้
budgetData['NewHC'] = 'Yes';
budgetData['NewPeriod'] = 'Jan';
budgetData['LEnOfMonth'] = '12';

// ✅ หลังแก้
budgetData['NewHcCode'] = 'Yes';
budgetData['NewVacPeriod'] = 'Jan';
budgetData['LeOfMonth'] = '12';
```

**ตัวอย่างที่ 3: FormData**
```javascript
// ❌ ก่อนแก้
formData.append('NewHC', 'Yes');
formData.append('NewPeriod', 'Jan');

// ✅ หลังแก้
formData.append('NewHcCode', 'Yes');
formData.append('NewVacPeriod', 'Jan');
```

---

## ✅ วิธีทดสอบหลังแก้

### 1. ตรวจสอบ Network Request (Browser DevTools)

1. เปิด Browser DevTools (F12)
2. ไปที่ Tab "Network"
3. กดปุ่ม "Save" ในระบบ
4. คลิกที่ API request (มักจะเป็น POST/PUT)
5. ดูที่ "Payload" หรือ "Request Payload"
6. ตรวจสอบว่า field names ถูกต้อง:

```json
{
  "NewHcCode": "Yes",       // ✅ ต้องเป็นชื่อนี้
  "NewVacPeriod": "Jan",    // ✅ ต้องเป็นชื่อนี้
  "NewVacLe": "Non",        // ✅ ต้องเป็นชื่อนี้
  "LeOfMonth": "12",        // ✅ ต้องเป็นชื่อนี้
  "NoOfMonth": "12",        // ✅ ต้องเป็นชื่อนี้
  ...
}
```

### 2. ตรวจสอบ Database

หลังจาก Save แล้ว ตรวจสอบใน SQL Server:

```sql
-- ตรวจสอบ BIGC
SELECT TOP 1 
    NEW_HC_CODE,      -- ต้องไม่เป็น NULL
    NEW_VAC_PERIOD,   -- ต้องไม่เป็น NULL
    NEW_VAC_LE,       -- ต้องไม่เป็น NULL
    LE_OF_MONTH,      -- ต้องไม่เป็น NULL
    NO_OF_MONTH       -- ต้องไม่เป็น NULL
FROM HRB_BUDGET_BIGC
ORDER BY UPDATED_DATE DESC;

-- ตรวจสอบ BJC
SELECT TOP 1 
    NEW_HC_CODE,
    NEW_VAC_PERIOD,
    NEW_VAC_LE,
    LE_OF_MONTH,
    NO_OF_MONTH
FROM HRB_BUDGET_BJC
ORDER BY UPDATED_DATE DESC;
```

**ผลลัพธ์ที่คาดหวัง:**
```
NEW_HC_CODE    | NEW_VAC_PERIOD | NEW_VAC_LE | LE_OF_MONTH | NO_OF_MONTH
---------------|----------------|------------|-------------|-------------
Yes            | Jan            | Non        | 12          | 12
```

---

## 🚨 Fields ที่ไม่ต้องแก้ (OK แล้ว)

Fields เหล่านี้ชื่อตรงกันแล้ว หรือ ASP.NET Core จับคู่ได้ (case-insensitive):

```javascript
// ✅ ไม่ต้องแก้ - ใช้ได้แล้ว
FocusHC: "Y",        // → FocusHc (OK - case insensitive)
FocusPE: "Y",        // → FocusPe (OK - case insensitive)
Bonus: 2,            // → Bonus (OK)
BonusLe: 2,          // → BonusLe (OK)
Interest: 2,         // → Interest (OK)
InterestLe: 2,       // → InterestLe (OK)
Premium: 2,          // → Premium (OK)
MedicalInHouse: 2,   // → MedicalInHouse (OK)
// ... fields อื่นๆ ที่ชื่อตรงกัน
```

---

## 📝 Checklist สำหรับ SA

- [ ] หาไฟล์ JavaScript ที่ส่งข้อมูล (มักเป็น `plan.event.js`)
- [ ] หาฟังก์ชัน `collectRowData()` หรือฟังก์ชันที่รวบรวมข้อมูล
- [ ] แก้ไข field names ทั้ง 9 fields:
  - [ ] `NewHC` → `NewHcCode`
  - [ ] `NewPeriod` → `NewVacPeriod`
  - [ ] `NewLEPeriod` → `NewVacLe`
  - [ ] `LEnOfMonth` → `LeOfMonth`
  - [ ] `NOfMonth` → `NoOfMonth`
  - [ ] `HRBP` → `HrbpEmpCode`
  - [ ] `Group` → `GroupName`
  - [ ] `GroupDiv` → `GroupLevel1`
  - [ ] `GroupDept` → `GroupLevel2`
- [ ] ทดสอบ Save budget
- [ ] ตรวจสอบ Network Request (DevTools)
- [ ] ตรวจสอบ Database ว่ามีค่าไม่เป็น NULL

---

## 🆘 ถ้าแก้แล้วยังไม่ได้

### Case 1: ยังเป็น NULL อยู่

1. ตรวจสอบว่าแก้ไฟล์ถูกต้องหรือไม่ (อาจมีหลายไฟล์)
2. Clear browser cache (Ctrl+Shift+Delete)
3. Refresh หน้าเว็บ (Ctrl+F5)
4. ตรวจสอบ Network Request ว่าส่ง field names ถูกต้องหรือไม่

### Case 2: Backend Error

1. ตรวจสอบ Console Log ของ Backend
2. ตรวจสอบว่ามี validation error หรือไม่
3. ตรวจสอบว่า data type ถูกต้องหรือไม่ (string vs number)

### Case 3: ไม่แน่ใจว่าแก้ถูกหรือไม่

**Console.log ก่อนส่ง:**
```javascript
console.log('Budget Data Before Send:', JSON.stringify(budgetData, null, 2));
// ตรวจสอบว่า field names ถูกต้องหรือไม่
```

---

## 📞 ติดต่อ Backend Team

ถ้ามีปัญหาหรือข้อสงสัย:
- Backend Mapping: ✅ ถูกต้องครบถ้วนแล้ว
- ปัญหาอยู่ที่ Frontend ส่ง field names ผิด
- ต้องการความช่วยเหลือเพิ่มเติม: แจ้ง Backend Team

---

## ✨ หมายเหตุ

- **Backend ไม่ต้องแก้ไขอะไรเพิ่ม** - Mapping ถูกต้องครบถ้วนแล้ว
- **เฉพาะ Frontend ที่ต้องแก้** - เปลี่ยนชื่อ field 9 ตัว
- แก้แล้วจะทำให้ข้อมูลถูก save ลง database ครบถ้วน

---

**สร้างโดย:** Backend Team  
**วันที่:** 21 October 2025  
**ไฟล์อ้างอิง:** DATA.md (Frontend console.log)
