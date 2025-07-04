import { useContracts, useTenants, useAddresses, useGuarantors } from '../hooks/api';
import { ContractStatus } from '../types';
import type { Contract } from '../types';

export default function Dashboard() {
    const { data: contracts = [] } = useContracts();
    const { data: tenants = [] } = useTenants();
    const { data: addresses = [] } = useAddresses();
    const { data: guarantors = [] } = useGuarantors();

    const activeContracts = contracts.filter((c: Contract) => c.status === ContractStatus.Active);
    const totalRent = activeContracts.reduce((sum: number, contract: Contract) => sum + contract.rent, 0);

    const stats = [
        {
            name: 'Total Tenants',
            value: tenants.length,
            icon: '👥',
            color: 'bg-blue-500',
        },
        {
            name: 'Active Contracts',
            value: activeContracts.length,
            icon: '📋',
            color: 'bg-green-500',
        },
        {
            name: 'Total Properties',
            value: addresses.length,
            icon: '🏠',
            color: 'bg-purple-500',
        },
        {
            name: 'Total Guarantors',
            value: guarantors.length,
            icon: '🤝',
            color: 'bg-orange-500',
        },
    ];

    return (
        <div className="space-y-8">
            {/* Header */}
            <div>
                <h1 className="text-2xl font-bold text-gray-900">Dashboard</h1>
                <p className="text-gray-600">Overview of your rental properties</p>
            </div>

            {/* Stats Grid */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                {stats.map((stat) => (
                    <div key={stat.name} className="bg-white rounded-lg shadow p-6">
                        <div className="flex items-center">
                            <div className={`p-3 rounded-full ${stat.color} text-white mr-4`}>
                                <span className="text-xl">{stat.icon}</span>
                            </div>
                            <div>
                                <p className="text-sm text-gray-600">{stat.name}</p>
                                <p className="text-2xl font-bold text-gray-900">{stat.value}</p>
                            </div>
                        </div>
                    </div>
                ))}
            </div>

            {/* Revenue Card */}
            <div className="bg-white rounded-lg shadow p-6">
                <h3 className="text-lg font-semibold text-gray-900 mb-4">Monthly Revenue</h3>
                <div className="text-3xl font-bold text-green-600">
                    ${totalRent.toLocaleString()}
                </div>
                <p className="text-sm text-gray-600 mt-2">
                    From {activeContracts.length} active contracts
                </p>
            </div>

            {/* Recent Contracts */}
            <div className="bg-white rounded-lg shadow">
                <div className="px-6 py-4 border-b border-gray-200">
                    <h3 className="text-lg font-semibold text-gray-900">Recent Contracts</h3>
                </div>
                <div className="overflow-x-auto">
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
                                    Status
                                </th>
                            </tr>
                        </thead>
                        <tbody className="bg-white divide-y divide-gray-200">
                            {contracts.slice(0, 5).map((contract: Contract) => (
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
                                        <span className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${contract.status === ContractStatus.Active
                                                ? 'bg-green-100 text-green-800'
                                                : contract.status === ContractStatus.Expired
                                                    ? 'bg-red-100 text-red-800'
                                                    : 'bg-gray-100 text-gray-800'
                                            }`}>
                                            {contract.status}
                                        </span>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
}
