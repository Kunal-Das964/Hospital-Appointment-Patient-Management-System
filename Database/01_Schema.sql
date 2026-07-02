-- PATIENTS TABLE
CREATE TABLE Patients (
    PatientId       INT IDENTITY(1,1) PRIMARY KEY,
    PatientCode     VARCHAR(20)  NOT NULL UNIQUE,
    FullName        NVARCHAR(100) NOT NULL,
    DateOfBirth     DATE         NOT NULL,
    Gender          CHAR(1)      NOT NULL CHECK (Gender IN ('M','F','O')),
    PhoneNumber     VARCHAR(15)  NOT NULL UNIQUE,
    Email           VARCHAR(100) NULL,
    IsActive        BIT          NOT NULL DEFAULT 1, 
    CreatedAt       DATETIME     NOT NULL DEFAULT GETDATE()
);
GO

-- TABLE 2 : DOCTORS
CREATE TABLE Doctors
(
    DoctorId        INT            IDENTITY(1,1) PRIMARY KEY,
    DoctorCode      VARCHAR(20)    NOT NULL UNIQUE,
    FullName        NVARCHAR(100)  NOT NULL,
    Specialization  NVARCHAR(100)  NOT NULL,
    PhoneNumber     VARCHAR(15)    NOT NULL UNIQUE,
    ConsultationFee DECIMAL(10, 2) NOT NULL,
    IsAvailable     BIT            NOT NULL DEFAULT 1,
    CreatedAt       DATETIME       NOT NULL DEFAULT GETDATE()
);
GO

-- TABLE 3 : APPOINTMENTS
CREATE TABLE Appointments
(
    AppointmentId   INT         IDENTITY(1,1) PRIMARY KEY,
    PatientId       INT         NOT NULL,
    DoctorId        INT         NOT NULL,
    AppointmentDate DATETIME    NOT NULL,
    Status          VARCHAR(20) NOT NULL DEFAULT 'Scheduled' CHECK (Status IN ('Scheduled', 'Completed', 'Cancelled')),
    CancelledAt     DATETIME    NULL,
    CreatedAt       DATETIME    NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Appointments_Patient
        FOREIGN KEY (PatientId) REFERENCES Patients(PatientId),

    CONSTRAINT FK_Appointments_Doctor
        FOREIGN KEY (DoctorId) REFERENCES Doctors(DoctorId)
);
GO