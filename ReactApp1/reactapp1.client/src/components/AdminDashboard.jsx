import React, { useState } from 'react';
import TableManager from './TableManager';

const entities = [
    {
        name: 'AspnetApplications',
        columns: ['applicationId', 'applicationName', 'loweredApplicationName', 'description'],
        idField: 'applicationId',
    },
    {
        name: 'AspnetRoles',
        columns: ['applicationId', 'roleId', 'roleName', 'loweredRoleName', 'description'],
        idField: 'roleId',
    },
    {
        name: 'AspnetUsers',
        columns: ['applicationId', 'userId', 'userName', 'loweredUserName', 'mobileAlias', 'isAnonymous', 'lastActivityDate'],
        idField: 'userId',
    },
    {
        name: 'AspnetUsersInRole',
        columns: ['userId', 'roleId'],
        idField: '', // No formal PK, handled in TableManager
    },
    {
        name: 'LuPermissions',
        columns: ['permissionId', 'permissionTitle', 'permissionDescription', 'applicationId'],
        idField: 'permissionId',
    },
    {
        name: 'PermissionInRoles',
        columns: ['permissionInRoleId', 'roleId', 'permissionId'],
        idField: 'permissionInRoleId',
    },
    {
        name: 'Profiles',
        columns: ['profileId', 'userName', 'firstName', 'middleName', 'lastName', 'active', 'email', 'oneHealthcareUuid'],
        idField: 'profileId',
    },
];

export default function AdminDashboard() {
    const [selectedIndex, setSelectedIndex] = useState(0);
    const { name, columns, idField } = entities[selectedIndex];

    return (
        <div className="d-flex vh-100 small">
            <nav className="border-end p-2" style={{ width: 200, fontSize: '0.85rem' }}>
                <h6 className="text-center mb-3">Admin Tables</h6>
                <ul className="nav flex-column">
                    {entities.map((entity, i) => (
                        <li key={entity.name} className="nav-item mb-1">
                            <button
                                className={`nav-link btn btn-outline-primary btn-sm w-100 text-start ${i === selectedIndex ? 'active' : ''}`}
                                style={{ fontSize: '0.75rem', padding: '0.15rem 0.4rem' }}
                                onClick={() => setSelectedIndex(i)}
                            >
                                {entity.name}
                            </button>
                        </li>
                    ))}
                </ul>
            </nav>

            <main className="flex-grow-1 p-2 overflow-auto" style={{ fontSize: '0.85rem' }}>
                <TableManager entityName={name} columns={columns} idField={idField} />
            </main>
        </div>
    );
}
