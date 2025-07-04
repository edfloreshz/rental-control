package main

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"os"

	"rent-control-api/config"
	"rent-control-api/database"
	"rent-control-api/handlers"
	customMiddleware "rent-control-api/middleware"

	"github.com/MarceloPetrucio/go-scalar-api-reference"
	"github.com/go-chi/chi/v5"
	"github.com/go-chi/chi/v5/middleware"
	"github.com/go-chi/cors"
	"github.com/joho/godotenv"
)

func main() {
	// Load environment variables
	if err := godotenv.Load(); err != nil {
		log.Println("No .env file found")
	}

	// Initialize configuration
	cfg := config.New()

	// Initialize database
	db, err := database.Initialize(cfg.DatabaseURL)
	if err != nil {
		log.Fatal("Failed to initialize database:", err)
	}

	// Initialize Chi router
	r := chi.NewRouter()

	// CORS middleware - must be first to handle preflight requests
	r.Use(cors.Handler(cors.Options{
		AllowedOrigins:   []string{"*"}, // Allow all origins for now
		AllowedMethods:   []string{"GET", "POST", "PUT", "DELETE", "OPTIONS"},
		AllowedHeaders:   []string{"Origin", "Content-Type", "Authorization", "Accept", "X-Requested-With"},
		AllowCredentials: false, // Set to false when using wildcard origin
		MaxAge:           300,   // Cache preflight response for 5 minutes
	}))

	// Chi built-in middleware
	r.Use(middleware.RequestID)
	r.Use(middleware.RealIP)
	r.Use(middleware.Logger)
	r.Use(middleware.Recoverer)
	r.Use(middleware.StripSlashes) // Add this to handle trailing slashes

	// Debug middleware to log requests
	r.Use(func(next http.Handler) http.Handler {
		return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
			log.Printf("Request: %s %s", r.Method, r.URL.Path)
			next.ServeHTTP(w, r)
		})
	})

	// Custom middleware
	r.Use(customMiddleware.ErrorHandler())
	r.Use(customMiddleware.Logger())

	// API routes
	r.Route("/api/v1", func(r chi.Router) {
		handlers.SetupTenantRoutes(r, db)
		handlers.SetupAddressRoutes(r, db)
		handlers.SetupContractRoutes(r, db)
		handlers.SetupGuarantorRoutes(r, db)
	})

	// Health check endpoint
	r.Get("/health", func(w http.ResponseWriter, r *http.Request) {
		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(map[string]string{"status": "healthy"})
	})

	// Root endpoint
	r.Get("/", func(w http.ResponseWriter, r *http.Request) {
		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(map[string]string{"message": "Rent Control API", "version": "1.0"})
	})

	r.Get("/reference", func(w http.ResponseWriter, r *http.Request) {
		htmlContent, err := scalar.ApiReferenceHTML(&scalar.Options{
			SpecURL: "https://generator3.swagger.io/openapi.json", // allow external URL or local path file
			CustomOptions: scalar.CustomOptions{
				PageTitle: "Rent Control API Documentation",
			},
			DarkMode: true,
		})

		if err != nil {
			fmt.Printf("%v", err)
		}

		fmt.Fprintln(w, htmlContent)
	})

	// Start server
	port := os.Getenv("PORT")
	if port == "" {
		port = "8080"
	}

	log.Printf("Server starting on port %s", port)
	if err := http.ListenAndServe(":"+port, r); err != nil {
		log.Fatal("Failed to start server:", err)
	}
}
