

interface RequirementsInputProps {
  requirements: string[];
  onUpdateRequirement: (index: number, value: string) => void;
  onAddRequirement: () => void;
  onRemoveRequirement: (index: number) => void;
}

export default function RequirementsInput({
  requirements,
  onUpdateRequirement,
  onAddRequirement,
  onRemoveRequirement,
}: RequirementsInputProps) {
  return (
    <div className="space-y-2">
      <label className="block text-sm font-medium text-gray-700">
        Requirements *
      </label>
      {requirements.map((req, index) => (
        <div key={index} className="flex gap-2">
          <input
            type="text"
            value={req}
            onChange={(e) => onUpdateRequirement(index, e.target.value)}
            placeholder={`Requirement ${index + 1}`}
            className="flex-1 px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
          />
          {requirements.length > 1 && (
            <button
              type="button"
              onClick={() => onRemoveRequirement(index)}
              className="px-3 py-2 text-red-600 hover:text-red-800 border border-red-300 rounded-lg hover:bg-red-50"
            >
              Remove
            </button>
          )}
        </div>
      ))}
      <button
        type="button"
        onClick={onAddRequirement}
        className="px-4 py-2 text-indigo-600 hover:text-indigo-800 border border-indigo-300 rounded-lg hover:bg-indigo-50"
      >
        + Add Requirement
      </button>
    </div>
  );
}