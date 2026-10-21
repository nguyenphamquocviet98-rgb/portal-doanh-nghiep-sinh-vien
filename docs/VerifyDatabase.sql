SET NOCOUNT ON;
DECLARE @passed int = 0;
BEGIN TRY
    BEGIN TRANSACTION;
    INSERT INTO Applications (StudentId, InternshipPositionId, CvPath, Status, CreatedAt)
        SELECT TOP 1 StudentId, InternshipPositionId, CvPath, 0, SYSUTCDATETIME() FROM Applications;
    ROLLBACK;
    THROW 51000, 'Duplicate application was accepted', 1;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    IF ERROR_NUMBER() NOT IN (2601,2627) THROW;
    PRINT 'PASS: unique student/position'; SET @passed += 1;
END CATCH;
BEGIN TRY
    BEGIN TRANSACTION;
    INSERT INTO Internships (ApplicationId, StudentId, InternshipPeriodId, StartDate, EndDate, Status, CreatedAt)
        SELECT a.Id, s.Id, p.Id, p.StartDate, p.EndDate, 0, SYSUTCDATETIME()
        FROM Applications a JOIN Students s ON s.Id=a.StudentId
        JOIN InternshipPositions pos ON pos.Id=a.InternshipPositionId
        JOIN InternshipPeriods p ON p.Id=pos.InternshipPeriodId
        WHERE a.Status=6;
    ROLLBACK;
    THROW 51000, 'Duplicate student/period was accepted', 1;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    IF ERROR_NUMBER() NOT IN (2601,2627) THROW;
    PRINT 'PASS: unique student/period'; SET @passed += 1;
END CATCH;
BEGIN TRY
    BEGIN TRANSACTION;
    INSERT INTO InternshipDiaries (InternshipId, WeekNumber, StartDate, EndDate, WorkContent, Results, CreatedAt)
        SELECT TOP 1 InternshipId,WeekNumber,StartDate,EndDate,WorkContent,Results,SYSUTCDATETIME() FROM InternshipDiaries;
    ROLLBACK;
    THROW 51000, 'Duplicate week was accepted', 1;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    IF ERROR_NUMBER() NOT IN (2601,2627) THROW;
    PRINT 'PASS: unique diary/week'; SET @passed += 1;
END CATCH;
BEGIN TRY
    BEGIN TRANSACTION;
    INSERT INTO Evaluations (InternshipId, ProfessionalKnowledge, WorkSkills, Attitude, Responsibility, Communication, TaskCompletion, Comment, CreatedAt)
        SELECT TOP 1 InternshipId,8,8,8,8,8,8,N'Demo test',SYSUTCDATETIME() FROM Evaluations;
    ROLLBACK;
    THROW 51000, 'Duplicate evaluation was accepted', 1;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    IF ERROR_NUMBER() NOT IN (2601,2627) THROW;
    PRINT 'PASS: unique evaluation'; SET @passed += 1;
END CATCH;
BEGIN TRY
    BEGIN TRANSACTION;
    UPDATE TOP (1) Evaluations SET ProfessionalKnowledge=11;
    ROLLBACK;
    THROW 51000, 'Invalid score was accepted', 1;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    IF ERROR_NUMBER() <> 547 THROW;
    PRINT 'PASS: score range'; SET @passed += 1;
END CATCH;
BEGIN TRY
    BEGIN TRANSACTION;
    UPDATE TOP (1) Applications SET Status=4, Note=NULL;
    ROLLBACK;
    THROW 51000, 'Rejection without note was accepted', 1;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    IF ERROR_NUMBER() <> 547 THROW;
    PRINT 'PASS: rejection note'; SET @passed += 1;
END CATCH;
BEGIN TRY
    BEGIN TRANSACTION;
    INSERT INTO Majors (FacultyId,Code,Name,IsActive,CreatedAt) VALUES (-1,N'FK-TEST',N'Test',1,SYSUTCDATETIME());
    ROLLBACK;
    THROW 51000, 'Invalid faculty reference was accepted', 1;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    IF ERROR_NUMBER() <> 547 THROW;
    PRINT 'PASS: foreign key'; SET @passed += 1;
END CATCH;
IF @passed <> 7 THROW 51000, 'Database validation incomplete', 1;
PRINT 'All 7 checks passed; all test mutations rolled back.';