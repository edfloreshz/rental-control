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

const API_BASE_URL = "http://rentalcontrol:8080";

class ApiService {
    private async request<T>(
        endpoint: string,
        options: RequestInit = {},
    ): Promise<T> {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, {
            headers: {
                "Content-Type": "application/json",
                ...options.headers,
            },
            ...options,
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        return response.json();
    }

    // Address endpoints
    async getAddresses(): Promise<Address[]> {
        return this.request<Address[]>("/api/v1/address");
    }

    async getAddress(id: string): Promise<Address> {
        return this.request<Address>(`/api/v1/address/${id}`);
    }

    async createAddress(address: CreateAddress): Promise<Address> {
        return this.request<Address>("/api/v1/address", {
            method: "POST",
            body: JSON.stringify({ address }),
        });
    }

    async updateAddress(address: UpdateAddress): Promise<Address> {
        return this.request<Address>(`/api/v1/address/${address.id}`, {
            method: "PUT",
            body: JSON.stringify({ address }),
        });
    }

    async deleteAddress(id: string): Promise<void> {
        return this.request<void>(`/api/v1/address/${id}`, {
            method: "DELETE",
        });
    }

    // Tenant endpoints
    async getTenants(): Promise<Tenant[]> {
        return this.request<Tenant[]>("/api/v1/tenant");
    }

    async getTenant(id: string): Promise<Tenant> {
        return this.request<Tenant>(`/api/v1/tenant/${id}`);
    }

    async createTenant(tenant: CreateTenant): Promise<Tenant> {
        return this.request<Tenant>("/api/v1/tenant", {
            method: "POST",
            body: JSON.stringify({ tenant }),
        });
    }

    async updateTenant(tenant: UpdateTenant): Promise<Tenant> {
        return this.request<Tenant>(`/api/v1/tenant/${tenant.id}`, {
            method: "PUT",
            body: JSON.stringify({ tenant }),
        });
    }

    async deleteTenant(id: string): Promise<void> {
        return this.request<void>(`/api/v1/tenant/${id}`, {
            method: "DELETE",
        });
    }

    // Guarantor endpoints
    async getGuarantors(): Promise<Guarantor[]> {
        return this.request<Guarantor[]>("/api/v1/guarantor");
    }

    async getGuarantor(id: string): Promise<Guarantor> {
        return this.request<Guarantor>(`/api/v1/guarantor/${id}`);
    }

    async createGuarantor(guarantor: CreateGuarantor): Promise<Guarantor> {
        return this.request<Guarantor>("/api/v1/guarantor", {
            method: "POST",
            body: JSON.stringify({ guarantor }),
        });
    }

    async updateGuarantor(guarantor: UpdateGuarantor): Promise<Guarantor> {
        return this.request<Guarantor>(`/api/v1/guarantor/${guarantor.id}`, {
            method: "PUT",
            body: JSON.stringify({ guarantor }),
        });
    }

    async deleteGuarantor(id: string): Promise<void> {
        return this.request<void>(`/api/v1/guarantor/${id}`, {
            method: "DELETE",
        });
    }

    // Contract endpoints
    async getContracts(): Promise<Contract[]> {
        return this.request<Contract[]>("/api/v1/contract");
    }

    async getContract(id: string): Promise<Contract> {
        return this.request<Contract>(`/api/v1/contract/${id}`);
    }

    async createContract(contract: CreateContract): Promise<Contract> {
        return this.request<Contract>("/api/v1/contract", {
            method: "POST",
            body: JSON.stringify({ contract }),
        });
    }

    async updateContract(contract: UpdateContract): Promise<Contract> {
        return this.request<Contract>(`/api/v1/contract/${contract.id}`, {
            method: "PUT",
            body: JSON.stringify({ contract }),
        });
    }

    async deleteContract(id: string): Promise<void> {
        return this.request<void>(`/api/v1/contract/${id}`, {
            method: "DELETE",
        });
    }

    async generateContractPdf(id: string): Promise<Blob> {
        const response = await fetch(
            `${API_BASE_URL}/api/v1/contract/${id}/pdf`,
        );
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.blob();
    }
}

export const apiService = new ApiService();
