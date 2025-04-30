CREATE DATABASE "RentalControl";

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

GRANT ALL PRIVILEGES ON DATABASE "RentalControl" TO eduardo;

CREATE TABLE "Addresses" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Street" TEXT NOT NULL,
    "Number" TEXT NOT NULL,
    "Neighborhood" TEXT NOT NULL,
    "City" TEXT NOT NULL,
    "State" TEXT NOT NULL,
    "ZipCode" TEXT NOT NULL,
    "Country" TEXT NOT NULL,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "Tenants" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "AddressId" UUID NOT NULL REFERENCES "Addresses"("Id") ON DELETE CASCADE,
    "Name" TEXT NOT NULL,
    "Email" TEXT UNIQUE NOT NULL,
    "Phone" TEXT,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "Contracts" (
   "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
   "TenantId" UUID REFERENCES "Tenants"("Id") ON DELETE CASCADE,
   "AddressId" UUID REFERENCES "Addresses"("Id") ON DELETE CASCADE,
   "Deposit" NUMERIC(10, 2) NOT NULL,
   "Rent" NUMERIC(10, 2) NOT NULL,
   "Business" TEXT NOT NULL,
   "StartDate" DATE NOT NULL,
   "EndDate" DATE NOT NULL,
   "Status" TEXT CHECK ("Status" IN ('active', 'expired', 'terminated')) NOT NULL,
   "Type" TEXT CHECK ("Type" IN ('yearly')) NOT NULL,
   "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "Guarantors" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "AddressId" UUID NOT NULL REFERENCES "Addresses"("Id") ON DELETE CASCADE,
    "Name" TEXT NOT NULL,
    "Phone" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "ContractGuarantors" (
    "ContractId" UUID REFERENCES "Contracts"("Id") ON DELETE CASCADE,
    "GuarantorId" UUID REFERENCES "Guarantors"("Id") ON DELETE CASCADE,
    PRIMARY KEY ("ContractId", "GuarantorId")
);