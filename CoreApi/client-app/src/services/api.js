import axios from 'axios';

const API_URL = 'http://localhost:7267/api/Profiles';

export const getProfiles = async () => {
    const response = await axios.get(API_URL);
    return response.data;
};

export const updateProfile = async (id, profileData) => {
    await axios.put(`${API_URL}/${id}`, profileData);
};