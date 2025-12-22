'use client';

import React, { useState } from 'react';
import SignUpModal from '@/components/auth/sign-up-modal';

export default function AuthButtonPanel() {
  const [showSignUpModal, setShowSignUpModal] = useState(false);

  return (
    <>
      <div className="flex items-center gap-4 pr-8">
        <button className="rounded-lg bg-blue-500 px-4 py-1.5 text-white shadow-md transition-all hover:bg-blue-400 hover:scale-110 active:bg-blue-600 active:scale-95">
          Sign In
        </button>
        <button
          className="rounded-md bg-blue-500 px-5 py-2 text-white shadow-md transition-all hover:bg-blue-400 hover:scale-110 active:bg-blue-600 active:scale-95"
          onClick={() => setShowSignUpModal(true)}
        >
          Sign Up
        </button>
      </div>
      <SignUpModal show={showSignUpModal} onClose={() => setShowSignUpModal(false)} />
    </>
  );
}
