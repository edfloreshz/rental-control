package dto

import (
	"github.com/google/uuid"
	"time"
)

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
