import { useEffect, useState } from 'react';

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
    const [inventoryItems, setInventoryItems] = useState<InventoryItem[]>();
    const [purchaseStatus, setPurchaseStatus] = useState<string>('');

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
                    <th>Stock Count</th>
                    <th>Quantity</th>
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

    const totalQuantity = inventoryItems?.reduce((acc, item) => acc + item.quantity, 0);
    const totalPrice = inventoryItems?.reduce((acc, item) => acc + item.price * item.quantity, 0);

    return (
        <div>
            <h2>Inventory</h2>
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
}

export default InventoryList;
