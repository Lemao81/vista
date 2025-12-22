'use client';

import React, { useContext } from 'react';
import { ModalContext } from '@/components/shared/modal-provider';
import Button from '@/components/ui/button';

export default function AuthButtonPanel() {
  const { setShowSignUpModal } = useContext(ModalContext);

  return (
    <>
      <div className="flex items-center gap-4 pr-8">
        <Button
          text={'Sign In'}
          className="bg-blue-500 hover:bg-blue-400 active:bg-blue-600 hover:scale-105"
        />
        <Button
          text={'Sign Up'}
          className="rounded-md bg-blue-500 px-5 py-2 hover:bg-blue-400 active:bg-blue-600 hover:scale-105"
          onClick={() => setShowSignUpModal(true)}
        />
      </div>
    </>
  );
}
