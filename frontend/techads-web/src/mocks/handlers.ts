import { http, HttpResponse } from 'msw';

// Mock data
const mockListings = [
  {
    id: '1',
    title: 'Senior React Developer',
    shortDescription: 'Build scalable web applications with React and TypeScript',
    requirements: ['5+ years React', 'TypeScript', 'Node.js'],
    tags: ['React', 'TypeScript', 'Remote'],
    ownerId: 'user-1',
    createdAt: new Date().toISOString(),
  },
  {
    id: '2',
    title: 'Backend Engineer',
    shortDescription: 'Design and implement RESTful APIs',
    requirements: ['Python', 'FastAPI', 'PostgreSQL'],
    tags: ['Python', 'API', 'Full-time'],
    ownerId: 'user-2',
    createdAt: new Date().toISOString(),
  },
];

let nextId = 3;

export const handlers = [
  // Auth endpoints
  http.post('/api/auth/login', async ({ request }) => {
    const body = await request.json() as { email: string; password: string };
    
    if (body.email === 'user@example.com' && body.password === 'password') {
      return HttpResponse.json({
        token: 'mock-jwt-token-' + Date.now(),
        user: {
          id: 'user-1',
          email: 'user@example.com',
          name: 'Test User',
        },
      });
    }
    
    return HttpResponse.json(
      { message: 'Invalid credentials' },
      { status: 401 }
    );
  }),

  // Listings endpoints
  http.get('/api/listings', () => {
    return HttpResponse.json({ listings: mockListings });
  }),

  http.get('/api/listings/:id', ({ params }) => {
    const listing = mockListings.find(l => l.id === params.id);
    if (!listing) {
      return HttpResponse.json(
        { message: 'Listing not found' },
        { status: 404 }
      );
    }
    return HttpResponse.json({ listing });
  }),

  http.post('/api/listings', async ({ request }) => {
    const authHeader = request.headers.get('Authorization');
    if (!authHeader || !authHeader.startsWith('Bearer ')) {
      return HttpResponse.json(
        { message: 'Unauthorized' },
        { status: 401 }
      );
    }

    const body = await request.json() as {
      title: string;
      shortDescription: string;
      requirements: string[];
      tags: string[];
    };

    const newListing = {
      id: String(nextId++),
      ...body,
      ownerId: 'user-1',
      createdAt: new Date().toISOString(),
    };

    mockListings.push(newListing);
    return HttpResponse.json({ listing: newListing }, { status: 201 });
  }),

  http.put('/api/listings/:id', async ({ request, params }) => {
    const authHeader = request.headers.get('Authorization');
    if (!authHeader || !authHeader.startsWith('Bearer ')) {
      return HttpResponse.json(
        { message: 'Unauthorized' },
        { status: 401 }
      );
    }

    const listing = mockListings.find(l => l.id === params.id);
    if (!listing) {
      return HttpResponse.json(
        { message: 'Listing not found' },
        { status: 404 }
      );
    }

    if (listing.ownerId !== 'user-1') {
      return HttpResponse.json(
        { message: 'Forbidden' },
        { status: 403 }
      );
    }

    const body = await request.json() as Partial<typeof listing>;
    Object.assign(listing, body);
    return HttpResponse.json({ listing });
  }),

  http.delete('/api/listings/:id', ({ request, params }) => {
    const authHeader = request.headers.get('Authorization');
    if (!authHeader || !authHeader.startsWith('Bearer ')) {
      return HttpResponse.json(
        { message: 'Unauthorized' },
        { status: 401 }
      );
    }

    const index = mockListings.findIndex(l => l.id === params.id);
    if (index === -1) {
      return HttpResponse.json(
        { message: 'Listing not found' },
        { status: 404 }
      );
    }

    const listing = mockListings[index];
    if (listing.ownerId !== 'user-1') {
      return HttpResponse.json(
        { message: 'Forbidden' },
        { status: 403 }
      );
    }

    mockListings.splice(index, 1);
    return HttpResponse.json({ message: 'Listing deleted' });
  }),

  // Application endpoint
  http.post('/api/listings/:id/apply', ({ request, params }) => {
    const authHeader = request.headers.get('Authorization');
    if (!authHeader || !authHeader.startsWith('Bearer ')) {
      return HttpResponse.json(
        { message: 'Unauthorized' },
        { status: 401 }
      );
    }

    const listing = mockListings.find(l => l.id === params.id);
    if (!listing) {
      return HttpResponse.json(
        { message: 'Listing not found' },
        { status: 404 }
      );
    }

    return HttpResponse.json({
      message: 'Application submitted successfully',
      applicationId: 'app-' + Date.now(),
    });
  }),
];
