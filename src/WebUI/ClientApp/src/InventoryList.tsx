import { useEffect, useState } from 'react';

interface InventoryItem {
    id: string;
    title: string;
    artist: string;
    year: number;
    genre: string;
    price: number;
}

function InventoryList() {
    const [inventoryItems, setInventoryItems] = useState<InventoryItem[]>();

    useEffect(() => {
        populateInventoryItems();
    }, []);

    const contents = inventoryItems === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <table className="">
            <thead>
                <tr>
                    <th>Title</th>
                    <th>Artist</th>
                    <th>Year</th>
                    <th>Genre</th>
                    <th>Price</th>
                </tr>
            </thead>
            <tbody>
                {inventoryItems.map(inventoryItem =>
                    <tr key={inventoryItem.id}>
                        <td>{inventoryItem.title}</td>
                        <td>{inventoryItem.artist}</td>
                        <td>{inventoryItem.year}</td>
                        <td>{inventoryItem.genre}</td>
                        <td>{inventoryItem.price}</td>
                    </tr>
                )}
            </tbody>
        </table>;


    return (
        <div>
            <h2>Inventory</h2>
            {contents}
        </div>
    );

    async function populateInventoryItems() {
        const response = await fetch('inventory');
        const data = await response.json();
        setInventoryItems(data);
    }
}

export default InventoryList;
