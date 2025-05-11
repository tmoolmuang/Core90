import { useEffect, useState } from "react";
import axios from "axios";
import ApplicationEditModal from "./ApplicationEditModal"; // Updated import

const API_URL = "https://localhost:7076/api/AspnetApplications";

function ApplicationList() {
    const [applications, setApplications] = useState([]);
    const [selectedApp, setSelectedApp] = useState(null);
    const [isModalOpen, setModalOpen] = useState(false);

    useEffect(() => {
        axios.get(API_URL)
            .then(response => {
                console.log("API Response:", response.data);
                setApplications(response.data);
            })
            .catch(error => console.error("Error fetching data:", error.response?.data || error.message));
    }, []);

    //const handleEditClick = (app) => {
    //    setSelectedApp(app);
    //    setModalOpen(true);
    //};
    const handleEditClick = (app) => {
        console.log("Edit button clicked for:", app); // Debugging output
        setSelectedApp(app);
        setModalOpen(true);
    };

    return (
        <div>
            <h1>Application List</h1>
            <table border="1">
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

            {console.log("Modal state:", isModalOpen)}
            {/*{isModalOpen && (*/}
            {/*    <div style={{*/}
            {/*        position: "fixed", top: 0, left: 0, width: "100vw", height: "100vh",*/}
            {/*        backgroundColor: "rgba(0,0,0,0.5)", display: "flex",*/}
            {/*        alignItems: "center", justifyContent: "center", zIndex: 9999*/}
            {/*    }}>*/}
            {/*        <div style={{ backgroundColor: "white", padding: "2rem", borderRadius: "8px" }}>*/}
            {/*            <h2>Test Modal</h2>*/}
            {/*            <button onClick={() => setModalOpen(false)}>Close</button>*/}
            {/*        </div>*/}
            {/*    </div>*/}
            {/*)}*/}

            {isModalOpen && (
                <ApplicationEditModal
                    app={selectedApp}
                    closeModal={() => setModalOpen(false)}
                    setApplications={setApplications}
                />
            )}
        </div>
    );
}

export default ApplicationList;