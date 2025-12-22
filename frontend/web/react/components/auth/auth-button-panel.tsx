'use client';

import React, { useContext } from 'react';
import { ModalContext } from '@/components/shared/modal-provider';

export default function AuthButtonPanel() {
  const { setShowSignUpModal } = useContext(ModalContext);

  return (
    <>
      <div className="flex items-center gap-4 pr-8">
        <button className="rounded-lg bg-blue-500 px-4 py-1.5 text-white shadow-md transition-all hover:bg-blue-400 hover:scale-105 active:bg-blue-600 active:scale-95 cursor-pointer">
          Sign In
        </button>
        <button
          className="rounded-md bg-blue-500 px-5 py-2 text-white shadow-md transition-all hover:bg-blue-400 hover:scale-105 active:bg-blue-600 active:scale-95 cursor-pointer"
          onClick={() => setShowSignUpModal(true)}
        >
          Sign Up
        </button>
      </div>
    </>
  );
}
