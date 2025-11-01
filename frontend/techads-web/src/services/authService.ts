const API_BASE = import.meta.env.VITE_API_URL || "http://localhost:8000";

export interface LoginCredentials {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  user: {
    id: string;
    email: string;
    name: string;
  };
}

export const authService = {
  async login(credentials: LoginCredentials): Promise<AuthResponse> {
    const response = await fetch(`${API_BASE}/api/auth/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(credentials),
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || "Login failed");
    }

    const data = await response.json();
    // Backend retorna: { Token: "...", User: {...} }
    // Frontend espera: { token: "...", user: {...} }
    return {
      token: data.token || data.Token,
      user: data.user || data.User,
    };
  },
};
