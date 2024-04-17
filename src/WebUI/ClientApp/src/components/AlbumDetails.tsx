import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

interface AlbumDetailsProps {
    id: string;
    title: string;
    artist: string;
    year: number;
    genre: string;
    price: number;
    stockCount: number;
}

function AlbumDetails() {
    const { id } = useParams<{ id: string }>();
    const [albumDetails, setAlbumDetails] = useState<AlbumDetailsProps | null>(null);

    useEffect(() => {
        fetchAlbumDetails(id);
    }, [id]);

    async function fetchAlbumDetails(albumId: string) {
        try {
            const response = await fetch(`inventory/${albumId}`);
            if (response.ok) {
                const data = await response.json();
                setAlbumDetails(data);
            } else {
                console.error('Failed to fetch album details:', response.statusText);
            }
        } catch (error) {
            console.error('Failed to fetch album details:', error);
        }
    }

    if (!albumDetails) {
        return <p>Loading...</p>;
    }

    return (
        <div>
            <h2>Album Details</h2>
            <p>Title: {albumDetails.title}</p>
            <p>Artist: {albumDetails.artist}</p>
            <p>Year: {albumDetails.year}</p>
            <p>Genre: {albumDetails.genre}</p>
            <p>Price: ${albumDetails.price}</p>
            <p>Stock Count: {albumDetails.stockCount}</p>
        </div>
    );
}

export default AlbumDetails;
