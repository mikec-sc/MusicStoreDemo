import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';

interface InventoryItem {
    id: string;
    title: string;
    artist: string;
    year: number;
    genre: string;
    price: number;
    stockCount: number;
    quantity: number;
}

function InventoryList() {
    const [inventoryItems, setInventoryItems] = useState<InventoryItem[]>([]);
    const [sortedColumn, setSortedColumn] = useState<string>('');
    const [sortDirection, setSortDirection] = useState<'asc' | 'desc'>('asc');
    const [filterText, setFilterText] = useState<string>('');
    const [purchaseStatus, setPurchaseStatus] = useState<string>('');

    useEffect(() => {
        populateInventoryItems();
    }, []);

    const sortedItems = [...inventoryItems].sort((a, b) => {
        if (sortedColumn === 'title') {
            return sortDirection === 'asc' ? a.title.localeCompare(b.title) : b.title.localeCompare(a.title);
        } else if (sortedColumn === 'artist') {
            return sortDirection === 'asc' ? a.artist.localeCompare(b.artist) : b.artist.localeCompare(a.artist);
        } else if (sortedColumn === 'year') {
            return sortDirection === 'asc' ? a.year - b.year : b.year - a.year;
        } else if (sortedColumn === 'genre') {
            return sortDirection === 'asc' ? a.genre.localeCompare(b.genre) : b.genre.localeCompare(a.genre);
        } else if (sortedColumn === 'price') {
            return sortDirection === 'asc' ? a.price - b.price : b.price - a.price;
        } else if (sortedColumn === 'stockCount') {
            return sortDirection === 'asc' ? a.stockCount - b.stockCount : b.stockCount - a.stockCount;
        }
        return 0;
    });

    const filteredItems = sortedItems.filter(item =>
        item.title.toLowerCase().includes(filterText.toLowerCase())
    );

    const contents = inventoryItems === undefined || filteredItems.length === 0
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <table className="">
            <thead>
                <tr>
                    <th onClick={() => handleSort('title')}>Title {sortedColumn === 'title' && renderSortArrow()}</th>
                    <th onClick={() => handleSort('artist')}>Artist {sortedColumn === 'artist' && renderSortArrow()}</th>
                    <th onClick={() => handleSort('year')}>Year {sortedColumn === 'year' && renderSortArrow()}</th>
                    <th onClick={() => handleSort('genre')}>Genre {sortedColumn === 'genre' && renderSortArrow()}</th>
                    <th onClick={() => handleSort('price')}>Price {sortedColumn === 'price' && renderSortArrow()}</th>
                    <th onClick={() => handleSort('stockCount')}>Stock Count {sortedColumn === 'stockCount' && renderSortArrow()}</th>
                    <th>Quantity</th>
                </tr>
            </thead>
            <tbody>
                {filteredItems.map(inventoryItem =>
                    <tr key={inventoryItem.id}>
                        <td>
                            <Link to={`/${inventoryItem.id}`}>{inventoryItem.title}</Link>
                        </td>
                        <td>{inventoryItem.artist}</td>
                        <td>{inventoryItem.year}</td>
                        <td>{inventoryItem.genre}</td>
                        <td>{inventoryItem.price}</td>
                        <td>{inventoryItem.stockCount}</td>
                        <td>
                            <input
                                type="number"
                                min="0"
                                value={inventoryItem.quantity}
                                onChange={(e) => handleQuantityChange(inventoryItem.id, parseInt(e.target.value))}
                            />
                            <button onClick={() => incrementQuantity(inventoryItem.id)}>+</button>
                            <button onClick={() => decrementQuantity(inventoryItem.id)}>-</button>
                        </td>
                    </tr>
                )}
            </tbody>
        </table>;

    const totalQuantity = inventoryItems.reduce((acc, item) => acc + item.quantity, 0);
    const totalPrice = inventoryItems.reduce((acc, item) => acc + item.price * item.quantity, 0);

    return (
        <div>
            <h2>Inventory</h2>
            <input
                type="text"
                placeholder="Filter by album name"
                value={filterText}
                onChange={(e) => setFilterText(e.target.value)}
            />
            {contents}
            <div>
                <p>Total Quantity: {totalQuantity}</p>
                <p>Total Price: ${totalPrice ? totalPrice.toFixed(2) : 0}</p>
                <button onClick={purchaseItems} disabled={totalQuantity === 0}>Purchase</button>
                <p>{purchaseStatus}</p>
            </div>
        </div>
    );

    async function populateInventoryItems() {
        const response = await fetch('inventory');
        const data = await response.json();
        setInventoryItems(data.map((item: InventoryItem) => ({ ...item, quantity: 0 })));
    }

    function handleQuantityChange(id: string, quantity: number) {
        setInventoryItems(prevItems =>
            prevItems.map(item =>
                item.id === id ? { ...item, quantity } : item
            )
        );
    }

    function incrementQuantity(id: string) {
        setInventoryItems(prevItems =>
            prevItems.map(item =>
                item.id === id ? { ...item, quantity: item.quantity + 1 } : item
            )
        );
    }

    function decrementQuantity(id: string) {
        setInventoryItems(prevItems =>
            prevItems.map(item =>
                item.id === id && item.quantity > 0 ? { ...item, quantity: item.quantity - 1 } : item
            )
        );
    }

    async function purchaseItems() {
        const itemsToPurchase: { [key: string]: number } = {};
        inventoryItems.forEach(item => {
            if (item.quantity > 0) {
                itemsToPurchase[item.id] = item.quantity;
            }
        });

        const payload = {
            inventoryItemsToPurchase: itemsToPurchase
        };

        const requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        };

        try {
            const response = await fetch('inventory', requestOptions);
            if (response.ok) {
                setPurchaseStatus('Items purchased successfully!');
                // refresh inventory after successful purchase, could do for other events too?
                populateInventoryItems();
            } else {
                setPurchaseStatus('Failed to purchase items: ' + response.statusText);
            }
        } catch (error) {
            setPurchaseStatus('Failed to purchase items: ' + error.message);
        }
    }

    function handleSort(column: string) {
        if (column === sortedColumn) {
            setSortDirection(sortDirection === 'asc' ? 'desc' : 'asc');
        } else {
            setSortedColumn(column);
            setSortDirection('asc');
        }
    }

    function renderSortArrow() {
        return sortDirection === 'asc' ? <span>&uarr;</span> : <span>&darr;</span>;
    }
}

export default InventoryList;
