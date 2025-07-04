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

func SetupTenantRoutes(r chi.Router, db *gorm.DB) {
	service := services.NewTenantService(db)

	r.Route("/tenants", func(r chi.Router) {
		r.Get("/", listTenants(service))
		r.Get("/{id}", getTenant(service))
		r.Post("/", createTenant(service))
		r.Put("/{id}", updateTenant(service))
		r.Delete("/{id}", deleteTenant(service))
	})
}

func listTenants(service *services.TenantService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		tenants, err := service.GetAll()
		if err != nil {
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to retrieve tenants", err.Error())
			return
		}

		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(tenants)
	}
}

func getTenant(service *services.TenantService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		idParam := chi.URLParam(r, "id")
		id, err := uuid.Parse(idParam)
		if err != nil {
			middleware.WriteJSONError(w, http.StatusBadRequest, "Invalid tenant ID", "ID must be a valid UUID")
			return
		}

		tenant, err := service.GetByID(id)
		if err != nil {
			if err.Error() == "tenant not found" {
				middleware.WriteJSONError(w, http.StatusNotFound, "Tenant not found", "No tenant found with the given ID")
				return
			}
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to retrieve tenant", err.Error())
			return
		}

		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(tenant)
	}
}

func createTenant(service *services.TenantService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		var req dto.CreateTenantRequest
		if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
			middleware.WriteJSONError(w, http.StatusBadRequest, "Invalid request body", err.Error())
			return
		}

		tenant, err := service.Create(req)
		if err != nil {
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to create tenant", err.Error())
			return
		}

		w.Header().Set("Content-Type", "application/json")
		w.WriteHeader(http.StatusCreated)
		json.NewEncoder(w).Encode(tenant)
	}
}

func updateTenant(service *services.TenantService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		var req dto.UpdateTenantRequest
		if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
			middleware.WriteJSONError(w, http.StatusBadRequest, "Invalid request body", err.Error())
			return
		}

		tenant, err := service.Update(req)
		if err != nil {
			if err.Error() == "tenant not found" {
				middleware.WriteJSONError(w, http.StatusNotFound, "Tenant not found", "No tenant found with the given ID")
				return
			}
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to update tenant", err.Error())
			return
		}

		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(tenant)
	}
}

func deleteTenant(service *services.TenantService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		idParam := chi.URLParam(r, "id")
		id, err := uuid.Parse(idParam)
		if err != nil {
			middleware.WriteJSONError(w, http.StatusBadRequest, "Invalid tenant ID", "ID must be a valid UUID")
			return
		}

		err = service.Delete(id)
		if err != nil {
			if err.Error() == "tenant not found" {
				middleware.WriteJSONError(w, http.StatusNotFound, "Tenant not found", "No tenant found with the given ID")
				return
			}
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to delete tenant", err.Error())
			return
		}

		w.WriteHeader(http.StatusNoContent)
	}
}
