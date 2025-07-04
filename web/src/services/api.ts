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

class ApiService {
    private async request<T>(
        endpoint: string,
        options: RequestInit = {},
    ): Promise<T> {
        console.log("Making API request to:", endpoint);

        // Set headers for PostgREST compatibility
        const headers = new Headers({
            "Accept": "application/json",
        });

        if (options.body) {
            headers.set("Content-Type", "application/json");
        }

        // Add any additional headers from options
        if (options.headers) {
            const optionsHeaders = new Headers(options.headers);
            optionsHeaders.forEach((value, key) => {
                headers.set(key, value);
            });
        }

        const response = await fetch(endpoint, {
            ...options,
            headers,
        });

        console.log("API response status:", response.status);

        if (!response.ok) {
            const errorText = await response.text();
            console.error("API error:", response.status, errorText);
            throw new Error(
                `HTTP error! status: ${response.status}, message: ${errorText}`,
            );
        }

        // Handle 204 No Content responses
        if (response.status === 204) {
            return null as T;
        }

        const data = await response.json();
        console.log("API response data:", data);
        return data;
    }

    // Address endpoints
    getAddresses = (): Promise<Address[]> => {
        return this.request<Address[]>("/api/v1/address");
    };

    getAddress = (id: string): Promise<Address> => {
        return this.request<Address>(`/api/v1/address/${id}`);
    };

    createAddress = (address: CreateAddress): Promise<Address> => {
        return this.request<Address>("/api/v1/address", {
            method: "POST",
            body: JSON.stringify(address),
        });
    };

    updateAddress = (address: UpdateAddress): Promise<Address> => {
        return this.request<Address>(`/api/v1/address/${address.id}`, {
            method: "PUT",
            body: JSON.stringify(address),
        });
    };

    deleteAddress = (id: string): Promise<void> => {
        return this.request<void>(`/api/v1/address/${id}`, {
            method: "DELETE",
        });
    };

    // Tenant endpoints
    getTenants = (): Promise<Tenant[]> => {
        return this.request<Tenant[]>("/api/v1/tenant");
    };

    getTenant = (id: string): Promise<Tenant> => {
        return this.request<Tenant>(`/api/v1/tenant/${id}`);
    };

    createTenant = (tenant: CreateTenant): Promise<Tenant> => {
        return this.request<Tenant>("/api/v1/tenant", {
            method: "POST",
            body: JSON.stringify(tenant),
        });
    };

    updateTenant = (tenant: UpdateTenant): Promise<Tenant> => {
        return this.request<Tenant>(`/api/v1/tenant/${tenant.id}`, {
            method: "PUT",
            body: JSON.stringify(tenant),
        });
    };

    deleteTenant = (id: string): Promise<void> => {
        return this.request<void>(`/api/v1/tenant/${id}`, {
            method: "DELETE",
        });
    };

    // Guarantor endpoints
    getGuarantors = (): Promise<Guarantor[]> => {
        return this.request<Guarantor[]>("/api/v1/guarantor");
    };

    getGuarantor = (id: string): Promise<Guarantor> => {
        return this.request<Guarantor>(`/api/v1/guarantor/${id}`);
    };

    createGuarantor = (guarantor: CreateGuarantor): Promise<Guarantor> => {
        return this.request<Guarantor>("/api/v1/guarantor", {
            method: "POST",
            body: JSON.stringify(guarantor),
        });
    };

    updateGuarantor = (guarantor: UpdateGuarantor): Promise<Guarantor> => {
        return this.request<Guarantor>(`/api/v1/guarantor/${guarantor.id}`, {
            method: "PUT",
            body: JSON.stringify(guarantor),
        });
    };

    deleteGuarantor = (id: string): Promise<void> => {
        return this.request<void>(`/api/v1/guarantor/${id}`, {
            method: "DELETE",
        });
    };

    // Contract endpoints
    getContracts = (): Promise<Contract[]> => {
        return this.request<Contract[]>("/api/v1/contract");
    };

    getContract = (id: string): Promise<Contract> => {
        return this.request<Contract>(`/api/v1/contract/${id}`);
    };

    createContract = (contract: CreateContract): Promise<Contract> => {
        return this.request<Contract>("/api/v1/contract", {
            method: "POST",
            body: JSON.stringify(contract),
        });
    };

    updateContract = (contract: UpdateContract): Promise<Contract> => {
        return this.request<Contract>(`/api/v1/contract/${contract.id}`, {
            method: "PUT",
            body: JSON.stringify(contract),
        });
    };

    deleteContract = (id: string): Promise<void> => {
        return this.request<void>(`/api/v1/contract/${id}`, {
            method: "DELETE",
        });
    };

    generateContractPdf = (id: string): Promise<Blob> => {
        return fetch(`/api/v1/contract/${id}/pdf`)
            .then((response) => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.blob();
            });
    };
}

export const apiService = new ApiService();
