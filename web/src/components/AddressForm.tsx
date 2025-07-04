import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { useCreateAddress, useUpdateAddress } from '../hooks/api';
import type { Address, CreateAddress, UpdateAddress } from '../types';

interface AddressFormProps {
    address?: Address | null;
    onClose: () => void;
}

interface FormData {
    street: string;
    number: string;
    neighborhood: string;
    city: string;
    state: string;
    zipCode: string;
    country: string;
}

export default function AddressForm({ address, onClose }: AddressFormProps) {
    const createAddress = useCreateAddress();
    const updateAddress = useUpdateAddress();
    const [isSubmitting, setIsSubmitting] = useState(false);

    const { register, handleSubmit, reset, formState: { errors } } = useForm<FormData>({
        defaultValues: {
            street: address?.street || '',
            number: address?.number || '',
            neighborhood: address?.neighborhood || '',
            city: address?.city || '',
            state: address?.state || '',
            zipCode: address?.zipCode || '',
            country: address?.country || '',
        },
    });

    useEffect(() => {
        if (address) {
            reset({
                street: address.street,
                number: address.number,
                neighborhood: address.neighborhood,
                city: address.city,
                state: address.state,
                zipCode: address.zipCode,
                country: address.country,
            });
        }
    }, [address, reset]);

    const onSubmit = async (data: FormData) => {
        setIsSubmitting(true);
        try {
            if (address) {
                const updateData: UpdateAddress = {
                    id: address.id,
                    ...data,
                };
                await updateAddress.mutateAsync(updateData);
            } else {
                const createData: CreateAddress = data;
                await createAddress.mutateAsync(createData);
            }
            onClose();
        } catch (error) {
            console.error('Error saving address:', error);
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
            <div className="bg-white rounded-lg p-6 w-full max-w-md">
                <h2 className="text-xl font-bold mb-4">
                    {address ? 'Edit Address' : 'Add New Address'}
                </h2>

                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            Street
                        </label>
                        <input
                            type="text"
                            {...register('street', { required: 'Street is required' })}
                            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                        {errors.street && (
                            <p className="text-red-600 text-sm mt-1">{errors.street.message}</p>
                        )}
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            Number
                        </label>
                        <input
                            type="text"
                            {...register('number', { required: 'Number is required' })}
                            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                        {errors.number && (
                            <p className="text-red-600 text-sm mt-1">{errors.number.message}</p>
                        )}
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            Neighborhood
                        </label>
                        <input
                            type="text"
                            {...register('neighborhood', { required: 'Neighborhood is required' })}
                            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                        {errors.neighborhood && (
                            <p className="text-red-600 text-sm mt-1">{errors.neighborhood.message}</p>
                        )}
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            City
                        </label>
                        <input
                            type="text"
                            {...register('city', { required: 'City is required' })}
                            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                        {errors.city && (
                            <p className="text-red-600 text-sm mt-1">{errors.city.message}</p>
                        )}
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            State
                        </label>
                        <input
                            type="text"
                            {...register('state', { required: 'State is required' })}
                            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                        {errors.state && (
                            <p className="text-red-600 text-sm mt-1">{errors.state.message}</p>
                        )}
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            ZIP Code
                        </label>
                        <input
                            type="text"
                            {...register('zipCode', { required: 'ZIP Code is required' })}
                            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                        {errors.zipCode && (
                            <p className="text-red-600 text-sm mt-1">{errors.zipCode.message}</p>
                        )}
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            Country
                        </label>
                        <input
                            type="text"
                            {...register('country', { required: 'Country is required' })}
                            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                        {errors.country && (
                            <p className="text-red-600 text-sm mt-1">{errors.country.message}</p>
                        )}
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
                            {isSubmitting ? 'Saving...' : (address ? 'Update' : 'Create')}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}
