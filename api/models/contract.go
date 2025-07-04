package models

import (
	"github.com/google/uuid"
	"gorm.io/gorm"
	"time"
)

type ContractStatus string

const (
	ContractStatusActive     ContractStatus = "active"
	ContractStatusExpired    ContractStatus = "expired"
	ContractStatusTerminated ContractStatus = "terminated"
)

type ContractType string

const (
	ContractTypeYearly ContractType = "yearly"
)

type Contract struct {
	ID        uuid.UUID      `json:"id" gorm:"type:uuid;primaryKey"`
	TenantID  uuid.UUID      `json:"tenant_id" gorm:"type:uuid"`
	AddressID uuid.UUID      `json:"address_id" gorm:"type:uuid"`
	Deposit   float64        `json:"deposit"`
	Rent      float64        `json:"rent"`
	Business  string         `json:"business"`
	StartDate time.Time      `json:"start_date"`
	EndDate   time.Time      `json:"end_date"`
	Status    ContractStatus `json:"status"`
	Type      ContractType   `json:"type"`
	CreatedAt time.Time      `json:"created_at"`
	UpdatedAt time.Time      `json:"updated_at"`

	Tenant     Tenant      `json:"tenant" gorm:"foreignKey:TenantID"`
	Address    Address     `json:"address" gorm:"foreignKey:AddressID"`
	Guarantors []Guarantor `json:"guarantors" gorm:"many2many:contract_guarantors;"`
}

func (c *Contract) BeforeCreate(tx *gorm.DB) error {
	if c.ID == uuid.Nil {
		c.ID = uuid.New()
	}
	return nil
}

type ContractGuarantor struct {
	ContractID  uuid.UUID `json:"contract_id" gorm:"type:uuid;primaryKey"`
	GuarantorID uuid.UUID `json:"guarantor_id" gorm:"type:uuid;primaryKey"`
}
