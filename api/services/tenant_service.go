package services

import (
	"errors"
	"rent-control-api/dto"
	"rent-control-api/models"

	"github.com/google/uuid"
	"gorm.io/gorm"
)

type TenantService struct {
	db *gorm.DB
}

func NewTenantService(db *gorm.DB) *TenantService {
	return &TenantService{db: db}
}

func (s *TenantService) GetAll() ([]dto.TenantResponse, error) {
	var tenants []models.Tenant
	if err := s.db.Preload("Address").Find(&tenants).Error; err != nil {
		return nil, err
	}

	var response []dto.TenantResponse
	for _, tenant := range tenants {
		response = append(response, dto.TenantResponse{
			ID:        tenant.ID,
			Name:      tenant.Name,
			Email:     tenant.Email,
			Phone:     tenant.Phone,
			AddressID: tenant.AddressID,
			CreatedAt: tenant.CreatedAt,
			Address: dto.AddressResponse{
				ID:           tenant.Address.ID,
				Street:       tenant.Address.Street,
				Number:       tenant.Address.Number,
				Neighborhood: tenant.Address.Neighborhood,
				City:         tenant.Address.City,
				State:        tenant.Address.State,
				ZipCode:      tenant.Address.ZipCode,
				Country:      tenant.Address.Country,
				CreatedAt:    tenant.Address.CreatedAt,
			},
		})
	}

	return response, nil
}

func (s *TenantService) GetByID(id uuid.UUID) (*dto.TenantResponse, error) {
	var tenant models.Tenant
	if err := s.db.Preload("Address").First(&tenant, "id = ?", id).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, errors.New("tenant not found")
		}
		return nil, err
	}

	return &dto.TenantResponse{
		ID:        tenant.ID,
		Name:      tenant.Name,
		Email:     tenant.Email,
		Phone:     tenant.Phone,
		AddressID: tenant.AddressID,
		CreatedAt: tenant.CreatedAt,
		Address: dto.AddressResponse{
			ID:           tenant.Address.ID,
			Street:       tenant.Address.Street,
			Number:       tenant.Address.Number,
			Neighborhood: tenant.Address.Neighborhood,
			City:         tenant.Address.City,
			State:        tenant.Address.State,
			ZipCode:      tenant.Address.ZipCode,
			Country:      tenant.Address.Country,
			CreatedAt:    tenant.Address.CreatedAt,
		},
	}, nil
}

func (s *TenantService) Create(req dto.CreateTenantRequest) (*dto.TenantResponse, error) {
	// Check if address exists
	var address models.Address
	if err := s.db.First(&address, "id = ?", req.AddressID).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, errors.New("address not found")
		}
		return nil, err
	}

	tenant := models.Tenant{
		Name:      req.Name,
		Email:     req.Email,
		Phone:     req.Phone,
		AddressID: req.AddressID,
	}

	if err := s.db.Create(&tenant).Error; err != nil {
		return nil, err
	}

	return &dto.TenantResponse{
		ID:        tenant.ID,
		Name:      tenant.Name,
		Email:     tenant.Email,
		Phone:     tenant.Phone,
		AddressID: tenant.AddressID,
		CreatedAt: tenant.CreatedAt,
		Address: dto.AddressResponse{
			ID:           address.ID,
			Street:       address.Street,
			Number:       address.Number,
			Neighborhood: address.Neighborhood,
			City:         address.City,
			State:        address.State,
			ZipCode:      address.ZipCode,
			Country:      address.Country,
			CreatedAt:    address.CreatedAt,
		},
	}, nil
}

func (s *TenantService) Update(req dto.UpdateTenantRequest) (*dto.TenantResponse, error) {
	var tenant models.Tenant
	if err := s.db.First(&tenant, "id = ?", req.ID).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, errors.New("tenant not found")
		}
		return nil, err
	}

	// Update fields if provided
	if req.Name != "" {
		tenant.Name = req.Name
	}
	if req.Email != "" {
		tenant.Email = req.Email
	}
	if req.Phone != "" {
		tenant.Phone = req.Phone
	}
	if req.AddressID != uuid.Nil {
		// Check if new address exists
		var address models.Address
		if err := s.db.First(&address, "id = ?", req.AddressID).Error; err != nil {
			if errors.Is(err, gorm.ErrRecordNotFound) {
				return nil, errors.New("address not found")
			}
			return nil, err
		}
		tenant.AddressID = req.AddressID
	}

	if err := s.db.Save(&tenant).Error; err != nil {
		return nil, err
	}

	// Get updated tenant with address
	if err := s.db.Preload("Address").First(&tenant, "id = ?", tenant.ID).Error; err != nil {
		return nil, err
	}

	return &dto.TenantResponse{
		ID:        tenant.ID,
		Name:      tenant.Name,
		Email:     tenant.Email,
		Phone:     tenant.Phone,
		AddressID: tenant.AddressID,
		CreatedAt: tenant.CreatedAt,
		Address: dto.AddressResponse{
			ID:           tenant.Address.ID,
			Street:       tenant.Address.Street,
			Number:       tenant.Address.Number,
			Neighborhood: tenant.Address.Neighborhood,
			City:         tenant.Address.City,
			State:        tenant.Address.State,
			ZipCode:      tenant.Address.ZipCode,
			Country:      tenant.Address.Country,
			CreatedAt:    tenant.Address.CreatedAt,
		},
	}, nil
}

func (s *TenantService) Delete(id uuid.UUID) error {
	result := s.db.Delete(&models.Tenant{}, "id = ?", id)
	if result.Error != nil {
		return result.Error
	}
	if result.RowsAffected == 0 {
		return errors.New("tenant not found")
	}
	return nil
}
