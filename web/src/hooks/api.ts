import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { apiService } from "../services/api";

// Address hooks
export const useAddresses = () => {
    return useQuery({
        queryKey: ["addresses"],
        queryFn: apiService.getAddresses,
    });
};

export const useAddress = (id: string) => {
    return useQuery({
        queryKey: ["address", id],
        queryFn: () => apiService.getAddress(id),
        enabled: !!id,
    });
};

export const useCreateAddress = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: apiService.createAddress,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["addresses"] });
        },
    });
};

export const useUpdateAddress = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: apiService.updateAddress,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["addresses"] });
        },
    });
};

export const useDeleteAddress = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: apiService.deleteAddress,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["addresses"] });
        },
    });
};

// Tenant hooks
export const useTenants = () => {
    return useQuery({
        queryKey: ["tenants"],
        queryFn: apiService.getTenants,
    });
};

export const useTenant = (id: string) => {
    return useQuery({
        queryKey: ["tenant", id],
        queryFn: () => apiService.getTenant(id),
        enabled: !!id,
    });
};

export const useCreateTenant = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: apiService.createTenant,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["tenants"] });
        },
    });
};

export const useUpdateTenant = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: apiService.updateTenant,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["tenants"] });
        },
    });
};

export const useDeleteTenant = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: apiService.deleteTenant,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["tenants"] });
        },
    });
};

// Guarantor hooks
export const useGuarantors = () => {
    return useQuery({
        queryKey: ["guarantors"],
        queryFn: apiService.getGuarantors,
    });
};

export const useGuarantor = (id: string) => {
    return useQuery({
        queryKey: ["guarantor", id],
        queryFn: () => apiService.getGuarantor(id),
        enabled: !!id,
    });
};

export const useCreateGuarantor = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: apiService.createGuarantor,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["guarantors"] });
        },
    });
};

export const useUpdateGuarantor = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: apiService.updateGuarantor,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["guarantors"] });
        },
    });
};

export const useDeleteGuarantor = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: apiService.deleteGuarantor,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["guarantors"] });
        },
    });
};

// Contract hooks
export const useContracts = () => {
    return useQuery({
        queryKey: ["contracts"],
        queryFn: apiService.getContracts,
    });
};

export const useContract = (id: string) => {
    return useQuery({
        queryKey: ["contract", id],
        queryFn: () => apiService.getContract(id),
        enabled: !!id,
    });
};

export const useCreateContract = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: apiService.createContract,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["contracts"] });
        },
    });
};

export const useUpdateContract = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: apiService.updateContract,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["contracts"] });
        },
    });
};

export const useDeleteContract = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: apiService.deleteContract,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["contracts"] });
        },
    });
};

export const useGenerateContractPdf = () => {
    return useMutation({
        mutationFn: apiService.generateContractPdf,
    });
};
