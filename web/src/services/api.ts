import type {
    Address,
    Contract,
    CreateAddress,
    CreateContract,
    CreateGuarantor,
    CreateTenant,
    Guarantor,
    Tenant,
    UpdateAddress,
    UpdateContract,
    UpdateGuarantor,
    UpdateTenant,
} from "../types";
import { objectToCamelCase, objectToSnakeCase } from "../utils";

// Helper function to convert date fields to RFC3339 format
function formatDateForAPI(obj: any): any {
    if (!obj || typeof obj !== "object") return obj;

    const result = Array.isArray(obj) ? [...obj] : { ...obj };

    for (const key in result) {
        if (result[key] && typeof result[key] === "object") {
            result[key] = formatDateForAPI(result[key]);
        } else if (typeof result[key] === "string") {
            // Check if it's a date field (startDate, endDate, etc.) and looks like YYYY-MM-DD
            if (
                (key === "startDate" || key === "endDate" ||
                    key === "start_date" || key === "end_date") &&
                /^\d{4}-\d{2}-\d{2}$/.test(result[key])
            ) {
                // Convert YYYY-MM-DD to YYYY-MM-DDTHH:mm:ssZ (RFC3339)
                result[key] = result[key] + "T00:00:00Z";
            }
        }
    }

    return result;
}

// Helper function to convert string numeric fields to numbers
function convertNumericFields(obj: any): any {
    if (!obj || typeof obj !== "object") return obj;

    const result = Array.isArray(obj) ? [...obj] : { ...obj };

    for (const key in result) {
        if (result[key] && typeof result[key] === "object") {
            result[key] = convertNumericFields(result[key]);
        } else if (typeof result[key] === "string") {
            // Check if it's a numeric field and convert to number
            if (
                (key === "deposit" || key === "rent" || key === "amount" ||
                    key === "price" || key === "cost") &&
                /^\d+(\.\d+)?$/.test(result[key])
            ) {
                result[key] = parseFloat(result[key]);
            }
        }
    }

    return result;
}

// Helper function to convert RFC3339 date fields back to simple date strings
function formatDateFromAPI(obj: any): any {
    if (!obj || typeof obj !== "object") return obj;

    const result = Array.isArray(obj) ? [...obj] : { ...obj };

    for (const key in result) {
        if (result[key] && typeof result[key] === "object") {
            result[key] = formatDateFromAPI(result[key]);
        } else if (typeof result[key] === "string") {
            // Check if it's a date field and looks like RFC3339 (YYYY-MM-DDTHH:mm:ssZ)
            if (
                (key === "startDate" || key === "endDate" ||
                    key === "start_date" || key === "end_date") &&
                /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(.?\d{3})?Z?$/.test(
                    result[key],
                )
            ) {
                // Convert to YYYY-MM-DD
                result[key] = result[key].split("T")[0];
            }
        }
    }

    return result;
}

class ApiService {
    private baseUrl: string;

    constructor() {
        // Use environment variable for API base URL, fallback to relative URL for development
        this.baseUrl = import.meta.env.VITE_API_BASE_URL || "";
    }

    private async request<T>(
        endpoint: string,
        options: RequestInit = {},
    ): Promise<T> {
        const fullUrl = `${this.baseUrl}${endpoint}`;
        console.log("Making API request to:", fullUrl);

        // Only set Content-Type for requests with a body
        const headers: Record<string, string> = {};
        if (options.body) {
            headers["Content-Type"] = "application/json";
        }

        // Convert request body to snake_case if it's a JSON string
        let body = options.body;
        if (body && typeof body === "string") {
            try {
                const parsed = JSON.parse(body);
                // First format dates, then convert numeric fields, then convert to snake_case
                const dateFormattedData = formatDateForAPI(parsed);
                const numericConvertedData = convertNumericFields(
                    dateFormattedData,
                );
                const snakeCaseData = objectToSnakeCase(numericConvertedData);
                body = JSON.stringify(snakeCaseData);
            } catch (error) {
                // If parsing fails, use the original body
                console.warn(
                    "Failed to parse request body for case conversion:",
                    error,
                );
            }
        }

        const response = await fetch(fullUrl, {
            headers: {
                ...headers,
                ...options.headers,
            },
            ...options,
            body,
        });

        console.log("API response status:", response.status);

        if (!response.ok) {
            const errorText = await response.text();
            console.error("API error:", response.status, errorText);
            throw new Error(
                `HTTP error! status: ${response.status}, message: ${errorText}`,
            );
        }

        // Check if response has content before trying to parse JSON
        const contentType = response.headers.get("content-type");
        const contentLength = response.headers.get("content-length");

        // If no content or content-length is 0, return empty for void responses
        if (
            !contentType?.includes("application/json") ||
            contentLength === "0" || response.status === 204
        ) {
            return undefined as T;
        }

        // Try to parse JSON only if we expect content
        const text = await response.text();
        if (!text) {
            return undefined as T;
        }

        try {
            const data = JSON.parse(text);
            console.log("API response data (before conversion):", data);
            const camelCaseData = objectToCamelCase(data);
            const dateFormattedData = formatDateFromAPI(camelCaseData);
            console.log(
                "API response data (after conversion):",
                dateFormattedData,
            );
            return dateFormattedData ?? [] as T;
        } catch (error) {
            console.warn("Failed to parse JSON response:", error);
            return undefined as T;
        }
    }

    // Address endpoints
    getAddresses = (): Promise<Address[]> => {
        return this.request<Address[]>("/api/v1/addresses");
    };

    getAddress = (id: string): Promise<Address> => {
        return this.request<Address>(`/api/v1/addresses/${id}`);
    };

    createAddress = (address: CreateAddress): Promise<Address> => {
        return this.request<Address>("/api/v1/addresses", {
            method: "POST",
            body: JSON.stringify(address),
        });
    };

    updateAddress = (address: UpdateAddress): Promise<Address> => {
        return this.request<Address>(`/api/v1/addresses/${address.id}`, {
            method: "PUT",
            body: JSON.stringify(address),
        });
    };

    deleteAddress = (id: string): Promise<void> => {
        return this.request<void>(`/api/v1/addresses/${id}`, {
            method: "DELETE",
        });
    };

    // Tenant endpoints
    getTenants = (): Promise<Tenant[]> => {
        return this.request<Tenant[]>("/api/v1/tenants");
    };

    getTenant = (id: string): Promise<Tenant> => {
        return this.request<Tenant>(`/api/v1/tenants/${id}`);
    };

    createTenant = (tenant: CreateTenant): Promise<Tenant> => {
        return this.request<Tenant>("/api/v1/tenants", {
            method: "POST",
            body: JSON.stringify(tenant),
        });
    };

    updateTenant = (tenant: UpdateTenant): Promise<Tenant> => {
        return this.request<Tenant>(`/api/v1/tenants/${tenant.id}`, {
            method: "PUT",
            body: JSON.stringify(tenant),
        });
    };

    deleteTenant = (id: string): Promise<void> => {
        return this.request<void>(`/api/v1/tenants/${id}`, {
            method: "DELETE",
        });
    };

    // Guarantor endpoints
    getGuarantors = (): Promise<Guarantor[]> => {
        return this.request<Guarantor[]>("/api/v1/guarantors");
    };

    getGuarantor = (id: string): Promise<Guarantor> => {
        return this.request<Guarantor>(`/api/v1/guarantors/${id}`);
    };

    createGuarantor = (guarantor: CreateGuarantor): Promise<Guarantor> => {
        return this.request<Guarantor>("/api/v1/guarantors", {
            method: "POST",
            body: JSON.stringify(guarantor),
        });
    };

    updateGuarantor = (guarantor: UpdateGuarantor): Promise<Guarantor> => {
        return this.request<Guarantor>(`/api/v1/guarantors/${guarantor.id}`, {
            method: "PUT",
            body: JSON.stringify(guarantor),
        });
    };

    deleteGuarantor = (id: string): Promise<void> => {
        return this.request<void>(`/api/v1/guarantors/${id}`, {
            method: "DELETE",
        });
    };

    // Contract endpoints
    getContracts = (): Promise<Contract[]> => {
        return this.request<Contract[]>("/api/v1/contracts");
    };

    getContract = (id: string): Promise<Contract> => {
        return this.request<Contract>(`/api/v1/contracts/${id}`);
    };

    createContract = (contract: CreateContract): Promise<Contract> => {
        return this.request<Contract>("/api/v1/contracts", {
            method: "POST",
            body: JSON.stringify(contract),
        });
    };

    updateContract = (contract: UpdateContract): Promise<Contract> => {
        return this.request<Contract>(`/api/v1/contracts/${contract.id}`, {
            method: "PUT",
            body: JSON.stringify(contract),
        });
    };

    deleteContract = (id: string): Promise<void> => {
        return this.request<void>(`/api/v1/contracts/${id}`, {
            method: "DELETE",
        });
    };

    generateContractPdf = (id: string): Promise<Blob> => {
        return fetch(`${this.baseUrl}/api/v1/contracts/${id}/pdf`)
            .then((response) => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.blob();
            });
    };
}

export const apiService = new ApiService();
