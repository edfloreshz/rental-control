import { useState } from 'react';
import { useContracts, useDeleteContract, useGenerateContractPdf } from '../hooks/api';
import type { Contract } from '../types';
import { ContractStatus } from '../types';
import { downloadFile } from '../utils';
import ContractForm from './ContractForm';

export default function Contracts() {
    const [isFormOpen, setIsFormOpen] = useState(false);
    const [selectedContract, setSelectedContract] = useState<Contract | null>(null);
    const { data: contracts = [], isLoading } = useContracts();
    const deleteContract = useDeleteContract();
    const generatePdf = useGenerateContractPdf();

    const handleEdit = (contract: Contract) => {
        setSelectedContract(contract);
        setIsFormOpen(true);
    };

    const handleDelete = async (id: string) => {
        if (window.confirm('Are you sure you want to delete this contract?')) {
            await deleteContract.mutateAsync(id);
        }
    };

    const handleGeneratePdf = async (id: string, tenantName: string) => {
        try {
            const blob = await generatePdf.mutateAsync(id);
            downloadFile(blob, `contract-${tenantName}.pdf`);
        } catch (error) {
            console.error('Error generating PDF:', error);
        }
    };

    const handleCloseForm = () => {
        setIsFormOpen(false);
        setSelectedContract(null);
    };

    if (isLoading) {
        return <div className="flex justify-center items-center h-64">Loading...</div>;
    }

    return (
        <div className="space-y-6">
            {/* Header */}
            <div className="flex justify-between items-center">
                <div>
                    <h1 className="text-2xl font-bold text-gray-900">Contracts</h1>
                    <p className="text-gray-600">Manage your rental contracts</p>
                </div>
                <button
                    onClick={() => setIsFormOpen(true)}
                    className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg font-medium"
                >
                    Add Contract
                </button>
            </div>

            {/* Contracts Table */}
            <div className="bg-white rounded-lg shadow overflow-hidden">
                <table className="min-w-full divide-y divide-gray-200">
                    <thead className="bg-gray-50">
                        <tr>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                Tenant
                            </th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                Property
                            </th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                Rent
                            </th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                Deposit
                            </th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                Period
                            </th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                Status
                            </th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                Actions
                            </th>
                        </tr>
                    </thead>
                    <tbody className="bg-white divide-y divide-gray-200">
                        {contracts.map((contract: Contract) => (
                            <tr key={contract.id}>
                                <td className="px-6 py-4 whitespace-nowrap">
                                    <div className="text-sm font-medium text-gray-900">
                                        {contract.tenant.name}
                                    </div>
                                    <div className="text-sm text-gray-500">
                                        {contract.tenant.email}
                                    </div>
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap">
                                    <div className="text-sm text-gray-900">
                                        {contract.address.street} {contract.address.number}
                                    </div>
                                    <div className="text-sm text-gray-500">
                                        {contract.address.city}, {contract.address.state}
                                    </div>
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap">
                                    <div className="text-sm font-medium text-gray-900">
                                        ${contract.rent.toLocaleString()}
                                    </div>
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap">
                                    <div className="text-sm text-gray-900">
                                        ${contract.deposit.toLocaleString()}
                                    </div>
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap">
                                    <div className="text-sm text-gray-900">
                                        {new Date(contract.startDate).toLocaleDateString()}
                                    </div>
                                    <div className="text-sm text-gray-500">
                                        to {new Date(contract.endDate).toLocaleDateString()}
                                    </div>
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap">
                                    <span className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${contract.status === ContractStatus.Active
                                            ? 'bg-green-100 text-green-800'
                                            : contract.status === ContractStatus.Expired
                                                ? 'bg-red-100 text-red-800'
                                                : 'bg-gray-100 text-gray-800'
                                        }`}>
                                        {contract.status}
                                    </span>
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                                    <button
                                        onClick={() => handleEdit(contract)}
                                        className="text-blue-600 hover:text-blue-900 mr-3"
                                    >
                                        Edit
                                    </button>
                                    <button
                                        onClick={() => handleGeneratePdf(contract.id, contract.tenant.name)}
                                        className="text-green-600 hover:text-green-900 mr-3"
                                    >
                                        PDF
                                    </button>
                                    <button
                                        onClick={() => handleDelete(contract.id)}
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
                <ContractForm
                    contract={selectedContract}
                    onClose={handleCloseForm}
                />
            )}
        </div>
    );
}
