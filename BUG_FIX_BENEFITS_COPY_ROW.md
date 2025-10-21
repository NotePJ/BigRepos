# Budget Planning — `budget.plan.events.js` Method Map# Budget Planning — `budget.plan.events.js` Method Map



> **Last Updated:** 2025-10-19 (After Copy Row Bug Fix)  > แผนที่เมธอด → จุดเรียกใช้ / ลูป / ผลข้างเคียง (for quick review & onboarding)

> แผนที่เมธอด → จุดเรียกใช้ / ลูป / ผลข้างเคียง (for quick review & onboarding)

| Method | Purpose (สั้น) | Triggers / Callers | Internal Loops | Side Effects / Notes |

## 🎯 Core Object: `batchEntryManager`|---|---|---|---|---|

| `batchEntryManager` (object) | ศูนย์กลางจัดการ batch entry | ใช้โดยหน้า Budget ทั้งหมด | — | เก็บ state: `activeRows`, `nextRowId`, WeakMaps ต่าง ๆ |

**State Properties:**| `initialize()` | บูตอีเวนต์/สถานะเริ่มต้น | DOM ready → `initializeBudgetSystem()` | — | ผูกปุ่ม Add/Validate/Save, ตั้ง debounced funcs |

- `nextRowId`: Counter for unique row IDs| `initializeDebounced()` | สร้าง debounce wrappers | `initialize()` | — | ลด API calls ซ้ำเมื่อเปลี่ยนฟิลด์เร็ว |

- `activeRows`: Map of active row elements| `debounce(func, wait)` | ยูทิลดีบาวซ์ | ใช้ทุกเมธอดโหลด/validate | — | ยกเลิก timeout เดิม, เรียกเมื่อหยุดอินพุตครบเวลา |

- `rowEventListeners`: WeakMap for event cleanup| `attachEventListeners()` | ผูกอีเวนต์ระดับหน้า | `initialize()` | — | ปุ่ม add/delete/validate/save, กันกดรัวด้วย debounce |

- `rowValidationState`: WeakMap for validation state| `addBatchRow()` | เพิ่มแถวใหม่ในฟอร์ม | ปุ่ม Add / สคริปต์ | วนตั้ง `name/id` ให้ทุกฟิลด์ของแถว | สร้าง DOM, register แถวใน `activeRows`, ผูกอีเวนต์ระดับ row |

- `rowDOMReferences`: WeakMap for DOM references| `updateFormElementNames(row, rowId)` | รีไลน์ชื่อฟิลด์ให้ยูนีค | `addBatchRow()` | วนทุก input/select ใน row | ป้องกันชน `name/id` |

- `isInitialized`: Initialization flag (prevents double-init)| `attachRowEventListeners(row, rowId)` | ผูกอีเวนต์ในระดับแถว | `addBatchRow()` | — | change/input → cascade + validate ทันที |

- `isCopyingRow`: Flag for copy operation in progress| `removeExistingEventListeners(row)` | ถอดลิสต์เนอร์เก่า | ก่อน rebind | — | กันซ้อน/เมมโมรีรั่ว |

- `copySourceRowId`: Source row ID during copy| `populateDropdownAsync(el, url, def, cb)` | โหลด option แบบ async | เมธอด updateXXX | วน `data.forEach` เติม `<option>` | เคลียร์ option เดิม, ใส่ default, อัปเดต select2 |

| `populatePrimaryDropdownsAsync(row, rowId, …)` | โหลดเซ็ต dropdown พื้นฐาน | `addBatchRow()` | วน mapping `{selector, api}` | เติมค่าเริ่มต้นตามฟิลเตอร์ (company/year/…) |

---| `populateStaticDropdownsAsync(row)` | โหลด static แบบ async | `addBatchRow()` | วนชุด static | สำหรับฟิลด์ newHC/LE ฯลฯ |

| `populateStaticDropdowns(row)` | โหลด static แบบ sync | `addBatchRow()` | วนชุด static | สำรองในบางหน้า/เงื่อนไข |

## 📋 Method Reference Table| `setupBatchRowCascadingRelationships(row, rowId)` | ผูก parent→child cascading | `addBatchRow()` | — | change(parent) → call debounced update(child) |

| `updateBatchRowCostCenters(row, rowId)` | โหลด cost center | cascade/initial | วน options | เคลียร์ dropdown ลูกที่ลึกกว่า |

| Method | Purpose (สั้น) | Triggers / Callers | Internal Loops | Side Effects / Notes || `updateBatchRowDivisions(row, rowId)` | โหลด division | cascade | วน options | ใช้พารามิเตอร์ company/costCenter |

|---|---|---|---|---|| `updateBatchRowDepartments(row, rowId)` | โหลด department | cascade | วน options | — |

| **🔧 INITIALIZATION** || `updateBatchRowSections(row, rowId)` | โหลด section | cascade | วน options | — |

| `initialize()` | บูตอีเวนต์/สถานะเริ่มต้น + ป้องกัน double-init | DOM ready → `initializeBudgetSystem()` | — | ✅ ตรวจสอบ `isInitialized` ก่อนรัน, ผูกปุ่ม Add/Validate/Save || `updateBatchRowCompStore(row, rowId)` | โหลด comp/store | cascade | วน options | — |

| `initializeDebounced()` | สร้าง debounce wrappers สำหรับ API calls | `initialize()` | — | ลด API calls ซ้ำเมื่อเปลี่ยนฟิลด์เร็ว || `updateBatchRowPositions(row, rowId)` | โหลด position | cascade | วน options | — |

| `debounce(func, wait)` | ยูทิลดีบาวซ์ทั่วไป | ใช้ทุกเมธอดโหลด/validate | — | ยกเลิก timeout เดิม, เรียกเมื่อหยุดอินพุตครบเวลา || `updateBatchRowJobBands(row, rowId)` | โหลด job band | cascade | วน options | — |

| `attachEventListeners()` | ผูกอีเวนต์ระดับหน้า (global buttons) | `initialize()` | — | ปุ่ม add/delete/validate/save, กันกดรัวด้วย debounce || `updateBatchRowEmployeeStatus(row, rowId)` | โหลด employee status | cascade | วน options | — |

| `setupGlobalBenefitsValidation()` | ตั้งค่า benefits validation system | `initialize()` | — | เตรียม benefits field validators || `updateBatchRowGroupRunRates(row, rowId)` | โหลด group run rate | cascade | วน options | — |

| **➕ ROW OPERATIONS** || `updateBatchRowPlanCostCenters(row, rowId)` | โหลด plan cost center | cascade | วน options | — |

| `addBatchRow()` | เพิ่มแถวใหม่จาก template | ปุ่ม Add / Copy Row | วนตั้ง `name/id` ให้ทุกฟิลด์ | ✅ Clone template, register ใน `activeRows`, ผูกอีเวนต์, โหลด dropdowns || `clearBatchRowDependentDropdowns(row, level)` | ล้างค่าลูกที่ลึกกว่า | ทุกครั้งที่ parent เปลี่ยน | วน selectors ลูกหลายตัว | รีเซ็ต `.value = ''`, trigger `change` |

| `updateFormElementNames(row, rowId)` | รีไลน์ชื่อฟิลด์ให้ยูนีคต่อแถว | `addBatchRow()` | วนทุก input/select ใน row | ป้องกันชน `name/id` ระหว่างแถว || `initializeRowValidation(row, rowId)` | เตรียม state validation | หลังสร้าง row | วนสร้าง/แนบ container msg | เก็บใน WeakMap |

| `attachRowEventListeners(row, rowId)` | ผูกอีเวนต์ในระดับแถว | `addBatchRow()` | — | change/input → cascade + validate ทันที, เก็บใน WeakMap || `validateFieldRealTime(field, rowId)` | ตรวจทันทีเมื่อเปลี่ยนค่า | input/change | — | required/numeric/decimal → แสดงผลผ่าน UI |

| `removeExistingEventListeners()` | ถอดลิสต์เนอร์เก่า (global scope) | ก่อน rebind | — | กันซ้อน/เมมโมรีรั่ว || `validateRow(rowId)` | ตรวจทั้งแถว (เร็ว) | ก่อน save / manual | วนทุกฟิลด์ใน row | ส่งคืนผลรวมเบื้องต้น |

| `deleteBatchRow(rowId)` | ลบแถวเดียว | ปุ่มลบในแถว | — | ถอดอีเวนต์ + ลบจาก `activeRows` + อัปเดต counter || `validateRowComplete(rowId)` | ตรวจทั้งแถว (ละเอียด) | ก่อน save / manual | วนทุกฟิลด์ + benefits | รวมเป็น pass/warn/error |

| `deleteSelectedRows()` | ลบหลายแถวที่เลือก | ปุ่ม "Delete Selected" | วนชุดที่ checked | อัปเดตตัวนับหลังลบ || `validateBenefitsFields(row, companyId)` | ตรวจฟิลด์ benefits ไดนามิก | ระหว่าง validate row | วนรายการคอนฟิกของบริษัท | รองรับ BJC/BIGC template |

| `selectAllRows()` | เลือก/ยกเลิกเลือกทุกแถว | ปุ่ม "Select All" checkbox | วนทุกแถว | เช็ก/ยกเลิกเช็กกล่องเลือก || `displayValidation(field, opts)` | แสดงผล validate บนฟิลด์ | จาก validate* | — | เรียก render/apply styling + summary |

| `expandAllRows()` | ขยายทุก accordion | ปุ่ม "Expand All" | วนทุกแถว | เพิ่มคลาส `.show` ให้ collapse elements || `applyValidationStyling(field, type, warn, opts)` | ใส่คลาสสถานะ | จาก displayValidation | — | `.is-valid/.is-warning/.is-invalid` |

| `collapseAllRows()` | ย่อทุก accordion | ปุ่ม "Collapse All" | วนทุกแถว | ลบคลาส `.show` จาก collapse elements || `displayUnifiedValidationUI(row, result)` | สรุปผลต่อแถว | `validateRow*()` | — | กล่องสรุป pass/warn/error |

| `updateRowCounter()` | อัปเดตตัวนับจำนวนแถว | หลัง add/delete | — | แสดง "X rows" ใน UI || `displayGlobalSummaryUI(container, summary)` | สรุปภาพรวมทั้งหมด | หลัง validate หลายแถว | วนรวมตัวเลขหลายแถว | ใช้คงข้อความระหว่าง regen |

| `updateDeleteButtonState()` | เปิด/ปิดปุ่มลบรวม | เมื่อมีการเลือก/ยกเลิก | — | disable/enable ตามจำนวนที่เลือก || `displayGlobalValidationSummaryPreserved(summary)` | แสดงสรุปแบบ preserve | เมื่อ DOM อาจ regen | — | กันกระพริบ/ข้อความหาย |

| **📥 DROPDOWN MANAGEMENT** || `deleteBatchRow(rowId)` | ลบแถวเดียว | ปุ่มลบในแถว | — | ถอดอีเวนต์ + ลบจาก `activeRows` |

| `populateDropdownAsync(el, url, def, cb)` | โหลด dropdown options แบบ async | เมธอด updateXXX ทุกตัว | วน `data.forEach` เติม `<option>` | เคลียร์ option เดิม, ใส่ default option, อัปเดต select2 || `deleteSelectedRows()` | ลบหลายแถวที่เลือก | ปุ่มลบรวม | วนชุดที่ checked | อัปเดตตัวนับหลังลบ |

| `populatePrimaryDropdownsAsync(row, rowId, …)` | โหลดเซ็ต dropdown พื้นฐานทั้งหมด | `addBatchRow()` | วน mapping `{selector, api}` | เติมค่าเริ่มต้นตามฟิลเตอร์ (company/year/costCenter/…) || `selectAllRows()` | เลือกทุกแถว | ปุ่ม select all | วนทุกแถว | เช็กกล่องเลือก |

| `populateStaticDropdownsAsync(row)` | โหลด static dropdown แบบ async | `addBatchRow()` | วนชุด static APIs | สำหรับฟิลด์ newHC/LE ที่ไม่ขึ้นกับ cascade || `expandAllRows()` / `collapseAllRows()` | ขยาย/ย่อรายละเอียด | ปุ่ม expand/collapse | วนทุกแถว | เพิ่ม/ลบคลาส เปิด/ปิดส่วนรายละเอียด |

| `populateStaticDropdowns(row)` | โหลด static dropdown แบบ sync (fallback) | บางหน้าที่ไม่มี async | วนชุด static | สำรองในบางหน้า/เงื่อนไข || `updateRowCounter()` | อัปเดตตัวนับแถว | หลัง add/delete | — | แสดงจำนวนแถวปัจจุบัน |

| `populateRowDropdownsAsync(rowId)` | โหลด dropdowns สำหรับแถวเฉพาะ | หลังสร้างแถว | — | Wrapper สำหรับโหลด dropdowns ทั้งหมดของแถว || `updateDeleteButtonState()` | เปิด/ปิดปุ่มลบรวม | เมื่อมีการเลือก | — | disable/enable ตามจำนวนที่เลือก |

| **🔗 CASCADING DROPDOWNS** || `extractBenefitsData(row, rowId)` | ดึงค่าชุด benefits | copy flow | วนเหนือฟิลด์ benefits | คืนอ็อบเจกต์ key:value |

| `setupBatchRowCascadingRelationships(row, rowId)` | ผูกความสัมพันธ์ parent→child | `addBatchRow()` | — | change(parent) → call debounced update(child) || `applyBenefitsData(row, rowId, data, srcId)` | ใส่ค่าชุด benefits | copy flow | วน `Object.entries(data)` | เซตค่า + trigger validate |

| `updateBatchRowCostCenters(row, rowId)` | โหลด cost center ตาม company | cascade จาก company | วน options | เคลียร์ dropdown ลูก (division, dept, section) || `copyBatchRow(srcRowId)` | คัดลอกทั้งแถว | ปุ่ม Copy | — | สร้างแถวใหม่ + รอ template พร้อม |

| `updateBatchRowDivisions(row, rowId)` | โหลด division ตาม cost center | cascade จาก cost center | วน options | ใช้ company + costCenter || `copyRowData(srcRowId, dstRowId, preData?)` | คัดลอกค่าฟิลด์ทั้งหมด | `copyBatchRow()` | วนทุกฟิลด์ + benefits | อาจสั่ง regenerate benefits แล้วค่อย apply |

| `updateBatchRowDepartments(row, rowId)` | โหลด department ตาม division | cascade จาก division | วน options | ใช้ company + costCenter + division || `clearBatchRow(rowId)` | รีเซ็ตค่าในแถว | ปุ่ม Clear | วนทุกฟิลด์ | ล้างข้อความ validate ด้วย |

| `updateBatchRowSections(row, rowId)` | โหลด section ตาม department | cascade จาก department | วน options | ใช้ company + costCenter + division + dept || `cancelBatchEntry()` | ยกเลิกการป้อน | ปุ่ม Cancel | วนลบ/ล้างทั้งหน้า | คืนสู่สถานะก่อนเริ่ม |

| `updateBatchRowCompStore(row, rowId)` | โหลด comp/store ตาม section | cascade จาก section | วน options | ใช้ทุก parent ด้านบน || `resetBatchEntryForSearch()` | เคลียร์เพื่อค้นหาใหม่ | ปุ่ม Reset | วนล้างค่า/สถานะ | ปรับ UI ให้พร้อมค้นหา |

| `updateBatchRowPositions(row, rowId)` | โหลด position ตาม company/filters | cascade | วน options | — || `resetBatchEntryData()` | เคลียร์ข้อมูล batch | ขั้นตอน reset | วนเคลียร์ `activeRows` | ลบ DOM ทุกแถว |

| `updateBatchRowJobBands(row, rowId)` | โหลด job band ตาม position | cascade จาก position | วน options | — || `preserveValidationMessages(rowId)` | เก็บ snapshot ข้อความ validate | ก่อน regen DOM | วนเก็บ messages | ป้องกันข้อความหาย |

| `updateBatchRowEmployeeStatus(row, rowId)` | โหลด employee status | cascade | วน options | — || `restoreValidationMessages(rowId, preserved, delay)` | คืน snapshot ข้อความ | หลัง regen DOM | วนใส่คืนทุกฟิลด์ | มี delay สั้น ๆ ให้ DOM พร้อม |

| `updateBatchRowGroupRunRates(row, rowId)` | โหลด group run rate | cascade | วน options | — || `validateRowWithPreservation(rowId, skip?)` | validate พร้อม preserve | ใน flows ที่ regen DOM | — | รวม preserve/restore อัตโนมัติ |

| `updateBatchRowPlanCostCenters(row, rowId)` | โหลด plan cost center | cascade | วน options | — || `collectRowData(rowId)` | รวม payload ต่อแถว | ก่อน save | วนทุกอินพุต/เซเล็กต์ | รวม benefits เข้าด้วย |

| `clearBatchRowDependentDropdowns(row, level)` | ล้างค่า child dropdowns | ทุกครั้งที่ parent เปลี่ยน | วน selectors ลูกหลายตัว | รีเซ็ต `.value = ''`, trigger `change` event || `saveBatchEntry()` | ตรวจรวม + สร้าง payload | ปุ่ม Save | วน validate ทุกแถว + วน collect | ยิง API/แสดงผลสรุป (mock/real ตามหน้า) |

| **✅ VALIDATION** |

| `initializeRowValidation(row, rowId)` | เตรียม validation state สำหรับแถว | หลังสร้าง row ใน `addBatchRow()` | วนสร้าง/แนบ validation containers | เก็บ state ใน `rowValidationState` WeakMap |> หมายเหตุ: ชื่อเมธอดบางรายการอาจมีความแตกต่างเล็กน้อยตามเวอร์ชันไฟล์ของคุณ แต่โครงสร้างหน้าที่และการไหลจะสอดคล้องกับตารางนี้

| `validateFieldRealTime(field, rowId)` | ตรวจสอบฟิลด์ทันทีเมื่อเปลี่ยนค่า | input/change events | — | required/numeric/decimal → แสดงผลผ่าน UI |

| `validateRow(rowId)` | ตรวจสอบทั้งแถว (quick validation) | Manual trigger / ก่อน save | วนทุกฟิลด์ใน row | ส่งคืนผลรวมเบื้องต้น (pass/warn/error counts) |## Usage Notes

| `validateRowComplete(rowId)` | ตรวจสอบทั้งแถว (full validation) | ก่อน save / manual | วนทุกฟิลด์ + benefits | รวมเป็น pass/warn/error พร้อม messages |- จุดที่มี `cascade` มักจะเรียก “เวอร์ชันดีบาวซ์” ของเมธอด (ลด API call ถี่)  

| `validateBenefitsFields(row, companyId)` | ตรวจสอบ benefits fields ไดนามิก | ระหว่าง `validateRow*()` | วนรายการ config ของบริษัท | รองรับ BJC/BIGC templates ต่างกัน |- การคัดลอกแถวพึ่งพา “การ generate benefits template ให้พร้อมก่อน” แล้วจึง apply ค่า (แนะนำปรับเป็น Promise + MutationObserver เพื่อลด race)  

| `validateAllRows()` | ตรวจสอบทุกแถวพร้อมกัน | ปุ่ม "Validate All" / ก่อน save | วนทุก active rows | รวมผลเป็น global summary |- การแสดงผล validate ใช้คลาส `.is-valid/.is-warning/.is-invalid` ให้สอดคล้องกับ CSS ใหม่ใน parallel folder

| `displayValidation(field, opts)` | แสดงผล validation บนฟิลด์ | จาก validate methods | — | เรียก render/apply styling + update summary |

| `applyValidationStyling(field, type, warn, opts)` | ใส่คลาสสถานะ validation | จาก `displayValidation()` | — | `.is-valid/.is-warning/.is-invalid` |
| `displayUnifiedValidationUI(row, result)` | แสดงสรุปผล validation ต่อแถว | `validateRow*()` | — | กล่องสรุป pass/warn/error ใต้แต่ละแถว |
| `displayGlobalSummaryUI(container, summary)` | แสดงสรุปภาพรวมทั้งหมด | หลัง validate หลายแถว | วนรวมตัวเลขจากทุกแถว | แสดงผลรวม errors/warnings/valid |
| `displayGlobalValidationSummaryPreserved(summary)` | แสดงสรุป validation แบบ preserve | เมื่อ DOM อาจ regenerate | — | กันกระพริบ/ข้อความหายเมื่อ DOM update |
| `preserveValidationMessages(rowId)` | เก็บ snapshot validation messages | ก่อน regenerate DOM | วนเก็บ messages จากทุกฟิลด์ | ป้องกันข้อความหายเมื่อ template regenerate |
| `restoreValidationMessages(rowId, preserved, delay)` | คืน snapshot validation messages | หลัง regenerate DOM | วนใส่คืนทุกฟิลด์ | มี delay สั้น ๆ ให้ DOM พร้อม |
| `validateRowWithPreservation(rowId, skip?)` | Validate พร้อม preserve/restore messages | ใน flows ที่ regenerate DOM | — | รวม preserve/restore อัตโนมัติ |
| **📋 COPY ROW (FIXED 2025-10-19)** |
| `copyBatchRow(sourceRowId)` | 🔥 **REWRITTEN** คัดลอกทั้งแถวแบบ 9-step | ปุ่ม Copy Row | — | ✅ **FIX:** ใช้ row-specific extraction + Promise chain + wait for template generation |
| `getBenefitsFieldsForRow(rowIndex)` | ดึงรายการ benefits fields ของแถว | `copyBatchRow()` Step 1 | วนหา `batchLe*_` และ `batchBg*_` | ค้นหาภายใน specific row element |
| `extractBenefitsFieldData(sourceRowIndex)` | ดึงค่า benefits fields เป็น Map | `copyBatchRow()` Step 1 | วนทุก LE + BG fields | ✅ **FIX:** ใช้ `querySelectorAll(\`[id^="batchLe${rowIndex}_"]\`)` แทน global search |
| `applyBenefitsFieldData(targetRowIndex, fieldDataMap)` | Apply ค่า benefits ไปแถวเป้าหมาย | `copyBatchRow()` Step 8 | วน `Map.forEach()` | ✅ **FIX:** ใช้ `getElementById()` แทน global selector, trigger events |
| `waitForBenefitsFields(targetRowIndex, companyID)` | 🗑️ **DEPRECATED** รอ benefits fields พร้อม | (ไม่ใช้แล้ว) | — | ⚠️ Replaced by Promise + setTimeout in main flow |
| **🔧 ROW UTILITIES** |
| `clearBatchRow(rowId)` | รีเซ็ตค่าในแถว | ปุ่ม Clear Row | วนทุก input/select | ล้างค่า + validation messages |
| `cancelBatchEntry()` | ยกเลิกการป้อน batch ทั้งหมด | ปุ่ม Cancel | วนลบ/ล้างทุกแถว | คืนสู่สถานะว่าง |
| `resetBatchEntryForSearch()` | รีเซ็ตเพื่อค้นหาใหม่ | ปุ่ม Reset | วนล้างค่า/สถานะ | เก็บ filters, ล้างแถว |
| `resetBatchEntryData()` | เคลียร์ข้อมูล batch ทั้งหมด | `cancelBatchEntry()`, `resetBatchEntryForSearch()` | วนเคลียร์ `activeRows` | ลบ DOM ทุกแถว + รีเซ็ต state |
| **💾 SAVE & COLLECT** |
| `collectRowData(rowId)` | รวม payload สำหรับแถวเดียว | `saveBatchEntry()` | วนทุก input/select + benefits | สร้าง object พร้อม primary + benefits data |
| `saveBatchEntry()` | ตรวจสอบ + สร้าง payload + save | ปุ่ม Save | วน validate ทุกแถว → วน collect data | ยิง API (mock/real), แสดงผลสรุป |
| **🎨 UI MANAGEMENT** |
| `showBatchEntryLoading(message)` | แสดง loading overlay + message | ก่อน async operations | — | แสดง spinner + custom message |
| `hideBatchEntryLoading()` | ซ่อน loading overlay | หลัง async operations | — | ซ่อน spinner |
| `updateBatchEntryLoadingMessage(message)` | อัปเดตข้อความ loading | ระหว่าง multi-step operations | — | เปลี่ยนข้อความโดยไม่ซ่อน overlay |
| **🛠️ HELPER METHODS** |
| `renderBasicValidation(field, opts)` | Render basic validation UI | `displayValidation()` | — | สำหรับ simple validation |
| `renderEnhancedValidation(field, opts)` | Render enhanced validation UI | `displayValidation()` | — | สำหรับ complex validation with details |
| `displayFieldValidation(field, config, isValid, type)` | Legacy field validation display | Legacy code paths | — | Backward compatibility |
| `displayEnhancedValidation(field, config, isValid, type, result)` | Legacy enhanced validation | Legacy code paths | — | Backward compatibility |
| `applyFieldStylingOnly(field, type, warningType)` | Apply CSS classes only (no messages) | Direct styling needs | — | `.is-valid/.is-warning/.is-invalid` |
| `clearFieldValidation(field)` | ล้าง validation state ของฟิลด์ | Clear operations | — | ลบคลาส + messages |
| `applyLegacyEnhancedValidation(field, config, isValid, type, result)` | Legacy enhanced validation logic | Legacy paths | — | Backward compatibility |
| `displaySummaryValidation(row, validation)` | แสดงสรุป validation (legacy) | Legacy paths | — | Backward compatibility |
| `setupAutoPopulateRules(row, rowId)` | ตั้งค่า auto-populate rules | `addBatchRow()` | — | Auto-fill dependent fields |
| `createSummaryContainer(row)` | สร้าง container สำหรับ validation summary | `initializeRowValidation()` | — | เพิ่ม DOM element สำหรับแสดงผล |
| `getFieldSelector(element)` | สร้าง CSS selector สำหรับฟิลด์ | Validation helpers | — | ใช้ในการ debug/logging |

---

## 🔧 Global Functions (นอก batchEntryManager)

| Function | Purpose | Triggers | Notes |
|---|---|---|---|
| `updateCardYears()` | อัปเดตตัวเลือกปีในการ์ด | Page load | Populate year dropdowns |
| `checkModuleDependencies()` | ตรวจสอบ module dependencies | Initialization | Verify required scripts loaded |
| `handleEditButtonClick(e)` | จัดการปุ่ม Edit | Click on Edit button | Open edit form with row data |
| `handleToggleMasterGridClick(e)` | Toggle master grid visibility | Click toggle button | Show/hide master data grid |
| `handleSearchClick()` | ค้นหาข้อมูล budget | ปุ่ม Search | Load budget data with filters |
| `handleResetClick()` | รีเซ็ตฟอร์มค้นหา | ปุ่ม Reset | Clear search filters |
| `handleCompanyFilterChange()` | จัดการเมื่อเปลี่ยน company filter | Company dropdown change | Update dependent dropdowns |
| `initializeUIState()` | ตั้งค่า UI state เริ่มต้น | Page load | Set initial UI state |
| `bindEventListeners()` | ผูก global event listeners | Page load | Bind search/filter events |
| `populateEditForm(rowData)` | Populate edit form with data | Edit button click | Fill form fields from row |
| `initializeEnhancedValidation()` | Initialize enhanced validation system | Page load | Setup validation framework |
| `setupEnhancedValidationConfig()` | Setup validation configuration | Validation init | Configure validation rules |
| `determineValidationType(field)` | กำหนดประเภท validation | During validation | Determine validation type for field |
| `isZeroOrNegativeValue(value)` | ตรวจสอบค่า zero/negative | Validation checks | Helper for numeric validation |
| `getValidationWarningType(field)` | กำหนดประเภท warning | Validation display | Determine warning severity |
| `bindEnhancedValidationNative()` | Bind native validation events | Validation init | Attach native browser validation |
| `shouldValidateField(field)` | ตรวจสอบว่าควร validate ไหม | Before validation | Filter fields for validation |
| `applyDirectValidation(field, type, warningType)` | Apply validation directly to field | Direct validation calls | Quick validation application |
| `initializeUnifiedValidationTooltips()` | Initialize Bootstrap tooltips | Page load | Setup tooltips for validation |

---

## 🐛 Bug Fix History

### 2025-10-19: Copy Row Benefits Fields Fix

**Problem:**  
Benefits fields ของ Row 2 ไปปรากฏใน Row 1 accordion (fields ซ้ำกัน)

**Root Cause:**  
jQuery selector `$('.batch-row-le-benefit-form .row')` ใน `budget.plan.benefits.templates.js` → หา element **แรกสุด** ใน document เสมอ → ทำให้ fields ของ Row 2 ถูกสร้างใน Row 1

**Solution Applied:**
```javascript
// ❌ BEFORE (Wrong):
const container = $('.batch-row-le-benefit-form .row'); // Always finds Row 1

// ✅ AFTER (Fixed):
const rowElement = document.querySelector(`[data-batch-row-id="batch-row-${batchRowIndex}"]`);
const container = $(rowElement).find('.batch-row-le-benefit-form .row'); // Row-specific
```

**Files Changed:**
1. `budget.plan.benefits.templates.js`:
   - `generateBatchLeBenefitsForm()` → ✅ Fixed with row-specific container
   - `generateBatchBgBenefitsForm()` → ✅ Fixed with row-specific container

2. `budget.plan.events.js`:
   - `copyBatchRow()` → ✅ Completely rewritten with 9-step Promise chain
   - `extractBenefitsFieldData()` → ✅ Uses row-specific selectors
   - `applyBenefitsFieldData()` → ✅ Uses `getElementById()` for exact targeting

**Result:**  
✅ Benefits fields now generate in correct row  
✅ Copy Row works correctly  
✅ No field duplication  

---

## 💡 Usage Notes

### Cascading Dropdowns
- ทุก cascade จะเรียกเวอร์ชัน **debounced** เพื่อลด API calls
- Chain: Company → Cost Center → Division → Department → Section → Comp/Store
- เมื่อ parent เปลี่ยน → `clearBatchRowDependentDropdowns()` ถูกเรียกก่อน

### Copy Row Flow (Updated 2025-10-19)
```
1. Validate source row exists + has company
2. Extract Primary fields → Map
3. Extract Benefits fields (LE + BG) → Map
4. Create new row (addBatchRow)
5. Copy Primary fields → trigger events
6. Clean old Benefits fields (if any)
7. Generate Benefits template (company-specific)
8. Wait 500ms for template render
9. Copy Benefits field values → trigger events
```

### Validation System
- **Real-time:** `validateFieldRealTime()` triggered on input/change
- **Row-level:** `validateRow()` (quick) vs `validateRowComplete()` (full)
- **Global:** `validateAllRows()` → aggregate summary
- **Classes:** `.is-valid`, `.is-warning`, `.is-invalid`
- **Benefits:** Company-specific validation based on BJC/BIGC config

### Memory Management
- Uses **WeakMaps** for automatic garbage collection
- Event listeners stored in WeakMap → auto-cleanup when row removed
- Validation state stored in WeakMap → no memory leaks

### Performance
- Debouncing on all async dropdown updates (300ms default)
- Lazy loading of benefits templates (only when company selected)
- Incremental field generation (only new rows, not regenerating existing)

---

## 🔗 Related Files

- `budget.plan.benefits.templates.js` → Benefits template generation (FIXED 2025-10-19)
- `budget.plan.validation.js` → Extended validation rules
- `BudgetPlanning.cshtml` → HTML template with `data-batch-row-id` attributes
- `budget.plan.css` → Validation styling (`.is-valid`, `.is-warning`, `.is-invalid`)

---

**Version:** 2.0.1  
**Last Bug Fix:** Copy Row Benefits Field Duplication (2025-10-19)  
**Status:** ✅ Production Ready
