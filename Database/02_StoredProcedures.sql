-- PATIENT STORED PROCEDURES

-- SP 1: Register Patient
CREATE PROCEDURE sp_RegisterPatient
    @PatientCode  VARCHAR(20),
    @FullName     NVARCHAR(100),
    @DateOfBirth  DATE,
    @Gender       CHAR(1),
    @PhoneNumber  VARCHAR(15),
    @Email        VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Patients WHERE PhoneNumber = @PhoneNumber)
        THROW 50001, 'Phone number already registered.', 1;

    IF @Email IS NOT NULL AND
       EXISTS (SELECT 1 FROM Patients WHERE Email = @Email)
        THROW 50002, 'Email already registered.', 1;

    INSERT INTO Patients
        (PatientCode, FullName, DateOfBirth, Gender, PhoneNumber, Email)
    VALUES
        (@PatientCode, @FullName, @DateOfBirth, @Gender, @PhoneNumber, @Email);

    SELECT SCOPE_IDENTITY() AS NewPatientId;
END;
GO

-- SP 2: Update Patient Details
CREATE PROCEDURE sp_UpdatePatient
    @PatientId    INT,
    @FullName     NVARCHAR(100),
    @PhoneNumber  VARCHAR(15),
    @Email        VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Patients WHERE PatientId = @PatientId AND IsActive = 1)
        THROW 50003, 'Patient not found or inactive.', 1;

    IF EXISTS (SELECT 1 FROM Patients
               WHERE PhoneNumber = @PhoneNumber AND PatientId != @PatientId)
        THROW 50001, 'Phone number already registered.', 1;

    IF @Email IS NOT NULL AND
       EXISTS (SELECT 1 FROM Patients
               WHERE Email = @Email AND PatientId != @PatientId)
        THROW 50002, 'Email already registered.', 1;

    UPDATE Patients
    SET
        FullName    = @FullName,
        PhoneNumber = @PhoneNumber,
        Email       = @Email
    WHERE PatientId = @PatientId;
END;
GO

-- SP 3: Deactivate Patient
CREATE PROCEDURE sp_DeactivatePatient
    @PatientId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Patients WHERE PatientId = @PatientId AND IsActive = 1)
        THROW 50003, 'Patient not found or already inactive.', 1;

    UPDATE Patients
    SET IsActive = 0
    WHERE PatientId = @PatientId;
END;
GO

-- SP 4: Get Active Patient
CREATE PROCEDURE sp_GetAllActivePatients
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        PatientId,
        PatientCode,
        FullName,
        DateOfBirth,        
        Gender,             
        PhoneNumber,
        Email,
        IsActive,
        CreatedAt

    FROM Patients
    WHERE IsActive = 1;
END;
GO

-- SP 5: Single Patient by ID
CREATE PROCEDURE sp_GetPatientById
    @PatientId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        PatientId, PatientCode, FullName,
        DateOfBirth, Gender, PhoneNumber, Email, IsActive
    FROM Patients
    WHERE PatientId = @PatientId AND IsActive = 1;
END;
GO


-- DOCTOR STORED PROCEDURES

-- SP 6: Get all doctor by filter
CREATE PROCEDURE sp_GetDoctors
    @Specialization VARCHAR(100) = NULL,
    @IsAvailable    BIT          = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        DoctorId, DoctorCode, FullName,
        Specialization, PhoneNumber,
        ConsultationFee, IsAvailable
    FROM Doctors
    WHERE
        (@Specialization IS NULL OR Specialization = @Specialization)
        AND
        (@IsAvailable IS NULL OR IsAvailable = @IsAvailable);
END;
GO

-- SP 7: Single Doctor by ID
CREATE PROCEDURE sp_GetDoctorById
    @DoctorId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        DoctorId, DoctorCode, FullName,
        Specialization, PhoneNumber,
        ConsultationFee, IsAvailable
    FROM Doctors
    WHERE DoctorId = @DoctorId;
END;
GO


-- APPOINTMENT STORED PROCEDURES

-- SP 8: Book Appointment
CREATE PROCEDURE sp_BookAppointment
    @PatientId       INT,
    @DoctorId        INT,
    @AppointmentDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Doctors
                       WHERE DoctorId = @DoctorId AND IsAvailable = 1)
            THROW 50004, 'Doctor is not available for booking.', 1;

        IF NOT EXISTS (SELECT 1 FROM Patients
                       WHERE PatientId = @PatientId AND IsActive = 1)
            THROW 50003, 'Patient not found or inactive.', 1;

        INSERT INTO Appointments (PatientId, DoctorId, AppointmentDate)
        VALUES (@PatientId, @DoctorId, @AppointmentDate);

        SELECT SCOPE_IDENTITY() AS NewAppointmentId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;               
    END CATCH
END;
GO

-- SP 9: Cancel Appointment 
CREATE PROCEDURE sp_CancelAppointment
    @AppointmentId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Appointments
                   WHERE AppointmentId = @AppointmentId AND Status = 'Scheduled')
        THROW 50005, 'Only Scheduled appointments can be cancelled.', 1;

    UPDATE Appointments
    SET
        Status      = 'Cancelled',
        CancelledAt = GETDATE()
    WHERE AppointmentId = @AppointmentId;
END;
GO

-- SP 10: Upcoming Appointments (Next 7 Days)
CREATE PROCEDURE sp_GetUpcomingAppointments
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        a.AppointmentId,
        p.FullName AS PatientName,
        d.FullName AS DoctorName,
        d.Specialization,
        a.AppointmentDate,
        FORMAT(a.AppointmentDate, 'dd MMM yyyy hh:mm tt') AS AppointmentDateFormatted,
        a.Status,
        d.ConsultationFee
    FROM Appointments a
    JOIN Patients p ON a.PatientId = p.PatientId
    JOIN Doctors  d ON a.DoctorId  = d.DoctorId
    WHERE
        a.AppointmentDate BETWEEN GETDATE() AND DATEADD(DAY, 7, GETDATE())
        AND a.Status = 'Scheduled';
END;
GO

-- SP 11: Doctor's Appointment
CREATE PROCEDURE sp_GetAppointmentsByDoctor
    @DoctorId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        a.AppointmentId,
        p.FullName AS PatientName,
        d.FullName AS DoctorName,
        d.Specialization,
        FORMAT(a.AppointmentDate, 'dd MMM yyyy hh:mm tt') AS AppointmentDateFormatted,
        a.Status,
        a.CancelledAt,
        d.ConsultationFee
    FROM Appointments a
    JOIN Patients p ON a.PatientId = p.PatientId
    JOIN Doctors  d ON a.DoctorId  = d.DoctorId
    WHERE a.DoctorId = @DoctorId;
END;
GO


-- REPORTING STORED PROCEDURES

-- SP 12: Consolidated Report
CREATE PROCEDURE sp_GetConsolidatedReport
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        a.AppointmentId,
        p.FullName AS PatientName,
        d.FullName AS DoctorName,
        d.Specialization,
        FORMAT(a.AppointmentDate, 'dd MMM yyyy hh:mm tt') AS AppointmentDate,
        a.Status,
        d.ConsultationFee AS Fee
    FROM Appointments a
    JOIN Patients p ON a.PatientId = p.PatientId
    JOIN Doctors  d ON a.DoctorId  = d.DoctorId
    ORDER BY a.AppointmentDate DESC;
END;
GO

exec sp_GetConsolidatedReport

-- SP 13: Total Appointments per Doctor (sirf >2 wale)
CREATE PROCEDURE sp_GetDoctorAppointmentCounts
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        d.DoctorId,
        d.FullName AS DoctorName,
        d.Specialization,
        COUNT(*) AS TotalAppointments
    FROM Appointments a
    JOIN Doctors d ON a.DoctorId = d.DoctorId
    GROUP BY d.DoctorId, d.FullName, d.Specialization
    HAVING COUNT(*) > 2;
END;
GO

-- SP 14: Revenue by Specialization
CREATE PROCEDURE sp_GetRevenueBySpecialization
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        d.Specialization,
        COUNT(*) AS TotalAppointments,
        SUM(d.ConsultationFee) AS TotalRevenue
    FROM Appointments a
    JOIN Doctors d ON a.DoctorId = d.DoctorId
    WHERE a.Status != 'Cancelled'
    GROUP BY d.Specialization;
END;
GO

-- SP 15: Duplicate Bookings
CREATE PROCEDURE sp_GetDuplicateBookings
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.FullName AS PatientName,
        d.FullName AS DoctorName,
        d.Specialization,
        CAST(a.AppointmentDate AS DATE) AS BookingDate,
        COUNT(*) AS BookingCount
    FROM Appointments a
    JOIN Patients p ON a.PatientId = p.PatientId
    JOIN Doctors  d ON a.DoctorId  = d.DoctorId
    GROUP BY
        p.PatientId, p.FullName,
        d.DoctorId,  d.FullName, d.Specialization,
        CAST(a.AppointmentDate AS DATE)
    HAVING COUNT(*) > 1;
END;
GO