package models

import (
	"time"

	"github.com/google/uuid"
	"gorm.io/gorm"
)

type AddressType string

const (
	AddressTypeTenant   AddressType = "tenant"
	AddressTypeProperty AddressType = "property"
)

type Address struct {
	ID           uuid.UUID   `json:"id" gorm:"type:uuid;primaryKey"`
	Street       string      `json:"street"`
	Number       string      `json:"number"`
	Neighborhood string      `json:"neighborhood"`
	City         string      `json:"city"`
	State        string      `json:"state"`
	ZipCode      string      `json:"zip_code"`
	Country      string      `json:"country"`
	Type         AddressType `json:"type"`
	CreatedAt    time.Time   `json:"created_at"`
	UpdatedAt    time.Time   `json:"updated_at"`
}

func (a *Address) BeforeCreate(tx *gorm.DB) error {
	if a.ID == uuid.Nil {
		a.ID = uuid.New()
	}
	return nil
}
