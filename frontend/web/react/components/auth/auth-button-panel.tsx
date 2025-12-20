import React from 'react';

export default function AuthButtonPanel() {
  return (
    <div className="flex items-center gap-4 pr-8">
      <button className="rounded-lg bg-blue-500 px-4 py-1.5 text-white shadow-md transition-all hover:bg-blue-400">
        Sign In
      </button>
      <button className="rounded-md bg-blue-500 px-5 py-2 text-white shadow-md transition-all hover:bg-blue-400">
        Sign Up
      </button>
    </div>
  );
}
