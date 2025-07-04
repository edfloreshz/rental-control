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
import { objectToSnakeCase } from "../utils";

// Use relative URL to leverage Vite's proxy configuration

class ApiService {
    private async request<T>(
        endpoint: string,
        options: RequestInit = {},
    ): Promise<T> {
        console.log("Making API request to:", endpoint);

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
                const snakeCaseData = objectToSnakeCase(parsed);
                body = JSON.stringify(snakeCaseData);
            } catch (error) {
                // If parsing fails, use the original body
                console.warn(
                    "Failed to parse request body for case conversion:",
                    error,
                );
            }
        }

        const response = await fetch(endpoint, {
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

        const data = await response.json();
        console.log("API response data:", data);
        return data ?? [] as T;
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
        return fetch(`/api/v1/contracts/${id}/pdf`)
            .then((response) => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.blob();
            });
    };
}

export const apiService = new ApiService();
