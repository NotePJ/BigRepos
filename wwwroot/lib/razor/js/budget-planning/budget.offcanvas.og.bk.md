/**
 * Budget Offcanvas Functions
 * Handles offcanvas operations, form management, and dropdown cascading
 */

// Populate all Offcanvas dropdowns from API
function populateOffcanvasDropdowns() {
  const companyID = document.getElementById('editCompany')?.value || document.getElementById('companyFilter')?.value || '';

  console.log('populateOffcanvasDropdowns called with company:', companyID);

  // Companies: { CompanyCode, CompanyId }
  // Always call B0Companies without companyID
  populateDropdown('editCompany', BUDGET_API.companies, 'Select Company', (option, item) => {
    option.value = item.companyId;
    option.textContent = item.companyCode;
  }, true);

  // Setup cascading dropdown relationships
  setupOffcanvasDropdownRelationships();

  // Initial population based on current company
  if (companyID) {
    updateOffcanvasEmpFormats(companyID);
    updateOffcanvasYears(companyID);
    updateOffcanvasNewPeriod();
    updateOffcanvasNOfMonth();
    updateOffcanvasNewLEPeriod();
    updateOffcanvasLEnOfMonth();
    updateOffcanvasNewLEnOfMonth();
    updateOffcanvasJobBands();
  }

  // Initialize allocation management if function exists - delay to ensure DOM is ready
  setTimeout(() => {
    if (typeof initializeAllocationManagement === 'function') {
      console.log('Initializing allocation management from populateOffcanvasDropdowns...');
      initializeAllocationManagement();
    } else {
      console.warn('initializeAllocationManagement function not found');
    }
  }, 300);
}

// Setup cascading relationships for Add Row mode
function setupOffcanvasDropdownRelationshipsForAdd() {
  // Remove existing event listeners to prevent duplicates
  $('#editCompany').off('change.offcanvas');
  $('#editFormat').off('change.offcanvas');
  $('#editYear').off('change.offcanvas');
  $('#editCostCenter').off('change.offcanvas');
  $('#editDivision').off('change.offcanvas');
  $('#editDepartment').off('change.offcanvas');
  $('#editSection').off('change.offcanvas');
  $('#editLocation').off('change.offcanvas');
  $('#editPosition').off('change.offcanvas');

  // Company change - update all dependent dropdowns sequentially
  $('#editCompany').on('change.offcanvas', function () {
    const companyID = $(this).val();
    if (companyID) {
      // Update Format and Year first (immediate)
      updateOffcanvasEmpFormats(companyID);
      updateOffcanvasYears(companyID);

      // Use debounced functions for other APIs to prevent rapid calls
      console.log('🔄 Calling debouncedUpdateOffcanvasEmpStatus for companyID:', companyID);
      debouncedUpdateOffcanvasEmpStatus();
      debouncedUpdateOffcanvasCostCenters();
      debouncedUpdateOffcanvasEmployeeTypes();
      debouncedUpdateOffcanvasNewHC();
      debouncedUpdateOffcanvasNewPeriod();
      debouncedUpdateOffcanvasNOfMonth();
      debouncedUpdateOffcanvasNewLEPeriod();
      debouncedUpdateOffcanvasLEnOfMonth();
      debouncedUpdateOffcanvasNewLEnOfMonth();
      debouncedUpdateOffcanvasDivisions();
      debouncedUpdateOffcanvasDepartments();
      debouncedUpdateOffcanvasSections();
      debouncedUpdateOffcanvasLocations();
      debouncedUpdateOffcanvasPositions();
      debouncedUpdateOffcanvasJobBands();
    } else {
      clearOffcanvasDropdowns(['editFormat', 'editYear', 'editCostCenter', 'editDivision', 'editDepartment', 'editSection', 'editLocation', 'editEmpStatus', 'editEmpType', 'editNewHC', 'editNewPeriod', 'editNOfMonth', 'editNewLEPeriod', 'editLEnOfMonth', 'editNewLEnOfMonth', 'editPosition', 'editJobBand']);
    }
  });

  // Format/Year change - update dependent dropdowns
  $('#editFormat, #editYear').on('change.offcanvas', function () {
    const companyID = $('#editCompany').val();
    if (companyID) {
      debouncedUpdateOffcanvasCostCenters();
      debouncedUpdateOffcanvasDivisions();
      debouncedUpdateOffcanvasDepartments();
      debouncedUpdateOffcanvasSections();
      debouncedUpdateOffcanvasLocations();
      debouncedUpdateOffcanvasPositions();
      debouncedUpdateOffcanvasJobBands();
    }
  });

  // Cost Center change
  $('#editCostCenter').on('change.offcanvas', function () {
    // 1. Update dropdowns first
    debouncedUpdateOffcanvasDivisions();
    debouncedUpdateOffcanvasDepartments();
    debouncedUpdateOffcanvasSections();
    debouncedUpdateOffcanvasLocations();
    debouncedUpdateOffcanvasPositions();
    debouncedUpdateOffcanvasJobBands();

    // 2. Allocation card show/hide logic (custom event)
    const selectedValue = this.value;
    const allocationCard = document.getElementById('budgetAllocationCard');
    const allocationContainer = document.getElementById('allocationContainer');
    if (allocationCard) {
      if (selectedValue === AllocationCostCenterCode) {
        allocationCard.classList.remove('d-none');
        // If allocationContainer is empty, trigger allocation row add
        if (allocationContainer && allocationContainer.children.length === 0) {
          // Notify allocation module to add row
          const event = new CustomEvent('allocationCardVisible', { detail: { costCenter: selectedValue } });
          window.dispatchEvent(event);
        }
      } else {
        allocationCard.classList.add('d-none');
        // Clear allocation data when hiding
        if (allocationContainer) {
          allocationContainer.innerHTML = '';
          // Notify allocation module to reset
          const event = new CustomEvent('allocationCardHidden', { detail: { costCenter: selectedValue } });
          window.dispatchEvent(event);
        }
      }
    }
  });

  // Division change
  $('#editDivision').on('change.offcanvas', function () {
    debouncedUpdateOffcanvasDepartments();
    debouncedUpdateOffcanvasSections();
    debouncedUpdateOffcanvasLocations();
    debouncedUpdateOffcanvasPositions();
    debouncedUpdateOffcanvasJobBands();
  });

  // Department change
  $('#editDepartment').on('change.offcanvas', function () {
    debouncedUpdateOffcanvasSections();
    debouncedUpdateOffcanvasLocations();
    debouncedUpdateOffcanvasPositions();
    debouncedUpdateOffcanvasJobBands();
  });

  // Section change
  $('#editSection').on('change.offcanvas', function () {
    debouncedUpdateOffcanvasLocations();
    debouncedUpdateOffcanvasPositions();
    debouncedUpdateOffcanvasJobBands();
  });

  // Location change
  $('#editLocation').on('change.offcanvas', function () {
    debouncedUpdateOffcanvasPositions();
    debouncedUpdateOffcanvasJobBands();
  });

  // Position change
  $('#editPosition').on('change.offcanvas', function () {
    debouncedUpdateOffcanvasJobBands();
  });

  console.log('Offcanvas dropdown relationships for Add mode set up successfully');
}

// Setup cascading relationships for Edit Row mode
function setupOffcanvasDropdownRelationshipsForEdit() {
  // Similar to Add mode but may have different behavior for edit
  setupOffcanvasDropdownRelationshipsForAdd();
  console.log('Offcanvas dropdown relationships for Edit mode set up successfully');
}

/**
 * Setup cascading relationships between offcanvas dropdowns (Legacy - maintained for compatibility)
 */
function setupOffcanvasDropdownRelationships() {
  const formMode = document.getElementById('formMode')?.value;

  if (formMode === 'edit') {
    setupOffcanvasDropdownRelationshipsForEdit();
  } else {
    setupOffcanvasDropdownRelationshipsForAdd();
  }
}

// Clear specified offcanvas dropdowns
function clearOffcanvasDropdowns(dropdownIds) {
  dropdownIds.forEach(id => {
    const dropdown = document.getElementById(id);
    if (dropdown) {
      dropdown.innerHTML = '<option value="">Select...</option>';
      // Update select2 if initialized
      if ($(dropdown).hasClass('select2-hidden-accessible')) {
        $(dropdown).trigger('change');
      }
    }
  });
}

// Create debounced versions of offcanvas update functions
const debouncedUpdateOffcanvasCostCenters = debounce(updateOffcanvasCostCenters, DEBOUNCE_DELAYS.offcanvasCostCenters);
const debouncedUpdateOffcanvasDivisions = debounce(updateOffcanvasDivisions, DEBOUNCE_DELAYS.offcanvasDivisions);
const debouncedUpdateOffcanvasDepartments = debounce(updateOffcanvasDepartments, DEBOUNCE_DELAYS.offcanvasDepartments);
const debouncedUpdateOffcanvasSections = debounce(updateOffcanvasSections, DEBOUNCE_DELAYS.offcanvasSections);
const debouncedUpdateOffcanvasLocations = debounce(updateOffcanvasLocations, DEBOUNCE_DELAYS.offcanvasLocations);
const debouncedUpdateOffcanvasPositions = debounce(updateOffcanvasPositions, DEBOUNCE_DELAYS.offcanvasPositions);
const debouncedUpdateOffcanvasJobBands = debounce(updateOffcanvasJobBands, DEBOUNCE_DELAYS.offcanvasJobBands);
const debouncedUpdateOffcanvasEmpStatus = debounce(updateOffcanvasEmpStatus, DEBOUNCE_DELAYS.offcanvasEmpStatus);
const debouncedUpdateOffcanvasEmployeeTypes = debounce(updateOffcanvasEmployeeTypes, DEBOUNCE_DELAYS.offcanvasEmployeeTypes);
const debouncedUpdateOffcanvasNewHC = debounce(updateOffcanvasNewHC, DEBOUNCE_DELAYS.offcanvasNewHC);
const debouncedUpdateOffcanvasNewPeriod = debounce(updateOffcanvasNewPeriod, DEBOUNCE_DELAYS.offcanvasNewPeriod);
const debouncedUpdateOffcanvasNOfMonth = debounce(updateOffcanvasNOfMonth, DEBOUNCE_DELAYS.offcanvasNOfMonth);
const debouncedUpdateOffcanvasNewLEPeriod = debounce(updateOffcanvasNewLEPeriod, DEBOUNCE_DELAYS.offcanvasNewLEPeriod);
const debouncedUpdateOffcanvasLEnOfMonth = debounce(updateOffcanvasLEnOfMonth, DEBOUNCE_DELAYS.offcanvasLEnOfMonth);
const debouncedUpdateOffcanvasNewLEnOfMonth = debounce(updateOffcanvasNewLEnOfMonth, DEBOUNCE_DELAYS.offcanvasNewLEnOfMonth);

// Update Employee Formats dropdown
function updateOffcanvasEmpFormats(companyID) {
  if (!companyID) return;

  populateDropdown('editFormat', `${BUDGET_API.empFormats}?companyID=${companyID}`, 'Select Format', (option, item) => {
    option.value = item;
    option.textContent = item;
  });
}

// Update Years dropdown
function updateOffcanvasYears(companyID) {
  let yearParams = [];
  if (companyID) yearParams.push(`companyID=${companyID}`);
  const selectedFormat = document.getElementById('editFormat')?.value;
  if (selectedFormat) yearParams.push(`empFormat=${encodeURIComponent(selectedFormat)}`);
  const yearQuery = yearParams.length ? `?${yearParams.join('&')}` : '';

  populateDropdown('editYear', `${BUDGET_API.budgetYears}${yearQuery}`, 'Select Year', (option, item) => {
    option.value = item;
    option.textContent = item;
  }, true);
}

// Update Cost Centers dropdown
function updateOffcanvasCostCenters() {
  const companyID = document.getElementById('editCompany')?.value;
  const selectedFormat = document.getElementById('editFormat')?.value;
  const selectedYear = document.getElementById('editYear')?.value;

  let costCenterParams = [];
  if (companyID) costCenterParams.push(`companyID=${companyID}`);
  if (selectedFormat) costCenterParams.push(`empFormat=${encodeURIComponent(selectedFormat)}`);
  if (selectedYear) costCenterParams.push(`budgetYear=${encodeURIComponent(selectedYear)}`);
  const costCenterQuery = costCenterParams.length ? `?${costCenterParams.join('&')}` : '';

  populateDropdown('editCostCenter', `${BUDGET_API.costCenters}${costCenterQuery}`, 'Select Cost Center', (option, item) => {
    option.value = item.costCenterCode;
    option.textContent = `${item.costCenterName} (${item.costCenterCode})`;
  });
}

// Update Employee Status dropdown
function updateOffcanvasEmpStatus() {

  const companyID = document.getElementById('editCompany')?.value;
  const formMode = document.getElementById('formMode')?.value;

  if (formMode === 'edit') {
    const selectedFormat = document.getElementById('editFormat')?.value;
    const selectedYear = document.getElementById('editYear')?.value;
    const selectedCostCenter = document.getElementById('editCostCenter')?.value;
    const selectedDivision = document.getElementById('editDivision')?.value;
    const selectedDepartment = document.getElementById('editDepartment')?.value;
    const selectedSection = document.getElementById('editSection')?.value;
    const selectedLocation = document.getElementById('editLocation')?.value;
    const selectedPosition = document.getElementById('editPosition')?.value;

    let empStatusParams = [];
    if (companyID) empStatusParams.push(`companyID=${companyID}`);
    if (selectedFormat) empStatusParams.push(`empFormat=${encodeURIComponent(selectedFormat)}`);
    if (selectedYear) empStatusParams.push(`budgetYear=${encodeURIComponent(selectedYear)}`);
    if (selectedCostCenter) empStatusParams.push(`costCenterCode=${encodeURIComponent(selectedCostCenter)}`);
    if (selectedDivision) empStatusParams.push(`divisionCode=${encodeURIComponent(selectedDivision)}`);
    if (selectedDepartment) empStatusParams.push(`departmentCode=${encodeURIComponent(selectedDepartment)}`);
    if (selectedSection) empStatusParams.push(`sectionCode=${encodeURIComponent(selectedSection)}`);
    if (selectedLocation) empStatusParams.push(`locationCode=${encodeURIComponent(selectedLocation)}`);
    if (selectedPosition) empStatusParams.push(`positionCode=${encodeURIComponent(selectedPosition)}`);
    const empStatusQuery = empStatusParams.length ? `?${empStatusParams.join('&')}` : '';

    populateDropdown('editEmpStatus', `${BUDGET_API.empStatuses}${empStatusQuery}`, 'Select Employee Status', (option, item) => {
      option.value = item;
      option.textContent = item;
    }, true);
  } else {
    const statusParams = [`statusType=Budget`];
    if (companyID) statusParams.push(`companyId=${companyID}`);
    const statusQuery = statusParams.length ? `?${statusParams.join('&')}` : '';

    populateDropdown('editEmpStatus', `/api/Select/statuses${statusQuery}`, 'Select Employee Status', (option, item) => {
      option.value = item.statusCode;
      option.textContent = item.statusName;
    }, true);

    //$('#editEmpStatus').removeAttr('disabled');
  }

}

// Update Employee Types dropdown
function updateOffcanvasEmployeeTypes() {
  const companyID = document.getElementById('editCompany')?.value;

  if (companyID) {
    populateDropdown('editEmpType', `${SELECT_API.employeeTypes}?companyId=${companyID}`, 'Select Employee Type', (option, item) => {
      option.value = item.itemCode;
      option.textContent = item.itemName;
    }, true);
  }
}

// Update New HC dropdown
function updateOffcanvasNewHC() {
  const companyID = document.getElementById('editCompany')?.value;

  if (companyID) {
    populateDropdown('editNewHC', `${SELECT_API.newHC}?companyId=${companyID}`, 'Select New HC', (option, item) => {
      option.value = item.itemCode;
      option.textContent = item.itemName;
    }, true);
  }
}

// Update New Period dropdown
function updateOffcanvasNewPeriod() {
  const companyID = document.getElementById('editCompany')?.value;

  if (companyID) {
    populateDropdown('editNewPeriod', `${SELECT_API.noOfMonths}?companyId=${companyID}`, 'Select New Period', (option, item) => {
      option.value = item.itemCode;
      option.textContent = item.itemName;
    }, true);
  }
}

// Update No. of Month dropdown (uses same API as New Period)
function updateOffcanvasNOfMonth() {
  const companyID = document.getElementById('editCompany')?.value;

  if (companyID) {
    populateDropdown('editNOfMonth', `${SELECT_API.noOfMonths}?companyId=${companyID}`, 'Select No. of Month', (option, item) => {
      option.value = item.itemCode;
      option.textContent = item.itemName;
    }, true);
  }
}

// Update New LE Period dropdown
function updateOffcanvasNewLEPeriod() {
  const companyID = document.getElementById('editCompany')?.value;

  if (companyID) {
    populateDropdown('editNewLEPeriod', `${SELECT_API.leNoOfMonths}?companyId=${companyID}`, 'Select New LE Period', (option, item) => {
      option.value = item.itemCode;
      option.textContent = item.itemName;
    }, true);
  }
}

// Update LE No. of Month dropdown (uses same API as New LE Period)
function updateOffcanvasLEnOfMonth() {
  const companyID = document.getElementById('editCompany')?.value;

  if (companyID) {
    populateDropdown('editLEnOfMonth', `${SELECT_API.leNoOfMonths}?companyId=${companyID}`, 'Select LE No. of Month', (option, item) => {
      option.value = item.itemCode;
      option.textContent = item.itemName;
    }, true);
  }
}

// Update New LE No. of Month dropdown (uses same API as New LE Period)
function updateOffcanvasNewLEnOfMonth() {
  const companyID = document.getElementById('editCompany')?.value;

  if (companyID) {
    populateDropdown('editLEnOfMonth', `${SELECT_API.leNoOfMonths}?companyId=${companyID}`, 'Select LE No. of Month', (option, item) => {
      option.value = item.itemValue;
      option.textContent = item.itemValue;
    }, true);
  }
}

// Update Divisions dropdown
function updateOffcanvasDivisions() {
  const companyID = document.getElementById('editCompany')?.value;
  const selectedFormat = document.getElementById('editFormat')?.value;
  const selectedYear = document.getElementById('editYear')?.value;
  const selectedCostCenter = document.getElementById('editCostCenter')?.value;

  let divisionParams = [];
  if (companyID) divisionParams.push(`companyID=${companyID}`);
  if (selectedFormat) divisionParams.push(`empFormat=${encodeURIComponent(selectedFormat)}`);
  if (selectedYear) divisionParams.push(`budgetYear=${encodeURIComponent(selectedYear)}`);
  if (selectedCostCenter) divisionParams.push(`costCenterCode=${encodeURIComponent(selectedCostCenter)}`);
  const divisionQuery = divisionParams.length ? `?${divisionParams.join('&')}` : '';

  populateDropdown('editDivision', `${BUDGET_API.divisions}${divisionQuery}`, 'Select Division', (option, item) => {
    option.value = item;
    option.textContent = item;
  });
}

// Update Departments dropdown
function updateOffcanvasDepartments() {
  const companyID = document.getElementById('editCompany')?.value;
  const selectedFormat = document.getElementById('editFormat')?.value;
  const selectedYear = document.getElementById('editYear')?.value;
  const selectedCostCenter = document.getElementById('editCostCenter')?.value;
  const selectedDivision = document.getElementById('editDivision')?.value;

  let departmentParams = [];
  if (companyID) departmentParams.push(`companyID=${companyID}`);
  if (selectedFormat) departmentParams.push(`empFormat=${encodeURIComponent(selectedFormat)}`);
  if (selectedYear) departmentParams.push(`budgetYear=${encodeURIComponent(selectedYear)}`);
  if (selectedCostCenter) departmentParams.push(`costCenterCode=${encodeURIComponent(selectedCostCenter)}`);
  if (selectedDivision) departmentParams.push(`divisionCode=${encodeURIComponent(selectedDivision)}`);
  const departmentQuery = departmentParams.length ? `?${departmentParams.join('&')}` : '';

  populateDropdown('editDepartment', `${BUDGET_API.departments}${departmentQuery}`, 'Select Department', (option, item) => {
    option.value = item;
    option.textContent = item;
  });
}

// Update Sections dropdown
function updateOffcanvasSections() {
  const companyID = document.getElementById('editCompany')?.value;
  const selectedFormat = document.getElementById('editFormat')?.value;
  const selectedYear = document.getElementById('editYear')?.value;
  const selectedCostCenter = document.getElementById('editCostCenter')?.value;
  const selectedDivision = document.getElementById('editDivision')?.value;
  const selectedDepartment = document.getElementById('editDepartment')?.value;

  let sectionParams = [];
  if (companyID) sectionParams.push(`companyID=${companyID}`);
  if (selectedFormat) sectionParams.push(`empFormat=${encodeURIComponent(selectedFormat)}`);
  if (selectedYear) sectionParams.push(`budgetYear=${encodeURIComponent(selectedYear)}`);
  if (selectedCostCenter) sectionParams.push(`costCenterCode=${encodeURIComponent(selectedCostCenter)}`);
  if (selectedDivision) sectionParams.push(`divisionCode=${encodeURIComponent(selectedDivision)}`);
  if (selectedDepartment) sectionParams.push(`departmentCode=${encodeURIComponent(selectedDepartment)}`);
  const sectionQuery = sectionParams.length ? `?${sectionParams.join('&')}` : '';

  populateDropdown('editSection', `${BUDGET_API.sections}${sectionQuery}`, 'Select Section', (option, item) => {
    option.value = item;
    option.textContent = item;
  });
}

// Update Locations dropdown
function updateOffcanvasLocations() {
  const companyID = document.getElementById('editCompany')?.value;
  const selectedFormat = document.getElementById('editFormat')?.value;
  const selectedYear = document.getElementById('editYear')?.value;
  const selectedCostCenter = document.getElementById('editCostCenter')?.value;
  const selectedDivision = document.getElementById('editDivision')?.value;
  const selectedDepartment = document.getElementById('editDepartment')?.value;
  const selectedSection = document.getElementById('editSection')?.value;

  let locationParams = [];
  if (companyID) locationParams.push(`companyID=${companyID}`);
  if (selectedFormat) locationParams.push(`empFormat=${encodeURIComponent(selectedFormat)}`);
  if (selectedYear) locationParams.push(`budgetYear=${encodeURIComponent(selectedYear)}`);
  if (selectedCostCenter) locationParams.push(`costCenterCode=${encodeURIComponent(selectedCostCenter)}`);
  if (selectedDivision) locationParams.push(`divisionCode=${encodeURIComponent(selectedDivision)}`);
  if (selectedDepartment) locationParams.push(`departmentCode=${encodeURIComponent(selectedDepartment)}`);
  if (selectedSection) locationParams.push(`sectionCode=${encodeURIComponent(selectedSection)}`);
  const locationQuery = locationParams.length ? `?${locationParams.join('&')}` : '';

  populateDropdown('editLocation', `${BUDGET_API.storeNames}${locationQuery}`, 'Select Location', (option, item) => {
    option.value = item;
    option.textContent = item;
  });
}

// Update Positions dropdown
function updateOffcanvasPositions() {
  const companyID = document.getElementById('editCompany')?.value;
  const selectedFormat = document.getElementById('editFormat')?.value;
  const selectedYear = document.getElementById('editYear')?.value;
  const selectedCostCenter = document.getElementById('editCostCenter')?.value;
  const selectedDivision = document.getElementById('editDivision')?.value;
  const selectedDepartment = document.getElementById('editDepartment')?.value;
  const selectedSection = document.getElementById('editSection')?.value;
  const selectedLocation = document.getElementById('editLocation')?.value;

  let positionParams = [];
  if (companyID) positionParams.push(`companyID=${companyID}`);
  if (selectedFormat) positionParams.push(`empFormat=${encodeURIComponent(selectedFormat)}`);
  if (selectedYear) positionParams.push(`budgetYear=${encodeURIComponent(selectedYear)}`);
  if (selectedCostCenter) positionParams.push(`costCenterCode=${encodeURIComponent(selectedCostCenter)}`);
  if (selectedDivision) positionParams.push(`divisionCode=${encodeURIComponent(selectedDivision)}`);
  if (selectedDepartment) positionParams.push(`departmentCode=${encodeURIComponent(selectedDepartment)}`);
  if (selectedSection) positionParams.push(`sectionCode=${encodeURIComponent(selectedSection)}`);
  if (selectedLocation) positionParams.push(`locationCode=${encodeURIComponent(selectedLocation)}`);
  const positionQuery = positionParams.length ? `?${positionParams.join('&')}` : '';

  populateDropdown('editPosition', `${BUDGET_API.positions}${positionQuery}`, 'Select Position', (option, item) => {
    option.value = item.positionCode;
    option.textContent = `${item.positionName} (${item.positionCode})`;
  }, true);
}

// Update Job Bands dropdown
function updateOffcanvasJobBands() {
  const companyID = document.getElementById('editCompany')?.value;
  const selectedFormat = document.getElementById('editFormat')?.value;
  const selectedYear = document.getElementById('editYear')?.value;
  const selectedCostCenter = document.getElementById('editCostCenter')?.value;
  const selectedDivision = document.getElementById('editDivision')?.value;
  const selectedDepartment = document.getElementById('editDepartment')?.value;
  const selectedSection = document.getElementById('editSection')?.value;
  const selectedLocation = document.getElementById('editLocation')?.value;
  const selectedPosition = document.getElementById('editPosition')?.value;
  const selectedEmpStatus = document.getElementById('editEmpStatus')?.value;

  let jobBandParams = [];
  if (companyID) jobBandParams.push(`companyID=${companyID}`);
  if (selectedFormat) jobBandParams.push(`empFormat=${encodeURIComponent(selectedFormat)}`);
  if (selectedYear) jobBandParams.push(`budgetYear=${encodeURIComponent(selectedYear)}`);
  if (selectedCostCenter) jobBandParams.push(`costCenterCode=${encodeURIComponent(selectedCostCenter)}`);
  if (selectedDivision) jobBandParams.push(`divisionCode=${encodeURIComponent(selectedDivision)}`);
  if (selectedDepartment) jobBandParams.push(`departmentCode=${encodeURIComponent(selectedDepartment)}`);
  if (selectedSection) jobBandParams.push(`sectionCode=${encodeURIComponent(selectedSection)}`);
  if (selectedLocation) jobBandParams.push(`locationCode=${encodeURIComponent(selectedLocation)}`);
  if (selectedPosition) jobBandParams.push(`positionCode=${encodeURIComponent(selectedPosition)}`);
  if (selectedEmpStatus) jobBandParams.push(`empStatus=${encodeURIComponent(selectedEmpStatus)}`);
  const jobBandQuery = jobBandParams.length ? `?${jobBandParams.join('&')}` : '';

  populateDropdown('editJobBand', `${BUDGET_API.jobBands}${jobBandQuery}`, 'Select Job Band', (option, item) => {
    option.value = item;
    option.textContent = item;
  }, true);
}

// Prevent closing side panel except by btn-close or cancelEditBtn
let allowOffcanvasClose = false;

// Intercept offcanvas hide event for both CoreUI and Bootstrap
function preventOffcanvasHide(e) {
  console.log('preventOffcanvasHide called, allowOffcanvasClose:', allowOffcanvasClose);
  if (!allowOffcanvasClose) {
    console.log('Blocking offcanvas hide');
    e.preventDefault();
    return false;
  }
  console.log('Allowing offcanvas hide');
}

// Only allow close by btn-close or cancelEditBtn
function safeCloseOffcanvas() {
  document.activeElement.blur(); // Prevent ARIA focus error
  allowOffcanvasClose = true;
  setTimeout(() => {
    const offcanvasEl = document.getElementById('offcanvasAddRow');
    const instance = getOffcanvasInstance(offcanvasEl);
    instance.hide();
    setTimeout(() => {
      allowOffcanvasClose = false;

      // Deselect any selected rows in budget grid to allow re-selection
      const budgetGridApi = window.getGridApi ? window.getGridApi() : null;
      if (budgetGridApi) {
        budgetGridApi.deselectAll();
        console.log('Budget grid rows deselected after closing offcanvasAddRow');
      }

      // Sync position after close if fullscreen manager is available
      if (typeof window.forceUpdateOffcanvasPositions === 'function') {
        window.forceUpdateOffcanvasPositions();
      }
    }, 500);
  }, 10);
}

//Only allow close by btn-close [Side Panel Benefits (Offcanvas)]
function safeCloseBenefitsOffcanvas() {
  document.activeElement.blur(); // Prevent ARIA focus error
  allowOffcanvasClose = true;
  setTimeout(() => {
    const offcanvasEl = document.getElementById('offcanvasBenefits');
    const instance = getOffcanvasInstance(offcanvasEl);
    instance.hide();
    setTimeout(() => {
      allowOffcanvasClose = false;
      // Sync position after close if fullscreen manager is available
      if (typeof window.forceUpdateOffcanvasPositions === 'function') {
        window.forceUpdateOffcanvasPositions();
      }
    }, 500);
  }, 10);
}

// Initialize offcanvas event handlers
function initializeOffcanvasHandlers() {
  const offcanvasEl = document.getElementById('offcanvasAddRow');
  const benefitsOffcanvasEl = document.getElementById('offcanvasBenefits');

  // Prevent closing except by specific buttons
  if (window.bootstrap && window.bootstrap.Offcanvas) {
    offcanvasEl.addEventListener('hide.bs.offcanvas', preventOffcanvasHide);
    benefitsOffcanvasEl.addEventListener('hide.bs.offcanvas', preventOffcanvasHide);
  }
  if (window.coreui && window.coreui.Offcanvas) {
    offcanvasEl.addEventListener('hide.coreui.offcanvas', preventOffcanvasHide);
    benefitsOffcanvasEl.addEventListener('hide.coreui.offcanvas', preventOffcanvasHide);
  }
  // Prevent backdrop click from closing (for Bootstrap)
  document.addEventListener('mousedown', function (e) {
    const backdrop = document.querySelector('.offcanvas-backdrop');
    if (backdrop && e.target === backdrop && !allowOffcanvasClose) {
      e.stopPropagation();
      e.preventDefault();
    }
  });

  // Close button handlers
  document.getElementById('cancelEditBtn').addEventListener('click', function () {
    safeCloseOffcanvas();
  });

  document.getElementById('closeOffcanvasBtn').addEventListener('click', function () {
    safeCloseOffcanvas();
  });

  document.getElementById('closeOffcanvasBenefitsBtn').addEventListener('click', function () {
    safeCloseBenefitsOffcanvas();
  });

  // Save button handler: show confirm modal
  const saveEditBtn = document.getElementById('saveEditBtn');
  saveEditBtn.addEventListener('click', function (e) {
    e.preventDefault();
    const confirmModal = new bootstrap.Modal(document.getElementById('confirmSaveModal'));
    confirmModal.show();
  });

  // Confirm Save modal logic
  let pendingFormSubmit = null;
  document.getElementById('confirmSaveBtn').addEventListener('click', function () {
    // Actually submit the form
    pendingFormSubmit = true;
    document.getElementById('editRowForm').requestSubmit();
    const confirmModal = bootstrap.Modal.getInstance(document.getElementById('confirmSaveModal'));
    confirmModal.hide();
  });

  document.getElementById('cancelSaveBtn').addEventListener('click', function () {
    pendingFormSubmit = false;
    // Just close modal, stay in side panel
  });

  // Intercept form submit to only allow after confirm
  const editRowForm = document.getElementById('editRowForm');
  editRowForm.addEventListener('submit', function (e) {
    if (!pendingFormSubmit) {
      e.preventDefault();
      return;
    }
    // After confirmed, allow closing side panel
    safeCloseOffcanvas();
    pendingFormSubmit = false;
    const mode = document.getElementById('formMode').value;
    if (mode === 'edit') {
      const rowIndex = this.getAttribute('data-edit-index');
      if (rowIndex !== null) {
        console.log('Updating row at index:', rowIndex);
        // Handle edit logic here
      }
    } else if (mode === 'add') {
      console.log('Adding new row');
      // Handle add logic here
    }
  });

  // Initialize fullscreen offcanvas management if available
  if (typeof window.initializeOffcanvasFullscreenManager === 'function') {
    console.log('🔗 Integrating with Offcanvas Fullscreen Manager');
    // The fullscreen manager will auto-initialize via DOMContentLoaded
  } else {
    console.log('⚠️ Offcanvas Fullscreen Manager not available - make sure budget.offcanvas.fullscreen.js is loaded');
  }

  console.log('Offcanvas handlers initialized successfully');
}

/**
 * Synchronize offcanvas positions when showing/hiding offcanvas
 * This ensures proper positioning regardless of fullscreen state
 */
function syncOffcanvasPosition(offcanvasElement) {
  if (!offcanvasElement) return;

  // Check if fullscreen manager is available and force position update
  if (typeof window.forceUpdateOffcanvasPositions === 'function') {
    console.log('🔄 Syncing offcanvas position with fullscreen state');
    window.forceUpdateOffcanvasPositions();
  }
}

/**
 * Enhanced safe close function with fullscreen position sync
 */
function safeCloseOffcanvasWithSync(offcanvasId) {
  const offcanvasElement = document.getElementById(offcanvasId);

  // Sync position before closing
  syncOffcanvasPosition(offcanvasElement);

  // Use appropriate close function
  if (offcanvasId === 'offcanvasAddRow') {
    safeCloseOffcanvas();
  } else if (offcanvasId === 'offcanvasBenefits') {
    safeCloseBenefitsOffcanvas();
  }
}

// Export new synchronization functions
window.syncOffcanvasPosition = syncOffcanvasPosition;
window.safeCloseOffcanvasWithSync = safeCloseOffcanvasWithSync;

// Export functions to global scope for use by other modules
window.populateOffcanvasDropdowns = populateOffcanvasDropdowns;
window.setupOffcanvasDropdownRelationships = setupOffcanvasDropdownRelationships;
window.setupOffcanvasDropdownRelationshipsForAdd = setupOffcanvasDropdownRelationshipsForAdd;
window.setupOffcanvasDropdownRelationshipsForEdit = setupOffcanvasDropdownRelationshipsForEdit;
window.initializeOffcanvasHandlers = initializeOffcanvasHandlers;
window.safeCloseOffcanvas = safeCloseOffcanvas;
window.safeCloseBenefitsOffcanvas = safeCloseBenefitsOffcanvas;

// Export all offcanvas update functions
window.updateOffcanvasEmpFormats = updateOffcanvasEmpFormats;
window.updateOffcanvasYears = updateOffcanvasYears;
window.updateOffcanvasCostCenters = updateOffcanvasCostCenters;
window.updateOffcanvasEmpStatus = updateOffcanvasEmpStatus;
window.updateOffcanvasEmployeeTypes = updateOffcanvasEmployeeTypes;
window.updateOffcanvasNewHC = updateOffcanvasNewHC;
window.updateOffcanvasNewPeriod = updateOffcanvasNewPeriod;
window.updateOffcanvasNOfMonth = updateOffcanvasNOfMonth;
window.updateOffcanvasNewLEPeriod = updateOffcanvasNewLEPeriod;
window.updateOffcanvasLEnOfMonth = updateOffcanvasLEnOfMonth;
window.updateOffcanvasNewLEnOfMonth = updateOffcanvasNewLEnOfMonth;
window.updateOffcanvasDivisions = updateOffcanvasDivisions;
window.updateOffcanvasDepartments = updateOffcanvasDepartments;
window.updateOffcanvasSections = updateOffcanvasSections;
window.updateOffcanvasLocations = updateOffcanvasLocations;
window.updateOffcanvasPositions = updateOffcanvasPositions;
window.updateOffcanvasJobBands = updateOffcanvasJobBands;

// Export debounced functions for use by other modules
window.debouncedUpdateOffcanvasCostCenters = debouncedUpdateOffcanvasCostCenters;
window.debouncedUpdateOffcanvasDivisions = debouncedUpdateOffcanvasDivisions;
window.debouncedUpdateOffcanvasDepartments = debouncedUpdateOffcanvasDepartments;
window.debouncedUpdateOffcanvasSections = debouncedUpdateOffcanvasSections;
window.debouncedUpdateOffcanvasLocations = debouncedUpdateOffcanvasLocations;
window.debouncedUpdateOffcanvasPositions = debouncedUpdateOffcanvasPositions;
window.debouncedUpdateOffcanvasJobBands = debouncedUpdateOffcanvasJobBands;
window.debouncedUpdateOffcanvasEmpStatus = debouncedUpdateOffcanvasEmpStatus;
window.debouncedUpdateOffcanvasEmployeeTypes = debouncedUpdateOffcanvasEmployeeTypes;
window.debouncedUpdateOffcanvasNewHC = debouncedUpdateOffcanvasNewHC;
window.debouncedUpdateOffcanvasNewPeriod = debouncedUpdateOffcanvasNewPeriod;
window.debouncedUpdateOffcanvasNOfMonth = debouncedUpdateOffcanvasNOfMonth;
window.debouncedUpdateOffcanvasNewLEPeriod = debouncedUpdateOffcanvasNewLEPeriod;
window.debouncedUpdateOffcanvasLEnOfMonth = debouncedUpdateOffcanvasLEnOfMonth;
window.debouncedUpdateOffcanvasNewLEnOfMonth = debouncedUpdateOffcanvasNewLEnOfMonth;



