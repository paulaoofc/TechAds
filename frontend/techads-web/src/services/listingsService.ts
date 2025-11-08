const API_BASE = import.meta.env.VITE_API_URL || "http://localhost:5025";

export interface Listing {
  id: string;
  title: string;
  shortDescription: string;
  requirements: string;
  tags: { value: string }[];
  ownerId: string;
  createdAt: string;
}

export interface CreateListingData {
  title: string;
  shortDescription: string;
  requirements: string;
  tags: string[];
}

const getAuthHeaders = () => {
  const token = localStorage.getItem("token");
  return {
    "Content-Type": "application/json",
    ...(token && { Authorization: `Bearer ${token}` }),
  };
};

export const listingsService = {
  async getAll(): Promise<Listing[]> {
    const response = await fetch(`${API_BASE}/api/listings`);
    const data = await response.json();
    return Array.isArray(data) ? data : data.listings || [];
  },

  async getById(id: string): Promise<Listing> {
    const response = await fetch(`${API_BASE}/api/listings/${id}`);
    if (!response.ok) {
      throw new Error("Listing not found");
    }
    const data = await response.json();
    return data.listing || data;
  },

  async create(listing: CreateListingData): Promise<Listing> {
    const response = await fetch(`${API_BASE}/api/listings`, {
      method: "POST",
      headers: getAuthHeaders(),
      body: JSON.stringify(listing),
    });

    if (!response.ok) {
      throw new Error("Failed to create listing");
    }

    const data = await response.json();
    return data.listing || data;
  },

  async update(
    id: string,
    listing: Partial<CreateListingData>
  ): Promise<Listing> {
    const response = await fetch(`${API_BASE}/api/listings/${id}`, {
      method: "PUT",
      headers: getAuthHeaders(),
      body: JSON.stringify(listing),
    });

    if (!response.ok) {
      throw new Error("Failed to update listing");
    }

    const data = await response.json();
    return data.listing || data;
  },

  async delete(id: string): Promise<void> {
    const response = await fetch(`${API_BASE}/api/listings/${id}`, {
      method: "DELETE",
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      throw new Error("Failed to delete listing");
    }
  },

  async apply(id: string, message: string): Promise<{ id: string }> {
    const response = await fetch(
      `${API_BASE}/api/listings/${id}/applications`,
      {
        method: "POST",
        headers: getAuthHeaders(),
        body: JSON.stringify({ message }),
      }
    );

    if (!response.ok) {
      throw new Error("Failed to apply");
    }

    return response.json();
  },
};
