━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
🔍 SaveBatchEntry - Incoming Request
   Request is null: False
   Budgets is null: False
   Budgets count: 1
   CreatedBy: 'DevUser'
   ModelState.IsValid: True
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (25ms) [Parameters=[@__budget_EmpCode_0='?' (Size = 10), @__budget_BudgetYear_1='?' (DbType = Int32), @__budget_CostCenterCode_2='?' (Size = 50)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [dbo].[HRB_BUDGET_BIGC] AS [h]
              WHERE [h].[EMP_CODE] = @__budget_EmpCode_0 AND [h].[BUDGET_YEAR] = @__budget_BudgetYear_1 AND [h].[COST_CENTER_CODE] = @__budget_CostCenterCode_2) THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (105ms) [Parameters=[@p0='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p1='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p2='?' (Size = 50), @p3='?' (Size = 50), @p4='?' (DbType = Int32), @p5='?' (DbType = Int32), @p6='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p7='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p8='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p9='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p10='?' (Size = 50), @p11='?' (Size = 50), @p12='?' (DbType = Int32), @p13='?' (Size = 50), @p14='?' (Size = 255), @p15='?' (Size = 50), @p16='?' (Size = 255), @p17='?' (Size = 50), @p18='?' (Size = 255), @p19='?' (Size = 10), @p20='?' (Size = 50), @p21='?' (Size = 10), @p22='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p23='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p24='?' (Size = 50), @p25='?' (Precision 
= 18) (Scale = 2) (DbType = Decimal), @p26='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p27='?' (Size = 255), @p28='?' (Size = 255), @p29='?' (Size = 1), @p30='?' (Size = 1), @p31='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p32='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p33='?' (Size = 255), @p34='?' (Size = 255), @p35='?' (Size = 255), @p36='?' (Size = 255), @p37='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p38='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p39='?' (Size = 10), @p40='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p41='?' (Precision 
= 18) (Scale = 2) (DbType = Decimal), @p42='?' (Size = 50), @p43='?' (DbType = DateTime2), @p44='?' (Size = 4000), @p45='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p46='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p47='?' (DbType = Int32), @p48='?' (Precision = 18) (Scale = 2) (DbType 
= Decimal), @p49='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p50='?' (Size = 255), @p51='?' (Size = 255), @p52='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p53='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p54='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p55='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p56='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p57='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p58='?' (Size = 10), @p59='?' (Size = 10), @p60='?' (Size = 10), @p61='?' (DbType = Int32), @p62='?' (Size = 50), @p63='?' (Size = 100), @p64='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p65='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p66='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p67='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p68='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p69='?' (Precision 
= 18) (Scale = 2) (DbType = Decimal), @p70='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p71='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p72='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p73='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p74='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p75='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p76='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p77='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p78='?' (Size = 50), @p79='?' (Size = 255), @p80='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p81='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p82='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p83='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p84='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p85='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p86='?' (Size = 
1), @p87='?' (Size = 255), @p88='?' (Size = 50), @p89='?' (Size = 50), @p90='?' (Size = 255), @p91='?' (Precision = 18) (Scale = 2) (DbType = Decimal), 
@p92='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p93='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p94='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p95='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p96='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p97='?' (Size = 255), @p98='?' (Size = 50), @p99='?' (Size = 50), @p100='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p101='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p102='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p103='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p104='?' (Size = 100), @p105='?' (DbType = DateTime2), @p106='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p107='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p108='?' (DbType = Int32), @p109='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [dbo].[HRB_BUDGET_BIGC] ([BONUS], [BONUS_LE], [BONUS_TYPES], [BU], [BUDGET_YEAR], [BUDGET_YEAR_LE], [CAR_ALLOWANCE], [CAR_ALLOWANCE_LE], [CAR_RENTAL_PE], [CAR_RENTAL_PE_LE], [COBU], [COMPANY_CODE], [COMPANY_ID], [COST_CENTER_CODE], [COST_CENTER_NAME], [COST_CENTER_PLAN], [DEPARTMENT], 
[DISCOUNT], [DIVISION], [EMP_CODE], [EMP_STATUS], [EMP_TYPE_CODE], [EMPLOYEE_WELFARE], [EMPLOYEE_WELFARE_LE], [EXECUTIVE], [FLEET_CARD_PE], [FLEET_CARD_PE_LE], [FNAME_EN], [FNAME_TH], [FOCUS_HC], [FOCUS_PE], [GASOLINE_ALLOWANCE], [GASOLINE_ALLOWANCE_LE], [GROUP_LEVEL_1], [GROUP_LEVEL_2], [GROUP_LEVEL_3], [GROUP_NAME], [HOUSING_ALLOWANCE], [HOUSING_ALLOWANCE_LE], [HRBP_EMP_CODE], [INTEREST], [INTEREST_LE], [JOB_BAND], [JOIN_DATE], [JOIN_PVF], [LABOR_FUND_FEE], [LABOR_FUND_FEE_LE], [LE_OF_MONTH], [LICENSE_ALLOWANCE], [LICENSE_ALLOWANCE_LE], [LNAME_EN], [LNAME_TH], [LONG_SERVICE], [LONG_SERVICE_LE], [MEDICAL_EXPENSE], [MEDICAL_EXPENSE_LE], [MEDICAL_INHOUSE], [MEDICAL_INHOUSE_LE], [NEW_HC_CODE], [NEW_VAC_LE], [NEW_VAC_PERIOD], [NO_OF_MONTH], [ORG_UNIT_CODE], [ORG_UNIT_NAME], [OTHER_ALLOWANCE], [OTHER_ALLOWANCE_LE], [OTHER_STAFF_BENEFIT], [OTHER_STAFF_BENEFIT_LE], [PAYROLL], [PAYROLL_LE], [PE_MTH], [PE_MTH_LE], [PE_SB_MTH], [PE_SB_MTH_LE], [PE_SB_YEAR], [PE_SB_YEAR_LE], [PE_YEAR], [PE_YEAR_LE], [POSITION_CODE], [POSITION_NAME], [PREMIUM], [PREMIUM_LE], 
[PROVIDENT_FUND], [PROVIDENT_FUND_LE], [PROVISION], [PROVISION_LE], [PVF], [REASON], [RUNRATE_CODE], [SALARY_STRUCTURE], [SECTION], [SKILL_PAY_ALLOWANCE], [SKILL_PAY_ALLOWANCE_LE], [SOCIAL_SECURITY], [SOCIAL_SECURITY_LE], [STAFF_INSURANCE], [STAFF_INSURANCE_LE], [STORE_NAME], [TITLE_EN], [TITLE_TH], [TOTAL_PAYROLL], [TOTAL_PAYROLL_LE], [TRAINING], [TRAINING_LE], [UPDATED_BY], [UPDATED_DATE], [WAGE_STUDENT], [WAGE_STUDENT_LE], [YOS_CURR_YEAR], [YOS_NEXT_YEAR])
      OUTPUT INSERTED.[BUDGET_ID]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20, @p21, @p22, @p23, @p24, @p25, @p26, @p27, @p28, @p29, @p30, @p31, @p32, @p33, @p34, @p35, @p36, @p37, @p38, @p39, @p40, @p41, @p42, @p43, @p44, @p45, @p46, @p47, @p48, @p49, 
@p50, @p51, @p52, @p53, @p54, @p55, @p56, @p57, @p58, @p59, @p60, @p61, @p62, @p63, @p64, @p65, @p66, @p67, @p68, @p69, @p70, @p71, @p72, @p73, @p74, @p75, @p76, @p77, @p78, @p79, @p80, @p81, @p82, @p83, @p84, @p85, @p86, @p87, @p88, @p89, @p90, @p91, @p92, @p93, @p94, @p95, @p96, @p97, @p98, @p99, @p100, @p101, @p102, @p103, @p104, @p105, @p106, @p107, @p108, @p109);
Error in CreateBigcBudgetAsync: An error occurred while saving the entity changes. See the inner exception for details.
Error in CreateBatchBudgetsAsync: Error at row 0: An error occurred while saving the entity changes. See the inner exception for details.
fail: Microsoft.EntityFrameworkCore.Update[10000]
      An exception occurred in the database while saving changes for context type 'HCBPCoreUI_Backend.Models.HRBudgetDbContext'.
      Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while saving the entity changes. See the inner exception for details.
       ---> Microsoft.Data.SqlClient.SqlException (0x80131904): String or binary data would be truncated in table 'HRBUDGET.dbo.HRB_BUDGET_BIGC', column 'JOIN_PVF'. Truncated value: 'P'.
         at Microsoft.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
         at Microsoft.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
         at Microsoft.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
         at Microsoft.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet 
bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
         at Microsoft.Data.SqlClient.SqlDataReader.TryHasMoreRows(Boolean& moreRows)
         at Microsoft.Data.SqlClient.SqlDataReader.TryReadInternal(Boolean setTimeout, Boolean& more)
         at Microsoft.Data.SqlClient.SqlDataReader.ReadAsyncExecute(Task task, Object state)
         at Microsoft.Data.SqlClient.SqlDataReader.InvokeAsyncCall[T](SqlDataReaderBaseAsyncCallContext`1 context)
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeResultSetAsync(Int32 startCommandIndex, RelationalDataReader reader, CancellationToken cancellationToken)
      ClientConnectionId:85cb39b1-c2e7-4a97-85f5-b5ba8aa8a690
      Error Number:2628,State:1,Class:16
         --- End of inner exception stack trace ---
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeResultSetAsync(Int32 startCommandIndex, RelationalDataReader reader, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeAsync(RelationalDataReader reader, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.SqlServer.Update.Internal.SqlServerModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.RelationalDatabase.SaveChangesAsync(IList`1 entries, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
      Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while saving the entity changes. See the inner exception for details.
       ---> Microsoft.Data.SqlClient.SqlException (0x80131904): String or binary data would be truncated in table 'HRBUDGET.dbo.HRB_BUDGET_BIGC', column 'JOIN_PVF'. Truncated value: 'P'.
         at Microsoft.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
         at Microsoft.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
         at Microsoft.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
         at Microsoft.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet 
bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
         at Microsoft.Data.SqlClient.SqlDataReader.TryHasMoreRows(Boolean& moreRows)
         at Microsoft.Data.SqlClient.SqlDataReader.TryReadInternal(Boolean setTimeout, Boolean& more)
         at Microsoft.Data.SqlClient.SqlDataReader.ReadAsyncExecute(Task task, Object state)
         at Microsoft.Data.SqlClient.SqlDataReader.InvokeAsyncCall[T](SqlDataReaderBaseAsyncCallContext`1 context)
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeResultSetAsync(Int32 startCommandIndex, RelationalDataReader reader, CancellationToken cancellationToken)
      ClientConnectionId:85cb39b1-c2e7-4a97-85f5-b5ba8aa8a690
      Error Number:2628,State:1,Class:16
         --- End of inner exception stack trace ---
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeResultSetAsync(Int32 startCommandIndex, RelationalDataReader reader, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeAsync(RelationalDataReader reader, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.SqlServer.Update.Internal.SqlServerModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.RelationalDatabase.SaveChangesAsync(IList`1 entries, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
