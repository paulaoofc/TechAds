import { useEffect, useState } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import { listingsService } from '../services/listingsService';
import type { Listing } from '../services/listingsService';

export default function ListingDetail() {
  const { id } = useParams<{ id: string }>();
  const { user } = useAuth();
  const navigate = useNavigate();
  const [listing, setListing] = useState<Listing | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (id) {
      loadListing();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id]);

  const loadListing = async () => {
    if (!id) return;
    try {
      const data = await listingsService.getById(id);
      setListing(data);
    } catch {
      alert('Failed to load listing');
      navigate('/dashboard');
    } finally {
      setLoading(false);
    }
  };

  const handleApply = async () => {
    if (!id) return;
    try {
      await listingsService.apply(id);
      alert('Application submitted successfully!');
    } catch {
      alert('Failed to apply');
    }
  };

  const handleDelete = async () => {
    if (!id || !confirm('Are you sure you want to delete this listing?')) return;
    try {
      await listingsService.delete(id);
      navigate('/dashboard');
    } catch {
      alert('Failed to delete listing');
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100 flex items-center justify-center">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600"></div>
      </div>
    );
  }

  if (!listing) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100 flex items-center justify-center">
        <div className="text-center">
          <h2 className="text-2xl font-bold text-gray-900 mb-4">Listing not found</h2>
          <Link to="/dashboard" className="text-indigo-600 hover:text-indigo-700 font-semibold">
            Back to Dashboard
          </Link>
        </div>
      </div>
    );
  }

  const isOwner = user?.id === listing.ownerId;

  return (
    <div className="min-h-screen bg-linear-to-br from-blue-50 to-indigo-100">
      <header className="bg-white shadow-sm">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4">
          <Link to="/dashboard" className="text-indigo-600 hover:text-indigo-700 font-semibold">
            ← Back to Dashboard
          </Link>
        </div>
      </header>

      <main className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="bg-white rounded-2xl shadow-xl p-8">
          <h1 className="text-4xl font-bold text-gray-900 mb-4">{listing.title}</h1>
          
          <div className="flex flex-wrap gap-2 mb-6">
            {listing.tags.map((tag) => (
              <span
                key={tag}
                className="px-4 py-2 bg-indigo-100 text-indigo-700 rounded-full text-sm font-semibold"
              >
                {tag}
              </span>
            ))}
          </div>

          <div className="mb-8">
            <h2 className="text-xl font-semibold text-gray-900 mb-3">Description</h2>
            <p className="text-gray-700 leading-relaxed">{listing.shortDescription}</p>
          </div>

          <div className="mb-8">
            <h2 className="text-xl font-semibold text-gray-900 mb-3">Requirements</h2>
            <ul className="space-y-2">
              {listing.requirements.map((req, index) => (
                <li key={index} className="flex items-start gap-2">
                  <span className="text-indigo-600 font-bold">•</span>
                  <span className="text-gray-700">{req}</span>
                </li>
              ))}
            </ul>
          </div>

          <div className="text-sm text-gray-500 mb-6">
            Posted on {new Date(listing.createdAt).toLocaleDateString()}
          </div>

          <div className="flex gap-4">
            {isOwner ? (
              <>
                <button
                  onClick={handleDelete}
                  className="px-6 py-3 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors font-semibold"
                >
                  Delete Listing
                </button>
              </>
            ) : (
              <button
                onClick={handleApply}
                className="px-6 py-3 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition-colors font-semibold"
              >
                Apply Now
              </button>
            )}
          </div>
        </div>
      </main>
    </div>
  );
}
