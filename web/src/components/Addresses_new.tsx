import { useState } from 'react';
import { useAddresses, useDeleteAddress, useTenantAddresses, usePropertyAddresses } from '../hooks/api';
import type { Address } from '../types';
import { AddressType } from '../types';
import AddressForm from './AddressForm';
import { useTranslation } from 'react-i18next';

export default function Addresses() {
    const { t } = useTranslation();
    const [isFormOpen, setIsFormOpen] = useState(false);
    const [selectedAddress, setSelectedAddress] = useState<Address | null>(null);
    const [filter, setFilter] = useState<'all' | 'tenant' | 'property'>('all');
    const { data: allAddresses = [], isLoading: allLoading, error: allError } = useAddresses();
    const { data: tenantAddresses = [], isLoading: tenantLoading, error: tenantError } = useTenantAddresses();
    const { data: propertyAddresses = [], isLoading: propertyLoading, error: propertyError } = usePropertyAddresses();
    const deleteAddress = useDeleteAddress();

    // Select the appropriate data based on filter
    const getAddressesData = () => {
        switch (filter) {
            case 'tenant':
                return { addresses: tenantAddresses, isLoading: tenantLoading, error: tenantError };
            case 'property':
                return { addresses: propertyAddresses, isLoading: propertyLoading, error: propertyError };
            default:
                return { addresses: allAddresses, isLoading: allLoading, error: allError };
        }
    };

    const { addresses, isLoading, error } = getAddressesData();

    const handleEdit = (address: Address) => {
        setSelectedAddress(address);
        setIsFormOpen(true);
    };

    const handleDelete = async (id: string) => {
        if (window.confirm(t('addresses.confirmDelete'))) {
            await deleteAddress.mutateAsync(id);
        }
    };

    const handleCloseForm = () => {
        setIsFormOpen(false);
        setSelectedAddress(null);
    };

    if (isLoading) {
        return <div className="flex justify-center items-center h-64">{t('common.loading')}</div>;
    }

    if (error) {
        return (
            <div className="flex justify-center items-center h-64">
                <div className="text-red-600">
                    Error loading addresses: {error.message}
                </div>
            </div>
        );
    }

    return (
        <div className="space-y-6">
            {/* Header */}
            <div className="flex justify-between items-center">
                <div>
                    <h1 className="text-2xl font-bold text-gray-900">{t('addresses.title')}</h1>
                    <p className="text-gray-600">{t('addresses.subtitle')}</p>
                </div>
                <button
                    onClick={() => setIsFormOpen(true)}
                    className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg font-medium"
                >
                    {t('addresses.addAddress')}
                </button>
            </div>

            {/* Filter buttons */}
            <div className="flex space-x-4">
                <button
                    onClick={() => setFilter('all')}
                    className={`px-4 py-2 rounded-lg font-medium ${filter === 'all'
                            ? 'bg-blue-600 text-white'
                            : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
                        }`}
                >
                    {t('addresses.filters.all')}
                </button>
                <button
                    onClick={() => setFilter('tenant')}
                    className={`px-4 py-2 rounded-lg font-medium ${filter === 'tenant'
                            ? 'bg-blue-600 text-white'
                            : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
                        }`}
                >
                    {t('addresses.filters.tenant')}
                </button>
                <button
                    onClick={() => setFilter('property')}
                    className={`px-4 py-2 rounded-lg font-medium ${filter === 'property'
                            ? 'bg-blue-600 text-white'
                            : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
                        }`}
                >
                    {t('addresses.filters.property')}
                </button>
            </div>

            {/* Empty state */}
            {(!addresses || addresses.length === 0) && (
                <div className="text-center py-12">
                    <p className="text-gray-500">{t('addresses.noAddresses')}</p>
                </div>
            )}

            {/* Addresses Grid */}
            {addresses && addresses.length > 0 && (
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    {addresses.map((address: Address) => (
                        <div key={address.id} className="bg-white rounded-lg shadow p-6">
                            <div className="flex justify-between items-start mb-4">
                                <div className="flex-1">
                                    <h3 className="text-lg font-semibold text-gray-900">
                                        {address.street} {address.number}
                                    </h3>
                                    <p className="text-sm text-gray-600">
                                        {address.neighborhood}
                                    </p>
                                    <p className="text-sm text-gray-600">
                                        {address.city}, {address.state} {address.zipCode}
                                    </p>
                                    <p className="text-sm text-gray-600">
                                        {address.country}
                                    </p>
                                    <span className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full mt-2 ${address.type === AddressType.Property
                                            ? 'bg-green-100 text-green-800'
                                            : 'bg-blue-100 text-blue-800'
                                        }`}>
                                        {t(`addresses.types.${address.type}`)}
                                    </span>
                                </div>
                                <div className="flex space-x-2">
                                    <button
                                        onClick={() => handleEdit(address)}
                                        className="text-blue-600 hover:text-blue-900"
                                    >
                                        {t('common.edit')}
                                    </button>
                                    <button
                                        onClick={() => handleDelete(address.id)}
                                        className="text-red-600 hover:text-red-900"
                                    >
                                        {t('common.delete')}
                                    </button>
                                </div>
                            </div>
                            <div className="mb-4">
                                <div className="text-sm text-gray-500">
                                    {t('navigation.contracts')}: {address.contracts?.length || 0}
                                </div>
                                <div className="text-sm text-gray-500">
                                    {t('tenants.createdAt')}: {new Date(address.createdAt).toLocaleDateString()}
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}

            {/* Form Modal */}
            {isFormOpen && (
                <AddressForm
                    address={selectedAddress}
                    onClose={handleCloseForm}
                />
            )}
        </div>
    );
}
