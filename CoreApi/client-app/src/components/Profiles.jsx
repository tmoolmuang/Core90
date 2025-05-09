import React, { useEffect, useState } from 'react';
import { getProfiles } from "../services/api";
import { EditProfileModal } from "../services/api";
//import EditProfileModal from './EditProfileModal';

const Profiles = () => {
    const [profiles, setProfiles] = useState([]);
    const [selectedProfile, setSelectedProfile] = useState(null);

    useEffect(() => {
        fetchProfiles();
    }, []);

    const fetchProfiles = async () => {
        const data = await getProfiles();
        setProfiles(data);
    };

    return (
        <div>
            <h2>Profile List</h2>
            <table border="1">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>UserName</th>
                        <th>Email</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                    {profiles.map((profile) => (
                        <tr key={profile.profileId}>
                            <td>{profile.profileId}</td>
                            <td>{profile.userName}</td>
                            <td>{profile.email}</td>
                            <td>
                                <button onClick={() => setSelectedProfile(profile)}>Edit</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {selectedProfile && (
                <EditProfileModal profile={selectedProfile} onClose={() => setSelectedProfile(null)} refresh={fetchProfiles} />
            )}
        </div>
    );
};

export default Profiles;