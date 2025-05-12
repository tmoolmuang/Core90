import { useEffect, useState } from "react";
import axios from "axios";
import ApplicationEditModal from "./ApplicationEditModal";

const API_URL = "https://localhost:7076/api/AspnetApplications";

function ApplicationList() {
    const [applications, setApplications] = useState([]);
    const [selectedApp, setSelectedApp] = useState(null);
    const [isModalOpen, setModalOpen] = useState(false);
    const [modalMode, setModalMode] = useState("edit"); // "edit" or "insert"

    useEffect(() => {
        axios.get(API_URL)
            .then(response => {
                console.log("API Response:", response.data);
                setApplications(response.data);
            })
            .catch(error => console.error("Error fetching data:", error.response?.data || error.message));
    }, []);

    const handleEditClick = (app) => {
        setSelectedApp(app);
        setModalMode("edit");
        setModalOpen(true);
    };

    const handleAddNew = () => {
        setSelectedApp(null);
        setModalMode("insert");
        setModalOpen(true);
    };

    return (
        <div>
            <h1>Application List</h1>

            <button onClick={handleAddNew} style={{ marginBottom: "1rem" }}>
                Add New Application
            </button>

            <table border="1" cellPadding="8">
                <thead>
                    <tr>
                        <th>Application ID</th>
                        <th>Application Name</th>
                        <th>Lowered Application Name</th>
                        <th>Description</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {applications.map((app, index) => (
                        <tr key={`${app.applicationId}-${index}`}>
                            <td>{app.applicationId ?? "N/A"}</td>
                            <td>{app.applicationName ?? "N/A"}</td>
                            <td>{app.loweredApplicationName ?? "N/A"}</td>
                            <td>{app.description ?? "N/A"}</td>
                            <td>
                                <button onClick={() => handleEditClick(app)}>Edit</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {isModalOpen && (
                <ApplicationEditModal
                    mode={modalMode}
                    app={selectedApp}
                    closeModal={() => setModalOpen(false)}
                    setApplications={setApplications}
                />
            )}
        </div>
    );
}

export default ApplicationList;
