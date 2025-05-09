import React, { useState } from 'react';
import { updateProfile } from './api';

const EditProfileModal = ({ profile, onClose, refresh }) => {
    const [formData, setFormData] = useState(profile);

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleUpdate = async () => {
        await updateProfile(formData.profileId, formData);
        refresh();
        onClose();
    };

    return (
        <div style={{
            position: 'fixed', top: '50%', left: '50%', transform: 'translate(-50%, -50%)',
            background: 'white', padding: '20px', borderRadius: '5px'
        }}>
            <h3>Edit Profile</h3>
            <label>UserName: <input type="text" name="userName" value={formData.userName} onChange={handleChange} /></label><br />
            <label>Email: <input type="email" name="email" value={formData.email} onChange={handleChange} /></label><br />
            <button onClick={handleUpdate}>Update</button>
            <button onClick={onClose}>Cancel</button>
        </div>
    );
};

export default EditProfileModal;