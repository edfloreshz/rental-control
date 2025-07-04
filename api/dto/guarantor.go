package dto

import (
	"github.com/google/uuid"
	"time"
)

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
