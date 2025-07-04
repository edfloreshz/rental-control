export interface Address {
    id: string;
    street: string;
    number: string;
    neighborhood: string;
    city: string;
    state: string;
    zipCode: string;
    country: string;
    createdAt: string;
    contracts: Contract[];
}

export interface Tenant {
    id: string;
    addressId: string;
    name: string;
    email: string;
    phone: string;
    createdAt: string;
    address: Address;
    contracts: Contract[];
}

export interface Guarantor {
    id: string;
    addressId: string;
    name: string;
    phone: string;
    email: string;
    createdAt: string;
    address: Address;
}

export interface Contract {
    id: string;
    tenantId: string;
    addressId: string;
    deposit: number;
    rent: number;
    business: string;
    startDate: string;
    endDate: string;
    status: ContractStatus;
    type: ContractType;
    createdAt: string;
    tenant: Tenant;
    address: Address;
    guarantors: Guarantor[];
}

export const ContractStatus = {
    Active: "Active",
    Expired: "Expired",
    Terminated: "Terminated",
} as const;

export type ContractStatus = typeof ContractStatus[keyof typeof ContractStatus];

export const ContractType = {
    Yearly: "Yearly",
} as const;

export type ContractType = typeof ContractType[keyof typeof ContractType];

// Create types
export interface CreateTenant {
    addressId: string;
    name: string;
    email: string;
    phone: string;
}

export interface CreateAddress {
    street: string;
    number: string;
    neighborhood: string;
    city: string;
    state: string;
    zipCode: string;
    country: string;
}

export interface CreateGuarantor {
    addressId: string;
    name: string;
    phone: string;
    email: string;
}

export interface CreateContract {
    tenantId: string;
    addressId: string;
    deposit: number;
    rent: number;
    business: string;
    startDate: string;
    endDate: string;
    status: ContractStatus;
    type: ContractType;
    guarantorIds: string[];
}

// Update types
export interface UpdateTenant {
    id: string;
    addressId: string;
    name: string;
    email: string;
    phone: string;
}

export interface UpdateAddress {
    id: string;
    street: string;
    number: string;
    neighborhood: string;
    city: string;
    state: string;
    zipCode: string;
    country: string;
}

export interface UpdateGuarantor {
    id: string;
    addressId: string;
    name: string;
    phone: string;
    email: string;
}

export interface UpdateContract {
    id: string;
    tenantId: string;
    addressId: string;
    deposit: number;
    rent: number;
    business: string;
    startDate: string;
    endDate: string;
    status: ContractStatus;
    type: ContractType;
    guarantorIds: string[];
}
