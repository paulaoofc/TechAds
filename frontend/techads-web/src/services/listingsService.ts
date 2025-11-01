const API_BASE = '/api';

export interface Listing {
  id: string;
  title: string;
  shortDescription: string;
  requirements: string[];
  tags: string[];
  ownerId: string;
  createdAt: string;
}

export interface CreateListingData {
  title: string;
  shortDescription: string;
  requirements: string[];
  tags: string[];
}

const getAuthHeaders = () => {
  const token = localStorage.getItem('token');
  return {
    'Content-Type': 'application/json',
    ...(token && { Authorization: `Bearer ${token}` }),
  };
};

export const listingsService = {
  async getAll(): Promise<Listing[]> {
    const response = await fetch(`${API_BASE}/listings`);
    const data = await response.json();
    return data.listings;
  },

  async getById(id: string): Promise<Listing> {
    const response = await fetch(`${API_BASE}/listings/${id}`);
    if (!response.ok) {
      throw new Error('Listing not found');
    }
    const data = await response.json();
    return data.listing;
  },

  async create(listing: CreateListingData): Promise<Listing> {
    const response = await fetch(`${API_BASE}/listings`, {
      method: 'POST',
      headers: getAuthHeaders(),
      body: JSON.stringify(listing),
    });

    if (!response.ok) {
      throw new Error('Failed to create listing');
    }

    const data = await response.json();
    return data.listing;
  },

  async update(id: string, listing: Partial<CreateListingData>): Promise<Listing> {
    const response = await fetch(`${API_BASE}/listings/${id}`, {
      method: 'PUT',
      headers: getAuthHeaders(),
      body: JSON.stringify(listing),
    });

    if (!response.ok) {
      throw new Error('Failed to update listing');
    }

    const data = await response.json();
    return data.listing;
  },

  async delete(id: string): Promise<void> {
    const response = await fetch(`${API_BASE}/listings/${id}`, {
      method: 'DELETE',
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      throw new Error('Failed to delete listing');
    }
  },

  async apply(id: string): Promise<{ message: string; applicationId: string }> {
    const response = await fetch(`${API_BASE}/listings/${id}/apply`, {
      method: 'POST',
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      throw new Error('Failed to apply');
    }

    return response.json();
  },
};
