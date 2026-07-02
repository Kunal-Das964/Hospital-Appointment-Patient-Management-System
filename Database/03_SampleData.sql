-- DOCTORS DATA
INSERT INTO Doctors (DoctorCode, FullName, Specialization, PhoneNumber, ConsultationFee, IsAvailable)
VALUES
('DOC001', 'Dr. Rajesh Sharma',    'Cardiology',       '9876543201', 800.00,  1),
('DOC002', 'Dr. Priya Mehta',      'Dermatology',      '9876543202', 500.00,  1),
('DOC003', 'Dr. Anil Verma',       'Orthopedics',      '9876543203', 700.00,  1),
('DOC004', 'Dr. Sunita Rao',       'Neurology',        '9876543204', 900.00,  1),
('DOC005', 'Dr. Vikram Singh',     'Cardiology',       '9876543205', 850.00,  0),
('DOC006', 'Dr. Neha Joshi',       'Dermatology',      '9876543206', 450.00,  1),
('DOC007', 'Dr. Ramesh Gupta',     'Orthopedics',      '9876543207', 650.00,  1);
GO

-- PATIENTS DATA
INSERT INTO Patients (PatientCode, FullName, DateOfBirth, Gender, PhoneNumber, Email)
VALUES
('PAT001', 'Amit Kumar',      '1990-05-15', 'M', '9111111101', 'amit@gmail.com'),
('PAT002', 'Sneha Patel',     '1985-08-22', 'F', '9111111102', 'sneha@gmail.com'),
('PAT003', 'Rohit Sharma',    '2000-01-10', 'M', '9111111103', NULL),           
('PAT004', 'Kavita Singh',    '1978-11-30', 'F', '9111111104', 'kavita@gmail.com'),
('PAT005', 'Deepak Jain',     '1995-03-25', 'M', '9111111105', NULL),           
('PAT006', 'Pooja Verma',     '1992-07-14', 'F', '9111111106', 'pooja@gmail.com'),
('PAT007', 'Suresh Yadav',    '1988-09-05', 'M', '9111111107', NULL);           
GO

-- APPOINTMENTS DATA
INSERT INTO Appointments (PatientId, DoctorId, AppointmentDate, Status)
VALUES
(1, 1, DATEADD(DAY,  1, GETDATE()), 'Scheduled'),
(2, 1, DATEADD(DAY,  2, GETDATE()), 'Scheduled'),
(3, 1, DATEADD(DAY,  3, GETDATE()), 'Scheduled'),
(4, 2, DATEADD(DAY,  1, GETDATE()), 'Scheduled'),
(5, 2, DATEADD(DAY,  2, GETDATE()), 'Scheduled'),
(6, 2, DATEADD(DAY,  3, GETDATE()), 'Scheduled'),
(1, 3, DATEADD(DAY,  4, GETDATE()), 'Scheduled'),
(2, 3, DATEADD(DAY,  5, GETDATE()), 'Scheduled'),
(3, 1, DATEADD(DAY, -5, GETDATE()), 'Completed'),
(4, 2, DATEADD(DAY, -3, GETDATE()), 'Completed'),
(5, 1, DATEADD(DAY, -1, GETDATE()), 'Cancelled'),
(1, 4, DATEADD(DAY,  6, GETDATE()), 'Scheduled'),
(2, 4, DATEADD(DAY,  6, GETDATE()), 'Scheduled');
GO