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

func SetupGuarantorRoutes(r chi.Router, db *gorm.DB) {
	service := services.NewGuarantorService(db)

	r.Route("/guarantors", func(r chi.Router) {
		r.Get("/", listGuarantors(service))
		r.Get("/{id}", getGuarantor(service))
		r.Post("/", createGuarantor(service))
		r.Put("/{id}", updateGuarantor(service))
		r.Delete("/{id}", deleteGuarantor(service))
	})
}

func listGuarantors(service *services.GuarantorService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		guarantors, err := service.GetAll()
		if err != nil {
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to retrieve guarantors", err.Error())
			return
		}

		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(guarantors)
	}
}

func getGuarantor(service *services.GuarantorService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		idParam := chi.URLParam(r, "id")
		id, err := uuid.Parse(idParam)
		if err != nil {
			middleware.WriteJSONError(w, http.StatusBadRequest, "Invalid guarantor ID", "ID must be a valid UUID")
			return
		}

		guarantor, err := service.GetByID(id)
		if err != nil {
			if err.Error() == "guarantor not found" {
				middleware.WriteJSONError(w, http.StatusNotFound, "Guarantor not found", "No guarantor found with the given ID")
				return
			}
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to retrieve guarantor", err.Error())
			return
		}

		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(guarantor)
	}
}

func createGuarantor(service *services.GuarantorService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		var req dto.CreateGuarantorRequest
		if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
			middleware.WriteJSONError(w, http.StatusBadRequest, "Invalid request body", err.Error())
			return
		}

		guarantor, err := service.Create(req)
		if err != nil {
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to create guarantor", err.Error())
			return
		}

		w.Header().Set("Content-Type", "application/json")
		w.WriteHeader(http.StatusCreated)
		json.NewEncoder(w).Encode(guarantor)
	}
}

func updateGuarantor(service *services.GuarantorService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		var req dto.UpdateGuarantorRequest
		if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
			middleware.WriteJSONError(w, http.StatusBadRequest, "Invalid request body", err.Error())
			return
		}

		guarantor, err := service.Update(req)
		if err != nil {
			if err.Error() == "guarantor not found" {
				middleware.WriteJSONError(w, http.StatusNotFound, "Guarantor not found", "No guarantor found with the given ID")
				return
			}
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to update guarantor", err.Error())
			return
		}

		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(guarantor)
	}
}

func deleteGuarantor(service *services.GuarantorService) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		idParam := chi.URLParam(r, "id")
		id, err := uuid.Parse(idParam)
		if err != nil {
			middleware.WriteJSONError(w, http.StatusBadRequest, "Invalid guarantor ID", "ID must be a valid UUID")
			return
		}

		err = service.Delete(id)
		if err != nil {
			if err.Error() == "guarantor not found" {
				middleware.WriteJSONError(w, http.StatusNotFound, "Guarantor not found", "No guarantor found with the given ID")
				return
			}
			middleware.WriteJSONError(w, http.StatusInternalServerError, "Failed to delete guarantor", err.Error())
			return
		}

		w.WriteHeader(http.StatusNoContent)
	}
}
