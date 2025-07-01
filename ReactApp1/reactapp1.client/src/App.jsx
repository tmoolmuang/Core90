import React from 'react';
import { Routes, Route } from 'react-router-dom';
import AdminDashboard from './components/AdminDashboard';
import UserDetailPage from './components/UserDetailPage';

export default function App() {
    return (
        <Routes>
            <Route path="/*" element={<AdminDashboard />} />
            <Route path="/userdetail/:userId" element={<UserDetailPage />} />
        </Routes>
    );
}
