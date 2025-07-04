import { useState } from 'react';
import { useGuarantors, useDeleteGuarantor } from '../hooks/api';
import type { Guarantor } from '../types';
import GuarantorForm from './GuarantorForm';

export default function Guarantors() {
    const [isFormOpen, setIsFormOpen] = useState(false);
    const [selectedGuarantor, setSelectedGuarantor] = useState<Guarantor | null>(null);
    const { data: guarantors = [], isLoading } = useGuarantors();
    const deleteGuarantor = useDeleteGuarantor();

    const handleEdit = (guarantor: Guarantor) => {
        setSelectedGuarantor(guarantor);
        setIsFormOpen(true);
    };

    const handleDelete = async (id: string) => {
        if (window.confirm('Are you sure you want to delete this guarantor?')) {
            await deleteGuarantor.mutateAsync(id);
        }
    };

    const handleCloseForm = () => {
        setIsFormOpen(false);
        setSelectedGuarantor(null);
    };

    if (isLoading) {
        return <div className="flex justify-center items-center h-64">Loading...</div>;
    }

    return (
        <div className="space-y-6">
            {/* Header */}
            <div className="flex justify-between items-center">
                <div>
                    <h1 className="text-2xl font-bold text-gray-900">Guarantors</h1>
                    <p className="text-gray-600">Manage your contract guarantors</p>
                </div>
                <button
                    onClick={() => setIsFormOpen(true)}
                    className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg font-medium"
                >
                    Add Guarantor
                </button>
            </div>

            {/* Guarantors Table */}
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
                                Actions
                            </th>
                        </tr>
                    </thead>
                    <tbody className="bg-white divide-y divide-gray-200">
                        {guarantors.map((guarantor: Guarantor) => (
                            <tr key={guarantor.id}>
                                <td className="px-6 py-4 whitespace-nowrap">
                                    <div className="text-sm font-medium text-gray-900">
                                        {guarantor.name}
                                    </div>
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap">
                                    <div className="text-sm text-gray-900">{guarantor.email}</div>
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap">
                                    <div className="text-sm text-gray-900">{guarantor.phone}</div>
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap">
                                    <div className="text-sm text-gray-900">
                                        {guarantor.address.street} {guarantor.address.number}
                                    </div>
                                    <div className="text-sm text-gray-500">
                                        {guarantor.address.city}, {guarantor.address.state}
                                    </div>
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                                    <button
                                        onClick={() => handleEdit(guarantor)}
                                        className="text-blue-600 hover:text-blue-900 mr-3"
                                    >
                                        Edit
                                    </button>
                                    <button
                                        onClick={() => handleDelete(guarantor.id)}
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
                <GuarantorForm
                    guarantor={selectedGuarantor}
                    onClose={handleCloseForm}
                />
            )}
        </div>
    );
}
