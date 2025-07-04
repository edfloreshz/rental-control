import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { useTenants, useAddresses, useGuarantors, useCreateContract, useUpdateContract } from '../hooks/api';
import type { Contract, CreateContract, UpdateContract } from '../types';
import { ContractStatus, ContractType } from '../types';

interface ContractFormProps {
    contract?: Contract | null;
    onClose: () => void;
}

interface FormData {
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

export default function ContractForm({ contract, onClose }: ContractFormProps) {
    const { data: tenants = [] } = useTenants();
    const { data: addresses = [] } = useAddresses();
    const { data: guarantors = [] } = useGuarantors();
    const createContract = useCreateContract();
    const updateContract = useUpdateContract();
    const [isSubmitting, setIsSubmitting] = useState(false);

    const { register, handleSubmit, reset, formState: { errors } } = useForm<FormData>({
        defaultValues: {
            tenantId: contract?.tenantId || '',
            addressId: contract?.addressId || '',
            deposit: contract?.deposit || 0,
            rent: contract?.rent || 0,
            business: contract?.business || '',
            startDate: contract?.startDate ? contract.startDate.split('T')[0] : '',
            endDate: contract?.endDate ? contract.endDate.split('T')[0] : '',
            status: contract?.status || ContractStatus.Active,
            type: contract?.type || ContractType.Yearly,
            guarantorIds: contract?.guarantors?.map(g => g.id) || [],
        },
    });

    useEffect(() => {
        if (contract) {
            reset({
                tenantId: contract.tenantId,
                addressId: contract.addressId,
                deposit: contract.deposit,
                rent: contract.rent,
                business: contract.business,
                startDate: contract.startDate.split('T')[0],
                endDate: contract.endDate.split('T')[0],
                status: contract.status,
                type: contract.type,
                guarantorIds: contract.guarantors.map(g => g.id),
            });
        }
    }, [contract, reset]);

    const onSubmit = async (data: FormData) => {
        setIsSubmitting(true);
        try {
            if (contract) {
                const updateData: UpdateContract = {
                    id: contract.id,
                    ...data,
                };
                await updateContract.mutateAsync(updateData);
            } else {
                const createData: CreateContract = data;
                await createContract.mutateAsync(createData);
            }
            onClose();
        } catch (error) {
            console.error('Error saving contract:', error);
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
            <div className="bg-white rounded-lg p-6 w-full max-w-2xl max-h-[90vh] overflow-y-auto">
                <h2 className="text-xl font-bold mb-4">
                    {contract ? 'Edit Contract' : 'Add New Contract'}
                </h2>

                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">
                                Tenant
                            </label>
                            <select
                                {...register('tenantId', { required: 'Tenant is required' })}
                                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                            >
                                <option value="">Select a tenant</option>
                                {tenants.map((tenant) => (
                                    <option key={tenant.id} value={tenant.id}>
                                        {tenant.name}
                                    </option>
                                ))}
                            </select>
                            {errors.tenantId && (
                                <p className="text-red-600 text-sm mt-1">{errors.tenantId.message}</p>
                            )}
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">
                                Property Address
                            </label>
                            <select
                                {...register('addressId', { required: 'Address is required' })}
                                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                            >
                                <option value="">Select an address</option>
                                {addresses.map((address) => (
                                    <option key={address.id} value={address.id}>
                                        {address.street} {address.number}, {address.city}
                                    </option>
                                ))}
                            </select>
                            {errors.addressId && (
                                <p className="text-red-600 text-sm mt-1">{errors.addressId.message}</p>
                            )}
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">
                                Monthly Rent
                            </label>
                            <input
                                type="number"
                                step="0.01"
                                {...register('rent', { required: 'Rent is required', min: 0 })}
                                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                            />
                            {errors.rent && (
                                <p className="text-red-600 text-sm mt-1">{errors.rent.message}</p>
                            )}
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">
                                Security Deposit
                            </label>
                            <input
                                type="number"
                                step="0.01"
                                {...register('deposit', { required: 'Deposit is required', min: 0 })}
                                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                            />
                            {errors.deposit && (
                                <p className="text-red-600 text-sm mt-1">{errors.deposit.message}</p>
                            )}
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">
                                Business Purpose
                            </label>
                            <input
                                type="text"
                                {...register('business', { required: 'Business purpose is required' })}
                                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                                placeholder="e.g., Residential, Commercial"
                            />
                            {errors.business && (
                                <p className="text-red-600 text-sm mt-1">{errors.business.message}</p>
                            )}
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">
                                Contract Type
                            </label>
                            <select
                                {...register('type', { required: 'Contract type is required' })}
                                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                            >
                                <option value={ContractType.Yearly}>Yearly</option>
                            </select>
                            {errors.type && (
                                <p className="text-red-600 text-sm mt-1">{errors.type.message}</p>
                            )}
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">
                                Start Date
                            </label>
                            <input
                                type="date"
                                {...register('startDate', { required: 'Start date is required' })}
                                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                            />
                            {errors.startDate && (
                                <p className="text-red-600 text-sm mt-1">{errors.startDate.message}</p>
                            )}
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">
                                End Date
                            </label>
                            <input
                                type="date"
                                {...register('endDate', { required: 'End date is required' })}
                                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                            />
                            {errors.endDate && (
                                <p className="text-red-600 text-sm mt-1">{errors.endDate.message}</p>
                            )}
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">
                                Status
                            </label>
                            <select
                                {...register('status', { required: 'Status is required' })}
                                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                            >
                                <option value={ContractStatus.Active}>Active</option>
                                <option value={ContractStatus.Expired}>Expired</option>
                                <option value={ContractStatus.Terminated}>Terminated</option>
                            </select>
                            {errors.status && (
                                <p className="text-red-600 text-sm mt-1">{errors.status.message}</p>
                            )}
                        </div>
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            Guarantors
                        </label>
                        <div className="space-y-2 max-h-32 overflow-y-auto">
                            {guarantors.map((guarantor) => (
                                <label key={guarantor.id} className="flex items-center">
                                    <input
                                        type="checkbox"
                                        value={guarantor.id}
                                        {...register('guarantorIds')}
                                        className="mr-2"
                                    />
                                    <span className="text-sm">{guarantor.name} - {guarantor.email}</span>
                                </label>
                            ))}
                        </div>
                    </div>

                    <div className="flex justify-end space-x-3 pt-4">
                        <button
                            type="button"
                            onClick={onClose}
                            className="px-4 py-2 text-gray-700 bg-gray-200 rounded-md hover:bg-gray-300"
                        >
                            Cancel
                        </button>
                        <button
                            type="submit"
                            disabled={isSubmitting}
                            className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 disabled:opacity-50"
                        >
                            {isSubmitting ? 'Saving...' : (contract ? 'Update' : 'Create')}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}
