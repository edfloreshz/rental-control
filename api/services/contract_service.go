package services

import (
	"errors"
	"rent-control-api/dto"
	"rent-control-api/models"

	"github.com/google/uuid"
	"gorm.io/gorm"
)

type ContractService struct {
	db *gorm.DB
}

func NewContractService(db *gorm.DB) *ContractService {
	return &ContractService{db: db}
}

func (s *ContractService) GetAll() ([]dto.ContractResponse, error) {
	var contracts []models.Contract
	if err := s.db.Preload("Tenant").Preload("Tenant.Address").Preload("Address").Preload("Guarantors").Preload("Guarantors.Address").Find(&contracts).Error; err != nil {
		return nil, err
	}

	var response []dto.ContractResponse
	for _, contract := range contracts {
		guarantors := make([]dto.GuarantorResponse, len(contract.Guarantors))
		for i, guarantor := range contract.Guarantors {
			guarantors[i] = dto.GuarantorResponse{
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
			}
		}

		response = append(response, dto.ContractResponse{
			ID:        contract.ID,
			TenantID:  contract.TenantID,
			AddressID: contract.AddressID,
			Deposit:   contract.Deposit,
			Rent:      contract.Rent,
			Business:  contract.Business,
			StartDate: contract.StartDate,
			EndDate:   contract.EndDate,
			Status:    string(contract.Status),
			Type:      string(contract.Type),
			CreatedAt: contract.CreatedAt,
			Tenant: dto.TenantResponse{
				ID:        contract.Tenant.ID,
				Name:      contract.Tenant.Name,
				Email:     contract.Tenant.Email,
				Phone:     contract.Tenant.Phone,
				AddressID: contract.Tenant.AddressID,
				CreatedAt: contract.Tenant.CreatedAt,
				Address: dto.AddressResponse{
					ID:           contract.Tenant.Address.ID,
					Street:       contract.Tenant.Address.Street,
					Number:       contract.Tenant.Address.Number,
					Neighborhood: contract.Tenant.Address.Neighborhood,
					City:         contract.Tenant.Address.City,
					State:        contract.Tenant.Address.State,
					ZipCode:      contract.Tenant.Address.ZipCode,
					Country:      contract.Tenant.Address.Country,
					CreatedAt:    contract.Tenant.Address.CreatedAt,
				},
			},
			Address: dto.AddressResponse{
				ID:           contract.Address.ID,
				Street:       contract.Address.Street,
				Number:       contract.Address.Number,
				Neighborhood: contract.Address.Neighborhood,
				City:         contract.Address.City,
				State:        contract.Address.State,
				ZipCode:      contract.Address.ZipCode,
				Country:      contract.Address.Country,
				CreatedAt:    contract.Address.CreatedAt,
			},
			Guarantors: guarantors,
		})
	}

	return response, nil
}

func (s *ContractService) GetByID(id uuid.UUID) (*dto.ContractResponse, error) {
	var contract models.Contract
	if err := s.db.Preload("Tenant").Preload("Tenant.Address").Preload("Address").Preload("Guarantors").Preload("Guarantors.Address").First(&contract, "id = ?", id).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, errors.New("contract not found")
		}
		return nil, err
	}

	guarantors := make([]dto.GuarantorResponse, len(contract.Guarantors))
	for i, guarantor := range contract.Guarantors {
		guarantors[i] = dto.GuarantorResponse{
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
		}
	}

	return &dto.ContractResponse{
		ID:        contract.ID,
		TenantID:  contract.TenantID,
		AddressID: contract.AddressID,
		Deposit:   contract.Deposit,
		Rent:      contract.Rent,
		Business:  contract.Business,
		StartDate: contract.StartDate,
		EndDate:   contract.EndDate,
		Status:    string(contract.Status),
		Type:      string(contract.Type),
		CreatedAt: contract.CreatedAt,
		Tenant: dto.TenantResponse{
			ID:        contract.Tenant.ID,
			Name:      contract.Tenant.Name,
			Email:     contract.Tenant.Email,
			Phone:     contract.Tenant.Phone,
			AddressID: contract.Tenant.AddressID,
			CreatedAt: contract.Tenant.CreatedAt,
			Address: dto.AddressResponse{
				ID:           contract.Tenant.Address.ID,
				Street:       contract.Tenant.Address.Street,
				Number:       contract.Tenant.Address.Number,
				Neighborhood: contract.Tenant.Address.Neighborhood,
				City:         contract.Tenant.Address.City,
				State:        contract.Tenant.Address.State,
				ZipCode:      contract.Tenant.Address.ZipCode,
				Country:      contract.Tenant.Address.Country,
				CreatedAt:    contract.Tenant.Address.CreatedAt,
			},
		},
		Address: dto.AddressResponse{
			ID:           contract.Address.ID,
			Street:       contract.Address.Street,
			Number:       contract.Address.Number,
			Neighborhood: contract.Address.Neighborhood,
			City:         contract.Address.City,
			State:        contract.Address.State,
			ZipCode:      contract.Address.ZipCode,
			Country:      contract.Address.Country,
			CreatedAt:    contract.Address.CreatedAt,
		},
		Guarantors: guarantors,
	}, nil
}

func (s *ContractService) Create(req dto.CreateContractRequest) (*dto.ContractResponse, error) {
	// Check if tenant exists
	var tenant models.Tenant
	if err := s.db.First(&tenant, "id = ?", req.TenantID).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, errors.New("tenant not found")
		}
		return nil, err
	}

	// Check if address exists
	var address models.Address
	if err := s.db.First(&address, "id = ?", req.AddressID).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, errors.New("address not found")
		}
		return nil, err
	}

	contract := models.Contract{
		TenantID:  req.TenantID,
		AddressID: req.AddressID,
		Deposit:   req.Deposit,
		Rent:      req.Rent,
		Business:  req.Business,
		StartDate: req.StartDate,
		EndDate:   req.EndDate,
		Status:    models.ContractStatus(req.Status),
		Type:      models.ContractType(req.Type),
	}

	if err := s.db.Create(&contract).Error; err != nil {
		return nil, err
	}

	// Add guarantors if provided
	if len(req.GuarantorIDs) > 0 {
		var guarantors []models.Guarantor
		if err := s.db.Where("id IN ?", req.GuarantorIDs).Find(&guarantors).Error; err != nil {
			return nil, err
		}

		if len(guarantors) != len(req.GuarantorIDs) {
			return nil, errors.New("some guarantors not found")
		}

		if err := s.db.Model(&contract).Association("Guarantors").Append(guarantors); err != nil {
			return nil, err
		}
	}

	// Get the created contract with all relations
	return s.GetByID(contract.ID)
}

func (s *ContractService) Update(req dto.UpdateContractRequest) (*dto.ContractResponse, error) {
	var contract models.Contract
	if err := s.db.First(&contract, "id = ?", req.ID).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, errors.New("contract not found")
		}
		return nil, err
	}

	// Update fields if provided
	if req.TenantID != uuid.Nil {
		var tenant models.Tenant
		if err := s.db.First(&tenant, "id = ?", req.TenantID).Error; err != nil {
			if errors.Is(err, gorm.ErrRecordNotFound) {
				return nil, errors.New("tenant not found")
			}
			return nil, err
		}
		contract.TenantID = req.TenantID
	}

	if req.AddressID != uuid.Nil {
		var address models.Address
		if err := s.db.First(&address, "id = ?", req.AddressID).Error; err != nil {
			if errors.Is(err, gorm.ErrRecordNotFound) {
				return nil, errors.New("address not found")
			}
			return nil, err
		}
		contract.AddressID = req.AddressID
	}

	if req.Deposit != 0 {
		contract.Deposit = req.Deposit
	}
	if req.Rent != 0 {
		contract.Rent = req.Rent
	}
	if req.Business != "" {
		contract.Business = req.Business
	}
	if !req.StartDate.IsZero() {
		contract.StartDate = req.StartDate
	}
	if !req.EndDate.IsZero() {
		contract.EndDate = req.EndDate
	}
	if req.Status != "" {
		contract.Status = models.ContractStatus(req.Status)
	}
	if req.Type != "" {
		contract.Type = models.ContractType(req.Type)
	}

	if err := s.db.Save(&contract).Error; err != nil {
		return nil, err
	}

	// Update guarantors if provided
	if len(req.GuarantorIDs) > 0 {
		var guarantors []models.Guarantor
		if err := s.db.Where("id IN ?", req.GuarantorIDs).Find(&guarantors).Error; err != nil {
			return nil, err
		}

		if len(guarantors) != len(req.GuarantorIDs) {
			return nil, errors.New("some guarantors not found")
		}

		if err := s.db.Model(&contract).Association("Guarantors").Replace(guarantors); err != nil {
			return nil, err
		}
	}

	// Get the updated contract with all relations
	return s.GetByID(contract.ID)
}

func (s *ContractService) Delete(id uuid.UUID) error {
	result := s.db.Delete(&models.Contract{}, "id = ?", id)
	if result.Error != nil {
		return result.Error
	}
	if result.RowsAffected == 0 {
		return errors.New("contract not found")
	}
	return nil
}
