import { useState } from 'react';
import { useTenants, useDeleteTenant } from '../hooks/api';
import type { Tenant } from '../types';
import TenantForm from './TenantForm';

export default function Tenants() {
    const [isFormOpen, setIsFormOpen] = useState(false);
    const [selectedTenant, setSelectedTenant] = useState<Tenant | null>(null);
    const { data: tenants = [], isLoading } = useTenants();
    const deleteTenant = useDeleteTenant();

    const handleEdit = (tenant: Tenant) => {
        setSelectedTenant(tenant);
        setIsFormOpen(true);
    };

    const handleDelete = async (id: string) => {
        if (window.confirm('Are you sure you want to delete this tenant?')) {
            await deleteTenant.mutateAsync(id);
        }
    };

    const handleCloseForm = () => {
        setIsFormOpen(false);
        setSelectedTenant(null);
    };

    if (isLoading) {
        return <div className="flex justify-center items-center h-64">Loading...</div>;
    }

    return (
        <div className="space-y-6">
            {/* Header */}
            <div className="flex justify-between items-center">
                <div>
                    <h1 className="text-2xl font-bold text-gray-900">Tenants</h1>
                    <p className="text-gray-600">Manage your tenants</p>
                </div>
                <button
                    onClick={() => setIsFormOpen(true)}
                    className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg font-medium"
                >
                    Add Tenant
                </button>
            </div>

            {/* Tenants Table */}
            <div className="bg-white rounded-lg shadow overflow-hidden">
                <table className="min-w-full divide-y divide-gray-200">
                    <thead className="bg-gray-50">
                        <tr>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                Name
                            </th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                Email
                            </th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                Phone
                            </th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                Address
                            </th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                Contracts
                            </th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                Actions
                            </th>
                        </tr>
                    </thead>
                    <tbody className="bg-white divide-y divide-gray-200">
                        {tenants.map((tenant: Tenant) => (
                            <tr key={tenant.id}>
                                <td className="px-6 py-4 whitespace-nowrap">
                                    <div className="text-sm font-medium text-gray-900">
                                        {tenant.name}
                                    </div>
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap">
                                    <div className="text-sm text-gray-900">{tenant.email}</div>
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap">
                                    <div className="text-sm text-gray-900">{tenant.phone}</div>
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap">
                                    <div className="text-sm text-gray-900">
                                        {tenant.address.street} {tenant.address.number}
                                    </div>
                                    <div className="text-sm text-gray-500">
                                        {tenant.address.city}, {tenant.address.state}
                                    </div>
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap">
                                    <div className="text-sm text-gray-900">
                                        {tenant.contracts?.length || 0} contracts
                                    </div>
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                                    <button
                                        onClick={() => handleEdit(tenant)}
                                        className="text-blue-600 hover:text-blue-900 mr-3"
                                    >
                                        Edit
                                    </button>
                                    <button
                                        onClick={() => handleDelete(tenant.id)}
                                        className="text-red-600 hover:text-red-900"
                                    >
                                        Delete
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>

            {/* Form Modal */}
            {isFormOpen && (
                <TenantForm
                    tenant={selectedTenant}
                    onClose={handleCloseForm}
                />
            )}
        </div>
    );
}
