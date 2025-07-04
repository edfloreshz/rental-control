package services

import (
	"errors"
	"rent-control-api/dto"
	"rent-control-api/models"

	"github.com/google/uuid"
	"gorm.io/gorm"
)

type GuarantorService struct {
	db *gorm.DB
}

func NewGuarantorService(db *gorm.DB) *GuarantorService {
	return &GuarantorService{db: db}
}

func (s *GuarantorService) GetAll() ([]dto.GuarantorResponse, error) {
	var guarantors []models.Guarantor
	if err := s.db.Preload("Address").Find(&guarantors).Error; err != nil {
		return nil, err
	}

	var response []dto.GuarantorResponse
	for _, guarantor := range guarantors {
		response = append(response, dto.GuarantorResponse{
			ID:        guarantor.ID,
			Name:      guarantor.Name,
			Email:     guarantor.Email,
			Phone:     guarantor.Phone,
			AddressID: guarantor.AddressID,
			CreatedAt: guarantor.CreatedAt,
			Address: dto.AddressResponse{
				ID:           guarantor.Address.ID,
				Street:       guarantor.Address.Street,
				Number:       guarantor.Address.Number,
				Neighborhood: guarantor.Address.Neighborhood,
				City:         guarantor.Address.City,
				State:        guarantor.Address.State,
				ZipCode:      guarantor.Address.ZipCode,
				Country:      guarantor.Address.Country,
				CreatedAt:    guarantor.Address.CreatedAt,
			},
		})
	}

	return response, nil
}

func (s *GuarantorService) GetByID(id uuid.UUID) (*dto.GuarantorResponse, error) {
	var guarantor models.Guarantor
	if err := s.db.Preload("Address").First(&guarantor, "id = ?", id).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, errors.New("guarantor not found")
		}
		return nil, err
	}

	return &dto.GuarantorResponse{
		ID:        guarantor.ID,
		Name:      guarantor.Name,
		Email:     guarantor.Email,
		Phone:     guarantor.Phone,
		AddressID: guarantor.AddressID,
		CreatedAt: guarantor.CreatedAt,
		Address: dto.AddressResponse{
			ID:           guarantor.Address.ID,
			Street:       guarantor.Address.Street,
			Number:       guarantor.Address.Number,
			Neighborhood: guarantor.Address.Neighborhood,
			City:         guarantor.Address.City,
			State:        guarantor.Address.State,
			ZipCode:      guarantor.Address.ZipCode,
			Country:      guarantor.Address.Country,
			CreatedAt:    guarantor.Address.CreatedAt,
		},
	}, nil
}

func (s *GuarantorService) Create(req dto.CreateGuarantorRequest) (*dto.GuarantorResponse, error) {
	// Check if address exists
	var address models.Address
	if err := s.db.First(&address, "id = ?", req.AddressID).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, errors.New("address not found")
		}
		return nil, err
	}

	guarantor := models.Guarantor{
		Name:      req.Name,
		Email:     req.Email,
		Phone:     req.Phone,
		AddressID: req.AddressID,
	}

	if err := s.db.Create(&guarantor).Error; err != nil {
		return nil, err
	}

	return &dto.GuarantorResponse{
		ID:        guarantor.ID,
		Name:      guarantor.Name,
		Email:     guarantor.Email,
		Phone:     guarantor.Phone,
		AddressID: guarantor.AddressID,
		CreatedAt: guarantor.CreatedAt,
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

func (s *GuarantorService) Update(req dto.UpdateGuarantorRequest) (*dto.GuarantorResponse, error) {
	var guarantor models.Guarantor
	if err := s.db.First(&guarantor, "id = ?", req.ID).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, errors.New("guarantor not found")
		}
		return nil, err
	}

	// Update fields if provided
	if req.Name != "" {
		guarantor.Name = req.Name
	}
	if req.Email != "" {
		guarantor.Email = req.Email
	}
	if req.Phone != "" {
		guarantor.Phone = req.Phone
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
		guarantor.AddressID = req.AddressID
	}

	if err := s.db.Save(&guarantor).Error; err != nil {
		return nil, err
	}

	// Get updated guarantor with address
	if err := s.db.Preload("Address").First(&guarantor, "id = ?", guarantor.ID).Error; err != nil {
		return nil, err
	}

	return &dto.GuarantorResponse{
		ID:        guarantor.ID,
		Name:      guarantor.Name,
		Email:     guarantor.Email,
		Phone:     guarantor.Phone,
		AddressID: guarantor.AddressID,
		CreatedAt: guarantor.CreatedAt,
		Address: dto.AddressResponse{
			ID:           guarantor.Address.ID,
			Street:       guarantor.Address.Street,
			Number:       guarantor.Address.Number,
			Neighborhood: guarantor.Address.Neighborhood,
			City:         guarantor.Address.City,
			State:        guarantor.Address.State,
			ZipCode:      guarantor.Address.ZipCode,
			Country:      guarantor.Address.Country,
			CreatedAt:    guarantor.Address.CreatedAt,
		},
	}, nil
}

func (s *GuarantorService) Delete(id uuid.UUID) error {
	result := s.db.Delete(&models.Guarantor{}, "id = ?", id)
	if result.Error != nil {
		return result.Error
	}
	if result.RowsAffected == 0 {
		return errors.New("guarantor not found")
	}
	return nil
}
