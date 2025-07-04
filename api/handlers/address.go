package handlers

import (
	"encoding/json"
	"net/http"
	"rent-control-api/dto"
	"rent-control-api/middleware"
	"rent-control-api/models"
	"rent-control-api/services"

	"github.com/go-chi/chi/v5"
	"github.com/google/uuid"
	"gorm.io/gorm"
)

func SetupAddressRoutes(r chi.Router, db *gorm.DB) {
	service := services.NewAddressService(db)

	r.Route("/addresses", func(r chi.Router) {
		r.Get("/", listAddresses(service))
		r.Post("/", createAddress(service))

		// Use a different pattern to avoid conflict
		r.Route("/filter", func(r chi.Router) {
			r.Get("/tenant", listTenantAddresses(service))
			r.Get("/property", listPropertyAddresses(service))
		})

		r.Put("/{id}", updateAddress(service))
		r.Delete("/{id}", deleteAddress(service))
		r.Get("/{id}", getAddress(service))
	})
}

func listAddresses(service *services.AddressService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		addresses, err := service.GetAll()
		if err != nil {
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to retrieve addresses", err.Error())
			return
		}

		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(addresses)
	}
}

func getAddress(service *services.AddressService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		idParam := chi.URLParam(r, "id")
		id, err := uuid.Parse(idParam)
		if err != nil {
			middleware.WriteJSONError(w, http.StatusBadRequest, "Invalid address ID", "ID must be a valid UUID")
			return
		}

		address, err := service.GetByID(id)
		if err != nil {
			if err.Error() == "address not found" {
				middleware.WriteJSONError(w, http.StatusNotFound, "Address not found", "No address found with the given ID")
				return
			}
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to retrieve address", err.Error())
			return
		}

		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(address)
	}
}

func createAddress(service *services.AddressService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		var req dto.CreateAddressRequest
		if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
			middleware.WriteJSONError(w, http.StatusBadRequest, "Invalid request body", err.Error())
			return
		}

		address, err := service.Create(req)
		if err != nil {
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to create address", err.Error())
			return
		}

		w.Header().Set("Content-Type", "application/json")
		w.WriteHeader(http.StatusCreated)
		json.NewEncoder(w).Encode(address)
	}
}

func updateAddress(service *services.AddressService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		var req dto.UpdateAddressRequest
		if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
			middleware.WriteJSONError(w, http.StatusBadRequest, "Invalid request body", err.Error())
			return
		}

		address, err := service.Update(req)
		if err != nil {
			if err.Error() == "address not found" {
				middleware.WriteJSONError(w, http.StatusNotFound, "Address not found", "No address found with the given ID")
				return
			}
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to update address", err.Error())
			return
		}

		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(address)
	}
}

func deleteAddress(service *services.AddressService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		idParam := chi.URLParam(r, "id")
		id, err := uuid.Parse(idParam)
		if err != nil {
			middleware.WriteJSONError(w, http.StatusBadRequest, "Invalid address ID", "ID must be a valid UUID")
			return
		}

		err = service.Delete(id)
		if err != nil {
			if err.Error() == "address not found" {
				middleware.WriteJSONError(w, http.StatusNotFound, "Address not found", "No address found with the given ID")
				return
			}
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to delete address", err.Error())
			return
		}

		w.WriteHeader(http.StatusNoContent)
	}
}

func listTenantAddresses(service *services.AddressService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		addresses, err := service.GetByType(models.AddressTypeTenant)
		if err != nil {
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to retrieve tenant addresses", err.Error())
			return
		}

		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(addresses)
	}
}

func listPropertyAddresses(service *services.AddressService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		addresses, err := service.GetByType(models.AddressTypeProperty)
		if err != nil {
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to retrieve property addresses", err.Error())
			return
		}

		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(addresses)
	}
}
