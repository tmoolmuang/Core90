import { useEffect, useState } from 'react';
import apiClient from '../api/apiClient';

export default function SearchPage() {
    const [applications, setApplications] = useState([]);
    const [usernames, setUsernames] = useState([]);
    const [filters, setFilters] = useState({ ApplicationId: '', UserName: '', LastName: '', FirstName: '' });
    const [results, setResults] = useState([]);

    useEffect(() => {
        apiClient.get('/AspnetApplications').then(res => setApplications(res.data));
    }, []);

    useEffect(() => {
        if (filters.ApplicationId) {
            apiClient.get('/AspnetUsers', { params: { ApplicationId: filters.ApplicationId } })
                .then(res => setUsernames(res.data));
        } else {
            setUsernames([]);
        }
    }, [filters.ApplicationId]);

    const handleSearch = () => {
        apiClient.get('/UserSearch', { params: filters }).then(res => setResults(res.data));
    };

    return (
        <div>
            <h4>User Search</h4>
            <div className="row mb-3">
                <div className="col">
                    <label>Application</label>
                    <select
                        className="form-select form-select-sm"
                        value={filters.ApplicationId}
                        onChange={e => setFilters(f => ({ ...f, ApplicationId: e.target.value }))}
                    >
                        <option value="">-- All --</option>
                        {applications.map(app => (
                            <option key={app.applicationId} value={app.applicationId}>{app.applicationName}</option>
                        ))}
                    </select>
                </div>
                <div className="col">
                    <label>UserName</label>
                    <select
                        className="form-select form-select-sm"
                        value={filters.UserName}
                        onChange={e => setFilters(f => ({ ...f, UserName: e.target.value }))}
                    >
                        <option value="">-- All --</option>
                        {usernames.map(user => (
                            <option key={user.userId} value={user.userName}>{user.userName}</option>
                        ))}
                    </select>
                </div>
                <div className="col">
                    <label>LastName</label>
                    <input
                        className="form-control form-control-sm"
                        value={filters.LastName}
                        onChange={e => setFilters(f => ({ ...f, LastName: e.target.value }))}
                    />
                </div>
                <div className="col">
                    <label>FirstName</label>
                    <input
                        className="form-control form-control-sm"
                        value={filters.FirstName}
                        onChange={e => setFilters(f => ({ ...f, FirstName: e.target.value }))}
                    />
                </div>
                <div className="col d-flex align-items-end">
                    <button className="btn btn-sm btn-primary w-100" onClick={handleSearch}>Search</button>
                </div>
            </div>

            <table className="table table-sm table-bordered">
                <thead>
                    <tr>
                        <th>ApplicationName</th>
                        <th>UserName</th>
                        <th>LastName</th>
                        <th>FirstName</th>
                        <th>Active</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                    {results.map(r => (
                        <tr key={r.UserId}>
                            <td>{r.ApplicationName}</td>
                            <td>{r.UserName}</td>
                            <td>{r.LastName}</td>
                            <td>{r.FirstName}</td>
                            <td>{r.Active ? 'Yes' : 'No'}</td>
                            <td><button className="btn btn-sm btn-outline-secondary">Edit</button></td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}
