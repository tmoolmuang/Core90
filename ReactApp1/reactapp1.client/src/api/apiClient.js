import axios from 'axios';

const apiClient = axios.create({
    baseURL: 'https://localhost:7076/api',  // Adjust to your API base URL
    headers: {
        'Content-Type': 'application/json',
    },
});

export default apiClient;