import { BrowserRouter, Routes, Route } from 'react-router-dom';
import InventoryList from './components/InventoryList';
import AlbumDetails from './components/AlbumDetails';

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<InventoryList />} />
                <Route path="/:id" element={<AlbumDetails />} />
            </Routes>
        </BrowserRouter>
    );
}

export default App;
