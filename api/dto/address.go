package dto

import (
	"rent-control-api/models"
	"time"

	"github.com/google/uuid"
)

type CreateAddressRequest struct {
	Street       string             `json:"street" binding:"required"`
	Number       string             `json:"number" binding:"required"`
	Neighborhood string             `json:"neighborhood" binding:"required"`
	City         string             `json:"city" binding:"required"`
	State        string             `json:"state" binding:"required"`
	ZipCode      string             `json:"zip_code" binding:"required"`
	Country      string             `json:"country" binding:"required"`
	Type         models.AddressType `json:"type" binding:"required"`
}

type UpdateAddressRequest struct {
	ID           uuid.UUID          `json:"id" binding:"required"`
	Street       string             `json:"street"`
	Number       string             `json:"number"`
	Neighborhood string             `json:"neighborhood"`
	City         string             `json:"city"`
	State        string             `json:"state"`
	ZipCode      string             `json:"zip_code"`
	Country      string             `json:"country"`
	Type         models.AddressType `json:"type"`
}

type AddressResponse struct {
	ID           uuid.UUID          `json:"id"`
	Street       string             `json:"street"`
	Number       string             `json:"number"`
	Neighborhood string             `json:"neighborhood"`
	City         string             `json:"city"`
	State        string             `json:"state"`
	ZipCode      string             `json:"zip_code"`
	Country      string             `json:"country"`
	Type         models.AddressType `json:"type"`
	CreatedAt    time.Time          `json:"created_at"`
}
