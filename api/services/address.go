package services

import (
	"errors"
	"rent-control-api/dto"
	"rent-control-api/models"

	"github.com/google/uuid"
	"gorm.io/gorm"
)

type AddressService struct {
	db *gorm.DB
}

func NewAddressService(db *gorm.DB) *AddressService {
	return &AddressService{db: db}
}

func (s *AddressService) GetAll() ([]dto.AddressResponse, error) {
	var addresses []models.Address
	if err := s.db.Find(&addresses).Error; err != nil {
		return nil, err
	}

	var response []dto.AddressResponse
	for _, addr := range addresses {
		response = append(response, dto.AddressResponse{
			ID:           addr.ID,
			Street:       addr.Street,
			Number:       addr.Number,
			Neighborhood: addr.Neighborhood,
			City:         addr.City,
			State:        addr.State,
			ZipCode:      addr.ZipCode,
			Country:      addr.Country,
			Type:         addr.Type,
			CreatedAt:    addr.CreatedAt,
		})
	}

	return response, nil
}

func (s *AddressService) GetByType(addressType models.AddressType) ([]dto.AddressResponse, error) {
	var addresses []models.Address
	if err := s.db.Where("type = ?", addressType).Find(&addresses).Error; err != nil {
		return nil, err
	}

	var response []dto.AddressResponse
	for _, addr := range addresses {
		response = append(response, dto.AddressResponse{
			ID:           addr.ID,
			Street:       addr.Street,
			Number:       addr.Number,
			Neighborhood: addr.Neighborhood,
			City:         addr.City,
			State:        addr.State,
			ZipCode:      addr.ZipCode,
			Country:      addr.Country,
			Type:         addr.Type,
			CreatedAt:    addr.CreatedAt,
		})
	}

	return response, nil
}

func (s *AddressService) GetByID(id uuid.UUID) (*dto.AddressResponse, error) {
	var address models.Address
	if err := s.db.First(&address, "id = ?", id).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, errors.New("address not found")
		}
		return nil, err
	}

	return &dto.AddressResponse{
		ID:           address.ID,
		Street:       address.Street,
		Number:       address.Number,
		Neighborhood: address.Neighborhood,
		City:         address.City,
		State:        address.State,
		ZipCode:      address.ZipCode,
		Country:      address.Country,
		Type:         address.Type,
		CreatedAt:    address.CreatedAt,
	}, nil
}

func (s *AddressService) Create(req dto.CreateAddressRequest) (*dto.AddressResponse, error) {
	address := models.Address{
		Street:       req.Street,
		Number:       req.Number,
		Neighborhood: req.Neighborhood,
		City:         req.City,
		State:        req.State,
		ZipCode:      req.ZipCode,
		Country:      req.Country,
		Type:         req.Type,
	}

	if err := s.db.Create(&address).Error; err != nil {
		return nil, err
	}

	return &dto.AddressResponse{
		ID:           address.ID,
		Street:       address.Street,
		Number:       address.Number,
		Neighborhood: address.Neighborhood,
		City:         address.City,
		State:        address.State,
		ZipCode:      address.ZipCode,
		Country:      address.Country,
		Type:         address.Type,
		CreatedAt:    address.CreatedAt,
	}, nil
}

func (s *AddressService) Update(req dto.UpdateAddressRequest) (*dto.AddressResponse, error) {
	var address models.Address
	if err := s.db.First(&address, "id = ?", req.ID).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, errors.New("address not found")
		}
		return nil, err
	}

	// Update fields if provided
	if req.Street != "" {
		address.Street = req.Street
	}
	if req.Number != "" {
		address.Number = req.Number
	}
	if req.Neighborhood != "" {
		address.Neighborhood = req.Neighborhood
	}
	if req.City != "" {
		address.City = req.City
	}
	if req.State != "" {
		address.State = req.State
	}
	if req.ZipCode != "" {
		address.ZipCode = req.ZipCode
	}
	if req.Country != "" {
		address.Country = req.Country
	}
	if req.Type != "" {
		address.Type = req.Type
	}

	if err := s.db.Save(&address).Error; err != nil {
		return nil, err
	}

	return &dto.AddressResponse{
		ID:           address.ID,
		Street:       address.Street,
		Number:       address.Number,
		Neighborhood: address.Neighborhood,
		City:         address.City,
		State:        address.State,
		ZipCode:      address.ZipCode,
		Country:      address.Country,
		Type:         address.Type,
		CreatedAt:    address.CreatedAt,
	}, nil
}

func (s *AddressService) Delete(id uuid.UUID) error {
	result := s.db.Delete(&models.Address{}, "id = ?", id)
	if result.Error != nil {
		return result.Error
	}
	if result.RowsAffected == 0 {
		return errors.New("address not found")
	}
	return nil
}
