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
	Description varchar(100)
);
GO
INSERT INTO PaymentTerm (Id, Name, Description) VALUES 
(1, 'Contado', 'Pago inmediato al recibir la factura' ),
(2, 'Neto 15 días', 'Plazo de 15 días para cancelar'),
(3, 'Neto 30 días', 'Plazo de 30 días para cancelar'),
(4, 'Abono Recurrente', 'Se descuenta de saldo a favor previo');
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
	CONSTRAINT FK_InvoiceDetail_Invoice
        FOREIGN KEY (InvoiceId) REFERENCES Invoice(Id)
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
);*/