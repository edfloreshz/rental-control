package dto

import (
	"time"

	"github.com/google/uuid"
)

// Address DTOs
type CreateAddressRequest struct {
	Street       string `json:"street" binding:"required"`
	Number       string `json:"number" binding:"required"`
	Neighborhood string `json:"neighborhood" binding:"required"`
	City         string `json:"city" binding:"required"`
	State        string `json:"state" binding:"required"`
	ZipCode      string `json:"zip_code" binding:"required"`
	Country      string `json:"country" binding:"required"`
}

type UpdateAddressRequest struct {
	ID           uuid.UUID `json:"id" binding:"required"`
	Street       string    `json:"street"`
	Number       string    `json:"number"`
	Neighborhood string    `json:"neighborhood"`
	City         string    `json:"city"`
	State        string    `json:"state"`
	ZipCode      string    `json:"zip_code"`
	Country      string    `json:"country"`
}

type AddressResponse struct {
	ID           uuid.UUID `json:"id"`
	Street       string    `json:"street"`
	Number       string    `json:"number"`
	Neighborhood string    `json:"neighborhood"`
	City         string    `json:"city"`
	State        string    `json:"state"`
	ZipCode      string    `json:"zip_code"`
	Country      string    `json:"country"`
	CreatedAt    time.Time `json:"created_at"`
}

// Tenant DTOs
type CreateTenantRequest struct {
	Name      string    `json:"name" binding:"required"`
	Email     string    `json:"email" binding:"required,email"`
	Phone     string    `json:"phone" binding:"required"`
	AddressID uuid.UUID `json:"address_id" binding:"required"`
}

type UpdateTenantRequest struct {
	ID        uuid.UUID `json:"id" binding:"required"`
	Name      string    `json:"name"`
	Email     string    `json:"email"`
	Phone     string    `json:"phone"`
	AddressID uuid.UUID `json:"address_id"`
}

type TenantResponse struct {
	ID        uuid.UUID       `json:"id"`
	Name      string          `json:"name"`
	Email     string          `json:"email"`
	Phone     string          `json:"phone"`
	AddressID uuid.UUID       `json:"address_id"`
	CreatedAt time.Time       `json:"created_at"`
	Address   AddressResponse `json:"address"`
}

// Guarantor DTOs
type CreateGuarantorRequest struct {
	Name      string    `json:"name" binding:"required"`
	Email     string    `json:"email" binding:"required,email"`
	Phone     string    `json:"phone" binding:"required"`
	AddressID uuid.UUID `json:"address_id" binding:"required"`
}

type UpdateGuarantorRequest struct {
	ID        uuid.UUID `json:"id" binding:"required"`
	Name      string    `json:"name"`
	Email     string    `json:"email"`
	Phone     string    `json:"phone"`
	AddressID uuid.UUID `json:"address_id"`
}

type GuarantorResponse struct {
	ID        uuid.UUID       `json:"id"`
	Name      string          `json:"name"`
	Email     string          `json:"email"`
	Phone     string          `json:"phone"`
	AddressID uuid.UUID       `json:"address_id"`
	CreatedAt time.Time       `json:"created_at"`
	Address   AddressResponse `json:"address"`
}

// Contract DTOs
type CreateContractRequest struct {
	TenantID     uuid.UUID   `json:"tenant_id" binding:"required"`
	AddressID    uuid.UUID   `json:"address_id" binding:"required"`
	Deposit      float64     `json:"deposit" binding:"required"`
	Rent         float64     `json:"rent" binding:"required"`
	Business     string      `json:"business" binding:"required"`
	StartDate    time.Time   `json:"start_date" binding:"required"`
	EndDate      time.Time   `json:"end_date" binding:"required"`
	Status       string      `json:"status" binding:"required"`
	Type         string      `json:"type" binding:"required"`
	GuarantorIDs []uuid.UUID `json:"guarantor_ids"`
}

type UpdateContractRequest struct {
	ID           uuid.UUID   `json:"id" binding:"required"`
	TenantID     uuid.UUID   `json:"tenant_id"`
	AddressID    uuid.UUID   `json:"address_id"`
	Deposit      float64     `json:"deposit"`
	Rent         float64     `json:"rent"`
	Business     string      `json:"business"`
	StartDate    time.Time   `json:"start_date"`
	EndDate      time.Time   `json:"end_date"`
	Status       string      `json:"status"`
	Type         string      `json:"type"`
	GuarantorIDs []uuid.UUID `json:"guarantor_ids"`
}

type ContractResponse struct {
	ID         uuid.UUID           `json:"id"`
	TenantID   uuid.UUID           `json:"tenant_id"`
	AddressID  uuid.UUID           `json:"address_id"`
	Deposit    float64             `json:"deposit"`
	Rent       float64             `json:"rent"`
	Business   string              `json:"business"`
	StartDate  time.Time           `json:"start_date"`
	EndDate    time.Time           `json:"end_date"`
	Status     string              `json:"status"`
	Type       string              `json:"type"`
	CreatedAt  time.Time           `json:"created_at"`
	Tenant     TenantResponse      `json:"tenant"`
	Address    AddressResponse     `json:"address"`
	Guarantors []GuarantorResponse `json:"guarantors"`
}

type ErrorResponse struct {
	Error   string `json:"error"`
	Message string `json:"message,omitempty"`
}
