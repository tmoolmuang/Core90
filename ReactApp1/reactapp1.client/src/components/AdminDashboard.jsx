import { useState } from 'react';
import TableManager from './TableManager';
import SearchPage from './SearchPage';

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
        idField: '',
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
    {
        name: 'UserSearch',
        isSearch: true,
    },
];

export default function AdminDashboard() {
    const [selectedEntity, setSelectedEntity] = useState(entities[0]);

    return (
        <div className="d-flex vh-100">
            <div className="bg-light p-3" style={{ minWidth: '200px' }}>
                <h5>Admin Menu</h5>
                {entities.map((entity) => (
                    <button
                        key={entity.name}
                        className="btn btn-sm btn-outline-primary w-100 mb-2"
                        onClick={() => setSelectedEntity(entity)}
                    >
                        {entity.isSearch ? 'User Search' : entity.name}
                    </button>
                ))}
            </div>
            <div className="flex-fill p-3 overflow-auto">
                {selectedEntity.isSearch ? (
                    <SearchPage />
                ) : (
                    <TableManager
                        entityName={selectedEntity.name}
                        columns={selectedEntity.columns}
                        idField={selectedEntity.idField}
                    />
                )}
            </div>
        </div>
    );
}
