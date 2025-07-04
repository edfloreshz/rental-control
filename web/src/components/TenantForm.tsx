import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { useTenantAddresses, useCreateTenant, useUpdateTenant } from '../hooks/api';
import type { Tenant, CreateTenant, UpdateTenant } from '../types';
import { useTranslation } from 'react-i18next';

interface TenantFormProps {
    tenant?: Tenant | null;
    onClose: () => void;
}

interface FormData {
    name: string;
    email: string;
    phone: string;
    addressId: string;
}

export default function TenantForm({ tenant, onClose }: TenantFormProps) {
    const { t } = useTranslation();
    const { data: addresses = [] } = useTenantAddresses();
    const createTenant = useCreateTenant();
    const updateTenant = useUpdateTenant();
    const [isSubmitting, setIsSubmitting] = useState(false);

    const { register, handleSubmit, reset, formState: { errors } } = useForm<FormData>({
        defaultValues: {
            name: tenant?.name || '',
            email: tenant?.email || '',
            phone: tenant?.phone || '',
            addressId: tenant?.addressId || '',
        },
    });

    useEffect(() => {
        if (tenant) {
            reset({
                name: tenant.name,
                email: tenant.email,
                phone: tenant.phone,
                addressId: tenant.addressId,
            });
        }
    }, [tenant, reset]);

    const onSubmit = async (data: FormData) => {
        setIsSubmitting(true);
        try {
            if (tenant) {
                const updateData: UpdateTenant = {
                    id: tenant.id,
                    ...data,
                };
                await updateTenant.mutateAsync(updateData);
            } else {
                const createData: CreateTenant = data;
                await createTenant.mutateAsync(createData);
            }
            onClose();
        } catch (error) {
            console.error('Error saving tenant:', error);
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
            <div className="bg-white rounded-lg p-6 w-full max-w-md">
                <h2 className="text-xl font-bold mb-4">
                    {tenant ? t('tenants.editTenant') : t('tenants.addNewTenant')}
                </h2>

                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            {t('common.name')}
                        </label>
                        <input
                            type="text"
                            {...register('name', { required: t('tenants.nameRequired') })}
                            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                        {errors.name && (
                            <p className="text-red-600 text-sm mt-1">{errors.name.message}</p>
                        )}
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            {t('common.email')}
                        </label>
                        <input
                            type="email"
                            {...register('email', {
                                required: t('tenants.emailRequired'),
                                pattern: {
                                    value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                                    message: t('common.invalidEmail')
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
                            {t('common.phone')}
                        </label>
                        <input
                            type="tel"
                            {...register('phone', { required: t('tenants.phoneRequired') })}
                            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                        {errors.phone && (
                            <p className="text-red-600 text-sm mt-1">{errors.phone.message}</p>
                        )}
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            {t('common.address')}
                        </label>
                        <select
                            {...register('addressId', { required: t('tenants.addressRequired') })}
                            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        >
                            <option value="">{t('tenants.selectAddress')}</option>
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
                            className="px-4 py-2 text-gray-600 hover:text-gray-800"
                        >
                            {t('common.cancel')}
                        </button>
                        <button
                            type="submit"
                            disabled={isSubmitting}
                            className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-md disabled:opacity-50"
                        >
                            {isSubmitting ? t('common.loading') : t('common.save')}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}
