import { useState, useEffect } from "react";
import axios from "axios";

function ApplicationEditModal({ mode = "edit", app, closeModal, setApplications }) {
    const isInsert = mode === "insert";

    const [formData, setFormData] = useState({
        applicationId: "",
        applicationName: "",
        loweredApplicationName: "",
        description: ""
    });

    useEffect(() => {
        if (!isInsert && app) {
            setFormData({
                applicationId: app.applicationId || "",
                applicationName: app.applicationName || "",
                loweredApplicationName: app.loweredApplicationName || "",
                description: app.description || ""
            });
        } else if (isInsert) {
            setFormData({
                applicationId: "00000000-0000-0000-0000-000000000000",
                applicationName: "",
                loweredApplicationName: "",
                description: ""
            });
        }
    }, [app, isInsert]);

    const handleChange = (e) => {
        const { name, value } = e.target;

        // Automatically update loweredApplicationName when applicationName changes
        if (name === "applicationName") {
            setFormData(prev => ({
                ...prev,
                applicationName: value,
                loweredApplicationName: value.toLowerCase()
            }));
        } else {
            setFormData(prev => ({
                ...prev,
                [name]: value
            }));
        }
    };

    const handleSubmit = async () => {
        try {
            if (isInsert) {
                await axios.post(`https://localhost:7076/api/AspnetApplications`, formData);

                // Re-fetch the application list after insert
                const updatedList = await axios.get("https://localhost:7076/api/AspnetApplications");
                setApplications(updatedList.data);
            } else {
                await axios.put(`https://localhost:7076/api/AspnetApplications/${formData.applicationId}`, formData);
                setApplications(prev =>
                    prev.map(item =>
                        item.applicationId === formData.applicationId ? formData : item
                    )
                );
            }
            closeModal();
        } catch (error) {
            console.error(`Error ${isInsert ? "inserting" : "updating"} application:`, error);
        }
    };

    return (
        <div className="app-modal-overlay">
            <div className="app-modal-content">
                <h2>{isInsert ? "Add New Application" : "Edit Application"}</h2>

                <label htmlFor="applicationName">Application Name:</label>
                <input
                    id="applicationName"
                    type="text"
                    name="applicationName"
                    value={formData.applicationName}
                    onChange={handleChange}
                    className="app-modal-input"
                    autoComplete="off"
               />

                <label htmlFor="loweredApplicationName">Lowered Application Name:</label>
                <input
                    id="loweredApplicationName"
                    type="text"
                    name="loweredApplicationName"
                    value={formData.loweredApplicationName}
                    onChange={handleChange}
                    readOnly
                    className="app-modal-input app-modal-input-readonly"
                    autoComplete="off"
              />

                <label htmlFor="description">Description:</label>
                <input
                    id="description"
                    type="text"
                    name="description"
                    value={formData.description}
                    onChange={handleChange}
                    className="app-modal-input"
                    autoComplete="off"
             />

                <div className="app-modal-actions">
                    <button className="btn btn-primary" onClick={handleSubmit}>
                        {isInsert ? "Insert" : "Update"}
                    </button>
                    <button className="btn btn-primary" onClick={closeModal}>Cancel</button>
                </div>
            </div>
        </div>
    );
}

export default ApplicationEditModal;
