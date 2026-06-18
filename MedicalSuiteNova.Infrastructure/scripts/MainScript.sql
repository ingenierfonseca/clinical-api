USE master
GO
IF DB_ID('ClinicalSuiteNovaDB') IS NOT NULL
BEGIN
    ALTER DATABASE ClinicalSuiteNovaDB
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

    DROP DATABASE ClinicalSuiteNovaDB;
END
GO
CREATE DATABASE ClinicalSuiteNovaDB;
GO
USE ClinicalSuiteNovaDB
GO
CREATE TABLE StaffType (
    Id TINYINT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(250) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO
INSERT INTO StaffType (Name, Description) VALUES
('Administrador', 'Administrador del sistema con acceso total'),
('Doctor', 'Médico o especialista de la clínica'),
('Recepcionista', 'Personal de recepción y atención al paciente'),
('Asistente Médico', 'Asistente que apoya en procedimientos clínicos'),
('Cajero', 'Personal encargado de cobros y facturación');
GO
CREATE TABLE Staff (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Gender VARCHAR(20) NOT NULL,
    Phone NVARCHAR(15) NULL,
    Email NVARCHAR(60) NULL,
    HireDate DATETIME NULL,
    Address VARCHAR(100) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    Avatar NVARCHAR(500) NULL,
    StaffTypeId TINYINT NOT NULL,
    BirthDate DATE NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Staff_StaffType FOREIGN KEY (StaffTypeId) REFERENCES StaffType(Id)
);
GO
INSERT INTO Staff(FirstName, LastName, Gender, Phone, Email, HireDate, StaffTypeId, BirthDate)
VALUES ('Marlon', 'Fonseca', 'Masculino', '86422597', 'ingenierfonseca@gmail.com', GETDATE(), 1, '03/21/1989'),
    ('Melissa', 'Fonseca', 'Femenino', '86422597', 'eliafonseca@gmail.com', GETDATE(), 1, '05/22/1987')
GO
CREATE TABLE Currency (
    Id TINYINT PRIMARY KEY,
    Name varchar(50) NOT NULL,
	Code varchar(5) NOT NULL,
	Symbol varchar(3) not null
);
GO
INSERT INTO Currency (Id, Code, Symbol, Name) VALUES 
(1, 'NIO', 'C$', 'Córdoba Nicaragüense' ),
(2, 'USD', '$', 'Dólar');
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] identity(1,1) primary key,
    [DNI] NVARCHAR(20) NOT NULL,
	[FirstName] [NVARCHAR](50) NOT NULL,
	[LastName] [NVARCHAR](50) NOT NULL,
    Gender VARCHAR(20) NOT NULL,
	BirthDate DATE NULL,
	[Phone] [NVARCHAR](15),
	Email NVARCHAR(60),
    Address VARCHAR(100) NULL,
	Avatar NVARCHAR(500),
    [Balance] DECIMAL(18,2) NOT NULL DEFAULT 0,
	CreatedAt DATETIME DEFAULT GETDATE(),
    [CurrencyId] TINYINT NULL,
    CONSTRAINT UQ_Customer_DNI UNIQUE ([DNI]),
    CONSTRAINT FK_Customer_Currency FOREIGN KEY (CurrencyId) REFERENCES Currency(Id)
)
GO
CREATE TABLE Services (
    Id TINYINT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT UQ_Services_Name UNIQUE (Name)
);
GO
INSERT Services (Name) VALUES('Odontologia')
GO
CREATE TABLE Specialties (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
	ServiceId TINYINT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT UQ_Specialties_Name UNIQUE (Name),
	CONSTRAINT FK_Specialties_Service FOREIGN KEY (ServiceId) REFERENCES Services(Id),
);
GO
INSERT Specialties (Name, ServiceId) VALUES('Dentista General', 1),('Ortodoncia', 1),('Endodoncia', 1)
GO
create table Doctor(
	Id integer identity (1,1) primary key,
    StaffId INT NOT NULL,
    ServiceId TINYINT NOT NULL,
	SpecialtyId INT NOT NULL,
    Title VARCHAR(10) NOT NULL,
    CONSTRAINT FK_Doctor_Service FOREIGN KEY (ServiceId) REFERENCES Services(Id),
    CONSTRAINT FK_Doctor_Specialty FOREIGN KEY (SpecialtyId) REFERENCES Specialties(Id),
    CONSTRAINT FK_Doctor_Staff FOREIGN KEY (StaffId) REFERENCES Staff(Id)
)
GO
INSERT INTO DOCTOR (StaffId, ServiceId, SpecialtyId, Title) VALUES (2,1,1,'Dra.');
GO
create table AppointmentType(
	Id TINYINT identity (1,1) primary key,
	Name varchar(50) NOT NULL,
	Description varchar(100),
	DurationMinutes INT
)
GO
INSERT AppointmentType (Name, DurationMinutes) VALUES ('Revisión General', 30)
GO
CREATE TABLE ResourceType (
    Id TINYINT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL
);
GO
INSERT INTO ResourceType VALUES
(1,'Sillon Odontologia'),
(2,'Consultorio')
GO
CREATE TABLE Resource (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ResourceTypeId TINYINT NOT NULL,
    Name VARCHAR(100) NOT NULL,
    Description VARCHAR(500) NULL,
    Capacity INT NULL,
    Color VARCHAR(20) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL,
    CONSTRAINT FK_Resource_ResourceType FOREIGN KEY (ResourceTypeId) REFERENCES ResourceType(Id)
);
GO
CREATE TABLE AppointmentStatus (
    Id TINYINT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL
);
GO
INSERT INTO AppointmentStatus VALUES
(1,'Pending'),
(2,'Confirmed'),
(3,'InProgress'),
(4,'Completed'),
(5,'Cancelled'),
(6,'NoShow'),
(7,'Rescheduled');
GO
CREATE TABLE Appointment (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    DoctorId INT NOT NULL,
    ResourceId INT NULL,
    AppointmentTypeId TINYINT NOT NULL,
    Date DATE NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    StatusId TINYINT NOT NULL,
    Notes VARCHAR(1000) NULL,
    CancellationReason VARCHAR(500) NULL,
    IsConfirmed BIT NOT NULL DEFAULT 0,
    ReminderSent BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL,
    CreatedBy INT NULL,
    UpdatedBy INT NULL,
    CONSTRAINT FK_Appointment_Customer FOREIGN KEY (CustomerId) REFERENCES Customer(Id),
    CONSTRAINT FK_Appointment_Doctor FOREIGN KEY (DoctorId) REFERENCES Doctor(Id),
    CONSTRAINT FK_Appointment_Resource FOREIGN KEY (ResourceId) REFERENCES Resource(Id),
    CONSTRAINT FK_Appointment_AppointmentType FOREIGN KEY (AppointmentTypeId) REFERENCES AppointmentType(Id),
    CONSTRAINT FK_Appointment_AppointmentStatus FOREIGN KEY (StatusId) REFERENCES AppointmentStatus(Id)
);
GO
CREATE TABLE [dbo].[ClinicalVisits] (
    [Id] [bigint] IDENTITY(1,1) PRIMARY KEY NOT NULL,
    [CustomerId] [int] NOT NULL,
    [VisitDate] [datetime] NOT NULL DEFAULT GETDATE(),
    [AppointmentId] [bigint] NULL,
    [DoctorId] [int] NOT NULL,
    [Notes] [nvarchar](300) NULL,
    CONSTRAINT [FK_Visits_Customer] FOREIGN KEY([CustomerId]) REFERENCES [dbo].[Customer]([Id]),
    CONSTRAINT [FK_Visits_Doctor] FOREIGN KEY([DoctorId]) REFERENCES [dbo].[Doctor]([Id]),
    CONSTRAINT [FK_Visits_Appointment] FOREIGN KEY([AppointmentId]) REFERENCES [Appointment]([Id])
)
GO
CREATE TABLE InvoiceStatus (
    Id TINYINT PRIMARY KEY,
    Name varchar(50) NOT NULL,
	Description varchar(100)
);
GO
INSERT INTO InvoiceStatus (Id, Name) VALUES 
(1, 'Pendiente'),
(2, 'Pagada'),
(3, 'Vencida'),
(4, 'Anulada'),
(5, 'Pago Parcial'),
(6, 'Reembolsada');
GO
CREATE TABLE PaymentTerm (
    Id TINYINT PRIMARY KEY,
    Name varchar(50) NOT NULL,
	Description varchar(100),
    DaysToDue INT NOT NULL DEFAULT 0
);
GO
INSERT INTO PaymentTerm (Id, Name, Description, DaysToDue) VALUES 
(1, 'Contado', 'Pago inmediato al recibir la factura', 0),
(2, 'Neto 15 días', 'Plazo de 15 días para cancelar', 15),
(3, 'Neto 30 días', 'Plazo de 30 días para cancelar', 30),
(4, 'Abono Recurrente', 'Se descuenta de saldo a favor previo', 0);
GO
CREATE TABLE PaymentType (
    Id TINYINT PRIMARY KEY,
    Name varchar(50) NOT NULL,
	Description varchar(100)
);
GO
INSERT INTO PaymentType (Id, Name, Description) VALUES 
(1, 'Efectivo', 'Pago con billetes y monedas en caja' ),
(2, 'Transferencia Bancaria', 'Transferencia directa a cuenta de la clínica'),
(3, 'Cheque', 'Pago con cheque certificado o personal'),
(4, 'Depósito Bancario', 'Depósito realizado en ventanilla o ATM');
GO
CREATE TABLE [dbo].[CustomerAccountLedger] (
    [Id] [bigint] IDENTITY(1,1) PRIMARY KEY NOT NULL,
    [CustomerId] [int] NOT NULL,
    
    -- Tipo de Movimiento
    -- 'CHARGE': Aumenta la deuda (Aceptación de plan, cuota mensual, recargos)
    -- 'PAYMENT': Disminuye la deuda (Abonos vinculados a facturas)
    -- 'CREDIT_NOTE': Ajustes a favor del paciente (Descuentos posteriores)
    [TransactionType] [nvarchar](20) NOT NULL, 

    -- Trazabilidad: Permite saber exactamente qué originó el movimiento
    [ReferenceId] [bigint] NOT NULL, 
    [ReferenceTable] [nvarchar](50) NOT NULL, -- Ej: 'SessionPlanMaster', 'Invoices'

    -- Importes y Moneda
    [Amount] [decimal](18, 4) NOT NULL, 
    [CurrencyId] [TINYINT] NOT NULL, -- FK a tu tabla Currencies
    [ExchangeRate] [decimal](18, 6) NOT NULL DEFAULT 1.0, 

    -- Snapshot de Saldo
    -- Es vital guardar el saldo acumulado en cada registro para reportes rápidos
    [BalanceAfter] [decimal](18, 4) NOT NULL, 

    [Description] [nvarchar](500) NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT GETDATE(),
    [CreatedBy] [nvarchar](100) NULL, -- Auditoría de quién generó el movimiento

    -- Relaciones
    CONSTRAINT [FK_CustomerLedger_Customer] FOREIGN KEY([CustomerId]) REFERENCES [dbo].[Customer]([Id]),
    CONSTRAINT [FK_CustomerLedger_Currency] FOREIGN KEY([CurrencyId]) REFERENCES [dbo].[Currency]([Id])
);

-- Índice para consultas rápidas de estado de cuenta por paciente
CREATE INDEX [IX_Ledger_Customer_Date] ON [dbo].[CustomerAccountLedger] ([CustomerId], [CreatedAt] DESC);
GO
CREATE TABLE [dbo].[Treatment] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    Description NVARCHAR(500) NULL,
	CurrencyId TINYINT NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    DurationMinutes INT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
	CONSTRAINT FK_Treatment_Currency
		FOREIGN KEY (CurrencyId) REFERENCES Currency(Id)
);
GO
INSERT INTO [dbo].[Treatment] ([Name], [Description], [CurrencyId], [Price], [DurationMinutes], [IsActive])
VALUES ('Ortodoncia', 'Tratamiento de correccion dental', 2, 2500, 180, 1)
GO
CREATE TABLE [dbo].[TreatmentCategory] (
    [Id] [tinyint] IDENTITY(1,1) PRIMARY KEY NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [Description] [nvarchar](250) NULL,
    [IsActive] [bit] NOT NULL DEFAULT 1,
    [CreatedAt] [datetime] NOT NULL DEFAULT GETDATE()
);
GO
INSERT INTO [dbo].[TreatmentCategory] ([Name], [Description])
VALUES 
('Ortodoncia', 'Tratamientos de corrección de posición dental y mordida.'),
('Implantología', 'Procedimientos de colocación de implantes y prótesis sobre implantes.'),
('Estética', 'Tratamientos cosméticos como blanqueamientos y carillas.'),
('General', 'Odontología preventiva, limpiezas y restauraciones básicas.'),
('Endodoncia', 'Tratamientos de conductos y salud pulpar.'),
('Rehabilitación', 'Prótesis fijas, removibles y restauración integral de la función oral.'),
('Cirugía', 'Procedimientos quirúrgicos orales y maxilofaciales, incluyendo extracciones complejas.');
GO
CREATE TABLE [dbo].[TreatmentPlanTemplate](
    [Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
    [Title] [nvarchar](150) NOT NULL,
    [Description] [nvarchar](500) NULL,
    -- Clasificación
    [CategoryId] [tinyint] NOT NULL, -- 'Ortodoncia', 'Cirugía', etc.
    [Complexity] [nvarchar](20) NULL, -- 'Baja', 'Media', 'Alta'
    [CurrencyId] TINYINT NOT NULL,
    -- Parámetros del Plan
    [EstimatedDurationMonths] [int] NULL,
    [BasePrice] [decimal](10, 2) NULL, -- Precio sugerido del paquete completo
    -- Control de Versiones y Estado
    [Version] [int] NOT NULL DEFAULT 1,
    [IsActive] [bit] NOT NULL DEFAULT 1,
    [CreatedAt] [datetime] NOT NULL DEFAULT GETDATE(),
    [CreatedBy] [int] NOT NULL, -- ID del Usuario/Doctor
    CONSTRAINT [FK_TreatmentPlanTemplate_Category] FOREIGN KEY([CategoryId]) REFERENCES [dbo].[TreatmentCategory]([Id]),
    CONSTRAINT FK_TreatmentPlanTemplate_Currency FOREIGN KEY (CurrencyId) REFERENCES Currency(Id),
);
GO
INSERT INTO [dbo].[TreatmentPlanTemplate] 
([Title], [Description], [CategoryId], [Complexity], [CurrencyId], [EstimatedDurationMonths], [BasePrice], [Version], [IsActive], [CreatedAt], [CreatedBy])
VALUES
-- 1. Ortodoncia
('Ortodoncia Metálica Convencional', 'Tratamiento correctivo completo con brackets metálicos de acero inoxidable.', 1, 'Alta', 2, 24, 2500.00, 1, 1, GETDATE(), 1),
('Ortodoncia Estética (Zafiro)', 'Corrección dental mediante brackets transparentes de alta estética.', 1, 'Alta', 2, 18, 3200.00, 1, 1, GETDATE(), 1),

-- 2. Implantología
('Rehabilitación sobre Implante Dental', 'Fase quirúrgica y protésica para la sustitución de una pieza dental.', 2, 'Media', 2, 6, 1200.00, 1, 1, GETDATE(), 1),

-- 3. Estética Dental
('Diseño de Sonrisa (Carillas Porcelana)', 'Transformación estética mediante carillas de porcelana E-Max (6-8 piezas).', 3, 'Alta', 2, 2, 4500.00, 1, 1, GETDATE(), 1),
('Blanqueamiento Dental Combinado', 'Sesión clínica de luz LED más kit de refuerzo ambulatorio en casa.', 3, 'Baja', 2, 1, 350.00, 1, 1, GETDATE(), 1),

-- 4. Odontología General / Preventiva
('Saneamiento Básico y Prevención', 'Limpieza profunda (Scalling), aplicación de flúor y sellantes.', 4, 'Baja', 2, 1, 120.00, 1, 1, GETDATE(), 1),
('Restauración Estética Completa', 'Remoción de amalgamas antiguas y sustitución por resinas compuestas.', 4, 'Baja', 2, 1, 200.00, 1, 1, GETDATE(), 1),

-- 5. Endodoncia
('Tratamiento de Conducto (Molar)', 'Terapia endodóntica multirradicular para salvar la pieza dental.', 5, 'Media', 2, 1, 250.00, 1, 1, GETDATE(), 1),

-- 6. Rehabilitación Oral
('Prótesis Total Removible (Superior/Inferior)', 'Confección de dentadura completa para paciente edéntulo.', 6, 'Media', 2, 3, 800.00, 1, 1, GETDATE(), 1),

-- 7. Cirugía
('Cirugía de Terceros Molares (Cordales)', 'Extracción quirúrgica de 4 muelas del juicio bajo anestesia local.', 7, 'Media', 2, 1, 600.00, 1, 1, GETDATE(), 1);
GO
CREATE TABLE [dbo].[TreatmentPlanTemplateItem](
    [Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
    [TemplateId] [int] NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [Description] [nvarchar](250) NULL,
    [TreatmentId] [int] NOT NULL, -- Relación con tu tabla existente
    [Order] [tinyint] NOT NULL, -- Para saber en qué orden aparecen
    CONSTRAINT [FK_TemplateItem_Template] FOREIGN KEY([TemplateId]) REFERENCES [dbo].[TreatmentPlanTemplate]([Id]),
    CONSTRAINT [FK_TemplateItem_Treatment] FOREIGN KEY([TreatmentId]) REFERENCES [dbo].[Treatment]([Id]),
);
GO
INSERT INTO [dbo].[TreatmentPlanTemplateItem] ([TemplateId], [TreatmentId], [Name], [Order])
VALUES 
(1, 1, 'Diagnóstico y planificación', 1), -- 1 sería el ID de 'Ortodoncia'
(1, 1, 'Colocación de aparatología', 2),  -- 1 sería el ID de 'Ortodoncia'
(1, 1, 'Alineación y nivelación', 3),     -- 1 sería el ID de 'Ortodoncia'
(1, 1, 'Cierre de espacios', 4),          -- 1 sería el ID de 'Ortodoncia'
(1, 1, 'Detalle y acabado', 5),           -- 1 sería el ID de 'Ortodoncia'
(1, 1, 'Retención', 6);                   -- 1 sería el ID de 'Ortodoncia'
GO
CREATE TABLE ConsultationType (
    Id TINYINT PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(100) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT UQ_ConsultationType_Name UNIQUE (Name)
);
GO
INSERT ConsultationType (Id, Name) 
VALUES (1, 'Inicial / Primera vez'), 
	(2, 'Evolución / Seguimiento'), 
	(3, 'Lectura de exámenes'),
	(4, 'Urgencia'),
	(5, 'Procedimiento')
GO
CREATE TABLE [dbo].[ClinicalSession] (
    [Id] [bigint] IDENTITY(1,1) PRIMARY KEY,
    [CustomerId] [int] NOT NULL,
    [DoctorId] [int] NOT NULL,
    [Date] [datetime] DEFAULT GETDATE(),
    [ReasonForVisit] [nvarchar](200) NULL, -- "Dolor en molar inferior"
    ConsultationSpecialtyId TINYINT NOT NULL,
    ConsultationTypeId TINYINT NOT NULL,
    ConsultationId BIGINT NULL,
    CONSTRAINT [FK_Session_Patient] FOREIGN KEY([CustomerId]) REFERENCES [dbo].[Customer]([Id]),
    CONSTRAINT [FK_Session_Doctor] FOREIGN KEY([DoctorId]) REFERENCES [dbo].[Doctor]([Id]),
    CONSTRAINT [FK_Session_Specialty] FOREIGN KEY([ConsultationSpecialtyId]) REFERENCES [dbo].[Services]([Id]),
    CONSTRAINT [FK_Session_ConsultationType] FOREIGN KEY([ConsultationTypeId]) REFERENCES [dbo].[ConsultationType]([Id]),
    CONSTRAINT [FK_Session_Consultation] FOREIGN KEY([ConsultationId]) REFERENCES [dbo].[ClinicalSession]([Id])
);
GO
CREATE TABLE ClinicalNotes (
    Id INT IDENTITY(1,1) NOT NULL,
    ClinicalSessionId BIGINT NOT NULL,
    DoctorId INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_ClinicalNotes_CreatedAt DEFAULT SYSUTCDATETIME(),
    Note NVARCHAR(MAX) NOT NULL CONSTRAINT DF_ClinicalNotes_Note DEFAULT '',
    IsPrivate BIT NOT NULL,
    CONSTRAINT PK_ClinicalNotes PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_ClinicalNotes_ClinicalSession FOREIGN KEY (ClinicalSessionId) 
        REFERENCES ClinicalSession (Id),
    CONSTRAINT FK_ClinicalNotes_Doctor FOREIGN KEY (DoctorId) 
        REFERENCES Doctor (Id)
);
GO
CREATE TABLE ClinicalFile (
    Id INT IDENTITY(1,1) NOT NULL,
    ClinicalSessionId BIGINT NOT NULL,
    CustomerId INT NOT NULL,
    TypeId TINYINT NOT NULL,
    Url NVARCHAR(300) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE() 
    CONSTRAINT PK_ClinicalFile PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_ClinicalFile_ClinicalSession FOREIGN KEY (ClinicalSessionId) 
        REFERENCES ClinicalSession (Id),
    CONSTRAINT FK_ClinicalFile_Customer FOREIGN KEY (CustomerId) 
        REFERENCES Customer (Id)
);
GO
CREATE TABLE [dbo].[SessionPlanMaster](
    [Id] [bigint] IDENTITY(1,1) PRIMARY KEY NOT NULL,
    [SessionId] [bigint] NOT NULL,
    [CustomerId] [int] NOT NULL,
    [PaymentTermId] [tinyint] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
    [Status] [nvarchar](20) NOT NULL DEFAULT 'En Proceso', -- 'Pendiente', 'Completado', 'Suspendido'
    [StartDate] [datetime] NOT NULL DEFAULT GETDATE(),
    [EndDate] [datetime] NULL,
    [TotalEstimatedPrice] [decimal](10, 2) NOT NULL,
    [CurrencyId] [tinyint] NOT NULL,
    [IsFinanced] [BIT] NOT NULL,
    [DownPayment] [DECIMAL](18,2) NULL,
	[Comments] [nvarchar](300) NULL,
    CONSTRAINT [FK_SessionPlanMaster_ClinicalSession] FOREIGN KEY([SessionId]) REFERENCES [dbo].[ClinicalSession] ([Id]),
    CONSTRAINT [FK_SessionPlanMaster_Currency] FOREIGN KEY([CurrencyId]) REFERENCES [dbo].[Currency] ([Id]),
    CONSTRAINT [FK_SessionPlanMaster_Customer] FOREIGN KEY([CustomerId]) REFERENCES [dbo].[Customer] ([Id]),
    CONSTRAINT [FK_SessionPlanMaster_PaymentTerm] FOREIGN KEY([PaymentTermId]) REFERENCES [dbo].[PaymentTerm] ([Id])
);
GO
CREATE TABLE [dbo].[SessionPlanDetail](
    [Id] [bigint] IDENTITY(1,1) PRIMARY KEY NOT NULL,
    [SessionPlanMasterId] [bigint] NOT NULL,
    [TreatmentPlanTemplateItemId] [int] NOT NULL, -- Relación con tu tabla existente
    [Status] [nvarchar](20) NOT NULL DEFAULT 'Pendiente', -- 'Pendiente', 'En Proceso', 'Completo'
    [CompletedAt] [datetime] NULL,
    [Comments] [nvarchar](max) NULL, -- Notas clínicas de este paso
    CONSTRAINT [FK_Detail_Master] FOREIGN KEY([SessionPlanMasterId]) REFERENCES [dbo].[SessionPlanMaster]([Id]),
    CONSTRAINT [FK_Detail_Treatment] FOREIGN KEY([TreatmentPlanTemplateItemId]) REFERENCES [dbo].[TreatmentPlanTemplateItem]([Id])
);
GO
CREATE TABLE Invoice (
    Id INT PRIMARY KEY IDENTITY,
    Number VARCHAR(50) NOT NULL,
    CustomerId INT NOT NULL,
	CurrencyId TINYINT NOT NULL,
	PaymentTermId TINYINT NOT NULL,
    IssueDate DATETIME NOT NULL,
	DueDate DATETIME NOT NULL,
    Subtotal DECIMAL(18,2) NOT NULL,
    TaxTotal DECIMAL(18,2) NOT NULL,
	DiscountTotal DECIMAL(18,2) NOT NULL,
    Total DECIMAL(18,2) NOT NULL,
    StatusId TINYINT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
	CreatedBy VARCHAR(50),
    [OriginType] [nvarchar](20) NULL,
	[SessionPlanMasterId] [bigint] NULL,
	CONSTRAINT FK_Invoice_Customer
        FOREIGN KEY (CustomerId) REFERENCES Customer(Id),
	CONSTRAINT FK_Invoice_InvoiceStatus
		FOREIGN KEY (StatusId) REFERENCES InvoiceStatus(Id),
	CONSTRAINT FK_Invoice_Currency
		FOREIGN KEY (CurrencyId) REFERENCES Currency(Id),
	CONSTRAINT FK_Invoice_PaymentTerm
		FOREIGN KEY (PaymentTermId) REFERENCES PaymentTerm(Id),
    CONSTRAINT [FK_Invoice_SessionPlanMaster_SessionPlanMasterId] FOREIGN KEY([SessionPlanMasterId])
        REFERENCES [dbo].[SessionPlanMaster] ([Id])
);
GO
CREATE NONCLUSTERED INDEX IX_Invoice_SessionPlanMasterId 
ON Invoice(SessionPlanMasterId) 
WHERE SessionPlanMasterId IS NOT NULL;
GO
CREATE TABLE InvoiceItem (
    Id INT PRIMARY KEY IDENTITY,
    InvoiceId INT NOT NULL,
	ProductId INT NULL,
    ServiceId INT NULL,
    Description VARCHAR(250),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    TaxAmount DECIMAL(18,2) NOT NULL,
	Discount DECIMAL(18,2) NOT NULL,
    LineTotal DECIMAL(18,2) NOT NULL,
    OriginalCurrencyId TINYINT NOT NULL,
    OriginalPrice decimal(18, 2) NOT NULL,
	CONSTRAINT FK_InvoiceDetail_Invoice
        FOREIGN KEY (InvoiceId) REFERENCES Invoice(Id),
    CONSTRAINT FK_InvoiceItem_Currency
        FOREIGN KEY (OriginalCurrencyId) REFERENCES Currency(Id)
);
GO
CREATE TABLE Payment (
    Id INT PRIMARY KEY IDENTITY,
    InvoiceId INT NOT NULL,
	CustomerId INT NOT NULL,
    CurrencyId TINYINT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Date DATETIME NOT NULL,
    PaymentTypeId TINYINT, -- Cash, Card, Transfer
    CONSTRAINT FK_Payment_Invoice
        FOREIGN KEY (InvoiceId) REFERENCES Invoice(Id),
	CONSTRAINT FK_Payment_Customer
        FOREIGN KEY (CustomerId) REFERENCES Customer(Id),
	CONSTRAINT FK_Payment_PaymentMethod
        FOREIGN KEY (PaymentTypeId) REFERENCES PaymentType(Id),
    CONSTRAINT FK_Payment_Currency
        FOREIGN KEY (CurrencyId) REFERENCES Currency(Id)
);
GO
CREATE TABLE ExchangeRates (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FromCurrencyId TINYINT NOT NULL, -- Moneda origen (ej: USD)
    ToCurrencyId TINYINT NOT NULL,   -- Moneda destino (ej: CRC)
    Rate DECIMAL(18, 6) NOT NULL, -- El factor de conversión
    RateDate DATETIME NOT NULL DEFAULT GETDATE(), -- Cuándo se registró
    IsActive BIT DEFAULT 1,       -- Para desactivar tasas erróneas
    Source NVARCHAR(50),          -- Opcional: Banco Central, Manual, Reuters, etc.
    
    CONSTRAINT FK_FromCurrency FOREIGN KEY (FromCurrencyId) REFERENCES Currency(Id),
    CONSTRAINT FK_ToCurrency FOREIGN KEY (ToCurrencyId) REFERENCES Currency(Id)
);
GO
-- Índice para búsquedas rápidas por fecha y monedas
CREATE INDEX IX_ExchangeRates_Lookup ON ExchangeRates (FromCurrencyId, ToCurrencyId, RateDate DESC);
GO
INSERT INTO ExchangeRates (FromCurrencyId, ToCurrencyId, Rate, RateDate, IsActive, Source)
VALUES (2, 1, 36.550000, GETDATE(), 1, 'Manual/Banco Central');
INSERT INTO ExchangeRates (FromCurrencyId, ToCurrencyId, Rate, RateDate, IsActive, Source)
VALUES (1, 2, 0.027360, GETDATE(), 1, 'Manual/Banco Central');
GO
CREATE TABLE [dbo].[Role](
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(50) NOT NULL,
    [Description] NVARCHAR(250) NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    CONSTRAINT UQ_Role_Name UNIQUE ([Name])
);
GO
INSERT INTO [dbo].[Role] ([Name], [Description]) VALUES
('SuperAdmin', 'Administrador del sistema con acceso total'),
('Admin', 'Administrador del sistema con acceso total'),
('Doctor', 'Médico o especialista de la clínica'),
('Staff', 'Personal de recepción y atención al paciente');
GO
CREATE TABLE [dbo].[User](
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Username] NVARCHAR(50) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL,
    [PasswordHash] NVARCHAR(500) NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [RefreshToken] NVARCHAR(500) NULL,
    [RefreshTokenExpiry] DATETIME NULL,
    [StaffId] INT NULL,
    [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_User_Username UNIQUE ([Username]),
    CONSTRAINT UQ_User_Email UNIQUE ([Email]),
    CONSTRAINT FK_User_Staff FOREIGN KEY ([StaffId]) REFERENCES [dbo].[Staff]([Id])
);
GO
CREATE TABLE [dbo].[Permission](
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(250) NULL,
    [Module] NVARCHAR(50) NOT NULL,
    CONSTRAINT UQ_Permission_Name UNIQUE ([Name])
);
GO
INSERT INTO [dbo].[Permission] ([Name], [Description], [Module]) VALUES
('appointments.create', 'Crear citas', 'Citas'),
('appointments.view', 'Ver citas', 'Citas'),
('appointments.update', 'Actualizar citas', 'Citas'),
('patients.create', 'Crear pacientes', 'Pacientes'),
('patients.view', 'Ver pacientes', 'Pacientes'),
('patients.update', 'Actualizar pacientes', 'Pacientes'),
('clinical.create', 'Crear expediente clínico', 'Expediente Clínico'),
('clinical.view', 'Ver expediente clínico', 'Expediente Clínico'),
('billing.create', 'Crear facturas', 'Facturación'),
('billing.view', 'Ver facturas', 'Facturación'),
('billing.pay', 'Registrar pagos', 'Facturación'),
('upload-patients.view', 'Ver carga', 'Carga'),
('upload-patients.create', 'Crear carga pacientes', 'Carga'),
('upload-patients.update', 'Actualizar carga pacientes', 'Carga'),
('administration-tretament-plan.view', 'Ver planes de tratamiento', 'Planes de tratamiento');
GO
CREATE TABLE [dbo].[UserRole](
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    CONSTRAINT PK_UserRole PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT FK_UserRole_User FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([Id]),
    CONSTRAINT FK_UserRole_Role FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role]([Id])
);
GO
CREATE TABLE [dbo].[RolePermission](
    [RoleId] INT NOT NULL,
    [PermissionId] INT NOT NULL,
    CONSTRAINT PK_RolePermission PRIMARY KEY ([RoleId], [PermissionId]),
    CONSTRAINT FK_RolePermission_Role FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role]([Id]),
    CONSTRAINT FK_RolePermission_Permission FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Permission]([Id])
);
GO
INSERT INTO [dbo].[RolePermission] ([RoleId], [PermissionId])
SELECT 1, Id FROM [dbo].[Permission];
GO
INSERT INTO [dbo].[User] ([Username], [Email], [PasswordHash], [IsActive], [StaffId])
VALUES ('admin', 'admin@clinica.com', '$2a$11$igebiSXTamBKbT//NOd5Z.sNDTSP0aduEV1fm2sKZnY8VWlQkm2j6', 1, 1),
    ('eliafonseca', 'eliafonseca@clinica.com', '$2a$11$igebiSXTamBKbT//NOd5Z.sNDTSP0aduEV1fm2sKZnY8VWlQkm2j6', 1, 2);
GO
INSERT INTO [dbo].[UserRole] ([UserId], [RoleId])
VALUES (1, 1), (2, 3);
