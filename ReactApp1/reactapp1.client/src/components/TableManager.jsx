import React, { useEffect, useState } from 'react';
import apiClient from '../api/apiClient';

export default function TableManager({ entityName, columns, idField }) {
    const [items, setItems] = useState([]);
    const [editingItem, setEditingItem] = useState(null);
    const [newItem, setNewItem] = useState({});
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const generateRowKey = (item, columns) => {
        return columns.map(col => item[col] ?? '').join('_');
    };

    useEffect(() => {
        setItems([]);
        setEditingItem(null);
        setNewItem({});
        fetchItems();
    }, [entityName]);

    const fetchItems = async () => {
        setLoading(true);
        setError(null);
        try {
            const res = await apiClient.get(`/${entityName}`);
            setItems(res.data);
        } catch (err) {
            setError(err.message || 'Failed to load');
        } finally {
            setLoading(false);
        }
    };

    const handleInputChange = (e, setter) => {
        const { name, value, type, checked } = e.target;
        setter(prev => ({
            ...prev,
            [name]: type === 'checkbox' ? checked : value,
        }));
    };

    const handleAdd = async () => {
        try {
            await apiClient.post(`/${entityName}`, newItem);
            setNewItem({});
            fetchItems();
        } catch (err) {
            alert('Add failed: ' + err.message);
        }
    };

    const handleEdit = (item) => {
        setEditingItem(item);
    };

    const handleUpdate = async () => {
        try {
            await apiClient.put(`/${entityName}/${editingItem[idField]}`, editingItem);
            setEditingItem(null);
            fetchItems();
        } catch (err) {
            alert('Update failed: ' + err.message);
        }
    };

    const handleDelete = async (id) => {
        if (!window.confirm('Delete this item?')) return;
        try {
            await apiClient.delete(`/${entityName}/${id}`);
            fetchItems();
        } catch (err) {
            alert('Delete failed: ' + err.message);
        }
    };

    return (
        <div>
            <h2 className="mb-3">{entityName}</h2>
            {loading && <div className="alert alert-info">Loading...</div>}
            {error && <div className="alert alert-danger">{error}</div>}

            <table className="table table-bordered table-hover">
                <thead className="table-light">
                    <tr>
                        {columns.map(col => <th key={col}>{col}</th>)}
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {items.map(item => (
                        <tr key={generateRowKey(item, columns)}>
                            {columns.map(col => (
                                <td key={col}>
                                    {editingItem && generateRowKey(editingItem, columns) === generateRowKey(item, columns) ? (
                                        col === 'isAnonymous' || col === 'active' ? (
                                            <input
                                                type="checkbox"
                                                name={col}
                                                checked={!!editingItem[col]}
                                                onChange={e => handleInputChange(e, setEditingItem)}
                                            />
                                        ) : (
                                            <input
                                                className="form-control form-control-sm"
                                                style={{ fontSize: '0.75rem', padding: '0.25rem 0.4rem' }}
                                                name={col}
                                                value={editingItem[col] ?? ''}
                                                onChange={e => handleInputChange(e, setEditingItem)}
                                            />
                                        )
                                    ) : (
                                        (item[col] !== null && item[col] !== undefined)
                                            ? (typeof item[col] === 'boolean' ? (item[col] ? 'Yes' : 'No') : item[col].toString())
                                            : ''
                                    )}
                                </td>
                            ))}
                            <td>
                                {editingItem && generateRowKey(editingItem, columns) === generateRowKey(item, columns) ? (
                                    <>
                                        <button
                                            className="btn btn-sm btn-success me-2"
                                            style={{ fontSize: '0.75rem', padding: '0.15rem 0.4rem' }}
                                            onClick={handleUpdate}
                                        >
                                            Save
                                        </button>
                                        <button
                                            className="btn btn-sm btn-secondary"
                                            style={{ fontSize: '0.75rem', padding: '0.15rem 0.4rem' }}
                                            onClick={() => setEditingItem(null)}
                                        >
                                            Cancel
                                        </button>
                                    </>
                                ) : (
                                    <>
                                        <button
                                            className="btn btn-sm btn-primary me-2"
                                            style={{ fontSize: '0.75rem', padding: '0.15rem 0.4rem' }}
                                            onClick={() => handleEdit(item)}
                                        >
                                            Edit
                                        </button>
                                        <button
                                            className="btn btn-sm btn-danger"
                                            style={{ fontSize: '0.75rem', padding: '0.15rem 0.4rem' }}
                                            onClick={() => handleDelete(item[idField] || generateRowKey(item, columns))}
                                        >
                                            Delete
                                        </button>
                                    </>
                                )}
                            </td>
                        </tr>
                    ))}

                    {/* Add new */}
                    <tr>
                        {columns.map(col => (
                            <td key={col}>
                                {col === 'isAnonymous' || col === 'active' ? (
                                    <input
                                        type="checkbox"
                                        name={col}
                                        checked={!!newItem[col]}
                                        onChange={e => handleInputChange(e, setNewItem)}
                                    />
                                ) : (
                                    <input
                                        className="form-control form-control-sm"
                                        style={{ fontSize: '0.75rem', padding: '0.25rem 0.4rem' }}
                                        name={col}
                                        value={newItem[col] ?? ''}
                                        onChange={e => handleInputChange(e, setNewItem)}
                                        placeholder={col}
                                    />
                                )}
                            </td>
                        ))}
                        <td>
                            <button
                                className="btn btn-sm btn-success"
                                style={{ fontSize: '0.75rem', padding: '0.15rem 0.4rem' }}
                                onClick={handleAdd}
                            >
                                Add
                            </button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    );
}
