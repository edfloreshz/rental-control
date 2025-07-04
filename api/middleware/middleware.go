package middleware

import (
	"encoding/json"
	"log"
	"net/http"
	"rent-control-api/dto"
	"time"
)

func ErrorHandler() func(next http.Handler) http.Handler {
	return func(next http.Handler) http.Handler {
		return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
			// Simply pass through to the next handler
			// Error handling is done in individual handlers
			next.ServeHTTP(w, r)
		})
	}
}

func Logger() func(next http.Handler) http.Handler {
	return func(next http.Handler) http.Handler {
		return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
			start := time.Now()

			// Process request
			next.ServeHTTP(w, r)

			// Log request details
			duration := time.Since(start)
			log.Printf("%s %s %v", r.Method, r.URL.Path, duration)
		})
	}
}

// Helper function to write JSON error responses
func WriteJSONError(w http.ResponseWriter, statusCode int, errorMsg, message string) {
	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(statusCode)

	errorResponse := dto.ErrorResponse{
		Error:   errorMsg,
		Message: message,
	}

	json.NewEncoder(w).Encode(errorResponse)
}
