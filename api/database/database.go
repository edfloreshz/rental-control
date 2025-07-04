package database

import (
	"rent-control-api/models"

	"gorm.io/driver/postgres"
	"gorm.io/gorm"
)

func Initialize(databaseURL string) (*gorm.DB, error) {
	db, err := gorm.Open(postgres.Open(databaseURL), &gorm.Config{})
	if err != nil {
		return nil, err
	}

	// Auto-migrate the schema
	if err := db.AutoMigrate(
		&models.Address{},
		&models.Tenant{},
		&models.Guarantor{},
		&models.Contract{},
		&models.ContractGuarantor{},
	); err != nil {
		return nil, err
	}

	return db, nil
}
