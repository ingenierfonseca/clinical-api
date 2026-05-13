/*USE master
DROP DATABASE ClinicalSuiteNovaDB
CREATE DATABASE ClinicalSuiteNovaDB
USE ClinicalSuiteNovaDB
CREATE TABLE [dbo].[Customer](
	[Id] [int] identity(1,1) primary key,
	[FirstName] [nchar](50) NOT NULL,
	[LastName] [nchar](50) NOT NULL,
	Age TINYINT,
	[Phone] [nchar](15),
	Email nchar(60),
	Avatar nchar(500),
	CreatedAt DATETIME DEFAULT GETDATE()
)
GO
create table Doctor(
	Id integer identity (1,1) primary key,
	FirstName varchar(50) NOT NULL,
	LastName varchar(50) NOT NULL,
	Age TINYINT,
	Specialist varchar(100) NOT NULL,
	[Phone] [nchar](15)
)
GO
create table AppointmentType(
	Id TINYINT identity (1,1) primary key,
	Name varchar(50) NOT NULL,
	Description varchar(100),
	DurationMinutes INT
)
GO
create table Appointment(
	Id integer identity (1,1) primary key,
	CustomerId integer,
	AppointmentDate DateTime,
	DoctorId integer,
	AppointmentTypeId TINYINT,
	CONSTRAINT FK_Appointment_Patient
        FOREIGN KEY (CustomerId) REFERENCES Customer(Id),
	CONSTRAINT FK_Appointment_Doctor
        FOREIGN KEY (DoctorId) REFERENCES Doctor(Id),
	CONSTRAINT FK_Appointment_AppointmentType
        FOREIGN KEY (AppointmentTypeId) REFERENCES AppointmentType(Id)
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
CREATE TABLE Treatment (
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
		FOREIGN KEY (CurrencyId) REFERENCES Currency(Id);
);
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
([Title], [Description], [CategoryId], [Complexity], [EstimatedDurationMonths], [BasePrice], [Version], [IsActive], [CreatedAt], [CreatedBy])
VALUES
-- 1. Ortodoncia
('Ortodoncia Metálica Convencional', 'Tratamiento correctivo completo con brackets metálicos de acero inoxidable.', 1, 'Alta', 24, 2500.00, 1, 1, GETDATE(), 1),
('Ortodoncia Estética (Zafiro)', 'Corrección dental mediante brackets transparentes de alta estética.', 1, 'Alta', 18, 3200.00, 1, 1, GETDATE(), 1),

-- 2. Implantología
('Rehabilitación sobre Implante Dental', 'Fase quirúrgica y protésica para la sustitución de una pieza dental.', 2, 'Media', 6, 1200.00, 1, 1, GETDATE(), 1),

-- 3. Estética Dental
('Diseño de Sonrisa (Carillas Porcelana)', 'Transformación estética mediante carillas de porcelana E-Max (6-8 piezas).', 3, 'Alta', 2, 4500.00, 1, 1, GETDATE(), 1),
('Blanqueamiento Dental Combinado', 'Sesión clínica de luz LED más kit de refuerzo ambulatorio en casa.', 3, 'Baja', 1, 350.00, 1, 1, GETDATE(), 1),

-- 4. Odontología General / Preventiva
('Saneamiento Básico y Prevención', 'Limpieza profunda (Scalling), aplicación de flúor y sellantes.', 4, 'Baja', 1, 120.00, 1, 1, GETDATE(), 1),
('Restauración Estética Completa', 'Remoción de amalgamas antiguas y sustitución por resinas compuestas.', 4, 'Baja', 1, 200.00, 1, 1, GETDATE(), 1),

-- 5. Endodoncia
('Tratamiento de Conducto (Molar)', 'Terapia endodóntica multirradicular para salvar la pieza dental.', 5, 'Media', 1, 250.00, 1, 1, GETDATE(), 1),

-- 6. Rehabilitación Oral
('Prótesis Total Removible (Superior/Inferior)', 'Confección de dentadura completa para paciente edéntulo.', 6, 'Media', 3, 800.00, 1, 1, GETDATE(), 1),

-- 7. Cirugía
('Cirugía de Terceros Molares (Cordales)', 'Extracción quirúrgica de 4 muelas del juicio bajo anestesia local.', 7, 'Media', 1, 600.00, 1, 1, GETDATE(), 1);
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
(1, 11, 'Diagnóstico y planificación', 1), -- 11 sería el ID de 'Ortodoncia'
(1, 11, 'Colocación de aparatología', 2),  -- 11 sería el ID de 'Ortodoncia'
(1, 11, 'Alineación y nivelación', 3),     -- 11 sería el ID de 'Ortodoncia'
(1, 11, 'Cierre de espacios', 4),          -- 11 sería el ID de 'Ortodoncia'
(1, 11, 'Detalle y acabado', 5),           -- 11 sería el ID de 'Ortodoncia'
(1, 11, 'Retención', 6);                   -- 11 sería el ID de 'Ortodoncia'
GO
CREATE TABLE [dbo].[ClinicalSession] (
    [Id] [bigint] IDENTITY(1,1) PRIMARY KEY,
    [CustomerId] [int] NOT NULL,
    [DoctorId] [int] NOT NULL,
    [Date] [datetime] DEFAULT GETDATE(),
    [ReasonForVisit] [nvarchar](200) NULL, -- "Dolor en molar inferior"
    [ClinicalNotes] [nvarchar](300) NULL,
    CONSTRAINT [FK_Session_Patient] FOREIGN KEY([CustomerId]) REFERENCES [dbo].[Customer]([Id]),
    CONSTRAINT [FK_Session_Doctor] FOREIGN KEY([DoctorId]) REFERENCES [dbo].[Doctor]([Id])
);
GO
CREATE TABLE [dbo].[SessionPlanMaster](
    [Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
    [PatientId] [int] NOT NULL, -- Tu tabla de pacientes
    [DoctorId] [int] NOT NULL,  -- Tu tabla de doctores
    [TemplateId] [int] NULL,    -- Opcional, por si es un plan manual
    [Status] [nvarchar](20) NOT NULL DEFAULT 'En Proceso', -- 'Pendiente', 'Completado', 'Suspendido'
    [StartDate] [datetime] NOT NULL DEFAULT GETDATE(),
    [EndDate] [datetime] NULL,
    [TotalEstimatedPrice] [decimal](10, 2) NOT NULL
);

CREATE TABLE [dbo].[PatientPlanDetail](
    [Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
    [PatientPlanMasterId] [int] NOT NULL,
    [TreatmentPlanTemplateItemId] [int] NOT NULL, -- Relación con tu tabla existente
    [Status] [nvarchar](20) NOT NULL DEFAULT 'Pendiente', -- 'Pendiente', 'En Proceso', 'Completo'
    [CompletedAt] [datetime] NULL,
    [Comments] [nvarchar](max) NULL, -- Notas clínicas de este paso
    CONSTRAINT [FK_Detail_Master] FOREIGN KEY([PatientPlanMasterId]) REFERENCES [dbo].[PatientPlanMaster]([Id]),
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
	CONSTRAINT FK_Invoice_Customer
        FOREIGN KEY (CustomerId) REFERENCES Customer(Id),
	CONSTRAINT FK_Invoice_InvoiceStatus
		FOREIGN KEY (StatusId) REFERENCES InvoiceStatus(Id),
	CONSTRAINT FK_Invoice_Currency
		FOREIGN KEY (CurrencyId) REFERENCES Currency(Id),
	CONSTRAINT FK_Invoice_PaymentTerm
		FOREIGN KEY (PaymentTermId) REFERENCES PaymentTerm(Id)
);
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
    Amount DECIMAL(18,2) NOT NULL,
    Date DATETIME NOT NULL,
    PaymentTypeId TINYINT, -- Cash, Card, Transfer
    CONSTRAINT FK_Payment_Invoice
        FOREIGN KEY (InvoiceId) REFERENCES Invoice(Id),
	CONSTRAINT FK_Payment_Customer
        FOREIGN KEY (CustomerId) REFERENCES Customer(Id),
	CONSTRAINT FK_Payment_PaymentMethod
        FOREIGN KEY (PaymentTypeId) REFERENCES PaymentType(Id)
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
*/