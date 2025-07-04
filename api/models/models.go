package models

import (
	"time"

	"github.com/google/uuid"
	"gorm.io/gorm"
)

type Address struct {
	ID           uuid.UUID `json:"id" gorm:"type:uuid;primaryKey"`
	Street       string    `json:"street"`
	Number       string    `json:"number"`
	Neighborhood string    `json:"neighborhood"`
	City         string    `json:"city"`
	State        string    `json:"state"`
	ZipCode      string    `json:"zip_code"`
	Country      string    `json:"country"`
	CreatedAt    time.Time `json:"created_at"`
	UpdatedAt    time.Time `json:"updated_at"`
}

func (a *Address) BeforeCreate(tx *gorm.DB) error {
	if a.ID == uuid.Nil {
		a.ID = uuid.New()
	}
	return nil
}

type Tenant struct {
	ID        uuid.UUID `json:"id" gorm:"type:uuid;primaryKey"`
	Name      string    `json:"name"`
	Email     string    `json:"email"`
	Phone     string    `json:"phone"`
	AddressID uuid.UUID `json:"address_id" gorm:"type:uuid"`
	CreatedAt time.Time `json:"created_at"`
	UpdatedAt time.Time `json:"updated_at"`

	Address   Address    `json:"address" gorm:"foreignKey:AddressID"`
	Contracts []Contract `json:"contracts" gorm:"foreignKey:TenantID"`
}

func (t *Tenant) BeforeCreate(tx *gorm.DB) error {
	if t.ID == uuid.Nil {
		t.ID = uuid.New()
	}
	return nil
}

type Guarantor struct {
	ID        uuid.UUID `json:"id" gorm:"type:uuid;primaryKey"`
	Name      string    `json:"name"`
	Email     string    `json:"email"`
	Phone     string    `json:"phone"`
	AddressID uuid.UUID `json:"address_id" gorm:"type:uuid"`
	CreatedAt time.Time `json:"created_at"`
	UpdatedAt time.Time `json:"updated_at"`

	Address Address `json:"address" gorm:"foreignKey:AddressID"`
}

func (g *Guarantor) BeforeCreate(tx *gorm.DB) error {
	if g.ID == uuid.Nil {
		g.ID = uuid.New()
	}
	return nil
}

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
