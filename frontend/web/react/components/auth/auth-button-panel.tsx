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
          className="bg-(--primary) hover:bg-(--primary-bright) active:bg-(--primary-dim) hover:scale-105"
          text={'Sign In'}
        />
        <Button
          className="rounded-md bg-(--primary) px-5 py-2 hover:bg-(--primary-bright) active:bg-(--primary-dim) hover:scale-105"
          onClick={() => setShowSignUpModal(true)}
          text={'Sign Up'}
        />
      </div>
    </>
  );
}
