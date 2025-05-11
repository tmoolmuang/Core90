import { useState, useEffect } from "react";
import axios from "axios";

function ApplicationEditModal({ app, closeModal, setApplications }) {
    const [formData, setFormData] = useState({
        applicationId: "",
        applicationName: "",
        loweredApplicationName: "",
        description: ""
    });

    // Ensure formData is updated when `app` changes
    useEffect(() => {
        if (app) {
            setFormData({
                applicationId: app.applicationId || "",
                applicationName: app.applicationName || "",
                loweredApplicationName: app.loweredApplicationName || "",
                description: app.description || ""
            });
        }
    }, [app]);

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleUpdate = async () => {
        try {
            await axios.put(`https://localhost:7076/api/AspnetApplications/${formData.applicationId}`, formData);
            setApplications(prev =>
                prev.map(item =>
                    item.applicationId === formData.applicationId ? formData : item
                )
            );
            closeModal();
        } catch (error) {
            console.error("Error updating application:", error);
        }
    };

    return (
        <div style={{
            position: "fixed",
            top: 0,
            left: 0,
            width: "100vw",
            height: "100vh",
            backgroundColor: "rgba(0,0,0,0.5)",
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            zIndex: 1000
        }}>
            <div style={{
                backgroundColor: "white",
                padding: "2rem",
                borderRadius: "8px",
                minWidth: "300px"
            }}>
                <h2>Edit Application</h2>

                {/*<label htmlFor="applicationName">Application Name:</label>*/}
                {/*<input*/}
                {/*    id="applicationName"*/}
                {/*    name="applicationName"*/}
                {/*    value={formData.applicationName}*/}
                {/*    onChange={handleChange}*/}
                {/*    style={{ display: "block", width: "100%", marginBottom: "1rem" }}*/}
                {/*/>*/}


                <label htmlFor="applicationName">Application Name:</label>
                <input
                    id="applicationName"
                    type="text"
                    name="applicationName"
                    value={formData.applicationName}
                    onChange={handleChange}
                    style={{ display: "block", width: "100%", marginBottom: "1rem" }}
                />

                <label htmlFor="loweredApplicationName">Lowered Application Name:</label>
                <input
                    id="loweredApplicationName"
                    type="text"
                    name="loweredApplicationName"
                    value={formData.loweredApplicationName}
                    onChange={handleChange}
                    style={{ display: "block", width: "100%", marginBottom: "1rem" }}
                />

                <label htmlFor="description">Description:</label>
                <input
                    id="description"
                    type="text"
                    name="description"
                    value={formData.description}
                    onChange={handleChange}
                    style={{ display: "block", width: "100%", marginBottom: "1rem" }}
                />

                <div style={{ display: "flex", gap: "1rem", justifyContent: "flex-end" }}>
                    <button onClick={handleUpdate}>Update</button>
                    <button onClick={closeModal}>Cancel</button>
                </div>
            </div>
        </div>
    );
}

export default ApplicationEditModal;
