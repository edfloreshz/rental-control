package handlers

import (
	"encoding/json"
	"net/http"
	"rent-control-api/dto"
	"rent-control-api/middleware"
	"rent-control-api/services"

	"github.com/go-chi/chi/v5"
	"github.com/google/uuid"
	"gorm.io/gorm"
)

func SetupContractRoutes(r chi.Router, db *gorm.DB) {
	service := services.NewContractService(db)

	r.Route("/contracts", func(r chi.Router) {
		r.Get("/", listContracts(service))
		r.Get("/{id}", getContract(service))
		r.Post("/", createContract(service))
		r.Put("/{id}", updateContract(service))
		r.Delete("/{id}", deleteContract(service))
	})
}

func listContracts(service *services.ContractService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		contracts, err := service.GetAll()
		if err != nil {
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to retrieve contracts", err.Error())
			return
		}

		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(contracts)
	}
}

func getContract(service *services.ContractService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		idParam := chi.URLParam(r, "id")
		id, err := uuid.Parse(idParam)
		if err != nil {
			middleware.WriteJSONError(w, http.StatusBadRequest, "Invalid contract ID", "ID must be a valid UUID")
			return
		}

		contract, err := service.GetByID(id)
		if err != nil {
			if err.Error() == "contract not found" {
				middleware.WriteJSONError(w, http.StatusNotFound, "Contract not found", "No contract found with the given ID")
				return
			}
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to retrieve contract", err.Error())
			return
		}

		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(contract)
	}
}

func createContract(service *services.ContractService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		var req dto.CreateContractRequest
		if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
			middleware.WriteJSONError(w, http.StatusBadRequest, "Invalid request body", err.Error())
			return
		}

		contract, err := service.Create(req)
		if err != nil {
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to create contract", err.Error())
			return
		}

		w.Header().Set("Content-Type", "application/json")
		w.WriteHeader(http.StatusCreated)
		json.NewEncoder(w).Encode(contract)
	}
}

func updateContract(service *services.ContractService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		var req dto.UpdateContractRequest
		if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
			middleware.WriteJSONError(w, http.StatusBadRequest, "Invalid request body", err.Error())
			return
		}

		contract, err := service.Update(req)
		if err != nil {
			if err.Error() == "contract not found" {
				middleware.WriteJSONError(w, http.StatusNotFound, "Contract not found", "No contract found with the given ID")
				return
			}
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to update contract", err.Error())
			return
		}

		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(contract)
	}
}

func deleteContract(service *services.ContractService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		idParam := chi.URLParam(r, "id")
		id, err := uuid.Parse(idParam)
		if err != nil {
			middleware.WriteJSONError(w, http.StatusBadRequest, "Invalid contract ID", "ID must be a valid UUID")
			return
		}

		err = service.Delete(id)
		if err != nil {
			if err.Error() == "contract not found" {
				middleware.WriteJSONError(w, http.StatusNotFound, "Contract not found", "No contract found with the given ID")
				return
			}
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to delete contract", err.Error())
			return
		}

		w.WriteHeader(http.StatusNoContent)
	}
}
