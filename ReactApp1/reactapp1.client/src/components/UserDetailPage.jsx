import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import apiClient from '../api/apiClient';

export default function UserDetailPage() {
    const { userId } = useParams(); // Get userId from URL param
    const [user, setUser] = useState(null);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        if (!userId) return;

        setLoading(true);
        setError(null);

        apiClient.get('/UserSearch/UserDetail', { params: { userId } })
            .then(res => {
                setUser(res.data);
                setLoading(false);
            })
            .catch(err => {
                setError(err.response?.data || err.message);
                setLoading(false);
            });
    }, [userId]);

    if (loading) return <div>Loading user details...</div>;
    if (error) return <div className="alert alert-danger">Failed to load user details: {error}</div>;
    if (!user) return <div>User not found.</div>;

    return (
        <div>
            <h3>User Details</h3>
            <ul>
                <li><strong>UserName:</strong> {user.UserName}</li>
                <li><strong>UserId:</strong> {user.UserId}</li>
                <li><strong>ApplicationId:</strong> {user.ApplicationId}</li>
                <li><strong>ApplicationName:</strong> {user.ApplicationName}</li>
                <li><strong>FirstName:</strong> {user.FirstName}</li>
                <li><strong>MiddleName:</strong> {user.MiddleName}</li>
                <li><strong>LastName:</strong> {user.LastName}</li>
                <li><strong>Active:</strong> {user.Active ? 'Yes' : 'No'}</li>
                <li><strong>Email:</strong> {user.Email}</li>
                <li><strong>OneHealthcareUUID:</strong> {user.OneHealthcareUUID}</li>
            </ul>
        </div>
    );
}
