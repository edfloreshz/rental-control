import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { useAddresses, useCreateGuarantor, useUpdateGuarantor } from '../hooks/api';
import type { Guarantor, CreateGuarantor, UpdateGuarantor } from '../types';

interface GuarantorFormProps {
    guarantor?: Guarantor | null;
    onClose: () => void;
}

interface FormData {
    name: string;
    email: string;
    phone: string;
    addressId: string;
}

export default function GuarantorForm({ guarantor, onClose }: GuarantorFormProps) {
    const { data: addresses = [] } = useAddresses();
    const createGuarantor = useCreateGuarantor();
    const updateGuarantor = useUpdateGuarantor();
    const [isSubmitting, setIsSubmitting] = useState(false);

    const { register, handleSubmit, reset, formState: { errors } } = useForm<FormData>({
        defaultValues: {
            name: guarantor?.name || '',
            email: guarantor?.email || '',
            phone: guarantor?.phone || '',
            addressId: guarantor?.addressId || '',
        },
    });

    useEffect(() => {
        if (guarantor) {
            reset({
                name: guarantor.name,
                email: guarantor.email,
                phone: guarantor.phone,
                addressId: guarantor.addressId,
            });
        }
    }, [guarantor, reset]);

    const onSubmit = async (data: FormData) => {
        setIsSubmitting(true);
        try {
            if (guarantor) {
                const updateData: UpdateGuarantor = {
                    id: guarantor.id,
                    ...data,
                };
                await updateGuarantor.mutateAsync(updateData);
            } else {
                const createData: CreateGuarantor = data;
                await createGuarantor.mutateAsync(createData);
            }
            onClose();
        } catch (error) {
            console.error('Error saving guarantor:', error);
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
            <div className="bg-white rounded-lg p-6 w-full max-w-md">
                <h2 className="text-xl font-bold mb-4">
                    {guarantor ? 'Edit Guarantor' : 'Add New Guarantor'}
                </h2>

                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            Name
                        </label>
                        <input
                            type="text"
                            {...register('name', { required: 'Name is required' })}
                            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                        {errors.name && (
                            <p className="text-red-600 text-sm mt-1">{errors.name.message}</p>
                        )}
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            Email
                        </label>
                        <input
                            type="email"
                            {...register('email', {
                                required: 'Email is required',
                                pattern: {
                                    value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                                    message: 'Invalid email address'
                                }
                            })}
                            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                        {errors.email && (
                            <p className="text-red-600 text-sm mt-1">{errors.email.message}</p>
                        )}
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            Phone
                        </label>
                        <input
                            type="tel"
                            {...register('phone', { required: 'Phone is required' })}
                            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                        {errors.phone && (
                            <p className="text-red-600 text-sm mt-1">{errors.phone.message}</p>
                        )}
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            Address
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
                            {isSubmitting ? 'Saving...' : (guarantor ? 'Update' : 'Create')}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}
