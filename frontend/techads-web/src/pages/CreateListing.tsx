import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { useForm } from "../hooks/useForm";
import { listingsService } from "../services/listingsService";
import type { CreateListingData } from "../services/listingsService";

interface FormValues extends CreateListingData {
  tagInput: string;
  [key: string]: string | string[];
}

export default function CreateListing() {
  const navigate = useNavigate();
  const [tags, setTags] = useState<string[]>([]);
  const [requirements, setRequirements] = useState<string[]>([""]);

  const {
    values,
    errors,
    touched,
    isSubmitting,
    handleChange,
    handleBlur,
    handleSubmit,
  } = useForm<FormValues>({
    initialValues: {
      title: "",
      shortDescription: "",
      requirements: [],
      tags: [],
      tagInput: "",
    },
    validate: (values) => {
      const errors: Partial<Record<keyof FormValues, string>> = {};
      if (!values.title) errors.title = "Title is required";
      if (!values.shortDescription)
        errors.shortDescription = "Description is required";
      if (requirements.filter((r) => r.trim()).length === 0) {
        errors.requirements = "At least one requirement is required";
      }
      if (tags.length === 0) errors.tags = "At least one tag is required";
      return errors;
    },
    onSubmit: async (values) => {
      try {
        const listingData: CreateListingData = {
          title: values.title,
          shortDescription: values.shortDescription,
          requirements: requirements.filter((r) => r.trim()).join(", "),
          tags,
        };
        await listingsService.create(listingData);
        navigate("/dashboard");
      } catch (error) {
        alert(
          error instanceof Error ? error.message : "Failed to create listing"
        );
      }
    },
  });

  const addTag = () => {
    if (values.tagInput.trim() && !tags.includes(values.tagInput.trim())) {
      setTags([...tags, values.tagInput.trim()]);
      handleChange("tagInput")("");
    }
  };

  const removeTag = (tag: string) => {
    setTags(tags.filter((t) => t !== tag));
  };

  const updateRequirement = (index: number, value: string) => {
    const newRequirements = [...requirements];
    newRequirements[index] = value;
    setRequirements(newRequirements);
  };

  const addRequirement = () => {
    setRequirements([...requirements, ""]);
  };

  const removeRequirement = (index: number) => {
    setRequirements(requirements.filter((_, i) => i !== index));
  };

  return (
    <div className="min-h-screen bg-linear-to-br from-blue-50 to-indigo-100">
      <header className="bg-white shadow-sm">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4">
          <Link
            to="/dashboard"
            className="text-indigo-600 hover:text-indigo-700 font-semibold"
          >
            ← Back to Dashboard
          </Link>
        </div>
      </header>

      <main className="max-w-3xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <h1 className="text-3xl font-bold text-gray-900 mb-8">
          Create New Listing
        </h1>

        <form
          onSubmit={handleSubmit}
          className="bg-white rounded-2xl shadow-xl p-8 space-y-6"
        >
          <div>
            <label
              htmlFor="title"
              className="block text-sm font-medium text-gray-700 mb-2"
            >
              Job Title
            </label>
            <input
              id="title"
              type="text"
              value={values.title}
              onChange={(e) => handleChange("title")(e.target.value)}
              onBlur={handleBlur("title")}
              className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none"
              placeholder="e.g., Senior React Developer"
            />
            {touched.title && errors.title && (
              <p className="mt-1 text-sm text-red-600">{errors.title}</p>
            )}
          </div>

          <div>
            <label
              htmlFor="shortDescription"
              className="block text-sm font-medium text-gray-700 mb-2"
            >
              Short Description
            </label>
            <textarea
              id="shortDescription"
              value={values.shortDescription}
              onChange={(e) => handleChange("shortDescription")(e.target.value)}
              onBlur={handleBlur("shortDescription")}
              rows={3}
              className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none"
              placeholder="Brief description of the role..."
            />
            {touched.shortDescription && errors.shortDescription && (
              <p className="mt-1 text-sm text-red-600">
                {errors.shortDescription}
              </p>
            )}
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Requirements
            </label>
            {requirements.map((req, index) => (
              <div key={index} className="flex gap-2 mb-2">
                <input
                  type="text"
                  value={req}
                  onChange={(e) => updateRequirement(index, e.target.value)}
                  className="flex-1 px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none"
                  placeholder="e.g., 5+ years React experience"
                />
                {requirements.length > 1 && (
                  <button
                    type="button"
                    onClick={() => removeRequirement(index)}
                    className="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700"
                  >
                    Remove
                  </button>
                )}
              </div>
            ))}
            <button
              type="button"
              onClick={addRequirement}
              className="mt-2 px-4 py-2 bg-gray-200 text-gray-700 rounded-lg hover:bg-gray-300"
            >
              Add Requirement
            </button>
            {errors.requirements && (
              <p className="mt-1 text-sm text-red-600">{errors.requirements}</p>
            )}
          </div>

          <div>
            <label
              htmlFor="tagInput"
              className="block text-sm font-medium text-gray-700 mb-2"
            >
              Tags
            </label>
            <div className="flex gap-2 mb-2">
              <input
                id="tagInput"
                type="text"
                value={values.tagInput}
                onChange={(e) => handleChange("tagInput")(e.target.value)}
                onKeyPress={(e) =>
                  e.key === "Enter" && (e.preventDefault(), addTag())
                }
                className="flex-1 px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none"
                placeholder="e.g., React, Remote"
              />
              <button
                type="button"
                onClick={addTag}
                className="px-4 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700"
              >
                Add
              </button>
            </div>
            <div className="flex flex-wrap gap-2">
              {tags.map((tag) => (
                <span
                  key={tag}
                  className="px-3 py-1 bg-indigo-100 text-indigo-700 rounded-full text-sm flex items-center gap-2"
                >
                  {tag}
                  <button
                    type="button"
                    onClick={() => removeTag(tag)}
                    className="hover:text-indigo-900"
                  >
                    ×
                  </button>
                </span>
              ))}
            </div>
            {errors.tags && (
              <p className="mt-1 text-sm text-red-600">{errors.tags}</p>
            )}
          </div>

          <button
            type="submit"
            disabled={isSubmitting}
            className="w-full py-3 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition-colors font-semibold disabled:opacity-50 disabled:cursor-not-allowed"
          >
            {isSubmitting ? "Creating..." : "Create Listing"}
          </button>
        </form>
      </main>
    </div>
  );
}
