import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import ApplicationList from "./components/ApplicationList.jsx";

function App() {
    return (
        <Router>
            <nav>
                <Link to="/">Application List</Link>
            </nav>
            <Routes>
                <Route path="/" element={<ApplicationList />} />
            </Routes>
        </Router>
    );
}

export default App;