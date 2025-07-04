package models

import (
	"github.com/google/uuid"
	"gorm.io/gorm"
	"time"
)

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
