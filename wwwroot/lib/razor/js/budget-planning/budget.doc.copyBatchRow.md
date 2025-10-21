
copyBatchRow: function (sourceRowId) {
    console.log(`━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━`);
    console.log(`🎯 COPY ROW: ${sourceRowId}`);
    console.log(`━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━`);

    // ═══════════════════════════════════════════════════════════════
    // STEP 1: Validate source row
    // ═══════════════════════════════════════════════════════════════
    const sourceElement = document.querySelector(`[data-batch-row-id="${sourceRowId}"]`);
    if (!sourceElement) {
      console.error('❌ Source row not found');
      alert('Error: Source row not found');
      return Promise.reject('Source row not found');
    }

    const sourceRowIndex = parseInt(sourceRowId.replace('batch-row-', ''));
    console.log(`📍 Source row index: ${sourceRowIndex}`);

    // ═══════════════════════════════════════════════════════════════
    // STEP 2: Get company ID (required for benefits template)
    // ═══════════════════════════════════════════════════════════════
    const companyField = sourceElement.querySelector('.batch-company');
    const companyID = companyField?.value;

    if (!companyID) {
      console.error('❌ Company not selected');
      alert('Cannot copy row: Company not selected');
      return Promise.reject('Company not selected');
    }

    console.log(`🏢 Company ID: ${companyID}`);

    // ═══════════════════════════════════════════════════════════════
    // STEP 3: Extract ALL data from source row
    // ═══════════════════════════════════════════════════════════════
    console.log(`📊 Extracting source row data...`);

    // 3a. Extract Primary DDL fields
    const sourceFields = sourceElement.querySelectorAll('input:not([id^="batchLe"]):not([id^="batchBg"]), select:not([id^="batchLe"]):not([id^="batchBg"]), textarea:not([id^="batchLe"]):not([id^="batchBg"])');
    const primaryFieldData = new Map();

    sourceFields.forEach(field => {
      if (!field.name) return;
      const baseName = field.name.replace(`${sourceRowId}_`, '');

      if (field.tagName === 'SELECT') {
        primaryFieldData.set(baseName, {
          type: 'select',
          value: field.value,
          selectedIndex: field.selectedIndex,
          text: field.options[field.selectedIndex]?.text || ''
        });
      } else {
        primaryFieldData.set(baseName, {
          type: 'input',
          value: field.value
        });
      }
    });

    console.log(`  ✅ Primary fields: ${primaryFieldData.size}`);

    // 3b. Extract Benefits fields
    const benefitsFieldData = this.extractBenefitsFieldData(sourceRowIndex);
    console.log(`  ✅ Benefits fields: ${benefitsFieldData.size}`);

    // ═══════════════════════════════════════════════════════════════
    // STEP 4: Set copy operation flags
    // ═══════════════════════════════════════════════════════════════
    this.isCopyingRow = true;
    this.copySourceRowId = sourceRowId;

    // ═══════════════════════════════════════════════════════════════
    // STEP 5: Create new row
    // ═══════════════════════════════════════════════════════════════
    console.log(`➕ Creating new row...`);

    // 🔧 FIX: Declare counters OUTSIDE Promise chain for proper scope access
    let copiedPrimaryCount = 0;
    let copiedBenefitsCount = 0;
    let failedBenefitsCount = 0;
    const failedBenefitsFields = [];

    return this.addBatchRow().then(() => {
      const targetRowId = `batch-row-${this.nextRowId - 1}`;
      const targetRowIndex = this.nextRowId - 1;
      const targetElement = document.querySelector(`[data-batch-row-id="${targetRowId}"]`);

      if (!targetElement) {
        console.error('❌ Target row element not found');
        this.isCopyingRow = false;
        this.copySourceRowId = null;
        return Promise.reject('Target row element not found');
      }

      console.log(`✅ Target row created: ${targetRowId} (index: ${targetRowIndex})`);

      // ═══════════════════════════════════════════════════════════════
      // STEP 6: Copy Primary DDL fields IMMEDIATELY
      // ═══════════════════════════════════════════════════════════════
      console.log(`📋 Copying ${primaryFieldData.size} primary fields...`);

      const targetFields = targetElement.querySelectorAll('input, select, textarea');

      targetFields.forEach(field => {
        if (!field.name) return;
        const baseName = field.name.replace(`${targetRowId}_`, '');
        const sourceData = primaryFieldData.get(baseName);

        if (!sourceData) return;

        try {
          if (sourceData.type === 'select' && field.tagName === 'SELECT') {
            if (sourceData.selectedIndex >= 0 && sourceData.selectedIndex < field.options.length) {
              field.selectedIndex = sourceData.selectedIndex;

              // Trigger Select2 change
              if (window.jQuery && window.jQuery(field).data('select2')) {
                window.jQuery(field).trigger('change.select2');
              } else {
                field.dispatchEvent(new Event('change', { bubbles: true }));
              }

              copiedPrimaryCount++;
            }
          } else if (sourceData.type === 'input') {
            field.value = sourceData.value || '';
            field.dispatchEvent(new Event('input', { bubbles: true }));
            copiedPrimaryCount++;
          }
        } catch (err) {
          console.warn(`⚠️ Error copying field ${baseName}:`, err);
        }
      });

      console.log(`  ✅ Primary fields copied: ${copiedPrimaryCount}/${primaryFieldData.size}`);

      // ═══════════════════════════════════════════════════════════════
      // STEP 6.5: Clean & Generate Benefits template for target row
      // ═══════════════════════════════════════════════════════════════
      console.log(`🏗️ [Clean Copy] Preparing benefits template for row ${targetRowIndex}...`);

      // 🧹 STEP 6.5A: Remove any existing fields for target row (cleanup duplicates)
      console.log(`🧹 [Clean Copy] Removing any existing fields for row ${targetRowIndex}...`);

      const existingLeFields = document.querySelectorAll(`[id^="batchLe${targetRowIndex}_"]`);
      const existingBgFields = document.querySelectorAll(`[id^="batchBg${targetRowIndex}_"]`);

      console.log(`🧹 [Clean Copy] Found ${existingLeFields.length} LE + ${existingBgFields.length} BG fields to remove`);

      existingLeFields.forEach(field => {
        const parentCol = field.closest('.col-md-4, .col-md-6, .col-12, [class*="col-"]');
        if (parentCol) {
          parentCol.remove();
          console.log(`  🗑️ Removed LE field: ${field.id}`);
        }
      });

      existingBgFields.forEach(field => {
        const parentCol = field.closest('.col-md-4, .col-md-6, .col-12, [class*="col-"]');
        if (parentCol) {
          parentCol.remove();
          console.log(`  🗑️ Removed BG field: ${field.id}`);
        }
      });

      console.log(`✅ [Clean Copy] Cleanup completed - container is ready for fresh fields`);

      // 🏗️ STEP 6.5B: Generate fresh template
      // Use Promise-based waiting for template generation
      return new Promise((resolve, reject) => {
        if (typeof window.generateBatchTemplatesForCompany === 'function') {
          console.log(`  📞 [Clean Copy] Calling generateBatchTemplatesForCompany(${companyID}, ${targetRowIndex})`);

          // Call with callback that resolves the Promise
          window.generateBatchTemplatesForCompany(companyID, targetRowIndex, () => {
            console.log(`  ✅ [Clean Copy] Template generation callback executed`);
            resolve();
          });
        } else {
          console.error(`❌ generateBatchTemplatesForCompany function not found!`);
          reject(new Error('Template generation function not found'));
        }
      });

    }).then(() => {
      const targetRowId = `batch-row-${this.nextRowId - 1}`;
      const targetRowIndex = this.nextRowId - 1;
      const targetElement = document.querySelector(`[data-batch-row-id="${targetRowId}"]`);

      // ═══════════════════════════════════════════════════════════════
      // STEP 7: Copy Benefits fields directly (no template regeneration)
      // ═══════════════════════════════════════════════════════════════
      console.log(`📋 Copying ${benefitsFieldData.size} benefits fields directly...`);

      // 🔍 DEBUG: Check if target row benefits fields exist
      const targetLeFields = document.querySelectorAll(`[id^="batchLe${targetRowIndex}_"]`);
      const targetBgFields = document.querySelectorAll(`[id^="batchBg${targetRowIndex}_"]`);
      console.log(`🔍 [DEBUG] Target row ${targetRowIndex} has ${targetLeFields.length} LE fields + ${targetBgFields.length} BG fields`);

      if (targetLeFields.length > 0) {
        console.log(`🔍 [DEBUG] Sample LE fields for row ${targetRowIndex}:`);
        targetLeFields.forEach((field, idx) => {
          if (idx < 3) { // Show first 3
            console.log(`  - id="${field.id}", name="${field.name || 'N/A'}", type="${field.type || field.tagName}"`);
          }
        });
      }

      // 🔧 FIX: Remove duplicate declarations - variables already declared in STEP 5
      // Copy Benefits fields (LE + BG)
      benefitsFieldData.forEach((sourceData, fieldBaseName) => {
        // Construct target field ID based on field type
        let targetFieldId;

        if (sourceData.isLeField) {
          // LE field: editLePayroll → batchLe2_editLePayroll
          targetFieldId = `batchLe${targetRowIndex}_${fieldBaseName}`;
        } else if (sourceData.isBgField) {
          // BG field: editBgBonus → batchBg2_editBgBonus
          targetFieldId = `batchBg${targetRowIndex}_${fieldBaseName}`;
        } else {
          console.warn(`⚠️ Unknown field type for ${fieldBaseName}`);
          failedBenefitsCount++;
          failedBenefitsFields.push(fieldBaseName);
          return;
        }

        const targetField = document.getElementById(targetFieldId);

        if (!targetField) {
          console.warn(`⚠️ Target benefits field not found: ${targetFieldId}`);
          failedBenefitsCount++;
          failedBenefitsFields.push(targetFieldId);
          return;
        }

        try {
          if (sourceData.type === 'select' && targetField.tagName === 'SELECT') {
            // Copy select field
            if (sourceData.selectedIndex >= 0 && sourceData.selectedIndex < targetField.options.length) {
              targetField.selectedIndex = sourceData.selectedIndex;

              // Trigger Select2 if exists
              if (window.jQuery && window.jQuery(targetField).data('select2')) {
                window.jQuery(targetField).trigger('change.select2');
              } else {
                targetField.dispatchEvent(new Event('change', { bubbles: true }));
              }

              copiedBenefitsCount++;
              console.log(`  ✅ ${targetFieldId} = option[${sourceData.selectedIndex}]`);
            }
          } else if (sourceData.type === 'input' || sourceData.type === 'textarea' || sourceData.type === 'number') {
            // Copy input/textarea/number field
            targetField.value = sourceData.value || '';
            targetField.dispatchEvent(new Event('input', { bubbles: true }));
            copiedBenefitsCount++;
            console.log(`  ✅ ${targetFieldId} = "${sourceData.value}"`);
          }
        } catch (err) {
          console.error(`❌ Error copying benefits field ${targetFieldId}:`, err);
          failedBenefitsCount++;
          failedBenefitsFields.push(targetFieldId);
        }
      });

      console.log(`  ✅ Benefits fields copied: ${copiedBenefitsCount}/${benefitsFieldData.size}`);

      // ═══════════════════════════════════════════════════════════════
      // FINAL SUMMARY
      // ═══════════════════════════════════════════════════════════════
      console.log(`━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━`);
      console.log(`📊 COPY SUMMARY:`);
      console.log(`  Primary Fields: ${copiedPrimaryCount}/${primaryFieldData.size}`);
      console.log(`  Benefits Fields: ${copiedBenefitsCount}/${benefitsFieldData.size}`);
      console.log(`  Total Failed: ${failedBenefitsCount}`);

      if (failedBenefitsCount > 0) {
        console.warn(`  ⚠️ Failed benefits fields:`, failedBenefitsFields);
      }

      console.log(`✅ COPY COMPLETED: ${sourceRowId} → ${targetRowId}`);
      console.log(`━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━`);

      // Reset copy flags
      this.isCopyingRow = false;
      this.copySourceRowId = null;

      return Promise.resolve({
        success: true,
        targetRowId,
        copiedPrimaryCount,
        copiedBenefitsCount,
        totalFailed: failedBenefitsCount,
        failedFields: failedBenefitsFields
      });

    }).catch(error => {
      console.error('❌ Error during copy operation:', error);
      this.isCopyingRow = false;
      this.copySourceRowId = null;
      return Promise.reject(error);
    });
  },
