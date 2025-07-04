package config

import (
	"os"
)

type Config struct {
	DatabaseURL string
	Port        string
	Environment string
}

func New() *Config {
	return &Config{
		DatabaseURL: getEnv("DATABASE_URL", "postgres://rentalcontrol:password@localhost/rental_control?sslmode=disable"),
		Port:        getEnv("PORT", "8080"),
		Environment: getEnv("ENVIRONMENT", "development"),
	}
}

func getEnv(key, defaultValue string) string {
	if value := os.Getenv(key); value != "" {
		return value
	}
	return defaultValue
}
